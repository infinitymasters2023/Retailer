using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace Patner_Retailer_ADO
{
    public partial class BuyInfySalePlan : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iaplConnectionString"].ConnectionString);
        static public void DisplayMessage(Control page, string msg)
        {
            string msg1 = String.Format("alert('{0}');", msg);
            ScriptManager.RegisterStartupScript(page, page.GetType(), "msg", msg1, true);
        }
        private const int MaxOTPAttempts = 3;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MobileNo"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                CalendarExtender3.EndDate = DateTime.Today;
                CalendarExtender1.EndDate = DateTime.Today;

                BindSubCategory();
                bindsubcatg();
                getallBrand();
                bindSerielNo();

                PlanPanel.Visible = false;
                ApplyPromoCodePanel.Visible = false;
                txtPurchaseDate.Attributes.Add("ReadOnly", "readonly");
                txtDateOfImpl.Attributes.Add("ReadOnly", "readonly");
                btnEditPlan.Visible = false;
            }
        }

        protected void BindSubCategory()
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 1);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    ddlsubcatg.DataSource = dt;
                    ddlsubcatg.DataValueField = "subcatid";
                    ddlsubcatg.DataTextField = "description";
                    ddlsubcatg.DataBind();

                    ddlsubcatg.Items.Insert(0, new ListItem("Select Product", "0"));

                    ListItem item = ddlsubcatg.Items.FindByValue("2");
                    if (item != null)
                    {
                        ddlsubcatg.SelectedValue = "2";
                    }
                }
            }
            con.Close();
        }
        protected void ddlsubcatg_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            bindsubcatg();
            bindSerielNo();
            //HideOtherPanels();
            ddlProductType.Focus();
        }

        public void bindsubcatg()
        {
            try
            {
                SqlDataAdapter adp = new SqlDataAdapter("sp_iapl_PartnerRetailer", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@type", SqlDbType.Int).Value = 2;
                adp.SelectCommand.Parameters.Add("@description", SqlDbType.NVarChar).Value = ddlsubcatg.SelectedItem.Text;
                con.Open();
                DataSet ds = new DataSet();
                adp.Fill(ds);
                ddlProductType.DataSource = ds.Tables[0];
                ddlProductType.DataTextField = "subcategoryname";
                ddlProductType.DataValueField = "subcategoryid";
                ddlProductType.DataBind();
                ddlProductType.Items.Insert(0, new ListItem("Select Product Type", "0"));

                con.Close();
            }
            catch (SqlException se)
            {
                DisplayMessage(this, "" + se.Message + "");
                return;
            }
        }

        protected void ddlProductType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            txtMake.Text = "";
            getallBrand();
            //HideOtherPanels();
            txtPrice.Focus();
        }

        protected void getallBrand()
        {
            SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 3;
            cmd.Parameters.AddWithValue("@subcatgId", ddlProductType.SelectedValue);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            ddlBrand.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {                
                txtMake.Visible = false;
                ddlBrand.Visible = true;
                string s = Convert.ToString(ds.Tables[0].Rows.Count + 1);
                ddlBrand.DataTextField = "Brand";
                ddlBrand.DataValueField = "Brand";
                ddlBrand.DataSource = ds.Tables[0];
                ddlBrand.DataBind();
                ddlBrand.Items.Insert(0, new ListItem("Select Make", string.Empty));
                ddlBrand.Items.Add(new ListItem("My Brand is not in the List", s));
            }
            else
            {
                ddlBrand.Items.Insert(0, new ListItem("Select Make", "0"));
            }
            con.Close();
        }
        private void BindPlanData()
        {
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 4);
                cmd.Parameters.AddWithValue("@ProductType", ddlProductType.SelectedValue);
                cmd.Parameters.AddWithValue("@ProductSubCat", ddlsubcatg.SelectedValue);
                cmd.Parameters.AddWithValue("@ProductPrice", txtPrice.Text.ToString());

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    pnlNoPlans.Visible = false;
                    rptPlans.DataSource = dt;
                    rptPlans.DataBind();
                }
                else
                {
                    pnlNoPlans.Visible = true;
                    rptPlans.DataSource = null;
                    rptPlans.DataBind();
                }
            }
        }
        protected void AddToCart(object sender, EventArgs e)
        {
            string planName = string.Empty;
            string planPrice = string.Empty;
            string SKU = string.Empty;
            foreach (RepeaterItem item in rptPlans.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                if (chkSelect.Checked)
                {
                    planName = ((Label)item.FindControl("lblPlanName"))?.Text ?? string.Empty;
                    planPrice = ((HiddenField)item.FindControl("hdnPlanPrice")).Value;
                    SKU = ((HiddenField)item.FindControl("hdnSKU")).Value;
                }
            }

            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@type", 5);
                cmd.Parameters.AddWithValue("@Saleschannel", "Retailer");
                cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"].ToString());
                cmd.Parameters.AddWithValue("@ProductSubCat", ddlsubcatg.SelectedValue);
                cmd.Parameters.AddWithValue("@Productname", ddlProductType.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@ProductType", ddlProductType.SelectedValue);
                cmd.Parameters.AddWithValue("@Productsubcategoryname", ddlsubcatg.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@Brand", ddlBrand.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@ModalName", txtModel.Text.ToString());
                cmd.Parameters.AddWithValue("@imei", txtSerialNo.Text.ToString());
                cmd.Parameters.AddWithValue("@serialno", txtSerialNo.Text.ToString());
                cmd.Parameters.AddWithValue("@DevicePurchasePrice", !string.IsNullOrWhiteSpace(txtPrice.Text) ? Convert.ToDecimal(txtPrice.Text.ToString()) : 0);
                cmd.Parameters.AddWithValue("@ProductPurchaseDate", !string.IsNullOrWhiteSpace(txtPurchaseDate.Text) ? Convert.ToDateTime(txtPurchaseDate.Text.ToString()) : DateTime.Now);
                cmd.Parameters.AddWithValue("@CustomerEmailID", txtCustomerEmail.Text.ToString());
                cmd.Parameters.AddWithValue("@CustomerMobileNo", txtCustomerMobile.Text.ToString());
                cmd.Parameters.AddWithValue("@PlanName", planName);
                cmd.Parameters.AddWithValue("@PlanPrice", !string.IsNullOrWhiteSpace(planPrice) ? Convert.ToDecimal(planPrice) : 0);
                cmd.Parameters.AddWithValue("@Promocode", txtPromoDiscount.Value.ToString());
                cmd.Parameters.AddWithValue("@PromoDiscount", !string.IsNullOrWhiteSpace(txtPromoDiscount.Value) ? Convert.ToDecimal(txtPromoDiscount.Value.ToString()) : 0);
                cmd.Parameters.AddWithValue("@DateofImplementation", !string.IsNullOrWhiteSpace(txtDateOfImpl.Text) ? Convert.ToDateTime(txtDateOfImpl.Text.ToString()) : DateTime.Now);
                cmd.Parameters.AddWithValue("@ManufacturerWarranty_yymmdd", validationCustom09.Text.ToString() + "/" + validationCustom010.Text.ToString() + "/" + validationCustom011.Text.ToString());
                cmd.Parameters.AddWithValue("@PlanSKU", SKU);
                cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"] != null ? Session["salesOrderID"].ToString() : null);
                cmd.Parameters.AddWithValue("@Customer_OrderID", Session["Customerorder"] != null ? Session["Customerorder"].ToString() : null);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string salesOrderID = reader["salesorderid"].ToString();
                        string Customerorder = reader["Customerorder"].ToString();
                        Session["salesOrderID"] = salesOrderID;
                        Session["Customerorder"] = Customerorder;
                        Response.Redirect("CartDetails.aspx?salesorder=" + salesOrderID);
                    }
                }
            }
            con.Close();
            Response.Redirect("CartDetails.aspx");
        }

        protected void SubmitPlanInfo(object sender, EventArgs e)
        {

            int count = 0;
            if (ddlsubcatg.SelectedValue == "0")
            {
                ddlsubcatg.BorderColor = System.Drawing.Color.Red;
                count = count + 1;
            }
            else
                ddlsubcatg.BorderColor = System.Drawing.Color.LightGray;

            if (ddlProductType.SelectedValue == "0")
            {
                ddlProductType.BorderColor = System.Drawing.Color.Red;
                count = count + 1;
            }
            else
                ddlProductType.BorderColor = System.Drawing.Color.LightGray;
            if (string.IsNullOrWhiteSpace(ddlBrand.SelectedValue) || ddlBrand.SelectedValue == "0")
            {
                ddlBrand.BorderColor = System.Drawing.Color.Red;
                count = count + 1;
            }
            else
                ddlBrand.BorderColor = System.Drawing.Color.LightGray;
            if (count > 0)
            {
                return;
            }

            if (txtCustomerMobile.Text != "")
            {
                string newopt = newotp();
                Session.Remove("OTP");
                Session["OTP"] = newopt;
                sendSMSOTP(txtCustomerMobile.Text.Trim(), Session["otp"].ToString());
                btnSubmitPlan.Visible = false;
                OTPPanel.Visible = true;
                btnSubmitOTP.Visible = true;
                txtOTP.Value = null;
                Session["OTPAttemptSessionKey"] = 0;

                btnEditPlan.Visible = true;
                ddlsubcatg.Enabled = false;
                ddlProductType.Enabled = false;
                txtPrice.Enabled = false;                
                txtPurchaseDate.Enabled = false;
                ddlBrand.Enabled = false;
                txtModel.Enabled = false;
                validationCustom06.Enabled = false;
                txtSerialNo.Enabled = false;
                txtimeiNo.Enabled = false;
                txtDateOfImpl.Enabled = false;
                validationCustom09.Enabled = false;
                validationCustom010.Enabled = false;
                validationCustom011.Enabled = false;
                txtCustomerEmail.Enabled = false;
                txtCustomerMobile.Enabled = false;

                txtOTP.Focus();
            }
        }

        protected void lnkResendOTP_Click(object sender, EventArgs e)
        {
            int attempts = (int)Session["OTPAttemptSessionKey"];
            if (attempts < MaxOTPAttempts)
            {
                if (!string.IsNullOrEmpty(txtCustomerMobile.Text))
                {
                    string generatedOTP = newotp();
                    Session["OTP"] = generatedOTP;
                    lblMessage.Text = $"OTP sent to {txtCustomerMobile.Text} ";
                    sendSMSOTP(txtCustomerMobile.Text, generatedOTP);
                    Session["OTPAttemptSessionKey"] = attempts + 1;
                    txtOTP.Focus();
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

        protected string newotp()
        {
            string otp1 = "";
            string numbers = "1234567890";
            string characters = numbers;
            int length = 6;
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            otp1 = otp;
            return otp1;
        }

        protected void sendSMSOTP(string mobileno, string otp)
        {
            try
            {
                string responseString = "";
                string message = "Welcome to Infinity, Your OTP to Login to Infinity TechCare Lounge is " + otp + ". For Help, Call Infinity 8447882424. 9AM-6PM Mon-Sat"; //"Your OTP is : " + hiddenString + "";
                string content_temID = "1107162426891569578";
                string no = mobileno;
                string sender12 = "ISHILD";

                // string url1 = "https://msg.wemonde.com/api/sendSMS?token=f2fdee93271556e428dd9507b3da7235&senderid=" + sender12 + "&route=14&number=" + no + "&message=" + message + "&contentID=" + content_temID;
                string url1 = "https://api.mobilnxt.in/api/push?accesskey=uW9h2HHRlctDRlGwOQKEicLgsgBi2V&to=" + no + "&text=" + message + "&from=" + sender12 + "&tid=" + content_temID;

                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url1);
                HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
                StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                responseString = respStreamReader.ReadToEnd();
                string s1 = responseString.Substring(0, 17);
                respStreamReader.Close();
                myResp.Close();
            }
            catch (Exception e1)
            {
                DisplayMessage(this, e1.Message);
                return;
            }
        }
        public void sendotpmail(string email, string otp)
        {
            try
            {
                Session["email"] = email;
                string hiddenString = otp.Substring(0, 3) + "***";
                string s = Server.MapPath("infySign.png");
                string MSG1 = "<table align='left' cellpadding='5' cellspacing='1' style='width:100%; '><tr><td style='font-size: 14px;text-align: justify; font-family: Arial;' colspan='2'>Dear Customer," + "\t\t" +
                "</td></tr>" +
                  "<tr><td style='font-size: 14px;text-align: justify; font-family: Arial;' colspan='2'>" + "\t\t<b><u>" + hiddenString + "</b></u> is your OTP. " + "</td></tr>" +
                "<tr><td style='font-size: 14px;text-align: justify; font-family: Arial;' colspan='2'>Thanking you,<br/><br/>Team Infinity<br/>InfyShield<br/><br/><img src=cid:companylogo></td></tr></table>";

                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress("no-reply@infinityassurance.com");
                Msg.To.Add(email.ToString());
                Msg.Subject = "Welcome to InfyShield - Your OTP is here";
                Msg.IsBodyHtml = true;
                Msg.Body = MSG1;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("no-reply@infinityassurance.com", "mlas jsej cdzd fmdc");
                smtp.EnableSsl = true;
                smtp.Send(Msg);
                Msg = null;
            }
            catch (Exception ex)
            {
                DisplayMessage(this, "" + ex.Message.ToString() + "");
                return;
            }
        }

        protected void SubmitOTP(object sender, EventArgs e)
        {
            if (txtOTP.Value.Trim() != null)
            {
                if (txtOTP.Value.Trim() == Session["OTP"].ToString())
                {
                    PlanPanel.Visible = true;
                    btnSubmitOTP.Visible = false;
                    lblMessage.Visible = false;
                    lnkResendOTP.Visible = false;
                    BindPlanData();

                    if (rptPlans.Items.Count > 0)
                    {
                        CheckBox chkFirst = (CheckBox)rptPlans.Items[0].FindControl("chkSelect");
                        if (chkFirst != null)
                        {
                            chkFirst.Focus();
                        }
                    }
                }
            }
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            ApplyPromoCodePanel.Visible = true;
            txtPromoDiscount.Focus();
        }

        protected void bindSerielNo()
        {
            if (ddlsubcatg.SelectedValue == "2")
            {
                txtimeiNo.Enabled = true;
                txtSerialNo.Enabled = false;
                txtDateOfImpl.Enabled = false;
                txtDateOfImpl.Text = null;
                txtSerialNo.Text = null;

                rfvIMEINo.Enabled = true;
                rfvSerialNo.Enabled = false;
                rfvDateOfImpl.Enabled = false;
            }
            else
            {
                txtimeiNo.Enabled = false;
                txtSerialNo.Enabled = true;
                txtDateOfImpl.Enabled = true;
                txtimeiNo.Text = null;

                rfvIMEINo.Enabled = false;
                rfvSerialNo.Enabled = true;
                rfvDateOfImpl.Enabled = true;
            }
        }      

        protected void EditPlanInfo(object sender, EventArgs e)
        {
            btnEditPlan.Visible = false;
            btnSubmitPlan.Visible = true;
            ddlsubcatg.Enabled = true;
            ddlProductType.Enabled = true;
            txtPrice.Enabled = true;
            txtPurchaseDate.Enabled = true;
            ddlBrand.Enabled = true;
            txtModel.Enabled = true;
            validationCustom06.Enabled = true;                        
            validationCustom09.Enabled = true;
            validationCustom010.Enabled = true;
            validationCustom011.Enabled = true;
            txtCustomerEmail.Enabled = true;
            txtCustomerMobile.Enabled = true;
            HideOtherPanels();
            bindSerielNo();
        }

        protected void HideOtherPanels()
        {
            PlanPanel.Visible = false;
            OTPPanel.Visible = false;
            ApplyPromoCodePanel.Visible = false;
            btnSubmitPlan.Visible = true;
        }
    }
}