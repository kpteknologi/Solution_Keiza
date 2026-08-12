using System;
using System.Runtime.Serialization;

/// <summary>
/// Data Model for Asnaf Profile and Asnaf Application
/// Represents both Asnaf Profile and Asnaf Application records in MS SQL Server
/// Merged model containing asnaf_application table fields + asnaf_profile table fields
/// </summary>
[DataContract]
public class MainModelMSSQL
{
    #region Asnaf Application Fields

    // app_uid - Application Primary Key
    private string appUid = "";
    [DataMember]
    public string GetSetappUid
    {
        get { return appUid ?? ""; }
        set { appUid = value; }
    }

    // asnaf_uid - Foreign Key to asnaf_profile table
    private string asnafUid = "";
    [DataMember]
    public string GetSetasnafUid
    {
        get { return asnafUid ?? ""; }
        set { asnafUid = value; }
    }

    // application_no - Application Number
    private string applicationNo = "";
    [DataMember]
    public string GetSetapplicationNo
    {
        get { return applicationNo ?? ""; }
        set { applicationNo = value; }
    }

    // application_date - Application Date
    private string applicationDate = "";
    [DataMember]
    public string GetSetapplicationDate
    {
        get { return applicationDate ?? ""; }
        set { applicationDate = value; }
    }

    // submitted_date - Date Submitted
    private string submittedDate = "";
    [DataMember]
    public string GetSetsubmittedDate
    {
        get { return submittedDate ?? ""; }
        set { submittedDate = value; }
    }

    // application_status - Application Status
    private string applicationStatus = "";
    [DataMember]
    public string GetSetapplicationStatus
    {
        get { return applicationStatus ?? ""; }
        set { applicationStatus = value; }
    }

    // amount - Application Amount
    private string amount = "";
    [DataMember]
    public string GetSetamount
    {
        get { return amount ?? ""; }
        set { amount = value; }
    }

    // currency - Currency Code
    private string currency = "";
    [DataMember]
    public string GetSetcurrency
    {
        get { return currency ?? ""; }
        set { currency = value; }
    }

    // remarks - Application Remarks
    private string remarks = "";
    [DataMember]
    public string GetSetremarks
    {
        get { return remarks ?? ""; }
        set { remarks = value; }
    }

    // submitted_by - Submitted By User
    private string submittedBy = "";
    [DataMember]
    public string GetSetsubmittedBy
    {
        get { return submittedBy ?? ""; }
        set { submittedBy = value; }
    }

    // reviewed_by - Reviewed By User
    private string reviewedBy = "";
    [DataMember]
    public string GetSetreviewedBy
    {
        get { return reviewedBy ?? ""; }
        set { reviewedBy = value; }
    }

    // reviewed_date - Review Date
    private string reviewedDate = "";
    [DataMember]
    public string GetSetreviewedDate
    {
        get { return reviewedDate ?? ""; }
        set { reviewedDate = value; }
    }

    // application confirmeddate
    private string applicationConfirmedDate = "";
    [DataMember]
    public string GetSetapplicationConfirmedDate
    {
        get { return applicationConfirmedDate ?? ""; }
        set { applicationConfirmedDate = value; }
    }

    // application createdby
    private string applicationCreatedBy = "";
    [DataMember]
    public string GetSetapplicationCreatedBy
    {
        get { return applicationCreatedBy ?? ""; }
        set { applicationCreatedBy = value; }
    }

    // application createddate
    private string applicationCreatedDate = "";
    [DataMember]
    public string GetSetapplicationCreatedDate
    {
        get { return applicationCreatedDate ?? ""; }
        set { applicationCreatedDate = value; }
    }

    // application modifiedby
    private string applicationModifiedBy = "";
    [DataMember]
    public string GetSetapplicationModifiedBy
    {
        get { return applicationModifiedBy ?? ""; }
        set { applicationModifiedBy = value; }
    }

    // application modifieddate
    private string applicationModifiedDate = "";
    [DataMember]
    public string GetSetapplicationModifiedDate
    {
        get { return applicationModifiedDate ?? ""; }
        set { applicationModifiedDate = value; }
    }

    #endregion

    #region Asnaf Profile Table Fields

    // asnaf_name - Beneficiary name
    private string asnafName = "";
    [DataMember]
    public string GetSetasnafName
    {
        get { return asnafName ?? ""; }
        set { asnafName = value; }
    }

    // asnaf_icno - IC/Passport number
    private string asnafIcno = "";
    [DataMember]
    public string GetSetasnafIcno
    {
        get { return asnafIcno ?? ""; }
        set { asnafIcno = value; }
    }

    // res_address1 - Residential Address line 1
    private string resAddress1 = "";
    [DataMember]
    public string GetSetresAddress1
    {
        get { return resAddress1 ?? ""; }
        set { resAddress1 = value; }
    }

    // res_address2 - Residential Address line 2
    private string resAddress2 = "";
    [DataMember]
    public string GetSetresAddress2
    {
        get { return resAddress2 ?? ""; }
        set { resAddress2 = value; }
    }

    // res_address3 - Residential Address line 3
    private string resAddress3 = "";
    [DataMember]
    public string GetSetresAddress3
    {
        get { return resAddress3 ?? ""; }
        set { resAddress3 = value; }
    }

    // postcode - Postal code
    private string postcode = "";
    [DataMember]
    public string GetSetpostcode
    {
        get { return postcode ?? ""; }
        set { postcode = value; }
    }

    // district - District
    private string district = "";
    [DataMember]
    public string GetSetdistrict
    {
        get { return district ?? ""; }
        set { district = value; }
    }

    // state - State
    private string state = "";
    [DataMember]
    public string GetSetstate
    {
        get { return state ?? ""; }
        set { state = value; }
    }

    // email - Email address
    private string email = "";
    [DataMember]
    public string GetSetemail
    {
        get { return email ?? ""; }
        set { email = value; }
    }

    // contactno - Contact number
    private string contactno = "";
    [DataMember]
    public string GetSetcontactno
    {
        get { return contactno ?? ""; }
        set { contactno = value; }
    }

    // asnaf_status - Status (Active, Pending, etc.)
    private string asnafStatus = "";
    [DataMember]
    public string GetSetasnafStatus
    {
        get { return asnafStatus ?? ""; }
        set { asnafStatus = value; }
    }

    // createdby - Who created the Asnaf Profile record
    private string createdBy = "";
    [DataMember]
    public string GetSetcreatedBy
    {
        get { return createdBy ?? ""; }
        set { createdBy = value; }
    }

    // createddate - When Asnaf Profile record created
    private string createdDate = "";
    [DataMember]
    public string GetSetcreatedDate
    {
        get { return createdDate ?? ""; }
        set { createdDate = value; }
    }

    // modifiedby - Who modified the Asnaf Profile record
    private string modifiedBy = "";
    [DataMember]
    public string GetSetmodifiedBy
    {
        get { return modifiedBy ?? ""; }
        set { modifiedBy = value; }
    }

    // modifieddate - When Asnaf Profile record modified
    private string modifiedDate = "";
    [DataMember]
    public string GetSetmodifiedDate
    {
        get { return modifiedDate ?? ""; }
        set { modifiedDate = value; }
    }

    // confirmedby - Who confirmed the Asnaf Profile record
    private string confirmedBy = "";
    [DataMember]
    public string GetSetconfirmedBy
    {
        get { return confirmedBy ?? ""; }
        set { confirmedBy = value; }
    }

    // confirmeddate - When Asnaf Profile record confirmed
    private string confirmedDate = "";
    [DataMember]
    public string GetSetconfirmedDate
    {
        get { return confirmedDate ?? ""; }
        set { confirmedDate = value; }
    }

    #endregion

    #region Dashboard Scoreboard Fields

    // Dashboard-specific properties (optional - for structured data)

    private int totalApplications = 0;
    [DataMember]
    public int GetSettotalApplications
    {
        get { return totalApplications; }
        set { totalApplications = value; }
    }

    private int pendingApplications = 0;
    [DataMember]
    public int GetSetpendingApplications
    {
        get { return pendingApplications; }
        set { pendingApplications = value; }
    }

    private int approvedApplications = 0;
    [DataMember]
    public int GetSetapprovedApplications
    {
        get { return approvedApplications; }
        set { approvedApplications = value; }
    }

    private int rejectedApplications = 0;
    [DataMember]
    public int GetSetrejectedApplications
    {
        get { return rejectedApplications; }
        set { rejectedApplications = value; }
    }

    private int totalAsnaf = 0;
    [DataMember]
    public int GetSettotalAsnaf
    {
        get { return totalAsnaf; }
        set { totalAsnaf = value; }
    }

    private int todayApplications = 0;
    [DataMember]
    public int GettodayApplications
    {
        get { return todayApplications; }
        set { todayApplications = value; }
    }

    #endregion
}