using UnityEngine;
using UnityEditor;

namespace com.immortalhydra.gdtb.epeditor
{
    public static class Controls
    {
        public static bool Button(Rect controlRect, GUIContent controlContent)
        {
            var shouldFire = false;
            var controlID = GUIUtility.GetControlID(FocusType.Passive);

            switch (Event.current.GetTypeForControl(controlID))
            {
                case EventType.Repaint:
                {
                    // Calc the rectangle for the content.
                    var contentRect = new Rect(
                        controlRect.x,
                        controlRect.y,
                        controlRect.width,
                        controlRect.height
                    );

                    // If mouse over button
                    if(controlRect.Contains(Event.current.mousePosition) && Event.current.button == 0)
                    {
                        DrawPressedButton(contentRect, controlContent);
                    }
                    else
                    {
                        DrawUnpressedButton(contentRect, controlContent);
                    }
                    break;
                }
                case EventType.MouseUp:
                {
                    if (controlRect.Contains(Event.current.mousePosition) && Event.current.button == 0)
                    {
                        GUI.changed = true;
                        Event.current.Use();
                        shouldFire = true;
                    }
                    break;
                }
            }
            return shouldFire;
        }




        private static void DrawUnpressedButton(Rect aRect, GUIContent aContent)
        {
            var style = new GUIStyle();

            // Draw "border".
            EditorGUI.DrawRect(aRect, Preferences.Secondary);

            // Draw the darker internal rect.
            var internalRect = new Rect(aRect.x + 1, aRect.y + 1, aRect.width - 2, aRect.height - 2);
            EditorGUI.DrawRect(internalRect, Preferences.Primary);

            // Text formatting.
            style.active.textColor = style.onActive.textColor = style.normal.textColor = style.onNormal.textColor = Preferences.Tertiary;
            style.imagePosition = ImagePosition.TextOnly;
            style.alignment = TextAnchor.MiddleCenter;

            // Label inside the button.
            var textRect = internalRect;
            textRect.y--;
            EditorGUI.LabelField(textRect, aContent.text, style);
        }

        private static void DrawPressedButton(Rect aRect, GUIContent aContent)
        {
            var style = new GUIStyle();

            EditorGUI.DrawRect(aRect, Preferences.Secondary);

            // Text formatting.
            style.active.textColor = style.onActive.textColor = style.normal.textColor = style.onNormal.textColor = Preferences.Primary;
            style.imagePosition = ImagePosition.TextOnly;
            style.alignment = TextAnchor.MiddleCenter;

            var textRect = aRect;
            textRect.y--;
            EditorGUI.LabelField(textRect, aContent.text, style);
        }
    }
}