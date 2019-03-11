using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum LogDetails : byte
{
    None = 0,
    Type = 1 << 0,
    Method = 1 << 1,
    Filename = 1 << 2,
    LineNumber = 1 << 3,
    All = Type | Method | Filename
}

public class LogAttribute : Attribute
{
    public LogData Data = new LogData();
    public string MethodName;
    public string DeclaringType;

    public LogAttribute(
        string contentFormat = "",
        string headerFormat = "{0}\t{1}\t{2}{3}",
        string logFormat = "{0}\t{1}",
        LogDetails LogDetails = LogDetails.Type | LogDetails.Method)
    {
        this.Data.contentFormat = contentFormat;
        this.Data.headerFormat = headerFormat;
        this.Data.logFormat = logFormat;
        this.Data.LogDetails = LogDetails;
    }

    [Serializable]
    public class LogData
    {
        public LogDetails LogDetails;

        public string headerFormat;

        public string contentFormat;

        public string logFormat;
    }
}

public class LogParamsAttribute : LogAttribute
{
    public LogParamsAttribute(
        string contentFormat = "{0} {1} {2} {3} {4}",
        string headerFormat = "{0}\t{1}\t{2}{3}",
        string logFormat = "{0}\t{1}",
        LogDetails LogDetails = LogDetails.Method)
        : base(contentFormat, headerFormat, logFormat, LogDetails)
    {
    }
}