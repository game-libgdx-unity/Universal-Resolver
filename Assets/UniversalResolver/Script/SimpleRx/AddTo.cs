/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
using System.Collections.Generic;
using UnityEngine;

public class AddTo : MonoBehaviour
{
    internal HashSet<IDisposable> disposables = new HashSet<IDisposable>();

    private void OnDestroy()
    {
        if (disposables != null)
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
        }
    }
}