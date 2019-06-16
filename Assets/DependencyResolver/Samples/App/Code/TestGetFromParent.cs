using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGetFromParent : MonoBehaviour
{
    [Parents] private IDialogue uiManager;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(uiManager != null);
        uiManager.setVisible(true);
    }
}
