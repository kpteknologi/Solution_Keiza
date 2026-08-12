using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Web;

/// <summary>
/// Summary description for DBConnectMSSQL
/// Database Connection Manager for MS SQL Server
/// </summary>
public class DBConnectMSSQL
{
    public SqlConnection connection;
    private string sErrorLog = "";
    private MainController oMainCon;

    // Constructor
    public DBConnectMSSQL(String _sErrorLog)
    {
        Initialize();
        sErrorLog = _sErrorLog;
        oMainCon = new MainController(sErrorLog);
    }

    // Initialize values
    private void Initialize()
    {
        try
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MSSQLConnection"].ConnectionString;
            connection = new SqlConnection(connectionString);
        }
        catch (Exception ex)
        {
            if (oMainCon != null)
            {
                oMainCon.WriteToLogFile("DBConnectMSSQL-Initialize: " + ex.Message);
            }
        }
    }

    // Open connection to database
    public bool OpenConnection()
    {
        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return true;
        }
        catch (SqlException ex)
        {
            // Handle common SQL Server errors
            switch (ex.Number)
            {
                case -2:
                    oMainCon.WriteToLogFile("DBConnectMSSQL-OpenConnection: Timeout expired. Cannot connect to server.");
                    break;
                case 18456:
                    oMainCon.WriteToLogFile("DBConnectMSSQL-OpenConnection: Login failed. Invalid username or password.");
                    break;
                case 4221:
                    oMainCon.WriteToLogFile("DBConnectMSSQL-OpenConnection: Login timeout expired.");
                    break;
                default:
                    oMainCon.WriteToLogFile("DBConnectMSSQL-OpenConnection: SQL Error " + ex.Number + " - " + ex.Message);
                    break;
            }
            return false;
        }
        catch (Exception ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-OpenConnection: " + ex.Message);
            return false;
        }
    }

    // Close connection
    public bool CloseConnection()
    {
        try
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return true;
        }
        catch (SqlException ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-CloseConnection: " + ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-CloseConnection: " + ex.Message);
            return false;
        }
    }

    // Insert statement
    public bool Insert(string query)
    {
        try
        {
            if (this.OpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                this.CloseConnection();
                return true;
            }
            return false;
        }
        catch (SqlException ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-Insert: " + ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-Insert: " + ex.Message);
            return false;
        }
    }

    // Update statement
    public bool Update(string query)
    {
        try
        {
            if (this.OpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                this.CloseConnection();
                return true;
            }
            return false;
        }
        catch (SqlException ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-Update: " + ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-Update: " + ex.Message);
            return false;
        }
    }

    // Delete statement
    public bool Delete(string query)
    {
        try
        {
            if (this.OpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                this.CloseConnection();
                return true;
            }
            return false;
        }
        catch (SqlException ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-Delete: " + ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-Delete: " + ex.Message);
            return false;
        }
    }

    // Select statement - Returns DataTable
    public DataTable Select(string query)
    {
        DataTable dt = new DataTable();
        try
        {
            if (this.OpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                this.CloseConnection();
            }
            return dt;
        }
        catch (SqlException ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-Select: " + ex.Message);
            return dt;
        }
        catch (Exception ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-Select: " + ex.Message);
            return dt;
        }
    }

    // Select statement - Returns SqlDataReader
    public SqlDataReader ExecuteQuery(string query)
    {
        try
        {
            if (this.OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                return dataReader;
            }
            return null;
        }
        catch (SqlException ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-ExecuteQuery: " + ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-ExecuteQuery: " + ex.Message);
            return null;
        }
    }

    // Count statement
    public int Count(string query)
    {
        int count = -1;
        try
        {
            if (this.OpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        int.TryParse(result.ToString(), out count);
                    }
                }
                this.CloseConnection();
            }
            return count;
        }
        catch (SqlException ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-Count: " + ex.Message);
            return count;
        }
        catch (Exception ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-Count: " + ex.Message);
            return count;
        }
    }

    // Execute Scalar - Returns single value
    public object ExecuteScalar(string query)
    {
        try
        {
            if (this.OpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    object result = cmd.ExecuteScalar();
                    this.CloseConnection();
                    return result;
                }
            }
            return null;
        }
        catch (SqlException ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-ExecuteScalar: " + ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-ExecuteScalar: " + ex.Message);
            return null;
        }
    }

    // Execute Non Query - Returns affected rows
    public int ExecuteNonQuery(string query)
    {
        try
        {
            if (this.OpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    int result = cmd.ExecuteNonQuery();
                    this.CloseConnection();
                    return result;
                }
            }
            return -1;
        }
        catch (SqlException ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-ExecuteNonQuery: " + ex.Message);
            return -1;
        }
        catch (Exception ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-ExecuteNonQuery: " + ex.Message);
            return -1;
        }
    }

    // Replace Null Values
    public string ReplaceNull(SqlDataReader dataReader, string columnName)
    {
        try
        {
            object oValue = dataReader[columnName];
            if (oValue == DBNull.Value)
            {
                return "";
            }
            else
            {
                return oValue.ToString();
            }
        }
        catch
        {
            return "";
        }
    }

    // Replace Null Values from DataTable
    public string ReplaceNull(DataTable dt, int rowIndex, string columnName)
    {
        try
        {
            if (dt.Rows.Count > rowIndex)
            {
                object oValue = dt.Rows[rowIndex][columnName];
                if (oValue == DBNull.Value)
                {
                    return "";
                }
                else
                {
                    return oValue.ToString();
                }
            }
            return "";
        }
        catch
        {
            return "";
        }
    }

    // Get Connection State
    public ConnectionState GetConnectionState()
    {
        return connection.State;
    }

    // Dispose Connection
    public void Dispose()
    {
        try
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
                connection.Dispose();
            }
        }
        catch (Exception ex)
        {
            oMainCon.WriteToLogFile("DBConnectMSSQL-Dispose: " + ex.Message);
        }
    }
}