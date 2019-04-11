using System;
using System.Collections;

[AttributeUsage(AttributeTargets.Class)]
public class ProcessingOrderAttribute : Attribute, IComparer
{
    public int Order { get; set; }

    public ProcessingOrderAttribute(int order = 10)
    {
        this.Order = order;
    }

    public int Compare(object x, object y)
    {
        return (x as ProcessingOrderAttribute).Order - (y as ProcessingOrderAttribute).Order;
    }
}