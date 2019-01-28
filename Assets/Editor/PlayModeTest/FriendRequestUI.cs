using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Client))]
public class FriendRequestUI : MonoBehaviour
{
    private Client client;

    private void Start()
    {
        client = GetComponent<Client>();
        
        //show current users to ui canvas
        var requestList = Instantiate(Resources.Load<GameObject>("FriendListUI"));
        var requestItemPrefab = Resources.Load<GameObject>("PlayerPrefab");
        
        foreach (Transform tf in requestList.transform.GetChild(0))
        {
            tf.gameObject.SetActive(false);
            GameObject.Destroy(tf.gameObject);
        }

        client.FriendRequestIncoming
            .ObserveOnMainThread()
            .Subscribe(id =>
            {
                
                var userId = id;
                var requestUI = Instantiate(requestItemPrefab, 
                    requestList.transform.GetChild(0), 
                    true);

                requestUI.name = id;
                requestUI.GetComponentInChildren<Text>().text = userId;
                requestUI.GetComponent<Button>()
                    .OnClickAsObservable()
                    .Subscribe(__ =>
                    {
                        Debug.Log("Hello ..." + userId);
                    })
                    .AddTo(requestUI);
                
            })
            .AddTo(gameObject);


        client.FriendRequestRemoval
            .ObserveOnMainThread()
            .Subscribe(id =>
            {
                
                var requestUI = requestList.transform.GetChild(0).Find(id);
                GameObject o;
                (o = requestUI.gameObject).SetActive(false);
                Destroy(o);
                
            })
            .AddTo(gameObject);
    }
}