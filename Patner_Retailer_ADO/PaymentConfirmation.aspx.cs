using paytm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                //BindProductInfo();
                BindPaymentInfo();
            }
        }

        protected void BindPaymentInfo()
        {
            var querymode = Request.QueryString["m"];
            var Mobile = Request.QueryString["Mobile"];
            var RetailerUniqueID = Request.QueryString["RetailerUniqueID"];
            var Role = Request.QueryString["Role"];            
            if (querymode == "PayTm")
            {
                Session["salesOrderID"] = HttpContext.Current.Cache["TransactionId"].ToString();
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
                    //lbltransno.Text = order_id;
                    if (msg == "TXN_SUCCESS")
                    {
                        //GridView1.DataSource = (DataTable)Session["Cart"];
                        //GridView1.DataBind();
                        lblSucess1.Text = "Your payment transaction is successfully completed.";

                        BindProductInfo();
                        CreateNewTickets();
                        //UpdateProductInfo();

                        //bindGrid();
                        //GenrateSkupin_Click(new object(), new EventArgs());
                    }
                    else
                    {
                        //Response.Redirect("failedpayment.aspx");
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

        protected void BindProductInfo()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 8);
                    cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"].ToString());

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ViewState["AllInfo"] = dt;
                    con.Close();

                    if (dt.Rows.Count > 0)
                    {
                        rptPlans.DataSource = dt;
                        rptPlans.DataBind();

                        rptProductInfo.DataSource = dt;
                        rptProductInfo.DataBind();

                        rptPlanInfo.DataSource = dt;
                        rptPlanInfo.DataBind();
                    }
                    else
                    {
                        rptPlans.DataSource = null;
                        rptPlans.DataBind();

                        rptProductInfo.DataSource = null;
                        rptProductInfo.DataBind();

                        rptPlanInfo.DataSource = null;
                        rptPlanInfo.DataBind();
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
                    con.Open();

                    foreach (DataRow row in dt.Rows)
                    {
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
                        string dateofpurchase = row["ProductPurchaseDate"].ToString().Trim().Replace("&nbsp;", "");
                        string planid = string.Empty;
                        var yymmdd = row["ManufacturerWarranty_yymmdd"].ToString().Trim().Replace("&nbsp;", "");
                        string[] parts = yymmdd.Split('/');

                        int yy = (parts.Length > 0 && int.TryParse(parts[0], out int y)) ? y : 0;
                        int mm = (parts.Length > 1 && int.TryParse(parts[1], out int m)) ? m : 0;
                        int dd = (parts.Length > 2 && int.TryParse(parts[2], out int d)) ? d : 0;
                        using (SqlCommand getPlanIdCmd = new SqlCommand("SELECT producttypeid FROM iapl_producttype WHERE description = @desc", con))
                        {
                            getPlanIdCmd.Parameters.AddWithValue("@desc", planName);
                            object result = getPlanIdCmd.ExecuteScalar();
                            planid = result != null ? result.ToString() : null;
                        }
                        if (planid != "" && dateofpurchase != "")
                        {
                            resultDt = getServicePlanCreate(planName, dateofpurchase);
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
                        int subcatid = 1;
                        subcatid = getSubcatid(row["SubCategoryID"].ToString().Trim().Replace("&nbsp;", ""));                        
                        SqlCommand cmdreg = new SqlCommand("IAPL_CRM_stockgenerate_Infysales", con);
                        cmdreg.CommandType = CommandType.StoredProcedure;

                        cmdreg.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 1;
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
                            cmdreg.Parameters.AddWithValue("@invoicedate_productdetails", invoicedate_productdetails.ToString("dd/MM/yyyy"));
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
                        cmdreg.Parameters.AddWithValue("@ClientID", SqlDbType.NVarChar).Value = 2;
                        cmdreg.Parameters.AddWithValue("@ProjectId", SqlDbType.NVarChar).Value = 68;
                        cmdreg.Parameters.AddWithValue("@IPRN", Session["salesOrderID"] == null ? null : Session["salesOrderID"].ToString());
                        cmdreg.Parameters.AddWithValue("@Status", SqlDbType.VarChar).Value = "Under Approval";
                        cmdreg.Parameters.AddWithValue("@adpstartdate", adpstartdate);
                        cmdreg.Parameters.AddWithValue("@adpenddate", adpenddate);
                        cmdreg.Parameters.AddWithValue("@EWSstart", extendedwarrantystartdate);
                        cmdreg.Parameters.AddWithValue("@EWSEND", extendedwarrantyenddate);
                        cmdreg.Parameters.AddWithValue("@manufacturewarrantystartdate", manufacturewarrantystartdate);
                        cmdreg.Parameters.AddWithValue("@manufacturewarrantyenddate", manufacturewarrantyenddate);
                        cmdreg.Parameters.AddWithValue("@sdpstartdate", sdpstartdate);
                        cmdreg.Parameters.AddWithValue("@sdpenddate", sdpenddate);

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

                        cmdtic.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 2;
                        cmdtic.Parameters.AddWithValue("@SkuandSerial", SqlDbType.VarChar).Value = skunos;
                        cmdtic.Parameters.AddWithValue("@RegistrationNo", SqlDbType.NVarChar).Value = regno;
                        cmdtic.Parameters.AddWithValue("@TicketNumber", SqlDbType.DateTime).Value = tic;
                        cmdtic.Parameters.AddWithValue("@UserName", string.IsNullOrEmpty(row["CustomerName"].ToString().Trim().Replace("&nbsp;", "")) ? DBNull.Value : (object)row["CustomerName"].ToString().Trim().Replace("&nbsp;", ""));
                        cmdtic.Parameters.AddWithValue("@CallSource", SqlDbType.NVarChar).Value = 19;
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
                        UpdateProductInfo(regno, skunos, row["Mid"].ToString().Trim().Replace("&nbsp;", ""));
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

        protected void UpdateProductInfo(string regno, string skunos, string Mid)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@type", 9);
                    cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"].ToString());
                    cmd.Parameters.AddWithValue("@Mid", Mid);                    
                    cmd.Parameters.AddWithValue("@SKU", skunos);
                    cmd.Parameters.AddWithValue("@RegistrationNo", regno);
               

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

            SqlCommand cmd = new SqlCommand("CRM_getcustomersdetailsAll", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", 25);
            cmd.Parameters.AddWithValue("@PurchaseDate", dateofpurchase);
            cmd.Parameters.AddWithValue("@ServicePlan", planid);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);            

            return dt;
        }

    }
}