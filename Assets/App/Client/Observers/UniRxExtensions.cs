///**
// * Author:    Vinh Vu Thanh
// * This class is a part of Unity IoC project that can be downloaded free at 
// * https://github.com/game-libgdx-unity/UnityEngine.IoC or http://u3d.as/1rQJ
// * (c) Copyright by MrThanhVinh168@gmail.com
// **/
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public static class UniRxExtensions
{
    public static IDisposable SubscribeButton<T>(
        this IObservable<T> observable,
        Button button,
        Action<T> onClicked)
    {
        if (!button.gameObject.activeInHierarchy)
        {
            button.gameObject.SetActive(true);
        }

        return observable.Subscribe(id => { button.OnClickAsObservable().Subscribe(_ => { onClicked(id); }); })
            .AddTo(button);
    }

    public static IDisposable SubscribeToText<T>(
        this IObservable<T> observable,
        Text text)
    {
        if (!text.gameObject.activeInHierarchy)
        {
            text.gameObject.SetActive(true);
        }

        return observable.Subscribe(t => { text.text = t as string; }).AddTo(text);
    }

    public static IDisposable SubscribeToGUI<T>(
        this IObservable<T> observable,
        string namesubject = "")
    {
        return observable.Subscribe(new GUIObserver<T>(namesubject));
    }

    public static IDisposable SubscribeToConsole<T>(
        this IObservable<T> observable,
        string namesubject = "")
    {
        return observable.Subscribe(new ConsoleObserver<T>(namesubject));
    }

    public static IDisposable SubscribeTransform(
        this IObservable<Transform> observable,
        Transform parent)
    {
        return observable.Subscribe(transform => { transform.SetParent(parent); })
            .AddTo(parent.gameObject);
    }


    public static IDisposable SubscribeToListCanvas(
        this IObservable<IList<string>> observable,
        GameObject canvas,
        GameObject prefab,
        Action<string> onPlayerIdClicked)
    {
        return observable.Subscribe(new ListItemObserver(canvas, prefab, onPlayerIdClicked));
    }

    public static IDisposable SubscribeToChallengeOptionCanvas(
        this IObservable<string> observable,
        GameObject canvas,
        Action<string> onPlayerIdClicked,
        Action<string> onCancelClicked)
    {
        return observable.Subscribe(new ChallengeOptionCanvasObserver(canvas, onPlayerIdClicked, onCancelClicked));
    }

    public static IDisposable SubscribeToDialogueCanvas(
        this IObservable<string> observable,
        GameObject canvas,
        string message,
        Action<string> OnAcceptButtonClick,
        Action<string> OnCancelButtonClick)
    {
        return observable.Subscribe(new DialogueObserver(canvas, message, OnAcceptButtonClick, OnCancelButtonClick));
    }
}