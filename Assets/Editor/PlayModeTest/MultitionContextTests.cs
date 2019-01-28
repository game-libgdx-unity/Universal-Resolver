using System.Linq;
using NUnit.Framework;
using SimpleIoc;
using UnityEngine;

public class MultitionContextTests
{
    [SetUp]
    public void ZalClientTestRunnerSimplePasses()
    {
        var  multiContext = Singleton<Multiton<byte, Context>>.Instance;
        
        var _context = new Context();
        _context.Initialize(this);
        multiContext[0] = _context;
        
        _context = new Context();
        _context.Initialize(this);
        multiContext[1] = _context;
        
    }
    
    
    [Test]
    public void Test1_Create_Object_Instance()
    {
        var  multiContext = Singleton<Multiton<byte, Context>>.Instance;
        
        foreach (var _context in multiContext)
        {
            _context.loadDefaultSetting = false;
        }
        
        //assert
        Assert.AreEqual(multiContext.Count(), 2);
    }
}