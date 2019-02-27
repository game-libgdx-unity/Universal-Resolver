using System.Linq;
using NUnit.Framework;
using UnityIoC;
using UnityEngine;

public class MultitionContextTests
{
    [SetUp]
    public void Setup()
    {
        var  multiContext = Singleton<Multiton<byte, AssemblyContext>>.Instance;
        
        var _context = new AssemblyContext();
        _context.Initialize(typeof(AssemblyContext));
        multiContext[0] = _context;
        
        _context = new AssemblyContext();
        _context.Initialize(typeof(AssemblyContext));
        multiContext[1] = _context;
        
    }
    
    
    [Test]
    public void Test1_Create_Object_Instance()
    {
        var  multiContext = Singleton<Multiton<byte, AssemblyContext>>.Instance;
        
        foreach (var _context in multiContext)
        {
            
        }
        
        //assert
        Assert.AreEqual(multiContext.Count(), 2);
    }
}