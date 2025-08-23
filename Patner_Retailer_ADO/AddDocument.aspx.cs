using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Patner_Retailer_ADO
{
    public partial class AddDocument : System.Web.UI.Page
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
                btnUploadFront.Visible = false;
                BindDocument();
                string qu = Request.QueryString["qu"];
                if (!string.IsNullOrWhiteSpace(qu))
                {
                    string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(qu));
                    bindDocumentDetails(decoded);
                    btnSubmit.Visible = false;
                    btnEdit.Visible = true;
                    hdrtext.InnerText = "Edit Document Details";
                }
            }
        }
        protected void BindDocument()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 10);

                ddlDocumentName.Items.Clear();
                con.Open();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlDocumentName.DataSource = ds.Tables[0];
                    ddlDocumentName.DataTextField = "DocumentName";
                    ddlDocumentName.DataValueField = "mid";
                    ddlDocumentName.DataBind();

                    ddlDocumentName.Items.Insert(0, new ListItem("-- Select Document --", ""));
                }
                con.Close();
            }
            catch (Exception)
            {
            }
        }
        protected void bindDocumentDetails(string docid)
        {

            try
            {
                if (string.IsNullOrEmpty(docid))
                    return;
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 30);
                    cmd.Parameters.AddWithValue("@Mid", docid);
                    cmd.Parameters.AddWithValue("@UserRole", Session["Role"]?.ToString() ?? "");

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        ddlDocumentName.SelectedValue = dr["DocID"] != DBNull.Value ? dr["DocID"].ToString() : "";
                        txtDocumentNumber.Text = dr["documentNumber"] != DBNull.Value ? dr["documentNumber"].ToString() : "";
                    }
                    else
                    {
                        ddlDocumentName.SelectedValue = "";
                        txtDocumentNumber.Text = "";
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }

        protected void btnUploadFront_Click(object sender, EventArgs e)
        {
            if (!fuFrontSide.HasFile)
            {
                lblDocument.Visible = true;
                DataTable dt = (DataTable)ViewState["DocumentData"];
                gvDocuments.DataSource = dt;
                gvDocuments.DataBind();
                if (gvDocuments.HeaderRow != null)
                {
                    gvDocuments.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                return;
            }
            if (fuFrontSide.HasFile)
            {
                lblDocument.Visible = false;
                try
                {

                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
                    string fileExtension = Path.GetExtension(fuFrontSide.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return;
                    }
                    string docName = ddlDocumentName.SelectedItem.Text;
                    string docNumber = txtDocumentNumber.Text.ToUpper().Trim();
                    string fileName = Path.GetFileName(fuFrontSide.FileName);
                    string folderPath = Server.MapPath("~/UploadedDocuments/");
                    string DocId = ddlDocumentName.SelectedValue.ToString();
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName;
                    string fullPath = Path.Combine(folderPath, uniqueFileName);
                    fuFrontSide.SaveAs(fullPath);

                    DataTable dt;
                    if (ViewState["DocumentData"] == null)
                    {
                        dt = new DataTable();
                        dt.Columns.Add("SrNo");
                        dt.Columns.Add("DocId");
                        dt.Columns.Add("DocumentName");
                        dt.Columns.Add("DocumentNumber");
                        dt.Columns.Add("DocumentPath");
                        dt.Columns.Add("Status");
                        dt.Columns.Add("Size");
                    }
                    else
                    {
                        dt = (DataTable)ViewState["DocumentData"];

                        bool alreadyExists = dt.AsEnumerable()
                           .Any(row => row.Field<string>("DocId") == DocId);
                        if (alreadyExists)
                        {
                            lblDocument.Text = "Document already exists.";
                            lblDocument.ForeColor = System.Drawing.Color.Red;
                            lblDocument.Visible = true;

                            gvDocuments.DataSource = dt;
                            gvDocuments.DataBind();
                            if (gvDocuments.HeaderRow != null)
                            {
                                gvDocuments.HeaderRow.TableSection = TableRowSection.TableHeader;
                            }
                            return;
                        }
                    }

                    // Add new row
                    DataRow dr = dt.NewRow();
                    dr["SrNo"] = dt.Rows.Count + 1;
                    dr["DocId"] = DocId;
                    dr["DocumentName"] = docName;
                    dr["DocumentNumber"] = docNumber;
                    dr["DocumentPath"] = uniqueFileName;
                    dr["Status"] = "Uploaded";
                    dr["Size"] = (fuFrontSide.PostedFile.ContentLength / 1024.0).ToString("0.00") + " KB";
                    dt.Rows.Add(dr);

                    ViewState["DocumentData"] = dt;
                    gvDocuments.DataSource = dt;
                    gvDocuments.DataBind();
                    if (gvDocuments.HeaderRow != null)
                    {
                        gvDocuments.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                    btnSubmit.Visible = true;
                }
                catch (Exception ex)
                {
                }
            }
        }
        protected void btnAddDocument_Click(object sender, EventArgs e)
        {
            if (!fuFrontSide.HasFile)
            {
                lblDocument.Visible = true;
                if (gvDocuments.HeaderRow != null)
                {
                    gvDocuments.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                return;
            }
            try
            {
                if(ddlDocumentName.SelectedValue == "13" || ddlDocumentName.SelectedValue == "57" || ddlDocumentName.SelectedValue == "58")
                {
                    string cleanedDocNumber = txtDocumentNumber.Text.Replace("-", "");
                    if (!string.IsNullOrWhiteSpace(txtDocumentNumber.Text) && !System.Text.RegularExpressions.Regex.IsMatch(cleanedDocNumber, @"^\d{4}$"))
                    {
                        lblDocFormatError.Text = "Invalid Aadhaar number.";
                        lblDocFormatError.CssClass = "text-danger";
                        lblDocFormatError.Attributes.Add("style", "display:block");
                        return;
                    }
                }
                if (ddlDocumentName.SelectedValue == "19")
                {
                    if (!string.IsNullOrWhiteSpace(txtDocumentNumber.Text) && !System.Text.RegularExpressions.Regex.IsMatch(txtDocumentNumber.Text, @"^[A-Z]{5}[0-9]{4}[A-Z]$"))
                    {
                        lblDocFormatError.Text = "Invalid PAN Card format.";
                        lblDocFormatError.CssClass = "text-danger";
                        lblDocFormatError.Attributes.Add("style", "display:block");
                        return;
                    }
                }

                if (fuFrontSide.HasFile)
                {
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
                    string fileExtension = Path.GetExtension(fuFrontSide.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return;
                    }
                    string fileName = Path.GetFileName(fuFrontSide.FileName);
                    string folderPath = Server.MapPath("~/UploadedDocuments/");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName;
                    string fullPath = Path.Combine(folderPath, uniqueFileName);
                    fuFrontSide.SaveAs(fullPath);

                    SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (Session["Role"].ToString() == "Admin")
                        cmd.Parameters.AddWithValue("@Type", 9);
                    else if (Session["Role"].ToString() == "Agent")
                        cmd.Parameters.AddWithValue("@Type", 14);
                    cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@DocID", ddlDocumentName.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@DocumentPath", uniqueFileName);
                    cmd.Parameters.AddWithValue("@documentNumber", txtDocumentNumber.Text.ToUpper().Replace("-", "").Trim());
                    //cmd.Parameters.AddWithValue("@Remarks", txtDocumentNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@Status", "Uploaded");
                    cmd.Parameters.AddWithValue("@IPAddress", Request.UserHostAddress);

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                //string script = $@"
                //            <script type='text/javascript'>
                //                alert('Documents have been saved successfully!');
                //                window.location.href = 'Profile.aspx';
                //            </script>";

                //ClientScript.RegisterStartupScript(this.GetType(), "ProfileRedirect", script);
                btnUploadFront_Click(sender, e);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000)
                {
                    DisplayMessage(this, ex.Message);
                    return;
                }
                throw;
            }

        }
        protected void btnEditDocument_Click(object sender, EventArgs e)
        {
            try
            {
                string qu = Request.QueryString["qu"];
                if (!string.IsNullOrWhiteSpace(qu))
                {
                    if (!fuFrontSide.HasFile)
                    {
                        lblDocument.Visible = true;
                        return;
                    }
                    if (fuFrontSide.HasFile)
                    {
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
                        string fileExtension = Path.GetExtension(fuFrontSide.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            return;
                        }
                        string fileName = Path.GetFileName(fuFrontSide.FileName);
                        string folderPath = Server.MapPath("~/UploadedDocuments/");
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        string uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName;
                        string fullPath = Path.Combine(folderPath, uniqueFileName);
                        fuFrontSide.SaveAs(fullPath);
                        string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(qu));
                        SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Type", 31);
                        cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString().Trim());
                        cmd.Parameters.AddWithValue("@DocID", ddlDocumentName.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@DocumentPath", uniqueFileName);
                        cmd.Parameters.AddWithValue("@documentNumber", txtDocumentNumber.Text.ToUpper().Trim());
                        cmd.Parameters.AddWithValue("@Remarks", txtDocumentNumber.Text.ToUpper().Trim());
                        cmd.Parameters.AddWithValue("@UserRole", Session["Role"]?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("@Mid", decoded);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    string script = $@"
                            <script type='text/javascript'>
                                alert('Documents have been saved successfully!');
                                window.location.href = 'Profile.aspx?qu=Document';
                            </script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "ProfileRedirect", script);
                }
                else
                {
                    string script = $@"
                            <script type='text/javascript'>
                                alert('Invalid Link');
                                window.location.href = 'Profile.aspx?qu=Document';
                            </script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "ProfileRedirect", script);
                    return;
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }
    }
}