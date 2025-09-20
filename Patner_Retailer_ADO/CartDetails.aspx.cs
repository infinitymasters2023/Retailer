using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net.NetworkInformation;
using paytm;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using Label = System.Web.UI.WebControls.Label;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Diagnostics.PerformanceData;
using System.Security.Cryptography;


namespace Patner_Retailer_ADO
{
    public partial class CartDetails : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iaplConnectionString"].ConnectionString);
        static public void DisplayMessage(Control page, string msg)
        {
            string msg1 = String.Format("alert('{0}');", msg);
            ScriptManager.RegisterStartupScript(page, page.GetType(), "msg", msg1, true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string mes = Request.QueryString["pay"];
                if(!string.IsNullOrWhiteSpace(mes) && mes == "cancel")
                {
                    BindCustomerInfo();
                }
                if (!string.IsNullOrWhiteSpace(mes) && mes == "Requestfromsalesperson")
                {
                    BindCustomerInfo(); 
                }
                BindProductInfo();
                BindOrderSummary();
                if(Session["Role"] != null && Session["Role"].ToString() == "Agent")
                {
                    btnContinuePayment.Text = "Request to Retailer";
                }    
            }
        }

        protected void BindProductInfo()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 6);
                    cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"].ToString());
                    cmd.Parameters.AddWithValue("@Customer_OrderID", Session["Customerorder"].ToString());
                    cmd.Parameters.AddWithValue("@CustomerMobileNo", Session["CustomerMobileNo"].ToString());

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        rptPlans.DataSource = dt;
                        rptPlans.DataBind();
                        txtFirstName.Text = Session["CustomerName"].ToString();
                        txtEmail.Text = dt.Rows[0]["CustomerEmailID"]?.ToString();
                        txtCustomerMobileNo.Text = dt.Rows[0]["CustomerMobileNo"]?.ToString();
                        Session["UID"] = dt.Rows[0]["UID"]?.ToString();
                    }
                    else
                    {
                        rptPlans.DataSource = null;
                        rptPlans.DataBind();
                        Response.Redirect("ProductInformation.aspx");
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        protected void BindOrderSummary()
        {
            try
            {
                int totalQuantity = 0;
                decimal taxableValue = 0;
                foreach (RepeaterItem item in rptPlans.Items)
                {
                    var lblQuantity = item.FindControl("lblQuantity") as Label;
                    var lblPlanPrice = item.FindControl("lblPlanPrice") as Label;

                    if (lblQuantity != null && int.TryParse(lblQuantity.Text, out int quantity) &&
                        lblPlanPrice != null && decimal.TryParse(lblPlanPrice.Text, out decimal planPrice))
                    {
                        totalQuantity += quantity;
                        taxableValue += planPrice * quantity;
                    }
                }
                decimal taxableamount = Math.Round(taxableValue / 1.18m, 2);
                decimal taxAmount = Math.Round(taxableamount * 0.18m, 2);

                decimal commision = calculateCommision(taxableValue);
                decimal Taxablecommision = Math.Round(commision / 1.18m, 2);
                decimal commisionTax = Math.Round(Taxablecommision * 0.18m, 2);
                decimal totalAmountPay = (taxableamount - Taxablecommision) + taxAmount;

                txtQuantity.InnerText = totalQuantity.ToString();
                TotalValueAmount.InnerText = taxableValue.ToString("N2");
                TaxableValue.InnerText = "Rs. " + taxableamount.ToString("N2");
                TaxAmout.InnerText = "Rs. " + taxAmount.ToString("N2");
                TotalValue.InnerText = "Rs. " + taxableValue.ToString("N2");
                TotalCommisionValue.InnerText = "- Rs. " + commision.ToString("N2");
                CommisionValue.InnerText = "- Rs. " + Taxablecommision.ToString("N2");
                ComminsionTax.InnerText = "- Rs. " + commisionTax.ToString("N2");
                TotalAmountPay.InnerText = "Rs. " + totalAmountPay.ToString("N2");
            }
            catch (Exception)
            {
                return;
            }
        }

        protected void BindCustomerInfo()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 66);
                    cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"]);

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            txtFirstName.Text = rdr["CustomerName"].ToString();
                            txtCustomerMobileNo.Text = rdr["MobileNo"].ToString();
                            txtAlternativeMobile.Text = rdr["AlternativeMobile"].ToString();
                            txtWhatsappNo.Text = rdr["WhatsappNo"].ToString();
                            txtEmail.Text = rdr["EmailIDAddress"].ToString();
                            txtAlternativeEmail.Text = rdr["AlternativeEmail"].ToString();
                            txtPincode.Text = rdr["Pincode"].ToString();
                            txtCity.Text = rdr["City"].ToString();
                            txtState.Text = rdr["State"].ToString();
                            txtAddressLine1.InnerText = rdr["AddressLine1"].ToString();
                            txtLandmark.InnerText = rdr["Landmark"].ToString();
                            txtProductInstalledPincode.Text = rdr["InstalledPincode"].ToString();
                            txtProductInstalledCity.Text = rdr["InstalledCity"].ToString();
                            txtProductInstalledState.Text = rdr["InstalledState"].ToString();
                            txtAvaility.InnerText = rdr["Availity"].ToString();
                            txtinstalledLandmark.InnerText = rdr["installedLandmark"].ToString();
                            Session["Customerorder"] = rdr["Customer_OrderID"].ToString();
                            Session["CustomerMobileNo"] = txtCustomerMobileNo.Text;
                            Session["CustomerName"] = txtFirstName.Text;
                            Session["CustomerEmailID"] = txtEmail.Text;
                            Session["CustomerMobileNo"] = txtCustomerMobileNo.Text;
                            
                            string mid = rdr["Mid"].ToString();
                            string payMid = rdr["PayMid"].ToString();
                            if (!string.IsNullOrEmpty(mid) && !string.IsNullOrEmpty(payMid))
                            {
                                UpdateUID(mid, payMid);
                            }
                        }
                    }
                    con.Close();

                }
            }
            catch (Exception)
            {
                return;
            }
        }

        protected void UpdateUID(string mid, string PayMid)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 78);
                    cmd.Parameters.AddWithValue("@Mid", mid);
                    cmd.Parameters.AddWithValue("@PayMid", PayMid);

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
                return;
            }
        }


        protected void txtPIN_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPincode.Text.Length != 6)
                {
                    txtCity.Text = "";
                    txtState.Text = "";

                    txtCity.Enabled = false;
                    txtState.Enabled = false;
                    lblPincodeError.Text = "Enter a valid 6-digit pincode";
                    lblPincodeError.Visible = true;
                    return;
                }
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pincode", SqlDbType.Int).Value = txtPincode.Text.Trim();
                cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 4;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    txtCity.Text = dt.Rows[0]["CityName"].ToString();
                    txtState.Text = dt.Rows[0]["statename"].ToString();

                    txtCity.Enabled = false;
                    txtState.Enabled = false;
                    lblPincodeError.Visible = false;
                    txtAddressLine1.Focus();
                }
                else
                {
                    txtCity.Text = "";
                    txtState.Text = "";

                    txtCity.Enabled = false;
                    txtState.Enabled = false;
                    lblPincodeError.Text = "Enter a valid pincode";
                    lblPincodeError.Visible = true;
                    txtAddressLine1.Focus();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        protected void txtInstalledPIN_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtProductInstalledPincode.Text.Length != 6)
                {
                    txtProductInstalledCity.Text = "";
                    txtProductInstalledState.Text = "";

                    txtProductInstalledCity.Enabled = false;
                    txtProductInstalledState.Enabled = false;
                    lblProductInstalledPincodeError.Text = "Enter a valid 6-digit pincode";
                    lblProductInstalledPincodeError.Visible = true;
                    return;
                }
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pincode", SqlDbType.Int).Value = txtProductInstalledPincode.Text.Trim();
                cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 4;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    txtProductInstalledCity.Text = dt.Rows[0]["CityName"].ToString();
                    txtProductInstalledState.Text = dt.Rows[0]["statename"].ToString();

                    txtProductInstalledCity.Enabled = false;
                    txtProductInstalledState.Enabled = false;
                    lblProductInstalledPincodeError.Visible = false;
                    txtAvaility.Focus();
                }
                else
                {
                    txtProductInstalledCity.Text = "";
                    txtProductInstalledState.Text = "";

                    txtProductInstalledCity.Enabled = false;
                    txtProductInstalledState.Enabled = false;
                    lblProductInstalledPincodeError.Text = "Enter a valid pincode";
                    lblProductInstalledPincodeError.Visible = true;
                    txtAvaility.Focus();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        protected void ContinuePayment(object sender, EventArgs e)
        {
            try
            {
                int count = 0;

                if(txtLandmark.InnerText.Length < 3)
                {
                    lblLandmarkError.Visible = true;
                    lblLandmarkError.Text = "Landmark must be 3 character";
                    count++;
                }
                else if (string.IsNullOrWhiteSpace(txtLandmark.InnerText))
                {
                    lblLandmarkError.Visible = true;
                    lblLandmarkError.Text = "Landmark is required";
                    count++;
                }
                else { lblLandmarkError.Visible = false;}

                string address = txtAddressLine1.InnerText?.Trim() ?? "";
                int wordCount = address.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;

                if (wordCount < 3)
                {
                    lblAddressError.Text = "Address must contain at least 3 words";
                    lblAddressError.Visible = true;
                    count++;
                }
                else if(string.IsNullOrWhiteSpace(address))
                {
                    lblAddressError.Text = "Address is required";
                    lblAddressError.Visible = true;
                    count++;
                }
                else { lblAddressError.Visible = false; }
                string pincode = txtPincode.Text.Trim();

                if (string.IsNullOrWhiteSpace(pincode))
                {
                    lblPincodeError.Text = "Pincode is required";
                    lblPincodeError.Visible = true;
                    count++;
                }
                else if (!string.IsNullOrWhiteSpace(pincode) && !Regex.IsMatch(pincode, @"^\d{6}$"))
                {
                    lblPincodeError.Text = "Enter a valid 6-digit pincode";
                    lblPincodeError.Visible = true;
                    count++;
                }
                else { lblPincodeError.Visible = false; }


                bool blockuserem = CheckBlockCustomer(txtAlternativeEmail.Text, "");
                bool existuserem = CheckSalesPersonOrRetailer(txtAlternativeEmail.Text, "");
                if (existuserem)
                {
                    lblAtlEmail.InnerText = "This Email is already registered";
                    lblAltMobileNo.InnerText = "";
                    lblAtlEmail.Attributes.Add("style", "display: block;");
                    count++;
                }
                else if (blockuserem)
                {
                    lblAtlEmail.InnerText = lblAltMobileNo.InnerText;
                    lblAltMobileNo.InnerText = "";
                    lblAtlEmail.Attributes.Add("style", "display: block;");
                    count++;
                }
                else { lblAtlEmail.Attributes.Add("style", "display: none;");}

                Regex emailRegex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

                if (!string.IsNullOrWhiteSpace(txtAlternativeEmail.Text.Trim().ToLower()))
                {
                    if (!emailRegex.IsMatch(txtAlternativeEmail.Text.Trim().ToLower()))
                    {
                        lblAtlEmail.InnerText = "Invalid email format";
                        lblAtlEmail.Attributes.Add("style", "display: block;");
                        count++;
                    }
                    else
                    {
                        lblAtlEmail.Visible = false;
                        lblAtlEmail.Attributes.Add("style", "display: none;");
                    }
                }
                else { lblAtlEmail.Attributes.Add("style", "display: none;"); }
                
                string altMobile = txtAlternativeMobile.Text.Trim();
                string whatsAppNo = txtWhatsappNo.Text.Trim();

                Regex mobileRegex = new Regex(@"^[6-9]\d{9}$");

                if (!string.IsNullOrWhiteSpace(altMobile))
                {
                    if (!mobileRegex.IsMatch(altMobile))
                    {
                        lblAltMobileNo.InnerText = "Enter valid 10-digit mobile number";
                        lblAltMobileNo.Attributes.Add("style", "display: block;");
                        count++;
                    }
                    else { lblAltMobileNo.Attributes.Add("style", "display: none;"); }
                }
                else { lblAltMobileNo.Attributes.Add("style", "display: block;"); }

                if (!string.IsNullOrWhiteSpace(whatsAppNo) && !mobileRegex.IsMatch(whatsAppNo))
                {
                    lblWhatsAppError.InnerText = "Enter valid 10-digit number";
                    lblWhatsAppError.Attributes.Add("style", "display: block;");
                    count++;
                }
                else { lblWhatsAppError.Attributes.Add("style", "display: none;"); }

                bool blockuser = CheckBlockCustomer("", txtAlternativeMobile.Text);
                bool existuser = CheckSalesPersonOrRetailer("", txtAlternativeMobile.Text);
                if (existuser)
                {
                    lblAltMobileNo.InnerText = "This Mobile No is already registered";
                    lblAltMobileNo.Attributes.Add("style", "display: block;");
                    txtAlternativeMobile.Focus();
                    count++;
                }
                else if (blockuser)
                {
                    lblAltMobileNo.InnerText = lblAltMobileNo.InnerText;
                    lblAltMobileNo.Attributes.Add("style", "display: block;");
                    txtAlternativeMobile.Focus();
                    count++;
                }
                else { lblAltMobileNo.Attributes.Add("style", "display: none;"); }

                bool blockuserwa = CheckBlockCustomer("", txtWhatsappNo.Text);
                bool existuserwa = CheckSalesPersonOrRetailer("", txtWhatsappNo.Text);
                if (existuserwa)
                {
                    lblWhatsAppError.InnerText = "This Mobile No is already registered";
                    lblWhatsAppError.Attributes.Add("style", "display: block;");
                    txtWhatsappNo.Focus();
                    count++;
                }
                else if (blockuserwa)
                {
                    lblWhatsAppError.InnerText = lblAltMobileNo.InnerText;
                    lblWhatsAppError.Attributes.Add("style", "display: block;");
                    lblAltMobileNo.InnerText = "";
                    txtWhatsappNo.Focus();
                    count++;
                }
                else { lblWhatsAppError.Attributes.Add("style", "display: none;"); }

                if (count > 0)
                {
                    return;
                }
                bool payment = false;
                if (Session["Role"] != null && Session["Role"].ToString() == "Agent")
                {
                    RequestToRetailerIncomepltePurchase();
                    payment = AddPaymentSalesPerson();
                }
                else
                {
                    payment = AddPayment();
                }
                if (payment)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@type", 7);
                        cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"].ToString());
                        cmd.Parameters.AddWithValue("@CustomerName", txtFirstName.Text.ToString());
                        cmd.Parameters.AddWithValue("@AddressLine1", txtAddressLine1.Value.ToString());
                        cmd.Parameters.AddWithValue("@City", txtCity.Text.ToString());
                        cmd.Parameters.AddWithValue("@State", txtState.Text.ToString());
                        cmd.Parameters.AddWithValue("@Pincode", txtPincode.Text.ToString());
                        cmd.Parameters.AddWithValue("@CustomerMobileNo", txtCustomerMobileNo.Text.ToString());
                        cmd.Parameters.AddWithValue("@AlternativeMobile", txtAlternativeMobile.Text.ToString());
                        cmd.Parameters.AddWithValue("@CustomerEmailID", txtEmail.Text.ToString());
                        cmd.Parameters.AddWithValue("@AlternativeEmail", txtAlternativeEmail.Text.ToString());
                        cmd.Parameters.AddWithValue("@Landmark", txtLandmark.Value.ToString());
                        cmd.Parameters.AddWithValue("@Availity", txtAvaility.Value.ToString());
                        cmd.Parameters.AddWithValue("@InstalledPincode", txtProductInstalledPincode.Text.ToString());
                        cmd.Parameters.AddWithValue("@InstalledState", txtProductInstalledState.Text.ToString());
                        cmd.Parameters.AddWithValue("@InstalledCity", txtProductInstalledCity.Text.ToString());
                        cmd.Parameters.AddWithValue("@installedLandmark", txtinstalledLandmark.Value.ToString());
                        cmd.Parameters.AddWithValue("@Saleschannel", "Retailer");
                        cmd.Parameters.AddWithValue("@WhatsappNo", txtWhatsappNo.Text.ToString());
                        cmd.Parameters.AddWithValue("@TaxableValue", TaxableValue.InnerText.Replace("Rs. ", "").Replace(",", "").Trim());
                        cmd.Parameters.AddWithValue("@TaxAmout", TaxAmout.InnerText.Replace("Rs. ", "").Replace(",", "").Trim());
                        cmd.Parameters.AddWithValue("@TotalAmountPay", TotalAmountPay.InnerText.Replace("Rs. ", "").Replace(",", "").Trim());
                        //if (Commossiontag.InnerText.Contains("%"))
                        //{
                        //    cmd.Parameters.AddWithValue("@CommissionType", "Percantage");
                        //    cmd.Parameters.AddWithValue("@CommissionValue", "0");
                        //    cmd.Parameters.AddWithValue("@CommissionPercentage", ViewState["CommissionPercantage"] != null ? ViewState["CommissionPercantage"].ToString() : "0");
                        //}
                        //else
                        //{
                        //    cmd.Parameters.AddWithValue("@CommissionType", "Value");
                        //    cmd.Parameters.AddWithValue("@CommissionValue", CommisionValue.InnerText.Replace("Rs. ", "").Replace(",", "").Replace("-", "").Trim());
                        //    cmd.Parameters.AddWithValue("@CommissionPercentage", "0");
                        //}
                        //cmd.Parameters.AddWithValue("@GSTCommissionValue", "");
                        //cmd.Parameters.AddWithValue("@GSTCommissionPercentage", "18");
                        //cmd.Parameters.AddWithValue("@GSTCommissionPaid", "No");
                        cmd.Parameters.AddWithValue("@TranStatus", "Pending");
                        cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OrderId", "");
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["RetailerUniqueID"].ToString());

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd.ExecuteNonQuery();
                        Session.Remove("CustomerName");
                        bool paymentType = CheckPaymentMethod();
                        if (paymentType)
                        {
                            //Commision
                            ViewState["ItemPrice"] = TotalAmountPay.InnerText.Replace("Rs. ", "").Replace(",", "").Trim();
                            //ViewState["ItemPrice"] = "1";
                        }
                        else
                        {
                            //Full payment
                            ViewState["ItemPrice"] = TotalValue.InnerText.Replace("Rs. ", "").Replace(",", "").Trim();
                            //ViewState["ItemPrice"] = "1";
                        }
                        if (Session["Role"] != null && Session["Role"].ToString() == "Agent")
                        {
                            con.Close();
                            Response.Redirect("PaymentConfirmation.aspx?qu=" + Session["salesOrderID"].ToString(), false);
                            Context.ApplicationInstance.CompleteRequest();
                            return;
                        }
                        else
                        {
                            UpdateIncomepltePurchase();
                            getpaytm();
                        }
                        con.Close();
                    }
                }
                else
                {
                    DisplayMessage(this, "Payemnt Failed");
                    return;
                }
                DisplayMessage(this, "Record Added Successfully");
                return;
            }
            catch (Exception ex)
            {
                DisplayMessage(this, "" + ex.Message + "");
                return;
            }
        }



        public void getpaytm()
        {
            Session["mode"] = "PayTm";

            string serverUrl = ConfigurationManager.AppSettings["ServerURL"];
            HttpContext.Current.Cache.Insert("TransactionId", (Session["salesOrderID"].ToString() + Session["UID"].ToString()), null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
            //   HttpContext.Current.Cache.Insert("RetailerMobileNo", Session["MobileNo"], null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);

            String merchantKey = "Xv#3x9vZ%cawdcD1";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("MID", "InfinA73791511910258");
            parameters.Add("CHANNEL_ID", "WEB");
            parameters.Add("INDUSTRY_TYPE_ID", "Retail109");
            parameters.Add("WEBSITE", "InfinAWEB");
            parameters.Add("EMAIL", txtEmail.Text);
            parameters.Add("MOBILE_NO", txtCustomerMobileNo.Text);
            parameters.Add("CUST_ID", "250");
            parameters.Add("ORDER_ID", (Session["salesOrderID"].ToString() + Session["UID"].ToString()));
            parameters.Add("TXN_AMOUNT", ViewState["ItemPrice"].ToString());

            parameters.Add("CALLBACK_URL", serverUrl + "/PaymentConfirmation.aspx?m=" + Session["mode"] + "&Mobile=" + Session["MobileNo"] + "&RetailerUniqueID=" + Session["RetailerUniqueID"] + "&Role=" + Session["Role"]);

            string checksum = CheckSum.generateCheckSum(merchantKey, parameters);

            if (checksum != null)
            {
                string paytmURL = "https://secure.paytm.in/oltp-web/processTransaction";
                string outputHTML = "<html>";
                outputHTML += "<head>";
                outputHTML += "<title>Merchant Check Out Page</title>";
                outputHTML += "</head>";
                outputHTML += "<body>";
                outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
                outputHTML += "<form method='post' action='" + paytmURL + "' name='f1'>";
                outputHTML += "<table border='1'>";
                outputHTML += "<tbody>";

                foreach (string key in parameters.Keys)
                {
                    outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
                }
                outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
                outputHTML += "</tbody>";
                outputHTML += "</table>";
                outputHTML += "<script type='text/javascript'>";
                outputHTML += "document.f1.submit();";
                outputHTML += "</script>";
                outputHTML += "</form>";
                outputHTML += "</body>";
                outputHTML += "</html>";
                Response.Write(outputHTML);
            }
        }


        protected void AddMoreProducts(object sender, EventArgs e)
        {
            Response.Redirect("ProductInformation.aspx?AddProduct=Add");
        }

        protected void DeletePlanInfo(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkDelete = (LinkButton)sender;
                string[] args = lnkDelete.CommandArgument.Split('|');
                string mid = args[0];
                string cartItemId = args[1];
                string uniqueId = args[2];


                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Type", 14);
                cmd.Parameters.AddWithValue("@Mid", mid);

                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                DeleteIncompeteInfo(cartItemId, uniqueId);
                BindProductInfo();
                BindOrderSummary();
            }
            catch (Exception ex)
            {
            }
        }
        protected void EditPlanInfo(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string[] args = btn.CommandArgument.Split('|');

            string mid = args[0];
            string cartItemId = args[1];
            string uniqueId = args[2];

            foreach (RepeaterItem item in rptPlans.Items)
            {
                HiddenField hdnUID = item.FindControl("hdnUID") as HiddenField;
                LinkButton lnlEdit = item.FindControl("lnlEdit") as LinkButton;

                if (hdnUID != null && hdnUID.Value == uniqueId)
                {
                    Label lblSelectedPlan = item.FindControl("lblSelectedPlan") as Label;
                    DropDownList ddlPlan = item.FindControl("ddlPlan") as DropDownList;

                    LinkButton lnkBack = item.FindControl("lnkBack") as LinkButton;
                    LinkButton lnkSave = item.FindControl("lnkSave") as LinkButton;
                    if (lnlEdit != null) lnlEdit.Visible = false;
                    if (lnkBack != null) lnkBack.Visible = true;
                    if (lnkSave != null) lnkSave.Visible = true;
                    if (lblSelectedPlan != null) lblSelectedPlan.Visible = false;
                    if (ddlPlan != null)
                    {
                        ddlPlan.Visible = true;
                        lblSelectedPlan.Visible = false;
                        DataTable dtProductInfo = GetProductInfo(mid);

                        if (dtProductInfo.Rows.Count > 0)
                        {
                            string planPrice = dtProductInfo.Rows[0]["DevicePurchasePrice"]?.ToString();
                            string subProductTypeId = dtProductInfo.Rows[0]["SubProductTypeID"]?.ToString();
                            string subCatId = dtProductInfo.Rows[0]["SubCategoryID"]?.ToString();
                            string productTypeId = dtProductInfo.Rows[0]["ProductTypeID"]?.ToString();

                            DataTable plans = GetPlans(productTypeId, subCatId, subProductTypeId, planPrice);

                            ddlPlan.DataSource = plans;
                            ddlPlan.DataTextField = "PlanNicknameSelection";
                            ddlPlan.DataValueField = "SKU";
                            ddlPlan.DataBind();

                            ddlPlan.Items.Insert(0, new ListItem("--Select Plan--", ""));
                        }
                    }

                   // break;
                }
                else
                {
                    if (lnlEdit != null)
                        lnlEdit.Enabled = false;
                }
            }
        }


        private DataTable GetProductInfo(string mid)
        {
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 75);
                cmd.Parameters.AddWithValue("@Mid", mid);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        private DataTable GetPlans(string productTypeId, string subCatId, string subProductTypeId, string productPrice)
        {
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 4);
                cmd.Parameters.AddWithValue("@ProductType", productTypeId);
                cmd.Parameters.AddWithValue("@subcatgId", subCatId);
                cmd.Parameters.AddWithValue("@SubProductType", subProductTypeId);
                cmd.Parameters.AddWithValue("@ProductPrice", productPrice);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }

        protected void CancelPlanInfo(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string[] args = btn.CommandArgument.Split('|');

            string mid = args[0];
            string cartItemId = args[1];
            string uniqueId = args[2];

            foreach (RepeaterItem item in rptPlans.Items)
            {
                HiddenField hdnUID = item.FindControl("hdnUID") as HiddenField;
                LinkButton lnlEdit = item.FindControl("lnlEdit") as LinkButton;

                if (hdnUID != null && hdnUID.Value == uniqueId)
                {
                    Label lblSelectedPlan = item.FindControl("lblSelectedPlan") as Label;
                    DropDownList ddlPlan = item.FindControl("ddlPlan") as DropDownList;
                    Label lblPlanPrice = item.FindControl("lblPlanPrice") as Label;

                    LinkButton lnkBack = item.FindControl("lnkBack") as LinkButton;
                    LinkButton lnkSave = item.FindControl("lnkSave") as LinkButton;
                    if (lnlEdit != null) lnlEdit.Visible = true;
                    if (lnkBack != null) lnkBack.Visible = false;
                    if (lnkSave != null) lnkSave.Visible = false;
                    lblPlanPrice.Text = ViewState["OriginalPrice"] != null ? ViewState["OriginalPrice"].ToString() : lblPlanPrice.Text;
                    if (lblSelectedPlan != null)
                        lblSelectedPlan.Visible = true;

                    if (ddlPlan != null)
                        ddlPlan.Visible = false;

                   // break;
                }
                else
                {
                    if (lnlEdit != null)
                        lnlEdit.Enabled = true;
                }
            }
        }


        protected void SavePlanInfo(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                string[] args = btn.CommandArgument.Split('|');

                string mid = args[0];
                string cartItemId = args[1];
                string uid = args[2];

                foreach (RepeaterItem item in rptPlans.Items)
                {
                    HiddenField hdnUID = item.FindControl("hdnUID") as HiddenField;
                    LinkButton lnkEdit = item.FindControl("lnlEdit") as LinkButton;

                    if (hdnUID != null && hdnUID.Value == uid)
                    {
                        DropDownList ddlPlan = item.FindControl("ddlPlan") as DropDownList;
                        Label lblPlanPrice = item.FindControl("lblPlanPrice") as Label;

                        string selectedSku = ddlPlan.SelectedValue;
                        decimal planPrice = GetPlanPriceFromDB(selectedSku);
                        lblPlanPrice.Text = planPrice.ToString("0.00");
                        
                        UpdatePlanDetails(uid, selectedSku, planPrice);

                        ddlPlan.Visible = false;
                        Label lblSelectedPlan = item.FindControl("lblSelectedPlan") as Label;
                        lblSelectedPlan.Visible = true;
                        lblSelectedPlan.Text = ddlPlan.SelectedItem.Text;

                        LinkButton lnkBack = item.FindControl("lnkBack") as LinkButton;
                        LinkButton lnkSave = item.FindControl("lnkSave") as LinkButton;

                        lnkEdit.Visible = true;
                        lnkBack.Visible = false;
                        lnkSave.Visible = false;

                        //break;
                    }
                    else
                    {
                        if (lnkEdit != null)
                            lnkEdit.Enabled = true;
                    }
                }
                BindOrderSummary();
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
                return;
            }
        }
        private void UpdatePlanDetails(string uid, string sku, decimal planPrice)
        {
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 77);
                cmd.Parameters.AddWithValue("@UID", uid);
                cmd.Parameters.AddWithValue("@SKU", sku);
                cmd.Parameters.AddWithValue("@PlanPrice", planPrice);

                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }


        protected decimal calculateCommision(decimal totalValue)
        {
            decimal commission = 0;

            SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@type", 41);
            cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString());
            cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());
            if (con.State != ConnectionState.Open)
                con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                string type = row["Type"].ToString();
                decimal commissionValue = (row["CommissionValue"] != DBNull.Value && !string.IsNullOrEmpty(row["CommissionValue"].ToString())) ? Convert.ToDecimal(row["CommissionValue"]) : 0m;
                decimal commissionPercentage = (row["CommissionPercentage"] != DBNull.Value && !string.IsNullOrEmpty(row["CommissionPercentage"].ToString())) ? Convert.ToDecimal(row["CommissionPercentage"]) : 0m;

                if (type == "Percentage")
                {
                    commission = (commissionPercentage / 100m) * totalValue;
                    Commossiontag.InnerText = "Retailer Commission(" + commissionPercentage.ToString() + "%)";
                    ViewState["CommissionPercantage"] = commissionPercentage;
                }
                else
                {
                    commission = commissionValue;
                    Commossiontag.InnerText = "Retailer Commission";
                }
            }
            con.Close();
            return commission;
        }
        protected bool CheckPaymentMethod()
        {
            try
            {
                if (Session["Role"].ToString() == "Agent")
                {
                    return false;
                }
                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 42);
                cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString());
                cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());
                if (con.State != ConnectionState.Open)
                    con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    string paymentMethod = row["PaymentProcessMethod"].ToString();
                    if (paymentMethod == "2")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                con.Close();
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected DataTable BindProduct()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 8);
                cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"].ToString());
                if (con.State != ConnectionState.Open)
                    con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return dt;
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
                return null;
            }
        }

        protected void DeleteIncompeteInfo(string cartitemId, string uniqueId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 58);
                cmd.Parameters.AddWithValue("@Status", "Deleted");
                cmd.Parameters.AddWithValue("@customer_status", "Plan Deleted from Cart");
                cmd.Parameters.AddWithValue("@cartItemId", cartitemId);
                cmd.Parameters.AddWithValue("@UniqueID", uniqueId);
                cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"] != null ? Session["MobileNo"].ToString() : null);
                if (con.State != ConnectionState.Open)
                    con.Open();

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        protected void UpdateIncomepltePurchase()
        {
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@type", 59);
                cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"].ToString());
                cmd.Parameters.AddWithValue("@Status", "Payment Pending from Customer");
                cmd.Parameters.AddWithValue("@AddressLine1", txtAddressLine1.Value.ToString());
                cmd.Parameters.AddWithValue("@City", txtCity.Text.ToString());
                cmd.Parameters.AddWithValue("@State", txtState.Text.ToString());
                cmd.Parameters.AddWithValue("@Pincode", txtPincode.Text.ToString());
                cmd.Parameters.AddWithValue("@Landmark", txtLandmark.Value.ToString());
                cmd.Parameters.AddWithValue("@TaxableValue", TaxableValue.InnerText.Replace("Rs. ", "").Replace(",", "").Trim());
                cmd.Parameters.AddWithValue("@TaxAmout", TaxAmout.InnerText.Replace("Rs. ", "").Replace(",", "").Trim());
                cmd.Parameters.AddWithValue("@TotalAmountPay", TotalAmountPay.InnerText.Replace("Rs. ", "").Replace(",", "").Trim());
                if (Commossiontag.InnerText.Contains("%"))
                {
                    cmd.Parameters.AddWithValue("@PaymentMethod", "Percantage");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PaymentMethod", "Value");
                }
                cmd.Parameters.AddWithValue("@AlternativeMobile", txtAlternativeMobile.Text.ToString());
                cmd.Parameters.AddWithValue("@AlternativeEmail", txtAlternativeEmail.Text.ToString());
                cmd.Parameters.AddWithValue("@installedLandmark", txtinstalledLandmark.Value.ToString());
                cmd.Parameters.AddWithValue("@Availity", txtAvaility.Value.ToString());
                cmd.Parameters.AddWithValue("@customer_status", "Payment Pending from Customer");
                cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"] != null ? Session["MobileNo"].ToString() : null);

                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        protected void RequestToRetailerIncomepltePurchase()
        {
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@type", 59);
                cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"].ToString());
                cmd.Parameters.AddWithValue("@Status", "Request for Retailer payment");
                cmd.Parameters.AddWithValue("@AddressLine1", txtAddressLine1.Value.ToString());
                cmd.Parameters.AddWithValue("@City", txtCity.Text.ToString());
                cmd.Parameters.AddWithValue("@State", txtState.Text.ToString());
                cmd.Parameters.AddWithValue("@Pincode", txtPincode.Text.ToString());
                cmd.Parameters.AddWithValue("@Landmark", txtLandmark.Value.ToString());
                if (Commossiontag.InnerText.Contains("%"))
                {
                    cmd.Parameters.AddWithValue("@PaymentMethod", "Percantage");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PaymentMethod", "Value");
                }
                cmd.Parameters.AddWithValue("@AlternativeMobile", txtAlternativeMobile.Text.ToString());
                cmd.Parameters.AddWithValue("@AlternativeEmail", txtAlternativeEmail.Text.ToString());
                cmd.Parameters.AddWithValue("@installedLandmark", txtinstalledLandmark.Value.ToString());
                cmd.Parameters.AddWithValue("@Availity", txtAvaility.Value.ToString());
                cmd.Parameters.AddWithValue("@customer_status", "Request for Retailer payment");
                cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"] != null ? Session["MobileNo"].ToString() : null);

                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        protected void chkWhatsAppChange(object sender, EventArgs e)
        {
            if (chkWhatsAppNo.Checked)
            {
                txtWhatsappNo.Text = txtCustomerMobileNo.Text;
                txtWhatsappNo.Enabled = false;
            }
            else
            {
                txtWhatsappNo.Text = "";
                txtWhatsappNo.Enabled = true;
            }

        }

        protected void txtAltCustomerMobile_TextChanged(object sender, EventArgs e)
        {
            bool blockuser = CheckBlockCustomer("", txtAlternativeMobile.Text);
            bool existuser = CheckSalesPersonOrRetailer("", txtAlternativeMobile.Text);
            if (existuser)
            {
                lblAltMobileNo.InnerText = "This Mobile No is already registered";
                lblAltMobileNo.Attributes.Add("style", "display: block;");
                return;
            }
            else if (blockuser)
            {
                lblAltMobileNo.InnerText = lblAltMobileNo.InnerText;
                lblAltMobileNo.Attributes.Add("style", "display: block;");
                return;
            }
            if (!string.IsNullOrEmpty(txtAlternativeMobile.Text) && txtAlternativeMobile.Text.Replace("+91", "").Length != 10)
            {
                lblAltMobileNo.InnerText = "Please enter your phone number.";
                lblAltMobileNo.Attributes.Add("style", "display: block;");
                txtAlternativeMobile.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtAlternativeMobile.Text) && Regex.IsMatch(txtAlternativeMobile.Text, @"^[0-5]"))
            {
                lblAltMobileNo.InnerText = "Invalid number";
                lblAltMobileNo.Attributes.Add("style", "display: block;");
                txtAlternativeMobile.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtAlternativeMobile.Text) && Regex.IsMatch(txtAlternativeMobile.Text, @"^(\d)\1{9}$"))
            {
                lblAltMobileNo.InnerText = "Invalid number.";
                lblAltMobileNo.Attributes.Add("style", "display: block;");
                txtAlternativeMobile.Focus();
                return;
            }
            string mobile = txtCustomerMobileNo.Text.Trim();
            string altMobile = txtAlternativeMobile.Text.Trim();

            if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
            {
                lblAltMobileNo.InnerText = "Mobile No and alternate mobile numbers cannot be the same.";
                lblAltMobileNo.Attributes.Add("style", "display: block;");
                txtAlternativeMobile.Focus();
                return;
            }
            else { lblAltMobileNo.Attributes.Add("style", "display: none;"); }
            lblAltMobileNo.InnerText = "";
            lblAltMobileNo.Attributes.Add("style", "display: none;");
        }
        protected void txtWhatsCustomerMobile_TextChanged(object sender, EventArgs e)
        {
            bool blockuser = CheckBlockCustomer("", txtWhatsappNo.Text);
            bool existuser = CheckSalesPersonOrRetailer("", txtWhatsappNo.Text);
            if (existuser)
            {
                lblWhatsAppError.InnerText = "This Mobile No is already registered";
                lblWhatsAppError.Attributes.Add("style", "display: block;");
                return;
            }
            else if (blockuser)
            {
                lblWhatsAppError.InnerText = lblAltMobileNo.InnerText;
                lblWhatsAppError.Attributes.Add("style", "display: block;");
                return;
            }
            if (!string.IsNullOrEmpty(txtWhatsappNo.Text) && txtWhatsappNo.Text.Replace("+91", "").Length != 10)
            {
                lblWhatsAppError.InnerText = "Please enter your phone number.";
                lblWhatsAppError.Attributes.Add("style", "display: block;");
                txtWhatsappNo.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtWhatsappNo.Text) && Regex.IsMatch(txtWhatsappNo.Text, @"^[0-5]"))
            {
                lblWhatsAppError.InnerText = "Invalid number";
                lblWhatsAppError.Attributes.Add("style", "display: block;");
                txtWhatsappNo.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtWhatsappNo.Text) && Regex.IsMatch(txtWhatsappNo.Text, @"^(\d)\1{9}$"))
            {
                lblWhatsAppError.InnerText = "Invalid number.";
                lblWhatsAppError.Attributes.Add("style", "display: block;");
                lblWhatsAppError.Focus();
                return;
            }
            else { lblWhatsAppError.Attributes.Add("style", "display: none;"); }
            lblAltMobileNo.InnerText = "";
            lblWhatsAppError.InnerText = "";
            lblWhatsAppError.Attributes.Add("style", "display: none;");
            txtAlternativeEmail.Focus();
        }
        protected void txtCustomerEmail_TextChanged(object sender, EventArgs e)
        {
            bool blockuser = CheckBlockCustomer(txtEmail.Text, "");
            bool existuser = CheckSalesPersonOrRetailer(txtEmail.Text, "");
            if (existuser)
            {
                lblEmailAddress.InnerText = "This Email is already registered";
                lblAltMobileNo.InnerText = "";
                lblEmailAddress.Attributes.Add("style", "display: block;");
                txtEmail.Focus();
                return;
            }
            else if (blockuser)
            {
                lblEmailAddress.InnerText = lblAltMobileNo.InnerText;
                lblAltMobileNo.InnerText = "";
                lblEmailAddress.Attributes.Add("style", "display: block;");
                txtEmail.Focus();
                return;
            }
            string mobile = txtEmail.Text.Trim();
            string altMobile = txtAlternativeEmail.Text.Trim();

            if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
            {
                lblEmailAddress.InnerText = "Email ID and alternate Email ID cannot be the same.";
                lblEmailAddress.Attributes.Add("style", "display: block;");
                txtEmail.Focus();
                return;
            }
            else if (!Regex.IsMatch(txtEmail.Text.Trim(), @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
            {
                lblEmailAddress.InnerText = "Invalid email format.";
                lblEmailAddress.Attributes.Add("style", "display: block;");
                txtEmail.Focus();
                return;
            }
            lblEmailAddress.InnerText = "";
            lblEmailAddress.Attributes.Add("style", "display: none;");
            lblAtlEmail.InnerText = "";
            lblAtlEmail.Attributes.Add("style", "display: none;");
            txtAlternativeEmail.Focus();
        }
        protected void txtAltEmail_TextChanged(object sender, EventArgs e)
        {
            bool blockuser = CheckBlockCustomer(txtAlternativeEmail.Text, "");
            bool existuser = CheckSalesPersonOrRetailer(txtAlternativeEmail.Text, "");
            if (existuser)
            {
                lblAtlEmail.InnerText = "This Email is already registered";
                lblAltMobileNo.InnerText = "";
                lblAtlEmail.Attributes.Add("style", "display: block;");
                return;
            }
            else if (blockuser)
            {
                lblAtlEmail.InnerText = lblAltMobileNo.InnerText;
                lblAltMobileNo.InnerText = "";
                lblAtlEmail.Attributes.Add("style", "display: block;");
                return;
            }
            string mobile = txtEmail.Text.Trim();
            string altMobile = txtAlternativeEmail.Text.Trim();

            if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
            {
                lblAtlEmail.InnerText = "Email ID and alternate Email ID cannot be the same.";
                lblAtlEmail.Attributes.Add("style", "display: block;");
                txtAlternativeEmail.Focus();
                return;
            }
            if (!string.IsNullOrWhiteSpace(txtAlternativeEmail.Text) && !Regex.IsMatch(txtAlternativeEmail.Text.Trim(), @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
            {
                lblAtlEmail.InnerText = "Invalid email format.";
                lblAtlEmail.Attributes.Add("style", "display: block;");
                txtAlternativeEmail.Focus();
                return;
            }
            lblAtlEmail.InnerText = "";
            lblAtlEmail.Attributes.Add("style", "display: none;");
            txtPincode.Focus();
        }

        protected bool CheckBlockCustomer(string email, string mobileno)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerMobileNo", SqlDbType.VarChar).Value = !string.IsNullOrWhiteSpace(mobileno) ? mobileno.ToString() : null;
                cmd.Parameters.AddWithValue("@CustomerEmailID", SqlDbType.VarChar).Value = !string.IsNullOrWhiteSpace(email) ? email.ToString() : null;
                cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 39;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string suspicious = dt.Rows[0]["suspicious"].ToString();
                    if (suspicious == "1")
                    {
                        lblAltMobileNo.InnerText = "This Customer is Under Watch.Please contact your Manager";
                        return true;
                    }
                    else if (suspicious == "2")
                    {
                        lblAltMobileNo.InnerText = "This Customer is Black Listed..Please contact your Manager";
                        return true;
                    }
                    else if (suspicious == "")
                    {
                        lblAltMobileNo.InnerText = "";
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
        }
        protected bool CheckSalesPersonOrRetailer(string email, string mobileno)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerMobileNo", !string.IsNullOrWhiteSpace(mobileno) ? mobileno : null);
                    cmd.Parameters.AddWithValue("@CustomerEmailID", !string.IsNullOrWhiteSpace(email) ? email : null);
                    cmd.Parameters.AddWithValue("@Mid", "");
                    cmd.Parameters.AddWithValue("@type", 50);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        lblAltMobileNo.InnerText = "This Mobile No or Email is already registered";
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected bool AddPayment()
        {
            try
            {
                foreach (RepeaterItem item in rptPlans.Items)
                {
                    Label lblPlanPrice = item.FindControl("lblPlanPrice") as Label;
                    HiddenField hdnUID = item.FindControl("hdnUID") as HiddenField;

                    decimal taxableValue = Convert.ToDecimal(lblPlanPrice.Text);
                    decimal taxableamount = Math.Round(taxableValue / 1.18m, 2);
                    decimal taxAmount = Math.Round(taxableamount * 0.18m, 2);

                    decimal commision = calculateCommision(taxableValue);
                    decimal Taxablecommision = Math.Round(commision / 1.18m, 2);
                    decimal commisionTax = Math.Round(Taxablecommision * 0.18m, 2);
                    decimal totalAmountPay = (taxableamount - Taxablecommision) + taxAmount;

                    using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@type", 69);
                        cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"].ToString());
                        cmd.Parameters.AddWithValue("@Saleschannel", "Retailer");
                        cmd.Parameters.AddWithValue("@TaxableValue", taxableamount);
                        cmd.Parameters.AddWithValue("@TaxAmout", taxAmount);
                        cmd.Parameters.AddWithValue("@TotalAmountPay", totalAmountPay);
                        if (Commossiontag.InnerText.Contains("%"))
                        {
                            cmd.Parameters.AddWithValue("@CommissionType", "Percantage");
                            cmd.Parameters.AddWithValue("@CommissionValue", "0");
                            cmd.Parameters.AddWithValue("@CommissionPercentage", ViewState["CommissionPercantage"] != null ? ViewState["CommissionPercantage"].ToString() : "0");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@CommissionType", "Value");
                            cmd.Parameters.AddWithValue("@CommissionValue", commision);
                            cmd.Parameters.AddWithValue("@CommissionPercentage", "0");
                        }
                        cmd.Parameters.AddWithValue("@GSTCommissionValue", "");
                        cmd.Parameters.AddWithValue("@GSTCommissionPercentage", "18");
                        cmd.Parameters.AddWithValue("@GSTCommissionPaid", "No");
                        cmd.Parameters.AddWithValue("@TranStatus", "Pending");
                        cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OrderId", "");
                        cmd.Parameters.AddWithValue("@UID", hdnUID.Value);
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["RetailerUniqueID"].ToString());
                        string mes = Request.QueryString["pay"];
                        if (string.IsNullOrWhiteSpace(mes) && mes != "cancel")
                        {
                            cmd.Parameters.AddWithValue("@PaymentMethod", "New Payment");
                        }
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected bool AddPaymentSalesPerson()
        {
            try
            {
                foreach (RepeaterItem item in rptPlans.Items)
                {
                    Label lblPlanPrice = item.FindControl("lblPlanPrice") as Label;
                    HiddenField hdnUID = item.FindControl("hdnUID") as HiddenField;

                    decimal taxableValue = Convert.ToDecimal(lblPlanPrice.Text);
                    decimal taxableamount = Math.Round(taxableValue / 1.18m, 2);
                    decimal taxAmount = Math.Round(taxableamount * 0.18m, 2);

                    decimal commision = calculateCommision(taxableValue);
                    decimal Taxablecommision = Math.Round(commision / 1.18m, 2);
                    decimal commisionTax = Math.Round(Taxablecommision * 0.18m, 2);
                    decimal totalAmountPay = (taxableamount - Taxablecommision) + taxAmount;

                    using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@type", 85);
                        cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"].ToString());
                        cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString());
                        cmd.Parameters.AddWithValue("@Saleschannel", "Retailer");
                        cmd.Parameters.AddWithValue("@TaxableValue", taxableamount);
                        cmd.Parameters.AddWithValue("@TaxAmout", taxAmount);
                        cmd.Parameters.AddWithValue("@TotalAmountPay", totalAmountPay);
                        if (Commossiontag.InnerText.Contains("%"))
                        {
                            cmd.Parameters.AddWithValue("@CommissionType", "Percantage");
                            cmd.Parameters.AddWithValue("@CommissionValue", "0");
                            cmd.Parameters.AddWithValue("@CommissionPercentage", ViewState["CommissionPercantage"] != null ? ViewState["CommissionPercantage"].ToString() : "0");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@CommissionType", "Value");
                            cmd.Parameters.AddWithValue("@CommissionValue", commision);
                            cmd.Parameters.AddWithValue("@CommissionPercentage", "0");
                        }
                        cmd.Parameters.AddWithValue("@GSTCommissionValue", "");
                        cmd.Parameters.AddWithValue("@GSTCommissionPercentage", "18");
                        cmd.Parameters.AddWithValue("@GSTCommissionPaid", "No");
                        cmd.Parameters.AddWithValue("@TranStatus", "Pending");
                        cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OrderId", "");
                        cmd.Parameters.AddWithValue("@UID", hdnUID.Value);
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["RetailerUniqueID"].ToString());
                        string mes = Request.QueryString["pay"];
                        if (string.IsNullOrWhiteSpace(mes) && mes != "cancel")
                        {
                            cmd.Parameters.AddWithValue("@PaymentMethod", "New Payment");
                        }
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected void ddlPlanChange(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlPlan = (DropDownList)sender;
                RepeaterItem item = (RepeaterItem)ddlPlan.NamingContainer;

                string selectedPlanId = ddlPlan.SelectedValue;
                decimal newPrice = GetPlanPriceFromDB(selectedPlanId);

                Label lblPlanPrice = item.FindControl("lblPlanPrice") as Label;
                ViewState["OriginalPrice"] = lblPlanPrice.Text;
                if (lblPlanPrice != null)
                {
                    lblPlanPrice.Text = newPrice.ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }
        private decimal GetPlanPriceFromDB(string planId)
        {
            decimal price = 0;
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 76);
                cmd.Parameters.AddWithValue("@SKU", planId);

                if (con.State != ConnectionState.Open)
                    con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        decimal mrp = reader["MRP"] != DBNull.Value ? Convert.ToDecimal(reader["MRP"]) : 0;
                        decimal discount = reader["Discount"] != DBNull.Value ? Convert.ToDecimal(reader["Discount"]) : 0;

                        price = mrp - (mrp * discount / 100);
                    }
                }
                con.Close();
            }
            return price;
        }





    }
}