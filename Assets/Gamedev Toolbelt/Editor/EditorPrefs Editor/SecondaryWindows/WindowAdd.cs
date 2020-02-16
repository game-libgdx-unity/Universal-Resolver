using UnityEngine;
using UnityEditor;

namespace com.immortalhydra.gdtb.epeditor
{
    public class WindowAdd : EditorWindow
    {
        #region FIELDS AND PROPERTIES

        // Constants.
        private const int ButtonWidth = 60;
        private const int ButtonHeight = 18;

        // Fields.
        private Rect _prefKeyLabelRect, _prefKeyRect;
        private Rect _prefTypeLabelRect, _prefTypeRect;
        private Rect _prefValueLabelRect;
        private Rect _addRect;

        private string _prefKey = "";
        private int _indexOfPrefType = 0;
        private string[] _prefType = {"Bool", "Int", "Float", "String"};

        private bool _boolValueOfPref = false;
        private int _boolValuesIndex = 0;
        private string[] _boolValues = {"False", "True"};
        private int _intValueOfPref = 0;
        private float _floatValueOfPref = 0.0f;
        private string _stringValueOfPref = "";

        private GUISkin _customSkin;
        private GUIStyle _boldStyle, _selectedGridButtonStyle, _textButtonStyle;

        // Properties.
        public static WindowAdd Instance { get; private set; }

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
            GUI.skin = _customSkin;

            DrawWindowBackground();
            DrawKey();
            DrawType();
            DrawValue();
            DrawAdd();
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
            WindowAdd window = (WindowAdd) GetWindow(typeof(WindowAdd));
            window.minSize = new Vector2(275, 209);

#if UNITY_5_3_OR_NEWER || UNITY_5_1 || UNITY_5_2
            window.titleContent = new GUIContent("Add EditorPref");
#else
                window.title = "Add EditorPref";
            #endif

            window.CloseOtherWindows();
            window.ShowUtility();
        }

        /// Load custom styles and apply colors from preferences.
        public void LoadStyles()
        {
            _boldStyle = _customSkin.GetStyle("GDTB_EPEditor_key");
            _boldStyle.normal.textColor = Preferences.Secondary;
            _boldStyle.active.textColor = Preferences.Secondary;
            _selectedGridButtonStyle = _customSkin.GetStyle("GDTB_EPEditor_selectionGrid");
            _selectedGridButtonStyle.normal.textColor = Preferences.Primary;
            _selectedGridButtonStyle.active.textColor = Preferences.Primary;
            _textButtonStyle = _customSkin.GetStyle("GDTB_EPEditor_buttonText");
            _textButtonStyle.active.textColor = Preferences.Tertiary;
            _textButtonStyle.normal.textColor = Preferences.Tertiary;
        }




        /// Draw the background texture.
        private void DrawWindowBackground()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), Preferences.Primary);
        }

        /// Draw key input field.
        private void DrawKey()
        {
            _prefKeyLabelRect = new Rect(10, 10, position.width - 20, 16);
            EditorGUI.LabelField(_prefKeyLabelRect, "Key:", _boldStyle);

            _prefKeyRect = new Rect(10, 29, position.width - 20, 32);
            _prefKey = EditorGUI.TextField(_prefKeyRect, _prefKey);
        }

        /// Draw type popup.
        private void DrawType()
        {
            _prefTypeLabelRect = new Rect(10, 71, Mathf.Clamp(position.width - 20, 80, 500), 16);
            EditorGUI.LabelField(_prefTypeLabelRect, "Type:", _boldStyle);

            _prefTypeRect = new Rect(10, 90, position.width - 20, 20);
            _indexOfPrefType = GUI.SelectionGrid(_prefTypeRect, _indexOfPrefType, _prefType, _prefType.Length,
                _selectedGridButtonStyle);
            DrawingUtils.DrawSelectionGrid(_prefTypeRect, _prefType, _indexOfPrefType, 60, 5, _textButtonStyle,
                _selectedGridButtonStyle);
        }

        /// Draw value input field.
        private void DrawValue()
        {
            _prefValueLabelRect = new Rect(10, 118, Mathf.Clamp(position.width - 20, 80, 500), 16);
            EditorGUI.LabelField(_prefValueLabelRect, "Value:", _boldStyle);

            switch (_indexOfPrefType)
            {
                case 0:
                    var boolRect = new Rect(10, 137, 130, 20);
                    _boolValuesIndex = GUI.SelectionGrid(boolRect, _boolValuesIndex, _boolValues, _boolValues.Length,
                        _selectedGridButtonStyle);
                    DrawingUtils.DrawSelectionGrid(boolRect, _boolValues, _boolValuesIndex, 60, 5, _textButtonStyle,
                        _selectedGridButtonStyle);
                    _boolValueOfPref = _boolValuesIndex == 0 ? false : true;
                    break;
                case 1:
                    var intRect = new Rect(10, 137, position.width - 20, 16);
                    _intValueOfPref = EditorGUI.IntField(intRect, _intValueOfPref);
                    break;
                case 2:
                    var floatRect = new Rect(10, 137, position.width - 20, 16);
                    _floatValueOfPref = EditorGUI.FloatField(floatRect, _floatValueOfPref);
                    break;
                case 3:
                    var stringRect = new Rect(10, 137, position.width - 20, 32);
                    _stringValueOfPref = EditorGUI.TextField(stringRect, _stringValueOfPref);
                    break;
            }
        }

        /// Draw Add button based of preferences.
        private void DrawAdd()
        {
            Pref currentPref = null;
            GUIContent addContent;

            SetupButton_Add(out _addRect, out addContent);

            if (Controls.Button(_addRect, addContent))
            {
                AddButtonPressed(currentPref); // In another function to help readability of this one.
            }
        }

        private void SetupButton_Add(out Rect aRect, out GUIContent aContent)
        {
            aRect = new Rect(Mathf.Clamp((Screen.width / 2) - ButtonWidth / 2, 0, position.width), 179, ButtonWidth,
                ButtonHeight);
            aContent = new GUIContent("Add", "Add this new EditorPref");
        }

        /// Close other sub-windows when this one is opened.
        private void CloseOtherWindows()
        {
            if (WindowEdit.IsOpen)
            {
                GetWindow(typeof(WindowEdit)).Close();
            }
            if (WindowGet.IsOpen)
            {
                GetWindow(typeof(WindowGet)).Close();
            }
            if (WindowWelcome.IsOpen)
            {
                var window = GetWindow(typeof(WindowWelcome)) as WindowWelcome;
                window.LoadStyle();
            }
        }

        private void AddButtonPressed(Pref currentPref)
        {
            // A: we want a key.
            if (_prefKey == "")
            {
                EditorUtility.DisplayDialog("No key to add", "Please add a key.", "Ok");
            }
            // B: The key exists in EditorPrefs:
            else if (NewEditorPrefs.HasKey(_prefKey))
            {
                var inPrefs = false;

                // B-a: is the key in the Prefs list already?
                foreach (var pref in WindowMain.Prefs)
                {
                    if (pref.Key == _prefKey)
                    {
                        inPrefs = true;
                        currentPref = pref;
                        break;
                    }
                }

                // B-b: the key is not in the list (but it exists in EditorPrefs).
                if (inPrefs == false)
                {
                    switch (_indexOfPrefType) // Get the key's value based on the type given.
                    {
                        case 0:
                            _boolValueOfPref = NewEditorPrefs.GetBool(_prefKey); //_boolValuesIndex == 0 ? false : true;
                            currentPref = new Pref(PrefType.BOOL, _prefKey, _boolValueOfPref.ToString());
                            break;
                        case 1:
                            _intValueOfPref = NewEditorPrefs.GetInt(_prefKey);
                            currentPref = new Pref(PrefType.INT, _prefKey, _intValueOfPref.ToString());
                            break;
                        case 2:
                            _floatValueOfPref = NewEditorPrefs.GetFloat(_prefKey);
                            currentPref = new Pref(PrefType.FLOAT, _prefKey, _floatValueOfPref.ToString());
                            break;
                        case 3:
                            _stringValueOfPref = NewEditorPrefs.GetString(_prefKey);
                            currentPref = new Pref(PrefType.STRING, _prefKey, _stringValueOfPref);
                            break;
                    }
                }

                // B-c: does the user want to edit the already existing key before adding it?
                if (currentPref != null)
                {
                    if (EditorUtility.DisplayDialog("Pref already exists.",
                        "The key you're trying to use already exists.\nDo you want to edit it before adding it to the list?",
                        "Edit", "Add"))
                    {
                        WindowEdit.Init(currentPref);
                    }
                    else
                    {
                        PrefOps.AddPref(currentPref);
                    }
                    GetWindow(typeof(WindowAdd)).Close();
                }
            }
            // C: the pref doesn't exist in EditorPrefs.
            else
            {
                // Get confirmation through dialog (or not if the user doesn't want to).
                var canExecute = false;
                if (Preferences.ShowConfirmationDialogs)
                {
                    if (EditorUtility.DisplayDialog("Add editor preference?",
                        "Are you sure you want to add this key to EditorPrefs?", "Add key", "Cancel"))
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
                    switch (_indexOfPrefType)
                    {
                        case 0:
                            NewEditorPrefs.SetBool(_prefKey, _boolValueOfPref);
                            break;
                        case 1:
                            NewEditorPrefs.SetInt(_prefKey, _intValueOfPref);
                            break;
                        case 2:
                            NewEditorPrefs.SetFloat(_prefKey, _floatValueOfPref);
                            break;
                        case 3:
                            NewEditorPrefs.SetString(_prefKey, _stringValueOfPref);
                            break;
                    }
                    GetWindow(typeof(WindowAdd)).Close();
                }
            }
        }

        #endregion

    }
}