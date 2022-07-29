using Common.Class;
using System;
using System.Data;
using System.Web.UI;

public partial class admin_login : ClsErrorLog
{
    CommonPerception MySql = new CommonPerception();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                Session.Abandon();
            }
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (txtUserName.Text != "" && txtPassword.Text != "")
        {
            DataSet dsAdmin = MySql.GetDataSet("sp_getadminMaster @userName = '" + txtUserName.Text + "', @password = '" + txtPassword.Text + "'");
            if (dsAdmin != null && dsAdmin.Tables[0].Rows.Count > 0)
            {
                Session["UserName"] = txtUserName.Text.ToString();
                Response.Redirect("dashboard.aspx");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Please enter valid credentials.');", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Please enter credentials.');", true);
        }
    }
}