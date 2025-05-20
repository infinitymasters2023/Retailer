using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Patner_Retailer_ADO
{
    public partial class ClaimList : System.Web.UI.Page
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
                string skuNumber = Request.QueryString["sku"].ToString();
                GetRecord(skuNumber);
            }
        }
        protected void GetRecord(string skuNumber)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", 18);
                    cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"].ToString());
                    cmd.Parameters.AddWithValue("@ClientID", 2);
                    cmd.Parameters.AddWithValue("@SKU", skuNumber);
                    //cmd.Parameters.AddWithValue("@LoanNo", !string.IsNullOrWhiteSpace(txtLoanNo.Text) ? txtLoanNo.Text.ToString() : null);
                    //cmd.Parameters.AddWithValue("@CustomerMobileNo", !string.IsNullOrWhiteSpace(txtMobileNo.Text) ? txtMobileNo.Text : null);
                    //cmd.Parameters.AddWithValue("@imei", !string.IsNullOrWhiteSpace(txtIMEISerialNo.Text) ? txtIMEISerialNo.Text : null);
                    //cmd.Parameters.AddWithValue("@ticketno", !string.IsNullOrWhiteSpace(txtTicketNo.Text) ? txtTicketNo.Text : null);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);                    
                    if (dt.Rows.Count > 0)
                    {
                        ReportTotal.Text = "Total " + dt.Rows.Count.ToString();
                        GvReport.DataSource = dt;
                        GvReport.DataBind();
                        if (GvReport.HeaderRow != null)
                        {
                            GvReport.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                    }
                    else
                    {
                        ReportTotal.Text = "";
                        GvReport.DataSource = null;
                        GvReport.DataBind();
                    }
                    bool allClosed = dt.AsEnumerable()
                      .All(row => row["claimstatus"].ToString().Equals("Closed", StringComparison.OrdinalIgnoreCase));

                    pnlRegisterClaim.Visible = allClosed;
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }

        protected void RegisterClaim(object sender, EventArgs e)
        {
            //Response.Redirect("ViewClaims.aspx?TicketNumber=" +refNo);

            try
            {
                string skuNumber = Request.QueryString["sku"].ToString();
                LinkButton btn = (LinkButton)sender;
                string refNo = btn.CommandArgument;
                string url = "";
                string sr = "";

                SqlCommand cmd = new SqlCommand("Iapl_crm_usp_getProblemStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 5;
                cmd.Parameters.AddWithValue("@skuandserialno", SqlDbType.NVarChar).Value = skuNumber.Replace(" ", "");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Response.Redirect("ViewClaims.aspx?ticket=" + refNo.Trim() + "&sr=" + sr);                  
                }
                else
                {
                    Response.Redirect("ViewClaims.aspx?ticket=" + refNo.Trim() + "&sr=" + sr);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        protected void RegisterNewCliam(object sender, EventArgs e)
        {
            string skuNumber = Request.QueryString["sku"].ToString();
            Response.Redirect("RegisterNewClaim.aspx?sku=" + skuNumber);
        }
    }
}