using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MainController
/// </summary>
public class MainController
{
    private String sErrorLog = "";

    public MainController()
    {
        sErrorLog = "";
    }
    public MainController(String _sErrorLog)
    {
        sErrorLog = _sErrorLog;
    }    
    
    public ArrayList getUserProfile(String idno, String idtype)
    {
        ArrayList lsUsers = new ArrayList();
        DBConnect dbConnect = new DBConnect(sErrorLog);
        String query = "";
        String result = "0";

        try
        {
            if (dbConnect.OpenConnection() == true)
            {
                //if(idno.Length > 0 && idtype.Length > 0)
                {
                    query = @" select id, idno, idtype, name, status 
                               from   userprofile 
                               where  idno is not null ";
                    if (idno.Length > 0)
                    {
                        query = query + " and idno = '" + idno + "'";
                    }
                    if (idtype.Length > 0)
                    {
                        query = query + " and idtype = '" + idtype + "'";
                    }
                    //WriteToLogFile("MainController-getUserStatus: [SQL] " + query);
                    MySqlCommand cmd = new MySqlCommand(query, dbConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        MainModel oMod = new MainModel();
                        oMod.GetSetid = replaceNull(dataReader, "id");
                        oMod.GetSetidno = replaceNull(dataReader, "idno");
                        oMod.GetSetidtype = replaceNull(dataReader, "idtype");
                        oMod.GetSetname = replaceNull(dataReader, "name");
                        oMod.GetSetstatus = replaceNull(dataReader, "status");
                        lsUsers.Add(oMod);
                    }
                }
            }
        }
        catch (Exception e)
        {
            WriteToLogFile("MainController-getUserProfile: " + e.Message.ToString());
        }
        finally
        {
            if (dbConnect.connection.State.HasFlag(ConnectionState.Open))
            {
                dbConnect.CloseConnection();
            }
        }
        return lsUsers;
    }

    public MainModel getUserProfileDetails(String id)
    {
        MainModel oMod = new MainModel();
        DBConnect dbConnect = new DBConnect(sErrorLog);
        String query = "";

        try
        {
            if (dbConnect.OpenConnection() == true)
            {
                //if(idno.Length > 0 && idtype.Length > 0)
                {
                    query = @" select id, idno, idtype, name, status 
                               from   userprofile 
                               where  idno is not null ";
                    if (id.Length > 0)
                    {
                        query = query + " and id = " + id;
                    }
                    //WriteToLogFile("MainController-getUserStatus: [SQL] " + query);
                    MySqlCommand cmd = new MySqlCommand(query, dbConnect.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.Read())
                    {
                        oMod.GetSetid = replaceNull(dataReader, "id");
                        oMod.GetSetidno = replaceNull(dataReader, "idno");
                        oMod.GetSetidtype = replaceNull(dataReader, "idtype");
                        oMod.GetSetname = replaceNull(dataReader, "name");
                        oMod.GetSetstatus = replaceNull(dataReader, "status");
                    }
                }
            }
        }
        catch (Exception e)
        {
            WriteToLogFile("MainController-getUserProfileDetails: " + e.Message.ToString());
        }
        finally
        {
            if (dbConnect.connection.State.HasFlag(ConnectionState.Open))
            {
                dbConnect.CloseConnection();
            }
        }
        return oMod;
    }

    public String deleteUserProfileDetails(String id)
    {
        String result = "Y";
        DBConnect dbConnect = new DBConnect(sErrorLog);
        String query = "";

        try
        {
            if (dbConnect.OpenConnection() == true)
            {
                query = "";
                query = query + " DELETE FROM userprofile ";
                query = query + " WHERE  id = ?id ";
                MySqlCommand cmd = new MySqlCommand(query, dbConnect.connection);
                cmd.Parameters.Add("?id", MySqlDbType.Int64).Value = id;
                cmd.CommandTimeout = 60;
                if (cmd.ExecuteNonQuery() > 0)
                {
                    result = "Y";
                }
                else
                {
                    result = "N";
                }
            }
            dbConnect.CloseConnection();
        }
        catch (Exception e)
        {
            result = "N";
            WriteToLogFile("MainController-deleteUserProfileDetails: " + e.Message.ToString());
        }
        finally
        {
            if (dbConnect.connection.State.HasFlag(ConnectionState.Open))
            {
                dbConnect.CloseConnection();
            }
        }
        return result;
    }

    public String addUserProfileDetails(MainModel oMod)
    {
        String result = "Y";
        DBConnect dbConnect = new DBConnect(sErrorLog);
        String query = "";

        try
        {
            if (dbConnect.OpenConnection() == true)
            {
                query = "";
                query = query + " INSERT INTO userprofile (name, idno, idtype, status) ";
                query = query + " VALUES (?name, ?idno, ?idtype, ?status) ";
                MySqlCommand cmd = new MySqlCommand(query, dbConnect.connection);
                cmd.Parameters.Add("?name", MySqlDbType.VarChar).Value = oMod.GetSetname;
                cmd.Parameters.Add("?idno", MySqlDbType.VarChar).Value = oMod.GetSetidno;
                cmd.Parameters.Add("?idtype", MySqlDbType.VarChar).Value = oMod.GetSetidtype;
                cmd.Parameters.Add("?status", MySqlDbType.VarChar).Value = oMod.GetSetstatus;
                cmd.CommandTimeout = 60;
                if (cmd.ExecuteNonQuery() > 0)
                {
                    result = "Y";
                }
                else
                {
                    result = "N";
                }
            }
            dbConnect.CloseConnection();
        }
        catch (Exception e)
        {
            result = "N";
            WriteToLogFile("MainController-addUserProfileDetails: " + e.Message.ToString());
        }
        finally
        {
            if (dbConnect.connection.State.HasFlag(ConnectionState.Open))
            {
                dbConnect.CloseConnection();
            }
        }
        return result;
    }

    public String updateUserProfileDetails(MainModel oMod)
    {
        String result = "Y";
        DBConnect dbConnect = new DBConnect(sErrorLog);
        String query = "";

        try
        {
            if (dbConnect.OpenConnection() == true)
            {
                query = "";
                query = query + " UPDATE userprofile ";
                query = query + " SET name = ?name, ";
                query = query + "     idno = ?idno, ";
                query = query + "     idtype = ?idtype, ";
                query = query + "     status = ?status ";
                query = query + " WHERE  id = ?id ";
                MySqlCommand cmd = new MySqlCommand(query, dbConnect.connection);
                cmd.Parameters.Add("?name", MySqlDbType.VarChar).Value = oMod.GetSetname;
                cmd.Parameters.Add("?idno", MySqlDbType.VarChar).Value = oMod.GetSetidno;
                cmd.Parameters.Add("?idtype", MySqlDbType.VarChar).Value = oMod.GetSetidtype;
                cmd.Parameters.Add("?status", MySqlDbType.VarChar).Value = oMod.GetSetstatus;
                cmd.Parameters.Add("?id", MySqlDbType.Int64).Value = oMod.GetSetid;
                cmd.CommandTimeout = 60;
                if (cmd.ExecuteNonQuery() > 0)
                {
                    result = "Y";
                }
                else
                {
                    result = "N";
                }
            }
            dbConnect.CloseConnection();
        }
        catch (Exception e)
        {
            result = "N";
            WriteToLogFile("MainController-updateUserProfileDetails: " + e.Message.ToString());
        }
        finally
        {
            if (dbConnect.connection.State.HasFlag(ConnectionState.Open))
            {
                dbConnect.CloseConnection();
            }
        }
        return result;
    }

    public String replaceNull(MySqlDataReader oDataReader, String sField)
    {
        if (oDataReader[sField] == DBNull.Value)
        {
            return "";
        }
        else
        {
            if (oDataReader[sField].ToString().Trim().ToUpper().Equals("NULL"))
            {
                return "";
            }
            else
            {
                return oDataReader[sField].ToString();
            }
        }
    }

    public String replaceNull(MySqlDataReader oDataReader, int iField)
    {
        if (oDataReader.IsDBNull(iField))
        {
            return "";
        }
        else
        {
            if (oDataReader[iField].ToString().Trim().ToUpper().Equals("NULL"))
            {
                return "";
            }
            else
            {
                return oDataReader[iField].ToString();
            }
        }
    }

    public int replaceZero(MySqlDataReader oDataReader, String sField)
    {
        if (oDataReader[sField] == DBNull.Value)
            return 0;
        else
            return int.Parse(oDataReader[sField].ToString());
    }
    public int replaceZero(MySqlDataReader oDataReader, int iField)
    {
        if (oDataReader.IsDBNull(iField))
            return 0;
        else
            return int.Parse(oDataReader[iField].ToString());
    }

    public double replaceDoubleZero(MySqlDataReader oDataReader, String sField)
    {
        if (oDataReader[sField] == DBNull.Value)
            return 0;
        else
            return double.Parse(oDataReader[sField].ToString());
    }
    public double replaceDoubleZero(MySqlDataReader oDataReader, int iField)
    {
        if (oDataReader.IsDBNull(iField))
            return 0;
        else
            return double.Parse(oDataReader[iField].ToString());
    }
    public decimal replaceDecimalZero(MySqlDataReader oDataReader, String sField)
    {
        if (oDataReader[sField] == DBNull.Value)
            return 0;
        else
            return decimal.Parse(oDataReader[sField].ToString());
    }
    public decimal replaceDecimalZero(MySqlDataReader oDataReader, int iField)
    {
        if (oDataReader.IsDBNull(iField))
            return 0;
        else
            return decimal.Parse(oDataReader[iField].ToString());
    }

    public void WriteToLogFile(string strMessage)
    {
        //Open a file for writing
        //Get a StreamWriter class that can be used to write to the file
        if (strMessage.Length > 0)
        {
            string strlogFile = ConfigurationSettings.AppSettings["LogFile"];
            strlogFile = MyHttpApplication.GetAppDataPath(strlogFile);
            //string strlogFile = sErrorLog;
            if (strlogFile.Trim().Length > 0)
            {
                System.IO.StreamWriter objStreamWriter;
                objStreamWriter = System.IO.File.AppendText(strlogFile);

                objStreamWriter.WriteLine(DateTime.Now.ToString() + ": " + strMessage);

                //Close the stream
                objStreamWriter.Close();
            }
        }
    }
    public void Close()
    {
        sErrorLog = "";
    }

    class MyHttpApplication : HttpApplication
    {
        // of course you can fetch&store the value at Application_Start
        public static string GetAppDataPath(String sLogFile)
        {
            try
            {
                return HttpContext.Current.Server.MapPath(sLogFile);
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}