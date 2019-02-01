#if !(UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using UnityEngine;
/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Scripting;
using UniRx;
using UniRx.Async;
using UnityEngine.SceneManagement;
#if CSHARP_7_OR_LATER || (UNITY_2018_3_OR_NEWER && (NET_STANDARD_2_0 || NET_4_6))
using System.Threading.Tasks;
#endif
using UnityEngine.Networking;
using UnityEngine.Experimental.LowLevel;
#if !UNITY_WSA
using Unity.Jobs;
#endif
using Unity.Collections;
using System.Threading;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace UniRx.AsyncTests
{
    public class RunTest
    {
#if CSHARP_7_OR_LATER || (UNITY_2018_3_OR_NEWER && (NET_STANDARD_2_0 || NET_4_6))
#if !UNITY_WSA

        [UnityTest]
        public IEnumerator RunThread() => UniTask.ToCoroutine(async () =>
        {
            var main = Thread.CurrentThread.ManagedThreadId;
            var v = await UniTask.Run(() => 3, false);
            v.Is(3);
            main.IsNot(Thread.CurrentThread.ManagedThreadId);
        });

        [UnityTest]
        public IEnumerator RunThreadConfigure() => UniTask.ToCoroutine(async () =>
        {
            var main = Thread.CurrentThread.ManagedThreadId;
            var v = await UniTask.Run(() => 3, true);
            v.Is(3);
            main.Is(Thread.CurrentThread.ManagedThreadId);
        });

        [UnityTest]
        public IEnumerator RunThreadException() => UniTask.ToCoroutine(async () =>
        {
            var main = Thread.CurrentThread.ManagedThreadId;
            try
            {
                await UniTask.Run<int>(() => throw new Exception(), false);
            }
            catch
            {
                main.IsNot(Thread.CurrentThread.ManagedThreadId);
            }
        });

        [UnityTest]
        public IEnumerator RunThreadExceptionConfigure() => UniTask.ToCoroutine(async () =>
        {
            var main = Thread.CurrentThread.ManagedThreadId;
            try
            {
                await UniTask.Run<int>(() => throw new Exception(), true);
            }
            catch
            {
                main.Is(Thread.CurrentThread.ManagedThreadId);
            }
        });


#endif
#endif
    }
}

#endif