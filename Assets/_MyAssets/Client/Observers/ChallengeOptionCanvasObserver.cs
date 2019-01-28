using System;
using UniRx;
using UnityEngine;

public class ChallengeOptionCanvasObserver : IObserver<string>
{
    private readonly GameObject canvas;
    private readonly Action<string> onPlayerIdClicked;
    private readonly Action<string> onCancelClicked;

    private GameObject canvasInstance;
    public ChallengeOptionCanvasObserver(GameObject canvas, Action<string> onPlayerIdClicked, Action<string> onCancelClicked)
    {
        this.canvas = canvas;
        this.onPlayerIdClicked = onPlayerIdClicked;
        this.onCancelClicked = onCancelClicked;
    }

    public void OnCompleted()
    {
        GameObject.Destroy(canvasInstance);
    }

    public void OnError(Exception error)
    {
        GameObject.Destroy(canvasInstance);
    }

    public void OnNext(string value)
    {
        if(canvasInstance == null)
        {
            canvasInstance = GameObject.Instantiate(canvas);
        }
        
        
    }
}