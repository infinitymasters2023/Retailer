using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Patner_Retailer_ADO
{
    public partial class RequestfromSalesperson : System.Web.UI.Page
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
                btnExportExcel.Visible = false;
                LodBind();
                Bindddl();
                mvViewType.ActiveViewIndex = 1;
                btnCardView.Attributes["class"] = "btn btn-primary card-btn";
                btnListView.Attributes["class"] = "btn btn-outline-primary card-btn";
                
            }
        }
        private void LodBind()
        {
            try
            {
                List<string> selectedPlans = new List<string>();
                List<string> selectedProducts = new List<string>();
                List<string> selectedBrand = new List<string>();

                foreach (ListItem item in ddlPlan.Items)
                {
                    if (item.Selected)
                    {
                        selectedPlans.Add(item.Text);
                    }
                }
                string plans = string.Join(",", selectedPlans);

                foreach (ListItem item in ddlProduct.Items)
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
                    if(Session["Role"] != null && Session["Role"].ToString() == "Agent")
                        cmd.Parameters.AddWithValue("@Type", 86);
                    else
                        cmd.Parameters.AddWithValue("@Type", 84);
                    cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"].ToString());
                    cmd.Parameters.AddWithValue("@ModalName", !string.IsNullOrWhiteSpace(txtModel.Text) ? txtModel.Text : null);
                    cmd.Parameters.AddWithValue("@CustomerName", !string.IsNullOrWhiteSpace(txtCustomerName.Text) ? txtCustomerName.Text : null);
                    cmd.Parameters.AddWithValue("@CustomerMobileNo", !string.IsNullOrWhiteSpace(txtMobileNo.Text) ? txtMobileNo.Text : null);
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
                    cmd.Parameters.AddWithValue("@UserRole", Session["Role"]?.ToString() ?? "");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ViewState["ReportInfo"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        GvReport.CssClass = "table-responsive table table-striped nowrap";

                        GvReport.DataSource = dt;
                        GvReport.DataBind();
                        rptIncompletePurchase.DataSource = dt;
                        rptIncompletePurchase.DataBind();
                        //btnExportExcel.Visible = true;
                        if (GvReport.HeaderRow != null)
                        {
                            GvReport.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                    }
                    else
                    {
                        GvReport.CssClass = "table table-striped nowrap";
                        GvReport.DataSource = null;
                        GvReport.DataBind();
                        rptIncompletePurchase.DataSource = null;
                        rptIncompletePurchase.DataBind();
                        //btnExportExcel.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DownloadExcel();
        }
        private void DownloadExcel()
        {
            try
            {
                DataTable dt = ViewState["ReportInfo"] as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                    return;

                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

                    worksheet.Cells["A1"].LoadFromDataTable(dt, true);
                    using (var range = worksheet.Cells[1, 1, 1, dt.Columns.Count])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    }
                    worksheet.Cells.AutoFitColumns();

                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", $"attachment;  filename=Sales_Report_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }

        protected void SubmitReport(object sender, EventArgs e)
        {
            LodBind();
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

                var productTypes = dt.DefaultView.ToTable(true, "ProductName");
                ddlProduct.Items.Clear();
                foreach (DataRow row in productTypes.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(row["ProductName"].ToString()))
                        ddlProduct.Items.Add(new ListItem(row["ProductName"].ToString()));
                }

                var plans = dt.DefaultView.ToTable(true, "PlanNicknameSelection");
                ddlPlan.Items.Clear();
                foreach (DataRow row in plans.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(row["PlanNicknameSelection"].ToString()))
                        ddlPlan.Items.Add(new ListItem(row["PlanNicknameSelection"].ToString()));
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
            GvReport.CssClass = "table-responsive table table-striped nowrap";
            if (GvReport.HeaderRow != null)
            {
                GvReport.CssClass = "table-responsive table table-striped nowrap";
                GvReport.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            btnListView.Attributes["class"] = "btn btn-primary card-btn";
            btnCardView.Attributes["class"] = "btn btn-outline-primary card-btn";
        }
        protected string FormatSerialWithSpaces(object value)
        {
            string input = value?.ToString() ?? "";
            return string.Join(" ", Enumerable.Range(0, input.Length / 4 + (input.Length % 4 == 0 ? 0 : 1))
                .Select(i => input.Substring(i * 4, Math.Min(4, input.Length - i * 4))));
        }
        protected string GetStatusBackground(string status)
        {
            switch (status?.ToLower())
            {
                case "failed":
                    return "background-color: red; color: white; padding: 2px 10px 2px 10px; border-radius: 10px;";
                case "approved":
                    return "background-color: green; color: white;padding: 2px 10px 2px 10px; border-radius: 10px;";
                case "pending":
                    return "background-color: yellow; color: black;padding: 2px 10px 2px 10px; border-radius: 10px;";
                default:
                    return "";
            }
        }

        protected void rptIncompletePurchase_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "BackToCart")
            {
                string mid = e.CommandArgument.ToString();
                SendBackToCart(Convert.ToInt32(mid));
            }
        }

        private void SendBackToCart(int mid)
        {
            try
            {
                string status = string.Empty;
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 67);
                    cmd.Parameters.AddWithValue("@Mid", mid);

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            status = reader["Status"]?.ToString();
                            if (status == "Request for Retailer payment")
                            {
                                Session["salesOrderID"] = reader["SalesOrderID"]?.ToString();
                                Response.Redirect("CartDetails.aspx?pay=Requestfromsalesperson", false);
                                Context.ApplicationInstance.CompleteRequest();
                                return;
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }
    }
}