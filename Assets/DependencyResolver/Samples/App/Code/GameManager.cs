using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField, Inject] private UIManager uimanager;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(uimanager);
    }
}
