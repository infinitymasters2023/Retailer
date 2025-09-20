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
    public partial class Incentive_Dashboard : System.Web.UI.Page
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
                if (Session["Role"] != null && Session["Role"].ToString() == "Admin")
                {
                    divSalesPersonPanel.Visible = true;
                    mainpanal.Visible = true;
                }
                else if (Session["Role"] != null && Session["Role"].ToString() == "Agent")
                {
                    divSalesPersonPanel.Visible = false;
                    mainpanal.Visible = true;
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
                        //Commision Slabs
                        lblBaseMarginSelles.Text = reader["TotalSoldPlan"].ToString();
                        lblBaseMarginAddMore.Text = "0";
                        lblBaseMarginCommision.Text = "0%";

                        lblNCBValue.Text = "0%";
                        lblNCBNo.Text = "0";
                        lblNCBRs.Text = "0";

                        lblEWMarginNo.Text = "0";
                        lblEWMarginRs.Text = "0";

                        lblIncentiveSellesRs.Text = "0";
                        IncentiveRs.Text = "0";

                        lblAdditionalMarginTotalSalesRs.Text = "0";
                        lblAdditionalMarginTotalSalesNo.Text = "0";
                        lblAdditionalMarginAddMoreRs.Text = "0";
                        lblAdditionalMarginAddMoreNo.Text = "0";
                        lblAdditionalMarginCommision.Text = "0%";

                        lblSalesExecutiveMarginTotalSales.Text = "0";
                        lblSalesExecutiveMarginTotalNo.Text = "0";
                        lblSalesExecutiveMarginAdditionalCommision.Text = "0%";
                    }
                }
            }
        }

        protected void BaseMargin_Click(object sender, EventArgs e)
        {
            Response.Redirect("BaseMargin.aspx");
        }
        protected void NoClaimBonus_Click(object sender, EventArgs e)
        {
            Response.Redirect("NoClaimBonus.aspx");
        }
        protected void EWMargin_Click(object sender, EventArgs e)
        {
            Response.Redirect("EWMargin.aspx");
        }
        protected void Incentive_Click(object sender, EventArgs e)
        {
            Response.Redirect("Incentive.aspx");
        }
        protected void SalesExecutiveEarning_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesExecutiveEarning.aspx");
        }
        protected void AdditionalMargin_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdditionalMargin.aspx");
        }
    }
}