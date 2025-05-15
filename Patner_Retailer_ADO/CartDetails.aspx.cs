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
                BindProductInfo();
                BindOrderSummary();
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

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        rptPlans.DataSource = dt;
                        rptPlans.DataBind();
                    }
                    else
                    {
                        rptPlans.DataSource = null;
                        rptPlans.DataBind();
                        Response.Redirect("BuyInfySalePlan.aspx");
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
                decimal taxAmount = Math.Round(taxableValue * 0.18m, 2);
                decimal totalAmountPay = taxableValue + taxAmount;

                txtQuantity.InnerText = totalQuantity.ToString();
                TaxableValue.InnerText = "₹" + taxableValue.ToString("N2");
                TaxAmout.InnerText = "₹" + taxAmount.ToString("N2");
                TotalAmountPay.InnerText = "₹" + totalAmountPay.ToString("N2");
            }
            catch (Exception)
            {
                return;
            }
        }
        protected void txtPIN_TextChanged(object sender, EventArgs e)
        {
            try
            {
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
                string[] words = txtAddressLine1.Value.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length < 3)
                {
                    lblAddressError.Text = "Address must contain at least 3 words.";
                    lblAddressError.Visible = true;
                    return;
                }
                else
                {
                    lblAddressError.Text = "";
                    lblAddressError.Visible = false;
                }
                if (!string.IsNullOrWhiteSpace(txtLandmark.Value))
                {
                    string[] landmarkwords = txtLandmark.Value.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (landmarkwords.Length < 3)
                    {
                        lblLandmarkError.Text = "Landmark must contain at least 3 words.";
                        lblLandmarkError.Visible = true;
                        return;
                    }
                    else
                    {
                        lblLandmarkError.Text = "";
                        lblLandmarkError.Visible = false;
                    }
                }
                if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || 
                    string.IsNullOrWhiteSpace(txtCustomerMobileNo.Text) || string.IsNullOrWhiteSpace(txtWhatsappNo.Text) || 
                    string.IsNullOrWhiteSpace(txtPincode.Text) || string.IsNullOrWhiteSpace(txtCity.Text) || string.IsNullOrWhiteSpace(txtState.Text) ||
                    string.IsNullOrWhiteSpace(txtAddressLine1.Value) || string.IsNullOrWhiteSpace(txtLandmark.Value) || txtCustomerMobileNo.Text.Length != 10 ||
                    txtWhatsappNo.Text.Length != 10 || txtPincode.Text.Length != 6)
                {
                    return;
                }

                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@type", 7);
                    cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"].ToString());
                    cmd.Parameters.AddWithValue("@CustomerName", txtFirstName.Text.ToString() + " " + txtLastName.Text.ToString());
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
                    cmd.Parameters.AddWithValue("@installedLandmark", txtinstalledLandmark.Value.ToString());
                    cmd.Parameters.AddWithValue("@Saleschannel", "Retailer");
                    cmd.Parameters.AddWithValue("@WhatsappNo", txtWhatsappNo.Text.ToString());
                    cmd.Parameters.AddWithValue("@TaxableValue", TaxableValue.InnerText.Replace("₹", "").Replace(",", "").Trim());
                    cmd.Parameters.AddWithValue("@TaxAmout", TaxAmout.InnerText.Replace("₹", "").Replace(",", "").Trim());
                    cmd.Parameters.AddWithValue("@TotalAmountPay", TotalAmountPay.InnerText.Replace("₹", "").Replace(",", "").Trim());
                    cmd.Parameters.AddWithValue("@TranStatus", "Pending");
                    cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@OrderId", "");
                    cmd.Parameters.AddWithValue("@CreatedBy", Session["RetailerUniqueID"].ToString());

                    con.Open();
                    cmd.ExecuteNonQuery();

                    ViewState["ItemPrice"] = "1";
                    //  ViewState["ItemPrice"]= TotalAmountPay.InnerText.Replace("₹", "").Replace(",", "").Trim();
                    getpaytm();
                }
                con.Close();

                DisplayMessage(this, "Record Added Successfully");
                return;
                //Response.Redirect("CartDetails.aspx");
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


            HttpContext.Current.Cache.Insert("TransactionId", Session["salesOrderID"], null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
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
            parameters.Add("ORDER_ID", Session["salesOrderID"].ToString());
            parameters.Add("TXN_AMOUNT", ViewState["ItemPrice"].ToString());

            parameters.Add("CALLBACK_URL", "http://localhost:44310/PaymentConfirmation.aspx?m=" + Session["mode"] + "&Mobile=" + Session["MobileNo"] + "&RetailerUniqueID=" + Session["RetailerUniqueID"] + "&Role=" + Session["Role"]);

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
            Response.Redirect("BuyInfySalePlan.aspx");
        }

        protected void DeletePlanInfo(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkDelete = (LinkButton)sender;
                string mid = lnkDelete.CommandArgument;

                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Type", 14);
                cmd.Parameters.AddWithValue("@Mid", mid);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                BindProductInfo();
            }
            catch (Exception ex)
            {
            }
        }
    }
}