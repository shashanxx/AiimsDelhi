using ClosedXML.Excel;
using Common.Class;
using System;
using System.Data;
using System.Web.UI;

public partial class admin_examdatasummary : System.Web.UI.Page
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

    protected void btnDownloadExcel_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dataSet = MySql.GetDataSet("spGet_FullExamdata");
            gvFullExcelData.DataSource = dataSet;
            gvFullExcelData.DataBind();

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                ExportGridToExcel(dataSet, "AiimsDelhiExamData");
            }
        }
        catch (Exception ex)
        {
            Response.Write("Message: " + ex.Message + "<br />Stack Trace: " + ex.StackTrace);
        }
    }

    protected void btnDownloadCSV_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dataSet = MySql.GetDataSet("sp_getCSV");
            gvFullExcelData.DataSource = dataSet;
            gvFullExcelData.DataBind();

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                ExportGridToExcel(dataSet, "AiimsDelhiExamDataCSV");
            }
        }
        catch (Exception ex)
        {
            Response.Write("Message: " + ex.Message + "<br />Stack Trace: " + ex.StackTrace);
        }
    }

    public void getExamDataDetails(string timeline)
    {
        DataSet dataSet = MySql.GetDataSet("spGet_FullExamdataDetails");

        if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
        {
            lblExamDataDetails.Text = timeline + " Uploaded Exam Date: " + Convert.ToDateTime(dataSet.Tables[0].Rows[0]["examdate"]).ToString("dd-MM-yyyy") + " & Count: " + Convert.ToString(dataSet.Tables[0].Rows[0]["totalCount"]);
        }
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
