using Common.Class;
using System;
using System.Data;

public partial class admin_adminMaster : System.Web.UI.MasterPage
{
    CommonPerception MySql = new CommonPerception();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
        {
            lblUserName.Text = Convert.ToString(Session["UserName"]);

            getSteps();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("login.aspx");
        }
    }

    public void getSteps()
    {
        DataSet dataSet = MySql.GetDataSet("spGet_steps");
        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
        {
            step2.Visible = false;
            step3.Visible = false;
            step4.Visible = false;

            if (Convert.ToInt32(dataSet.Tables[0].Rows[0]["step2"]) > 0 && Convert.ToInt32(dataSet.Tables[0].Rows[0]["step3"]) > 0 &&
                Convert.ToInt32(dataSet.Tables[0].Rows[0]["step4"]) > 0)
            {
                step2.Visible = true;
                step3.Visible = true;
                step4.Visible = true;
            }
            else if (Convert.ToInt32(dataSet.Tables[0].Rows[0]["step2"]) > 0 && Convert.ToInt32(dataSet.Tables[0].Rows[0]["step3"]) > 0 &&
                Convert.ToInt32(dataSet.Tables[0].Rows[0]["step4"]) == 0)
            {
                step2.Visible = true;
                step3.Visible = true;
            }
            else if (Convert.ToInt32(dataSet.Tables[0].Rows[0]["step2"]) > 0 && Convert.ToInt32(dataSet.Tables[0].Rows[0]["step3"]) == 0 &&
                Convert.ToInt32(dataSet.Tables[0].Rows[0]["step4"]) == 0)
            {
                step2.Visible = true;
            }
        }
    }
}
