using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Globalization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace Patner_Retailer_ADO
{
    public partial class RetailerBuyInfySalePlan : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                string encoded = Request.QueryString["qu"];
                string dd = Request.QueryString["dd"];
                string ed = Request.QueryString["ed"];
                if (!string.IsNullOrEmpty(encoded))
                {
                    string decodedEndDate = Encoding.UTF8.GetString(Convert.FromBase64String(ed));
                    if (!string.IsNullOrWhiteSpace(decodedEndDate))
                    {
                        DateTime linkEndDate = DateTime.ParseExact(decodedEndDate.Trim(), "dd-MMM-yyyy' 'HH:mm", CultureInfo.InvariantCulture);
                        DateTime currentTime = DateTime.Now;
                        if (currentTime > linkEndDate)
                        {
                            string script = $@"
                            <script type='text/javascript'>
                                alert('Link has expired. Please contact your supervisor!');
                                window.location.href = 'InvalidLink.aspx';
                            </script>";

                            ClientScript.RegisterStartupScript(this.GetType(), "expiredRedirect", script);
                            return;
                        }
                    }
                    string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
                    string decodeddd = Encoding.UTF8.GetString(Convert.FromBase64String(dd));
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
                    if (Session["salesOrderID"] != null)
                    {
                        txtCustomerName.Text = Session["CustomerName"].ToString();
                        txtCustomerEmail.Text = Session["CustomerEmailId"].ToString();
                        txtCustomerMobile.Text = Session["CustomerMobileNo"].ToString();

                        txtCustomerName.Enabled = false;
                        txtCustomerEmail.Enabled = false;
                        txtCustomerMobile.Enabled = false;
                    }
                }
                else
                {
                    string script = $@"
                            <script type='text/javascript'>
                                alert('Link is not valid. Please contact your supervisor!');
                                window.location.href = 'InvalidLink.aspx';
                            </script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "expiredRedirect", script);
                    return;
                }

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
            if (invalidCheck.Checked == false)
            {
                lblErrorTermCondition.Visible = true;
                return;
            }
            string encoded = Request.QueryString["qu"];
            string dd = Request.QueryString["dd"];
            string ed = Request.QueryString["ed"];
            if (!string.IsNullOrWhiteSpace(encoded))
            {
                string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
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
                string selectedWarranty = "";

                if (rb3M.Checked)
                    selectedWarranty = "00/03/00";
                else if (rb6M.Checked)
                    selectedWarranty = "00/06/00";
                else if (rb1Y.Checked)
                    selectedWarranty = "01/00/00";
                else if (rb2Y.Checked)
                    selectedWarranty = "02/00/00";
                else if (rb3Y.Checked)
                    selectedWarranty = "03/00/00";
                else if (rbCustom.Checked)
                {
                    string year = string.IsNullOrEmpty(ddlCustomYears.SelectedValue) ? "00" : ddlCustomYears.SelectedValue;
                    string month = string.IsNullOrEmpty(ddlCustomMonths.SelectedValue) ? "00" : ddlCustomMonths.SelectedValue;
                    string day = string.IsNullOrEmpty(ddlCustomDays.SelectedValue) ? "00" : ddlCustomDays.SelectedValue;

                    selectedWarranty = $"{year}/{month}/{day}";
                }

                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@type", 5);
                    cmd.Parameters.AddWithValue("@Saleschannel", "Retailer");
                    cmd.Parameters.AddWithValue("@Retailer_FreelanceID", decoded);
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
                    //cmd.Parameters.AddWithValue("@PromoDiscount", !string.IsNullOrWhiteSpace(txtPromoDiscount.Value) ? Convert.ToDecimal(txtPromoDiscount.Value.ToString()) : 0);
                    cmd.Parameters.AddWithValue("@DateofImplementation", !string.IsNullOrWhiteSpace(txtDateOfImpl.Text) ? Convert.ToDateTime(txtDateOfImpl.Text.ToString()) : DateTime.Now);
                    cmd.Parameters.AddWithValue("@ManufacturerWarranty_yymmdd", selectedWarranty);
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
                            Session["RetailerCustomerName"] = txtCustomerName.Text;
                            Session["RetailerCustomerEmailId"] = txtCustomerEmail.Text.ToString();
                            Session["RetailerCustomerMobileNo"] = txtCustomerMobile.Text.ToString();

                            Response.Redirect("RetailerCartDetails.aspx?salesorder=" + salesOrderID + "&qu=" + HttpUtility.UrlEncode(encoded) + "&dd=" + HttpUtility.UrlEncode(dd) + "&ed=" + HttpUtility.UrlEncode(ed));
                        }
                    }
                }
                con.Close();
                Response.Redirect("RetailerCartDetails.aspx?qu=" + HttpUtility.UrlEncode(encoded) + "&dd=" + HttpUtility.UrlEncode(dd) + "&ed=" + HttpUtility.UrlEncode(ed));
            }
            else
            {
                string script = $@"
                            <script type='text/javascript'>
                                alert('Link has expired. Please contact your supervisor!');
                                window.location.href = 'InvalidLink.aspx';
                            </script>";

                ClientScript.RegisterStartupScript(this.GetType(), "expiredRedirect", script);
                return;
            }
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

            bool checkcusotmer = CheckBlockCustomer();
            if (!checkcusotmer)
            {
                return;
            }
            if (Session["salesOrderID"] != null)
            {
                PlanPanel.Visible = true;
                btnSubmitOTP.Visible = false;
                lblMessage.Visible = false;
                lnkResendOTP.Visible = false;
                OTPPanel.Visible = false;
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
            else if(txtCustomerMobile.Text != "")
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
                rb3M.Enabled = true;
                rb6M.Enabled = true;
                rb1Y.Enabled = true;
                rb2Y.Enabled = true;
                rb3Y.Enabled = true;
                ddlCustomYears.Enabled = true;
                ddlCustomMonths.Enabled = true;
                ddlCustomDays.Enabled = true;
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
                string message = "Welcome to Infinity, Your OTP to Login to Infinity TechCare Lounge is " + otp + ". For Help, Call Infinity 8447882424. 9AM-6PM Mon-Sat";
                string content_temID = "1107162426891569578";
                string no = mobileno;
                string sender12 = "ISHILD";
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
                    OTPPanel.Visible = false;
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
            txtPromoDiscount.Value = "";
            lblDiscountAmount.Text = "";
            lblTotalAmount.Text = "";
            calculationdiv.Visible = false;
            txtPromoDiscount.Focus();
            lblErrorPromoCode.Visible = false; ;
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
                serielNoDiv.Visible = false;
                dateOfImplementationDic.Visible = false;
                imeiNoDiv.Visible = true;

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
                serielNoDiv.Visible = true;
                dateOfImplementationDic.Visible = true;
                imeiNoDiv.Visible = false;

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
            rb3M.Enabled = true;
            rb6M.Enabled = true;
            rb1Y.Enabled = true;
            rb2Y.Enabled = true;
            rb3Y.Enabled = true;
            ddlCustomYears.Enabled = true;
            ddlCustomMonths.Enabled = true;
            ddlCustomDays.Enabled = true;
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

        protected void ApplyPromoCode(object sender, EventArgs e)
        {
            try
            {
                decimal planPrice = 0;
                int planId = 0;
                foreach (RepeaterItem item in rptPlans.Items)
                {
                    CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                    if (chkSelect != null && chkSelect.Checked)
                    {
                        HiddenField hdnPlanPrice = (HiddenField)item.FindControl("hdnPlanPrice");
                        HiddenField hdnPlanId = (HiddenField)item.FindControl("hdnPlanId");
                        if (hdnPlanPrice != null)
                        {
                            planPrice = Convert.ToDecimal(hdnPlanPrice.Value);
                        }
                        if (hdnPlanId != null)
                        {
                            planId = Convert.ToInt32(hdnPlanId.Value);
                        }
                        break;
                    }
                }
                string encoded = Request.QueryString["qu"];
                string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 20);
                    cmd.Parameters.AddWithValue("@Promocode", txtPromoDiscount.Value.Trim());
                    cmd.Parameters.AddWithValue("@PlanID", planId);
                    cmd.Parameters.AddWithValue("@ProdMid", ddlProductType.SelectedValue.Trim());
                    cmd.Parameters.AddWithValue("@ProCat", ddlsubcatg.SelectedValue.Trim());
                    cmd.Parameters.AddWithValue("@Brand", ddlBrand.SelectedValue.Trim());
                    cmd.Parameters.AddWithValue("@ProductPrice", txtPrice.Text.Trim());
                    cmd.Parameters.AddWithValue("@imei", !string.IsNullOrWhiteSpace(txtimeiNo.Text) ? txtimeiNo.Text.Trim() : txtSerialNo.Text);
                    cmd.Parameters.AddWithValue("@Retailer_MobileNo", decoded);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        decimal discountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                        decimal discountPercent = Convert.ToDecimal(reader["DiscountPer"]);

                        if (planPrice > 0)
                        {
                            if (discountAmount > 0)
                            {
                                decimal total = planPrice - discountAmount;
                                lblPlanPrice.Text = "₹" + planPrice.ToString("0.00");
                                lblDiscountAmount.Text = "₹" + discountAmount.ToString("0.00");
                                lblTotalAmount.Text = "₹" + total.ToString("0.00");
                                calculationdiv.Visible = true;
                                lblPromoCodeDiscountAmount.InnerText = $"Promo Discount Amount :";
                                lblErrorPromoCode.Visible = false;
                            }
                            else if (discountPercent > 0)
                            {
                                discountAmount = (planPrice * discountPercent) / 100;
                                decimal total = planPrice - discountAmount;
                                lblPlanPrice.Text = "₹" + planPrice.ToString("0.00");
                                lblDiscountAmount.Text = "₹" + discountAmount.ToString("0.00");
                                lblTotalAmount.Text = "₹" + total.ToString("0.00");
                                calculationdiv.Visible = true;
                                lblPromoCodeDiscountAmount.InnerText = $"Promo Discount Amount({discountPercent}%) :";
                                lblErrorPromoCode.Visible = false;
                            }
                        }
                        else
                        {
                            lblErrorPromoCode.Visible = true;
                            lblErrorPromoCode.Text = "No plan selected.";
                        }
                    }
                    else
                    {
                        lblPlanPrice.Text = "₹" + planPrice.ToString("0.00");
                        lblDiscountAmount.Text = "₹" + "0.00";
                        lblTotalAmount.Text = "₹" + planPrice.ToString("0.00");
                        calculationdiv.Visible = true;
                        lblPromoCodeDiscountAmount.InnerText = $"Promo Discount Amount :";
                        lblErrorPromoCode.Visible = true;
                        lblErrorPromoCode.Text = "Invalid promo code.";
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, "Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        protected void WarrantyChanged(object sender, EventArgs e)
        {
            lbl3M.CssClass = "btn btn-outline-primary";
            lbl6M.CssClass = "btn btn-outline-primary";
            lbl1Y.CssClass = "btn btn-outline-primary";
            lbl2Y.CssClass = "btn btn-outline-primary";
            lbl3Y.CssClass = "btn btn-outline-primary";
            lblCustom.CssClass = "btn btn-outline-warning";

            if (rb3M.Checked)
            {
                lbl3M.CssClass += " selectWarranty";
                customWarrantyDiv.Visible = false;
            }
            else if (rb6M.Checked)
            {
                lbl6M.CssClass += " selectWarranty";
                customWarrantyDiv.Visible = false;
            }
            else if (rb1Y.Checked)
            {
                lbl1Y.CssClass += " selectWarranty";
                customWarrantyDiv.Visible = false;
            }
            else if (rb2Y.Checked)
            {
                lbl2Y.CssClass += " selectWarranty";
                customWarrantyDiv.Visible = false;
            }
            else if (rb3Y.Checked)
            {
                lbl3Y.CssClass += " selectWarranty";
                customWarrantyDiv.Visible = false;
            }
            else if (rbCustom.Checked)
            {
                lblCustom.CssClass += " selectCustomWarranty";
                customWarrantyDiv.Visible = true;

                if (ddlCustomYears.Items.Count <= 1)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        string value = i.ToString("D2");
                        ddlCustomYears.Items.Add(new ListItem(value, value));
                    }
                }
                else
                {
                    ddlCustomYears.SelectedIndex = 0;
                }

                if (ddlCustomMonths.Items.Count <= 1)
                {
                    for (int i = 0; i <= 12; i++)
                    {
                        string value = i.ToString("D2");
                        ddlCustomMonths.Items.Add(new ListItem(value, value));
                    }
                }
                else
                    ddlCustomMonths.SelectedIndex = 0;

                if (ddlCustomDays.Items.Count <= 1)
                {
                    for (int i = 0; i <= 31; i++)
                    {
                        string value = i.ToString("D2");
                        ddlCustomDays.Items.Add(new ListItem(value, value));
                    }
                }
                else
                    ddlCustomDays.SelectedIndex = 0;
            }
        }
        protected void ValidateWarranty(object source, ServerValidateEventArgs args)
        {
            if (rb3M.Checked || rb6M.Checked || rb1Y.Checked || rb2Y.Checked || rb3Y.Checked)
            {
                args.IsValid = true;
            }
            else if (rbCustom.Checked)
            {
                args.IsValid = !string.IsNullOrEmpty(ddlCustomYears.SelectedValue);
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected bool CheckBlockCustomer()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerMobileNo", SqlDbType.Int).Value = txtCustomerMobile.Text.ToString();
                cmd.Parameters.AddWithValue("@CustomerEmailID", SqlDbType.Int).Value = txtCustomerEmail.Text.ToString();
                cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 39;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string suspicious = dt.Rows[0]["suspicious"].ToString();
                    if (suspicious == "1")
                    {
                        BlockCustomerErrorMessage.Text = "This Customer is Under Watch.Please contact your Manager";
                        customerInfoDiv.Attributes.Add("style", "background:gray");
                        return false;
                    }
                    else if (suspicious == "2")
                    {
                        BlockCustomerErrorMessage.Text = "This Customer is Black Listed..Please contact your Manager";
                        customerInfoDiv.Attributes.Add("style", "background:pink");
                        return false;

                    }
                    else if (suspicious == "")
                    {
                        customerInfoDiv.Attributes.Add("style", "background:white");
                        BlockCustomerErrorMessage.Text = "";
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}