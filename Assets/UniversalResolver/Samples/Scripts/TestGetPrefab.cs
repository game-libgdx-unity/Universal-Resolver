using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityIoC;

namespace SceneTest
{
    public class TestGetPrefab : MonoBehaviour
    {
        [Prefab] private TestComponent prefab;
//        [Transient] private TestComponent prefab2;
//        [Singleton] private TestComponent prefab3;

        private void Awake()
        {
            Context.GetDefaultInstance(this, true);
            prefab.Afield = 100;
        }

        // Start is called before the first frame update
        void Start()
        {
//            var t1 = Context.Instantiate(prefab);
//            var t2 = Context.Instantiate(prefab);
//            var t3 = Context.Instantiate(prefab);
//
//            t1.abstractClass.DoSomething();
//            t2.abstractClass.DoSomething();
//            t3.abstractClass.DoSomething();
//
//            Debug.Log(t1.Afield);
//            Debug.Log(t3.Afield);

            Context.ResolveObject<TestComponent>().abstractClass.DoSomething();
            
            Invoke("Reset", 5);
        }

        private void Reset()
        {
            Context.DisposeDefaultInstance();
            Debug.Log("Reset.......................");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}