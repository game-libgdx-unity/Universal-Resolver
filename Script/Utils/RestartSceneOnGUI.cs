using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartSceneOnGUI : MonoBehaviour
{
    [SerializeField] private GUISkin skin;
    [SerializeField] private GUIStyle style;
    [SerializeField] private int width = 200;
    [SerializeField] private int height = 70;
    void OnGUI()
    {
        if (style != null)
        {
            GUI.skin = skin;
        }

        if (GUI.Button(new Rect(20, 40, width, height), "Reset"))
        {
            Application.LoadLevel(SceneManager.GetActiveScene().name);
        }
    }
}