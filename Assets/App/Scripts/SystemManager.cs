using System.Collections.Generic;
using UnityEngine;
using UnityIoC;

public class SystemManager : SingletonBehaviour<SystemManager>
{
    List<IUpdatable> updatables = new List<IUpdatable>(); 
    protected override void Awake()
    {
        base.Awake();
        
//        Context.OnDisposed.Subscribe(this, obj =>
//        {
//            var item = iup
//            if(obj)
//        })

        Application.targetFrameRate = target_fps;

    }

    
    private float game_time;
    private const float delta_time = 1f / target_fps;
    private const int target_fps = 60;

    private void OnDestroy()
    {
        foreach (var updatable in updatables)
        {
            updatable.Dispose();
        }
    }

    private void LateUpdate()
    {
        game_time += delta_time;

        foreach (var updatable in updatables)
        {
            if (updatable.Alive && updatable.Enable)
            {
                updatable.Update(delta_time, game_time);
            }
        }
    }
    
    public void Add(IUpdatable updatable)
    {
        updatables.Add(updatable);
    }
}