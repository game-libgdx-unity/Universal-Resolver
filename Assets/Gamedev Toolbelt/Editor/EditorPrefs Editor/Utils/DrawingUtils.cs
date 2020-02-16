using UnityEditor;
using UnityEngine;

namespace com.immortalhydra.gdtb.epeditor
{
    public static class DrawingUtils
    {
        /// Draw the button, based on the type, not pressed.
        public static void DrawButton(Rect aRect, string aText,
            GUIStyle aStyle)
        {
            DrawSelectionGridButton(aRect, aText, aStyle);
        }

        /// Draw text button, not pressed.
        public static void DrawSelectionGridButton(Rect aRect, string aText, GUIStyle aStyle)
        {
            EditorGUI.DrawRect(aRect, Preferences.Secondary);
            var bgRect = new Rect(aRect.x + Constants.BUTTON_BORDER_THICKNESS,
                aRect.y + Constants.BUTTON_BORDER_THICKNESS, aRect.width - Constants.BUTTON_BORDER_THICKNESS * 2,
                aRect.height - Constants.BUTTON_BORDER_THICKNESS * 2);

            EditorGUI.DrawRect(bgRect, Preferences.Primary);
            GUI.Label(new Rect(bgRect.x, bgRect.y - 1, bgRect.width, bgRect.height), aText, aStyle);
        }


        // Draw button based on the type, pressed.
        public static void DrawButtonPressed(Rect aRect, string aText, GUIStyle aStyle)
        {
            DrawSelectionGridButtonPressed(aRect, aText, aStyle);
        }


        /// Draw text button, pressed.
        public static void DrawSelectionGridButtonPressed(Rect aRect, string aText, GUIStyle aStyle)
        {
            EditorGUI.DrawRect(aRect, Preferences.Secondary);
            var bgRect = new Rect(aRect.x + Constants.BUTTON_BORDER_THICKNESS,
                aRect.y + Constants.BUTTON_BORDER_THICKNESS, aRect.width - Constants.BUTTON_BORDER_THICKNESS * 2,
                aRect.height - Constants.BUTTON_BORDER_THICKNESS * 2);
            GUI.Label(new Rect(bgRect.x, bgRect.y - 1, bgRect.width, bgRect.height), aText, aStyle);
        }

        /// Draw custom selectionGrid.
        public static void DrawSelectionGrid(Rect aRect, string[] anElementArray, int aSelectedIndex,
            float anHorizontalSize, float aSpace, GUIStyle aNormalStyle, GUIStyle aPressedStyle)
        {
            var x = aRect.x;
            for (var i = 0; i < anElementArray.Length; i++)
            {
                var gridRect = aRect;
                x = i == 0 ? x : x + anHorizontalSize + aSpace;
                // Add parameters to horizontal pos., but not for the first rect.
                gridRect.x = x;
                gridRect.width = anHorizontalSize;

                if (i != aSelectedIndex)
                {
                    DrawSelectionGridButton(gridRect, anElementArray[i], aNormalStyle);
                }
                else
                {
                    DrawSelectionGridButtonPressed(gridRect, anElementArray[i], aPressedStyle);
                }
            }
        }
    }
}