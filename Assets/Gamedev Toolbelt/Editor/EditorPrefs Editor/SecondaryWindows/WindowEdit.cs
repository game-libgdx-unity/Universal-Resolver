using UnityEngine;
using UnityEditor;

namespace com.immortalhydra.gdtb.epeditor
{
    public class WindowEdit : EditorWindow
    {
        #region FIELDS AND PROPERTIES

        // Constants.
        private const int ButtonWidth = 60;
        private const int ButtonHeight = 18;

        // Fields.
        private Rect _prefKeyLabelRect, _prefKeyRect;
        private Rect _prefTypeLabelRect, _prefTypeRect;
        private Rect _prefValueLabelRect;
        private Rect _editRect;

        private Pref _originalPref;
        private string _prefKey = "";
        private int _prefTypeIndex = 0;
        private string[] _prefType = {"Bool", "Int", "Float", "String"};

        private bool _boolValueOfPref = false;
        private int _boolValuesIndex = 0;
        private string[] _boolValues = {"False", "True"};
        private int _intValueOfPref = 0;
        private float _floatValueOfPref = 0.0f;
        private string _stringValueOfPref = "";

        private GUISkin _customSkin;
        private GUIStyle _boldStyle, _selectedGridButtonStyle, _buttonStyle;

        // Properties.
        public static WindowEdit Instance { get; private set; }

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
            DrawKey();
            DrawType();
            DrawValue();
            DrawEdit();
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

        public static void Init(Pref aPref)
        {
            WindowEdit window = (WindowEdit) GetWindow(typeof(WindowEdit));
            window.minSize = new Vector2(275, 209);

#if UNITY_5_3_OR_NEWER || UNITY_5_1 || UNITY_5_2
            window.titleContent = new GUIContent("Edit EditorPref");
#else
                window.title = "Edit EditorPref";
            #endif

            window.InitInputValues(aPref);
            window.CloseOtherWindows();

            window.ShowUtility();
        }

        /// Load custom styles and apply colors in preferences.
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


        /// Draw the background texture.
        private void DrawWindowBackground()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), Preferences.Primary);
        }

        /// Draw key input field.
        private void DrawKey()
        {
            _prefKeyLabelRect = new Rect(10, 10, Mathf.Clamp(position.width - 20, 80, position.width), 16);
            EditorGUI.LabelField(_prefKeyLabelRect, "Key:", _boldStyle);

            _prefKeyRect = new Rect(10, 29, Mathf.Clamp(position.width - 20, 80, position.width), 32);
            _prefKey = EditorGUI.TextField(_prefKeyRect, _prefKey);
        }

        /// Draw type selection.
        private void DrawType()
        {
            _prefTypeLabelRect = new Rect(10, 71, Mathf.Clamp(position.width - 20, 80, position.width), 16);
            EditorGUI.LabelField(_prefTypeLabelRect, "Type:", _boldStyle);

            _prefTypeRect = new Rect(10, 90, position.width - 20, 20);
            _prefTypeIndex = GUI.SelectionGrid(_prefTypeRect, _prefTypeIndex, _prefType, _prefType.Length,
                _selectedGridButtonStyle);
            DrawingUtils.DrawSelectionGrid(_prefTypeRect, _prefType, _prefTypeIndex, 60, 5, _buttonStyle,
                _selectedGridButtonStyle);
        }

        /// Draw value input field.
        private void DrawValue()
        {
            _prefValueLabelRect = new Rect(10, 118, Mathf.Clamp(position.width - 20, 80, position.width), 16);
            EditorGUI.LabelField(_prefValueLabelRect, "Value:", _boldStyle);

            switch (_prefTypeIndex)
            {
                case 0:
                    var boolRect = new Rect(10, 137, 130, 20);
                    _boolValuesIndex = GUI.SelectionGrid(boolRect, _boolValuesIndex, _boolValues, _boolValues.Length,
                        _selectedGridButtonStyle);
                    DrawingUtils.DrawSelectionGrid(boolRect, _boolValues, _boolValuesIndex, 60, 5, _buttonStyle,
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

        /// Draw Edit button based on preferences.
        private void DrawEdit()
        {
            GUIContent editContent;

            SetupButton_Edit(out _editRect, out editContent);

            if (Controls.Button(_editRect, editContent))
            {
                if (_prefKey == "") // We definitely want a key.
                {
                    EditorUtility.DisplayDialog("No key to use", "Please add a key.", "Ok");
                }
                else // What we do when editing is basically removing the old key and creating a new, updated one.
                {
                    // Get confirmation through dialog (or not if the user doesn't want to).
                    var canExecute = false;
                    if (Preferences.ShowConfirmationDialogs)
                    {
                        if (EditorUtility.DisplayDialog("Save edited Pref?",
                            "Are you sure you want to save the changes to this EditorPref?", "Save", "Cancel"))
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
                        NewEditorPrefs.DeleteKey(_originalPref.Key);

                        switch (_prefTypeIndex)
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
                        if (WindowMain.IsOpen)
                        {
                            GetWindow(typeof(WindowMain)).Repaint();
                        }
                        GetWindow(typeof(WindowEdit)).Close();
                    }
                }
            }
        }

        private void SetupButton_Edit(out Rect aRect, out GUIContent aContent)
        {
            aRect = new Rect(Mathf.Clamp(Screen.width / 2 - ButtonWidth / 2, 0, position.width), 179, ButtonWidth,
                ButtonHeight);
            aContent = new GUIContent("Save", "Save changes");
        }

        /// Set the values of the original pref for input fields.
        private void InitInputValues(Pref aPref)
        {
            _originalPref = aPref;
            _prefKey = _originalPref.Key;
            _prefTypeIndex = (int) _originalPref.Type;
            switch (_prefTypeIndex)
            {
                case 0:
                    _boolValueOfPref = aPref.Value == "True" ? true : false;
                    _boolValuesIndex = _boolValueOfPref == true ? 1 : 0;
                    break;
                case 1:
                    _intValueOfPref = int.Parse(aPref.Value);
                    break;
                case 2:
                    _floatValueOfPref = float.Parse(aPref.Value);
                    break;
                case 3:
                    _stringValueOfPref = aPref.Value;
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

        #endregion
    }
}