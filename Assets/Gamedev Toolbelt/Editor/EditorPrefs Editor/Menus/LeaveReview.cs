using UnityEngine;
using UnityEditor;

namespace com.immortalhydra.gdtb.epeditor
{
    public class LeaveReview : MonoBehaviour
    {
        [MenuItem("Window/Gamedev Toolbelt/EditorPrefs Editor/❤ Leave a review ❤")]
        private static void GoToAssetStorePage()
        {
            Application.OpenURL("https://www.assetstore.unity3d.com/#!/content/60351");
        }
    }
}
