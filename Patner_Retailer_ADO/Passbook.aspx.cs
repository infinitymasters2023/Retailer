using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Patner_Retailer_ADO
{
    public partial class Passbook : System.Web.UI.Page
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
                LodBind();
                BindbankDetails();
                BindCalculationHistory();
            }
        }

        protected void BindCalculationHistory()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer ", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 49);
                    cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["RetailerUniqueID"]?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@UserRole", Session["Role"]?.ToString() ?? "");

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        lblRevenue.Text = "Rs. " + (!dr.IsDBNull(dr.GetOrdinal("TotalAmount")) ? Convert.ToDecimal(dr["TotalAmount"]).ToString("N2") : "0.00");
                        lblApprovedAmount.Text = "Rs. " + (!dr.IsDBNull(dr.GetOrdinal("TotalApprovedAmount")) ? Convert.ToDecimal(dr["TotalApprovedAmount"]).ToString("N2") : "0.00");
                        lblWithdrawal.Text = "Rs. " + (!dr.IsDBNull(dr.GetOrdinal("TotalWithdrawalAmount")) ? Convert.ToDecimal(dr["TotalWithdrawalAmount"]).ToString("N2") : "0.00");
                        lblUnderProcess.Text = "Rs. " + (!dr.IsDBNull(dr.GetOrdinal("TotalUnderApprovedAmount")) ? Convert.ToDecimal(dr["TotalUnderApprovedAmount"]).ToString("N2") : "0.00");
                        lblPaymentFailed.Text = "Rs. " + (!dr.IsDBNull(dr.GetOrdinal("TotalFailedAmount")) ? Convert.ToDecimal(dr["TotalFailedAmount"]).ToString("N2") : "0.00");
                        lblPanelty.Text = "Rs. " + (!dr.IsDBNull(dr.GetOrdinal("TotalPaneltyAmount")) ? Convert.ToDecimal(dr["TotalPaneltyAmount"]).ToString("N2") : "0.00");
                        lblCancellationCharges.Text = "Rs. " + (!dr.IsDBNull(dr.GetOrdinal("TotalCancellationCharges")) ? Convert.ToDecimal(dr["TotalCancellationCharges"]).ToString("N2") : "0.00");
                    }
                    else
                    {
                        lblRevenue.Text = "Rs. 0";
                        lblApprovedAmount.Text = "Rs. 0";
                        lblWithdrawal.Text = "Rs. 0";
                        lblUnderProcess.Text = "Rs. 0";
                        lblPaymentFailed.Text = "Rs. 0";
                        lblPanelty.Text = "Rs. 0";
                        lblCancellationCharges.Text = "Rs. 0";
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }

        protected void BindbankDetails()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 40);
                    cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"]?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@UserRole", Session["Role"]?.ToString() ?? "");

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        lblAccountHolderName.Text = dr["AccountHolderName"].ToString();
                        lblAccountNumber.Text = dr["BankAccountNumber"].ToString();
                        lblIFSC.Text = dr["IFSCCode"].ToString();
                        lblBankName.Text = dr["BankName"].ToString();
                        lblBranch.Text = dr["BankBranch"].ToString();
                        lblBankAddress.Text = dr["BankBranchAddress"].ToString();
                    }
                    else
                    {
                        lblAccountHolderName.Text = "-";
                        lblAccountNumber.Text = "-";
                        lblIFSC.Text = "-";
                        lblBankName.Text = "-";
                        lblBranch.Text = "-";
                        lblBankAddress.Text = "-";
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }
        private void LodBind()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", 15);
                    cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["RetailerUniqueID"].ToString());
                    cmd.Parameters.AddWithValue("@UserRole", Session["Role"]?.ToString() ?? "");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ViewState["ReportInfo"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        GvTransactionDetails.CssClass = "table data-table table-striped nowrap";

                        GvTransactionDetails.DataSource = dt;
                        GvTransactionDetails.DataBind();
                        if (GvTransactionDetails.HeaderRow != null)
                        {
                            GvTransactionDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                    }
                    else
                    {
                        GvTransactionDetails.CssClass = "table table-striped nowrap";
                        GvTransactionDetails.DataSource = null;
                        GvTransactionDetails.DataBind();
                        GvTransactionDetails.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }
        protected void TotalEarning_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransactionEarningsReport.aspx?Transaction=Earning");
        }
        protected void ApprovedAmount_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransactionEarningsReport.aspx?Transaction=ApprovedAmount");
        }
        protected void TotalWithdrawal_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransactionEarningsReport.aspx?Transaction=Withdrawal");
        }
        protected void UnderProcess_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransactionEarningsReport.aspx?Transaction=UnderProcess");
        }
        protected void PaymentFailed_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransactionEarningsReport.aspx?Transaction=PaymentFailed");
        }
        protected void Penalty_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransactionEarningsReport.aspx?Transaction=Penalty");
        }
        protected void CancellationCharges_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransactionEarningsReport.aspx?Transaction=CancellationCharges");
        }

    }
}