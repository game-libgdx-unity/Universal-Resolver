using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityIoC;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        const int maxObj = 1000;
        
        var enemyGroup = new UpdatableGroup<BasicEnemy>(maxObj, enemy);
        SystemManager.Instance.Add(enemyGroup);

        for (int i = 0; i < maxObj; i++)
        {
            var enemyObj = enemyGroup.GetFromPool();
            //setup inital position
            enemyObj.rigidbody.transform.position = new Vector3(Random.Range(-10,10), -10, Random.Range(-10,10));
            enemyObj.rigidbody.transform.rotation = Quaternion.Euler(Random.Range(-60,-120), 0, 0);
            
            yield return null;
        }
        
        //enemyObj.Dosomething...
        
        yield return null;
    }
}
