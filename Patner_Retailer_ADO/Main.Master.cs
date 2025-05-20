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
    public partial class Main : System.Web.UI.MasterPage
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iaplConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MobileNo"] == null)
            {
                Response.Redirect("Login.aspx");
                return;

            }

            if (Session["RetailerUniqueID"] != null)
            {
                BindNotificationList();
                if (Session["Role"] != null)
                {
                    if (Session["Role"].ToString() == "Agent")
                    {
                        menuSalesPerson.Visible = false;
                        menuSalesReport.Visible = false;
                    }
                }

            }
            else
            {
                Response.Redirect("Login.aspx");

            }

            string pageName = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();

            if (pageName == "dashboard.aspx")
                menuDashboard.Attributes["class"] = "nav-item active";
            else if (pageName == "buyinfysaleplan.aspx")
                menuBuyPlan.Attributes["class"] = "nav-item active";
            else if (pageName == "viewsalesperson.aspx")
                menuSalesPerson.Attributes["class"] = "nav-item active";
            else if (pageName == "reportedclaims.aspx")
                menuClaims.Attributes["class"] = "nav-item active";
            else if (pageName == "reports.aspx")
                menuReports.Attributes["class"] = "nav-item active";
            else if(pageName == "salesreports.aspx")
                menuSalesReport.Attributes["class"] = "nav-item active";
            else if(pageName == "retaileraddurl.aspx")
                menuGenerateURL.Attributes["class"] = "nav-item active";
            //else if(pageName == "ClaimList.aspx")
            //    menuClaimList.Attributes["class"] = "nav-item active";
        }

        private void BindNotificationList()
        {
            if (Session["salesOrderID"] != null)
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", 13);
                    cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"].ToString());
                    cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"].ToString());
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptNotifications.DataSource = dt;
                    rptNotifications.DataBind();
                    con.Close();
                }
            }
        }
        protected string GetCartLinkAttributes()
        {
            if (Session["salesOrderID"] != null)
            {
                
                bool isCartDisabled = false;

                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 17);
                    cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"].ToString());

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    isCartDisabled = reader.HasRows;
                    con.Close();
                }

                if (isCartDisabled)
                {
                    return "href='javascript:void(0);' onclick='return false;' style='pointer-events:none;opacity:0.5;'";
                }
                else
                {
                    return "href='#' data-toggle='dropdown'";
                }
            }
            return "href='javascript:void(0);' onclick='return false;' style='pointer-events:none;opacity:0.5;'";
        }


    }
}