using UnityEngine;
using UTJ;

namespace UnityIoC
{
    /// <summary>
    /// Allow game objects to be updated in SystemManager
    /// </summary>
    public class UpdatableBehaviour : MonoBehaviour, IUpdatable
    {
        private void Start()
        {
            this.transform = GetComponent<Transform>();
        }
        
        public void Update(float delta_time, double game_time)
        {
            transform.localPosition = Transform.position;
            transform.localRotation = Transform.rotation;
        }

        
        public bool Alive { get; set; }
        public void Init()
        {
        }

        public void Destroy()
        {
        }

        public bool Enable { get; set; }

        private new Transform transform;
        public MyTransform Transform { get; set; }
    }
}