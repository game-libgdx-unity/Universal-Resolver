using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityIoC;
using UTJ;

public class BasicEnemy : IUpdatableItem
{
    public RigidbodyTransform rigidbody;

    public void Init()
    {
        targeted_pos = new Vector3(0, 0, 0);

        routine = GetRoutine();
    }
    public BasicEnemy()
    {
        //setup physics materials 
        rigidbody.setDamper(2f);
        rigidbody.setRotateDamper(4f);

    }


    private float game_time;
    private Vector3 targeted_pos;


    private IEnumerator routine;
    private IEnumerator GetRoutine()
    {
        for (var i = new Utility.WaitForSeconds(1f, game_time); !i.end(game_time);)
        {
            rigidbody.addSpringTorque(ref targeted_pos, 4f);
            rigidbody.addSpringForceY(target_y: targeted_pos.y, ratio: 4f);

            yield return null;
        }

        for (var i = new Utility.WaitForSeconds(3f, game_time); !i.end(game_time);)
        {
            rigidbody.addHorizontalStableTorque(8f /* torque_level */);
            rigidbody.addRelativeForceZ(80f);

            yield return null;
        }

        Alive = false;

        yield return null;
    }

    public bool Alive { get; set; }
    public bool Enable { get; set; }

    public void Update(float delta_time, float game_time)
    {
        this.game_time = game_time;

        routine.MoveNext();

        rigidbody.update(delta_time);
    }

    public Matrix4x4 Transform
    {
        get
        {
            return rigidbody.transform.getTRS();
        }
    }

    public void Dispose()
    {
    }
}