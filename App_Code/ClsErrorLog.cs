using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Common.Class;
using System.Configuration;
/// Summary description for ClsErrorLog
/// </summary>
public class ClsErrorLog : System.Web.UI.Page
{
    CommonPerception Mysql = new CommonPerception();
    public ClsErrorLog()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public void LogError(Exception ex)
    {
        String message = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
        message += Environment.NewLine;
        message += "-----------------------------------------------------------";
        message += Environment.NewLine;
        message += string.Format("Message: {0}", ex.Message);
        message += Environment.NewLine;
        message += string.Format("StackTrace: {0}", ex.StackTrace);
        message += Environment.NewLine;
        message += string.Format("Source: {0}", ex.Source);
        message += Environment.NewLine;
        message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
        message += Environment.NewLine;
        message += "-----------------------------------------------------------";
        message += Environment.NewLine;

        Boolean bln = Mysql.ExecuteNonQuery("Exec Sp_errlog '" + Convert.ToString(message).Replace("'", "''") + "' ");
    }

    public void RegistrationStart()
    {
        if (Convert.ToString(Session["CanId"]) == null)
        {
            Session.Abandon();
            Response.Redirect("../Home/ListsofExam.aspx");
        }
        DataSet Ds = new DataSet();
        Ds = Mysql.GetDataSetWithQuery("Exec sp_RegistrationStartEndDate ");
        if (Ds.Tables[0].Rows.Count > 0 && Convert.ToString(Ds.Tables[0].Rows[0]["currentdate"]) != "")
        {
            if (Convert.ToDateTime(Ds.Tables[0].Rows[0]["currentdate"]) < Convert.ToDateTime(Ds.Tables[0].Rows[0]["RegistrationStartDate"]))
            {
                Session.Abandon();
                Response.Redirect("../Candidate/ListsofExam.aspx");
            }
        }
    }


    public void RegistrationClosed()
    {
        if (Convert.ToString(Session["CanId"]) == null)
        {
            Session.Abandon();
            Response.Redirect("../Home/ListsofExam.aspx");
        }
        DataSet Ds = new DataSet();
        Ds = Mysql.GetDataSetWithQuery("Exec sp_RegistrationStartEndDate ");
        if (Ds.Tables[0].Rows.Count > 0 && Convert.ToString(Ds.Tables[0].Rows[0]["RegistrationEndDate"]) != "")
        {
            if (Convert.ToDateTime(Ds.Tables[0].Rows[0]["currentdate"]) > Convert.ToDateTime(Ds.Tables[0].Rows[0]["RegistrationEndDate"]))
            {
                Response.Redirect("../Candidate/RegClose.aspx");
            }
        }
    }

    public void fn_AdvtClosed(string CID)
    {
        try
        {
            bool flagPayment = false;
            string dsGetAdvt = Mysql.SingleCellResultInString("select AdvertisementNo from tbabmCandidateInfo where canid='" + CID + "'");
            DataSet dsDates = Mysql.GetDataSetWithQuery("sp_getDates");

            //if(dsGetAdvt == "ADVERTISEMENT NO. 59")
            //{
            //    flagPayment = true;
            //    return;
            //}

            if (!string.IsNullOrEmpty(dsGetAdvt))
            {
                if (dsDates.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dsDates.Tables[0].Rows)
                    {
                        if (dsGetAdvt == Convert.ToString(dataRow["advtNo"]))
                        {
                            flagPayment = true;
                            return;
                        }
                        else
                        {
                            flagPayment = false;
                        }
                    }
                }
                else
                {
                    flagPayment = false;
                }

                if (!flagPayment)
                {
                    Session.Abandon();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Notify", "alert('Registration process is closed for " + dsGetAdvt + ".'); window.location='../Candidate/RegClose.aspx'", true);
                }
            }
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }

    public void PaymentClosed()
    {
        if (Convert.ToString(Session["CanId"]) == null)
        {
            Session.Abandon();
            Response.Redirect("../Home/ListsofExam.aspx");
        }
        DataSet Ds = new DataSet();
        Ds = Mysql.GetDataSetWithQuery("Exec sp_RegistrationStartEndDate ");
        if (Ds.Tables[0].Rows.Count > 0 && Convert.ToString(Ds.Tables[0].Rows[0]["PaymentEndDate"]) != "")
        {
            if (Convert.ToDateTime(Ds.Tables[0].Rows[0]["currentdate"]) > Convert.ToDateTime(Ds.Tables[0].Rows[0]["PaymentEndDate"]))
            {
                Response.Redirect("../Candidate/RegClose.aspx");
            }
        }
    }

    public void getpage(string struserid, string strpwd)
    {
        if (Convert.ToString(Session["CanId"]) == null)
        {
            Session.Abandon();
            Response.Redirect("../Home/ListsofExam.aspx");
        }
        DataSet Ds = new DataSet();
        Ds = Mysql.GetDataSetWithQuery("Exec Sp_ValidateLoginDetails  '" + struserid + "' ,'" + strpwd + "' ");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            Session["canid"] = Convert.ToString(Ds.Tables[0].Rows[0]["Canid"]);
            Session["username"] = Convert.ToString(Ds.Tables[0].Rows[0]["UserName"]);
            Session["Password"] = Convert.ToString(Ds.Tables[0].Rows[0]["Password"]);
            Session["CAT"] = Convert.ToString(Ds.Tables[0].Rows[0]["Category"]);
            Session["SUBCAT"] = Convert.ToString(Ds.Tables[0].Rows[0]["subCategory"]);

            if (Convert.ToString(Session["CAT"]) == "" && Convert.ToString(Ds.Tables[0].Rows[0]["step1"]) == "" && Convert.ToString(Ds.Tables[0].Rows[0]["step2"]) == "")
            {
                Session["CurrentPage"] = "Application.aspx";
                Response.Redirect("../Candidate/Application.aspx");
            }
            else if (Convert.ToString(Session["CAT"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["step1"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["step2"]) == "")
            {
                Session["CurrentPage"] = "Application1.aspx";
                Response.Redirect("../Candidate/Application1.aspx");
            }
            else if (Convert.ToString(Session["CAT"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["step1"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["step2"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["EduCanID"]) == "")
            {
                Session["CurrentPage"] = "Education.aspx";
                Response.Redirect("~/candidate/Education.aspx");
            }

            else if (Convert.ToString(Session["CAT"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["step1"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["step2"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["EduCanID"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["PhotoPath"]) == "")
            {
                Session["CurrentPage"] = "UploadImage.aspx";
                Response.Redirect("~/candidate/UploadImage.aspx");
            }

            else if (Convert.ToString(Session["CAT"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["step1"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["step2"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["EduCanID"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["PhotoPath"]) != "" && (Convert.ToString(Ds.Tables[0].Rows[0]["lock"]) == "" || Convert.ToString(Ds.Tables[0].Rows[0]["lock"]) == "Y"))
            {
                Session["CurrentPage"] = "preview.aspx";
                Response.Redirect("~/candidate/preview.aspx");
            }

            else if (Convert.ToString(Session["CAT"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["step1"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["step2"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["EduCanID"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["PhotoPath"]) != "" && Convert.ToString(Ds.Tables[0].Rows[0]["lock"]) == "N")
            {
                Session["CurrentPage"] = "WelcomePage.aspx";
                Response.Redirect("~/candidate/WelcomePage.aspx");
            }
            else
            {
                Session.Abandon();
                Response.Redirect("../Home/ListsofExam.aspx");
            }
        }
    }
    public void CheckPayment()
    {
        if (Convert.ToString(Session["CanId"]) == null)
        {
            Session.Abandon();
            Response.Redirect("../Home/ListsofExam.aspx");
        }
        DataSet Ds = new DataSet();
        Ds = Mysql.GetDataSetWithQuery("Exec Sp_CheckPayment @CanId = '" + Convert.ToString(Session["CanId"]) + "'");
        if (Ds.Tables[0].Rows.Count > 0)
        {
            Session.Abandon();
            Response.Redirect("../Home/ListsofExam.aspx");
        }
    }

    public static string Decrypt(string cipherString, bool useHashing)
    {
        byte[] keyArray;
        //get the byte code of the string
        cipherString = cipherString.Replace(' ', '+');
        byte[] toEncryptArray = Convert.FromBase64String(cipherString);

        System.Configuration.AppSettingsReader settingsReader =
                                            new AppSettingsReader();
        //Get your key from config file to open the lock!
        string key = (string)settingsReader.GetValue("SecurityKey",
                                                     typeof(String));

        if (useHashing)
        {
            //if hashing was used get the hash code with regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //release any resource held by the MD5CryptoServiceProvider

            hashmd5.Clear();
        }
        else
        {
            //if hashing was not implemented get the byte code of the key
            keyArray = UTF8Encoding.UTF8.GetBytes(key);
        }

        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        //set the secret key for the tripleDES algorithm
        tdes.Key = keyArray;
        //mode of operation. there are other 4 modes. 
        //We choose ECB(Electronic code Book)

        tdes.Mode = CipherMode.ECB;
        //padding mode(if any extra byte added)
        tdes.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = tdes.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(
                             toEncryptArray, 0, toEncryptArray.Length);
        //Release resources held by TripleDes Encryptor                
        tdes.Clear();
        //return the Clear decrypted TEXT
        string RetrunValue = UTF8Encoding.UTF8.GetString(resultArray);
        return RetrunValue.Replace(' ', '+');
    }
    public static string Encrypt(string toEncrypt, bool useHashing)
    {
        byte[] keyArray;
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

        System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
        // Get the key from config file

        string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
        //System.Windows.Forms.MessageBox.Show(key);
        //If hashing use get hashcode regards to your key
        if (useHashing)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //Always release the resources and flush data
            // of the Cryptographic service provide. Best Practice

            hashmd5.Clear();
        }
        else
            keyArray = UTF8Encoding.UTF8.GetBytes(key);

        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        //set the secret key for the tripleDES algorithm
        tdes.Key = keyArray;
        //mode of operation. there are other 4 modes.
        //We choose ECB(Electronic code Book)
        tdes.Mode = CipherMode.ECB;
        //padding mode(if any extra byte added)

        tdes.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = tdes.CreateEncryptor();
        //transform the specified region of bytes array to resultArray
        byte[] resultArray =
          cTransform.TransformFinalBlock(toEncryptArray, 0,
          toEncryptArray.Length);
        //Release resources held by TripleDes Encryptor
        tdes.Clear();
        //Return the encrypted data into unreadable string format
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public void IsValidUser(string EncryCanId, string PageName, ref string CanId)
    {
        try
        {
            if (String.IsNullOrEmpty(EncryCanId))
            {
                Session.Abandon();
                Response.Redirect("~/home/ListofExam.aspx");
            }

            try
            {
                CanId = Decrypt(EncryCanId, true);
            }
            catch
            {
                Session.Abandon();
                Response.Redirect("~/home/ListofExam.aspx");
            }

            if (String.IsNullOrEmpty(CanId)) //if (Convert.ToString(Session["CanId"]) == null || Convert.ToString(Session["CanId"]) == "")
            {
                Session.Abandon();
                Response.Redirect("~/home/ListofExam.aspx");
            }

            string PaymentStatus = Mysql.SingleCellResultInString("Select Lock from tbabmCandidateInfo Where Canid = '" + CanId + "'");

            if (PaymentStatus.ToUpper() == "N")
            {
                if (PageName == "Print")
                {
                    return;
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("~/home/ListofExam.aspx");
                }
            }

        }
        catch
        {
            Response.Redirect("~/home/ListofExam.aspx");
        }
    }

}