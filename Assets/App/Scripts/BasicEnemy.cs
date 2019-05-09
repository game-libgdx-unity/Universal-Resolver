﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;
using UTJ;

public class BasicEnemy : IUpdatableBehaviour
{
    public Transform transform { get; }
    public Renderer renderer { get; }

    public BasicEnemy(GameObject GO)
    {
        if (!GO)
        {
            Debug.Log("No game objects in constructor");
            GO = Object.Instantiate(Resources.Load<GameObject>("zako"));
        }

        this.transform = GO.GetComponent<Transform>();
        this.renderer = GO.GetComponent<Renderer>();
        this.routine = GetRoutine();

        //setup physics materials 
        rigidbody.setDamper(2f);
        rigidbody.setRotateDamper(4f);

        //setup inital position
        rigidbody.transform.position = new Vector3(0, -10, -10);
        rigidbody.transform.rotation = Quaternion.Euler(-90, 0, 0);
        targeted_pos = new Vector3(0,0,0);

        Enable = true;
    }

    private RigidbodyTransform rigidbody;

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

        transform.localPosition = rigidbody.transform.position;
        transform.localRotation = rigidbody.transform.rotation;
    }
}