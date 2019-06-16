using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityIoC;

public class UIManager : MonoBehaviour, IDialogue
{
    [SerializeField, Inject] private PlayerListUI playerListUI;

    [SerializeField, Inject("Button")] private Button btnShowList;
    [SerializeField, Inject("Button2")] private Button btnCreateItem;
    [SerializeField, Inject("Button3")] private Button btnDeleteAll;

    [Children] private Button[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        btnShowList.onClick.AddListener(() => playerListUI.gameObject.SetActive(!playerListUI.gameObject.activeSelf));
        btnCreateItem.onClick.AddListener(() => Context.Resolve<PlayerData>(Random.Range(100,1000).ToString()));
        btnDeleteAll.onClick.AddListener(() => Context.DeleteAll<PlayerData>());
        
        Debug.Assert(buttons.Length == 3, "buttons.Length == 3");
    }

    public void setVisible(bool visible)
    {
        playerListUI.gameObject.SetActive(visible);
    }
}

public interface IDialogue
{
    void setVisible(bool visible);
}