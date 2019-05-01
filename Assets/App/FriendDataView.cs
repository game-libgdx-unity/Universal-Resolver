using UnityEngine;
using UnityEngine.UI;
using UnityIoC;

public class FriendDataView : MonoBehaviour, IDataView<FriendData>
{
    [Inject] private Text txtName;
    [Inject] private Button btnDelete;
    
    public void OnNext(FriendData data)
    {
        txtName.text = data.Name;
        btnDelete.onClick.AddListener(()=>Context.Delete(data));
    }
}