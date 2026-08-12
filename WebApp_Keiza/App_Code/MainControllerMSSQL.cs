using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Main Controller for MS SQL Server Operations
/// Handles CRUD operations for asnaf_profile and asnaf_application tables
/// </summary>
public class MainControllerMSSQL
{
    private string sErrorLog = "";

    public MainControllerMSSQL()
    {
        sErrorLog = System.AppDomain.CurrentDomain.BaseDirectory + "LogFile\\";
        if (!System.IO.Directory.Exists(sErrorLog))
        {
            System.IO.Directory.CreateDirectory(sErrorLog);
        }
    }

    public MainControllerMSSQL(String _sErrorLog)
    {
        sErrorLog = _sErrorLog;
    }

    #region Asnaf Profile CRUD Operations

    /// <summary>
    /// Get all Asnaf Profiles
    /// </summary>
    public ArrayList getAsnafProfiles()
    {
        ArrayList lsAsnafProfiles = new ArrayList();
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            query = @" SELECT asnaf_uid, asnaf_name, asnaf_icno, res_address1, res_address2, res_address3,
                             postcode, district, state, email, contactno, asnaf_status,
                             createdby, createddate, modifiedby, modifieddate, confirmedby, confirmeddate
                      FROM asnaf_profile
                      ORDER BY asnaf_uid DESC ";

            SqlDataReader dataReader = dbConnect.ExecuteQuery(query);

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    MainModelMSSQL oAsnaf = new MainModelMSSQL();
                    mapAsnafProfileFields(oAsnaf, dataReader, dbConnect);
                    lsAsnafProfiles.Add(oAsnaf);
                }
                dataReader.Close();
            }
        }
        catch (Exception ex)
        {
            WriteToLogFile("MainControllerMSSQL-getAsnafProfiles: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return lsAsnafProfiles;
    }

    /// <summary>
    /// Get Asnaf Profile by ID
    /// </summary>
    public MainModelMSSQL getAsnafProfileById(string asnafUid)
    {
        MainModelMSSQL oAsnaf = new MainModelMSSQL();
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(asnafUid))
            {
                query = @" SELECT asnaf_uid, asnaf_name, asnaf_icno, res_address1, res_address2, res_address3,
                                 postcode, district, state, email, contactno, asnaf_status,
                                 createdby, createddate, modifiedby, modifieddate, confirmedby, confirmeddate
                          FROM asnaf_profile
                          WHERE asnaf_uid = " + asnafUid;

                SqlDataReader dataReader = dbConnect.ExecuteQuery(query);

                if (dataReader != null && dataReader.Read())
                {
                    mapAsnafProfileFields(oAsnaf, dataReader, dbConnect);
                    dataReader.Close();
                }
            }
        }
        catch (Exception ex)
        {
            WriteToLogFile("MainControllerMSSQL-getAsnafProfileById: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return oAsnaf;
    }

    /// <summary>
    /// Search Asnaf Profile by Name or IC Number
    /// </summary>
    public ArrayList searchAsnafProfile(string searchText)
    {
        ArrayList lsAsnafProfiles = new ArrayList();
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = @" SELECT asnaf_uid, asnaf_name, asnaf_icno, res_address1, res_address2, res_address3,
                                 postcode, district, state, email, contactno, asnaf_status,
                                 createdby, createddate, modifiedby, modifieddate, confirmedby, confirmeddate
                          FROM asnaf_profile
                          WHERE asnaf_name LIKE '%" + searchText.Replace("'", "''") + @"%'
                             OR asnaf_icno LIKE '%" + searchText.Replace("'", "''") + @"%'
                          ORDER BY asnaf_name ASC ";

                SqlDataReader dataReader = dbConnect.ExecuteQuery(query);

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        MainModelMSSQL oAsnaf = new MainModelMSSQL();
                        mapAsnafProfileFields(oAsnaf, dataReader, dbConnect);
                        lsAsnafProfiles.Add(oAsnaf);
                    }
                    dataReader.Close();
                }
            }
        }
        catch (Exception ex)
        {
            WriteToLogFile("MainControllerMSSQL-searchAsnafProfile: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return lsAsnafProfiles;
    }

    /// <summary>
    /// Add New Asnaf Profile
    /// </summary>
    public string addAsnafProfile(MainModelMSSQL oAsnaf)
    {
        string result = "N";
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (oAsnaf != null && !string.IsNullOrWhiteSpace(oAsnaf.GetSetasnafName))
            {
                query = @" INSERT INTO asnaf_profile 
                          (asnaf_name, asnaf_icno, res_address1, res_address2, res_address3,
                           postcode, district, state, email, contactno, asnaf_status,
                           createdby, createddate)
                          VALUES (
                           '" + oAsnaf.GetSetasnafName.Replace("'", "''") + @"',
                           '" + oAsnaf.GetSetasnafIcno.Replace("'", "''") + @"',
                           '" + oAsnaf.GetSetresAddress1.Replace("'", "''") + @"',
                           '" + oAsnaf.GetSetresAddress2.Replace("'", "''") + @"',
                           '" + oAsnaf.GetSetresAddress3.Replace("'", "''") + @"',
                           '" + oAsnaf.GetSetpostcode.Replace("'", "''") + @"',
                           '" + oAsnaf.GetSetdistrict.Replace("'", "''") + @"',
                           '" + oAsnaf.GetSetstate.Replace("'", "''") + @"',
                           '" + oAsnaf.GetSetemail.Replace("'", "''") + @"',
                           '" + oAsnaf.GetSetcontactno.Replace("'", "''") + @"',
                           '" + oAsnaf.GetSetasnafStatus.Replace("'", "''") + @"',
                           '" + oAsnaf.GetSetcreatedBy.Replace("'", "''") + @"',
                           GETDATE()
                          )";

                bool bResult = dbConnect.Insert(query);
                result = bResult ? "Y" : "N";

                WriteToLogFile("MainControllerMSSQL-addAsnafProfile: Asnaf added successfully - " + oAsnaf.GetSetasnafName);
            }
            else
            {
                result = "N";
                WriteToLogFile("MainControllerMSSQL-addAsnafProfile: Invalid input - Name is required");
            }
        }
        catch (Exception ex)
        {
            result = "N";
            WriteToLogFile("MainControllerMSSQL-addAsnafProfile: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return result;
    }

    /// <summary>
    /// Update Asnaf Profile
    /// </summary>
    public string updateAsnafProfile(MainModelMSSQL oAsnaf)
    {
        string result = "N";
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (oAsnaf != null && !string.IsNullOrWhiteSpace(oAsnaf.GetSetasnafUid))
            {
                query = @" UPDATE asnaf_profile
                          SET asnaf_name = '" + oAsnaf.GetSetasnafName.Replace("'", "''") + @"',
                              asnaf_icno = '" + oAsnaf.GetSetasnafIcno.Replace("'", "''") + @"',
                              res_address1 = '" + oAsnaf.GetSetresAddress1.Replace("'", "''") + @"',
                              res_address2 = '" + oAsnaf.GetSetresAddress2.Replace("'", "''") + @"',
                              res_address3 = '" + oAsnaf.GetSetresAddress3.Replace("'", "''") + @"',
                              postcode = '" + oAsnaf.GetSetpostcode.Replace("'", "''") + @"',
                              district = '" + oAsnaf.GetSetdistrict.Replace("'", "''") + @"',
                              state = '" + oAsnaf.GetSetstate.Replace("'", "''") + @"',
                              email = '" + oAsnaf.GetSetemail.Replace("'", "''") + @"',
                              contactno = '" + oAsnaf.GetSetcontactno.Replace("'", "''") + @"',
                              asnaf_status = '" + oAsnaf.GetSetasnafStatus.Replace("'", "''") + @"',
                              modifiedby = '" + oAsnaf.GetSetmodifiedBy.Replace("'", "''") + @"',
                              modifieddate = GETDATE()
                          WHERE asnaf_uid = " + oAsnaf.GetSetasnafUid;

                bool bResult = dbConnect.Update(query);
                result = bResult ? "Y" : "N";

                WriteToLogFile("MainControllerMSSQL-updateAsnafProfile: Asnaf updated successfully - UID: " + oAsnaf.GetSetasnafUid);
            }
            else
            {
                result = "N";
                WriteToLogFile("MainControllerMSSQL-updateAsnafProfile: Invalid input - UID is required");
            }
        }
        catch (Exception ex)
        {
            result = "N";
            WriteToLogFile("MainControllerMSSQL-updateAsnafProfile: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return result;
    }

    /// <summary>
    /// Delete Asnaf Profile
    /// </summary>
    public string deleteAsnafProfile(string asnafUid)
    {
        string result = "N";
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(asnafUid))
            {
                // Check if asnaf has applications
                query = "SELECT COUNT(*) FROM asnaf_application WHERE asnaf_uid = " + asnafUid;
                object oCount = dbConnect.ExecuteScalar(query);
                int count = 0;
                //int.TryParse(oCount?.ToString() ?? "0", out count);
                if (oCount != null)
                {
                    int.TryParse(oCount.ToString(), out count);
                }
                else
                {
                    count = 0;
                }

                if (count > 0)
                {
                    result = "N";
                    WriteToLogFile("MainControllerMSSQL-deleteAsnafProfile: Cannot delete - Asnaf has " + count + " applications");
                }
                else
                {
                    query = "DELETE FROM asnaf_profile WHERE asnaf_uid = " + asnafUid;
                    bool bResult = dbConnect.Delete(query);
                    result = bResult ? "Y" : "N";

                    WriteToLogFile("MainControllerMSSQL-deleteAsnafProfile: Asnaf deleted successfully - UID: " + asnafUid);
                }
            }
            else
            {
                result = "N";
                WriteToLogFile("MainControllerMSSQL-deleteAsnafProfile: Invalid input - UID is required");
            }
        }
        catch (Exception ex)
        {
            result = "N";
            WriteToLogFile("MainControllerMSSQL-deleteAsnafProfile: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return result;
    }

    /// <summary>
    /// Confirm Asnaf Profile
    /// </summary>
    public string confirmAsnafProfile(string asnafUid, string confirmedBy)
    {
        string result = "N";
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(asnafUid) && !string.IsNullOrWhiteSpace(confirmedBy))
            {
                query = @" UPDATE asnaf_profile
                          SET asnaf_status = 'CONFIRMED',
                              confirmedby = '" + confirmedBy.Replace("'", "''") + @"',
                              confirmeddate = GETDATE()
                          WHERE asnaf_uid = " + asnafUid;

                bool bResult = dbConnect.Update(query);
                result = bResult ? "Y" : "N";

                WriteToLogFile("MainControllerMSSQL-confirmAsnafProfile: Asnaf confirmed - UID: " + asnafUid + " By: " + confirmedBy);
            }
        }
        catch (Exception ex)
        {
            result = "N";
            WriteToLogFile("MainControllerMSSQL-confirmAsnafProfile: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return result;
    }

    #endregion

    #region Asnaf Application CRUD Operations

    /// <summary>
    /// Get all Asnaf Applications
    /// </summary>
    public ArrayList getAsnafApplications()
    {
        ArrayList lsApplications = new ArrayList();
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            query = @" SELECT app_uid, asnaf_uid, application_no, application_date, submitted_date,
                             application_status, amount, currency, remarks, submitted_by, reviewed_by,
                             reviewed_date, confirmeddate, createdby, createddate, modifiedby, modifieddate
                      FROM asnaf_application
                      ORDER BY app_uid DESC ";

            SqlDataReader dataReader = dbConnect.ExecuteQuery(query);

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    MainModelMSSQL oApp = new MainModelMSSQL();
                    mapAsnafApplicationFields(oApp, dataReader, dbConnect);
                    lsApplications.Add(oApp);
                }
                dataReader.Close();
            }
        }
        catch (Exception ex)
        {
            WriteToLogFile("MainControllerMSSQL-getAsnafApplications: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return lsApplications;
    }

    /// <summary>
    /// Get Asnaf Application by ID
    /// </summary>
    public MainModelMSSQL getAsnafApplicationById(string appUid)
    {
        MainModelMSSQL oApp = new MainModelMSSQL();
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(appUid))
            {
                query = @" SELECT app_uid, asnaf_uid, application_no, application_date, submitted_date,
                                 application_status, amount, currency, remarks, submitted_by, reviewed_by,
                                 reviewed_date, confirmeddate, createdby, createddate, modifiedby, modifieddate
                          FROM asnaf_application
                          WHERE app_uid = " + appUid;

                SqlDataReader dataReader = dbConnect.ExecuteQuery(query);

                if (dataReader != null && dataReader.Read())
                {
                    mapAsnafApplicationFields(oApp, dataReader, dbConnect);
                    dataReader.Close();
                }
            }
        }
        catch (Exception ex)
        {
            WriteToLogFile("MainControllerMSSQL-getAsnafApplicationById: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return oApp;
    }

    /// <summary>
    /// Get Applications by Asnaf UID
    /// </summary>
    public ArrayList getAsnafApplicationsByAsnafUid(string asnafUid)
    {
        ArrayList lsApplications = new ArrayList();
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(asnafUid))
            {
                query = @" SELECT app_uid, asnaf_uid, application_no, application_date, submitted_date,
                                 application_status, amount, currency, remarks, submitted_by, reviewed_by,
                                 reviewed_date, confirmeddate, createdby, createddate, modifiedby, modifieddate
                          FROM asnaf_application
                          WHERE asnaf_uid = " + asnafUid + @"
                          ORDER BY application_date DESC ";

                SqlDataReader dataReader = dbConnect.ExecuteQuery(query);

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        MainModelMSSQL oApp = new MainModelMSSQL();
                        mapAsnafApplicationFields(oApp, dataReader, dbConnect);
                        lsApplications.Add(oApp);
                    }
                    dataReader.Close();
                }
            }
        }
        catch (Exception ex)
        {
            WriteToLogFile("MainControllerMSSQL-getAsnafApplicationsByAsnafUid: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return lsApplications;
    }

    /// <summary>
    /// Add New Asnaf Application
    /// </summary>
    public string addAsnafApplication(MainModelMSSQL oApp)
    {
        string result = "N";
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (oApp != null && !string.IsNullOrWhiteSpace(oApp.GetSetasnafUid))
            {
                query = @" INSERT INTO asnaf_application
                          (asnaf_uid, application_no, application_date, application_status, amount, currency,
                           remarks, createdby, createddate)
                          VALUES (
                           " + oApp.GetSetasnafUid + @",
                           '" + oApp.GetSetapplicationNo.Replace("'", "''") + @"',
                           GETDATE(),
                           '" + (string.IsNullOrWhiteSpace(oApp.GetSetapplicationStatus) ? "DRAFT" : oApp.GetSetapplicationStatus).Replace("'", "''") + @"',
                           " + (string.IsNullOrWhiteSpace(oApp.GetSetamount) ? "0" : oApp.GetSetamount) + @",
                           '" + oApp.GetSetcurrency.Replace("'", "''") + @"',
                           '" + oApp.GetSetremarks.Replace("'", "''") + @"',
                           '" + oApp.GetSetapplicationCreatedBy.Replace("'", "''") + @"',
                           GETDATE()
                          )";

                bool bResult = dbConnect.Insert(query);
                result = bResult ? "Y" : "N";

                WriteToLogFile("MainControllerMSSQL-addAsnafApplication: Application added successfully - Asnaf UID: " + oApp.GetSetasnafUid);
            }
            else
            {
                result = "N";
                WriteToLogFile("MainControllerMSSQL-addAsnafApplication: Invalid input - Asnaf UID is required");
            }
        }
        catch (Exception ex)
        {
            result = "N";
            WriteToLogFile("MainControllerMSSQL-addAsnafApplication: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return result;
    }

    /// <summary>
    /// Update Asnaf Application
    /// </summary>
    public string updateAsnafApplication(MainModelMSSQL oApp)
    {
        string result = "N";
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (oApp != null && !string.IsNullOrWhiteSpace(oApp.GetSetappUid))
            {
                query = @" UPDATE asnaf_application
                          SET application_no = '" + oApp.GetSetapplicationNo.Replace("'", "''") + @"',
                              application_status = '" + oApp.GetSetapplicationStatus.Replace("'", "''") + @"',
                              amount = " + (string.IsNullOrWhiteSpace(oApp.GetSetamount) ? "0" : oApp.GetSetamount) + @",
                              currency = '" + oApp.GetSetcurrency.Replace("'", "''") + @"',
                              remarks = '" + oApp.GetSetremarks.Replace("'", "''") + @"',
                              modifiedby = '" + oApp.GetSetapplicationModifiedBy.Replace("'", "''") + @"',
                              modifieddate = GETDATE()
                          WHERE app_uid = " + oApp.GetSetappUid;

                bool bResult = dbConnect.Update(query);
                result = bResult ? "Y" : "N";

                WriteToLogFile("MainControllerMSSQL-updateAsnafApplication: Application updated - App UID: " + oApp.GetSetappUid);
            }
            else
            {
                result = "N";
                WriteToLogFile("MainControllerMSSQL-updateAsnafApplication: Invalid input - App UID is required");
            }
        }
        catch (Exception ex)
        {
            result = "N";
            WriteToLogFile("MainControllerMSSQL-updateAsnafApplication: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return result;
    }

    /// <summary>
    /// Submit Asnaf Application
    /// </summary>
    public string submitAsnafApplication(string appUid, string submittedBy)
    {
        string result = "N";
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(appUid) && !string.IsNullOrWhiteSpace(submittedBy))
            {
                query = @" UPDATE asnaf_application
                          SET application_status = 'SUBMITTED',
                              submitted_date = GETDATE(),
                              submitted_by = '" + submittedBy.Replace("'", "''") + @"'
                          WHERE app_uid = " + appUid;

                bool bResult = dbConnect.Update(query);
                result = bResult ? "Y" : "N";

                WriteToLogFile("MainControllerMSSQL-submitAsnafApplication: Application submitted - App UID: " + appUid);
            }
        }
        catch (Exception ex)
        {
            result = "N";
            WriteToLogFile("MainControllerMSSQL-submitAsnafApplication: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return result;
    }

    /// <summary>
    /// Review Asnaf Application
    /// </summary>
    public string reviewAsnafApplication(string appUid, string reviewedBy, string applicationStatus)
    {
        string result = "N";
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(appUid) && !string.IsNullOrWhiteSpace(reviewedBy))
            {
                query = @" UPDATE asnaf_application
                          SET application_status = '" + applicationStatus.Replace("'", "''") + @"',
                              reviewed_date = GETDATE(),
                              reviewed_by = '" + reviewedBy.Replace("'", "''") + @"'
                          WHERE app_uid = " + appUid;

                bool bResult = dbConnect.Update(query);
                result = bResult ? "Y" : "N";

                WriteToLogFile("MainControllerMSSQL-reviewAsnafApplication: Application reviewed - App UID: " + appUid + " Status: " + applicationStatus);
            }
        }
        catch (Exception ex)
        {
            result = "N";
            WriteToLogFile("MainControllerMSSQL-reviewAsnafApplication: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return result;
    }

    /// <summary>
    /// Delete Asnaf Application
    /// </summary>
    public string deleteAsnafApplication(string appUid)
    {
        string result = "N";
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(appUid))
            {
                query = "DELETE FROM asnaf_application WHERE app_uid = " + appUid;
                bool bResult = dbConnect.Delete(query);
                result = bResult ? "Y" : "N";

                WriteToLogFile("MainControllerMSSQL-deleteAsnafApplication: Application deleted - App UID: " + appUid);
            }
            else
            {
                result = "N";
                WriteToLogFile("MainControllerMSSQL-deleteAsnafApplication: Invalid input - App UID is required");
            }
        }
        catch (Exception ex)
        {
            result = "N";
            WriteToLogFile("MainControllerMSSQL-deleteAsnafApplication: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return result;
    }

    /// <summary>
    /// Search Asnaf Applications by Asnaf UID (Foreign Key)
    /// </summary>
    public ArrayList searchAsnafApplicationsByAsnafUid(string asnafUid, string searchText)
    {
        ArrayList lsApplications = new ArrayList();
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(asnafUid))
            {
                query = @" SELECT app_uid, asnaf_uid, application_no, application_date, submitted_date,
                                 application_status, amount, currency, remarks, submitted_by, reviewed_by,
                                 reviewed_date, confirmeddate, createdby, createddate, modifiedby, modifieddate
                          FROM asnaf_application
                          WHERE asnaf_uid = " + asnafUid;

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query += @" AND (application_no LIKE '%" + searchText.Replace("'", "''") + @"%'
                                     OR remarks LIKE '%" + searchText.Replace("'", "''") + @"%')";
                }

                query += @" ORDER BY application_date DESC ";

                SqlDataReader dataReader = dbConnect.ExecuteQuery(query);

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        MainModelMSSQL oApp = new MainModelMSSQL();
                        mapAsnafApplicationFields(oApp, dataReader, dbConnect);
                        lsApplications.Add(oApp);
                    }
                    dataReader.Close();
                }
            }
        }
        catch (Exception ex)
        {
            WriteToLogFile("MainControllerMSSQL-searchAsnafApplicationsByAsnafUid: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return lsApplications;
    }

    /// <summary>
    /// Update Asnaf Application by Asnaf UID (Foreign Key)
    /// </summary>
    public string updateAsnafApplicationByAsnafUid(string asnafUid, MainModelMSSQL oApp)
    {
        string result = "N";
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(asnafUid) && oApp != null && !string.IsNullOrWhiteSpace(oApp.GetSetappUid))
            {
                query = @" UPDATE asnaf_application
                          SET application_no = '" + oApp.GetSetapplicationNo.Replace("'", "''") + @"',
                              application_status = '" + oApp.GetSetapplicationStatus.Replace("'", "''") + @"',
                              amount = " + (string.IsNullOrWhiteSpace(oApp.GetSetamount) ? "0" : oApp.GetSetamount) + @",
                              currency = '" + oApp.GetSetcurrency.Replace("'", "''") + @"',
                              remarks = '" + oApp.GetSetremarks.Replace("'", "''") + @"',
                              modifiedby = '" + oApp.GetSetapplicationModifiedBy.Replace("'", "''") + @"',
                              modifieddate = GETDATE()
                          WHERE app_uid = " + oApp.GetSetappUid + @"
                            AND asnaf_uid = " + asnafUid;

                bool bResult = dbConnect.Update(query);
                result = bResult ? "Y" : "N";

                WriteToLogFile("MainControllerMSSQL-updateAsnafApplicationByAsnafUid: Application updated - Asnaf UID: " + asnafUid + " App UID: " + oApp.GetSetappUid);
            }
            else
            {
                result = "N";
                WriteToLogFile("MainControllerMSSQL-updateAsnafApplicationByAsnafUid: Invalid input - Asnaf UID and App UID are required");
            }
        }
        catch (Exception ex)
        {
            result = "N";
            WriteToLogFile("MainControllerMSSQL-updateAsnafApplicationByAsnafUid: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return result;
    }

    /// <summary>
    /// Delete Asnaf Application by Asnaf UID (Foreign Key)
    /// </summary>
    public string deleteAsnafApplicationByAsnafUid(string asnafUid, string appUid)
    {
        string result = "N";
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(asnafUid) && !string.IsNullOrWhiteSpace(appUid))
            {
                query = @" DELETE FROM asnaf_application
                          WHERE app_uid = " + appUid + @"
                            AND asnaf_uid = " + asnafUid;

                bool bResult = dbConnect.Delete(query);
                result = bResult ? "Y" : "N";

                WriteToLogFile("MainControllerMSSQL-deleteAsnafApplicationByAsnafUid: Application deleted - Asnaf UID: " + asnafUid + " App UID: " + appUid);
            }
            else
            {
                result = "N";
                WriteToLogFile("MainControllerMSSQL-deleteAsnafApplicationByAsnafUid: Invalid input - Asnaf UID and App UID are required");
            }
        }
        catch (Exception ex)
        {
            result = "N";
            WriteToLogFile("MainControllerMSSQL-deleteAsnafApplicationByAsnafUid: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return result;
    }

    /// <summary>
    /// Delete All Applications by Asnaf UID (Foreign Key) - Cascade Delete
    /// </summary>
    public string deleteAllAsnafApplicationsByAsnafUid(string asnafUid)
    {
        string result = "N";
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);
        String query = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(asnafUid))
            {
                query = "DELETE FROM asnaf_application WHERE asnaf_uid = " + asnafUid;
                bool bResult = dbConnect.Delete(query);
                result = bResult ? "Y" : "N";

                WriteToLogFile("MainControllerMSSQL-deleteAllAsnafApplicationsByAsnafUid: All applications deleted - Asnaf UID: " + asnafUid);
            }
            else
            {
                result = "N";
                WriteToLogFile("MainControllerMSSQL-deleteAllAsnafApplicationsByAsnafUid: Invalid input - Asnaf UID is required");
            }
        }
        catch (Exception ex)
        {
            result = "N";
            WriteToLogFile("MainControllerMSSQL-deleteAllAsnafApplicationsByAsnafUid: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return result;
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Map SqlDataReader fields to Asnaf Profile properties
    /// </summary>
    private void mapAsnafProfileFields(MainModelMSSQL oAsnaf, SqlDataReader reader, DBConnectMSSQL dbConnect)
    {
        try
        {
            oAsnaf.GetSetasnafUid = dbConnect.ReplaceNull(reader, "asnaf_uid");
            oAsnaf.GetSetasnafName = dbConnect.ReplaceNull(reader, "asnaf_name");
            oAsnaf.GetSetasnafIcno = dbConnect.ReplaceNull(reader, "asnaf_icno");
            oAsnaf.GetSetresAddress1 = dbConnect.ReplaceNull(reader, "res_address1");
            oAsnaf.GetSetresAddress2 = dbConnect.ReplaceNull(reader, "res_address2");
            oAsnaf.GetSetresAddress3 = dbConnect.ReplaceNull(reader, "res_address3");
            oAsnaf.GetSetpostcode = dbConnect.ReplaceNull(reader, "postcode");
            oAsnaf.GetSetdistrict = dbConnect.ReplaceNull(reader, "district");
            oAsnaf.GetSetstate = dbConnect.ReplaceNull(reader, "state");
            oAsnaf.GetSetemail = dbConnect.ReplaceNull(reader, "email");
            oAsnaf.GetSetcontactno = dbConnect.ReplaceNull(reader, "contactno");
            oAsnaf.GetSetasnafStatus = dbConnect.ReplaceNull(reader, "asnaf_status");
            oAsnaf.GetSetcreatedBy = dbConnect.ReplaceNull(reader, "createdby");
            oAsnaf.GetSetcreatedDate = dbConnect.ReplaceNull(reader, "createddate");
            oAsnaf.GetSetmodifiedBy = dbConnect.ReplaceNull(reader, "modifiedby");
            oAsnaf.GetSetmodifiedDate = dbConnect.ReplaceNull(reader, "modifieddate");
            oAsnaf.GetSetconfirmedBy = dbConnect.ReplaceNull(reader, "confirmedby");
            oAsnaf.GetSetconfirmedDate = dbConnect.ReplaceNull(reader, "confirmeddate");
        }
        catch (Exception ex)
        {
            WriteToLogFile("MainControllerMSSQL-mapAsnafProfileFields: " + ex.Message);
        }
    }

    /// <summary>
    /// Map SqlDataReader fields to Asnaf Application properties
    /// </summary>
    private void mapAsnafApplicationFields(MainModelMSSQL oApp, SqlDataReader reader, DBConnectMSSQL dbConnect)
    {
        try
        {
            oApp.GetSetappUid = dbConnect.ReplaceNull(reader, "app_uid");
            oApp.GetSetasnafUid = dbConnect.ReplaceNull(reader, "asnaf_uid");
            oApp.GetSetapplicationNo = dbConnect.ReplaceNull(reader, "application_no");
            oApp.GetSetapplicationDate = dbConnect.ReplaceNull(reader, "application_date");
            oApp.GetSetsubmittedDate = dbConnect.ReplaceNull(reader, "submitted_date");
            oApp.GetSetapplicationStatus = dbConnect.ReplaceNull(reader, "application_status");
            oApp.GetSetamount = dbConnect.ReplaceNull(reader, "amount");
            oApp.GetSetcurrency = dbConnect.ReplaceNull(reader, "currency");
            oApp.GetSetremarks = dbConnect.ReplaceNull(reader, "remarks");
            oApp.GetSetsubmittedBy = dbConnect.ReplaceNull(reader, "submitted_by");
            oApp.GetSetreviewedBy = dbConnect.ReplaceNull(reader, "reviewed_by");
            oApp.GetSetreviewedDate = dbConnect.ReplaceNull(reader, "reviewed_date");
            oApp.GetSetapplicationConfirmedDate = dbConnect.ReplaceNull(reader, "confirmeddate");
            oApp.GetSetapplicationCreatedBy = dbConnect.ReplaceNull(reader, "createdby");
            oApp.GetSetapplicationCreatedDate = dbConnect.ReplaceNull(reader, "createddate");
            oApp.GetSetapplicationModifiedBy = dbConnect.ReplaceNull(reader, "modifiedby");
            oApp.GetSetapplicationModifiedDate = dbConnect.ReplaceNull(reader, "modifieddate");
        }
        catch (Exception ex)
        {
            WriteToLogFile("MainControllerMSSQL-mapAsnafApplicationFields: " + ex.Message);
        }
    }

    /// <summary>
    /// Write Error to Log File
    /// </summary>
    public void WriteToLogFile(String message)
    {
        try
        {
            string sLogPath = sErrorLog + "ErrorLog_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            using (System.IO.StreamWriter sw = System.IO.File.AppendText(sLogPath))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | " + message);
                sw.Close();
            }
        }
        catch { }
    }

    /// <summary>
    /// Safely gets a string value from a SqlDataReader by column name
    /// </summary>
    private string SafeGetString(SqlDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        if (!reader.IsDBNull(ordinal))
            return reader.GetValue(ordinal).ToString();
        return string.Empty;
    }

    #endregion

    #region Dashboard Scoreboard Operations

    /// <summary>
    /// Get comprehensive dashboard scoreboard statistics
    /// Returns all key metrics in a single call
    /// </summary>
    public Hashtable getDashboardScoreboard()
    {
        Hashtable dashboardData = new Hashtable();
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);

        try
        {
            // Total Applications
            string query = "SELECT COUNT(*) FROM asnaf_application";
            object totalApps = dbConnect.ExecuteScalar(query);
            dashboardData["totalApplications"] = totalApps != null ? Convert.ToInt32(totalApps) : 0;

            // Pending Applications
            query = "SELECT COUNT(*) FROM asnaf_application WHERE application_status = 'PENDING'";
            object pendingApps = dbConnect.ExecuteScalar(query);
            dashboardData["pendingApplications"] = pendingApps != null ? Convert.ToInt32(pendingApps) : 0;

            // Approved Applications
            query = "SELECT COUNT(*) FROM asnaf_application WHERE application_status = 'APPROVED'";
            object approvedApps = dbConnect.ExecuteScalar(query);
            dashboardData["approvedApplications"] = approvedApps != null ? Convert.ToInt32(approvedApps) : 0;

            // Rejected Applications
            query = "SELECT COUNT(*) FROM asnaf_application WHERE application_status = 'REJECTED'";
            object rejectedApps = dbConnect.ExecuteScalar(query);
            dashboardData["rejectedApplications"] = rejectedApps != null ? Convert.ToInt32(rejectedApps) : 0;

            // Total Asnaf
            query = "SELECT COUNT(*) FROM asnaf_profile";
            object totalAsnaf = dbConnect.ExecuteScalar(query);
            dashboardData["totalAsnaf"] = totalAsnaf != null ? Convert.ToInt32(totalAsnaf) : 0;

            // Active Asnaf
            query = "SELECT COUNT(*) FROM asnaf_profile WHERE asnaf_status = 'ACTIVE'";
            object activeAsnaf = dbConnect.ExecuteScalar(query);
            dashboardData["activeAsnaf"] = activeAsnaf != null ? Convert.ToInt32(activeAsnaf) : 0;

            // Today's Applications
            query = @"SELECT COUNT(*) FROM asnaf_application 
                     WHERE CONVERT(date, application_date) = CONVERT(date, GETDATE())";
            object todayApps = dbConnect.ExecuteScalar(query);
            dashboardData["todayApplications"] = todayApps != null ? Convert.ToInt32(todayApps) : 0;

            // This Week's Applications
            query = @"SELECT COUNT(*) FROM asnaf_application 
                     WHERE application_date >= DATEADD(day, -7, GETDATE())";
            object weekApps = dbConnect.ExecuteScalar(query);
            dashboardData["weekApplications"] = weekApps != null ? Convert.ToInt32(weekApps) : 0;

            // This Month's Applications
            query = @"SELECT COUNT(*) FROM asnaf_application 
                     WHERE MONTH(application_date) = MONTH(GETDATE()) 
                     AND YEAR(application_date) = YEAR(GETDATE())";
            object monthApps = dbConnect.ExecuteScalar(query);
            dashboardData["monthApplications"] = monthApps != null ? Convert.ToInt32(monthApps) : 0;

            // Total Amount (All Applications)
            query = @"SELECT ISNULL(SUM(CAST(amount AS DECIMAL(18,2))), 0) 
                     FROM asnaf_application 
                     WHERE amount IS NOT NULL AND amount <> ''";
            object totalAmount = dbConnect.ExecuteScalar(query);
            dashboardData["totalAmount"] = totalAmount != null ? Convert.ToDecimal(totalAmount) : 0;

            // Approved Amount
            query = @"SELECT ISNULL(SUM(CAST(amount AS DECIMAL(18,2))), 0) 
                     FROM asnaf_application 
                     WHERE application_status = 'APPROVED' 
                     AND amount IS NOT NULL AND amount <> ''";
            object approvedAmount = dbConnect.ExecuteScalar(query);
            dashboardData["approvedAmount"] = approvedAmount != null ? Convert.ToDecimal(approvedAmount) : 0;

            WriteToLogFile("MainControllerMSSQL-getDashboardScoreboard: Dashboard data retrieved successfully");
        }
        catch (Exception ex)
        {
            WriteToLogFile("MainControllerMSSQL-getDashboardScoreboard: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return dashboardData;
    }

    /// <summary>
    /// Get application statistics grouped by status
    /// </summary>
    public ArrayList getApplicationStatsByStatus()
    {
        ArrayList statsList = new ArrayList();
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);

        try
        {
            string query = @"SELECT application_status, COUNT(*) AS count
                            FROM asnaf_application
                            GROUP BY application_status
                            ORDER BY count DESC";

            SqlDataReader dataReader = dbConnect.ExecuteQuery(query);

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    Hashtable stats = new Hashtable();
                    stats["status"] = SafeGetString(dataReader, "application_status");
                    stats["count"] = Convert.ToInt32(dataReader["count"]);
                    statsList.Add(stats);
                }
                dataReader.Close();
            }
        }
        catch (Exception ex)
        {
            WriteToLogFile("MainControllerMSSQL-getApplicationStatsByStatus: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return statsList;
    }

    /// <summary>
    /// Get today's applications summary
    /// </summary>
    public Hashtable getTodayApplicationsSummary()
    {
        Hashtable todayData = new Hashtable();
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);

        try
        {
            // Total today
            string query = @"SELECT COUNT(*) FROM asnaf_application 
                           WHERE CONVERT(date, application_date) = CONVERT(date, GETDATE())";
            object totalToday = dbConnect.ExecuteScalar(query);
            todayData["totalToday"] = totalToday != null ? Convert.ToInt32(totalToday) : 0;

            // Pending today
            query = @"SELECT COUNT(*) FROM asnaf_application 
                     WHERE CONVERT(date, application_date) = CONVERT(date, GETDATE())
                     AND application_status = 'PENDING'";
            object pendingToday = dbConnect.ExecuteScalar(query);
            todayData["pendingToday"] = pendingToday != null ? Convert.ToInt32(pendingToday) : 0;

            // Approved today
            query = @"SELECT COUNT(*) FROM asnaf_application 
                     WHERE CONVERT(date, reviewed_date) = CONVERT(date, GETDATE())
                     AND application_status = 'APPROVED'";
            object approvedToday = dbConnect.ExecuteScalar(query);
            todayData["approvedToday"] = approvedToday != null ? Convert.ToInt32(approvedToday) : 0;

            // Amount today
            query = @"SELECT ISNULL(SUM(CAST(amount AS DECIMAL(18,2))), 0) 
                     FROM asnaf_application 
                     WHERE CONVERT(date, application_date) = CONVERT(date, GETDATE())
                     AND amount IS NOT NULL AND amount <> ''";
            object amountToday = dbConnect.ExecuteScalar(query);
            todayData["amountToday"] = amountToday != null ? Convert.ToDecimal(amountToday) : 0;

            WriteToLogFile("MainControllerMSSQL-getTodayApplicationsSummary: Today's data retrieved");
        }
        catch (Exception ex)
        {
            WriteToLogFile("MainControllerMSSQL-getTodayApplicationsSummary: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return todayData;
    }

    /// <summary>
    /// Get application trends for last N days
    /// </summary>
    public ArrayList getApplicationTrends(int days)
    {
        ArrayList trendsList = new ArrayList();
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);

        try
        {
            string query = @"SELECT CONVERT(date, application_date) AS app_date, 
                                   COUNT(*) AS count
                            FROM asnaf_application
                            WHERE application_date >= DATEADD(day, -" + days + @", GETDATE())
                            GROUP BY CONVERT(date, application_date)
                            ORDER BY app_date DESC";

            SqlDataReader dataReader = dbConnect.ExecuteQuery(query);

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    Hashtable trend = new Hashtable();
                    trend["date"] = SafeGetString(dataReader, "app_date");
                    trend["count"] = Convert.ToInt32(dataReader["count"]);
                    trendsList.Add(trend);
                }
                dataReader.Close();
            }

            WriteToLogFile("MainControllerMSSQL-getApplicationTrends: Retrieved trends for " + days + " days");
        }
        catch (Exception ex)
        {
            WriteToLogFile("MainControllerMSSQL-getApplicationTrends: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return trendsList;
    }

    /// <summary>
    /// Get monthly application summary for a specific year
    /// </summary>
    public ArrayList getMonthlyApplicationSummary(int year)
    {
        ArrayList monthlySummary = new ArrayList();
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);

        try
        {
            string query = @"SELECT MONTH(application_date) AS month,
                                   DATENAME(month, application_date) AS month_name,
                                   COUNT(*) AS total_count,
                                   SUM(CASE WHEN application_status = 'PENDING' THEN 1 ELSE 0 END) AS pending_count,
                                   SUM(CASE WHEN application_status = 'APPROVED' THEN 1 ELSE 0 END) AS approved_count,
                                   SUM(CASE WHEN application_status = 'REJECTED' THEN 1 ELSE 0 END) AS rejected_count
                            FROM asnaf_application
                            WHERE YEAR(application_date) = " + year + @"
                            GROUP BY MONTH(application_date), DATENAME(month, application_date)
                            ORDER BY MONTH(application_date)";

            SqlDataReader dataReader = dbConnect.ExecuteQuery(query);

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    Hashtable monthData = new Hashtable();
                    monthData["month"] = Convert.ToInt32(dataReader["month"]);
                    monthData["monthName"] = SafeGetString(dataReader, "month_name");
                    monthData["totalCount"] = Convert.ToInt32(dataReader["total_count"]);
                    monthData["pendingCount"] = Convert.ToInt32(dataReader["pending_count"]);
                    monthData["approvedCount"] = Convert.ToInt32(dataReader["approved_count"]);
                    monthData["rejectedCount"] = Convert.ToInt32(dataReader["rejected_count"]);
                    monthlySummary.Add(monthData);
                }
                dataReader.Close();
            }

            WriteToLogFile("MainControllerMSSQL-getMonthlyApplicationSummary: Retrieved data for year " + year);
        }
        catch (Exception ex)
        {
            WriteToLogFile("MainControllerMSSQL-getMonthlyApplicationSummary: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return monthlySummary;
    }

    /// <summary>
    /// Get applications breakdown by asnaf status
    /// </summary>
    public ArrayList getApplicationsByAsnafStatus()
    {
        ArrayList statusBreakdown = new ArrayList();
        DBConnectMSSQL dbConnect = new DBConnectMSSQL(sErrorLog);

        try
        {
            string query = @"SELECT ap.asnaf_status,
                                   COUNT(aa.app_uid) AS application_count
                            FROM asnaf_profile ap
                            LEFT JOIN asnaf_application aa ON ap.asnaf_uid = aa.asnaf_uid
                            WHERE ap.asnaf_status IS NOT NULL
                            GROUP BY ap.asnaf_status
                            ORDER BY application_count DESC";

            SqlDataReader dataReader = dbConnect.ExecuteQuery(query);

            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    Hashtable statusData = new Hashtable();
                    statusData["asnafStatus"] = SafeGetString(dataReader, "asnaf_status");
                    statusData["applicationCount"] = Convert.ToInt32(dataReader["application_count"]);
                    statusBreakdown.Add(statusData);
                }
                dataReader.Close();
            }

            WriteToLogFile("MainControllerMSSQL-getApplicationsByAsnafStatus: Status breakdown retrieved");
        }
        catch (Exception ex)
        {
            WriteToLogFile("MainControllerMSSQL-getApplicationsByAsnafStatus: " + ex.Message);
        }
        finally
        {
            dbConnect.CloseConnection();
            dbConnect.Dispose();
        }

        return statusBreakdown;
    }

    #endregion
}