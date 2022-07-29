using ClosedXML.Excel;
using Common.Class;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

public partial class admin_updatesubjectdata : System.Web.UI.Page
{
    CommonPerception MySql = new CommonPerception();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnUpdatedUpload_Click(object sender, EventArgs e)
    {
        try
        {
            string connectionString = getConnectionString();

            if (!fuUpdatedExcelData.HasFile)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Please upload file');", true);
                fuUpdatedExcelData.Focus();
                return;
            }

            string fileExtension = Path.GetExtension(fuUpdatedExcelData.FileName);
            if (fileExtension.ToLower() != ".xlsx")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Please upload excel with .xlsx extension');", true);
                fuUpdatedExcelData.Focus();
                return;
            }

            string filePath = Server.MapPath("~/files/") + Path.GetFileName(fuUpdatedExcelData.PostedFile.FileName);

            if (!Directory.Exists(Server.MapPath("~/files/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/files/"));
            }

            fuUpdatedExcelData.SaveAs(filePath);

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
                            if (cell.Address.ColumnLetter == "A" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column A (AppliedCourse) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }
                            if (cell.Address.ColumnLetter == "B" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column B (IsOPH) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }
                            if (cell.Address.ColumnLetter == "C" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column C (SubjectCode) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }
                            if (cell.Address.ColumnLetter == "D" && string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Column D (subjectcodePH) can not be empty.');window.location='dashboard.aspx'", true);
                                return;
                            }

                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }
                }

                foreach (DataRow dr in dt.Rows)
                {
                    using (SqlConnection sqlConnection2 = new SqlConnection(connectionString))
                    {
                        SqlCommand sqlCommand2 = new SqlCommand("spUpdate_ExamdataSubjectCodeExcelData N'" + dr[0].ToString().Replace("'", "''")
                            + "',N'" + dr[1].ToString().Replace("'", "''") + "',N'" + dr[2].ToString().Replace("'", "''") + "',N'" + dr[3].ToString().Replace("'", "''")
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

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Data Updated Successfully.');window.location='updatesubjectdata.aspx'", true);
                return;
            }
        }
        catch (Exception ex)
        {
            Response.Write("Message: " + ex.Message + "<br />Stack Trace: " + ex.StackTrace);
        }
    }

    protected void btnDownloadUpdatedExcelFormat_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dataSet = MySql.GetDataSet("spGet_FullCenterDetailsForSubjectCodeAndPH");
            gvFullFormatExcelData.DataSource = dataSet;
            gvFullFormatExcelData.DataBind();

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                ExportGridToExcel(dataSet, "FormatAiimsDelhiSubject");
            }
            //string URL = Server.MapPath("~/files/Format/") + "UpdatedFormatAiimsDelhi.xlsx";

            //Response.ContentType = "application/excel";
            //Response.AppendHeader("Content-Disposition", "attachment; filename=UpdatedFormatAiimsDelhi.xlsx");
            //Response.TransmitFile(URL);
            //Response.End();
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

    private void ExportGridToExcel(DataSet ds, string file)
    {
        if (ds != null)
        {
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(dt, file + ".xlsx");
                wb.SaveAs(Server.MapPath("~/files/DownloadData/") + file + ".xlsx");

                string URL = Server.MapPath("~/files/DownloadData/") + file + ".xlsx";

                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + file + ".xlsx");
                Response.TransmitFile(URL);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('No records found.');", true);
            }
        }
    }
}