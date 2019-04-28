using UnityEngine;

[CreateAssetMenu(fileName = TestRunner.TestConfigName, menuName = "IoC/Test Config")]

public class TestConfiguration :ScriptableObject
{
    [SerializeField] public string sceneFolderPath;
    [SerializeField] public Object[] ScenesToTest;
}