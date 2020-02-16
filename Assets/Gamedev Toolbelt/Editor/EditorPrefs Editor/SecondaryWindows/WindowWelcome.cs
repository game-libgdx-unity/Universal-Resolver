using UnityEngine;
using UnityEditor;

namespace com.immortalhydra.gdtb.epeditor
{
    public class WindowWelcome : EditorWindow
    {
        #region FIELDS AND PROPERTIES

        // Constants.
        private const int Offset = Constants.OFFSET;

        // Fields.
        private GUISkin _skin;
        private GUIStyle _wordWrappedColoredLabel, _headerLabel;
        private bool _welcomeValue;
        private float _usableWidth;

        // Properties.
        public static WindowWelcome Instance { get; private set; }

        public static bool IsOpen
        {
            get { return Instance != null; }
        }

        #endregion

        #region MONOBEHAVIOUR METHODS

        public void OnEnable()
        {
#if UNITY_5_3_OR_NEWER || UNITY_5_1 || UNITY_5_2
            titleContent = new GUIContent("Hello!");
#else
            title = "Hello!";
        #endif

            Instance = this;

            LoadSkin();
            LoadStyle();

            _welcomeValue = Preferences.ShowWelcome;
        }


        private void Update()
        {
            Repaint();
        }


        private void OnDestroy()
        {
            Resources.UnloadUnusedAssets();
        }


        private void OnGUI()
        {
            _usableWidth = position.width - Offset * 2;
            GUI.skin = _skin;

            DrawWindowBackground();
            var label1Content = new GUIContent("Hello! Here's how you can use EditorPrefs.");
            var label1Height = _wordWrappedColoredLabel.CalcHeight(label1Content, _usableWidth);
            var label1Rect = new Rect(Offset * 2, Offset * 2, _usableWidth - Offset * 2, label1Height);
            EditorGUI.LabelField(label1Rect, label1Content, _wordWrappedColoredLabel);

            var header1Content =
                new GUIContent(
                    "1a. If there's some EditorPrefs you want to track, press 'Get' and insert the details.");
            var header1Height = _headerLabel.CalcHeight(header1Content, _usableWidth);
            var header1Rect = new Rect(Offset * 2, Offset * 2 + 30, _usableWidth - Offset * 2, header1Height);
            EditorGUI.LabelField(header1Rect, header1Content, _headerLabel);

            DrawProcessButtons();

            var header2Content =
                new GUIContent(
                    "1b. If you want to add a new EditorPref, press 'Add' and do the same.");
            var header2Height = _headerLabel.CalcHeight(header2Content, _usableWidth);
            var header2Rect = new Rect(Offset * 2, Offset * 2 + 80, _usableWidth - Offset * 2, header2Height);
            header2Rect.y += 25;
            EditorGUI.LabelField(header2Rect, header2Content, _headerLabel);

            DrawRefreshButtons();

            var header3Content = new GUIContent("2. That's it! You are now tracking that EditorPref.");
            var header3Rect = new Rect(Offset * 2, Offset * 2 + 200, _usableWidth - Offset * 2, 0);
            header3Rect.height = _headerLabel.CalcHeight(header3Content, _usableWidth);
            EditorGUI.LabelField(header3Rect, header3Content, _headerLabel);

            var label5Content =
                new GUIContent(
                    "There are many settings you can change, a new section has been added to the Preferences window.");
            var label5Rect = new Rect(Offset * 2, Offset * 2 + 240, _usableWidth - Offset * 2, 0);
            ;
            label5Rect.height = _wordWrappedColoredLabel.CalcHeight(label5Content, _usableWidth);
            EditorGUI.LabelField(label5Rect, label5Content, _wordWrappedColoredLabel);

            var reviewContent =
                new GUIContent(
                    "If you like the extension, please leave a review!\nYou can do so by clicking this sentence, a browser window will be opened.");
            var reviewRect = new Rect(Offset * 2, Offset * 2 + 280, _usableWidth - Offset * 2, 0);
            ;
            reviewRect.height = _headerLabel.CalcHeight(reviewContent, _usableWidth);
            EditorGUIUtility.AddCursorRect(reviewRect, MouseCursor.Link);
            EditorGUI.LabelField(reviewRect, reviewContent, _headerLabel);
            if (Event.current.type == EventType.MouseUp && reviewRect.Contains(Event.current.mousePosition))
            {
                Application.OpenURL("https://www.assetstore.unity3d.com/#!/content/60351");
            }
            DrawToggle();
        }

        #endregion

        #region METHODS

        public static void Init()
        {
            // Get existing open window or if none, make a new one.
            var window = (WindowWelcome) GetWindow(typeof(WindowWelcome));
            window.SetMinSize();
            window.LoadSkin();
            window.Show();
        }


        /// Load CodeTODOs custom skin.
        public void LoadSkin()
        {
            _skin = Resources.Load(Constants.FILE_GUISKIN, typeof(GUISkin)) as GUISkin;
        }


        /// Load label styles.
        public void LoadStyle()
        {
            _wordWrappedColoredLabel = _skin.GetStyle("GDTB_EPEditor_label");
            _wordWrappedColoredLabel.active.textColor = Preferences.Tertiary;
            _wordWrappedColoredLabel.normal.textColor = Preferences.Tertiary;
            _wordWrappedColoredLabel.wordWrap = true;

            _headerLabel = _skin.GetStyle("GDTB_EPEditor_header");
            _headerLabel.active.textColor = Preferences.Secondary;
            _headerLabel.normal.textColor = Preferences.Secondary;
        }


        /// Draw the background texture.
        private void DrawWindowBackground()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), Preferences.Primary);
        }


        private void DrawProcessButtons()
        {
            var processRect = new Rect(60, 80, 80, 20);
            var processContent = new GUIContent("Get", "Load existing EditorPref");

            Controls.Button(processRect, processContent);
        }


        private void DrawRefreshButtons()
        {
            var refreshRect = new Rect(60, 160, 80, 20);
            var refreshContent = new GUIContent("Add", "Add new EditorPref");

            Controls.Button(refreshRect, refreshContent);
        }


        private void DrawToggle()
        {
            var rect = new Rect(Offset * 2, position.height - 20 - Offset, position.width, 20);
            _welcomeValue = EditorGUI.ToggleLeft(rect, " Show this window every time EditorPrefs Editor is opened",
                _welcomeValue,
                _wordWrappedColoredLabel);
            if (_welcomeValue != Preferences.ShowWelcome)
            {
                Preferences.SetWelcome(_welcomeValue);
            }
        }


        private void SetMinSize()
        {
            var window = GetWindow(typeof(WindowWelcome)) as WindowWelcome;
            window.minSize = new Vector2(450f, 370f);
        }

        #endregion
    }
}