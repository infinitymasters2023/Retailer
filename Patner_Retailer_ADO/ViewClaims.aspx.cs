using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Net.Sockets;

namespace Patner_Retailer_ADO
{
    public partial class ViewClaims : System.Web.UI.Page
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
                string sku = Request.QueryString["qe"];
                if (sku != null)
                {
                    BindProductInfo(sku);
                    getdocuments();
                    BindUoploadedDocuments(sku);
                }
            }
        }
        protected void BindProductInfo(string TicketNO)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 71);
                    cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"].ToString());
                    cmd.Parameters.AddWithValue("@TicketNO", TicketNO);

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ViewState["AllInfo"] = dt;
                    con.Close();

                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];

                        lblticketNo.Text = row["TicketNO"]?.ToString();
                        lblcoi.Text = row["skuandserialno"]?.ToString();
                        lblstatus.Text = row["Status"]?.ToString();
                        lblNameValue.Text = row["customername"]?.ToString();
                        
                        lblMobileNoValue.Text = row["mobileno"]?.ToString();
                        lblAltModileNoValue.Text = row["AlternativeNo"]?.ToString();
                        lblWhatsappNoValue.Text = row["WhatsAppNo"]?.ToString();
                        lblEmailValue.Text = row["emailidaddress"]?.ToString();
                        lblPincodeValue.Text = row["pincode"]?.ToString();
                        lblCityValue.Text = row["city"]?.ToString();
                        lblStateValue.Text = row["state"]?.ToString();
                        lblAddressValue.Text = row["addressline1"]?.ToString();
                        lblAddressValue2.Text = row["addressline2"]?.ToString();
                        lblAddressValue3.Text = row["addressline3"]?.ToString();

                        lblCategoryValue.Text = row["Category"].ToString();
                        lblProductValue.Text = row["ProductType"].ToString();
                        lblBrandValue.Text = row["brand"].ToString();
                        lblModelValue.Text = row["model"].ToString();
                        lblIMEI1Value.Text = row["IMEINo"].ToString();
                        lblIMEI2Value.Text = row["IMEINo2"].ToString();
                        lblserialNo.Text = row["serialno"].ToString();
                        lblDevicePriceValue.Text = row["DevicePurchasePrice"].ToString();
                        lblDeviceDateValue.Text = row["ProductPurchaseDate"].ToString();
                        lblProductStatusValue.Text = row["ProdStatus"].ToString();
                        string status = row["CallStatus"].ToString();

                        lblPlanValue.Text = row["PlanNicknameSelection"].ToString();
                        lblDescriptionValue.Text = row["FinalPlanNameDescription"].ToString();
                        lblSKUValue.Text = row["SKU"].ToString();
                        lblOfferValue.Text = string.Format("Rs. {0:N2}", Convert.ToDecimal(row["PlanPrice"]));
                        lblDiscountValue.Text = row["Discount"].ToString() + "%";
                        lblMRPValue.Text = string.Format("Rs. {0:N2}", Convert.ToDecimal(row["MRP"]));
                        List<string> dates = new List<string>();

                        if (row["adpDate"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["adpDate"].ToString()))
                            dates.Add(row["adpDate"].ToString());

                        if (row["ewsDate"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["ewsDate"].ToString()))
                            dates.Add(row["ewsDate"].ToString());

                        if (row["sdpDate"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["sdpDate"].ToString()))
                            dates.Add(row["sdpDate"].ToString());

                        lblPlanPeriodValue.Text = string.Join(" , ", dates);

                        anchorBackButton.HRef = "ClaimList.aspx?sku=" + lblcoi.Text;
                        if (status == "2")
                        {
                            UploadDocumentPanel.Visible = false;
                        }

                    }
                    else
                    {

                        lblNameValue.Text = "";
                        lblMobileNoValue.Text = "";
                        lblWhatsappNoValue.Text = "";
                        lblEmailValue.Text = "";
                        lblPincodeValue.Text = "";
                        lblCityValue.Text = "";
                        lblStateValue.Text = "";
                        lblAddressValue.Text = "";

                        lblProductValue.Text = "";
                        lblCategoryValue.Text = "";
                        lblBrandValue.Text = "";
                        lblModelValue.Text = "";
                        lblIMEI1Value.Text = "";
                        lblIMEI2Value.Text = "";
                        lblDevicePriceValue.Text = "";
                        lblDeviceDateValue.Text = "";
                        lblProductStatusValue.Text = "";

                        lblPlanValue.Text = "";
                        lblDescriptionValue.Text = "";
                        lblSKUValue.Text = "";
                        lblOfferValue.Text = "";
                        lblDiscountValue.Text = "";
                        lblMRPValue.Text = "";
                        lblPlanPeriodValue.Text = "";
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        protected void getdocuments()
        {
            string sku = Request.QueryString["qe"];
            if (con.State == ConnectionState.Open)
                con.Close();
            SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 72;
            cmd.Parameters.AddWithValue("@TicketNo", sku);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ddldocumentattached2.DataSource = dt;
                ddldocumentattached2.DataTextField = "DocumentName";
                ddldocumentattached2.DataValueField = "mid";
                ddldocumentattached2.DataBind();
                ddldocumentattached2.Items.Insert(0, "Select Document Title");
            }
        }
        protected void UploadImage1(object sender, EventArgs e)
        {
            if (!fupupload2.HasFile)
            {
                lblMessage.Text = "Please select a file to upload.";
                return;
            }
            if (string.IsNullOrEmpty(ddldocumentattached2.SelectedValue))
            {
                lblMessage.Text = "Please select a document name from the dropdown.";
                return;
            }
            try
            {
                string basePath = ConfigurationManager.AppSettings["FilePath3"];
                string ticketNo = Request.QueryString["qe"];
                String yy = DateTime.Now.Year.ToString();
                String mn = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);

                bool existsClient = System.IO.Directory.Exists(basePath+"\\InfyShield\\");
                if (!existsClient)
                    System.IO.Directory.CreateDirectory(basePath+"\\InfyShield\\");

                bool existsYear = System.IO.Directory.Exists(basePath+"\\InfyShield\\" + yy);
                if (!existsYear)
                    System.IO.Directory.CreateDirectory(basePath + "\\InfyShield\\" + yy);
                bool existsMonth = System.IO.Directory.Exists(basePath + "\\InfyShield\\" + yy + "/" + mn);
                if (!existsMonth)
                    System.IO.Directory.CreateDirectory(basePath + "\\InfyShield\\" +  yy + "/" + mn);

                string originalFileName = fupupload2.PostedFile.FileName;
                string sanitizedFileName = SanitizeFileName(originalFileName);
                sanitizedFileName = sanitizedFileName.Replace(" ", "_");


                string fn = ticketNo.ToString().Replace("/", "") + '_' + "InfyShield" + '_' + sanitizedFileName.Replace(" ", "_");
                fupupload2.SaveAs(basePath+"\\InfyShield\\" + yy + "/" + mn + "/" + fn);

                List<DocumentInfo> docs = Session["UploadedDocuments"] as List<DocumentInfo> ?? new List<DocumentInfo>();

                docs.Add(new DocumentInfo
                {
                    Mid= "0",
                    DocumentName = ddldocumentattached2.SelectedItem.Text,
                    FullDocumentPath = "https://doc.infyshield.com/Documents/InfyShield/" +  yy + "/" + mn +"/"+ fn,
                    UploadedDate = DateTime.Now.ToString("dd-MMM-yyyy HH:mm tt"),
                    Status = "Pending"
                });

                Session["UploadedDocuments"] = docs;
                UploadDocuemt("0", ddldocumentattached2.SelectedItem.Value, ("InfyShield/"+yy + "/" + mn + "/" + fn));
                BindDocumentsGrid();
                lblMessage.Text = "Document uploaded and saved successfully!";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Upload failed: " + ex.Message;
            }
        }
        private string SanitizeFileName(string fileName)
        {
            string pattern = "[^a-zA-Z0-9-_\\. ]";
            string sanitizedFileName = Regex.Replace(fileName, pattern, "");

            return sanitizedFileName;
        }

        private void BindDocumentsGrid()
        {
            var docs = Session["UploadedDocuments"] as List<DocumentInfo>;
            if (docs == null)
            {
                docs = new List<DocumentInfo>();
            }

            GVSupportingDoc.DataSource = docs;
            GVSupportingDoc.DataBind();
            GVSupportingDoc.CssClass = "table data-table table-striped nowrap";
            if (GVSupportingDoc.HeaderRow != null)
                GVSupportingDoc.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void BindUoploadedDocuments(string ticketNo)
        {
            try
            {
                List<DocumentInfo> docs = new List<DocumentInfo>();
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@type", 74);
                    cmd.Parameters.AddWithValue("@ticketno", ticketNo);

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            docs.Add(new DocumentInfo
                            {
                                Mid = reader["mid"].ToString(),
                                DocumentName = reader["DocumentName"].ToString(),
                                FullDocumentPath = "https://doc.infyshield.com/Documents/" + reader["DocumentPath"].ToString(),
                                UploadedDate = reader["CreateDate"].ToString(),
                                Status = reader["DocStatus"].ToString(),
                            });
                        }
                    }
                    con.Close();
                }

                Session["UploadedDocuments"] = docs;
                GVSupportingDoc.DataSource = docs;
                GVSupportingDoc.DataBind();
                GVSupportingDoc.CssClass = "table data-table table-striped nowrap";

                if (GVSupportingDoc.HeaderRow != null)
                    GVSupportingDoc.HeaderRow.TableSection = TableRowSection.TableHeader;
                else
                    GVSupportingDoc.CssClass = "table table-striped nowrap";
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }

        protected void UploadDocuemt(string mid, string documentNumber, string documentPath)
        {
            try
            {
                string ticketNo = Request.QueryString["qe"];
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@type", 73);
                    cmd.Parameters.AddWithValue("@Mid", mid);
                    cmd.Parameters.AddWithValue("@ticketno", ticketNo);
                    cmd.Parameters.AddWithValue("@documentNumber", documentNumber);
                    cmd.Parameters.AddWithValue("@DocumentPath", documentPath);
                    cmd.Parameters.AddWithValue("@CreatedBy", Session["Name"].ToString());

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
                return;
            }
        }
        protected void GVSupportingDoc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = DataBinder.Eval(e.Row.DataItem, "Status")?.ToString();
                Label lblStatus = e.Row.FindControl("lblDocStatus") as Label;
                if (string.IsNullOrWhiteSpace(status))
                {
                    status = "Pending";
                }
                if (lblStatus != null)
                {
                    lblStatus.Text = status;
                }
                if (!string.IsNullOrEmpty(status))
                {
                    switch (status.Trim().ToLower())
                    {
                        case "pending":
                            e.Row.ForeColor = System.Drawing.Color.Orange;
                            break;
                        case "approved":
                            e.Row.ForeColor = System.Drawing.Color.Green;
                            break;
                        case "rejected":
                            e.Row.ForeColor = System.Drawing.Color.Red;
                            break;
                    }
                }
            }
        }

        public class DocumentInfo
        {
            public string Mid { get; set; }
            public string DocumentName { get; set; }
            public string FullDocumentPath { get; set; }
            public string UploadedDate { get; set; }
            public string Status { get; set; }
        }



    }
}