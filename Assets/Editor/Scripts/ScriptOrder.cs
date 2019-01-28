using System;

[AttributeUsage(AttributeTargets.Class)]
public class ScriptOrder : Attribute
{
    public ScriptOrder(int order)
    {
        this.order = order;
    }

    public int order;
}