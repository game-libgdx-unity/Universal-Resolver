using UnityEngine;
using UnityEditor;

namespace com.immortalhydra.gdtb.epeditor
{
    public class OtherAssetsFromIH : MonoBehaviour
    {
        [MenuItem("Window/Gamedev Toolbelt/EditorPrefs Editor/Our other Assets")]
        private static void GoToAssetStorePage()
        {
            Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:15617");
        }
    }
}
