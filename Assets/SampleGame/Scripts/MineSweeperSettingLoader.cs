using UnityEngine;
using UnityIoC;

public class MineSweeperSettingLoader : MonoBehaviour
{
    public BindingSetting gameSetting;
    // Start is called before the first frame update
    void Awake()
    {
       var context = new AssemblyContext(this, false);
       context.LoadBindingSetting(gameSetting);
       context.ProcessInjectAttributeForMonoBehaviours();

       AssemblyContext.DefaultInstance = context;
    }
}
