using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "TestConfiguration", menuName = "Test/TestConfiguration", order = 1)]
public class ImplementClass :ScriptableObject
{
    [SerializeField] public Object[] classObjectsToLoad;
}
