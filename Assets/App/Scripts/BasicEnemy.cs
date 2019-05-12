using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityIoC;
using UTJ;

public class BasicEnemy : IUpdatable
{
    private static double game_time;
    
    public RigidbodyTransform rigidbody;
    public IUpdatable target;

    private Vector3 targeted_pos;
    private IEnumerator routine;
    
    Utility.WaitForSeconds moveUp = new Utility.WaitForSeconds(1f, game_time);
    Utility.WaitForSeconds moveForward = new Utility.WaitForSeconds(3f, game_time);

    public void Init()
    {
        targeted_pos = new Vector3(0, 0, 0);
        routine = GetRoutine();

        moveUp.reset(game_time);
        moveForward.reset(game_time);
    }
    public BasicEnemy()
    {
        //setup physics materials 
        rigidbody.setDamper(2f);
        rigidbody.setRotateDamper(4f);
    }

    private IEnumerator GetRoutine()
    {
        while (!moveUp.end(game_time))
        {
            rigidbody.addSpringTorque(ref targeted_pos, 4f);
            rigidbody.addSpringForceY(target_y: targeted_pos.y, ratio: 4f);

            yield return null;
        }

        while (!moveForward.end(game_time))
        {
            rigidbody.addHorizontalStableTorque(8f /* torque_level */);
            rigidbody.addRelativeForceZ(80f);

            yield return null;
        }

        yield return null;

        Alive = false;
        Enable = false;
    }
    

    public bool Alive { get; set; }
    public bool Enable { get; set; }

    public void Update(float delta_time, double gametime)
    {
        game_time = gametime;

        targeted_pos = target.Transform.position;

        routine.MoveNext();

        rigidbody.update(delta_time);
    }

    public MyTransform Transform
    {
        get
        {
            return rigidbody.transform;
        }
    }
}