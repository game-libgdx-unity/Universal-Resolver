using System;
using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityEngine.UI;
using UnityIoC;
using Object = UnityEngine.Object;

public class PlayerUI : MonoBehaviour, IDataBinding<PlayerView>
{
    [SerializeField, Inject] Text txtName;
    [SerializeField, Inject] Button btnDelete;

    public void OnNext(PlayerView playerView)
    {
        txtName.text = playerView.meta.DisplayName;
        btnDelete.onClick.AddListener(() => { Context.Delete(playerView); });
    }
}