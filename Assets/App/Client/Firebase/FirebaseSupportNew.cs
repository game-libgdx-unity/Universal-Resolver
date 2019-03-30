/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC or http://u3d.as/1rQJ
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UniRx;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Client))]
public class FirebaseSupportNew : MonoBehaviour
{
    #region Variables & constants

    [Component] Client client;
    [SerializeField] string userId;

    public BoolReactiveProperty IsReady = new BoolReactiveProperty();
    public BoolReactiveProperty IsLoading = new BoolReactiveProperty();

    /// <summary>
    /// https://YOUR-FIREBASE-APP.firebaseio.com/
    /// </summary>
    string editorDatabaseUrl = "https://zalgame-1ae27.firebaseio.com/";

    /// <summary>
    /// https://console.cloud.google.com/iam-admin/serviceaccounts/project?project=zalgame-1ae27
    /// </summary>
    string p12FileName = "zalgame-1ae27-b6849c793df6.p12";

    string acountEmail = "zalgame-1ae27@appspot.gserviceaccount.com";
    string passwd = "notasecret";
    string cloudFunctionUrl = "https://us-central1-zalgame-1ae27.cloudfunctions.net/";

    public const string FriendPath = "friends/";
    public const string FriendRequestPath = "friendrequests/";

    #endregion

    #region Private members

    // Use this for initialization
    void Start()
    {
        client = GetComponent<Client>();

        client.IsReady.Where(b => b).Take(1)
            .Subscribe(_ => { Connect(client.ChatClient.UserId); })
            .AddTo(gameObject);

        client.SentMessages
            .Where(data => data.GetActionId() == ActionId.SendFriendRequest)
            .Select(data => (string) data[Field.Receiver])
            .Subscribe(userId =>
            {
                Set(FriendRequestPath + this.userId + "/" + userId, new FirebaseManager.SendFriendRequest()
                {
                    created = DateTime.Now.Ticks
                });
            })
            .AddTo(gameObject);

        client.SentMessages
            .Where(data => data.GetActionId() == ActionId.AcceptFriendRequest)
            .Select(data => (string) data[Field.Receiver])
            .Subscribe(userId =>
            {
                Set(FriendPath + this.userId + "/" + userId, new FirebaseManager.SendFriendRequest()
                {
                    created = DateTime.Now.Ticks
                });
            })
            .AddTo(gameObject);
    }

    protected IObservable<bool> Delete(string path)
    {
        IsLoading.Value = true;
        var b = new Subject<bool>();
        DatabaseReference childToRemove = FirebaseDatabase.DefaultInstance.RootReference.Child(path);
        var task1 = childToRemove.RemoveValueAsync().ContinueWith(
            task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.Log("Fail");
                    b.OnError(task.Exception);
                    return;
                }

                IsLoading.Value = false;
                b.OnNext(true);
                b.OnCompleted();
            });


        return b.AsObservable();
    }

    protected IObservable<IList<T>> GetMany<T>(string path)
    {
        return GetDataSnapShot(path)
            .Where(ss => ss.HasChildren)
            .Select(snap =>
            {
                IList<T> list = new List<T>((int) snap.ChildrenCount);

                if (snap.HasChildren)
                {
                    foreach (var snapshot in snap.Children)
                    {
                        var value = snapshot.GetRawJsonValue().FromJson<T>();
                        list.Add(value);
                    }
                }

                return list;
            });
    }

//protected IObservable<bool> IsExist<T>(string path)
//    {
//        return GetDataSnapShot(path)
//            .Where(ss => ss.HasChildren)
//            .Select(snap =>
//            {
//                IList<T> list = new List<T>((int) snap.ChildrenCount);
//
//                if (snap.HasChildren)
//                {
//                    foreach (var snapshot in snap.Children)
//                    {
//                        var value = snapshot.GetRawJsonValue().FromJson<T>();
//                        list.Add(value);
//                    }
//                }
//
//                return list;
//            });
//    }

    /// <summary>
    /// get DataSnapshot
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public IObservable<DataSnapshot> GetDataSnapShot(string path)
    {
        IsLoading.Value = true;

        var getData = new Subject<DataSnapshot>();
        FirebaseDatabase.DefaultInstance
            .GetReference(path).GetValueAsync().ContinueWith((task) =>
            {
                if (task.IsFaulted)
                {
                    getData.OnError(task.Exception);
                    return;
                }

                getData.OnNext(task.Result);
                getData.OnCompleted();

                IsLoading.Value = false;

                var d = task.Result;
            });

        return getData.AsObservable();
    }

    /// <summary>
    /// Check if the path is available from database
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public IObservable<bool> IsExisting(string path)
    {
        IsLoading.Value = true;
        var getData = new ReactiveProperty<bool>();

        FirebaseDatabase.DefaultInstance
            .GetReference(path).GetValueAsync().ContinueWith((task) =>
            {
                if (task.IsFaulted)
                {
                    getData.SetValueAndForceNotify(false);
                    return;
                }

                getData.SetValueAndForceNotify(task.Result.Exists);
                getData.Dispose();
            });

        return getData.TakeLast(1);
    }

    protected IObservable<bool> Set(string datapath, object data)
    {
        var b = new Subject<bool>();
        var json = JsonUtility.ToJson(data);
        IsLoading.Value = true;

        FirebaseDatabase.DefaultInstance.GetReference(datapath).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            IsLoading.Value = false;

            if (task.IsFaulted)
            {
                b.OnError(task.Exception);
                return;
            }

            b.OnNext(task.IsCompleted);
            b.OnCompleted();
        });

        return b.TakeLast(1);
    }

    private void Connect(string userId)
    {
        this.userId = userId;

        //Stab.
#if UNITY_EDITOR

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(editorDatabaseUrl);
        FirebaseApp.DefaultInstance.SetEditorP12FileName(p12FileName);
        FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail(acountEmail);
        FirebaseApp.DefaultInstance.SetEditorP12Password(passwd);
        FirebaseApp.DefaultInstance.SetEditorAuthUserId(userId);

#endif

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                IsReady.Value = true;
                print("Using firebase with id: " + userId);
            }
            else
            {
                Debug.LogError(String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    #endregion

    #region Public members

    public IObservable<string> ObserverRequestsComing()
    {
        var reference = FirebaseDatabase.DefaultInstance
            .GetReference(FriendRequestPath + this.userId);

        return Observable.FromEventPattern<EventHandler<ChildChangedEventArgs>, ChildChangedEventArgs>(
                h => h.Invoke,
                h => reference.ChildAdded += h,
                h => reference.ChildAdded -= h)
            .Select(arg => arg.EventArgs.Snapshot.Key);
    }

    public IObservable<string> ObserverRequestRemoval()
    {
        var reference = FirebaseDatabase.DefaultInstance
            .GetReference(FriendRequestPath + this.userId);

        return Observable.FromEventPattern<EventHandler<ChildChangedEventArgs>, ChildChangedEventArgs>(
                h => h.Invoke,
                h => reference.ChildRemoved += h,
                h => reference.ChildRemoved -= h)
            .Select(arg => arg.EventArgs.Snapshot.Key);
    }

    public IObservable<IList<FirebaseManager.SendFriendRequest>> GetFriendRequests()
    {
        return GetMany<FirebaseManager.SendFriendRequest>(FriendRequestPath + this.userId);
    }

    #endregion
}