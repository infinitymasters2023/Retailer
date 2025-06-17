using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using OfficeOpenXml;

namespace Patner_Retailer_ADO
{
    public partial class Reports : System.Web.UI.Page
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
                btnExportExcel.Visible = false;
                LodBind();
            }
        }
        private void LodBind()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", 11);
                    cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"].ToString());
                    cmd.Parameters.AddWithValue("@Brand", !string.IsNullOrWhiteSpace(txtBrnad.Text) ? txtBrnad.Text.ToString() : null);
                    cmd.Parameters.AddWithValue("@ModalName", !string.IsNullOrWhiteSpace(txtModel.Text) ? txtModel.Text : null);
                    cmd.Parameters.AddWithValue("@CustomerName", !string.IsNullOrWhiteSpace(txtCustomerName.Text) ? txtCustomerName.Text : null);
                    cmd.Parameters.AddWithValue("@CustomerMobileNo", !string.IsNullOrWhiteSpace(txtMobileNo.Text) ? txtMobileNo.Text : null);
                    cmd.Parameters.AddWithValue("@PlanName", !string.IsNullOrWhiteSpace(txtPlanName.Text) ? txtPlanName.Text : null);
                    cmd.Parameters.AddWithValue("@Productname", !string.IsNullOrWhiteSpace(txtProductname.Text) ? txtProductname.Text : null);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ViewState["ReportInfo"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        GvReport.CssClass = "table-responsive table data-table table-striped table-bordered nowrap";
                        //ReportTotal.Text = "Total " + dt.Rows.Count.ToString();
                        GvReport.DataSource = dt;
                        GvReport.DataBind();                        
                        btnExportExcel.Visible = true;

                        if (GvReport.HeaderRow != null)
                        {
                            GvReport.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                    }
                    else
                    {
                        GvReport.CssClass = "table-responsive table table-striped table-bordered nowrap";
                        //ReportTotal.Text = "";
                        GvReport.DataSource = null;
                        GvReport.DataBind();
                        btnExportExcel.Visible = false;
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
                    Response.AddHeader("content-disposition", $"attachment;  filename=Report_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
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
    }
}