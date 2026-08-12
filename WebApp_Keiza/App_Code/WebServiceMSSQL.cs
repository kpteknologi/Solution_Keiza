using System;
using System.Collections;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

/// <summary>
/// Web Service for MS SQL Server Asnaf Profile and Application Operations
/// Provides AJAX endpoints for CRUD operations
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[System.Web.Script.Services.ScriptService]
public class WebServiceMSSQL : System.Web.Services.WebService
{
    private MainControllerMSSQL oMainControllerMSSQL;

    public WebServiceMSSQL()
    {
        oMainControllerMSSQL = new MainControllerMSSQL();
    }

    #region Asnaf Profile Web Methods

    /// <summary>
    /// Get all Asnaf Profiles
    /// </summary>
    [WebMethod]
    public void GetAsnafProfiles()
    {
        try
        {
            ArrayList lsAsnafProfiles = oMainControllerMSSQL.getAsnafProfiles();
            object objData = new { status = "Y", data = lsAsnafProfiles, message = "Asnaf profiles retrieved successfully" };
            fnReturnJsonResponse(objData);
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", data = new { }, message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    /// <summary>
    /// Get Asnaf Profile by ID
    /// </summary>
    [WebMethod]
    public void GetAsnafProfileById(String asnafUid)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(asnafUid))
            {
                object objDataError = new { status = "N", data = new { }, message = "Asnaf UID is required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            MainModelMSSQL oAsnaf = oMainControllerMSSQL.getAsnafProfileById(asnafUid);

            if (oAsnaf != null && !string.IsNullOrWhiteSpace(oAsnaf.GetSetasnafUid))
            {
                object objData = new { status = "Y", data = oAsnaf, message = "Asnaf profile retrieved successfully" };
                fnReturnJsonResponse(objData);
            }
            else
            {
                object objData = new { status = "N", data = new { }, message = "Asnaf profile not found" };
                fnReturnJsonResponse(objData);
            }
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", data = new { }, message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    /// <summary>
    /// Search Asnaf Profile by Name or IC Number
    /// </summary>
    [WebMethod]
    public void SearchAsnafProfile(String searchText)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                object objDataError = new { status = "N", data = new { }, message = "Search text is required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            ArrayList lsAsnafProfiles = oMainControllerMSSQL.searchAsnafProfile(searchText);
            object objData = new { status = "Y", data = lsAsnafProfiles, message = "Search completed successfully" };
            fnReturnJsonResponse(objData);
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", data = new { }, message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    /// <summary>
    /// Add New Asnaf Profile
    /// </summary>
    [WebMethod]
    public void AddAsnafProfile(String asnafName, String asnafIcno, String resAddress1, String resAddress2,
        String resAddress3, String postcode, String district, String state, String email, String contactno,
        String asnafStatus, String createdBy)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(asnafName))
            {
                object objDataError = new { status = "N", message = "Asnaf name is required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            MainModelMSSQL oAsnaf = new MainModelMSSQL();
            oAsnaf.GetSetasnafName = asnafName;
            oAsnaf.GetSetasnafIcno = asnafIcno;
            oAsnaf.GetSetresAddress1 = resAddress1;
            oAsnaf.GetSetresAddress2 = resAddress2;
            oAsnaf.GetSetresAddress3 = resAddress3;
            oAsnaf.GetSetpostcode = postcode;
            oAsnaf.GetSetdistrict = district;
            oAsnaf.GetSetstate = state;
            oAsnaf.GetSetemail = email;
            oAsnaf.GetSetcontactno = contactno;
            oAsnaf.GetSetasnafStatus = string.IsNullOrWhiteSpace(asnafStatus) ? "ACTIVE" : asnafStatus;
            oAsnaf.GetSetcreatedBy = createdBy;

            string result = oMainControllerMSSQL.addAsnafProfile(oAsnaf);

            if (result == "Y")
            {
                object objData = new { status = "Y", message = "Asnaf profile added successfully" };
                fnReturnJsonResponse(objData);
            }
            else
            {
                object objData = new { status = "N", message = "Failed to add Asnaf profile" };
                fnReturnJsonResponse(objData);
            }
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    /// <summary>
    /// Update Asnaf Profile
    /// </summary>
    [WebMethod]
    public void UpdateAsnafProfile(String asnafUid, String asnafName, String asnafIcno, String resAddress1,
        String resAddress2, String resAddress3, String postcode, String district, String state, String email,
        String contactno, String asnafStatus, String modifiedBy)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(asnafUid) || string.IsNullOrWhiteSpace(asnafName))
            {
                object objDataError = new { status = "N", message = "Asnaf UID and Name are required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            MainModelMSSQL oAsnaf = new MainModelMSSQL();
            oAsnaf.GetSetasnafUid = asnafUid;
            oAsnaf.GetSetasnafName = asnafName;
            oAsnaf.GetSetasnafIcno = asnafIcno;
            oAsnaf.GetSetresAddress1 = resAddress1;
            oAsnaf.GetSetresAddress2 = resAddress2;
            oAsnaf.GetSetresAddress3 = resAddress3;
            oAsnaf.GetSetpostcode = postcode;
            oAsnaf.GetSetdistrict = district;
            oAsnaf.GetSetstate = state;
            oAsnaf.GetSetemail = email;
            oAsnaf.GetSetcontactno = contactno;
            oAsnaf.GetSetasnafStatus = asnafStatus;
            oAsnaf.GetSetmodifiedBy = modifiedBy;

            string result = oMainControllerMSSQL.updateAsnafProfile(oAsnaf);

            if (result == "Y")
            {
                object objData = new { status = "Y", message = "Asnaf profile updated successfully" };
                fnReturnJsonResponse(objData);
            }
            else
            {
                object objData = new { status = "N", message = "Failed to update Asnaf profile" };
                fnReturnJsonResponse(objData);
            }
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    /// <summary>
    /// Delete Asnaf Profile
    /// </summary>
    [WebMethod]
    public void DeleteAsnafProfile(String asnafUid)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(asnafUid))
            {
                object objDataError = new { status = "N", message = "Asnaf UID is required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            string result = oMainControllerMSSQL.deleteAsnafProfile(asnafUid);

            if (result == "Y")
            {
                object objData = new { status = "Y", message = "Asnaf profile deleted successfully" };
                fnReturnJsonResponse(objData);
            }
            else
            {
                object objData = new { status = "N", message = "Cannot delete - Asnaf may have associated applications" };
                fnReturnJsonResponse(objData);
            }
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    /// <summary>
    /// Confirm Asnaf Profile
    /// </summary>
    [WebMethod]
    public void ConfirmAsnafProfile(String asnafUid, String confirmedBy)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(asnafUid) || string.IsNullOrWhiteSpace(confirmedBy))
            {
                object objDataError = new { status = "N", message = "Asnaf UID and Confirmed By are required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            string result = oMainControllerMSSQL.confirmAsnafProfile(asnafUid, confirmedBy);

            if (result == "Y")
            {
                object objData = new { status = "Y", message = "Asnaf profile confirmed successfully" };
                fnReturnJsonResponse(objData);
            }
            else
            {
                object objData = new { status = "N", message = "Failed to confirm Asnaf profile" };
                fnReturnJsonResponse(objData);
            }
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    #endregion

    #region Asnaf Application Web Methods

    [WebMethod]
    public void GetAsnafApplications()
    {
        try
        {
            ArrayList lsApplications = oMainControllerMSSQL.getAsnafApplications();
            object objData = new { status = "Y", data = lsApplications, message = "Applications retrieved successfully" };
            fnReturnJsonResponse(objData);
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", data = new { }, message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    [WebMethod]
    public void GetAsnafApplicationById(String appUid)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(appUid))
            {
                object objDataError = new { status = "N", data = new { }, message = "Application UID is required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            MainModelMSSQL oApp = oMainControllerMSSQL.getAsnafApplicationById(appUid);

            if (oApp != null && !string.IsNullOrWhiteSpace(oApp.GetSetappUid))
            {
                object objData = new { status = "Y", data = oApp, message = "Application retrieved successfully" };
                fnReturnJsonResponse(objData);
            }
            else
            {
                object objData = new { status = "N", data = new { }, message = "Application not found" };
                fnReturnJsonResponse(objData);
            }
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", data = new { }, message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    [WebMethod]
    public void GetAsnafApplicationsByAsnafUid(String asnafUid)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(asnafUid))
            {
                object objDataError = new { status = "N", data = new { }, message = "Asnaf UID is required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            ArrayList lsApplications = oMainControllerMSSQL.getAsnafApplicationsByAsnafUid(asnafUid);
            object objData = new { status = "Y", data = lsApplications, message = "Applications retrieved successfully" };
            fnReturnJsonResponse(objData);
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", data = new { }, message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    [WebMethod]
    public void SearchAsnafApplicationsByAsnafUid(String asnafUid, String searchText)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(asnafUid))
            {
                object objDataError = new { status = "N", data = new { }, message = "Asnaf UID is required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            ArrayList lsApplications = oMainControllerMSSQL.searchAsnafApplicationsByAsnafUid(asnafUid, searchText);
            object objData = new { status = "Y", data = lsApplications, message = "Search completed successfully" };
            fnReturnJsonResponse(objData);
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", data = new { }, message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    [WebMethod]
    public void AddAsnafApplication(String asnafUid, String applicationNo, String applicationStatus,
        String amount, String currency, String remarks, String createdBy)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(asnafUid) || string.IsNullOrWhiteSpace(applicationNo))
            {
                object objDataError = new { status = "N", message = "Asnaf UID and Application No are required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            MainModelMSSQL oApp = new MainModelMSSQL();
            oApp.GetSetasnafUid = asnafUid;
            oApp.GetSetapplicationNo = applicationNo;
            oApp.GetSetapplicationStatus = string.IsNullOrWhiteSpace(applicationStatus) ? "DRAFT" : applicationStatus;
            oApp.GetSetamount = amount;
            oApp.GetSetcurrency = currency;
            oApp.GetSetremarks = remarks;
            oApp.GetSetapplicationCreatedBy = createdBy;

            string result = oMainControllerMSSQL.addAsnafApplication(oApp);

            if (result == "Y")
            {
                object objData = new { status = "Y", message = "Application added successfully" };
                fnReturnJsonResponse(objData);
            }
            else
            {
                object objData = new { status = "N", message = "Failed to add application" };
                fnReturnJsonResponse(objData);
            }
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    [WebMethod]
    public void UpdateAsnafApplication(String appUid, String applicationNo, String applicationStatus,
        String amount, String currency, String remarks, String modifiedBy)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(appUid) || string.IsNullOrWhiteSpace(applicationNo))
            {
                object objDataError = new { status = "N", message = "Application UID and Application No are required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            MainModelMSSQL oApp = new MainModelMSSQL();
            oApp.GetSetappUid = appUid;
            oApp.GetSetapplicationNo = applicationNo;
            oApp.GetSetapplicationStatus = applicationStatus;
            oApp.GetSetamount = amount;
            oApp.GetSetcurrency = currency;
            oApp.GetSetremarks = remarks;
            oApp.GetSetapplicationModifiedBy = modifiedBy;

            string result = oMainControllerMSSQL.updateAsnafApplication(oApp);

            if (result == "Y")
            {
                object objData = new { status = "Y", message = "Application updated successfully" };
                fnReturnJsonResponse(objData);
            }
            else
            {
                object objData = new { status = "N", message = "Failed to update application" };
                fnReturnJsonResponse(objData);
            }
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    [WebMethod]
    public void UpdateAsnafApplicationByAsnafUid(String asnafUid, String appUid, String applicationNo,
        String applicationStatus, String amount, String currency, String remarks, String modifiedBy)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(asnafUid) || string.IsNullOrWhiteSpace(appUid))
            {
                object objDataError = new { status = "N", message = "Asnaf UID and Application UID are required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            MainModelMSSQL oApp = new MainModelMSSQL();
            oApp.GetSetappUid = appUid;
            oApp.GetSetapplicationNo = applicationNo;
            oApp.GetSetapplicationStatus = applicationStatus;
            oApp.GetSetamount = amount;
            oApp.GetSetcurrency = currency;
            oApp.GetSetremarks = remarks;
            oApp.GetSetapplicationModifiedBy = modifiedBy;

            string result = oMainControllerMSSQL.updateAsnafApplicationByAsnafUid(asnafUid, oApp);

            if (result == "Y")
            {
                object objData = new { status = "Y", message = "Application updated successfully" };
                fnReturnJsonResponse(objData);
            }
            else
            {
                object objData = new { status = "N", message = "Failed to update application" };
                fnReturnJsonResponse(objData);
            }
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    [WebMethod]
    public void SubmitAsnafApplication(String appUid, String submittedBy)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(appUid) || string.IsNullOrWhiteSpace(submittedBy))
            {
                object objDataError = new { status = "N", message = "Application UID and Submitted By are required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            string result = oMainControllerMSSQL.submitAsnafApplication(appUid, submittedBy);

            if (result == "Y")
            {
                object objData = new { status = "Y", message = "Application submitted successfully" };
                fnReturnJsonResponse(objData);
            }
            else
            {
                object objData = new { status = "N", message = "Failed to submit application" };
                fnReturnJsonResponse(objData);
            }
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    [WebMethod]
    public void ReviewAsnafApplication(String appUid, String reviewedBy, String applicationStatus)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(appUid) || string.IsNullOrWhiteSpace(reviewedBy))
            {
                object objDataError = new { status = "N", message = "Application UID and Reviewed By are required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            string result = oMainControllerMSSQL.reviewAsnafApplication(appUid, reviewedBy, applicationStatus);

            if (result == "Y")
            {
                object objData = new { status = "Y", message = "Application reviewed successfully" };
                fnReturnJsonResponse(objData);
            }
            else
            {
                object objData = new { status = "N", message = "Failed to review application" };
                fnReturnJsonResponse(objData);
            }
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    [WebMethod]
    public void DeleteAsnafApplication(String appUid)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(appUid))
            {
                object objDataError = new { status = "N", message = "Application UID is required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            string result = oMainControllerMSSQL.deleteAsnafApplication(appUid);

            if (result == "Y")
            {
                object objData = new { status = "Y", message = "Application deleted successfully" };
                fnReturnJsonResponse(objData);
            }
            else
            {
                object objData = new { status = "N", message = "Failed to delete application" };
                fnReturnJsonResponse(objData);
            }
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    [WebMethod]
    public void DeleteAsnafApplicationByAsnafUid(String asnafUid, String appUid)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(asnafUid) || string.IsNullOrWhiteSpace(appUid))
            {
                object objDataError = new { status = "N", message = "Asnaf UID and Application UID are required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            string result = oMainControllerMSSQL.deleteAsnafApplicationByAsnafUid(asnafUid, appUid);

            if (result == "Y")
            {
                object objData = new { status = "Y", message = "Application deleted successfully" };
                fnReturnJsonResponse(objData);
            }
            else
            {
                object objData = new { status = "N", message = "Failed to delete application" };
                fnReturnJsonResponse(objData);
            }
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    [WebMethod]
    public void DeleteAllAsnafApplicationsByAsnafUid(String asnafUid)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(asnafUid))
            {
                object objDataError = new { status = "N", message = "Asnaf UID is required" };
                fnReturnJsonResponse(objDataError);
                return;
            }

            string result = oMainControllerMSSQL.deleteAllAsnafApplicationsByAsnafUid(asnafUid);

            if (result == "Y")
            {
                object objData = new { status = "Y", message = "All applications deleted successfully" };
                fnReturnJsonResponse(objData);
            }
            else
            {
                object objData = new { status = "N", message = "Failed to delete applications" };
                fnReturnJsonResponse(objData);
            }
        }
        catch (Exception ex)
        {
            object objData = new { status = "N", message = "Error: " + ex.Message };
            fnReturnJsonResponse(objData);
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Return JSON Response and end the request properly
    /// </summary>

    private void fnReturnJsonResponse(object objData)
    {
        string jsonResponse = new JavaScriptSerializer().Serialize(objData);

        HttpContext context = HttpContext.Current;
        context.Response.Clear();
        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.Write(jsonResponse);

        // Complete the request and prevent any further output
        try
        {
            context.Response.Flush();
            context.Response.Close();
        }
        catch
        {
            // Suppress any errors
        }
    }

    #endregion

    #region Dashboard Scoreboard Web Methods

    /// <summary>
    /// Get Dashboard Scoreboard Statistics
    /// Returns real-time counts for applications and asnaf profiles
    /// </summary>
    [WebMethod]
    public void GetDashboardScoreboard()
    {
        try
        {
            Hashtable dashboardData = oMainControllerMSSQL.getDashboardScoreboard();

            if (dashboardData != null && dashboardData.Count > 0)
            {
                object objData = new
                {
                    status = "Y",
                    data = dashboardData,
                    message = "Dashboard statistics retrieved successfully",
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                fnReturnJsonResponse(objData);
            }
            else
            {
                object objData = new
                {
                    status = "N",
                    data = new { },
                    message = "No data available"
                };
                fnReturnJsonResponse(objData);
            }
        }
        catch (Exception ex)
        {
            object objData = new
            {
                status = "N",
                data = new { },
                message = "Error: " + ex.Message
            };
            fnReturnJsonResponse(objData);
        }
    }

    /// <summary>
    /// Get Application Statistics by Status
    /// Returns count of applications grouped by status
    /// </summary>
    [WebMethod]
    public void GetApplicationStatsByStatus()
    {
        try
        {
            ArrayList statsData = oMainControllerMSSQL.getApplicationStatsByStatus();
            object objData = new
            {
                status = "Y",
                data = statsData,
                message = "Application statistics retrieved successfully"
            };
            fnReturnJsonResponse(objData);
        }
        catch (Exception ex)
        {
            object objData = new
            {
                status = "N",
                data = new { },
                message = "Error: " + ex.Message
            };
            fnReturnJsonResponse(objData);
        }
    }

    /// <summary>
    /// Get Today's Application Summary
    /// Returns applications submitted today
    /// </summary>
    [WebMethod]
    public void GetTodayApplicationsSummary()
    {
        try
        {
            Hashtable todayData = oMainControllerMSSQL.getTodayApplicationsSummary();
            object objData = new
            {
                status = "Y",
                data = todayData,
                message = "Today's applications retrieved successfully"
            };
            fnReturnJsonResponse(objData);
        }
        catch (Exception ex)
        {
            object objData = new
            {
                status = "N",
                data = new { },
                message = "Error: " + ex.Message
            };
            fnReturnJsonResponse(objData);
        }
    }

    /// <summary>
    /// Get Application Trends by Date Range
    /// Returns application counts for last N days
    /// </summary>
    [WebMethod]
    public void GetApplicationTrends(int days)
    {
        try
        {
            if (days <= 0)
                days = 7; // Default to 7 days

            ArrayList trendsData = oMainControllerMSSQL.getApplicationTrends(days);
            object objData = new
            {
                status = "Y",
                data = trendsData,
                message = "Application trends retrieved successfully",
                period = days + " days"
            };
            fnReturnJsonResponse(objData);
        }
        catch (Exception ex)
        {
            object objData = new
            {
                status = "N",
                data = new { },
                message = "Error: " + ex.Message
            };
            fnReturnJsonResponse(objData);
        }
    }

    /// <summary>
    /// Get Monthly Application Summary
    /// Returns application counts by month for current year
    /// </summary>
    [WebMethod]
    public void GetMonthlyApplicationSummary(int year)
    {
        try
        {
            if (year <= 0)
                year = DateTime.Now.Year;

            ArrayList monthlyData = oMainControllerMSSQL.getMonthlyApplicationSummary(year);
            object objData = new
            {
                status = "Y",
                data = monthlyData,
                message = "Monthly summary retrieved successfully",
                year = year
            };
            fnReturnJsonResponse(objData);
        }
        catch (Exception ex)
        {
            object objData = new
            {
                status = "N",
                data = new { },
                message = "Error: " + ex.Message
            };
            fnReturnJsonResponse(objData);
        }
    }

    /// <summary>
    /// Get Application Statistics by Asnaf Status
    /// Returns breakdown of applications by asnaf status
    /// </summary>
    [WebMethod]
    public void GetApplicationsByAsnafStatus()
    {
        try
        {
            ArrayList statusData = oMainControllerMSSQL.getApplicationsByAsnafStatus();
            object objData = new
            {
                status = "Y",
                data = statusData,
                message = "Asnaf status breakdown retrieved successfully"
            };
            fnReturnJsonResponse(objData);
        }
        catch (Exception ex)
        {
            object objData = new
            {
                status = "N",
                data = new { },
                message = "Error: " + ex.Message
            };
            fnReturnJsonResponse(objData);
        }
    }

    #endregion
}