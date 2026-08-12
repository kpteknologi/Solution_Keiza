using System;
using System.Collections;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
//[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{

    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        String sFlag = "Hellow World";
        sFlag = sFlag + " : LZNK";
        return sFlag;
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public void HelloWorld2(String GetSetstatus, String GetSetmessage)
    {
        String sStatus = GetSetstatus;
        String sMessage = GetSetmessage;

        String jsonResponse = "";

        object objData = new { status = sStatus, message = sMessage };

        jsonResponse = new JavaScriptSerializer().Serialize(objData);

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
        HttpContext.Current.Response.Write(jsonResponse);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public void CalculateSQF(String GetSetwidth, String GetSetheight)
    {
        double dWidth = double.Parse(GetSetwidth);
        double dHeight = double.Parse(GetSetheight);
        double dSqf = dWidth * dHeight;

        String jsonResponse = "";

        object objData = new { status = "Y", sqf = dSqf };

        jsonResponse = new JavaScriptSerializer().Serialize(objData);

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
        HttpContext.Current.Response.Write(jsonResponse);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public void getUserProfileDetails(String GetSetid)
    {

        String jsonResponse = "";

        MainController oMainCon = new MainController();

        MainModel oUserProfile = oMainCon.getUserProfileDetails(GetSetid);
        //jsonResponse = new JavaScriptSerializer().Serialize(oMod);

        object objData = new { status = "Y", userprofile = oUserProfile };

        jsonResponse = new JavaScriptSerializer().Serialize(objData);

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
        HttpContext.Current.Response.Write(jsonResponse);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public void addUserProfileDetails(MainModel userprofile)
    {

        String jsonResponse = "";
        String sStatus = "N";
        String sMessage = "Internal Server Error!";

        MainController oMainCon = new MainController();

        ArrayList lsModUser = oMainCon.getUserProfile(userprofile.GetSetidno, userprofile.GetSetidtype);
        if (lsModUser.Count > 0)
        {
            sStatus = "N";
            sMessage = "Penambahan tidak berjaya! Record already exist for table userprofile - Name: " + userprofile.GetSetname;
        }
        else
        {
            MainModel modItem = new MainModel();
            modItem.GetSetname = userprofile.GetSetname;
            modItem.GetSetidno = userprofile.GetSetidno;
            modItem.GetSetidtype = userprofile.GetSetidtype;
            modItem.GetSetstatus = userprofile.GetSetstatus;

            String result = oMainCon.addUserProfileDetails(modItem);
            if (result.Equals("Y"))
            {
                sStatus = "Y";
                sMessage = "Penambahan berjaya!";
            }
            else
            {
                sStatus = "N";
                sMessage = "Penambahan tidak berjaya! Error on updating table userprofile...";
            }
        }

        object objData = new { status = sStatus, message = sMessage };

        jsonResponse = new JavaScriptSerializer().Serialize(objData);

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
        HttpContext.Current.Response.Write(jsonResponse);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public void updateUserProfileDetails(MainModel userprofile)
    {

        String jsonResponse = "";
        String sStatus = "N";
        String sMessage = "Internal Server Error!";

        MainController oMainCon = new MainController();

        MainModel modItem = oMainCon.getUserProfileDetails(userprofile.GetSetid);
        if (modItem.GetSetid.Length > 0)
        {
            modItem.GetSetname = userprofile.GetSetname;
            modItem.GetSetidno = userprofile.GetSetidno;
            modItem.GetSetidtype = userprofile.GetSetidtype;
            modItem.GetSetstatus = userprofile.GetSetstatus;

            String result = oMainCon.updateUserProfileDetails(modItem);
            if (result.Equals("Y"))
            {
                sStatus = "Y";
                sMessage = "Kemaskini berjaya!";
            }
            else
            {
                sStatus = "N";
                sMessage = "Kemaskini tidak berjaya! Error on updating table userprofile...";
            }
        }
        else
        {
            sStatus = "N";
            sMessage = "Kemaskini tidak berjaya! Record not found for table userprofile - Id: " + userprofile.GetSetid;
        }

        object objData = new { status = sStatus, message = sMessage };

        jsonResponse = new JavaScriptSerializer().Serialize(objData);

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
        HttpContext.Current.Response.Write(jsonResponse);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public void deleteUserProfileDetails(String GetSetid)
    {

        String jsonResponse = "";
        String sStatus = "N";
        String sMessage = "Internal Server Error!";

        MainController oMainCon = new MainController();

        MainModel modItem = oMainCon.getUserProfileDetails(GetSetid);
        if (modItem.GetSetid.Length > 0)
        {
            String result = oMainCon.deleteUserProfileDetails(modItem.GetSetid);
            if (result.Equals("Y"))
            {
                sStatus = "Y";
                sMessage = "Hapus berjaya!";
            }
            else
            {
                sStatus = "N";
                sMessage = "Hapus tidak berjaya! Error on deleting table userprofile...";
            }
        }
        else
        {
            sStatus = "N";
            sMessage = "Hapus tidak berjaya! Record not found for table userprofile - Id: " + GetSetid;
        }

        object objData = new { status = sStatus, message = sMessage };

        jsonResponse = new JavaScriptSerializer().Serialize(objData);

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
        HttpContext.Current.Response.Write(jsonResponse);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public void getUserProfile(String GetSetidtype, String GetSetidno)
    {

        String jsonResponse = "";

        MainController oMainCon = new MainController();

        ArrayList lsUserMod = oMainCon.getUserProfile(GetSetidno, GetSetidtype);
        //jsonResponse = new JavaScriptSerializer().Serialize(oMod);

        object objData = new { status = "Y", listuserprofiles = lsUserMod };

        jsonResponse = new JavaScriptSerializer().Serialize(objData);

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
        HttpContext.Current.Response.Write(jsonResponse);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }
}
