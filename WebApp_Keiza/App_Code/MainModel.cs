using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for MainModel
/// </summary>
public class MainModel
{
    private string id = "";
    [DataMember]
    public string GetSetid
    {
        get
        {
            string text = id;
            if (text != null)
                return text;
            else
                return string.Empty;
        }
        set
        {
            id = value;
        }
    }

    private string idno = "";
    [DataMember]
    public string GetSetidno
    {
        get
        {
            string text = idno;
            if (text != null)
                return text;
            else
                return string.Empty;
        }
        set
        {
            idno = value;
        }
    }

    private string idtype = "";
    [DataMember]
    public string GetSetidtype
    {
        get
        {
            string text = idtype;
            if (text != null)
                return text;
            else
                return string.Empty;
        }
        set
        {
            idtype = value;
        }
    }

    private string name = "";
    [DataMember]
    public string GetSetname
    {
        get
        {
            string text = name;
            if (text != null)
                return text;
            else
                return string.Empty;
        }
        set
        {
            name = value;
        }
    }

    private string status = "";
    [DataMember]
    public string GetSetstatus
    {
        get
        {
            string text = status;
            if (text != null)
                return text;
            else
                return string.Empty;
        }
        set
        {
            status = value;
        }
    }
}