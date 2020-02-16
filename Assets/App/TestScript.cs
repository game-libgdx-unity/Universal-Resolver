using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    Dictionary<Type, List<object>> obj = new Dictionary<Type, List<object>>();
    // Start is called before the first frame update
    void Start()
    {
        var canvas = GetComponent(typeof(Canvas));
        var canvasS = GetComponent(typeof(CanvasScaler));
        
        obj[typeof(TestScript)] = new List<object>();
        obj[typeof(TestScript)].Add(canvas);
        obj[typeof(TestScript)].Add(canvasS);
    }

    // Update is called once per frame
    void Update()
    {
        print(obj[typeof(TestScript)].Count);
    }
}
