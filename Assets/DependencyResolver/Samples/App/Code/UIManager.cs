using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityIoC;

public class UIManager : MonoBehaviour
{
    [SerializeField, Inject] private PlayerListUI playerListUI;

    [SerializeField, Inject("Button")] private Button btnShowList;
    [SerializeField, Inject("Button2")] private Button btnCreateItem;
    [SerializeField, Inject("Button3")] private Button btnDeleteAll;

    // Start is called before the first frame update
    void Start()
    {
        btnShowList.onClick.AddListener(() => playerListUI.gameObject.SetActive(!playerListUI.gameObject.activeSelf));
        btnCreateItem.onClick.AddListener(() => Context.Resolve<PlayerData>(Random.Range(100,1000).ToString()));
        btnDeleteAll.onClick.AddListener(() => Context.DeleteAll<PlayerData>());
    }
}