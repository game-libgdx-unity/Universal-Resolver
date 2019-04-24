using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] [Inject] private PlayerListUI listUI;

    [SerializeField] [Inject] private Button btnShowList;

    // Start is called before the first frame update
    void Start()
    {
        btnShowList.onClick.AddListener(() => listUI.gameObject.SetActive(true));
    }
}