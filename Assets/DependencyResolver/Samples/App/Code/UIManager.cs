using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField, Inject] private PlayerListUI playerListUI;

    [SerializeField, Inject("Button")] private Button btnShowList;

    // Start is called before the first frame update
    void Start()
    {
        btnShowList.onClick.AddListener(() => playerListUI.gameObject.SetActive(!playerListUI.gameObject.activeSelf));
    }
}