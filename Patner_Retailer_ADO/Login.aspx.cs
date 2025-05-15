using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.Security;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Random = System.Random;
using System.Net;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Data;

namespace Patner_Retailer_ADO
{
    public partial class Login : Page
    {
        private const string CaptchaSessionKey = "CaptchaCode";
        private const string OTPAttemptSessionKey = "OTPAttempts";
        private const int MaxOTPAttempts = 3;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iaplConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateCaptcha();
                Session[OTPAttemptSessionKey] = 0;
            }
        }

        private void GenerateCaptcha()
        {
            Random random = new Random();
            string captchaText = "";
            for (int i = 0; i < 6; i++)
            {
                int charType = random.Next(3);
                switch (charType)
                {
                    case 0: captchaText += random.Next(0, 10).ToString(); break;
                    case 1: captchaText += (char)random.Next(65, 91); break;
                    case 2: captchaText += (char)random.Next(97, 123); break;
                }
            }
            lblCaptcha.Text = captchaText;
            Session[CaptchaSessionKey] = captchaText;
        }

        protected void lnkRefreshCaptcha_Click(object sender, EventArgs e)
        {
            GenerateCaptcha();
        }

        protected void lnkResendOTP_Click(object sender, EventArgs e)
        {
            int attempts = (int)Session[OTPAttemptSessionKey];
            if (attempts < MaxOTPAttempts)
            {
                string mobileNumber = hdnCountryCode.Value + hdnPhoneNumber.Value;
                if (!string.IsNullOrEmpty(mobileNumber))
                {
                    string generatedOTP = newotp();
                    Session["OTP"] = generatedOTP;
                    lblMessage.Text = $"OTP sent to {mobileNumber} (for demo: {generatedOTP})";
                    sendSMSOTP(mobileNumber, generatedOTP);
                    Session[OTPAttemptSessionKey] = attempts + 1;
                }
                else
                {
                    lblMessage.Text = "Please enter a valid phone number.";
                }
            }
            else
            {
                lblMessage.Text = "Too many OTP resend attempts. Please try again later.";
                lnkResendOTP.Enabled = false;
            }
        }

        private string newotp()
        {
            string numbers = "1234567890";
            string otp = "";
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
                otp += numbers[rand.Next(numbers.Length)];
            return otp;
        }

        protected void sendSMSOTP(string mobileno, string otp)
        {
            try
            {
                string message = "Welcome to Infinity, Your OTP to Login to Infinity TechCare Lounge is " + otp + ". For Help, Call Infinity 8447882424. 9AM-6PM Mon-Sat";
                string content_temID = "1107162426891569578";
                string sender12 = "ISHILD";

                string url = $"https://api.mobilnxt.in/api/push?accesskey=uW9h2HHRlctDRlGwOQKEicLgsgBi2V&to={mobileno}&text={message}&from={sender12}&tid={content_temID}";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
                StreamReader respStreamReader = new StreamReader(myResp.GetResponseStream());
                string responseString = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                myResp.Close();
            }
            catch (Exception) { return; }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hdnPhoneNumber.Value))
            {
                lblMessage.Text = "Please enter your phone number.";
                return;
            }

            if (btnLogin.Text == "Get OTP")
            {
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 1);
                cmd.Parameters.AddWithValue("@MobileNo", hdnPhoneNumber.Value.Replace("+91",""));
              // cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        Session["RetailerUniqueID"] = dr["RetailerUniqueID"];
                        Session["Name"] = dr["Name"];
                        Session["MobileNo"] = dr["MobileNo"];
                        Session["Role"] = dr["Role"];
                        string newotpValue = newotp();
                        Session["OTP"] = newotpValue;
                        sendSMSOTP(hdnCountryCode.Value + hdnPhoneNumber.Value, newotpValue);
                        lblMessage.Text = "OTP sent successfully.";
                        btnLogin.Text = "Login";
                        divotppanel.Visible = true;

                       // Response.Redirect("Dashboard.aspx");
                    }
                    else
                    {
                        lblMessage.Text = "Invalid MobileNo.";
                    }
                }
               
            }
            else
            {
                if (string.IsNullOrEmpty(txtOTP.Text))
                {
                    lblMessage.Text = "Please enter the OTP.";
                    return;
                }

                if (Session[CaptchaSessionKey] == null || txtCaptcha.Text.ToLower() != Session[CaptchaSessionKey].ToString().ToLower())
                {
                    lblMessage.Text = "Invalid Captcha.";
                    GenerateCaptcha();
                    return;
                }

                if (Session["OTP"] != null && txtOTP.Text == Session["OTP"].ToString())
                {
                    FormsAuthentication.SetAuthCookie(hdnPhoneNumber.Value, false);
                    Response.Redirect("Dashboard.aspx");
                }
                else
                {
                    lblMessage.Text = "Invalid OTP.";
                }

                Session["OTP"] = null;
            }
        }
    }
}
