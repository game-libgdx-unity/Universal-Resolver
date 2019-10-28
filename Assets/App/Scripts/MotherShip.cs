using UnityEngine;
using UnityIoC;
using UTJ;

public class MotherShip : IUpdatable
{
    private Material material;
    private Mesh mesh;
    
    public MyTransform Transform { get; set; }
    
    public MotherShip(GameObject prefab)
    {
        Enable = true;
        mesh = prefab.GetComponent<MeshFilter>().sharedMesh;
        material = prefab.GetComponent<Renderer>().sharedMaterial;
    }
    public void Destroy()
    {
    }

    public bool Enable { get; set; }
    public void Update(float delta_time, double game_time)
    {
        Graphics.DrawMesh(mesh, Transform.position, Transform.rotation, material, 0);
    }

    
    public bool Alive { get; set; }
    public void OnReused()
    {
    }
}