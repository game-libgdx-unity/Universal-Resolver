using UnityEngine;

public class UnityComponent : MonoBehaviour
{
    
    [LogParams("{0}")]
    public void DoIt3(string msg)
    {
    }
    [LogParams()]
    public void DoIt(string msg)
    {
    }
    [Log()]
    public void DoIt2(string msg)
    {
    }
//    [Log]
//    public void DoIt()
//    {
//        Debug.Log("nt");
//        return;
//    }

    public void Start()
    {
        DoIt("method 1");
    }
    public void Start2()
    {
        DoIt2("method 2");
    }
    public void Start3()
    {
        DoIt3("method 3");
    }
}