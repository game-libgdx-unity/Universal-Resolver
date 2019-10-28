using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;
using UTJ;

public class DrawableObj : IUpdatable
{
    protected Material material;
    protected Mesh mesh;
    protected Vector3 position;
  
    public DrawableObj()
    {
        this.position = position;
        Enable = true;

        material = Resources.Load<Material>("zako_mat");
        mesh = Resources.Load<GameObject>("zako").GetComponent<MeshFilter>().sharedMesh;
    }

    public void OnReused()
    {
    }
    
    public bool Enable { get; set; }

    public void Update(float delta_time, double game_time)
    {
        Graphics.DrawMesh(mesh, position, Quaternion.identity, material, 0);
    }

    public MyTransform Transform { get; set; }

    public bool Alive { get; set; }
}