using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;

public class SystemManager : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private const int targeted_FPS = 60;
    private const float delta_time = 1f / targeted_FPS;

    private float game_time;

    private List<IUpdatable> updatables = new List<IUpdatable>();

    // Start is called before the first frame update
    void Start()
    {
        var enemy = Context.Resolve<BasicEnemy>(prefab);
        Add(enemy);

        for (int i = 0; i < 10; i++)
        {
            var obj = Context.Resolve<DrawableObj>(new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)));
            Add(obj);
        }
    }

    private void Add(IUpdatable obj)
    {
        updatables.Add(obj);
    }

    // Update is called once per frame
    void Update()
    {
        game_time += delta_time;

        foreach (var obj in updatables)
        {
            if (obj.Enable)
            {
                obj.Update(delta_time, game_time);
            }
        }
    }
}