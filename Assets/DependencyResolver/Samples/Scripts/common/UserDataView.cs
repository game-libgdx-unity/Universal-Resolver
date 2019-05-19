using UnityEngine;

namespace SceneTest
{
    public class UserDataView : MonoBehaviour
    {
        [Inject] UserData Data { get; set; }
    }
}