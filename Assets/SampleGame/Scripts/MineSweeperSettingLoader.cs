using System.Reflection;
using UnityEngine;
using UnityIoC;

public class MineSweeperSettingLoader : MonoBehaviour
{
    public BindingSetting gameSetting;

    // Start is called before the first frame update
    void Awake()
    {
        if (!AssemblyContext.DefaultInstanceInitialized)
        {
            AssemblyContext.GetDefaultInstance(this, false, false).LoadBindingSetting(gameSetting);
        }
        
        AssemblyContext.DefaultInstance.ProcessInjectAttributeForMonoBehaviours();
    }
}