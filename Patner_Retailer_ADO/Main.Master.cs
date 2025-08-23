using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Patner_Retailer_ADO
{
    public partial class Main : System.Web.UI.MasterPage
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["iaplConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
           // imgCompanyLogo.ImageUrl = "../assets/images/logo.png";
            profileImage.ImageUrl = "../assets/images/avatar5.png";
            if (Session["MobileNo"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (Session["RetailerUniqueID"] == null)
            {
                AuthenticateRetailer();
              
            }

            if (Session["RetailerUniqueID"] != null)
            {
                LogoNameShow();
                BindNotificationList();
                ConfigureMenuByRole();
                HandleAccessControl();
                HighlightActiveMenuItem();
            }
        }



        private void LogoNameShow()
        {
          

            try
            {

               
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", 1);
                    cmd.Parameters.AddWithValue("@MobileNo", Session["MobileNo"]);

                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lbllegalNameHeader.Text= dr["SellerName"].ToString();
                            imgCompanyLogo.ImageUrl = dr["CompanyLogoImage"] != DBNull.Value ? "../uploadeddocuments/" + dr["CompanyLogoImage"].ToString() : "";
                            profileImage.ImageUrl = dr["ProfileImage"] != DBNull.Value ? "../uploadeddocuments/" + dr["ProfileImage"].ToString() : "../assets/images/avatar5.png";
                            Session["Status"] = dr["Status"] != DBNull.Value ? dr["Status"].ToString() : "3";
                        }
                    }
                    con.Close();

                }
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ApplicationException("Error authenticating retailer", ex);
            }
        }


        private void AuthenticateRetailer()
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "5")
                return;

            try
            {
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", 1);
                    cmd.Parameters.AddWithValue("@MobileNo", Session["MobileNo"]);

                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            Session["RetailerUniqueID"] = dr["RetailerUniqueID"];
                            Session["Name"] = dr["Name"];
                            Session["Status"] = dr["Status"];
                            Session["Role"] = dr["Role"];
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ApplicationException("Error authenticating retailer", ex);
            }
        }

        private void BindNotificationList()
        {
            if (Session["salesOrderID"] == null) return;

            try
            {
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", 13);
                    cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"]);
                    cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"]);

                    var dt = new DataTable();
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                        lblCartCount.Text = dt.Rows.Count.ToString();
                        rptNotifications.DataSource = dt;
                        rptNotifications.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception
                lblCartCount.Text = "0";
            }
        }

        protected string GetCartLinkAttributes()
        {
            if (Session["salesOrderID"] == null)
                return "href='javascript:void(0);' onclick='return false;' style='pointer-events:none;'";

            try
            {
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 17);
                    cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"]);

                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.HasRows
                            ? "href='#' data-toggle='dropdown'"
                            : "href='javascript:void(0);' onclick='return false;' style='pointer-events:none;'";
                    }
                    con.Close();
                }
            }
            catch
            {
                return "href='javascript:void(0);' onclick='return false;' style='pointer-events:none;'";
            }
        }

        private void ConfigureMenuByRole()
        {
            if (Session["Role"]?.ToString() == "Agent")
            {
                menuSalesPerson.Visible = false;
                menuSalesReport.Visible = false;
                menuGenerateURL.Visible = false;
            }
        }

        private void HandleAccessControl()
        {
            if (Session["Status"]?.ToString() == "5") return;

            var allowedPages = new[]
            {
                "profile.aspx", "addbank.aspx", "adddealer.aspx", "adddocument.aspx", "dashboard.aspx"
            };

            string currentPage = GetCurrentPage();

            if (!allowedPages.Contains(currentPage))
            {
                DisableAllMenuLinks();
                Response.Redirect("Dashboard.aspx");
            }
            else
            {
                DisableAllMenuLinks();
            }
        }

        private string GetCurrentPage()
        {
            return System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();
        }

        private void HighlightActiveMenuItem()
        {
            string page = GetCurrentPage();

            if (page == "dashboard.aspx") SetActive(menuDashboard);
            else if (new[] { "productinformation.aspx", "buyinfysaleplan.aspx", "cartdetails.aspx", "paymentconfirmation.aspx" }.Any(p => page.Contains(p)))
                SetActive(menuBuyPlan);
            else if (page.Contains("createsalesperson") || page == "viewsalesperson.aspx")
                SetActive(menuSalesPerson);
            else if (page.Contains("claimlist") || page.Contains("viewclaims") || page.Contains("registernewclaim") || page == "reportedclaims.aspx")
                SetActive(menuClaims);
            else if (page == "salesreports.aspx" || page == "registrationdetails.aspx")
                SetActive(menuSalesReport);
            else if (page == "retaileraddurl.aspx") SetActive(menuGenerateURL);
            else if (page == "howitworks.aspx") SetActive(menuHowItWorks);
            else if (page == "faq.aspx") SetActive(menuFAQ);
            else if (page == "upgradeplan.aspx") SetActive(menuUpgradePlan);
            else if (page == "myoffer.aspx") SetActive(menuMyOffer);
            else if (page == "referfriend.aspx") SetActive(menuReferFriend);
            else if (page == "reachus.aspx") SetActive(menuReachUs);
            else if (page == "feedback.aspx") SetActive(menuFeedback);
            else if (page == "incompletepurchased.aspx") SetActive(menuIncomplete);
        }

        private void SetActive(HtmlGenericControl menuItem)
        {
            if (menuItem != null)
                menuItem.Attributes["class"] = "nav-item active";
        }

        private void DisableAllMenuLinks()
        {
            HtmlAnchor[] links = {
                linkBuyPlan, linkIncomplete, linkSalesPerson,
                linkClaims, linkSalesReport, linkGenerateURL,
                linkHowItWorks, linkFAQ, linkUpgradePlan,
                linkMyOffer, linkReferFriend, linkReachUs, linkFeedback
            };

            foreach (var link in links)
            {
                DisableLink(link);
            }
        }

        private void DisableLink(HtmlAnchor link)
        {
            if (link == null) return;

            link.Attributes["class"] = $"{link.Attributes["class"]} disabled-link".Trim();
            link.Attributes["tabindex"] = "-1";
            link.Attributes["aria-disabled"] = "true";
            link.HRef = "#";
        }
    }
}
