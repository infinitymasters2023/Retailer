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
                    string docName = ddlDocumentName.SelectedItem.Text;
                    string docNumber = txtDocumentNumber.Text.Trim();
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
                return;
            }
            if (fuFrontSide.HasFile)
            {
                string fileName = Path.GetFileName(fuFrontSide.FileName);
                string folderPath = Server.MapPath("~/UploadedDocuments/");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName;
                string fullPath = Path.Combine(folderPath, uniqueFileName);
                fuFrontSide.SaveAs(fullPath);

                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 9);
                cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString().Trim());
                cmd.Parameters.AddWithValue("@DocID", ddlDocumentName.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@DocumentPath", uniqueFileName);
                cmd.Parameters.AddWithValue("@documentNumber", txtDocumentNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@Remarks", txtDocumentNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@Status", "Uploaded");
                cmd.Parameters.AddWithValue("@IPAddress", Request.UserHostAddress);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            string script = $@"
                            <script type='text/javascript'>
                                alert('Documents have been saved successfully!');
                                window.location.href = 'Profile.aspx';
                            </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "ProfileRedirect", script);

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
                        string fileName = Path.GetFileName(fuFrontSide.FileName);
                        string folderPath = Server.MapPath("~/UploadedDocuments/");
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        string uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName;
                        string fullPath = Path.Combine(folderPath, uniqueFileName);
                        fuFrontSide.SaveAs(fullPath);

                        SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Type", 31);
                        cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString().Trim());
                        cmd.Parameters.AddWithValue("@DocID", ddlDocumentName.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@DocumentPath", uniqueFileName);
                        cmd.Parameters.AddWithValue("@documentNumber", txtDocumentNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@Remarks", txtDocumentNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@Status", "Uploaded");
                        cmd.Parameters.AddWithValue("@IPAddress", Request.UserHostAddress);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    string script = $@"
                            <script type='text/javascript'>
                                alert('Documents have been saved successfully!');
                                window.location.href = 'Profile.aspx';
                            </script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "ProfileRedirect", script);
                }
                else
                {
                    string script = $@"
                            <script type='text/javascript'>
                                alert('Invalid Link');
                                window.location.href = 'Profile.aspx';
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