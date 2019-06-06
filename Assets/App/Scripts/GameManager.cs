using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;
using UnityIoC;

/// <summary>
/// Manage object creations & add them to System Manager
/// </summary>
public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject motherShipPrefab;
    public int maxObj = 1;

    public Text text;
    UpdatableGroup<BasicEnemy> enemyGroup;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        enemyGroup = Context.Resolve<UpdatableGroup<BasicEnemy>>(LifeCycle.Singleton, null, maxObj, enemyPrefab);
        SystemManager.Instance.Add(enemyGroup);
        
        var motherShip = Context.Resolve<MotherShip>(motherShipPrefab);
        for (int i = 0; i < maxObj; i++)
        {
            var enemyObj = enemyGroup.GetFromPool();
            //setup inital position
            enemyObj.rigidbody.transform.position = new Vector3(Random.Range(-10, 10), -10, Random.Range(-10, 10));
            enemyObj.rigidbody.transform.rotation = Quaternion.Euler(Random.Range(-60, -120), 0, 0);
            enemyObj.target = motherShip;

            yield return null;
        }
    }

    private void Update()
    {
        text.text = enemyGroup.ActiveObjects.ToString();
    }
}