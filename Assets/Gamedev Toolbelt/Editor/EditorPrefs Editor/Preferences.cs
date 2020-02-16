using UnityEngine;
using UnityEditor;

namespace com.immortalhydra.gdtb.epeditor
{
    public class Preferences
    {
        #region fields

        // Confirmation dialogs
        private const string PREFS_EPEDITOR_CONFIRMATION_DIALOGS = "GDTB_EPEditor_ConfirmationDialogs";
        private static bool _confirmationDialogs = true;
        private static bool _confirmationDialogs_default = true;

        public static bool ShowConfirmationDialogs
        {
            get { return _confirmationDialogs; }
        }

        // Welcome window.
        private const string PREFS_EPEDITOR_WELCOME = "GDTB_EPEditor_Welcome";
        private static bool _showWelcome = true;
        private const bool ShowWelcomeDefault = true;

        public static bool ShowWelcome
        {
            get { return _showWelcome; }
            set { _showWelcome = value; }
        }

        #region Colors

        // Primary color.
        private const string PREFS_EPEDITOR_COLOR_PRIMARY = "GDTB_EPEditor_Primary";
        private static Color _primary = new Color(56, 56, 56, 1);
        private static Color _primary_dark = new Color(56, 56, 56, 1);
        private static Color _primary_light = new Color(233, 233, 233, 1);
        private static Color _primary_default = new Color(56, 56, 56, 1);

        public static Color Primary
        {
            get { return _primary; }
        }

        // Secondary color.
        private const string PREFS_EPEDITOR_COLOR_SECONDARY = "GDTB_EPEditor_Secondary";
        private static Color _secondary = new Color(55, 222, 179, 1);
        private static Color _secondary_dark = new Color(55, 222, 179, 1);
        private static Color _secondary_light = new Color(0, 101, 89, 1);
        private static Color _secondary_default = new Color(55, 222, 179, 1);

        public static Color Secondary
        {
            get { return _secondary; }
        }

        // Tertiary color.
        private const string PREFS_EPEDITOR_COLOR_TERTIARY = "GDTB_EPEditor_Tertiary";
        private static Color _tertiary = new Color(255, 255, 255, 1);
        private static Color _tertiary_dark = new Color(255, 255, 255, 1);
        private static Color _tertiary_light = new Color(56, 56, 56, 1);
        private static Color _tertiary_default = new Color(255, 255, 255, 1);

        public static Color Tertiary
        {
            get { return _tertiary; }
        }

        // Quaretnary color.
        private const string PREFS_EPEDITOR_COLOR_QUATERNARY = "GDTB_EPEditor_Quaternary";
        private static Color _quaternary = new Color(70, 70, 70, 1);
        private static Color _quaternary_dark = new Color(70, 70, 70, 1);
        private static Color _quaternary_light = new Color(220, 220, 220, 1);
        private static Color _quaternary_default = new Color(70, 70, 70, 1);

        public static Color Quaternary
        {
            get { return _quaternary; }
        }

        #endregion

        // Custom shortcut (no public get, can't see a use for it).
        private const string PREFS_EPEDITOR_SHORTCUT = "GDTB_EPEditor_Shortcut";
        private static string _shortcut = "%|e";
        private static string _newShortcut;
        private static string _shortcut_default = "%|e";
        private static bool[] _modifierKeys = {false, false, false}; // Ctrl/Cmd, Alt, Shift.
        private static int _mainShortcutKeyIndex = 0;
        // Restrict options to what we're sure works.
        private static string[] _shortcutKeys =
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u",
            "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "LEFT", "RIGHT", "UP", "DOWN",
            "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "HOME", "END", "PGUP", "PGDN"
        };

        #endregion fields

        [PreferenceItem("EP Editor")]
        public static void PreferencesGUI()
        {
            GetAllPrefValues(); // First we load all current preferences.

            // Then we actually display them.
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);
            _confirmationDialogs = EditorGUILayout.Toggle("Show confirmation dialogs", _confirmationDialogs);
            ShowWelcome = EditorGUILayout.Toggle("Show Welcome window", ShowWelcome);
            GUILayout.Space(20);
            EditorGUILayout.LabelField("UI", EditorStyles.boldLabel);
            _primary = EditorGUILayout.ColorField("Background and button color", _primary);
            _secondary = EditorGUILayout.ColorField("Accent color", _secondary);
            _tertiary = EditorGUILayout.ColorField("Text color", _tertiary);
            _quaternary = EditorGUILayout.ColorField("Element background color", _quaternary);
            EditorGUILayout.Separator();
            DrawThemeButtons();
            GUILayout.Space(20);
            _newShortcut = DrawShortcutSelector();
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Reset preferences", EditorStyles.boldLabel);
            DrawResetButton();
            EditorGUILayout.EndVertical();

            if (GUI.changed)
            {
                SetPrefValues();
            }
        }

        /// Set the value of ShowWelcome.
        public static void SetWelcome(bool val)
        {
            EditorPrefs.SetBool(PREFS_EPEDITOR_WELCOME, val);
        }

        /// If preferences have keys already saved in EditorPrefs, get them. Otherwise, set them.
        public static void GetAllPrefValues()
        {
            _confirmationDialogs = GetPrefValue(PREFS_EPEDITOR_CONFIRMATION_DIALOGS, _confirmationDialogs_default);
            ShowWelcome = GetPrefValue(PREFS_EPEDITOR_WELCOME, ShowWelcomeDefault);
            GetColorPrefs();
            _shortcut = GetPrefValue(PREFS_EPEDITOR_SHORTCUT, _shortcut_default);
            _newShortcut = _shortcut;
            ParseShortcutValues();
        }


        /// Set the value of all preferences.
        private static void SetPrefValues()
        {
            EditorPrefs.SetBool(PREFS_EPEDITOR_CONFIRMATION_DIALOGS, _confirmationDialogs);
            SetColorPrefs();
            SetShortcutPref();
            SetWelcome(ShowWelcome);
        }

        /// Set the value of a Color preference.
        private static void SetColorPrefs()
        {
            EditorPrefs.SetString(PREFS_EPEDITOR_COLOR_PRIMARY, RGBA.ColorToString(_primary));
            EditorPrefs.SetString(PREFS_EPEDITOR_COLOR_SECONDARY, RGBA.ColorToString(_secondary));
            EditorPrefs.SetString(PREFS_EPEDITOR_COLOR_TERTIARY, RGBA.ColorToString(_tertiary));
            EditorPrefs.SetString(PREFS_EPEDITOR_COLOR_QUATERNARY, RGBA.ColorToString(_quaternary));
        }

        /// Set the value of the shortcut preference.
        private static void SetShortcutPref(bool forceUpdate = false)
        {
            if (forceUpdate)
            {
                EditorPrefs.SetString(PREFS_EPEDITOR_SHORTCUT, _shortcut);
                var formattedShortcut = _shortcut.Replace("|", "");
                IO.OverwriteShortcut(formattedShortcut);
                _newShortcut = _shortcut;
            }
            else if (_newShortcut != _shortcut)
            {
                _shortcut = _newShortcut;
                EditorPrefs.SetString(PREFS_EPEDITOR_SHORTCUT, _shortcut);
                var formattedShortcut = _shortcut.Replace("|", "");
                IO.OverwriteShortcut(formattedShortcut);
            }
        }

        /// Load color preferences.
        private static void GetColorPrefs()
        {
            _primary = GetPrefValue(PREFS_EPEDITOR_COLOR_PRIMARY, _primary_default); // PRIMARY color.
            _secondary = GetPrefValue(PREFS_EPEDITOR_COLOR_SECONDARY, _secondary_default); // SECONDARY color.
            _tertiary = GetPrefValue(PREFS_EPEDITOR_COLOR_TERTIARY, _tertiary_default); // TERTIARY color.
            _quaternary = GetPrefValue(PREFS_EPEDITOR_COLOR_QUATERNARY, _quaternary_default); // QUATERNARY color.
        }

        /// Get the value of a bool preference.
        private static bool GetPrefValue(string aKey, bool aDefault)
        {
            bool val;
            if (!EditorPrefs.HasKey(aKey))
            {
                EditorPrefs.SetBool(aKey, aDefault);
                val = aDefault;
            }
            else
            {
                val = EditorPrefs.GetBool(aKey, aDefault);
            }

            return val;
        }

        /// Get the value of a string preference.
        private static string GetPrefValue(string aKey, string aDefault)
        {
            string val;
            if (!EditorPrefs.HasKey(aKey))
            {
                EditorPrefs.SetString(aKey, aDefault);
                val = aDefault;
            }
            else
            {
                val = EditorPrefs.GetString(aKey, aDefault);
            }

            return val;
        }

        /// Get the value of a Color preference.
        private static Color GetPrefValue(string aKey, Color aDefault)
        {
            Color val;
            if (!EditorPrefs.HasKey(aKey))
            {
                EditorPrefs.SetString(aKey, RGBA.ColorToString(aDefault));
                val = aDefault;
            }
            else
            {
                val = RGBA.StringToColor(EditorPrefs.GetString(aKey, RGBA.ColorToString(aDefault)));
            }

            return val;
        }

        /// Draw the shortcut selector.
        private static string DrawShortcutSelector()
        {
            // Differentiate between Mac Editor (CMD) and Win editor (CTRL).
            var platformKey = Application.platform == RuntimePlatform.OSXEditor ? "CMD" : "CTRL";
            var shortcut = "";
            ParseShortcutValues();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Shortcut ");
            GUILayout.Space(20);
            _modifierKeys[0] = GUILayout.Toggle(_modifierKeys[0], platformKey, EditorStyles.miniButton,
                GUILayout.Width(50));
            _modifierKeys[1] = GUILayout.Toggle(_modifierKeys[1], "ALT", EditorStyles.miniButton, GUILayout.Width(40));
            _modifierKeys[2] = GUILayout.Toggle(_modifierKeys[2], "SHIFT", EditorStyles.miniButton, GUILayout.Width(50));
            _mainShortcutKeyIndex = EditorGUILayout.Popup(_mainShortcutKeyIndex, _shortcutKeys, GUILayout.Width(55));
            GUILayout.EndHorizontal();

            // Generate shortcut string.
            if (_modifierKeys[0])
            {
                shortcut += "%|";
            }
            if (_modifierKeys[1])
            {
                shortcut += "&|";
            }
            if (_modifierKeys[2])
            {
                shortcut += "#|";
            }
            shortcut += _shortcutKeys[_mainShortcutKeyIndex];

            return shortcut;
        }

        /// Get usable values from the shortcut string pref.
        private static void ParseShortcutValues()
        {
            var foundCmd = false;
            var foundAlt = false;
            var foundShift = false;

            var keys = _shortcut.Split('|');
            for (var i = 0; i < keys.Length; i++)
            {
                switch (keys[i])
                {
                    case "%":
                        foundCmd = true;
                        break;
                    case "&":
                        foundAlt = true;
                        break;
                    case "#":
                        foundShift = true;
                        break;
                    default:
                        _mainShortcutKeyIndex = System.Array.IndexOf(_shortcutKeys, keys[i]);
                        break;
                }
            }
            _modifierKeys[0] = foundCmd; // Ctrl/Cmd.
            _modifierKeys[1] = foundAlt; // Alt.
            _modifierKeys[2] = foundShift; // Shift.
        }

        /// Draw Apply colors - Load dark theme - load light theme.
        private static void DrawThemeButtons()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Apply new colors"))
            {
                ReloadSkins();
                RepaintOpenWindows();
            }
            if (GUILayout.Button("Load dark theme"))
            {
                // Get confirmation through dialog (or not if the user doesn't want to).
                var canExecute = false;
                if (ShowConfirmationDialogs)
                {
                    if (EditorUtility.DisplayDialog("Change to dark theme?",
                        "Are you sure you want to change the color scheme to the dark (default) theme?",
                        "Change color scheme", "Cancel"))
                    {
                        canExecute = true;
                    }
                }
                else
                {
                    canExecute = true;
                }

                // Do it if we have permission.
                if (canExecute)
                {
                    _primary = new Color(_primary_dark.r / 255.0f, _primary_dark.g / 255.0f, _primary_dark.b / 255.0f,
                        1.0f);
                    _secondary = new Color(_secondary_dark.r / 255.0f, _secondary_dark.g / 255.0f,
                        _secondary_dark.b / 255.0f, 1.0f);
                    _tertiary = new Color(_tertiary_dark.r / 255.0f, _tertiary_dark.g / 255.0f,
                        _tertiary_dark.b / 255.0f, 1.0f);
                    _quaternary = new Color(_quaternary_dark.r / 255.0f, _quaternary_dark.g / 255.0f,
                        _quaternary_dark.b / 255.0f, 1.0f);
                    SetColorPrefs();
                    GetColorPrefs();

                    ReloadSkins();

                    RepaintOpenWindows();
                }
            }
            if (GUILayout.Button("Load light theme"))
            {
                // Get confirmation through dialog (or not if the user doesn't want to).
                var canExecute = false;
                if (ShowConfirmationDialogs)
                {
                    if (EditorUtility.DisplayDialog("Change to light theme?",
                        "Are you sure you want to change the color scheme to the light theme?", "Change color scheme",
                        "Cancel"))
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
                    _primary = new Color(_primary_light.r / 255.0f, _primary_light.g / 255.0f, _primary_light.b / 255.0f,
                        1.0f);
                    _secondary = new Color(_secondary_light.r / 255.0f, _secondary_light.g / 255.0f,
                        _secondary_light.b / 255.0f, 1.0f);
                    _tertiary = new Color(_tertiary_light.r / 255.0f, _tertiary_light.g / 255.0f,
                        _tertiary_light.b / 255.0f, 1.0f);
                    _quaternary = new Color(_quaternary_light.r / 255.0f, _quaternary_light.g / 255.0f,
                        _quaternary_light.b / 255.0f, 1.0f);
                    SetColorPrefs();
                    GetColorPrefs();

                    ReloadSkins();

                    RepaintOpenWindows();
                }
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
        }

        /// Draw reset button.
        private static void DrawResetButton()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Reset preferences", GUILayout.Width(120)))
            {
                // Get confirmation through dialog (or not if the user doesn't want to).
                var canExecute = false;
                if (ShowConfirmationDialogs)
                {
                    if (EditorUtility.DisplayDialog("Reset preferences?",
                        "Are you sure you want to reset preferences to their default values?", "Reset", "Cancel"))
                    {
                        canExecute = true;
                    }
                }
                else
                {
                    canExecute = true;
                }

                // If we have permission, do it.
                if (canExecute)
                {
                    ResetPrefsToDefault();
                }
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
        }

        #region Resets

        /// Reset all preferences to default.
        private static void ResetPrefsToDefault()
        {
            ResetConfirmationDialogs();
            ResetColors();
            ResetShortcut();
            ReloadSkins();
            SetPrefValues();
            ShowWelcome = ShowWelcomeDefault;
        }

        private static void ResetConfirmationDialogs()
        {
            _confirmationDialogs = _confirmationDialogs_default;
        }

        private static void ResetColors()
        {
            _primary = new Color(_primary_default.r / 255, _primary_default.g / 255, _primary_default.b / 255,
                _primary_default.a);
            _secondary = new Color(_secondary_default.r / 255, _secondary_default.g / 255, _secondary_default.b / 255,
                _secondary_default.a);
            _tertiary = new Color(_tertiary_default.r / 255, _tertiary_default.g / 255, _tertiary_default.b / 255,
                _tertiary_default.a);
            _quaternary = new Color(_quaternary_default.r / 255, _quaternary_default.g / 255,
                _quaternary_default.b / 255, _quaternary_default.a);
        }

        private static void ResetShortcut()
        {
            _shortcut = _shortcut_default;
            SetShortcutPref(true);
        }

        #endregion

        /// Repaint all open EPEditor windows.
        private static void RepaintOpenWindows()
        {
            if (WindowMain.IsOpen)
            {
                EditorWindow.GetWindow(typeof(WindowMain)).Repaint();
            }
            if (WindowAdd.IsOpen)
            {
                EditorWindow.GetWindow(typeof(WindowAdd)).Repaint();
            }
            if (WindowEdit.IsOpen)
            {
                EditorWindow.GetWindow(typeof(WindowEdit)).Repaint();
            }
            if (WindowGet.IsOpen)
            {
                EditorWindow.GetWindow(typeof(WindowGet)).Repaint();
            }
        }

        /// Reload skins of open windows.
        private static void ReloadSkins()
        {
            if (WindowMain.IsOpen)
            {
                var window = EditorWindow.GetWindow(typeof(WindowMain)) as WindowMain;
                window.LoadStyles();
            }
            if (WindowAdd.IsOpen)
            {
                var window = EditorWindow.GetWindow(typeof(WindowAdd)) as WindowMain;
                window.LoadStyles();
            }
            if (WindowEdit.IsOpen)
            {
                var window = EditorWindow.GetWindow(typeof(WindowEdit)) as WindowMain;
                window.LoadStyles();
            }
            if (WindowGet.IsOpen)
            {
                var window = EditorWindow.GetWindow(typeof(WindowGet)) as WindowMain;
                window.LoadStyles();
            }
            if (WindowWelcome.IsOpen)
            {
                var window = EditorWindow.GetWindow(typeof(WindowWelcome)) as WindowWelcome;
                window.LoadStyle();
            }
        }
    }
}