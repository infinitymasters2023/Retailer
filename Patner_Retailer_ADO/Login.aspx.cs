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
using System.Text;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.Globalization;
using PdfSharp.Pdf;
using PdfSharp.Drawing;


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
                Session.RemoveAll();
                string token = Request.QueryString["qa"];
                if(token != null && !string.IsNullOrWhiteSpace(token))
                {
                    bool flag = CheckToken(token);
                    if (!flag)
                    {
                        lblErrorMessage.Text = "Invalid Token.";
                        lblErrorMessage.Attributes.Add("style", "display:block");
                        return;
                    }
                    mobile_code.Value = Session["MobileNo1"] != null ? Session["MobileNo1"].ToString() : "";
                }
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
                    lblMessage.Text = $"OTP sent to {mobileNumber}";
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
                DateTime expiryTime = DateTime.UtcNow.AddMinutes(5);
                hdnOtpExpiry.Value = expiryTime.ToString("o");

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
            string phone = hdnPhoneNumber.Value.Replace("+91", "").Trim();

            if (string.IsNullOrEmpty(hdnPhoneNumber.Value) || hdnPhoneNumber.Value.Replace("+91", "").Length != 10)
            {
                lblErrorMessage.Text = "Please enter your phone number.";
                lblErrorMessage.Attributes.Add("style", "display:block");
                return;
            }

            if (phone.Length != 10)
            {
                lblErrorMessage.Text = "Invalid number.";
                lblErrorMessage.Attributes.Add("style", "display:block");
                return;
            }

            if (Regex.IsMatch(phone, @"^[0-5]"))
            {
                lblErrorMessage.Text = "Invalid number";
                lblErrorMessage.Attributes.Add("style", "display:block");
                return;
            }

            if (Regex.IsMatch(phone, @"^(\d)\1{9}$"))
            {
                lblErrorMessage.Text = "Invalid number.";
                lblErrorMessage.Attributes.Add("style", "display:block");
                return;
            }
            //SendWhatsApp();
            if (btnLogin.Text == "Get OTP")
            {
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 1);
                cmd.Parameters.AddWithValue("@MobileNo", hdnPhoneNumber.Value.Replace("+91", ""));
                // cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                if (con.State != ConnectionState.Open)
                    con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        Session["RetailerUniqueID"] = dr["RetailerUniqueID"];
                        Session["Name"] = dr["Name"];
                        Session["MobileNo"] = dr["MobileNo"];
                        Session["Status"] = dr["Status"];
                        Session["Role"] = dr["Role"];
                        Session["Email"] = dr["emailID"];
                        Session["SellerGSTIN"] = dr["SellerGSTINNo"];
                        string newotpValue = newotp();
                        Session["OTP"] = newotpValue;
                        Session["OTPGeneratedTime"] = DateTime.Now;
                        Response.Redirect("Dashboard.aspx");
                        //sendSMSOTP(hdnCountryCode.Value + hdnPhoneNumber.Value, newotpValue);
                        lblMessage.Text = "OTP sent successfully.";
                        btnLogin.Text = "Login";
                        divotppanel.Visible = true;
                        lblMessage.Style["color"] = "#2ec551 !important";
                        ViewState["SalerNotExist"] = "";
                    }
                    else
                    {
                        string newotpValue = newotp();
                        Session["OTP"] = newotpValue;
                        Session["OTPGeneratedTime"] = DateTime.Now;
                        Session["MobileNo"] = hdnPhoneNumber.Value.Replace("+91", "");
                        sendSMSOTP(hdnCountryCode.Value + hdnPhoneNumber.Value, newotpValue);
                        lblMessage.Text = "OTP sent successfully.";
                        btnLogin.Text = "Login";
                        divotppanel.Visible = true;
                        lblMessage.Style["color"] = "#2ec551 !important";
                        ViewState["SalerNotExist"] = "NotExist";
                    }
                }
                con.Close();

            }
            else
            {
                if (string.IsNullOrEmpty(txtOTP.Text))
                {
                    lblMessage.Text = "Please enter the OTP.";
                    lblMessage.Style["color"] = "red !important";
                    return;
                }
                if (Session[CaptchaSessionKey] == null || txtCaptcha.Text.ToLower() != Session[CaptchaSessionKey].ToString().ToLower())
                {
                    lblCaptchaError.Text = "Invalid Captcha.";
                    GenerateCaptcha();
                    return;
                }
                string enteredOtp = txtOTP.Text.Trim();
                string sessionOtp = Session["OTP"] as string;
                DateTime? otpTime = Session["OTPGeneratedTime"] as DateTime?;

                if (sessionOtp == null || otpTime == null)
                {
                    lblMessage.Text = "OTP has expired or not generated.";
                    lblMessage.Style["color"] = "red !important";
                    return;
                }

                TimeSpan timeElapsed = DateTime.Now - otpTime.Value;
                if (timeElapsed.TotalMinutes > 5)
                {
                    lblMessage.Text = "OTP expired. Please request a new one.";
                    lblMessage.Style["color"] = "red !important";
                    divotppanel.Visible = false;
                    return;
                }
                if (ViewState["SalerNotExist"].ToString() == "NotExist" && Session["OTP"] != null && txtOTP.Text == Session["OTP"].ToString())
                {
                    string token = Request.QueryString["qa"];
                    if (token != null && !string.IsNullOrWhiteSpace(token))
                    {
                        string mobile = Session["MobileNo1"] != null ? Session["MobileNo1"].ToString() : "";
                        if (!string.IsNullOrWhiteSpace(mobile) && mobile == hdnPhoneNumber.Value.Replace("+91", ""))
                        {
                            Session["Message"] = "Mobile number not registered. Please register to continue.";
                            string LoginAttemptType = "Mobile number not registered.";
                            string Status = "Success";
                            CreateLog(LoginAttemptType, Status);
                            Response.Redirect("SellerGSTIN.aspx?qa=" + token);
                        }
                        else
                        {
                            Session["Message"] = "Mobile number not registered. Please register to continue.";
                            lblErrorMessage.Text = "The mobile number associated with this link does not match your registered number.";
                            lblErrorMessage.Attributes.Add("style", "display:block");
                            string LoginAttemptType = "The mobile number associated with this link does not match your registered number.";
                            string Status = "Failed";
                            CreateLog(LoginAttemptType, Status);
                            Response.Redirect("SellerGSTIN.aspx?qa=" + token);
                        }
                    }
                    else
                    {
                        Session["Message"] = "Mobile number not registered. Please register to continue.";
                        bool checkByMobile = CheckRetailerByMobile();
                        if(checkByMobile)
                        {
                            string LoginAttemptType = "Retailer does not login with the Invitation link.";
                            string Status = "Failed";
                            token = Session["token"] != null ? Session["token"].ToString() : "";
                            CreateLog(LoginAttemptType, Status);
                            if(!string.IsNullOrWhiteSpace(token))
                                Response.Redirect("SellerGSTIN.aspx?qa=" + token);
                        }
                        Response.Redirect("SellerGSTIN.aspx");
                    }
                }
                else if (Session["OTP"] != null && txtOTP.Text == Session["OTP"].ToString())
                {
                    if (Session["Status"].ToString() == "1" || Session["Status"].ToString() == "2")
                    {
                        Response.Redirect("CreateAnAccount.aspx");
                    }
                    else if(Session["Status"].ToString() == "6")
                    {
                        lblMessage.Text = "Unfortunately, your application has been rejected due to incomplete or invalid information.";
                        lblMessage.Style["color"] = "red !important";
                    }
                    else if (Session["Status"].ToString() == "8")
                    {
                        lblMessage.Text = "Your application has been terminated due to non-compliance or failure to meet the required criteria.";
                        lblMessage.Style["color"] = "red !important";
                    }
                    else if (Session["Status"].ToString() == "9")
                    {
                        lblMessage.Text = "Your request to withdraw your application has been approved successfully.";
                        lblMessage.Style["color"] = "red !important";
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(hdnPhoneNumber.Value, false);
                        Response.Redirect("Dashboard.aspx");
                    }
                }
                else
                {
                    lblMessage.Text = "Invalid OTP.";
                    lblMessage.Style["color"] = "red !important";
                }

                Session["OTP"] = null;
            }
        }

        protected void SendWhatsApp()
        {
            string mobile = hdnPhoneNumber.Value;
            string name = "Amaan";
            string pincode = "110094";
            string city = "Delhi";
            string state = "New Delhi";
            string area = "Old Mustafabad";
            string dealerType = "Retailer";
            string company = "ABC";

            SendWhatsappMessage(mobile, name, pincode, city, state, area, dealerType, company);
        }

        private void SendWhatsappMessage(string mobile, string name, string pincode, string city, string state, string area, string dealerType, string company)
        {
            try
            {
                string url = "https://backend.api-wa.co/campaign/smartping/api/v2";
                string fullMobile = "91" + mobile;

                string jsonData = @"{
            ""apiKey"": ""eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjY3MWY1YWUwMTg3OWFjMGJlY2EyZmQ3ZSIsIm5hbWUiOiJJbmZ5U2hpZWxkIiwiYXBwTmFtZSI6IkFpU2Vuc3kiLCJjbGllbnRJZCI6IjY1OTNmZGI3MDBmODRmMzczMjNiODE5OCIsImlhdCI6MTczMDEwODEyOH0.LS_Trirwhav9NV-Vdp0F4MdkUU1C2f56h7ngUzdWqGU"",
            ""campaignName"": ""partner_acknowledgement"",
            ""destination"": """ + fullMobile + @""",
            ""userName"": """ + name + @""",
            ""templateParams"": [
                """ + name + @""",
                """ + mobile + @""",
                """ + pincode + @""",
                """ + city + @""",
                """ + state + @""",
                """ + area + @""",
                """ + dealerType + @""",
                """ + company + @"""
            ],
            ""source"": ""new-landing-page form"",
            ""media"": {},
            ""buttons"": [],
            ""carouselCards"": [],
            ""location"": {},
            ""attributes"": {}
        }";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";

                byte[] data = Encoding.UTF8.GetBytes(jsonData);
                request.ContentLength = data.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string result = reader.ReadToEnd();
                        Response.Write("<pre>" + Server.HtmlEncode(result) + "</pre>");
                    }
                }
            }
            catch (WebException ex)
            {
                using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    string error = reader.ReadToEnd();
                    Response.Write("<pre>Error: " + Server.HtmlEncode(error) + "</pre>");
                }
            }
        }

        protected bool CheckToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;
         
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 88);
                cmd.Parameters.AddWithValue("@token", token);
                if (con.State != ConnectionState.Open)
                    con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string mobile = reader["MobileNo1"].ToString();
                        string InvitationId = reader["mid"].ToString();
                        Session["MobileNo1"] = mobile;
                        Session["InvitationId"] = InvitationId;
                        return true;
                    }
                }
                con.Close();
            }

            return false;
        }
        protected void CreateLog(string LoginAttemptType, string Status)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@type", 89);
                    cmd.Parameters.AddWithValue("@InvitationId", Session["InvitationId"] != null ? Session["InvitationId"].ToString() : "0");
                    cmd.Parameters.AddWithValue("@InvitationMobileNo", Session["MobileNo1"] != null ? Session["MobileNo1"].ToString() : null);
                    cmd.Parameters.AddWithValue("@LoginMobileNo", Session["MobileNo"] != null ? Session["MobileNo"].ToString() : null);
                    cmd.Parameters.AddWithValue("@LoginAttemptType", LoginAttemptType);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@CreatedBy", Session["MobileNo"] != null ? Session["MobileNo"].ToString() : null);

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        protected bool CheckRetailerByMobile()
        {
            if (Session["MobileNo"] == null || string.IsNullOrWhiteSpace(Session["MobileNo"].ToString()))
                return false;

            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 91);
                cmd.Parameters.AddWithValue("@mobileno", Session["MobileNo"] != null ? Session["MobileNo"].ToString() : "");

                if (con.State != ConnectionState.Open)
                    con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string mobile = reader["MobileNo1"].ToString();
                        string token = reader["UniqueToken"].ToString();
                        Session["MobileNo1"] = mobile;
                        Session["token"] = token;
                        return true;
                    }
                }
                con.Close();
            }

            return false;
        }
        public void GenerateReceipt(string filePath, string amount, string bankTxnId, string paytmTxnId, DateTime txnDate, string orderRef, string ticketNo, string customerName, string iprnNo)
        {
            // Map image paths from server
            string leftImagePath = HttpContext.Current.Server.MapPath("~/assets/images/infinity-logo.png");
            string rightImagePath = HttpContext.Current.Server.MapPath("~/assets/images/Infyshield-logo.png");

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Payment Receipt";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Fonts
            XFont headerFont = new XFont("Arial", 16, XFontStyle.Bold);
            XFont normalFont = new XFont("Arial", 12, XFontStyle.Regular);
            XFont smallFont = new XFont("Arial", 9, XFontStyle.Regular); // smaller font for footer
            XFont italicFont = new XFont("Arial", 10, XFontStyle.Italic);

            // Colors
            XColor mainColor = XColor.FromArgb(0x01, 0x18, 0x93); // #011893
            XColor greenColor = XColor.FromArgb(0, 128, 0); // green for contact info

            // Draw logos
            if (File.Exists(leftImagePath))
            {
                XImage leftImage = XImage.FromFile(leftImagePath);
                gfx.DrawImage(leftImage, 40, 30, 120, 40); // adjust size as needed
            }

            if (File.Exists(rightImagePath))
            {
                XImage rightImage = XImage.FromFile(rightImagePath);
                gfx.DrawImage(rightImage, page.Width - 160, 20, 120, 120); // adjust size
            }

            double y = 150; // Start below logos

            // Title
            gfx.DrawString("RECEIPT", headerFont, new XSolidBrush(mainColor), new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);
            y += 40;
            // Fonts
            XFont labelFont = new XFont("Arial", 10, XFontStyle.Regular);
            XFont valueFont = new XFont("Arial", 10, XFontStyle.Bold); // regular values
            double lineSpacing = 25; // space between lines

       
            gfx.DrawString("We have received acknowledgement from PayTm for payment of following charges", labelFont, XBrushes.Black, new XRect(60, y, page.Width - 80, lineSpacing), XStringFormats.TopLeft);
            y += lineSpacing + 5;

            // Payment details as bold label + regular value
            void DrawDetail(string label, string value)
            {
                gfx.DrawString("•", labelFont, XBrushes.Black, 70, y);
                gfx.DrawString($"{label} - ", labelFont, XBrushes.Black, 90, y);
                gfx.DrawString(value, valueFont, XBrushes.Black, 90 + 130, y);
                y += lineSpacing;
            }

            // Example usage
            DrawDetail("Amount", $"Rs. {amount} (Rupees) via UPI");
            DrawDetail("Bank Transaction ID", bankTxnId);
            DrawDetail("Paytm Transaction ID", paytmTxnId);
            DrawDetail("Transaction Date", $"{txnDate:dd/MMM/yyyy hh:mm tt}");
            DrawDetail("Our Order Reference No.", orderRef);
            DrawDetail("Reference Ticket No.", $"{ticketNo} ({customerName})");

          
          
            XFont footerTitleFont = new XFont("Arial", 18, XFontStyle.Bold);
            // Footer (at bottom of page)
            double pageHeight = page.Height;
            double footerY = pageHeight - 80;

            gfx.DrawString($"(Infinity Receipt No.{iprnNo} dated {txnDate:dd MMM yyyy} for Internal Office Use)", italicFont, XBrushes.Black, new XRect(0, footerY, page.Width, 20), XStringFormats.TopCenter);

            footerY += 18;

            // Horizontal line
            gfx.DrawLine(new XPen(mainColor, 1), 40, footerY, page.Width - 40, footerY);
            footerY += 18;

            // Company name
            gfx.DrawString("Infinity Assurance Solutions Pvt. Ltd.", footerTitleFont, new XSolidBrush(mainColor),
                           new XRect(0, footerY, page.Width, 20), XStringFormats.TopCenter);
            footerY += 22;

            // Address
            gfx.DrawString("Regd. Office: 24, US Complex, Adjacent to Jasola Apollo Metro Station, 120, Mathura Road, New Delhi 110 076",
                           smallFont, new XSolidBrush(mainColor), new XRect(0, footerY, page.Width, 15), XStringFormats.TopCenter);
            footerY += 15;

            // Contact info
    /*        gfx.DrawString("Future Generali Claims: Tel: +91 8447 88 2424    email: claims.fg@infinityassurance.com",
                           smallFont, new XSolidBrush(greenColor), new XRect(0, footerY, page.Width, 15), XStringFormats.TopCenter);
            footerY += 15;

            gfx.DrawString("Tel: +91 8010 11 2277    email: contact@infinityassurance.com",
                           smallFont, new XSolidBrush(greenColor), new XRect(0, footerY, page.Width, 15), XStringFormats.TopCenter);
            footerY += 15;

            gfx.DrawString("Web: www.infinityassurance.com    www.infyshield.com",
                           smallFont, new XSolidBrush(greenColor), new XRect(0, footerY, page.Width, 15), XStringFormats.TopCenter);*/

            // Save PDF
            document.Save(filePath);
        }
       
        private string SanitizeFileName(string fileName)
        {
            string pattern = "[^a-zA-Z0-9-_\\. ]";
            string sanitizedFileName = Regex.Replace(fileName, pattern, "");

            return sanitizedFileName;
        }

    }
}
