using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityIoC;

public class UpdatableGroup<T> : IUpdatable where T : IUpdatableItem, new()
{
    private T[] items;
    private Matrix4x4[] matrixces;
    private int maxObj;
    private int activeObj;
    private int currentIndex;
    
    private float game_time;
    private Vector3 targeted_pos;

    private IEnumerator routine;
    private Material material;
    private Mesh mesh;

    public UpdatableGroup(int maxObj, GameObject gameObject)
    {
        this.maxObj = maxObj;
        items = new T[maxObj];

        for (int i = 0; i < maxObj; i++)
        {
            items[i] = new T();
        }

        matrixces = new Matrix4x4[maxObj];
        routine = GetRoutine();
        mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
        material = gameObject.GetComponent<Renderer>().sharedMaterial;

        //UpdatableGroup doesn't pool, so set them in constructors
        //otherwises, these properties shouldn't be modified.
        Alive = true;
        Enable = true;
    }

    public void Dispose(T obj)
    {
        obj.Alive = false;
        obj.Enable = false;
        obj.Dispose();
        activeObj--;
    }

    public T GetFromPool()
    {
        for (int i = 0; i < maxObj; i++)
        {
            if (!items[i].Alive)
            {
                activeObj++;
                items[i].Alive = true;
                items[i].Enable = true;
                items[i].Init();
                
                return items[i];
            }
        }

        Debug.LogError("Excess maxObj of "+GetType().Name);
        return default(T);
    }

    protected virtual IEnumerator GetRoutine()
    {
        yield return null;
    }

    public bool Alive { get; set; }
    public void Init()
    {
    }

    public bool Enable { get; set; }

    public void Update(float delta_time, float game_time)
    {
        this.game_time = game_time;

        routine.MoveNext();

        currentIndex = 0;
        
        for (var index = 0; index < items.Length; index++)
        {
            var updatableItem = items[index];
            if (updatableItem.Enable)
            {
                updatableItem.Update(delta_time, game_time);
                matrixces[currentIndex] = updatableItem.Transform;
                
                currentIndex++;
                
                if (currentIndex >= activeObj)
                {
                    break;
                }
            }
        }

        Graphics.DrawMeshInstanced(mesh, 0, material, matrixces, activeObj, null, ShadowCastingMode.Off, false);
    }

    public void Dispose()
    {
        foreach (var item in items)
        {
            item.Dispose();
        }
    }
}