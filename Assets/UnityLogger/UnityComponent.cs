using UnityEngine;

public class UnityComponent : MonoBehaviour
{
    [Log]
    public void DoIt(string msg)
    {
        Debug.Log(msg);
    }
//    [Log]
//    public void DoIt()
//    {
//        Debug.Log("nt");
//        return;
//    }

    public void Start()
    {
        DoIt("sasa");
    }
}