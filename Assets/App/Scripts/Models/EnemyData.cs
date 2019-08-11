using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Models/Enemy", order = 0)]
public class EnemyData : ScriptableObject
{
    public float moveUpDuration;
    public float forwardDuration;
}