using paytm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System.Xml.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Globalization;
using PdfSharp.Pdf;
using PdfSharp.Drawing;


namespace Patner_Retailer_ADO
{
    public partial class PaymentConfirmation : System.Web.UI.Page
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
                var qe = Request.QueryString["qu"];
                if (!string.IsNullOrWhiteSpace(qe))
                {
                    TicketPanel.Visible = false;
                    Session["salesOrderID"] = qe;
                    BindProductInfo();
                    Session.Remove("salesOrderID");
                }
                else
                    BindPaymentInfo();
            }
        }

        protected void BindPaymentInfo()
        {
            try
            {
                var querymode = Request.QueryString["m"];
                var Mobile = Request.QueryString["Mobile"];
                var RetailerUniqueID = Request.QueryString["RetailerUniqueID"];
                var Role = Request.QueryString["Role"];
                if (querymode == "PayTm")
                {
                    //Session["salesOrderID"] = HttpContext.Current.Cache["TransactionId"].ToString();
                    string full = HttpContext.Current.Cache["TransactionId"].ToString();
                    Session["salesOrderID"] = full.Substring(0, 16);
                    Session["UID"] = full.Substring(16);
                    Session["MobileNo"] = Mobile;
                    Session["RetailerUniqueID"] = RetailerUniqueID;
                    Session["Role"] = Role;
                    String merchantKey = "Xv#3x9vZ%cawdcD1";
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    string paytmChecksum = "";
                    foreach (string key in Request.Form.Keys)
                    {
                        parameters.Add(key.Trim(), Request.Form[key].Trim());
                    }
                    if (parameters.ContainsKey("CHECKSUMHASH"))
                    {
                        paytmChecksum = parameters["CHECKSUMHASH"];
                        parameters.Remove("CHECKSUMHASH");
                    }
                    if (CheckSum.verifyCheckSum(merchantKey, parameters, paytmChecksum))
                    {
                        string msg = parameters["STATUS"];
                        string order_id = parameters["ORDERID"];
                        string respMsg = Request.Form["RESPMSG"];
                        if (msg == "TXN_SUCCESS")
                        {
                            //lblSucess1.Text = "Your payment transaction is successfully completed.";

                            BindProductInfo();
                            CreateNewTickets();
                            DataTable dt = ViewState["AllInfo"] as DataTable;
                            if (dt != null)
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    row["TranStatus"] = "Paid";
                                }
                                GVPlanDetails.DataSource = dt;
                                GVPlanDetails.DataBind();
                                if (GVPlanDetails.HeaderRow != null)
                                {
                                    GVPlanDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                                }
                            }
                            DeleteIncompeteInfo("Deleted", "Payment transaction is successfully completed");
                            Session.Remove("salesOrderID");
                            //Retailer
                            //SendMail(Session["Email"].ToString(), "Retailer");
                            //SendWhatsApp("8077721485", "Afzal", "110025", "Delhi", "New Delhi", "Batla House", "Customer", "");
                            //Customer
                            SendMail("techmasters@infinityassurance.com", "Customer");
                            SendWhatsApp("8920646143", "Amaan", "110094", "Delhi", "New Delhi", "Old Mustafabad", "Retailer", "ABC");
                        }
                        else if (msg == "TXN_FAILURE" && respMsg != null && respMsg.ToLower().Contains("user has not completed transaction."))
                        {
                            Response.Redirect("CartDetails.aspx?pay=cancel", false);
                            Context.ApplicationInstance.CompleteRequest();
                            return;

                        }
                        else
                        {
                            PaymentFailed();
                            UpdateCommision("", "Failed", "");
                            DeleteIncompeteInfo("Payment Failed", "Payment transaction is Failed ");
                            lblSucess1.Text = "Your payment transaction is Failed !!";
                            lblSucess1.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        Response.Write("Checksum MisMatch");
                    }
                }
            }
            catch (Exception ex)
            {
                PaymentFailed();
                UpdateCommision("", "Failed", "");
                DeleteIncompeteInfo("Payment Failed", "Payment transaction is Failed ");
                DisplayMessage(this, ex.Message);
                lblSucess1.Text = "Your payment transaction is Failed !!";
                lblSucess1.ForeColor = Color.Red;
            }
        }
        //protected void BindProductInfo()
        //{
        //    try
        //    {
        //        using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@type", 8);
        //            cmd.Parameters.AddWithValue("@SalesOrderID", "0825-INFY-000082");// Session["salesOrderID"].ToString());

        //            if (con.State != ConnectionState.Open)
        //                con.Open();
        //            DataSet ds = new DataSet();
        //            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        //            {
        //                da.Fill(ds);
        //            }

        //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //            {
        //                GvCustomerDetails.DataSource = ds.Tables[0].DefaultView.ToTable(true, "MobileNo", "WhatsappNo", "EmailIDAddress", "Pincode", "City", "State", "AddressLine1");
        //                GvCustomerDetails.DataBind();

        //                GVProductDetails.DataSource = ds.Tables[0].DefaultView.ToTable(true, "Productname", "Productsubcategoryname", "Brand", "ModalName", "serialno", "DevicePurchasePrice", "DateofImplementation", "ManufacturerWarranty_yymmdd");
        //                GVProductDetails.DataBind();

        //                if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
        //                {
        //                    DataTable dtCombined = new DataTable();
        //                    dtCombined.Columns.AddRange(new DataColumn[]
        //                    {
        //                        new DataColumn("PlanNicknameSelection"),
        //                        new DataColumn("PlanSKU"),
        //                        new DataColumn("MRP", typeof(decimal)),
        //                        new DataColumn("PlanPrice", typeof(decimal)),
        //                        new DataColumn("TaxableValue", typeof(decimal)),
        //                        new DataColumn("TaxAmout", typeof(decimal)),
        //                        new DataColumn("Commission", typeof(decimal)),
        //                        new DataColumn("CommissionTaxableValue", typeof(decimal)),
        //                        new DataColumn("CommissionTaxValue", typeof(decimal)),
        //                        new DataColumn("TotalAmountPay", typeof(decimal)),
        //                        new DataColumn("TranStatus"),
        //                        new DataColumn("PaymentDate")
        //                    });

        //                    int rowCount = Math.Min(ds.Tables[0].Rows.Count, ds.Tables[1].Rows.Count);

        //                    for (int i = 0; i < rowCount; i++)
        //                    {
        //                        var planInfo = ds.Tables[0].Rows[i];
        //                        var paymentInfo = ds.Tables[1].Rows[i];

        //                        decimal planPrice = 0;
        //                        decimal.TryParse(planInfo["PlanPrice"].ToString(), out planPrice);

        //                        string commissionType = paymentInfo["CommissionType"].ToString();
        //                        decimal commissionValue = Convert.ToDecimal(paymentInfo["CommissionValue"] ?? 0);
        //                        decimal commissionPercentage = Convert.ToDecimal(paymentInfo["CommissionPercentage"] ?? 0);

        //                        decimal commission = commissionType == "Percantage"
        //                            ? (commissionPercentage / 100) * planPrice
        //                            : commissionValue;

        //                        decimal commissionTaxableValue = commission / 1.18m;
        //                        decimal commissionTaxValue = commission - commissionTaxableValue;

        //                        dtCombined.Rows.Add(
        //                            planInfo["PlanNicknameSelection"],
        //                            planInfo["PlanSKU"],
        //                            Convert.ToDecimal(planInfo["MRP"] ?? 0),
        //                            planPrice,
        //                            Convert.ToDecimal(paymentInfo["TaxableValue"] ?? 0),
        //                            Convert.ToDecimal(paymentInfo["TaxAmout"] ?? 0),
        //                            commission,
        //                            commissionTaxableValue,
        //                            commissionTaxValue,
        //                            Convert.ToDecimal(paymentInfo["TotalAmountPay"] ?? 0),
        //                            paymentInfo["TranStatus"],
        //                            paymentInfo["PaymentDate"]
        //                        );
        //                    }

        //                    GVPlanDetails.DataSource = dtCombined;
        //                    GVPlanDetails.DataBind();

        //                    ViewState["CustomerInfo"] = ds.Tables[0];
        //                    ViewState["PaymentInfo"] = ds.Tables[1];
        //                }


        //            }

        //            GvCustomerDetails.CssClass = "table data-table table-striped nowrap";
        //            GVProductDetails.CssClass = "table data-table table-striped nowrap";
        //            GVPlanDetails.CssClass = "table data-table table-striped nowrap";

        //            if (GvCustomerDetails.HeaderRow != null)
        //                GvCustomerDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
        //            if (GVProductDetails.HeaderRow != null)
        //                GVProductDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
        //            if (GVPlanDetails.HeaderRow != null)
        //                GVPlanDetails.HeaderRow.TableSection = TableRowSection.TableHeader;

        //            TicketInfoPanel.Visible = true;
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}


        protected void BindProductInfo()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 8);
                    cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"].ToString());
                    if(Session["Role"] != null && Session["Role"].ToString() == "Agent")
                        cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ViewState["AllInfo"] = dt;
                    con.Close();

                    if (dt.Rows.Count > 0)
                    {

                        GvCustomerDetails.DataSource = dt;
                        GvCustomerDetails.DataBind();

                        GVProductDetails.DataSource = dt;
                        GVProductDetails.DataBind();

                        GVPlanDetails.DataSource = dt;
                        GVPlanDetails.DataBind();

                        DataRow row = dt.Rows[0];

                        lblCustomerName.Text = row["CustomerName"] != DBNull.Value ? row["CustomerName"].ToString() : "";
                        var productNames = dt.AsEnumerable().Select(r => r.Field<string>("Productname")).Where(name => !string.IsNullOrEmpty(name)).Distinct().ToList();
                        lblProductName.Text = string.Join(", ", productNames);

                        var planNames = dt.AsEnumerable().Select(r => r.Field<string>("PlanNicknameSelection")).Where(name => !string.IsNullOrEmpty(name)).Distinct().ToList();
                        lblPlanName.Text = string.Join(", ", planNames);
                        //lblProductName.Text = row["Productname"] != DBNull.Value ? row["Productname"].ToString() : "";
                        //lblPlanName.Text = row["PlanNicknameSelection"] != DBNull.Value ? row["PlanNicknameSelection"].ToString() : "";
                        //List<string> ticketList = new List<string>();
                        //foreach (DataRow r in dt.Rows)
                        //{
                        //    if (r["TicketNO"] != DBNull.Value)
                        //    {
                        //        string ticket = r["TicketNO"].ToString().Trim();
                        //        if (!string.IsNullOrEmpty(ticket))
                        //            ticketList.Add(ticket);
                        //    }
                        //}
                        //ticketNo.InnerText = string.Join(", ", ticketList);

                        GvCustomerDetails.CssClass = "table data-table table-striped nowrap";
                        GVProductDetails.CssClass = "table data-table table-striped nowrap";
                        GVPlanDetails.CssClass = "table data-table table-striped nowrap";
                        if (GvCustomerDetails.HeaderRow != null)
                        {
                            GvCustomerDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                        if (GVProductDetails.HeaderRow != null)
                        {
                            GVProductDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                        if (GVPlanDetails.HeaderRow != null)
                        {
                            GVPlanDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }

                        TicketInfoPanel.Visible = true;
                    }
                    else
                    {
                        //rptPlans.DataSource = null;
                        //rptPlans.DataBind();

                        //rptProductInfo.DataSource = null;
                        //rptProductInfo.DataBind();

                        //rptPlanInfo.DataSource = null;
                        //rptPlanInfo.DataBind();
                        lblCustomerName.Text = "";
                        lblProductName.Text = "";
                        lblPlanName.Text = "";
                        ticketNo.InnerText = "";

                        GvCustomerDetails.DataSource = null;
                        GvCustomerDetails.DataBind();

                        GVProductDetails.DataSource = null;
                        GVProductDetails.DataBind();

                        GVPlanDetails.DataSource = null;
                        GVPlanDetails.DataBind();

                        GvCustomerDetails.CssClass = "table table-striped nowrap";
                        GVProductDetails.CssClass = "table table-striped nowrap";
                        GVPlanDetails.CssClass = "table table-striped nowrap";
                        TicketInfoPanel.Visible = false;
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        protected void CreateNewTickets()
        {
            if (ViewState["AllInfo"] != null)
            {
                DataTable dt = ViewState["AllInfo"] as DataTable;

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    string ProxyName = "", ProxyMobile1 = "", ProxyRelationship = "0", ProxyEmail = "", ProxyMobilewhatsappno = "";
                    DataTable proxyDetails = ProxyInfo();
                    if (proxyDetails != null && proxyDetails.Rows.Count > 0)
                    {
                        DataRow dataRow = proxyDetails.Rows[0];
                        ProxyName = dataRow["Name"].ToString();
                        ProxyMobile1 = dataRow["MobileNo"].ToString();
                        ProxyRelationship = "1";
                        ProxyEmail = dataRow["EmailID"].ToString();
                        ProxyMobilewhatsappno = dataRow["MobileNo_CheckWhatsapp"].ToString();
                        Session["Email"] = dataRow["EmailID"].ToString();

                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        DataTable resultDt = null;

                        string manufacturewarrantystartdate = string.Empty;
                        string manufacturewarrantyenddate = string.Empty;
                        string sdpstartdate = string.Empty;
                        string sdpenddate = string.Empty;
                        string adpstartdate = string.Empty;
                        string adpenddate = string.Empty;
                        string extendedwarrantystartdate = string.Empty;
                        string extendedwarrantyenddate = string.Empty;

                        // Check if both fields have values
                        string planName = row["PlanName"].ToString().Trim().Replace("&nbsp;", "");
                        string planNameSelection = row["PlanNicknameSelection"].ToString().Trim().Replace("&nbsp;", "");
                        string newPlanId = row["planMid"].ToString().Trim().Replace("&nbsp;", "");
                        string PlanSKU = row["PlanSKU"].ToString().Trim().Replace("&nbsp;", "");
                        string dateofpurchase = row["ProductPurchaseDate"].ToString().Trim().Replace("&nbsp;", "");
                        string planid = string.Empty;
                        var yymmdd = row["ManufacturerWarranty_yymmdd"].ToString().Trim().Replace("&nbsp;", "");
                        string[] parts = yymmdd.Split('/');

                        int yy = (parts.Length > 0 && int.TryParse(parts[0], out int y)) ? y : 0;
                        int mm = (parts.Length > 1 && int.TryParse(parts[1], out int m)) ? m : 0;
                        int dd = (parts.Length > 2 && int.TryParse(parts[2], out int d)) ? d : 0;
                        using (SqlCommand getPlanIdCmd = new SqlCommand("SELECT producttypeid FROM iapl_producttype WHERE description = @desc", con))
                        {
                            getPlanIdCmd.Parameters.AddWithValue("@desc", planNameSelection);
                            object result = getPlanIdCmd.ExecuteScalar();
                            planid = result != null ? result.ToString() : null;
                        }
                        if (planid != "" && dateofpurchase != "")
                        {
                            resultDt = getServicePlanCreate(planNameSelection, dateofpurchase);
                        }
                        if (resultDt != null && resultDt.Rows.Count > 0)
                        {
                            DataRow row10 = resultDt.Rows[0];
                            manufacturewarrantystartdate = row10["ManufacturingStartDate"].ToString();
                            manufacturewarrantyenddate = row10["ManufacturingEndDate"].ToString();
                            sdpstartdate = row10["SDPStartDate"].ToString();
                            sdpenddate = row10["SDPEndDate"].ToString();
                            adpstartdate = row10["ADPStartDate"].ToString();
                            adpenddate = row10["ADPEndDate"].ToString();
                            extendedwarrantystartdate = row10["EWSStartDate"].ToString();
                            extendedwarrantyenddate = row10["EWSEndDate"].ToString();
                        }
                        string IPRN = GenerateIPPNno();
                        string subcatid = row["categoryid"].ToString().Trim().Replace("&nbsp;", "");
                        //subcatid = getSubcatid(row["SubCategoryID"].ToString().Trim().Replace("&nbsp;", ""));
                        SqlCommand cmdreg = new SqlCommand("IAPL_CRM_stockgenerate_Infysales", con);
                        cmdreg.CommandType = CommandType.StoredProcedure;

                        cmdreg.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 1001;
                        cmdreg.Parameters.AddWithValue("@PlanID", SqlDbType.VarChar).Value = planid;
                        cmdreg.Parameters.AddWithValue("@ProCat", SqlDbType.NVarChar).Value = subcatid; //Cat Id
                        cmdreg.Parameters.AddWithValue("@ProSubcatID", string.IsNullOrEmpty(row["SubCategoryID"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["SubCategoryID"].ToString().Trim().Replace("&nbsp;", ""));//Sub cat Id
                        cmdreg.Parameters.AddWithValue("@ProductSubCatgID", string.IsNullOrEmpty(row["ProductTypeID"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["ProductTypeID"].ToString().Trim().Replace("&nbsp;", ""));//Product Type
                        cmdreg.Parameters.AddWithValue("@invoiceamount_productdetails", string.IsNullOrEmpty(row["DevicePurchasePrice"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["DevicePurchasePrice"].ToString().Trim().Replace("&nbsp;", ""));//Invoice Amount Product
                        //Length of Warranty
                        cmdreg.Parameters.AddWithValue("@warrnatyYear", yy);
                        cmdreg.Parameters.AddWithValue("@WarrantyMonth", mm);
                        cmdreg.Parameters.AddWithValue("@WarrantyDay", dd);

                        cmdreg.Parameters.AddWithValue("@regdate", DateTime.Now);
                        DateTime invoicedate_productdetails;
                        if (DateTime.TryParse(dateofpurchase, out invoicedate_productdetails))
                        {
                            cmdreg.Parameters.AddWithValue("@invoicedate_productdetails", invoicedate_productdetails.ToString("MM/dd/yyyy"));
                        }
                        else
                        {
                            cmdreg.Parameters.AddWithValue("@invoicedate_productdetails", DBNull.Value);
                        }
                        cmdreg.Parameters.AddWithValue("@customername", string.IsNullOrEmpty(row["CustomerName"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["CustomerName"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@ContactPerson", string.IsNullOrEmpty(row["CustomerName"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["CustomerName"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@serialno", string.IsNullOrEmpty(row["serialno"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["serialno"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@IMEINo", string.IsNullOrEmpty(row["imei"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["imei"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@brand", string.IsNullOrEmpty(row["Brand"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["Brand"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@SubProductType", string.IsNullOrEmpty(row["SubProductTypeID"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["SubProductTypeID"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@Make", string.IsNullOrEmpty(row["Brand"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["Brand"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@model", string.IsNullOrEmpty(row["ModalName"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["ModalName"].ToString().Trim().Replace("&nbsp;", ""));
                        //cmdreg.Parameters.AddWithValue("@ModelNo", string.IsNullOrEmpty(row["ModelNo"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["ModelNo"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@WhatsAppNo", string.IsNullOrEmpty(row["WhatsappNo"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["WhatsappNo"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@addressline1", string.IsNullOrEmpty(row["AddressLine1"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["AddressLine1"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@city", string.IsNullOrEmpty(row["City"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["City"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@state", string.IsNullOrEmpty(row["State"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["State"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@pincode", string.IsNullOrEmpty(row["Pincode"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["Pincode"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@mobileno", string.IsNullOrEmpty(row["MobileNo"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["MobileNo"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@emailidaddress", string.IsNullOrEmpty(row["EmailIDAddress"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["EmailIDAddress"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@productInstalledAddressLine1", string.IsNullOrEmpty(row["ProductInstallAddress"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["ProductInstallAddress"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@ProductInstalledLandmark", string.IsNullOrEmpty(row["installedLandmark"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["installedLandmark"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@ProductInstalledPincode", string.IsNullOrEmpty(row["InstalledPincode"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["InstalledPincode"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@ProductInstalledCity", string.IsNullOrEmpty(row["InstalledCity"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["InstalledCity"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@ProductInstalledState", string.IsNullOrEmpty(row["InstalledState"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["InstalledState"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdreg.Parameters.AddWithValue("@ClientID", SqlDbType.NVarChar).Value = 2;
                        cmdreg.Parameters.AddWithValue("@ProjectId", SqlDbType.NVarChar).Value = 11;
                        cmdreg.Parameters.AddWithValue("@IPRN", IPRN);
                        cmdreg.Parameters.AddWithValue("@Status", SqlDbType.VarChar).Value = "Under Approval";
                        cmdreg.Parameters.AddWithValue("@adpstartdate", adpstartdate);
                        cmdreg.Parameters.AddWithValue("@adpenddate", adpenddate);
                        cmdreg.Parameters.AddWithValue("@EWSstart", extendedwarrantystartdate);
                        cmdreg.Parameters.AddWithValue("@EWSEND", extendedwarrantyenddate);
                        if (!string.IsNullOrWhiteSpace(manufacturewarrantystartdate?.ToString()))
                        {
                            DateTime parsedDate;
                            if (DateTime.TryParse(manufacturewarrantystartdate.ToString(), out parsedDate))
                            {
                                cmdreg.Parameters.AddWithValue("@manufacturewarrantystartdate", parsedDate.ToString("MM/dd/yyyy"));
                            }
                            else
                            {
                                cmdreg.Parameters.AddWithValue("@manufacturewarrantystartdate", DBNull.Value);
                            }
                        }
                        else
                        {
                            cmdreg.Parameters.AddWithValue("@manufacturewarrantystartdate", DBNull.Value);
                        }
                        if (!string.IsNullOrWhiteSpace(manufacturewarrantystartdate?.ToString()))
                        {
                            DateTime parsedDate;
                            if (DateTime.TryParse(manufacturewarrantystartdate.ToString(), out parsedDate))
                            {
                                cmdreg.Parameters.AddWithValue("@manufacturewarrantyenddate", parsedDate.ToString("MM/dd/yyyy"));
                            }
                            else
                            {
                                cmdreg.Parameters.AddWithValue("@manufacturewarrantyenddate", DBNull.Value);
                            }
                        }
                        else
                        {
                            cmdreg.Parameters.AddWithValue("@manufacturewarrantyenddate", DBNull.Value);
                        }

                        //cmdreg.Parameters.AddWithValue("@manufacturewarrantystartdate", manufacturewarrantystartdate);
                        //cmdreg.Parameters.AddWithValue("@manufacturewarrantyenddate", manufacturewarrantyenddate);
                        cmdreg.Parameters.AddWithValue("@sdpstartdate", sdpstartdate);
                        cmdreg.Parameters.AddWithValue("@sdpenddate", sdpenddate);
                        cmdreg.Parameters.AddWithValue("@ProxyName", ProxyName);
                        cmdreg.Parameters.AddWithValue("@ProxyMobile1", ProxyMobile1);
                        cmdreg.Parameters.AddWithValue("@ProxyRelationship", ProxyRelationship);
                        cmdreg.Parameters.AddWithValue("@ProxyEmail", ProxyEmail);
                        cmdreg.Parameters.AddWithValue("@ProxyMobilewhatsappno", ProxyMobilewhatsappno);
                        cmdreg.Parameters.AddWithValue("@PlanSKU", PlanSKU);

                        SqlParameter regnoParam = new SqlParameter("@Regno", SqlDbType.NVarChar, 100);
                        regnoParam.Direction = ParameterDirection.Output;
                        cmdreg.Parameters.Add(regnoParam);

                        SqlParameter skunoParam = new SqlParameter("@SKUNOS", SqlDbType.NVarChar, 50);
                        skunoParam.Direction = ParameterDirection.Output;
                        cmdreg.Parameters.Add(skunoParam);

                        int checkreg = cmdreg.ExecuteNonQuery();

                        string regno = cmdreg.Parameters["@Regno"].Value.ToString();
                        string skunos = cmdreg.Parameters["@SKUNOS"].Value.ToString();
                        string tic = GenerateTicketno();

                        SqlCommand cmdtic = new SqlCommand("IAPL_CRM_stockgenerate_Infysales", con);
                        cmdtic.CommandType = CommandType.StoredProcedure;

                        cmdtic.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 1002;
                        cmdtic.Parameters.AddWithValue("@SkuandSerial", SqlDbType.VarChar).Value = skunos;
                        cmdtic.Parameters.AddWithValue("@RegistrationNo", SqlDbType.NVarChar).Value = regno;
                        cmdtic.Parameters.AddWithValue("@TicketNumber", SqlDbType.DateTime).Value = tic;
                        cmdtic.Parameters.AddWithValue("@UserName", string.IsNullOrEmpty(row["CustomerName"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["CustomerName"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdtic.Parameters.AddWithValue("@CallSource", SqlDbType.NVarChar).Value = 9;
                        cmdtic.Parameters.AddWithValue("@ProblemReported", SqlDbType.NVarChar).Value = "NEW Sales Enquiry";
                        cmdtic.Parameters.AddWithValue("@InfinityRemarks", SqlDbType.NVarChar).Value = "NEW Sales Enquiry";
                        cmdtic.Parameters.AddWithValue("@CallTypes", SqlDbType.NVarChar).Value = 18;
                        cmdtic.Parameters.AddWithValue("@CallType", SqlDbType.NVarChar).Value = planid;
                        cmdtic.Parameters.AddWithValue("@ServiceType", SqlDbType.NVarChar).Value = 10;
                        cmdtic.Parameters.AddWithValue("@ProblemNo", SqlDbType.NVarChar).Value = 1;
                        cmdtic.Parameters.AddWithValue("@CallStatus", SqlDbType.NVarChar).Value = 19;
                        cmdtic.Parameters.AddWithValue("@CallAction", SqlDbType.NVarChar).Value = 59;
                        cmdtic.Parameters.AddWithValue("@ClaimDate", SqlDbType.NVarChar).Value = Convert.ToString(DateTime.Now.ToString("MM/dd/yyyy"));
                        cmdtic.Parameters.AddWithValue("@ClaimTime", SqlDbType.NVarChar).Value = Convert.ToString(DateTime.Now.ToString("hh:mm tt"));

                        int checktic = cmdtic.ExecuteNonQuery();

                        if (planid == null)
                        {
                            AddServicePlan(skunos, planNameSelection, planid, newPlanId, subcatid, PlanSKU);
                        }
                        //string bankTxnId = string.Empty;
                        //string paytmTxnId = string.Empty;                        
                        //string orderRef = string.Empty;                        
                        
                        PaySlip(tic, row["CustomerName"].ToString().Trim().Replace("&nbsp;", ""), IPRN, row["TotalAmountPay"].ToString().Trim().Replace("&nbsp;", ""));
                        UpdateProductInfo(regno, skunos, row["prodMid"].ToString().Trim().Replace("&nbsp;", ""),
                            row["custMid"].ToString().Trim().Replace("&nbsp;", ""), row["payMid"].ToString().Trim().Replace("&nbsp;", ""), IPRN, planid, newPlanId, subcatid, PlanSKU);
                        UpdateCommision(row["payMid"].ToString().Trim().Replace("&nbsp;", ""), "Under Aproval", planid);
                        ticketNo.InnerText += (ticketNo.InnerText == "" ? "" : ", ") + tic.ToString();
                    }
                    con.Close();

                }
            }
        }

        protected string GenerateTicketno()
        {
            SqlCommand cmd = new SqlCommand("IAPL_CRM_stockgenerate_Infysales", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 5;

            SqlParameter ticParam = new SqlParameter("@ticketno", SqlDbType.NVarChar, 50);
            ticParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ticParam);

            int check = cmd.ExecuteNonQuery();
            //con.Close();
            ViewState["tickeno"] = cmd.Parameters["@ticketno"].Value.ToString();
            string checkticket = ViewState["tickeno"].ToString();
            return ViewState["tickeno"].ToString();


        }

        protected void UpdateProductInfo(string regno, string skunos, string prodMid, string custMid, string payMid, string IPRN, string PlanID, string newPlanId, string catid, string PlanSKU)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@type", 9);
                    cmd.Parameters.AddWithValue("@ProdMid", prodMid);
                    cmd.Parameters.AddWithValue("@CustMid", custMid);
                    cmd.Parameters.AddWithValue("@PayMid", payMid);
                    cmd.Parameters.AddWithValue("@PlanID", PlanID);
                    cmd.Parameters.AddWithValue("@NewPlanId", newPlanId);
                    cmd.Parameters.AddWithValue("@CategoryId", catid);
                    cmd.Parameters.AddWithValue("@SKU", skunos);
                    cmd.Parameters.AddWithValue("@RegistrationNo", regno);
                    cmd.Parameters.AddWithValue("@SalesOrderID", IPRN);
                    cmd.Parameters.AddWithValue("@PlanSKU", PlanSKU);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        protected int getSubcatid(string catid)
        {
            try
            {
                int id = 1;
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd.Parameters.AddWithValue("@type", 10);
                    cmd.Parameters.AddWithValue("@subcatgId", catid);
                    object result = cmd.ExecuteScalar();


                    if (result != null && result != DBNull.Value)
                    {
                        id = Convert.ToInt32(result);
                    }
                }
                return id;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        protected DataTable getServicePlanCreate(string planid, string dateofpurchase)
        {
            DataTable dt = new DataTable();

            SqlCommand cmd = new SqlCommand("usp_GetWarrantyDateRanges", con);
            cmd.CommandType = CommandType.StoredProcedure;
           // cmd.Parameters.AddWithValue("@Type", 25);
            cmd.Parameters.AddWithValue("@PurchaseDate", dateofpurchase);
            cmd.Parameters.AddWithValue("@ServicePlan", planid);
            if (con.State != ConnectionState.Open)
                con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }
        protected string GenerateIPPNno()
        {
            string salesorderid = string.Empty;
            SqlCommand cmd = new SqlCommand("IAPL_CRM_stockgenerate_Infysales", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 8;
            if (con.State != ConnectionState.Open)
                con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    salesorderid = reader["salesorderid"].ToString();
                }
            }
            return salesorderid;
        }

        protected DataTable ProxyInfo()
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", 3);
            cmd.Parameters.AddWithValue("@Mid", Session["RetailerUniqueID"].ToString());
            cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());
            if (con.State != ConnectionState.Open)
                con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        protected void PaymentFailed()
        {
            if (Session["salesOrderID"] != null)
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@type", 45);
                    cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"].ToString());
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        protected void UpdateCommision(string payMid, string status, string planId)
        {
            try
            {
                if (Session["salesOrderID"] != null)
                {
                    string CommissionType = string.Empty;
                    string CommissionValue = string.Empty;
                    string CommissionPercentage = string.Empty;
                    string GSTCommissionValue = string.Empty;
                    string GSTCommissionPercentage = string.Empty;
                    string prodMid = string.Empty;
                    string custMid = string.Empty;
                    string PlanID = string.Empty;

                    SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 47;
                    cmd.Parameters.AddWithValue("@Mid", SqlDbType.VarChar).Value = !string.IsNullOrWhiteSpace(payMid) ? payMid : null;
                    cmd.Parameters.AddWithValue("@SalesOrderID", SqlDbType.VarChar).Value = Session["salesOrderID"].ToString();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CommissionType = reader["CommissionType"].ToString();
                            CommissionValue = reader["CommissionValue"].ToString();
                            CommissionPercentage = reader["CommissionPercentage"].ToString();
                            GSTCommissionValue = reader["GSTCommissionValue"].ToString();
                            GSTCommissionPercentage = reader["GSTCommissionPercentage"].ToString();
                            prodMid = reader["prodMid"].ToString();
                            custMid = reader["custMid"].ToString();
                            PlanID = reader["PlanID"].ToString();
                        }
                    }
                    con.Close();

                    using (SqlCommand cmd1 = new SqlCommand("sp_iapl_PartnerRetailer", con))
                    {
                        cmd1.CommandType = CommandType.StoredProcedure;
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd1.Parameters.AddWithValue("@type", 46);
                        if (Session["Role"].ToString() == "Agent")
                        {
                            cmd1.Parameters.AddWithValue("@AgentProfileId", Session["RetailerUniqueID"].ToString());
                        }
                        else
                            cmd1.Parameters.AddWithValue("@Retailer_FreelanceID", Session["RetailerUniqueID"].ToString());
                        cmd1.Parameters.AddWithValue("@ProdMid", prodMid);
                        cmd1.Parameters.AddWithValue("@PlanID", !string.IsNullOrWhiteSpace(planId) ? planId : PlanID);
                        cmd1.Parameters.AddWithValue("@CustMid", custMid);
                        cmd1.Parameters.AddWithValue("@CommissionType", CommissionType);
                        cmd1.Parameters.AddWithValue("@CommissionValue", CommissionValue);
                        cmd1.Parameters.AddWithValue("@CommissionPercentage", CommissionPercentage);
                        cmd1.Parameters.AddWithValue("@GSTCommisionType", "Percantage");
                        cmd1.Parameters.AddWithValue("@GSTCommissionValue", GSTCommissionValue);
                        cmd1.Parameters.AddWithValue("@GSTCommissionPercentage", GSTCommissionPercentage);
                        cmd1.Parameters.AddWithValue("@CommisionTransactionType", status);
                        cmd1.Parameters.AddWithValue("@GSTCommisionTransactionType", status);
                        cmd1.Parameters.AddWithValue("@Remarks", "New Registration");
                        cmd1.Parameters.AddWithValue("@TranStatus", status);
                        cmd1.Parameters.AddWithValue("@CreatedBy", Session["RetailerUniqueID"].ToString());
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        protected void AddServicePlan(string sku, string planName, string planId, string newPlanId, string CategoryId, string PlanSKU)
        {
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Parameters.AddWithValue("@type", 48);
                cmd.Parameters.AddWithValue("@SKU", sku);
                cmd.Parameters.AddWithValue("@PlanName", planName);
                cmd.Parameters.AddWithValue("@PlanId", planId);
                cmd.Parameters.AddWithValue("@NewPlanId", newPlanId);
                cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
                cmd.Parameters.AddWithValue("@PlanSKU", PlanSKU);

                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        protected void SendMail(string email, string sendTo)
        {
            try
            {
                DataTable productdt = new DataTable();

                string emailBody = string.Empty;
                if (sendTo == "Retailer")
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    string s = Server.MapPath("infySign.png");
                    string templatePath = Server.MapPath("~/EmailTemplateFormat/RetailerPurchase.html");
                    emailBody = System.IO.File.ReadAllText(templatePath);

                    // Replace Customer Info
                    StringBuilder customerTable = new StringBuilder();
                    customerTable.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse; width: 100%;'>");
                    customerTable.Append("<tr><th>S.No</th><th>Customer Name</th><th>Mobile No</th><th>Whatsapp No</th><th>Email</th><th>Pincode</th><th>City</th><th>State</th><th>Address</th></tr>");

                    foreach (GridViewRow row in GvCustomerDetails.Rows)
                    {
                        customerTable.Append("<tr>");
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[0].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[1].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[2].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[3].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[4].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[5].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[6].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[7].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[8].Text);
                        customerTable.Append("</tr>");
                    }
                    customerTable.Append("</table>");

                    // Product Plan Info
                    StringBuilder productTable = new StringBuilder();
                    productTable.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse; width: 100%;'>");
                    productTable.Append("<tr><th>S.No</th><th>Product Name</th><th>Category</th><th>Brand</th><th>Model</th><th>Serial No</th><th>Device Price</th><th>Date of Implementation</th><th>Manufacturer Warranty</th></tr>");

                    foreach (GridViewRow row in GVProductDetails.Rows)
                    {
                        productTable.Append("<tr>");
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            productTable.AppendFormat("<td>{0}</td>", row.Cells[i].Text);
                        }
                        productTable.Append("</tr>");
                    }
                    productTable.Append("</table>");


                    // Replace Plan Info
                    StringBuilder planTable = new StringBuilder();
                    decimal totalOfferPrice = 0;

                    planTable.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse; width: 100%;'>");
                    planTable.Append("<tr><th>S.No</th><th>Plan Name</th><th>Plan Price</th><th>SKU</th><th>Offer Price</th><th>Tax Amout</th><th>Total Amount Pay</th><th>Transaction Status</th><th>Payment Date</th></tr>");

                    foreach (GridViewRow row in GVPlanDetails.Rows)
                    {
                        planTable.Append("<tr>");
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            string cellText = row.Cells[i].Text;

                            if (i == 5)
                            {
                                decimal price;
                                if (decimal.TryParse(cellText, out price))
                                {
                                    totalOfferPrice += price;
                                }
                            }

                            planTable.AppendFormat("<td>{0}</td>", cellText);
                        }
                        planTable.Append("</tr>");
                    }
                    planTable.Append("</table>");

                    emailBody = emailBody.Replace("{TotalAmount}", totalOfferPrice.ToString("0.00"));
                    emailBody = emailBody.Replace("{CustomerTable}", customerTable.ToString());
                    emailBody = emailBody.Replace("{ProductTable}", productTable.ToString());
                    emailBody = emailBody.Replace("{PlanTable}", planTable.ToString());

                }
                else if (sendTo == "Customer")
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    string s = Server.MapPath("infySign.png");
                    string templatePath = Server.MapPath("~/EmailTemplateFormat/RetailerPurchase.html");
                    emailBody = System.IO.File.ReadAllText(templatePath);

                    // Replace Customer Info
                    StringBuilder customerTable = new StringBuilder();
                    customerTable.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse; width: 100%;'>");
                    customerTable.Append("<tr><th>S.No</th><th>Customer Name</th><th>Mobile No</th><th>Whatsapp No</th><th>Email</th><th>Pincode</th><th>City</th><th>State</th><th>Address</th></tr>");

                    foreach (GridViewRow row in GvCustomerDetails.Rows)
                    {
                        customerTable.Append("<tr>");
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[0].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[1].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[2].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[3].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[4].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[5].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[6].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[7].Text);
                        customerTable.AppendFormat("<td>{0}</td>", row.Cells[8].Text);
                        customerTable.Append("</tr>");
                    }
                    customerTable.Append("</table>");

                    // Product Plan Info
                    StringBuilder productTable = new StringBuilder();
                    productTable.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse; width: 100%;'>");
                    productTable.Append("<tr><th>S.No</th><th>Product Name</th><th>Category</th><th>Brand</th><th>Model</th><th>Serial No</th><th>Device Price</th><th>Date of Implementation</th><th>Manufacturer Warranty</th></tr>");

                    foreach (GridViewRow row in GVProductDetails.Rows)
                    {
                        productTable.Append("<tr>");
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            productTable.AppendFormat("<td>{0}</td>", row.Cells[i].Text);
                        }
                        productTable.Append("</tr>");
                    }
                    productTable.Append("</table>");


                    // Replace Plan Info
                    StringBuilder planTable = new StringBuilder();
                    decimal totalOfferPrice = 0;

                    planTable.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse; width: 100%;'>");
                    planTable.Append("<tr><th>S.No</th><th>Plan Name</th><th>Plan Price</th><th>SKU</th><th>Offer Price</th><th>Tax Amout</th><th>Total Amount Pay</th><th>Transaction Status</th><th>Payment Date</th></tr>");

                    foreach (GridViewRow row in GVPlanDetails.Rows)
                    {
                        planTable.Append("<tr>");
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            string cellText = row.Cells[i].Text;

                            if (i == 5)
                            {
                                decimal price;
                                if (decimal.TryParse(cellText, out price))
                                {
                                    totalOfferPrice += price;
                                }
                            }

                            planTable.AppendFormat("<td>{0}</td>", cellText);
                        }
                        planTable.Append("</tr>");
                    }
                    planTable.Append("</table>");

                    emailBody = emailBody.Replace("{TotalAmount}", totalOfferPrice.ToString("0.00"));
                    emailBody = emailBody.Replace("{CustomerTable}", customerTable.ToString());
                    emailBody = emailBody.Replace("{ProductTable}", productTable.ToString());
                    emailBody = emailBody.Replace("{PlanTable}", planTable.ToString());
                }
                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress("no-reply@infinityassurance.com");
                Msg.To.Add(email);
                Msg.Subject = "Acknowledgement of InfyShield Protection Plan – Under Review";
                Msg.IsBodyHtml = true;
                Msg.Body = emailBody;


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
                return;
            }
        }

        protected void SendWhatsApp(string mobile, string name, string pincode, string city, string state, string area, string dealerType, string company)
        {
            SendWhatsappMessage(mobile, name, pincode, city, state, area, dealerType, company);
        }


        private void SendWhatsappMessage(string mobile, string name, string pincode, string city, string state, string area, string dealerType, string company)
        {
            try
            {
                string url = "https://backend.api-wa.co/campaign/smartping/api/v2";
                string fullMobile = "91" + mobile; // Assuming Indian format
                string Cmobile = "9911332320";
                string CEmailID = "support@infyshield.com";
                string jsonData = @"{
            ""apiKey"": ""eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjY3MWY1YWUwMTg3OWFjMGJlY2EyZmQ3ZSIsIm5hbWUiOiJJbmZ5U2hpZWxkIiwiYXBwTmFtZSI6IkFpU2Vuc3kiLCJjbGllbnRJZCI6IjY1OTNmZGI3MDBmODRmMzczMjNiODE5OCIsImlhdCI6MTczMDEwODEyOH0.LS_Trirwhav9NV-Vdp0F4MdkUU1C2f56h7ngUzdWqGU"",
            ""campaignName"": ""partner_acknowledgement"",
            ""destination"": """ + fullMobile + @""",
            ""userName"": """ + name + @""",
            ""templateParams"": [
                """ + name + @""",
                """ + mobile + @""",
                """ + area + @""",
                """ + pincode + @""",
                """ + city + @""",
                """ + state + @""",
                """ + Cmobile + @""",
                """ + CEmailID + @"""
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
        protected void DeleteIncompeteInfo(string status, string customerstatus)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 60);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@customer_status", customerstatus);
                cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"] != null ? Session["salesOrderID"].ToString() : null);
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


        protected void PaySlip(string ticketNo, string customerName, string iprnNo, string amount)
        {
            try
            {
                string filePath = string.Empty;
                string bankTxnId = string.Empty;
                string paytmTxnId = string.Empty;
                DateTime txnDate = DateTime.Now;
                string orderRef = Session["salesOrderID"].ToString();

                string basePath = ConfigurationManager.AppSettings["FilePath3"];
                String yy = DateTime.Now.Year.ToString();
                String mn = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);

                bool existsClient = System.IO.Directory.Exists(basePath + "\\InfyShield\\");
                if (!existsClient)
                    System.IO.Directory.CreateDirectory(basePath + "\\InfyShield\\");

                bool existsYear = System.IO.Directory.Exists(basePath + "\\InfyShield\\" + yy);
                if (!existsYear)
                    System.IO.Directory.CreateDirectory(basePath + "\\InfyShield\\" + yy);
                bool existsMonth = System.IO.Directory.Exists(basePath + "\\InfyShield\\" + yy + "/" + mn);
                if (!existsMonth)
                    System.IO.Directory.CreateDirectory(basePath + "\\InfyShield\\" + yy + "/" + mn);

                string originalFileName = "OnlinePaymentAdvice" + DateTime.Now.ToString("MMddyymmss");
                string sanitizedFileName = SanitizeFileName(originalFileName);
                sanitizedFileName = sanitizedFileName.Replace(" ", "_") + ".pdf";


                string fn = ticketNo.ToString().Replace("/", "") + '_' + "InfyShield" + '_' + sanitizedFileName.Replace(" ", "_");
                //fupupload2.SaveAs(basePath + "\\InfyShield\\" + yy + "/" + mn + "/" + fn);
                filePath = basePath + "\\InfyShield\\" + yy + "/" + mn + "/" + fn;

                GenerateReceipt(filePath, amount, bankTxnId, paytmTxnId, txnDate, orderRef, ticketNo, customerName, iprnNo);
                SaveDocument(filePath, ticketNo, "17");
            }
            catch(Exception)
            {
                return;
            }
        }
        private string SanitizeFileName(string fileName)
        {
            string pattern = "[^a-zA-Z0-9-_\\. ]";
            string sanitizedFileName = Regex.Replace(fileName, pattern, "");

            return sanitizedFileName;
        }

        public void GenerateReceipt(string filePath, string amount, string bankTxnId, string paytmTxnId, DateTime txnDate, string orderRef, string ticketNo, string customerName, string iprnNo)
        {
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
        protected void SaveDocument(string filePath, string ticketNo, string documentName)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@type", 73);
                    cmd.Parameters.AddWithValue("@Mid", "0");
                    cmd.Parameters.AddWithValue("@ticketno", ticketNo);
                    cmd.Parameters.AddWithValue("@documentNumber", documentName);
                    cmd.Parameters.AddWithValue("@DocumentPath", filePath);
                    cmd.Parameters.AddWithValue("@CreatedBy", Session["Name"].ToString());

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}