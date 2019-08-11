using System.Collections.Generic;
using UnityEngine;
using UnityIoC;

/// <summary>
/// Manage updatable objects are created in Game
/// </summary>
public class SystemManager : SingletonBehaviour<SystemManager>
{
    private static double game_time;
    private const float delta_time = 1f / target_fps;
    private const int target_fps = 60;

    HashSet<IUpdatable> updatables = new HashSet<IUpdatable>();

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = target_fps;
        
        Context.OnResolved<IUpdatable>().Subscribe(SystemManager.Instance.Add);
        Context.OnDisposed<IUpdatable>().Subscribe(SystemManager.Instance.Remove);

    }

    private void Update()
    {
        game_time += delta_time;

        foreach (var updatable in updatables)
        {
            if (updatable.Enable)
            {
                updatable.Update(delta_time, game_time);
            }
        }
    }

    public void Add(IUpdatable updatable)
    {
        updatables.Add(updatable);
    }

    public void Remove(IUpdatable updatable)
    {
        updatables.Remove(updatable);
    }
}