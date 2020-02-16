using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace com.immortalhydra.gdtb.epeditor
{
    public class WindowMain : EditorWindow
    {
        #region FIELDS AND PROPERTIES

        // Constants.
        private const int IconSize = Constants.ICON_SIZE;
        private const int ButtonWidth = 60;
        private const int ButtonHeight = 18;
        private const int Offset = Constants.OFFSET;

        // Fields.
        public static List<Pref> Prefs = new List<Pref>();
        private GUISkin _customSkin;
        private GUIStyle _prefTypeStyle, _prefKeyStyle, _prefValueStyle, _buttonStyle;
        private int _prefTypeWidth, _prefsWidth, _buttonsWidth;
        private int _prefTypeLabelWidth;
        private float _totalPrefHeight = 0;
        private Vector2 _scrollPosition = new Vector2(0.0f, 0.0f);
        private Rect _scrollViewRect, _scrollAreaRect, _prefTypeRect, _removeButtonRect, _prefRect, _buttonsRect;
        private bool _showingScrollbar = false;

        // Properties.
        public static WindowMain Instance { get; private set; }

        public static bool IsOpen
        {
            get { return Instance != null; }
        }

        #endregion

        #region MONOBEHAVIOUR METHODS

        public void OnEnable()
        {
#if UNITY_5_3_OR_NEWER || UNITY_5_1 || UNITY_5_2
            titleContent = new GUIContent("EditorPrefs Editor");
#else
            title = "EditorPrefs Editor";
        #endif

            Instance = this;
            /* Load current preferences (like colours, etc.).
             * We do this here so that most preferences are updated as soon as they're changed.
             */
            Preferences.GetAllPrefValues();

            _customSkin = Resources.Load(Constants.FILE_GUISKIN, typeof(GUISkin)) as GUISkin;
            
            LoadStyles();
        }

        private void OnDestroy()
        {
            Resources.UnloadUnusedAssets();
        }

        private void Update()
        {
            // Unfortunately, IMGUI is not really responsive to events, e.g. changing the style of a button
            // (like when you press it) shows some pretty abysmal delays in the GUI, the button will light up
            // and down too late after the actual click. We force the UI to update more often instead.
            Repaint();
        }

        private void OnGUI()
        {
            UpdateLayoutingSizes();
            GUI.skin = _customSkin; // Without this, almost everything will work aside from the scrollbar.

            // If the list is clean (for instance because we just recompiled) load Prefs again.
            if (Prefs.Count == 0)
            {
                Prefs.Clear();
                Prefs.AddRange(IO.LoadStoredPrefs());
            }

            DrawWindowBackground();

            // If the list is still clean after the above, then we really have no Prefs to show.
            if (Prefs.Count == 0)
            {
                DrawNoPrefsMessage();
            }

            DrawPrefs();
            DrawSeparator();
            DrawBottomButtons();
        }

        #endregion

        #region METHODS

        [MenuItem("Window/Gamedev Toolbelt/EditorPrefs Editor/Open EditorPrefs Editor %e", false, 1)]
        public static void Init()
        {
            // Get existing open window or if none, make a new one.
            var window = (WindowMain) GetWindow(typeof(WindowMain));
            window.SetMinSize(); // Window with icons and window with normal buttons need different minSizes.
            window.LoadStyles();
            window.UpdateLayoutingSizes(); // Calculate a rough size for each major element group (type, pref, buttons).
            PrefOps.RefreshPrefs(); // Load stored prefs (if any).

            window.Show();

            if (Preferences.ShowWelcome)
            {
                WindowWelcome.Init();
            }
        }

        /// Assign the proper values to styles based on colors in Preferences.
        public void LoadStyles()
        {
            _prefTypeStyle = _customSkin.GetStyle("GDTB_EPEditor_type");
            _prefTypeStyle.normal.textColor = Preferences.Tertiary;
            _prefTypeStyle.active.textColor = Preferences.Tertiary;
            _prefKeyStyle = _customSkin.GetStyle("GDTB_EPEditor_key");
            _prefKeyStyle.normal.textColor = Preferences.Secondary;
            _prefKeyStyle.active.textColor = Preferences.Secondary;
            _prefValueStyle = _customSkin.GetStyle("GDTB_EPEditor_value");
            _prefValueStyle.normal.textColor = Preferences.Tertiary;
            _prefValueStyle.active.textColor = Preferences.Tertiary;
            _buttonStyle = _customSkin.GetStyle("GDTB_EPEditor_buttonText");
            _buttonStyle.onActive.textColor = Preferences.Primary;
            _buttonStyle.onNormal.textColor = Preferences.Tertiary;

            _customSkin.settings.selectionColor = Preferences.Secondary;

            // Change scrollbar color.
            var scrollbar = Resources.Load(Constants.TEX_SCROLLBAR, typeof(Texture2D)) as Texture2D;

#if UNITY_5 || UNITY_5_3_OR_NEWER
            scrollbar.SetPixel(0, 0, Preferences.Secondary);
#else
            var pixels = scrollbar.GetPixels();
            // We do it like this because minimum texture size in older versions of Unity is 2x2.
            for(var i = 0; i < pixels.GetLength(0); i++)
            {
                scrollbar.SetPixel(i, 0, Preferences.Secondary);
                scrollbar.SetPixel(i, 1, Preferences.Secondary);
            }
        #endif

            scrollbar.Apply();
            _customSkin.verticalScrollbarThumb.normal.background = scrollbar;
        }

        /// Set the minSize of the window based on preferences.
        public void SetMinSize()
        {
            var window = GetWindow(typeof(WindowMain)) as WindowMain;
            window.minSize = new Vector2(322f, 150f);
        }


        /// Draw the background texture.
        private void DrawWindowBackground()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), Preferences.Primary);
        }

        /// Draw a message in center screen warning the user they have no prefs.
        private void DrawNoPrefsMessage()
        {
            var label =
                "There are currently no EditorPrefs loaded.\nYou can add a new EditorPref or\nget an existing one with the buttons below.\n\nIf you see this after the project recompiled,\ntry refreshing the window!\nYour prefs should come back just fine.";
            var labelContent = new GUIContent(label);

            Vector2 labelSize;
#if UNITY_UNITY_5_3_OR_NEWER
            labelSize = EditorStyles.centeredGreyMiniLabel.CalcSize(labelContent);
        #else
            labelSize = EditorStyles.wordWrappedMiniLabel.CalcSize(labelContent);
#endif

            var labelRect = new Rect(position.width / 2 - labelSize.x / 2,
                position.height / 2 - labelSize.y / 2 - Offset * 2.5f, labelSize.x, labelSize.y);

#if UNITY_5_3_OR_NEWER
            EditorGUI.LabelField(labelRect, labelContent, EditorStyles.centeredGreyMiniLabel);
#else
                EditorGUI.LabelField(labelRect, labelContent, EditorStyles.wordWrappedMiniLabel);
            #endif
        }

        /// Draw the list of EditorPrefs (with buttons etc.).
        private void DrawPrefs()
        {
            _scrollViewRect.height = _totalPrefHeight - Offset;

            // Diminish the width of scrollview and scroll area so that the scollbar is offset from the right edge of the window.
            _scrollAreaRect.width += IconSize - Offset;
            _scrollViewRect.width -= Offset;

            // Change size of the scroll area so that it fills the window when there's no scrollbar.
            if (_showingScrollbar == false)
            {
                _scrollViewRect.width += IconSize;
            }

            _scrollPosition = GUI.BeginScrollView(_scrollAreaRect, _scrollPosition, _scrollViewRect);

            _totalPrefHeight = Offset; // This includes all prefs, not just a single one.

            for (var i = 0; i < Prefs.Count; i++)
            {
                var key = new GUIContent(Prefs[i].Key);
                var val = new GUIContent(Prefs[i].Value);
                var prefKeyHeight = _prefKeyStyle.CalcHeight(key, _prefsWidth);
                var prefValueHeight = _prefValueStyle.CalcHeight(val, _prefsWidth);

                float prefBackgroundHeight = 0;
                prefBackgroundHeight = prefKeyHeight + prefValueHeight + Offset * 7;
                prefBackgroundHeight = prefBackgroundHeight < IconSize * 3 - 3
                    ? IconSize * 3 - 3
                    : prefBackgroundHeight;

                _prefRect = new Rect(Offset, _totalPrefHeight, _prefsWidth, prefBackgroundHeight);
                _prefTypeRect = new Rect(-2, _prefRect.y, _prefTypeWidth, prefBackgroundHeight);
                _removeButtonRect = new Rect(_prefTypeRect.x, 0, _prefTypeRect.width, _prefTypeRect.height);
                _removeButtonRect.y = _prefRect.y + _prefRect.height - Offset * 7;
                _buttonsRect = new Rect(_prefTypeWidth + _prefsWidth + IconSize, _prefRect.y, _buttonsWidth,
                    prefBackgroundHeight);

                var prefBackgroundRect = _prefRect;
                prefBackgroundRect.height = prefBackgroundHeight - Offset;

                if (_showingScrollbar == false) // If we're not showing the scrollbar, prefs need to be larger too.
                {
                    prefBackgroundRect.width = position.width - Offset * 2;
                }
                else
                {
                    prefBackgroundRect.width = _prefTypeWidth + _prefsWidth + _buttonsWidth;
                }

                _totalPrefHeight += prefBackgroundRect.height + Offset;

                // If the user removes a pref from the list in the middle of a draw call, the index in the for loop stays the same but Prefs.Count diminishes.
                // I couldn't find a way around it, so what we do is swallow the exception and wait for the next draw call.
                try
                {
                    DrawPrefBackground(prefBackgroundRect);
                    DrawType(_prefTypeRect, Prefs[i]);
                    DrawRemove(_removeButtonRect, Prefs[i]);
                    DrawKeyAndValue(_prefRect, Prefs[i], prefKeyHeight);
                    DrawEditAndDelete(_buttonsRect, Prefs[i]);
                }
                catch (System.Exception)
                {
                }
            }

            // Are we showing the scrollbar?
            if (_scrollAreaRect.height < _scrollViewRect.height)
            {
                _showingScrollbar = true;
            }
            else
            {
                _showingScrollbar = false;
            }
            GUI.EndScrollView();
        }

        /// Draw the rectangle that separates the prefs visually.
        private void DrawPrefBackground(Rect aRect)
        {
            EditorGUI.DrawRect(aRect, Preferences.Secondary);
            EditorGUI.DrawRect(new Rect(
                    aRect.x + Constants.BUTTON_BORDER_THICKNESS,
                    aRect.y + Constants.BUTTON_BORDER_THICKNESS,
                    aRect.width - Constants.BUTTON_BORDER_THICKNESS * 2,
                    aRect.height - Constants.BUTTON_BORDER_THICKNESS * 2),
                Preferences.Quaternary);
        }

        /// Draw the pref's type (string, int, etc).
        private void DrawType(Rect aRect, Pref aPref)
        {
            var typeRect = aRect;
            typeRect.width -= Offset;
            typeRect.height -= (IconSize / 2 + Offset);

            var newX = (int) typeRect.x + IconSize - Offset + 1;
            var newY = (int) typeRect.y + Offset;
            typeRect.position = new Vector2(newX, newY);

            var type = aPref.Type.ToString().ToLower();
            type = type.Substring(0, 1).ToUpper() + type.Substring(1);
            EditorGUI.LabelField(typeRect, type, _prefTypeStyle);
        }

        /// Draw the hide/eye button.
        private void DrawRemove(Rect aRect, Pref aPref)
        {
            Rect removeRect;
            GUIContent removeContent;
            SetupButton_Remove(aRect, out removeRect, out removeContent);

            // On click.
            if (Controls.Button(removeRect, removeContent))
            {
                PrefOps.RemovePref(aPref);
            }
        }


        /// Create rect and content for default Remove button.
        private void SetupButton_Remove(Rect aRect, out Rect aRemoveRect, out GUIContent aRemoveContent)
        {
            aRemoveRect = aRect;
            aRemoveRect.x += Offset * 2 + 1;
            aRemoveRect.y += Offset + 3;
            aRemoveRect.width = ButtonWidth / 2 + 3;
            aRemoveRect.height = ButtonHeight;
            aRemoveContent = new GUIContent("Hide",
                "Remove this EditorPref from\nthis list (without deleting it\nfrom EditorPrefs)");
        }

        /// Draw the key and value of the EditorPref.
        private void DrawKeyAndValue(Rect aRect, Pref aPref, float aHeight)
        {
            // Key.
            var keyRect = aRect;
            keyRect.x = _prefTypeWidth + IconSize / 2;
            keyRect.y += Offset;
            keyRect.height = aHeight;
            EditorGUI.LabelField(keyRect, aPref.Key, _prefKeyStyle);

            // Value.
            var valueRect = aRect;
            valueRect.x = _prefTypeWidth + IconSize / 2;
            valueRect.y = aRect.y + aHeight + Offset * 1.5f;

            EditorGUI.LabelField(valueRect, aPref.Value, _prefValueStyle);
        }

        /// Select which format to use based on the user preference.
        private void DrawEditAndDelete(Rect aRect, Pref aPref)
        {
            Rect editRect, deleteRect;
            GUIContent editContent, deleteContent;

            if (_showingScrollbar)
            {
                aRect.x -= Offset;
            }
            else
            {
                aRect.x = position.width - Offset * 2;
                aRect.x -= ButtonWidth;
            }

            SetupButton_Edit(aRect, out editRect, out editContent);
            SetupButton_Delete(aRect, out deleteRect, out deleteContent);


            if (Controls.Button(editRect, editContent))
            {
                WindowEdit.Init(aPref);
            }


            if (Controls.Button(deleteRect, deleteContent))
            {
                // Get confirmation through dialog (or not if the user doesn't want to).
                var canExecute = false;
                if (Preferences.ShowConfirmationDialogs)
                {
                    if (EditorUtility.DisplayDialog("Delete EditorPref",
                        "Are you sure you want to delete this EditorPref?", "Delete pref", "Cancel"))
                    {
                        canExecute = true;
                    }
                }
                else
                {
                    canExecute = true;
                }

                // Actually do the thing.
                if (canExecute)
                {
                    NewEditorPrefs.DeleteKey(aPref.Key);
                }
            }
        }

        private void SetupButton_Edit(Rect aRect, out Rect anEditRect, out GUIContent anEditContent)
        {
            anEditRect = aRect;
            anEditRect.y += Offset + 2;
            anEditRect.width = ButtonWidth;
            anEditRect.height = ButtonHeight;

            anEditContent = new GUIContent("Edit", "Edit this pref");
        }

        private void SetupButton_Delete(Rect aRect, out Rect aDeleteRect, out GUIContent aDeleteContent)
        {
            aDeleteRect = aRect;
            aDeleteRect.y += ButtonHeight + Offset + 8;
            aDeleteRect.width = ButtonWidth;
            aDeleteRect.height = ButtonHeight;

            aDeleteContent = new GUIContent("Delete", "Delete this EditorPref");
        }

        /// Draw a line separating scrollview and lower buttons.
        private void DrawSeparator()
        {
            var separator = new Rect(0, position.height - (Offset * 7), position.width, 1);
            EditorGUI.DrawRect(separator, Preferences.Secondary);
        }

        /// Draw Add, Get, Refresh, Settings and Nuke, based on preferences.
        private void DrawBottomButtons()
        {
            Rect addRect, getRect, refreshRect, settingsRect, nukeRect;
            GUIContent addContent, getContent, refreshContent, settingsContent, nukeContent;

            SetupButton_Add(out addRect, out addContent);
            SetupButton_Get(out getRect, out getContent);
            SetupButton_Refresh(out refreshRect, out refreshContent);
            SetupButton_Settings(out settingsRect, out settingsContent);
            SetupButton_Nuke(out nukeRect, out nukeContent);

            // Add new pref.
            if (Controls.Button(addRect, addContent))
            {
                WindowAdd.Init();
            }

            // Get already existing pref.
            if (Controls.Button(getRect, getContent))
            {
                WindowGet.Init();
            }

            // Refresh list of prefs.
            if (Controls.Button(refreshRect, refreshContent))
            {
                PrefOps.RefreshPrefs();
            }

            // Open settings.
            if (Controls.Button(settingsRect, settingsContent))
            {
                CloseOtherWindows();
                // Unfortunately EditorApplication.ExecuteMenuItem(...) doesn't work, so we have to rely on a bit of reflection.
                var assembly = System.Reflection.Assembly.GetAssembly(typeof(EditorWindow));
                var type = assembly.GetType("UnityEditor.PreferencesWindow");
                var method = type.GetMethod("ShowPreferencesWindow",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                method.Invoke(null, null);
            }

            // Nuke prefs.
            if (Controls.Button(nukeRect, nukeContent))
            {
                var canExecute = false;
                if (Preferences.ShowConfirmationDialogs)
                {
                    if (EditorUtility.DisplayDialog("Remove ALL EditorPrefs",
                        "Are you sure ABSOLUTELY sure you want to remove ALL EditorPrefs currently set?\nThis is IRREVERSIBLE, only do this if you know what you're doing.\nYOU WILL ALSO NEED TO RESTART UNITY, as you'll be deleting some EditorPrefs that are required for the engine to work.",
                        "Nuke EditorPrefs", "Cancel"))
                    {
                        canExecute = true;
                    }
                }
                else
                {
                    canExecute = true;
                }

                if (canExecute)
                {
                    NewEditorPrefs.DeleteAll();
                }
            }
        }

        private void SetupButton_Add(out Rect aRect, out GUIContent aContent)
        {
            aRect = new Rect(position.width / 2 - ButtonWidth * 2.5f - 8, position.height - ButtonHeight * 1.4f,
                ButtonWidth, ButtonHeight);
            aContent = new GUIContent("Add", "Add a new key");
        }

        private void SetupButton_Get(out Rect aRect, out GUIContent aContent)
        {
            aRect = new Rect(position.width / 2 - ButtonWidth * 1.5f - 4, position.height - ButtonHeight * 1.4f,
                ButtonWidth, ButtonHeight);
            aContent = new GUIContent("Get", "Add existing key");
        }

        private void SetupButton_Refresh(out Rect aRect, out GUIContent aContent)
        {
            aRect = new Rect(position.width / 2 - ButtonWidth / 2, position.height - ButtonHeight * 1.4f,
                ButtonWidth, ButtonHeight);
            aContent = new GUIContent("Refresh", "Refresh list");
        }

        private void SetupButton_Settings(out Rect aRect, out GUIContent aContent)
        {
            aRect = new Rect(position.width / 2 + ButtonWidth * 0.5f + 4, position.height - ButtonHeight * 1.4f,
                ButtonWidth, ButtonHeight);
            aContent = new GUIContent("Settings", "Open Settings");
        }

        private void SetupButton_Nuke(out Rect aRect, out GUIContent aContent)
        {
            aRect = new Rect(position.width / 2 + ButtonWidth * 1.5f + 8, position.height - (ButtonHeight * 1.4f),
                ButtonWidth, ButtonHeight);
            aContent = new GUIContent("Nuke all", "Delete ALL prefs from EditorPrefs");
        }

        /// Calculate the correct size of GUI elements based on preferences.
        private void UpdateLayoutingSizes()
        {
            _prefTypeLabelWidth = (int) _prefTypeStyle.CalcSize(new GUIContent("String")).x;
            var width = position.width - Offset * 2;
            _scrollAreaRect = new Rect(Offset, Offset, width - Offset * 2, position.height - IconSize - Offset * 4);
            _scrollViewRect = _scrollAreaRect;
            _prefTypeWidth = _prefTypeLabelWidth + (Offset * 2);

            if (_showingScrollbar)
                _buttonsWidth = ButtonWidth + Offset * 3;
            else
                _buttonsWidth = ButtonWidth + Offset * 1;
            _prefsWidth = (int) width - _prefTypeWidth - _buttonsWidth - Offset * 3;
        }

        /// Close open sub-windows (add, get, edit) when opening prefs.
        private void CloseOtherWindows()
        {
            if (WindowAdd.IsOpen)
            {
                GetWindow(typeof(WindowAdd)).Close();
            }
            if (WindowGet.IsOpen)
            {
                GetWindow(typeof(WindowGet)).Close();
            }
            if (WindowEdit.IsOpen)
            {
                GetWindow(typeof(WindowEdit)).Close();
            }
            if (WindowWelcome.IsOpen)
            {
                var window = GetWindow(typeof(WindowWelcome)) as WindowWelcome;
                window.LoadStyle();
            }
        }

        #endregion
    }
}