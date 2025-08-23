using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Patner_Retailer_ADO
{
    public partial class Dashboard : System.Web.UI.Page
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
                try
                {
                    if (Session["HiddenMessages"] != null)
                    {
                        hdrMessage.Visible = false;
                    }
                    var token = Request.QueryString["token"];
                    if (token != null)
                    {
                        TokenRedirection(token.ToString());
                    }

                    ApprovedWithdrawApplication.Visible = false;
                    TerminateApplication.Visible = false;
                    PendingWithdrawApplicationMessage.Visible = false;
                    AccountRejectMessage.Visible = false;
                    AccountMoreDocumentRequired.Visible = false;
                    AccountPendingMessage.Visible = false;
                    AccountApprovedMessage.Visible = false;
                    mainpanal.Attributes["style"] = "pointer-events: none; opacity: 0.6;";

                    if (Session["Status"] != null && Session["Status"].ToString() == "1")
                    {
                        AccountPendingMessage.Visible = true;
                    }
                    else if (Session["Status"] != null && Session["Status"].ToString() == "2")
                    {
                        AccountPendingMessage.Visible = true;
                    }

                   else  if (Session["Status"] != null && Session["Status"].ToString() == "3")
                    {
                        AccountPendingMessage.Visible = true;
                    }
                    else if (Session["Status"] != null && Session["Status"].ToString() == "4")
                    {
                        AccountMoreDocumentRequired.Visible = true;
                    }
                    else if (Session["Status"] != null && Session["Status"].ToString() == "6")
                    {
                        AccountRejectMessage.Visible = true;
                    }
                    else if (Session["Status"] != null && Session["Status"].ToString() == "7")
                    {
                        PendingWithdrawApplicationMessage.Visible = true;
                    }
                    else if (Session["Status"] != null && Session["Status"].ToString() == "8")
                    {
                        TerminateApplication.Visible = true;
                    }
                    else if (Session["Status"] != null && Session["Status"].ToString() == "9")
                    {
                        ApprovedWithdrawApplication.Visible = true;
                    }
                  
                    else if (Session["Status"] != null && Session["Status"].ToString() == "5")
                    {
                        AccountApprovedMessage.Visible = true;
                        mainpanal.Attributes["style"] = "";
                    }
                   
                    using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@type", 43);
                        cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());
                        cmd.Parameters.AddWithValue("@Retailer_MobileNo", Session["MobileNo"].ToString());
                        cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["RetailerUniqueID"].ToString());

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                           
                            lblServicePlansSoldNo.Text = reader["TotalSoldPlan"].ToString();
                            lblServicePlansSoldValue.Text = (!string.IsNullOrWhiteSpace(reader["TotalSoldPlanAmount"].ToString()) ? reader["TotalSoldPlanAmount"].ToString() : "0");
                            lblPlansSoldApproved.Text = reader["PlansSoldApproved"].ToString();
                            lblPlansRejected.Text = reader["PlansRejected"].ToString();
                            lblPlansUnderRegistrationApproval.Text = reader["PlansUnderRegistrationApproval"].ToString();
                            //lblPlansInCart.Text = reader["PlanInCart"].ToString();
                            if (reader.NextResult() && reader.Read())
                            {
                                lblTotalSales.Text = (!string.IsNullOrWhiteSpace(reader["TotalAmount"].ToString()) ? reader["TotalAmount"].ToString() : "0");
                            }
                            if (reader.NextResult() && reader.Read())
                            {
                                lblPendingForPayment.Text = reader["PendingForPaymentFromYou"].ToString();
                                lblUnsoldPlanInCart.Text = reader["TotalUnsoldPlansInCart"].ToString();
                                lblMissedSales.Text = reader["MissedSalesNotInCart"].ToString();
                            }

                            // Optional if you have data for these:
                            //lblPlansAfterOTP.Text = "0";
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisplayMessage(this, ex.Message);
                }
            }
        }
        protected void TotalEarning_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransactionEarningsReport.aspx?Transaction=Earning");
        }
        protected void ServicePlanSold_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportedClaims.aspx");
        }
        protected void ApprovedPlan_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportedClaims.aspx?TranStatus=Approved");
        }
        protected void RejectPlan_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportedClaims.aspx?TranStatus=Reject");
        }
        protected void PlanUnderApproval_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportedClaims.aspx?TranStatus=UnderApproval");
        }
        protected void PendingForPayment_Click(object sender, EventArgs e)
        {
            Response.Redirect("InCompletePurchased.aspx?tranStatus=Pending");
        }
        protected void UnsoldPlanInCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("InCompletePurchased.aspx?tranStatus=InCart");
        }
        protected void PlanAfterOTP_Click(object sender, EventArgs e)
        {
            Response.Redirect("InCompletePurchased.aspx?tranStatus=Pending");
        }
        protected void WalletDueForRedemption_Click(object sender, EventArgs e)
        {
        }
        protected void InProcess_Click(object sender, EventArgs e)
        {
        }
        protected void MissedSales_Click(object sender, EventArgs e)
        {
            Response.Redirect("InCompletePurchased.aspx?tranStatus=NotInCart");
        }
        protected void Refund_Click(object sender, EventArgs e)
        { }
        protected void RefundUnderApprovel_Click(object sender, EventArgs e)
        { }
        protected void TokenRedirection(string token)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.Parameters.AddWithValue("@type", 65);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader.FieldCount == 1 && reader["Message"] != DBNull.Value)
                            {
                                string message = reader["Message"].ToString();
                                DisplayMessage(this, message);
                                return;
                            }
                            else
                            {
                                Session["RetailerUniqueID"] = reader["RetailerUniqueID"].ToString();
                                Session["Name"] = reader["Name"].ToString();
                                Session["MobileNo"] = reader["MobileNo"].ToString();
                                Session["Status"] = reader["Status"].ToString();
                                Session["Role"] = reader["Role"].ToString();
                                Session["Email"] = reader["emailID"].ToString();
                            }
                        }
                    }
                    else
                    {
                        DisplayMessage(this, "No data returned.");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }

        protected void hideMessageClick(object sender, EventArgs e)
        {
            LinkButton clickedBtn = (LinkButton)sender;

            // Get or create the session list
            if (Session["HiddenMessages"] == null)
                Session["HiddenMessages"] = new List<string>();

            List<string> hiddenMessages = (List<string>)Session["HiddenMessages"];

            // Determine which div to hide based on LinkButton ID
            string divToHide = "";

            switch (clickedBtn.ID)
            {
                case "hideMessage":
                    AccountPendingMessage.Visible = false;
                    divToHide = "AccountPendingMessage";
                    break;
                case "LinkButton1":
                    AccountMoreDocumentRequired.Visible = false;
                    divToHide = "AccountMoreDocumentRequired";
                    break;
                case "LinkButton2":
                    AccountRejectMessage.Visible = false;
                    divToHide = "AccountRejectMessage";
                    break;
                case "LinkButton3":
                    PendingWithdrawApplicationMessage.Visible = false;
                    divToHide = "PendingWithdrawApplicationMessage";
                    break;
                case "LinkButton4":
                    TerminateApplication.Visible = false;
                    divToHide = "TerminateApplication";
                    break;
                case "LinkButton5":
                    ApprovedWithdrawApplication.Visible = false;
                    divToHide = "ApprovedWithdrawApplication";
                    break;
                case "LinkButton6":
                    AccountApprovedMessage.Visible = false;
                    divToHide = "AccountApprovedMessage";
                    break;
            }

            // Store in session
            if (!hiddenMessages.Contains(divToHide))
                hiddenMessages.Add(divToHide);

            Session["HiddenMessages"] = hiddenMessages;
        }


    }
}