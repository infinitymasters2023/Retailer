using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using AjaxControlToolkit;

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
                txtfromDate.Text = DateTime.Now.AddDays(-30).ToString("dd-MMM-yyyy");
                txttodate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                CalendarExtender1.EndDate = DateTime.Now;
                CalendarExtender2.EndDate = DateTime.Now;
                txtfromDate.Attributes.Add("Readonly", "readonly");
                txttodate.Attributes.Add("Readonly", "readonly");
                string skuNumber = Request.QueryString["sku"].ToString();
                GetRecord(skuNumber);
                Bindddl();
                mvViewType.ActiveViewIndex = 0;
                btnCardView.Attributes["class"] = "btn btn-outline-primary card-btn";
                btnListView.Attributes["class"] = "btn btn-primary card-btn";
            }
        }
        protected void GetRecord(string skuNumber)
        {
            try
            {
                List<string> selectedPlans = new List<string>();
                List<string> selectedProducts = new List<string>();
                List<string> selectedBrand = new List<string>();

                foreach (ListItem item in ddlplan.Items)
                {
                    if (item.Selected)
                    {
                        selectedPlans.Add(item.Text);
                    }
                }
                string plans = string.Join(",", selectedPlans);

                foreach (ListItem item in ddlproductType.Items)
                {
                    if (item.Selected)
                    {
                        selectedProducts.Add(item.Text);
                    }
                }
                string Products = string.Join(",", selectedProducts);


                foreach (ListItem item in ddlBrand.Items)
                {
                    if (item.Selected)
                    {
                        selectedBrand.Add(item.Text);
                    }
                }
                string Brand = string.Join(",", selectedBrand);
                DateTime fromDate, toDate;
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
                    cmd.Parameters.AddWithValue("@Brand", !string.IsNullOrWhiteSpace(Brand) ? Brand : null);
                    cmd.Parameters.AddWithValue("@PlanName", !string.IsNullOrWhiteSpace(plans) ? plans : null);
                    cmd.Parameters.AddWithValue("@Productname", !string.IsNullOrWhiteSpace(Products) ? Products : null);
                    cmd.Parameters.AddWithValue("@fromDate", DateTime.TryParse(txtfromDate.Text, out fromDate) ? (object)fromDate : DBNull.Value);
                    //cmd.Parameters.AddWithValue("@toDate", DateTime.TryParse(txttodate.Text, out toDate) ? (object)toDate : DBNull.Value);
                    if (DateTime.TryParse(txttodate.Text, out toDate))
                    {
                        toDate = toDate.Date.AddDays(1).AddSeconds(-1);
                        cmd.Parameters.AddWithValue("@toDate", toDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@toDate", DBNull.Value);
                    }
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ViewState["ReportInfo"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        //   bool allClosed = dt.AsEnumerable()
                        //.All(row => row["claimstatus"].ToString().Equals("Closed", StringComparison.OrdinalIgnoreCase) || row["claimstatus"].ToString().Equals("Close", StringComparison.OrdinalIgnoreCase));
                        bool allClosed = dt.AsEnumerable().All(row =>
                            (row["claimstatus"].ToString().Equals("Closed", StringComparison.OrdinalIgnoreCase) ||
                            row["claimstatus"].ToString().Equals("Close", StringComparison.OrdinalIgnoreCase)) &&
                            row["Status"].ToString().Equals("Approved", StringComparison.OrdinalIgnoreCase));


                        ViewState["ShowRegisterClaim"] = allClosed;
                        GvReport.CssClass = "table data-table table-striped nowrap";
                        GvReport.DataSource = dt;
                        GvReport.DataBind();
                        rptIncompletePurchase.DataSource = dt;
                        rptIncompletePurchase.DataBind();
                        if (GvReport.HeaderRow != null)
                        {
                            GvReport.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                    }
                    else
                    {
                        bool allClosed = dt.AsEnumerable()
                     .All(row => row["claimstatus"].ToString().Equals("Closed", StringComparison.OrdinalIgnoreCase) || row["claimstatus"].ToString().Equals("Close", StringComparison.OrdinalIgnoreCase));

                        ViewState["ShowRegisterClaim"] = allClosed;
                        GvReport.CssClass = "table table-striped nowrap";
                        GvReport.DataSource = null;
                        rptIncompletePurchase.DataSource = null;
                        rptIncompletePurchase.DataBind();
                        GvReport.DataBind();
                    }
                    //bool allClosed = dt.AsEnumerable()
                    //  .All(row => row["claimstatus"].ToString().Equals("Closed", StringComparison.OrdinalIgnoreCase) || row["claimstatus"].ToString().Equals("Close", StringComparison.OrdinalIgnoreCase));

                    //ViewState["ShowRegisterClaim"] = allClosed;
                    //pnlRegisterClaim.Visible = allClosed;
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
                string tokenNo = Guid.NewGuid().ToString();
                SqlCommand cmd = new SqlCommand("Iapl_crm_usp_getProblemStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 5;
                cmd.Parameters.AddWithValue("@skuandserialno", SqlDbType.NVarChar).Value = skuNumber.Replace(" ", "");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    
                    Response.Redirect("ViewClaims.aspx?qe=" + refNo + "&TokenNO=" + tokenNo);
                }
                else
                {
                    Response.Redirect("ViewClaims.aspx?qe=" + refNo + "&TokenNO=" + tokenNo);
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

        protected void GvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool showColumn = ViewState["ShowRegisterClaim"] != null && (bool)ViewState["ShowRegisterClaim"];

                int registerClaimColIndex = 2;

                if (!showColumn && registerClaimColIndex < e.Row.Cells.Count)
                {
                    e.Row.Cells[registerClaimColIndex].Visible = false;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                bool showColumn = ViewState["ShowRegisterClaim"] != null && (bool)ViewState["ShowRegisterClaim"];
                int registerClaimColIndex = 2; // same index as above

                if (!showColumn && registerClaimColIndex < e.Row.Cells.Count)
                {
                    e.Row.Cells[registerClaimColIndex].Visible = false;
                }
            }
        }
        protected void rptIncompletePurchase_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkRegisterClaim = e.Item.FindControl("lnkRegisterClaim") as LinkButton;
                if (lnkRegisterClaim != null)
                {
                    bool showRegisterClaim = ViewState["ShowRegisterClaim"] != null && (bool)ViewState["ShowRegisterClaim"];
                    lnkRegisterClaim.Visible = showRegisterClaim;
                }
            }
        }

        protected void SubmitReport(object sender, EventArgs e)
        {
            string skuNumber = Request.QueryString["sku"].ToString();
            GetRecord(skuNumber);
        }

        protected void Bindddl()
        {
            DataTable dt = ViewState["ReportInfo"] as DataTable;

            ddlBrand.Items.Clear();

            if (dt != null && dt.Rows.Count > 0)
            {
                var salesPersons = dt.DefaultView.ToTable(true, "Brand");
                foreach (DataRow row in salesPersons.Rows)
                {
                    string name = row["Brand"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        ddlBrand.Items.Add(new ListItem(name));
                    }
                }

                var productTypes = dt.DefaultView.ToTable(true, "Productname");
                ddlproductType.Items.Clear();
                foreach (DataRow row in productTypes.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(row["Productname"].ToString()))
                        ddlproductType.Items.Add(new ListItem(row["Productname"].ToString()));
                }

                var plans = dt.DefaultView.ToTable(true, "PlanNicknameSelection");
                ddlplan.Items.Clear();
                foreach (DataRow row in plans.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(row["PlanNicknameSelection"].ToString()))
                        ddlplan.Items.Add(new ListItem(row["PlanNicknameSelection"].ToString()));
                }
            }
        }

        protected void btnCardView_Click(object sender, EventArgs e)
        {
            mvViewType.ActiveViewIndex = 1;
            btnCardView.Attributes["class"] = "btn btn-primary card-btn";
            btnListView.Attributes["class"] = "btn btn-outline-primary card-btn";
        }

        protected void btnListView_Click(object sender, EventArgs e)
        {
            mvViewType.ActiveViewIndex = 0;
            GvReport.CssClass = "table table-striped nowrap";
            if (GvReport.HeaderRow != null)
            {
                GvReport.CssClass = "table data-table table-striped nowrap";
                GvReport.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            btnListView.Attributes["class"] = "btn btn-primary card-btn";
            btnCardView.Attributes["class"] = "btn btn-outline-primary card-btn";
        }
        protected string GetStatusBackground(string status)
        {
            switch (status?.ToLower())
            {
                case "reject":
                    return "background-color: red; color: white; margin:0px 0px 10px 0; padding: 2px 10px 2px 10px; border-radius: 10px;";
                case "approved":
                    return "background-color: green; color: white; margin:0px 0px 10px 0; padding: 2px 10px 2px 10px; border-radius: 10px;";
                case "under approval":
                    return "background-color: yellow; color: black; margin:0px 0px 10px 0; padding: 2px 10px 2px 10px; border-radius: 10px;";
                default:
                    return "";
            }
        }
        protected string GetPlanStatusBackground(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return "";

            var lowerStatus = status.ToLower();

            if (lowerStatus.Contains("active"))
                return "color: #228b22;";
            else if (lowerStatus.Contains("not started"))
                return "color: #FFA500;";
            else if (lowerStatus.Contains("expired") || lowerStatus.Contains("invalid"))
                return "color: red;";
            else
                return "";
        }
        protected string RemovePlanLabelPrefix(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            int start = input.IndexOf(":");
            int end = input.IndexOf(")");

            if (start > -1 && end > start)
            {
                return input.Substring(start + 1, end - start - 1).Trim() + input.Substring(end + 1);
            }

            return input;
        }
        protected string FormatSerialWithSpaces(object value)
        {
            string input = value?.ToString() ?? "";
            return string.Join(" ", Enumerable.Range(0, input.Length / 4 + (input.Length % 4 == 0 ? 0 : 1))
                .Select(i => input.Substring(i * 4, Math.Min(4, input.Length - i * 4))));
        }

    }
}