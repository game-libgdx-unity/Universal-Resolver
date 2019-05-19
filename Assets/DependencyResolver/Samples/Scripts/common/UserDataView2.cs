using UnityEngine;
using UnityIoC;

namespace SceneTest
{
    public class UserDataView2 : MonoBehaviour, IDataBinding<UserData>
    {
        public void OnNext(UserData t)
        {
            //write code to update UI here
        }
    }
}