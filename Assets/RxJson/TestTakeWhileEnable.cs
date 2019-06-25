using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TestTakeWhileEnable : MonoBehaviour
{
    public Text text;
    private FloatReactiveProperty CurrentTime = new FloatReactiveProperty();
    // Start is called before the first frame update
    void Start()
    {
        CurrentTime.TakeWhileEnable(this).SubscribeToText(text);
//        CurrentTime.TakeUntilDisable(this).SubscribeToText(text);
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime.Value = Time.time;
    }
}
