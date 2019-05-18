using System.Reflection;
using UnityEngine;
using UnityIoC;

public class MineSweeperSettingLoader : MonoBehaviour
{
    public BindingSetting gameSetting;

    // Start is called before the first frame update
    void Awake()
    {
        if (!Context.Initialized)
        {
            Context.Setting.AutoProcessBehavioursInScene = false;
            Context.GetDefaultInstance(this, false).LoadBindingSetting(gameSetting);
        }
        
        Context.DefaultInstance.ProcessInjectAttributeForMonoBehaviours();
    }
}