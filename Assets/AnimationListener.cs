using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationListener : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnStartAnimEnded()
    {
        Debug.Log("OnStartAnimEnded");

        var view = GetComponentInChildren<UniWebView>();
        view.Show();
    }
}
