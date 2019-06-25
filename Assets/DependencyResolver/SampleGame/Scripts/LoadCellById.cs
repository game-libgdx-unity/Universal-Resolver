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
        Context.OnViewResolved<FakeCellView>()
            .Subscribe(v => { v.transform.SetParent(transform.GetChild(0)); })
            .AddTo(this);

        var c0 = Context.Resolve<FakeCellData>(0);
        var c1 = Context.Resolve<FakeCellData>(1);
        var c2 = Context.Resolve<FakeCellData>(2);

        var json = JsonUtility.ToJson(new FakeCellData(2));
        Context.ResolveFromJson<FakeCellData>(json);
    }
}