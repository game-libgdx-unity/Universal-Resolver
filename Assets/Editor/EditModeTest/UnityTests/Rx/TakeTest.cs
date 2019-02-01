/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
using NUnit.Framework;

namespace UniRx.Tests.Operators
{
    public class TakeTest
    {
        [Test]
        public void TakeCount()
        {
            var range = Observable.Range(1, 10);

            Assert.Throws<ArgumentOutOfRangeException>(() => range.Take(-1));

            range.Take(0).ToArray().Wait().Length.Is(0);

            range.Take(3).ToArrayWait().Is(1, 2, 3);
            range.Take(15).ToArrayWait().Is(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }
    }
}
