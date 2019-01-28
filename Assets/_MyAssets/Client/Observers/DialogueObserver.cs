using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class DialogueObserver : IObserver<string>
{
    private readonly GameObject canvas;
    private readonly string prefabName;
    private readonly string message;
    private readonly bool sharedUI;
    private readonly Action<string> onAcceptButtonClick;
    private readonly Action<string> onCancelButtonClick;

    private GameObject canvasInstance;

    public DialogueObserver(
        GameObject canvas, 
        string message, 
        Action<string> onAcceptButtonClick,
        Action<string> onCancelButtonClick)
        : this(canvas, null, message, true, onAcceptButtonClick, onCancelButtonClick)
    {
    }
    
    public DialogueObserver(
        string prefabName, 
        string message, 
        Action<string> onAcceptButtonClick,
        Action<string> onCancelButtonClick)
        : this(null, prefabName, message, true, onAcceptButtonClick, onCancelButtonClick)
    {
    }

    protected DialogueObserver(GameObject canvas, string prefabName, string message, bool sharedUi,
        Action<string> onAcceptButtonClick, Action<string> onCancelButtonClick)
    {
        this.canvas = canvas;
        this.prefabName = prefabName;
        this.message = message;
        sharedUI = sharedUi;
        this.onAcceptButtonClick = onAcceptButtonClick;
        this.onCancelButtonClick = onCancelButtonClick;
    }


    public void OnNext(string value)
    {
        if (canvasInstance == null)
        {
            this.canvasInstance = GameObject.Instantiate(canvas);
        }

        //set message content
        canvasInstance.GetComponentInChildren<Text>().text = message;

        //hide cancel button if there are no callback for it
        canvasInstance.transform.Find("Background/Buttons/BtnCancel").gameObject.SetActive(onCancelButtonClick != null);

        canvasInstance.transform.Find("Background/Buttons/BtnAccept")
            .GetComponent<Button>()
            .OnClickAsObservable()
            .Take(1)
            .Subscribe(_ =>
            {
                if (onAcceptButtonClick != null)
                {
                    onAcceptButtonClick(value);
                }

                GameObject.Destroy(canvasInstance);
            });

        canvasInstance.transform.Find("Background/Buttons/BtnCancel")
            .GetComponent<Button>()
            .OnClickAsObservable()
            .Take(1)
            .Subscribe(_ =>
            {
                if (onCancelButtonClick != null)
                {
                    onCancelButtonClick(value);
                }

                GameObject.Destroy(canvasInstance);
            });
    }

    public void OnError(Exception error)
    {
        canvasInstance.GetComponentInChildren<Text>().text = "Error: " + error.Message;
        canvasInstance.transform.Find("Background/Buttons/BtnAccept")
            .GetComponent<Button>()
            .OnClickAsObservable()
            .Take(1)
            .Subscribe(_ => { GameObject.Destroy(canvasInstance); });
    }

    public void OnCompleted()
    {
        GameObject.Destroy(canvasInstance);
    }
}