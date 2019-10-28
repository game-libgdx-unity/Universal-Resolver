using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityIoC;
using UTJ;

public class BasicEnemy : IUpdatable, IUpdatableItem<BasicEnemy>
{
    private static double game_time;
    
    public RigidbodyTransform rigidbody;
    public IUpdatable target;

    private Vector3 targeted_pos;
    private IEnumerator routine;

    private Utility.WaitForSeconds moveUp;
    private Utility.WaitForSeconds moveForward;

    public EnemyData Data;

    public void OnReused()
    {
        //set the updating routine
        routine = GetRoutine();

        //setup routine timeline
        moveUp = new Utility.WaitForSeconds(Data.moveUpDuration, game_time);
        moveForward = new Utility.WaitForSeconds(Data.forwardDuration, game_time);
        
        //setup physics materials 
        rigidbody.setDamper(2f);
        rigidbody.setRotateDamper(4f);

        //setup target position
        targeted_pos = new Vector3(0, 0, 0);        
        

        moveUp.reset(game_time);
        moveForward.reset(game_time);
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

        Group.RemoveFromPool(this);
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
        set
        {
            rigidbody.transform = value;
        }
    }

    public UpdatableGroup<BasicEnemy> Group { get; set; }
}