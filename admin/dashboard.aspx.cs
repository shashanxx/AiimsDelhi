using ClosedXML.Excel;
using Common.Class;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

public partial class admin_dashboard : System.Web.UI.Page
{
    CommonPerception MySql = new CommonPerception();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            getExamDataDetails("Last");
        }
        catch (Exception ex)
        {
            Response.Write("Message: " + ex.Message + "<br />Stack Trace: " + ex.StackTrace);
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            string connectionString = getConnectionString();

            if (!fuExcelData.HasFile)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Please upload file');", true);
                fuExcelData.Focus();
                return;
            }

            string fileExtension = Path.GetExtension(fuExcelData.FileName);
            if (fileExtension.ToLower() != ".xlsx")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Please upload excel with .xlsx extension');", true);
                fuExcelData.Focus();
                return;
            }

            string filePath = Server.MapPath("~/files/") + Path.GetFileName(fuExcelData.PostedFile.FileName);

            if (!Directory.Exists(Server.MapPath("~/files/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/files/"));
            }

            fuExcelData.SaveAs(filePath);

            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                IXLWorksheet workSheet = workBook.Worksheet(1);
                DataTable dt = new DataTable();

                bool firstRow = true;
                foreach (IXLRow row in workSheet.Rows())
                {
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        dt.Rows.Add();
                        int i = 0;
                        foreach (IXLCell cell in row.Cells())
                        {
                            if(cell.Address.ColumnLetter == "A" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column A (RegistrationID) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }
                            if (cell.Address.ColumnLetter == "B" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column B (AppliedCourse) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }
                            if (cell.Address.ColumnLetter == "C" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column C (Name) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }
                            if (cell.Address.ColumnLetter == "D" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column D (PinNumber) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }
                            if (cell.Address.ColumnLetter == "J" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column J (IsOPH) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }
                            if (cell.Address.ColumnLetter == "K" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column K (CCODE) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }
                            if (cell.Address.ColumnLetter == "L" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column L (ROLL_NO) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }
                            if (cell.Address.ColumnLetter == "M" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column M (Centre Name) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }
                            if (cell.Address.ColumnLetter == "N" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column N (Centre Address) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }
                            if (cell.Address.ColumnLetter == "O" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column O (Centre Pincode) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }
                            if (cell.Address.ColumnLetter == "P" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column P (examdate) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }
                            if (cell.Address.ColumnLetter == "Q" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column Q (examtime) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }

                            //if (string.IsNullOrEmpty(cell.Value.ToString()))
                            //{
                            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Data Imported Successfully.');window.location='dashboard.aspx'", true);

                            //    return;
                            //}
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }
                }

                using (SqlConnection sqlConnection3 = new SqlConnection(connectionString))
                {
                    SqlCommand sqlCommand3 = new SqlCommand("spUpdate_ExamDataBackup", sqlConnection3);
                    sqlCommand3.CommandType = CommandType.StoredProcedure;

                    sqlConnection3.Open();
                    int inserted3 = sqlCommand3.ExecuteNonQuery();
                    if (inserted3 > 0)
                    {
                        sqlConnection3.Close();
                    }
                }

                foreach (DataRow dr in dt.Rows)
                {
                    using (SqlConnection sqlConnection2 = new SqlConnection(connectionString))
                    {
                        SqlCommand sqlCommand2 = new SqlCommand("spUpdate_ExamdataExcelData N'" + dr[0].ToString().Replace("'", "''")
                            + "',N'" + dr[1].ToString().Replace("'", "''") + "',N'" + dr[2].ToString().Replace("'", "''") + "',N'" + dr[3].ToString().Replace("'", "''")
                            + "',N'" + dr[4].ToString().Replace("'", "''") + "',N'" + dr[5].ToString().Replace("'", "''") + "',N'" + dr[6].ToString().Replace("'", "''")
                            + "',N'" + dr[7].ToString().Replace("'", "''") + "',N'" + dr[8].ToString().Replace("'", "''") + "',N'" + dr[9].ToString().Replace("'", "''")
                            + "',N'" + dr[10].ToString().Replace("'", "''") + "',N'" + dr[11].ToString().Replace("'", "''") + "',N'" + dr[12].ToString().Replace("'", "''")
                            + "',N'" + dr[13].ToString().Replace("'", "''") + "',N'" + dr[14].ToString().Replace("'", "''") + "',N'" + dr[15].ToString().Replace("'", "''")
                            + "',N'" + dr[16].ToString().Replace("'", "''") + "',N'" + dr[17].ToString().Replace("'", "''") + "',N'" + dr[18].ToString().Replace("'", "''")
                            + "'", sqlConnection2);
                        sqlCommand2.CommandType = CommandType.Text;

                        sqlConnection2.Open();
                        int inserted2 = sqlCommand2.ExecuteNonQuery();
                        if (inserted2 > 0)
                        {
                            sqlConnection2.Close();
                        }
                    }
                }

                getExamDataDetails("Current");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Data Imported Successfully.');window.location='dashboard.aspx'", true);

                Response.Redirect("dashboard.aspx");

                return;
            }
        }
        catch (Exception ex)
        {
            Response.Write("Message: " + ex.Message + "<br />Stack Trace: " + ex.StackTrace);
        }
    }

    protected void btnDownloadExcelFormat_Click(object sender, EventArgs e)
    {
        try
        {
            string URL = Server.MapPath("~/files/Format/") + "FormatAiimsDelhi.xlsx";

            Response.ContentType = "application/excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=FormatAiimsDelhi.xlsx");
            Response.TransmitFile(URL);
            Response.End();
        }
        catch (Exception ex)
        {
            Response.Write("Message: " + ex.Message + "<br />Stack Trace: " + ex.StackTrace);
        }
    }

    private string getConnectionString()
    {
        return ConfigurationManager.AppSettings["ConnectionString"];
    }

    public void getExamDataDetails(string timeline)
    {
        DataSet dataSet = MySql.GetDataSet("spGet_FullExamdataDetails");

        if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
        {
            lblExamDataDetails.Text = timeline + " Uploaded Exam Date: " + Convert.ToDateTime(dataSet.Tables[0].Rows[0]["examdate"]).ToString("dd-MM-yyyy") + " & Count: " + Convert.ToString(dataSet.Tables[0].Rows[0]["totalCount"]);
        }
    }
}