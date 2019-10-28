using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityIoC;
using UTJ;

namespace UnityIoC
{
    /// <summary>
    /// Manage updatable objects of a particular Type
    /// </summary>
    public class UpdatableGroup<T> : IUpdatable where T : IUpdatable, IUpdatableItem<T>, new()
    {
        private static double game_time;

        private T[] items;
        private Matrix4x4[] matrixces;
        private int maxObj;
        private int activeObj;
        private int currentIndex;

        private Vector3 targeted_pos;
        private IEnumerator routine;
        private Material material;
        private Mesh mesh;

        public UpdatableGroup(int maxObj, GameObject prefab)
        {
            this.maxObj = maxObj;
            items = new T[maxObj];

            for (int i = 0; i < maxObj; i++)
            {
                items[i] = new T();
                items[i].Group = this;
            }

            matrixces = new Matrix4x4[maxObj];
            routine = GetRoutine();
            mesh = prefab.GetComponent<MeshFilter>().sharedMesh;
            material = prefab.GetComponent<Renderer>().sharedMaterial;

            //UpdatableGroup doesn't pool, so set them in constructors
            //otherwises, these properties shouldn't be modified.
            Alive = true;
            Enable = true;
        }
        
        public void RemoveFromPool(T obj)
        {
            obj.Alive = false;
            obj.Enable = false;
            activeObj--;
        }

        public T GetFromPool(Action<T> beforeInit = null)
        {
            for (int i = 0; i < maxObj; i++)
            {
                if (!items[i].Alive)
                {
                    activeObj++;
                    items[i].Alive = true;
                    items[i].Enable = true;

                    beforeInit?.Invoke(items[i]);

                    items[i].OnReused();

                    return items[i];
                }
            }

            Debug.LogError("Excess maxObj of " + GetType().Name);
            return default(T);
        }

        protected virtual IEnumerator GetRoutine()
        {
            yield return null;
        }

        public int ActiveObjects
        {
            get { return activeObj; }
        }

        public bool Alive { get; set; }

        public void OnReused()
        {
        }

        public bool Enable { get; set; }

        public void Update(float delta_time, double gametime)
        {
            game_time = gametime;

            routine.MoveNext();

            currentIndex = 0;

            for (var index = 0; index < items.Length; index++)
            {
                var updatableItem = items[index];
                if (updatableItem.Enable)
                {
                    updatableItem.Update(delta_time, game_time);
                    matrixces[currentIndex] = updatableItem.Transform.getTRS();

                    currentIndex++;

                    if (currentIndex >= activeObj)
                    {
                        break;
                    }
                }
            }

            activeObj = currentIndex;

            Graphics.DrawMeshInstanced(mesh, 0, material, matrixces, currentIndex, null, ShadowCastingMode.Off, false);
        }

        public MyTransform Transform { get; set; }
    }
}