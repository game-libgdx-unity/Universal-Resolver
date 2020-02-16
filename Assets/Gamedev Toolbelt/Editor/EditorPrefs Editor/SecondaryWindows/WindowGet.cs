using UnityEngine;
using UnityEditor;

namespace com.immortalhydra.gdtb.epeditor
{
    public class WindowGet : EditorWindow
    {
        #region FIELDS AND PROPERTIES

        // Constants.
        private const int ButtonWidth = 60;
        private const int ButtonHeight = 18;

        // Fields.
        private Rect _getRect;
        private Rect _keyLabelRect, _keyRect;
        private Rect _typeLabelRect, _typeRect;

        private string _prefKey = "";
        private int _prefTypeIndex = 0;
        private string[] _prefTypes = {"Bool", "Int", "Float", "String"};

        private GUISkin _customSkin;
        private GUIStyle _boldStyle, _selectedGridButtonStyle, _buttonStyle;

        // Properties.
        public static WindowGet Instance { get; private set; }
        public static bool IsOpen
        {
            get { return Instance != null; }
        }

        #endregion

        #region MONOBEHAVIOUR METHODS
        public void OnEnable()
        {
            Instance = this;
            _customSkin = Resources.Load(Constants.FILE_GUISKIN, typeof(GUISkin)) as GUISkin;
            LoadStyles();
        }

        private void OnDestroy()
        {
            Resources.UnloadUnusedAssets();
        }

        public void OnGUI()
        {
            DrawWindowBackground();
            DrawType();
            DrawKeyField();
            DrawGet();
        }

        private void Update()
        {
            // Unfortunately, IMGUI is not really responsive to events, e.g. changing the style of a button
            // (like when you press it) shows some pretty abysmal delays in the GUI, the button will light up
            // and down too late after the actual click. We force the UI to update more often instead.
            Repaint();
        }
        #endregion


        #region METHODS

        public static void Init()
        {
            WindowGet window = (WindowGet) GetWindow(typeof(WindowGet));
            window.minSize = new Vector2(275, 154);

#if UNITY_5_3_OR_NEWER || UNITY_5_1 || UNITY_5_2
            window.titleContent = new GUIContent("Get EditorPref");
#else
                window.title = "Get EditorPref";
            #endif

            window.CloseOtherWindows();
            window.ShowUtility();
        }

        /// Load styles and apply color preferences to them.
        public void LoadStyles()
        {
            _boldStyle = _customSkin.GetStyle("GDTB_EPEditor_key");
            _boldStyle.normal.textColor = Preferences.Secondary;
            _boldStyle.active.textColor = Preferences.Secondary;
            _selectedGridButtonStyle = _customSkin.GetStyle("GDTB_EPEditor_selectionGrid");
            _selectedGridButtonStyle.active.textColor = Preferences.Primary;
            _selectedGridButtonStyle.normal.textColor = Preferences.Primary;
            _buttonStyle = _customSkin.GetStyle("GDTB_EPEditor_buttonText");
            _buttonStyle.active.textColor = Preferences.Primary;
            _buttonStyle.normal.textColor = Preferences.Tertiary;
        }




        /// Draw the background rectangle.
        private void DrawWindowBackground()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), Preferences.Primary);
        }

        /// Draw key input field.
        private void DrawKeyField()
        {
            _keyLabelRect = new Rect(10, 10, position.width - 20, 16);
            EditorGUI.LabelField(_keyLabelRect, "Key:", _boldStyle);

            _keyRect = new Rect(10, 29, position.width - 20, 32);
            _prefKey = EditorGUI.TextField(_keyRect, _prefKey);
        }

        /// Draw type selector.
        private void DrawType()
        {
            _typeLabelRect = new Rect(10, 71, position.width - 20, 16);
            EditorGUI.LabelField(_typeLabelRect, "Type:", _boldStyle);

            _typeRect = new Rect(10, 90, position.width - 20, 20);
            _prefTypeIndex = GUI.SelectionGrid(_typeRect, _prefTypeIndex, _prefTypes, _prefTypes.Length,
                _selectedGridButtonStyle);
            DrawingUtils.DrawSelectionGrid(_typeRect, _prefTypes, _prefTypeIndex, 60, 5, _buttonStyle,
                _selectedGridButtonStyle); // Draw our selectionGrid above Unity's one.
        }

        /// Draw Get button based on preferences.
        private void DrawGet()
        {
            GUIContent getContent;
            SetupButton_Get(out _getRect, out getContent);

            if (Controls.Button(_getRect, getContent))
            {
                if (_prefKey == "")
                {
                    EditorUtility.DisplayDialog("No key to look for", "Please add a key.", "Ok");
                }
                else
                {
                    // Get confirmation through dialog (or not if the user doesn't want to).
                    var canExecute = false;
                    if (Preferences.ShowConfirmationDialogs)
                    {
                        if (EditorUtility.DisplayDialog("Get editor preference?",
                            "Are you sure you want to get this key from EditorPrefs?\nIf the key is not found, we'll tell you.\nIf the type is wrong, a default key will be added.",
                            "Add key", "Cancel"))
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
                        if (NewEditorPrefs.HasKey(_prefKey))
                        {
                            AddEditorPref(_prefTypeIndex, _prefKey);

                            if (WindowMain.IsOpen)
                            {
                                GetWindow(typeof(WindowMain)).Repaint();
                            }
                            GetWindow(typeof(WindowGet)).Close();
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("No key found",
                                "The key you inserted was not found in EditorPrefs.\nPlease check that the spelling is correct (it's case sensitive).",
                                "Ok");
                        }
                    }
                }
            }
        }

        private void SetupButton_Get(out Rect aRect, out GUIContent aContent)
        {
            aRect = new Rect((Screen.width / 2) - ButtonWidth / 2, 126, ButtonWidth, ButtonHeight);
            aContent = new GUIContent("Get key", "Add existing key");
        }

        /// Add EditorPref to list.
        private void AddEditorPref(int aType, string aKey)
        {
            switch (aType)
            {
                case 0:
                    PrefOps.GetPref(aKey, PrefType.BOOL);
                    break;
                case 1:
                    PrefOps.GetPref(aKey, PrefType.INT);
                    break;
                case 2:
                    PrefOps.GetPref(aKey, PrefType.FLOAT);
                    break;
                case 3:
                    PrefOps.GetPref(aKey, PrefType.STRING);
                    break;
            }
        }

        /// Close other sub-windows when this one is opened.
        private void CloseOtherWindows()
        {
            if (WindowAdd.IsOpen)
            {
                GetWindow(typeof(WindowAdd)).Close();
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