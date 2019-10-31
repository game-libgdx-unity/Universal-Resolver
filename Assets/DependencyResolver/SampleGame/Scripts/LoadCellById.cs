using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityIoC;

public class LoadCellById : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Context.OnViewResolved<ScriptableDataCellView>()
            .Subscribe(v => { v.transform.SetParent(transform.GetChild(0)); })
            .AddTo(this);

        Context.OnUpdated<FakeCellScriptableObject>()
            .Subscribe(data => { Debug.Log("updated on FakeCellScriptableObject with id " + data.id); })
            .AddTo(this);
        
//        var c0 = Context.Resolve<FakeCellScriptableObject>("Data/Cell1");
//        var c1 = Context.Resolve<FakeCellScriptableObject>("Data/Cell2");
//        var c2 = Context.Resolve<FakeCellScriptableObject>("Data/Cell3");
//        var c3 = Context.Resolve<FakeCellScriptableObject>("Data/Cell2");
        
        Context.OnViewResolved<FakeCellView>()
            .Subscribe(v => { v.transform.SetParent(transform.GetChild(0)); })
            .AddTo(this);
        var c0 = Context.Resolve<FakeCellData>(0);
        var c1 = Context.Resolve<FakeCellData>(1);
        var c2 = Context.Resolve<FakeCellData>(2);
        var c3 = Context.Resolve<FakeCellData>(1);
        
        
//        c3.backgroundColor = Color.black;
//        Context.Update(c3);

        var views = Context.GetView(c3);
        Debug.Log("View C3: "+views.Count);
        
//        Context.Delete(c3);

//
//
//         views = Context.GetView(c2);
//        Debug.Log("View C2: "+views.Count);

//        var json = JsonUtility.ToJson(new FakeCellData(2));
//        Context.ResolveFromJson<FakeCellData>(json);
    }
}