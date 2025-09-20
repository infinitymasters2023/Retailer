using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using static Patner_Retailer_ADO.ViewClaims;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace Patner_Retailer_ADO
{
    public partial class CreateAnAccount : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iaplConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string token = Request.QueryString["qa"];
                if (token != null && !string.IsNullOrWhiteSpace(token))
                {
                    bool flag = CheckToken(token);
                    if (!flag)
                    {
                        lblSellerName.InnerText = "Invalid Token.";
                        lblSellerName.Attributes.Add("style", "display:block");
                        return;
                    }
                }
                if (Session["MobileNo"] == null || Session["SellerGSTIN"] == null)
                {
                    Response.Redirect("~/index.aspx");
                    return;
                }
                txtMobileNumber.Text = Session["MobileNo"] != null ? Session["MobileNo"].ToString() : "";
                txtOfficialMobileNo.Text = Session["MobileNo"] != null ? Session["MobileNo"].ToString() : "";
                txtGSTIN.Text = Session["SellerGSTIN"] != null ? Session["SellerGSTIN"].ToString() : null;

                BindDocument();
                BindBankSupportingDocument();
                hdnActiveTab.Value = "#step2";
                txtBankName.Enabled = false;
                txtBranchName.Enabled = false;
                txtBranchAddress.Enabled = false;
                lblSellerState.Visible = false;
                lblSellerCity.Visible = false;
                txtMobileNumber.Enabled = false;
              
                txtOfficialMobileNo.Enabled = false;
                lnkChangeBussinessMobileNo.Visible = true;
                btnSendOTP.Visible = false;
                TextBox1.Attributes.Add("ReadOnly", "readonly");
               
                txtGSTIN.Enabled = Session["SellerGSTIN"] != null ? false : true;
                //Session.Remove("SellerGSTIN");
                CalendarExtender3.EndDate = DateTime.Now.AddYears(-19);
                if (Session["RetailerUniqueID"] != null)
                {
                    BindInformation();
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
                if (con.State != ConnectionState.Open)
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

        protected void BindBankSupportingDocument()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 55);

                ddlSuppotingDoc.Items.Clear();
                if (con.State != ConnectionState.Open)
                    con.Open();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlSuppotingDoc.DataSource = ds.Tables[0];
                    ddlSuppotingDoc.DataTextField = "DocumentName";
                    ddlSuppotingDoc.DataValueField = "mid";
                    ddlSuppotingDoc.DataBind();

                    ddlSuppotingDoc.Items.Insert(0, new ListItem("-- Select Document --", ""));
                }
                con.Close();
            }
            catch (Exception)
            {
            }
        }

        protected void btnUploadFront_Click(object sender, EventArgs e)
        {
            if (fuFrontSide.HasFile)
            {
                try
                {
                    chckGSTIN.Attributes.Add("style", "display:none");
                    if ((ddlDocumentName.SelectedValue == "13" || ddlDocumentName.SelectedValue == "57" || ddlDocumentName.SelectedValue == "58") && !string.IsNullOrWhiteSpace(txtDocumentNumber.Text))
                    {
                        string cleanedDocNumber = txtDocumentNumber.Text.ToUpper().Replace("-", "");
                        if (!System.Text.RegularExpressions.Regex.IsMatch(cleanedDocNumber, @"^\d{4}$"))
                        {
                            lblDocFormatError.Text = "Invalid Aadhaar number.";
                            lblDocFormatError.CssClass = "text-danger";
                            lblDocFormatError.Attributes.Add("style", "display:block");
                            lblDocFormatError.Visible = true;
                            return;
                        }
                    }
                    if (ddlDocumentName.SelectedValue == "19" && !string.IsNullOrWhiteSpace(txtDocumentNumber.Text))
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(txtDocumentNumber.Text, @"^[A-Z]{5}[0-9]{4}[A-Z]$"))
                        {
                            lblDocFormatError.Text = "Invalid PAN Card format.";
                            lblDocFormatError.CssClass = "text-danger";
                            lblDocFormatError.Attributes.Add("style", "display:block");
                            lblDocFormatError.Visible = true;
                            return;
                        }
                        //if (!System.Text.RegularExpressions.Regex.IsMatch(txtDocumentNumber.Text, @"^[A-Z0-9]{4}$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                        //{
                        //    lblDocFormatError.Text = "Invalid PAN last 4 characters.";
                        //    lblDocFormatError.CssClass = "text-danger";
                        //    lblDocFormatError.Visible = true;
                        //    return;
                        //}
                    }
                    string gstinPattern = @"^\d{2}[A-Z]{5}\d{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$";
                    if(ddlDocumentName.SelectedValue == "146" && string.IsNullOrWhiteSpace(txtDocumentNumber.Text))
                    {
                        lblDocFormatError.Text = "GSTIN is required.";
                        lblDocFormatError.CssClass = "text-danger";
                        lblDocFormatError.Attributes.Add("style", "display:block");
                        lblDocFormatError.Visible = true;
                        chckGSTIN.Attributes.Add("style", "display:block");
                        return;
                    }
                    if (ddlDocumentName.SelectedValue == "146" && !string.IsNullOrWhiteSpace(txtDocumentNumber.Text) && !Regex.IsMatch(txtDocumentNumber.Text.ToUpper(), gstinPattern))
                    {
                        lblDocFormatError.Text = "Invalid GSTIN format.";
                        lblDocFormatError.CssClass = "text-danger";
                        lblDocFormatError.Attributes.Add("style", "display:block");
                        chckGSTIN.Attributes.Add("style", "display:block");
                        lblDocFormatError.Visible = true;
                        return;
                    }
                    if (ddlDocumentName.SelectedValue == "146" && txtDocumentNumber.Text != Session["SellerGSTIN"].ToString())
                    {
                        lblDocFormatError.Text = "GSTIN does not match.";
                        lblDocFormatError.CssClass = "text-danger";
                        lblDocFormatError.Attributes.Add("style", "display:block");
                        chckGSTIN.Attributes.Add("style", "display:block");
                        lblDocFormatError.Visible = true;
                        return;
                    }
                    lblDocFormatError.Attributes.Add("style", "display:none");
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
                    string fileExtension = Path.GetExtension(fuFrontSide.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return;
                    }
                    string docName = ddlDocumentName.SelectedItem.Text;
                    string docNumber = txtDocumentNumber.Text.ToUpper().Replace("-", "").Trim();
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
                    hdnActiveTab.Value = "#step4";
                    if (gvDocuments.HeaderRow != null)
                    {
                        gvDocuments.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                    txtDocumentNumber.Text = null;
                    txtDocumentNumber.Enabled = true;
                    ddlDocumentName.SelectedIndex = 0;
                    lblDocument.Visible = false;
                    CheckIfBothDocumentsUploaded();
                }
                catch (Exception ex)
                {
                    // Log or show error
                }
            }
            else
            {
                if (ddlDocumentName.SelectedValue == "146")
                {
                    chckGSTIN.Attributes.Add("style", "display:block");
                }
                else
                {
                    chckGSTIN.Attributes.Add("style", "display:none");
                }
                lblDocument.Visible=true;
            }
        }

        protected void btnUploadBack_Click(object sender, EventArgs e)
        {

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx", false);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {

            try
            {
                int count = 0;

                if (string.IsNullOrWhiteSpace(txtAddress.Text))
                {
                    lblCurrentAddress.Attributes.Add("style", "display: block;");
                    txtAddress.Focus();
                    count++;
                }
                if (string.IsNullOrWhiteSpace(txtAddress.Text) || txtAddress.Text.Trim().Split(' ').Length < 3)
                {
                    lblCurrentAddress.InnerText = "Please enter at least three words.";
                    lblCurrentAddress.Attributes.Add("style", "display: block;");
                    txtAddress.Focus();
                    count++;
                }
                else {
                    lblCurrentAddress.Attributes.Add("style", "display: none;");
                }
               
                if (string.IsNullOrWhiteSpace(txtPinCode.Text))
                {
                    lblPincode.Attributes.Add("style", "display: block;");
                    txtPinCode.Focus();
                    count++;
                }
                if (txtPinCode.Text.Length != 6)
                {
                    lblPincode.InnerText = "Enter a 6-digit Pincode";
                    lblPincode.Attributes.Add("style", "display: block;");
                    txtPinCode.Focus();
                    count++;
                }
                else {
                    lblPincode.Attributes.Add("style", "display: none;");
                }
                if (string.IsNullOrWhiteSpace(txtLandmark.Text))
                {
                    lblCurrentlandmark.Attributes.Add("style", "display: block;");
                    lblCurrentlandmark.Focus();
                    count++;
                }
                else if (string.IsNullOrWhiteSpace(txtLandmark.Text) || txtLandmark.Text.Trim().Length < 3)
                {
                    lblCurrentlandmark.InnerText = "Please enter at least 3 characters.";
                    lblCurrentlandmark.Attributes.Add("style", "display: block;");
                    txtLandmark.Focus();
                    count++;
                }
                else
                {
                    lblCurrentlandmark.Attributes.Add("style", "display: none;");
                }


                if (string.IsNullOrWhiteSpace(txtSellerAddressLine1.Text))
                {
                    lblSellerAddress.Attributes.Add("style", "display: block;");
                    txtSellerAddressLine1.Focus();
                    count++;
                }
                else if (string.IsNullOrWhiteSpace(txtSellerAddressLine1.Text) || txtSellerAddressLine1.Text.Trim().Split(' ').Length < 3)
                {
                    lblSellerAddress.InnerText = "Please enter at least three words.";
                    lblSellerAddress.Attributes.Add("style", "display: block;");
                  
                    txtSellerAddressLine1.Focus();
                    count++;
                }
                else
                {
                    lblSellerAddress.Attributes.Add("style", "display: none;");
                }

                if (string.IsNullOrWhiteSpace(txtSellerPincode.Text))
                {
                    lblSellerPINCode.Attributes.Add("style", "display: block;");
                    txtSellerPincode.Focus();
                    count++;
                }
                if (txtSellerPincode.Text.Length != 6)
                {
                    lblSellerPINCode.InnerText = "Enter a 6-digit Pincode";
                    lblSellerPINCode.Attributes.Add("style", "display: block;");
                    txtSellerPincode.Focus();
                    count++;
                }
                else
                {
                    lblSellerPINCode.Attributes.Add("style", "display: none;");
                }
                if (string.IsNullOrWhiteSpace(txtSellerLandmark.Text))
                {
                    lblSellerLandMark.Attributes.Add("style", "display: block;");
                    txtSellerLandmark.Focus();
                    count++;
                }
                else if (string.IsNullOrWhiteSpace(txtSellerLandmark.Text) || txtSellerLandmark.Text.Trim().Length < 3)
                {
                    lblSellerLandMark.InnerText = "Please enter at least 3 characters.";
                    lblSellerLandMark.Attributes.Add("style", "display: block;");
                    txtSellerLandmark.Focus();
                    count++;
                }
                else
                {
                    lblSellerLandMark.Attributes.Add("style", "display: none;");
                }
               
                if (count > 0)
                {
                    hdnActiveTab.Value = "#step1";
                    return;
                }
                else
                {
                    bool add = AddEditPersonal();
                    if (add)
                    {
                        bool dealer = AddEditDealer();
                        if (dealer)
                        {
                            string script = "$('.nav-tabs > .active').next('li').find('a').click();";
                            ScriptManager.RegisterStartupScript(this, GetType(), "MoveToNextTab", script, true);
                            hdnActiveTab.Value = "#step3";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                hdnActiveTab.Value = "#step1";
            }
        }

        protected void btnnext2_Click(object sender, EventArgs e)
        {
            int count = 0;

            if(ddlSuppotingDoc.SelectedItem.Value == "")
            {
                lblsupportingDocError.Attributes.Add("style", "display: block;");
                lblsupportingDocError.Text = "Please select the Supporting Document.";
                lblsupportingDocError.Focus();
                count++;
            }
            else
            {
                lblsupportingDocError.Attributes.Add("style", "display: none;");
            }
            if (!fuSuppotingDoc.HasFile)
            {
                lblSupportingDocumentError.Attributes.Add("style", "display: block;");
                lblSupportingDocumentError.Text = "Please upload the Supporting Document.";
                lblSupportingDocumentError.Focus();
                count++;
            }
            else
            {
                lblsupportingDocError.Attributes.Add("style", "display: none;");
            }
            if (!string.IsNullOrWhiteSpace(txtAccountHolderName.Text) && txtAccountHolderName.Text.Length < 3)
            {
                lblAccountHoldername.Attributes.Add("style", "display: block;");
                lblAccountHoldername.InnerText = "Account Holder Name must be between 3 and 30 characters.";
                txtAccountHolderName.Focus();
                count++;
            }
            if (string.IsNullOrWhiteSpace(txtAccountHolderName.Text))
            {
                lblAccountHoldername.Attributes.Add("style", "display: block;");
                txtAccountHolderName.Focus();
                count++;
            }
            else { lblAccountHoldername.Attributes.Add("style", "display: none;"); }

            string pattern = @"^[A-Z]{4}0[A-Z0-9]{6}$";
            if (!Regex.IsMatch(txtIFSCCode.Text, pattern))
            {
                lblIFSCCode.Style["display"] = "block";
                lblIFSCCode.InnerText = "Invalid IFSC code format.";
                txtIFSCCode.Focus();
                count++;
            }
            if (string.IsNullOrWhiteSpace(txtIFSCCode.Text))
            {
                lblIFSCCode.Style["display"] = "block";
                txtIFSCCode.Focus();
                count++;
            }
            else { lblIFSCCode.Style["display"] = "none"; }
            if (txtAccountNumber.Text != txtConfirmAccountNumber.Text)
            {
                lblConfirmAccountNumber.Style["display"] = "block";
                lblConfirmAccountNumber.InnerText = "Account numbers do not match.";
                txtConfirmAccountNumber.Focus();
                count++;
            }
            if (string.IsNullOrWhiteSpace(txtConfirmAccountNumber.Text))
            {
                lblConfirmAccountNumber.Style["display"] = "block";
                txtConfirmAccountNumber.Focus();
                count++;
            }
            else { lblConfirmAccountNumber.Style["display"] = "none"; }

            if (string.IsNullOrWhiteSpace(txtAccountNumber.Text))
            {
                lblAccountNumber.Style["display"] = "block";
                txtAccountNumber.Focus();
                count++;
            }
            else { lblAccountNumber.Style["display"] = "none"; }
            string mid = Session["bankMid"] != null ? Session["bankMid"].ToString() : "";
            bool validatedAccount = AccountNumberValidation(txtAccountNumber.Text, mid);
            if (!string.IsNullOrWhiteSpace(txtAccountNumber.Text) && validatedAccount)
            {
                return;
            }

            if (count > 0)
            {
                hdnActiveTab.Value = "#step3";
                return;
            }
            else
            {
                if (Session["UniqueMid"] != null)
                {
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
                    string fileExtension = Path.GetExtension(fuSuppotingDoc.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        lblsupportingDocError.Text = "Please upload only jpg, jpeg, png and pdf format.";
                        lblsupportingDocError.Attributes.Add("style", "display:block");
                        return;
                    }
                    lblsupportingDocError.Attributes.Add("style", "display:none");
                    string docName = ddlDocumentName.SelectedItem.Text;
                    string docNumber = txtDocumentNumber.Text.ToUpper().Replace("-", "").Trim();
                    string fileName = Path.GetFileName(fuSuppotingDoc.FileName);
                    string folderPath = Server.MapPath("~/UploadedDocuments/");
                    string DocId = ddlDocumentName.SelectedValue.ToString();
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName;
                    string fullPath = Path.Combine(folderPath, uniqueFileName);
                    fuSuppotingDoc.SaveAs(fullPath);


                    SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (Session["bankMid"] != null && Session["bankMid"].ToString() != "")
                    {
                        cmd.Parameters.AddWithValue("@Type", 24);
                        cmd.Parameters.AddWithValue("@Mid", Session["bankMid"].ToString());
                    }
                    else
                        cmd.Parameters.AddWithValue("@Type", 5);
                    cmd.Parameters.AddWithValue("@ProfileId", Session["UniqueMid"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@BankAccountNumber", txtAccountNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@IFSCCode", txtIFSCCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@AccountHolderName", txtAccountHolderName.Text.Trim());
                    cmd.Parameters.AddWithValue("@BankName", txtBankName.Text.Trim());
                    cmd.Parameters.AddWithValue("@BankBranch", txtBranchName.Text.Trim());
                    cmd.Parameters.AddWithValue("@BankBranchAddress", txtBranchAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Status", "Primary");
                    cmd.Parameters.AddWithValue("@UPIID", txtUPIID.Text.ToString());
                    cmd.Parameters.AddWithValue("@TypeofBankAccount", ddlTypeOfBank.SelectedValue == "" ? null : ddlTypeOfBank.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@IsThisYourJointAccount", chkJointAccount.SelectedValue == "" ? null : chkJointAccount.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@JointAccountHolderName", txtJointHolderName.Text.ToString());
                    cmd.Parameters.AddWithValue("@SupportingDocuments", ddlSuppotingDoc.SelectedValue == "" ? null : ddlSuppotingDoc.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@SupportingDocumentsPath", uniqueFileName);

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    object result = cmd.ExecuteScalar();
                    con.Close();
                    if (result != null)
                    {
                        Session["bankMid"] = result.ToString();

                        string script = "$('.nav-tabs > .active').next('li').find('a').click();";
                        ScriptManager.RegisterStartupScript(this, GetType(), "MoveToNextTab", script, true);
                        hdnActiveTab.Value = "#step4";
                    }
                }
            }
        }

        protected void btnnext3_Click(object sender, EventArgs e)
        {
            bool doccheck = CheckIfBothDocumentsUploaded();
            if (!doccheck)
            {
                hdnActiveTab.Value = "#step4";
                return;
            }
            else
            {
                try
                {
                    if (ViewState["DocumentData"] != null)
                    {
                        DataTable dt = (DataTable)ViewState["DocumentData"];

                        foreach (DataRow row in dt.Rows)
                        {
                            string docName = row["DocumentName"].ToString();
                            string docNumber = row["DocumentNumber"].ToString();
                            string status = row["Status"].ToString();
                            string size = row["Size"].ToString();
                            string DocumentPath = row["DocumentPath"].ToString();
                            string DocId = row["DocId"].ToString();

                            SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Type", 9);
                            cmd.Parameters.AddWithValue("@ProfileId", Session["UniqueMid"].ToString().Trim());
                            cmd.Parameters.AddWithValue("@DocID", DocId);
                            cmd.Parameters.AddWithValue("@DocumentPath", DocumentPath);
                            cmd.Parameters.AddWithValue("@documentNumber", docNumber);
                            //cmd.Parameters.AddWithValue("@Remarks", docNumber);
                            cmd.Parameters.AddWithValue("@Status", status);
                            cmd.Parameters.AddWithValue("@IPAddress", Request.UserHostAddress);

                            if (con.State != ConnectionState.Open)
                                con.Open();

                            // Capture inserted Mid
                            object result = cmd.ExecuteScalar();
                            con.Close();

                            int insertedMid = (result != null) ? Convert.ToInt32(result) : 0;

                            // Now pass it to your method
                            if (insertedMid > 0)
                            {
                                UploadDocumentTicket(insertedMid);
                            }

                        }
                        SqlCommand cmd1 = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@Type", 25);
                        cmd1.Parameters.AddWithValue("@ProfileId", Session["UniqueMid"].ToString().Trim());
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd1.ExecuteNonQuery();
                        con.Close();
                        //UploadDocumentTicket();
                    }

                    string script = "$('.nav-tabs > .active').next('li').find('a').click();";
                    ScriptManager.RegisterStartupScript(this, GetType(), "MoveToNextTab", script, true);
                    hdnActiveTab.Value = "#step5";
                    SendMail();
                    SendWhatsApp();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 50000)
                    {
                        string msg = ex.Message;
                        hdnActiveTab.Value = "#step4";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('Error: {msg}');", true);
                    }
                    else
                    {
                        hdnActiveTab.Value = "#step4";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('An error occurred while saving the documents.')", true);
                    }
                }
                catch (Exception ex)
                {
                    hdnActiveTab.Value = "#step4";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('Unexpected error: {ex.Message}')", true);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }



        protected void SendMail()
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                string s = Server.MapPath("infySign.png");
                string templatePath = Server.MapPath("~/EmailTemplateFormat/SignUp.html");
                string emailBody = System.IO.File.ReadAllText(templatePath);

                emailBody = emailBody.Replace("{Name}", txtFirstName.Text.Trim());
                emailBody = emailBody.Replace("{Email}", txtEmail.Text.Trim());
                emailBody = emailBody.Replace("{MobileNo}", txtMobileNumber.Text.Trim());
                emailBody = emailBody.Replace("{Address}", txtAddress.Text.Trim());
                emailBody = emailBody.Replace("{GSTIN}", txtGSTIN.Text.Trim());
                //emailBody = emailBody.Replace("{BussinessEmail}", txtOfficialEmail.Text.Trim());
                //emailBody = emailBody.Replace("{BussinessMobileNo}", txtOfficialMobileNo.Text.Trim());
                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress("no-reply@infinityassurance.com");
                Msg.To.Add(txtEmail.Text.ToString());
                Msg.Subject = "InfyShield – Your Interest in Business Association";
                Msg.IsBodyHtml = true;
                Msg.Body = emailBody;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("no-reply@infinityassurance.com", "mlas jsej cdzd fmdc");
                smtp.EnableSsl = true;
                smtp.Send(Msg);
                Msg = null;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        protected void SendWhatsApp()
        {
           
            // You can customize these values from your UI or database if needed
            string mobile = txtMobileNumber.Text.Trim();
            string name = txtFirstName.Text.Trim();
            string pincode = txtPinCode.Text.Trim();
            string city = txtCity.Text.Trim();
            string state = txtState.Text.Trim();
            string area = txtAddress.Text.Trim();
            string dealerType = "Retailer";
            string company = txtSellerName.Text.ToUpper().Trim();

            SendWhatsappMessage(mobile, name, pincode, city, state, area, dealerType, company);
        }


        private void SendWhatsappMessage(string mobile, string name, string pincode, string city, string state, string area, string dealerType, string company)
        {
            try
            {
                string url = "https://backend.api-wa.co/campaign/smartping/api/v2";
                string fullMobile = "91" + mobile; // Assuming Indian format

                string jsonData = @"{
            ""apiKey"": ""eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjY3MWY1YWUwMTg3OWFjMGJlY2EyZmQ3ZSIsIm5hbWUiOiJJbmZ5U2hpZWxkIiwiYXBwTmFtZSI6IkFpU2Vuc3kiLCJjbGllbnRJZCI6IjY1OTNmZGI3MDBmODRmMzczMjNiODE5OCIsImlhdCI6MTczMDEwODEyOH0.LS_Trirwhav9NV-Vdp0F4MdkUU1C2f56h7ngUzdWqGU"",
            ""campaignName"": ""partner_acknowledgement"",
            ""destination"": """ + fullMobile + @""",
            ""userName"": """ + name + @""",
            ""templateParams"": [
                """ + name + @""",
                """ + mobile + @""",
                """ + area + @""",
                """ + pincode + @""",
                """ + city + @""",
                """ + state + @""",
                """ + dealerType + @""",
                """ + company + @"""
            ],
            ""source"": ""new-landing-page form"",
            ""media"": {},
            ""buttons"": [],
            ""carouselCards"": [],
            ""location"": {},
            ""attributes"": {}
        }";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";

                byte[] data = Encoding.UTF8.GetBytes(jsonData);
                request.ContentLength = data.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string result = reader.ReadToEnd();
                        //Response.Write("<pre>" + Server.HtmlEncode(result) + "</pre>");
                    }
                }
            }
            catch (WebException ex)
            {
                using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    string error = reader.ReadToEnd();
                    //Response.Write("<pre>Error: " + Server.HtmlEncode(error) + "</pre>");
                }
            }
        }

        protected void txtPinCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPinCode.Text.Length == 6)
                {
                    SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pincode", SqlDbType.Int).Value = txtPinCode.Text.Trim();
                    cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 4;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        lblPincode.Attributes.Add("style", "display: none;");
                        txtCity.Text = dt.Rows[0]["CityName"].ToString();
                        txtState.Text = dt.Rows[0]["statename"].ToString();
                        txtCity.Enabled = false;
                        txtState.Enabled = false;
                        txtAddress.Focus();
                    }
                    else
                    {
                        txtCity.Text = "";
                        txtState.Text = "";
                        txtCity.Enabled = false;
                        txtState.Enabled = false;
                        txtPinCode.Focus();
                        lblPincode.Attributes.Add("style", "display: block;");
                        if (txtPinCode.Text.Length < 6)
                        {
                            lblPincode.InnerText = "Enter a 6-digit Pincode";
                        }
                        else
                        {
                            lblPincode.Attributes.Add("style", "display: block;");
                            lblPincode.InnerText = "Please enter Correct Pin Code";
                        }
                    }
                }
                else
                {
                    txtCity.Text = "";
                    txtState.Text = "";
                    lblPincode.InnerText = "Enter a 6-digit Pincode";
                    txtPinCode.Focus();
                    lblPincode.Attributes.Add("style", "display: block;");
                    //hdnActiveTab.Value = "#step1";
                    hdnActiveTab.Value = "#step2";
                }

                if (!string.IsNullOrWhiteSpace(txtAddress.Text))
                {
                    lblCurrentAddress.Attributes["style"] = "display: none;";
                }
                if (!string.IsNullOrWhiteSpace(txtLandmark.Text))
                {
                    lblCurrentlandmark.Attributes["style"] = "display: none;";
                }
                if (!string.IsNullOrWhiteSpace(txtSellerAddressLine1.Text))
                {
                    lblSellerAddress.Attributes["style"] = "display: none;";
                }
                if (!string.IsNullOrWhiteSpace(txtSellerLandmark.Text))
                {
                    lblSellerLandMark.Attributes["style"] = "display: none;";
                }
            }
            catch (Exception ex)
            {

                return;
            }
        }

        protected void txtSellerPinCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSellerPincode.Text.Length == 6)
                {
                    SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pincode", SqlDbType.Int).Value = txtSellerPincode.Text.Trim();
                    cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 4;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        lblSellerPINCode.Attributes.Add("style", "display: none;");
                        lblSellerCity.Text = dt.Rows[0]["CityName"].ToString();
                        lblSellerState.Text = dt.Rows[0]["statename"].ToString();
                        lblSellerCity.Visible = true;
                        lblSellerState.Visible = true;
                        lblSellerPINCode.InnerText = "";
                        txtSellerAddressLine1.Focus();

                    }
                    else
                    {
                        lblSellerPINCode.Attributes.Add("style", "display: block;");
                        lblSellerCity.Text = "";
                        lblSellerState.Text = "";
                        lblSellerCity.Text = null;
                        lblSellerState.Text = null;
                        lblSellerCity.Visible = false;
                        lblSellerState.Visible = false;
                        lblSellerPINCode.InnerText = "Please enter a valid pin code";
                        txtSellerPincode.Focus();
                    }
                }
                else
                {
                    lblSellerCity.Text = "";
                    lblSellerState.Text = "";
                    lblSellerPINCode.Attributes.Add("style", "display: block;");
                    lblSellerPINCode.InnerText = "Enter a 6-digit Pincode";
                    txtSellerPincode.Focus();
                    hdnActiveTab.Value = "#step2";
                }

                if (!string.IsNullOrWhiteSpace(txtAddress.Text))
                {
                    lblCurrentAddress.Attributes["style"] = "display: none;";
                }
                if (!string.IsNullOrWhiteSpace(txtLandmark.Text))
                {
                    lblCurrentlandmark.Attributes["style"] = "display: none;";
                }
                if (!string.IsNullOrWhiteSpace(txtSellerAddressLine1.Text))
                {
                    lblSellerAddress.Attributes["style"] = "display: none;";
                }
                if (!string.IsNullOrWhiteSpace(txtSellerLandmark.Text))
                {
                    lblSellerLandMark.Attributes["style"] = "display: none;";
                }
            }
            catch (Exception ex)
            {

                return;
            }
        }

        protected void txtIFSC_TextChanged(object sender, EventArgs e)
        {
            txtAccountNumber.Attributes["value"] = txtAccountNumber.Text;
            txtConfirmAccountNumber.Attributes["value"] = txtConfirmAccountNumber.Text;
            string pattern = @"^[A-Z]{4}0[A-Z0-9]{6}$";
            if (!Regex.IsMatch(txtIFSCCode.Text, pattern))
            {
                lblIFSCCode.Style["display"] = "block";
                lblIFSCCode.InnerText = "Invalid IFSC code format.";
                txtBankName.Text = "";
                txtBranchName.Text = "";
                txtBranchAddress.Text = "";
                return;
            }
            SqlCommand cmd = new SqlCommand("sp_iapl_crm_newsrvcall", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 11;
            cmd.Parameters.AddWithValue("@IFSC", SqlDbType.NVarChar).Value = txtIFSCCode.Text;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {

                txtBankName.Text = dt.Rows[0]["BANK"].ToString();
                txtBranchName.Text = dt.Rows[0]["BRANCH"].ToString();
                txtBranchAddress.Text = dt.Rows[0]["ADDRESS"].ToString();
                txtBranchAddress.Focus();

                lblIFSCCode.Style["display"] = "none";
            }
            else
            {
                txtBankName.Text = "";
                txtBranchName.Text = "";
                txtBranchAddress.Text = "";
                lblIFSCCode.Style["display"] = "block";
                lblIFSCCode.InnerText = "No matching IFSC code found.";
            }
            if (!string.IsNullOrWhiteSpace(txtAccountNumber.Text))
            {
                lblAccountNumber.Style["display"] = "none";
            }
            if (!string.IsNullOrWhiteSpace(txtConfirmAccountNumber.Text))
            {
                lblConfirmAccountNumber.Style["display"] = "none";
            }
            if (!string.IsNullOrWhiteSpace(txtAccountHolderName.Text))
            {
                lblAccountHoldername.Style["display"] = "none";
            }
            if (ddlSuppotingDoc.SelectedItem.Value != "")
            {
                lblsupportingDocError.Style["display"] = "none";
            }
            if (!fuSuppotingDoc.HasFile)
            {
                lblSupportingDocumentError.Style["display"] = "none";
            }
        }

        protected void btnSeller_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                if (string.IsNullOrWhiteSpace(txtAddress.Text))
                {
                    lblCurrentAddress.Attributes.Add("style", "display: block;");
                    txtAddress.Focus();
                    count++;
                }
                if (string.IsNullOrWhiteSpace(txtAddress.Text) || txtAddress.Text.Trim().Split(' ').Length < 3)
                {
                    lblCurrentAddress.InnerText = "Please enter at least three words.";
                    lblCurrentAddress.Attributes.Add("style", "display: block;");
                    txtAddress.Focus();
                    count++;
                }
                else
                {
                    lblCurrentAddress.Attributes.Add("style", "display: none;");
                }

                if (string.IsNullOrWhiteSpace(txtPinCode.Text))
                {
                    lblPincode.Attributes.Add("style", "display: block;");
                    txtPinCode.Focus();
                    count++;
                }
                if (txtPinCode.Text.Length != 6)
                {
                    lblPincode.InnerText = "Enter a 6-digit Pincode";
                    lblPincode.Attributes.Add("style", "display: block;");
                    txtPinCode.Focus();
                    count++;
                }
                else
                {
                    lblPincode.Attributes.Add("style", "display: none;");
                }
                if (string.IsNullOrWhiteSpace(txtLandmark.Text))
                {
                    lblCurrentlandmark.Attributes.Add("style", "display: block;");
                    lblCurrentlandmark.Focus();
                    count++;
                }
                else if (string.IsNullOrWhiteSpace(txtLandmark.Text) || txtLandmark.Text.Trim().Length < 3)
                {
                    lblCurrentlandmark.InnerText = "Please enter at least 3 characters.";
                    lblCurrentlandmark.Attributes.Add("style", "display: block;");
                    txtLandmark.Focus();
                    count++;
                }
                else
                {
                    lblCurrentlandmark.Attributes.Add("style", "display: none;");
                }


                if (string.IsNullOrWhiteSpace(txtSellerAddressLine1.Text))
                {
                    lblSellerAddress.Attributes.Add("style", "display: block;");
                    txtSellerAddressLine1.Focus();
                    count++;
                }
                else if (string.IsNullOrWhiteSpace(txtSellerAddressLine1.Text) || txtSellerAddressLine1.Text.Trim().Split(' ').Length < 3)
                {
                    lblSellerAddress.InnerText = "Please enter at least three words.";
                    lblSellerAddress.Attributes.Add("style", "display: block;");

                    txtSellerAddressLine1.Focus();
                    count++;
                }
                else
                {
                    lblSellerAddress.Attributes.Add("style", "display: none;");
                }

                if (string.IsNullOrWhiteSpace(txtSellerPincode.Text))
                {
                    lblSellerPINCode.Attributes.Add("style", "display: block;");
                    txtSellerPincode.Focus();
                    count++;
                }
                if (txtSellerPincode.Text.Length != 6)
                {
                    lblSellerPINCode.InnerText = "Enter a 6-digit Pincode";
                    lblSellerPINCode.Attributes.Add("style", "display: block;");
                    txtSellerPincode.Focus();
                    count++;
                }
                else
                {
                    lblSellerPINCode.Attributes.Add("style", "display: none;");
                }
                if (string.IsNullOrWhiteSpace(txtSellerLandmark.Text))
                {
                    lblSellerLandMark.Attributes.Add("style", "display: block;");
                    txtSellerLandmark.Focus();
                    count++;
                }
                else if (string.IsNullOrWhiteSpace(txtSellerLandmark.Text) || txtSellerLandmark.Text.Trim().Length < 3)
                {
                    lblSellerLandMark.InnerText = "Please enter at least 3 characters.";
                    lblSellerLandMark.Attributes.Add("style", "display: block;");
                    txtSellerLandmark.Focus();
                    count++;
                }
                else
                {
                    lblSellerLandMark.Attributes.Add("style", "display: none;");
                }
                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    lblEmailAddress.InnerText = "Email ID is required.";
                    lblEmailAddress.Attributes.Add("style", "display: block;");
                    txtEmail.Focus();
                    count++;
                }
                else if (!Regex.IsMatch(txtEmail.Text.Trim(), @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
                {
                    lblEmailAddress.InnerText = "Invalid email format.";
                    lblEmailAddress.Attributes.Add("style", "display: block;");
                    txtEmail.Focus();
                    count++;
                }
                else { lblEmailAddress.Attributes.Add("style", "display: none;"); }

                string email = txtEmail.Text.Trim();
                string altemail = txtAlternateEmail.Text.Trim();

                if (!string.IsNullOrEmpty(email) && email == altemail)
                {
                    lblAtlEmail.InnerText = "Email ID and alternate Email ID cannot be the same.";
                    lblAtlEmail.Attributes.Add("style", "display: block;");
                    txtAlternateMobile.Focus();
                    count++;
                }

                if (!string.IsNullOrWhiteSpace(txtAlternateEmail.Text) && !Regex.IsMatch(txtAlternateEmail.Text.Trim(), @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
                {
                    lblAtlEmail.InnerText = "Invalid email format.";
                    lblAtlEmail.Attributes.Add("style", "display: block;");
                    txtAlternateEmail.Focus();
                    count++;
                }
                else { lblAtlEmail.Attributes.Add("style", "display: none;"); }

                if (string.IsNullOrWhiteSpace(txtMobileNumber.Text))
                {
                    lblMobileNo.Attributes.Add("style", "display: block;");
                    txtMobileNumber.Focus();
                    count++;
                }

                if (string.IsNullOrEmpty(txtMobileNumber.Text) || txtMobileNumber.Text.Replace("+91", "").Length != 10)
                {
                    lblMobileNo.InnerText= "Please enter your phone number.";
                    lblMobileNo.Attributes.Add("style", "display: block;");
                    txtMobileNumber.Focus();
                    count++;
                }

                if (txtMobileNumber.Text.Length != 10)
                {
                    lblMobileNo.InnerText = "Invalid number.";
                    lblMobileNo.Attributes.Add("style", "display: block;");
                    txtMobileNumber.Focus();
                    count++;
                }

                if (Regex.IsMatch(txtMobileNumber.Text, @"^[0-5]"))
                {
                    lblMobileNo.InnerText = "Invalid number";
                    lblMobileNo.Attributes.Add("style", "display: block;");
                    txtMobileNumber.Focus();
                    count++;
                }

                if (Regex.IsMatch(txtMobileNumber.Text, @"^(\d)\1{9}$"))
                {
                    lblMobileNo.InnerText = "Invalid number.";
                    lblMobileNo.Attributes.Add("style", "display: block;");
                    txtMobileNumber.Focus();
                    count++;
                }
                else { lblMobileNo.Attributes.Add("style", "display: none;"); }

                ///////////////////////////
                if (!string.IsNullOrEmpty(txtAlternateMobile.Text) && txtAlternateMobile.Text.Replace("+91", "").Length != 10)
                {
                    lblAltMobileNo.InnerText = "Please enter your phone number.";
                    lblAltMobileNo.Attributes.Add("style", "display: block;");
                    txtAlternateMobile.Focus();
                    count++;
                }

                if (!string.IsNullOrEmpty(txtAlternateMobile.Text) && Regex.IsMatch(txtAlternateMobile.Text, @"^[0-5]"))
                {
                    lblAltMobileNo.InnerText = "Invalid number";
                    lblAltMobileNo.Attributes.Add("style", "display: block;");
                    txtAlternateMobile.Focus();
                    count++;
                }

                if (!string.IsNullOrEmpty(txtAlternateMobile.Text) && Regex.IsMatch(txtAlternateMobile.Text, @"^(\d)\1{9}$"))
                {
                    lblAltMobileNo.InnerText = "Invalid number.";
                    lblAltMobileNo.Attributes.Add("style", "display: block;");
                    txtAlternateMobile.Focus();
                    count++;
                }
                else { lblAltMobileNo.Attributes.Add("style", "display: none;"); }

                string mobile = txtMobileNumber.Text.Trim();
                string altMobile = txtAlternateMobile.Text.Trim();

                if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
                {
                    lblMobileNo.InnerText = "Mobile No and alternate mobile numbers cannot be the same.";
                    lblAltMobileNo.InnerText = "Mobile No and alternate mobile numbers cannot be the same.";
                    lblAltMobileNo.Attributes.Add("style", "display: block;");
                    txtAlternateMobile.Focus();
                    count++;
                }

                if (ddlFirmType.SelectedValue == "Select")
                {
                    lblFirmTypeError.Attributes.Add("style", "display: block;");
                    lblFirmTypeError.Visible = true;
                    count++;
                }
                else
                {
                    lblFirmTypeError.Visible = false;

                }

                if (!string.IsNullOrWhiteSpace(txtFirstName.Text) && txtFirstName.Text.Length < 3)
                {
                    lblFullName.InnerText = "Owner Name must be between 3 and 30 characters.";
                    lblFullName.Attributes.Add("style", "display: block;");
                    txtFirstName.Focus();
                    count++;
                }
                if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    lblFullName.InnerText = "Owner Name is required.";
                    lblFullName.Attributes.Add("style", "display: block;");
                    txtFirstName.Focus();
                    count++;
                }
                else
                {
                    lblFullName.Attributes.Add("style", "display: none;");
                }
                string gstinPattern = @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[A-Z0-9]{1}[Z]{1}[A-Z0-9]{1}$";
                if (string.IsNullOrWhiteSpace(txtGSTIN.Text))
                {
                    lblSellerGSTIN.Attributes.Add("style", "display: block;");
                    txtGSTIN.Focus();
                    count++;
                }               
                else if (!Regex.IsMatch(txtGSTIN.Text.Trim().ToUpper(), gstinPattern))
                {
                    lblSellerGSTIN.InnerText = "Invalid GSTIN format.";
                    lblSellerGSTIN.Attributes.Add("style", "display: block;");
                    txtGSTIN.Focus();
                    count++;
                }                
                else
                {
                    lblSellerGSTIN.Attributes.Add("style", "display: none;");
                }
                if (txtSellerName.Text.Length < 3)
                {
                    lblSellerName.InnerText = "Legal Name must be between 3 and 30 characters.";
                    lblSellerName.Attributes.Add("style", "display: block;");
                    txtSellerName.Focus();
                    count++;
                }
                if (string.IsNullOrWhiteSpace(txtSellerName.Text))
                {
                    lblSellerName.InnerText = "Legal Name is required.";
                    lblSellerName.Attributes.Add("style", "display: block;");
                    txtSellerName.Focus();
                    count++;
                }
                else
                {
                    lblSellerName.Attributes.Add("style", "display: none;");
                }
                if (count > 0)
                {
                    hdnActiveTab.Value = "#step2";
                    return;
                }
                if (!string.IsNullOrWhiteSpace(txtSellerName.Text) && (txtSellerName.Text.Length >= 3) && !string.IsNullOrWhiteSpace(txtGSTIN.Text))
                {
                    bool add = AddEditPersonal();
                    if (add)
                    {
                        bool dealer = AddEditDealer();
                        if (dealer)
                        {
                            bool checkByMobile = CheckRetailerByMobile();
                            if (!checkByMobile)
                            {
                                CreateTicket();
                            }
                            else
                            {
                                UpdateRetailerSKU();
                            }

                            string script = "$('.nav-tabs > .active').next('li').find('a').click();";
                            ScriptManager.RegisterStartupScript(this, GetType(), "MoveToNextTab", script, true);
                            //hdnActiveTab.Value = "#step1";
                            hdnActiveTab.Value = "#step3";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                hdnActiveTab.Value = "#step2";
            }

        }

        protected void gvDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (ViewState["DocumentData"] != null)
            {
                DataTable dt = (DataTable)ViewState["DocumentData"];
                int index = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "View")
                {
                    string filePath = dt.Rows[index]["DocumentPath"].ToString();
                    string fullPath = Server.MapPath("~/UploadedDocuments/" + filePath);

                    if (System.IO.File.Exists(fullPath))
                    {
                        string url = ResolveUrl("~/UploadedDocuments/" + filePath);
                        ScriptManager.RegisterStartupScript(this, GetType(), "OpenDoc", $"window.open('{url}', '_blank');", true);
                    }
                }
                else if (e.CommandName == "DeleteDoc")
                {
                    string docId = dt.Rows[index]["DocId"].ToString();
                    string docName = dt.Rows[index]["DocumentName"].ToString();

                    string filePath = dt.Rows[index]["DocumentPath"].ToString();
                    string fullPath = Server.MapPath("~/UploadedDocuments/" + filePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    dt.Rows.RemoveAt(index);
                    for (int i = 0; i < dt.Rows.Count; i++)
                        dt.Rows[i]["SrNo"] = i + 1;


                    if (ddlDocumentName.Items.FindByValue(docId) == null)
                    {
                        ddlDocumentName.Items.Add(new ListItem(docName, docId));
                    }
                    ViewState["DocumentData"] = dt;
                    gvDocuments.DataSource = dt;
                    gvDocuments.DataBind();
                    if (gvDocuments.HeaderRow != null)
                        gvDocuments.HeaderRow.TableSection = TableRowSection.TableHeader;
                    //CheckIfBothDocumentsUploaded();
                }
            }
        }
        protected void gvDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string fullDocNumber = DataBinder.Eval(e.Row.DataItem, "DocumentNumber")?.ToString();

                //if (!string.IsNullOrEmpty(fullDocNumber) && fullDocNumber.Length > 4)
                //{
                //    string masked = new string('*', fullDocNumber.Length - 4) + fullDocNumber.Substring(fullDocNumber.Length - 4);
                //    Label lblMaskedDoc = (Label)e.Row.FindControl("lblMaskedDoc");
                //    if (lblMaskedDoc != null)
                //    {
                //        lblMaskedDoc.Text = masked;
                //    }
                //}
                Label lblMaskedDoc = (Label)e.Row.FindControl("lblMaskedDoc");
                lblMaskedDoc.Text = fullDocNumber;
            }
        }
        protected void btnBackPersonalInformationHidden(object sender, EventArgs e)
        {
            hdnActiveTab.Value = "#step1";
        }
        protected void btnBackSellerInformationHidden(object sender, EventArgs e)
        {
            hdnActiveTab.Value = "#step2";
        }
        protected void btnBackDealerInfo(object sender, EventArgs e)
        {
            //hdnActiveTab.Value = "#step1";
            hdnActiveTab.Value = "#step2";
        }
        protected void btnBackBankDetails(object sender, EventArgs e)
        {
            hdnActiveTab.Value = "#step3";
        }
        protected void btnVerifyOTP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtVerifyOTP.Text))
            {
                lblMessage.Text = "Please enter the OTP.";
                lblMessage.Style["color"] = "red !important";
                lnkChangeBussinessMobileNo.Visible = false;
                return;
            }
            string enteredOtp = txtVerifyOTP.Text.Trim();
            string sessionOtp = Session["OTP"] as string;

            if (sessionOtp == null)
            {
                lblMessage.Text = "OTP has expired or not generated.";
                lblMessage.Style["color"] = "red !important";
                lnkChangeBussinessMobileNo.Visible = false;
                return;
            }
            if (Session["OTP"] != null && txtVerifyOTP.Text == Session["OTP"].ToString())
            {
                otpdiv.Visible = false;
                btnSeller.Enabled = true;
                txtOfficialMobileNo.Enabled = false;
                Label1.Visible = true;
                btnSeller.Visible = true;
                // lnkChangeBussinessMobileNo.Visible = false;
                Label1.InnerText = "Mobile No verified.";
                Label1.Style["color"] = "#0db02b";
                lnkChangeBussinessMobileNo.Visible = false;
                lnkResendOTP.Visible = false;
            }
            else
            {
                lblMessage.Text = "Invalid OTP.";
                lblMessage.Style["color"] = "red !important";
                lnkChangeBussinessMobileNo.Visible = false;
            }
        }
        protected void btnSendOTP_Click(Object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOfficialMobileNo.Text))
            {
                Label1.InnerText = "Please enter your mobile number.";
                Label1.Style["color"] = "red !important";
                Label1.Visible = true;
                return;
            }
            else if (txtOfficialMobileNo.MaxLength < 10)
            {
                Label1.InnerText = "Please enter valid mobile number.";
                Label1.Style["color"] = "red !important";
                Label1.Visible = true;
                return;
            }
            else
            {
                string newotpValue = newotp();
                Session["OTP"] = newotpValue;
                sendSMSOTP(txtOfficialMobileNo.Text, newotpValue);
                lblMessage.Text = "OTP sent successfully.";

                otpdiv.Visible = true;
                Label1.Visible = false;
                btnSendOTP.Visible = false;
                lnkResendOTP.Visible = true;
                txtOfficialMobileNo.Enabled = false;
                lnkChangeBussinessMobileNo.Visible = true;
            }
        }
        protected void sendSMSOTP(string mobileno, string otp)
        {
            try
            {
                string message = "Welcome to Infinity, Your OTP to Login to Infinity TechCare Lounge is " + otp + ". For Help, Call Infinity 8447882424. 9AM-6PM Mon-Sat";
                string content_temID = "1107162426891569578";
                string sender12 = "ISHILD";

                string url = $"https://api.mobilnxt.in/api/push?accesskey=uW9h2HHRlctDRlGwOQKEicLgsgBi2V&to={mobileno}&text={message}&from={sender12}&tid={content_temID}";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
                StreamReader respStreamReader = new StreamReader(myResp.GetResponseStream());
                string responseString = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                myResp.Close();
            }
            catch (Exception) { return; }
        }
        private string newotp()
        {
            string numbers = "1234567890";
            string otp = "";
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
                otp += numbers[rand.Next(numbers.Length)];
            return otp;
        }
        private bool CheckIfBothDocumentsUploaded()
        {
            bool hasAadhaar = false;
            bool hasPAN = false;
            bool hasGST = false;

            foreach (GridViewRow row in gvDocuments.Rows)
            {
                string docName = ((Label)row.FindControl("lblMaskedDoc"))?.Text.ToLower();
                string docId = gvDocuments.DataKeys[row.RowIndex]["DocId"].ToString();

                if (docId == "13" || docId == "57" || docId == "58")
                {
                    hasAadhaar = true;
                }
                else if (docId == "19")
                {
                    hasPAN = true;
                }
                else if (docId == "146")
                {
                    hasGST = true;
                }

                if (docName.Contains("aadhaar") || docName.Contains("aadhar"))
                    hasAadhaar = true;
                if (docName.Contains("pan"))
                    hasPAN = true;

                var itemToRemove = ddlDocumentName.Items.FindByValue(docId);
                if (itemToRemove != null)
                {
                    ddlDocumentName.Items.Remove(itemToRemove);
                }
            }

            btnnext3.Visible = hasAadhaar && hasPAN;
            if (hasAadhaar && hasPAN && hasGST)
            {
                lblRequiredDocuments.Visible = false;
                lblRequiredDocError.Text =  "";
                lblRequiredDocError.Attributes.Add("style", "display: none;");
                return true;
            }
            else
            {
                lblRequiredDocuments.Visible = true;
                lblRequiredDocError.Text = "Please submit Aadhaar, PAN card, and GST certificate details";
                lblRequiredDocError.Attributes.Add("style", "display: block;");
                return false;
            }
        }
        protected void btnSkip_Click(object sender, EventArgs e)
        {
            hdnActiveTab.Value = "#step4";
        }

        protected void BindInformation()
        {
            SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", 3);
            cmd.Parameters.AddWithValue("@Mid", Session["RetailerUniqueID"].ToString());

            cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());
            if (con.State != ConnectionState.Open)
                con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {


                if (dr.Read())
                {
                    //Personal Information
                    txtFirstName.Text = dr["Name"].ToString();
                    txtMobileNumber.Text = dr["MobileNo"].ToString();
                    txtAlternateMobile.Text = dr["MobileNo_2"].ToString();
                    txtEmail.Text = dr["EmailID"].ToString();
                    txtAlternateEmail.Text = dr["emailID_2"].ToString();
                    TextBox1.Text = dr["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateOfBirth"]).ToString("dd-MMM-yyyy") : null;
                    ddlGender.SelectedValue = dr["Gender"].ToString();
                    txtPinCode.Text = dr["perPincode"].ToString();
                    txtCity.Text = dr["perCity"].ToString();
                    txtState.Text = dr["perState"].ToString();
                    txtAddress.Text = dr["perAddressLine1"].ToString();
                    txtAddress1.Text = dr["perAddressLine2"].ToString();
                    txtLandmark.Text = dr["perLandmark"].ToString();
                    Session["UniqueMid"] = Session["RetailerUniqueID"].ToString();

                    if(dr["MobileNo_CheckWhatsapp"] != null && !string.IsNullOrWhiteSpace(dr["MobileNo_CheckWhatsapp"].ToString()))
                    {
                        chkMobileNumberWhatsApp.Checked = true;
                    }
                    if (dr["MobileNo2_CheckWhatsapp"] != null && !string.IsNullOrWhiteSpace(dr["MobileNo2_CheckWhatsapp"].ToString()))
                    {
                        chkAlternativeMobileNumber.Checked = true;
                    }

                    txtSellerName.Text = dr["SellerName"].ToString();
                    txtGSTIN.Text = dr["SellerGSTINNo"].ToString();
                    txtGSTIN.Enabled = false;
                    txtOfficialMobileNo.Text = dr["OfficialMobileNo"].ToString();
                    txtOfficialEmail.Text = dr["OfficialEmail"].ToString();
                    ddlFirmType.SelectedValue = dr["FirmType"].ToString();
                    txtSellerPincode.Text = dr["DealerPincode"].ToString();
                    lblSellerCity.Text = dr["DealerCity"].ToString();
                    lblSellerState.Text = dr["DealerState"].ToString();
                    txtSellerAddressLine1.Text = dr["DealerAddressLine1"].ToString();
                    txtSellerAddressLine2.Text = dr["DealerAddressLine2"].ToString();
                    txtSellerLandmark.Text = dr["DealerLandmark"].ToString();
                    Session["SellerMid"] = dr["SellerId"].ToString();

                    //Bank Information
                    txtAccountNumber.Text = dr["BankAccountNumber"].ToString();
                    txtConfirmAccountNumber.Text = dr["BankAccountNumber"].ToString();
                    txtIFSCCode.Text = dr["IFSCCode"].ToString();
                    txtAccountHolderName.Text = dr["BankAccountNumber"].ToString();
                    txtBankName.Text = dr["BankName"].ToString();
                    txtBranchName.Text = dr["BankBranch"].ToString();
                    txtBranchAddress.Text = dr["BankBranchAddress"].ToString();
                    Session["bankMid"] = dr["BankId"].ToString();
                }
            }
            dr.Close();
            con.Close();

        }
        protected void lnkChangeBussinessMobileNo_Click(object sender, EventArgs e)
        {
            lnkChangeBussinessMobileNo.Visible = false;
            btnSeller.Enabled = false;
            txtOfficialMobileNo.Enabled = true;
            btnSendOTP.Visible = true;
            lnkResendOTP.Visible = false;
            Label1.Visible = false;
            otpdiv.Visible = false;
        }

        protected void txtBussinessEmail_TextChanged(object sender, EventArgs e)
        {
            bool blockuser = CheckBlockCustomer(txtOfficialEmail.Text, "");
            bool existuser = CheckSalesPersonOrRetailer(txtOfficialEmail.Text, "");
            if (existuser)
            {
                Label2.InnerText = "This Email is already registered";
                Label1.InnerText = "";
                btnSeller.Enabled = false;
                Label2.Visible = true;
                return;
            }
            else if (blockuser)
            {
                Label2.InnerText = Label1.InnerText;
                Label1.InnerText = "";
                btnSeller.Enabled = false;
                Label2.Visible = true;
                return;
            }
            lblAtlEmail.InnerText = "";
            lblAtlEmail.Attributes.Add("style", "display: none;");
            Label2.InnerText = "";
            Label2.Visible = false;
            btnSeller.Enabled = true;
        }
        protected void txtBussinessMobile_TextChanged(object sender, EventArgs e)
        {
            bool blockuser = CheckBlockCustomer("", txtOfficialMobileNo.Text);
            bool existuser = CheckSalesPersonOrRetailer("", txtOfficialMobileNo.Text);
            if (existuser)
            {
                Label1.InnerText = "This Mobile No is already registered";
                btnSeller.Enabled = false;
                Label1.Visible = true;
                return;
            }
            else if (blockuser)
            {
                Label1.InnerText = Label1.InnerText;
                btnSeller.Enabled = false;
                Label1.Visible = true;
                return;
            }
            Label1.InnerText = "";
            btnSeller.Enabled = true;
            Label1.Visible = false;
        }

        protected void txtCustomerEmail_TextChanged(object sender, EventArgs e)
        {
            bool blockuser = CheckBlockCustomer(txtEmail.Text, "");
            bool existuser = CheckSalesPersonOrRetailer(txtEmail.Text, "");
            if (existuser)
            {
                lblEmailAddress.InnerText = "This Email is already registered";
                Label1.InnerText = "";
                //btnNext.Enabled = false;
                btnSeller.Enabled=false;
                lblEmailAddress.Attributes.Add("style", "display: block;");
                txtEmail.Focus();
                return;
            }
            else if (blockuser)
            {
                lblEmailAddress.InnerText = Label1.InnerText;
                Label1.InnerText = "";
                //btnNext.Enabled = false;
                btnSeller.Enabled = false;
                lblEmailAddress.Attributes.Add("style", "display: block;");
                txtEmail.Focus();
                return;
            }
            string mobile = txtEmail.Text.Trim();
            string altMobile = txtAlternateEmail.Text.Trim();

            if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
            {
                lblEmailAddress.InnerText = "Email ID and alternate Email ID cannot be the same.";
                lblEmailAddress.Attributes.Add("style", "display: block;");
                txtEmail.Focus();
                return;
            }
            //btnNext.Enabled = true;
            btnSeller.Enabled = true;
            lblEmailAddress.InnerText = "";
            lblEmailAddress.Attributes.Add("style", "display: none;");
            lblAtlEmail.InnerText = "";
            lblAtlEmail.Attributes.Add("style", "display: none;");
            txtAlternateEmail.Focus();

            if (txtSellerName.Text != "")
            {
                lblSellerName.Attributes["style"] = "display: none;";
            }
            if (txtFirstName.Text != "")
            {
                lblFullName.Attributes["style"] = "display: none;";
            }
            if(ddlFirmType.SelectedItem.Value != "Select")
            {
                lblFirmTypeError.Attributes["style"] = "display: none;";
            }
        }
        protected void txtCustomerMobile_TextChanged(object sender, EventArgs e)
        {
            bool blockuser = CheckBlockCustomer("", txtMobileNumber.Text);
            bool existuser = CheckSalesPersonOrRetailer("", txtMobileNumber.Text);
            if (existuser)
            {
                lblMobileNo.InnerText = "This Mobile No is already registered";
                //btnNext.Enabled = false;
                btnSeller.Enabled = false;
                lblMobileNo.Visible = true;
                return;
            }
            else if (blockuser)
            {
                lblMobileNo.InnerText = Label1.InnerText;
                //btnNext.Enabled = false;
                btnSeller.Enabled = false;
                lblMobileNo.Visible = true;
                return;
            }
            lblMobileNo.InnerText = "";
            lblMobileNo.Visible = false;
            //btnNext.Enabled = true;
            btnSeller.Enabled = true;
        }
        protected void txtAltEmail_TextChanged(object sender, EventArgs e)
        {
            bool blockuser = CheckBlockCustomer(txtAlternateEmail.Text, "");
            bool existuser = CheckSalesPersonOrRetailer(txtAlternateEmail.Text, "");
            if (existuser)
            {
                lblAtlEmail.InnerText = "This Email is already registered";
                lblAltMobileNo.InnerText = "";
                //btnNext.Enabled = false;
                btnSeller.Enabled = false;
                lblAtlEmail.Attributes.Add("style", "display: block;");
                return;
            }
            else if (blockuser)
            {
                lblAtlEmail.InnerText = Label1.InnerText;
                lblAltMobileNo.InnerText = "";
                //btnNext.Enabled = false;
                btnSeller.Enabled = false;
                lblAtlEmail.Attributes.Add("style", "display: block;");
                return;
            }
            string mobile = txtEmail.Text.Trim();
            string altMobile = txtAlternateEmail.Text.Trim();

            if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
            {
                lblAtlEmail.InnerText = "Email ID and alternate Email ID cannot be the same.";
                lblAtlEmail.Attributes.Add("style", "display: block;");
                txtAlternateMobile.Focus();
                return;
            }

            lblAtlEmail.InnerText = "";
            lblAtlEmail.Attributes.Add("style", "display: none;");
            //btnNext.Enabled = true;
            btnSeller.Enabled = true;
        }
        protected void txtAltCustomerMobile_TextChanged(object sender, EventArgs e)
        {
            bool blockuser = CheckBlockCustomer("", txtAlternateMobile.Text);
            bool existuser = CheckSalesPersonOrRetailer("", txtAlternateMobile.Text);
            if (existuser)
            {
                lblAltMobileNo.InnerText = "This Mobile No is already registered";
                //btnNext.Enabled = false;
                btnSeller.Enabled = false;
                lblAltMobileNo.Attributes.Add("style", "display: block;");
                return;
            }
            else if (blockuser)
            {
                lblAltMobileNo.InnerText = Label1.InnerText;
                //btnNext.Enabled = false;
                btnSeller.Enabled = false;
                lblAltMobileNo.Attributes.Add("style", "display: block;");
                return;
            }
            if (!string.IsNullOrEmpty(txtAlternateMobile.Text) && txtAlternateMobile.Text.Replace("+91", "").Length != 10)
            {
                lblAltMobileNo.InnerText = "Please enter your phone number.";
                lblAltMobileNo.Attributes.Add("style", "display: block;");
                txtAlternateMobile.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtAlternateMobile.Text) && Regex.IsMatch(txtAlternateMobile.Text, @"^[0-5]"))
            {
                lblAltMobileNo.InnerText = "Invalid number";
                lblAltMobileNo.Attributes.Add("style", "display: block;");
                txtAlternateMobile.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtAlternateMobile.Text) && Regex.IsMatch(txtAlternateMobile.Text, @"^(\d)\1{9}$"))
            {
                lblAltMobileNo.InnerText = "Invalid number.";
                lblAltMobileNo.Attributes.Add("style", "display: block;");
                txtAlternateMobile.Focus();
                return;
            }
            string mobile = txtMobileNumber.Text.Trim();
            string altMobile = txtAlternateMobile.Text.Trim();

            if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
            {
                lblAltMobileNo.InnerText = "Mobile No and alternate mobile numbers cannot be the same.";
                lblAltMobileNo.Attributes.Add("style", "display: block;");
                txtAlternateMobile.Focus();
                return;
            }
            else { lblAltMobileNo.Attributes.Add("style", "display: none;"); }
            lblAltMobileNo.InnerText = "";
            //btnNext.Enabled = true;
            btnSeller.Enabled = true;
            lblAltMobileNo.Attributes.Add("style", "display: none;");
        }
        protected bool CheckBlockCustomer(string email, string mobileno)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerMobileNo", SqlDbType.VarChar).Value = !string.IsNullOrWhiteSpace(mobileno) ? mobileno.ToString() : null;
                cmd.Parameters.AddWithValue("@CustomerEmailID", SqlDbType.VarChar).Value = !string.IsNullOrWhiteSpace(email) ? email.ToString() : null;
                cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 39;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string suspicious = dt.Rows[0]["suspicious"].ToString();
                    if (suspicious == "1")
                    {
                        Label1.InnerText = "This Customer is Under Watch.Please contact your Manager";
                        return true;
                    }
                    else if (suspicious == "2")
                    {
                        Label1.InnerText = "This Customer is Black Listed..Please contact your Manager";
                        return true;
                    }
                    else if (suspicious == "")
                    {
                        Label1.InnerText = "";
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
        }
        protected bool CheckSalesPersonOrRetailer(string email, string mobileno)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerMobileNo", !string.IsNullOrWhiteSpace(mobileno) ? mobileno : null);
                    cmd.Parameters.AddWithValue("@CustomerEmailID", !string.IsNullOrWhiteSpace(email) ? email : null);
                    cmd.Parameters.AddWithValue("@Mid", "");
                    cmd.Parameters.AddWithValue("@type", 50);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        Label1.InnerText = "This Mobile No or Email is already registered";
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected bool AddEditDealer()
        {
            SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (Session["SellerMid"] != null && Session["SellerMid"].ToString() != "")
            {
                cmd.Parameters.AddWithValue("@Type", 28);
                cmd.Parameters.AddWithValue("@Mid", Session["SellerMid"].ToString());
            }
            else
                cmd.Parameters.AddWithValue("@Type", 16);
            cmd.Parameters.AddWithValue("@CustomerName", txtSellerName.Text.ToUpper().Trim());
            cmd.Parameters.AddWithValue("@SellerGSTINNo", txtGSTIN.Text.Trim());
            cmd.Parameters.AddWithValue("@AddressLine1", txtSellerAddressLine1.Text.Trim());
            cmd.Parameters.AddWithValue("@AddressLine2", txtSellerAddressLine2.Text.Trim());
            cmd.Parameters.AddWithValue("@Landmark", txtSellerLandmark.Text.Trim());
            cmd.Parameters.AddWithValue("@Pincode", txtSellerPincode.Text.Trim());
            cmd.Parameters.AddWithValue("@City", lblSellerCity.Text.Trim());
            cmd.Parameters.AddWithValue("@State", lblSellerState.Text.Trim());
            cmd.Parameters.AddWithValue("@CustomerEmailID", txtOfficialEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@CustomerMobileNo", txtOfficialMobileNo.Text.Trim());
            cmd.Parameters.AddWithValue("@FirmType", ddlFirmType.SelectedItem.Text.Trim());
            cmd.Parameters.AddWithValue("@ProfileId", Session["UniqueMid"].ToString().Trim());

            if (con.State != ConnectionState.Open)
                con.Open();
            object result = cmd.ExecuteScalar();
            con.Close();
            if (result != null && result.ToString() == "0")
            {
                lblSellerGSTIN.InnerText = "Seller with this GSTIN already exists.";
                lblSellerGSTIN.Visible = true;
                return false;
            }

            else
            {
                Session["SellerMid"] = result.ToString();
                return true;
            }
        }

        protected bool AddEditPersonal()
        {
            SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (Session["UniqueMid"] != null && Session["UniqueMid"].ToString() != "")
            {
                cmd.Parameters.AddWithValue("@Type", 23);
                cmd.Parameters.AddWithValue("@ProfileId", Session["UniqueMid"].ToString());
            }
            else
                cmd.Parameters.AddWithValue("@Type", 2);

            cmd.Parameters.AddWithValue("@Name", txtFirstName.Text.Trim());
            // cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
            cmd.Parameters.AddWithValue("@MobileNo", txtMobileNumber.Text.Trim());
            cmd.Parameters.AddWithValue("@MobileNo_2", txtAlternateMobile.Text.Trim());
            cmd.Parameters.AddWithValue("@EmailID", txtEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@EmailID_2", txtAlternateEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@PinCode", txtPinCode.Text.Trim());
            cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
            cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
            cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
            cmd.Parameters.AddWithValue("@Address2", txtAddress1.Text.Trim());
            cmd.Parameters.AddWithValue("@Landmark", txtLandmark.Text.Trim());
            cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@Status", "1");
            DateTime dob;
            if (DateTime.TryParse(TextBox1.Text.Trim(), out dob))
                cmd.Parameters.AddWithValue("@DateOfBirth", dob.Date);
            else
                cmd.Parameters.AddWithValue("@DateOfBirth", DBNull.Value);
            cmd.Parameters.AddWithValue("@MobileNo_CheckWhatsapp", chkMobileNumberWhatsApp.Checked ? txtMobileNumber.Text.Trim() : null);
            cmd.Parameters.AddWithValue("@MobileNo2_CheckWhatsapp", chkAlternativeMobileNumber.Checked ? txtMobileNumber.Text.Trim() : null);
            cmd.Parameters.AddWithValue("@SellerId", Session["SellerMid"] != null ? Session["SellerMid"].ToString() : null);

            SqlParameter outputMid = new SqlParameter("@FreelanerIdd", SqlDbType.Int);
            outputMid.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputMid);

            if (con.State != ConnectionState.Open)
                con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (outputMid.Value != null && outputMid.Value.ToString() == "-2")
            {
                lblMobileNo.InnerText = "This mobile number is already registered.";
                lblMobileNo.Visible = true;
                return true;

            }
            else if (outputMid.Value != null)
            {
                Session["UniqueMid"] = Convert.ToInt32(outputMid.Value);
                return true;
            }
            return false;
        }

        protected void SameAsPersonalAddress(object sender, EventArgs e)
        {
            if (chkSameAddress.Checked)
            {
                lblSellerCity.Visible = true;
                lblSellerState.Visible = true;

                txtSellerPincode.Text = txtPinCode.Text;
                lblSellerCity.Text = txtCity.Text;
                lblSellerState.Text = txtState.Text;
                txtSellerAddressLine1.Text = txtAddress.Text;
                txtSellerAddressLine2.Text = txtAddress1.Text;
                txtSellerLandmark.Text = txtLandmark.Text;
            }
            else
            {
                lblSellerCity.Visible = false;
                lblSellerState.Visible = false;

                txtSellerPincode.Text = "";
                lblSellerCity.Text = "";
                lblSellerState.Text = "";
                txtSellerAddressLine1.Text = "";
                txtSellerAddressLine2.Text = "";
                txtSellerLandmark.Text = "";
            }
        }
        protected void ddlTypeOfBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTypeOfBank.SelectedValue == "Savings")
            {
                jointAcountpnl.Visible = true;
            }
            else
            {
                jointAcountpnl.Visible = false;
                jointAcountHolderpnl.Visible = false;
            }
            if (!string.IsNullOrWhiteSpace(txtAccountNumber.Text))
            {
                lblAccountNumber.Style["display"] = "none";
            }
            if (!string.IsNullOrWhiteSpace(txtConfirmAccountNumber.Text))
            {
                lblConfirmAccountNumber.Style["display"] = "none";
            }
            if (!string.IsNullOrWhiteSpace(txtAccountHolderName.Text))
            {
                lblAccountHoldername.Style["display"] = "none";
            }
            if (ddlSuppotingDoc.SelectedItem.Value != "")
            {
                lblsupportingDocError.Style["display"] = "none";
            }
            if (!fuSuppotingDoc.HasFile)
            {
                lblSupportingDocumentError.Style["display"] = "none";
            }
        }
        protected void chkJointAccount_Change(object sender, EventArgs e)
        {
            if (chkJointAccount.SelectedValue == "Yes")
            {
                jointAcountHolderpnl.Visible = true;
            }
            else
            {
                jointAcountHolderpnl.Visible = false;
            }
            if (!string.IsNullOrWhiteSpace(txtAccountNumber.Text))
            {
                lblAccountNumber.Style["display"] = "none";
            }
            if (!string.IsNullOrWhiteSpace(txtConfirmAccountNumber.Text))
            {
                lblConfirmAccountNumber.Style["display"] = "none";
            }
            if (!string.IsNullOrWhiteSpace(txtAccountHolderName.Text))
            {
                lblAccountHoldername.Style["display"] = "none";
            }
            if (ddlSuppotingDoc.SelectedItem.Value != "")
            {
                lblsupportingDocError.Style["display"] = "none";
            }
            if (!fuSuppotingDoc.HasFile)
            {
                lblSupportingDocumentError.Style["display"] = "none";
            }
        }

        protected void btnDashboard_Click(object sender, EventArgs e)
        {
            // ✅ First, check if the session is valid
            if (Session["MobileNo"] == null)
            {
                // Redirect to login if session expired
                Response.Redirect("~/index.aspx");
                return;
            }

            SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", 1);
            cmd.Parameters.AddWithValue("@MobileNo", Session["MobileNo"] != null ? Session["MobileNo"].ToString() : txtMobileNumber.Text.Trim());

            if (con.State != ConnectionState.Open)
                con.Open();
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    Session["RetailerUniqueID"] = dr["RetailerUniqueID"];
                    Session["Name"] = dr["Name"];
                    Session["MobileNo"] = dr["MobileNo"];
                    Session["Status"] = dr["Status"];
                    Session["Role"] = dr["Role"];
                    Session["Email"] = dr["emailID"];
                    Response.Redirect("Dashboard.aspx");
                }
            }
            con.Close();
        }
        protected void chkGSTIN_Change(object sender, EventArgs e)
        {
            if (Session["SellerGSTIN"] == null)
            {
                Response.Redirect("~/index.aspx");
                return;
            }
            if (chckGSTIN.Checked)
            {
                txtDocumentNumber.Text = Session["SellerGSTIN"].ToString();
                txtDocumentNumber.Enabled = false;
                chckGSTIN.Attributes.Add("style", "display:block");
            }
            else
            {
                txtDocumentNumber.Text = "";
                txtDocumentNumber.Enabled = true;
                chckGSTIN.Attributes.Add("style", "display:block");
            }

            hdnActiveTab.Value = "#step4";
        }
        protected void AccountNumberChange(object sender, EventArgs e)
        {
            try
            {
                string qu = Request.QueryString["qu"];
                string decoded = "0";
                if (!string.IsNullOrWhiteSpace(qu))
                {
                    decoded = Encoding.UTF8.GetString(Convert.FromBase64String(qu));
                }
                bool AccountNumber = AccountNumberValidation(txtAccountNumber.Text, decoded);
            }
            catch (Exception ex)
            {
            }
        }
        protected bool AccountNumberValidation(string accountNumber, string mid)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 82;
                cmd.Parameters.AddWithValue("@ProfileId", SqlDbType.Int).Value = mid;
                cmd.Parameters.AddWithValue("@BankAccountNumber", SqlDbType.NVarChar).Value = accountNumber;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    lblAccountNumber.Style.Add("display", "block");
                    lblAccountNumber.InnerText = "This bank account number already exists in our records.";
                    return true;
                }
                else
                {
                    lblAccountNumber.Style.Add("display", "none");
                    lblAccountNumber.InnerText = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        protected bool CheckToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 88);
                cmd.Parameters.AddWithValue("@token", token);

                if (con.State != ConnectionState.Open)
                    con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string mobile = reader["MobileNo1"].ToString();
                        return true;
                    }
                }
                con.Close();
            }
            return false;
        }

        protected bool CheckRetailerByMobile()
        {
            if (txtMobileNumber.Text == null || string.IsNullOrWhiteSpace(txtMobileNumber.Text.ToString()))
                return false;

            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 91);
                cmd.Parameters.AddWithValue("@mobileno", txtMobileNumber.Text != null ? txtMobileNumber.Text.ToString() : "");
                if (con.State != ConnectionState.Open)
                    con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string mobile = reader["MobileNo1"].ToString();
                        if (string.IsNullOrWhiteSpace(mobile))
                            return false;
                        Session["MobileNo1"] = mobile;
                        return true;
                    }
                }
                con.Close();
            }

            return false;
        }

        protected void CreateTicket()
        {
            try
            {
                    
                    using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@type", 90);
                        cmd.Parameters.AddWithValue("@PlanID", "1");
                        cmd.Parameters.AddWithValue("@ProCat","3");
                        cmd.Parameters.AddWithValue("@userID", Session["UniqueMid"] != null ? Session["UniqueMid"].ToString() : "");

                        cmd.Parameters.AddWithValue("@customername", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@addressline1", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@addressline2", txtAddress1.Text);
                        cmd.Parameters.AddWithValue("@addressline3", "");
                        cmd.Parameters.AddWithValue("@city", txtCity.Text);
                        cmd.Parameters.AddWithValue("@state", txtState.Text);
                        cmd.Parameters.AddWithValue("@pincode", txtPinCode.Text);
                        cmd.Parameters.AddWithValue("@mobileno", txtMobileNumber.Text);
                        cmd.Parameters.AddWithValue("@emailidaddress", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@productID", "22");
                        cmd.Parameters.AddWithValue("@ProductSubCatgID", "222");
                        cmd.Parameters.AddWithValue("@brand", "Bluestar");
                        cmd.Parameters.AddWithValue("@serialno", "98419812002132165149");
                        cmd.Parameters.AddWithValue("@model", "Bluestar");
                        cmd.Parameters.AddWithValue("@purchasefrom_productdetails", "");
                        cmd.Parameters.AddWithValue("@invoiceno_productdetails", "");
                        cmd.Parameters.AddWithValue("@invoicedate_productdetails", DateTime.Now.ToString("MM/dd/yyyy"));
                        cmd.Parameters.AddWithValue("@invoiceamount_productdetails", "60000");
                        cmd.Parameters.AddWithValue("@manufacturewarrantystartdate", DateTime.Now.ToString("MM/dd/yyyy"));
                        cmd.Parameters.AddWithValue("@manufacturewarrantyenddate", DateTime.Now.AddYears(1).AddDays(-1).ToString("MM/dd/yyyy"));
                        cmd.Parameters.AddWithValue("@extendedwarrantystartdate", DateTime.Now.ToString("MM/dd/yyyy"));
                        cmd.Parameters.AddWithValue("@extendedwarrantyenddate", DateTime.Now.AddYears(1).AddDays(-1).ToString("MM/dd/yyyy"));
                        cmd.Parameters.AddWithValue("@adpstartdate", DateTime.Now.ToString("MM/dd/yyyy"));
                        cmd.Parameters.AddWithValue("@adpenddate", DateTime.Now.AddYears(1).AddDays(-1).ToString("MM/dd/yyyy"));
                        cmd.Parameters.AddWithValue("@Status", "Under Approval");
                        cmd.Parameters.AddWithValue("@ClientID", "1479");
                        cmd.Parameters.AddWithValue("@warrnatyYear", "1");
                        cmd.Parameters.AddWithValue("@WarrantyMonth", "0");
                        cmd.Parameters.AddWithValue("@WarrantyDay", "0");
                        cmd.Parameters.AddWithValue("@ProSubcatID", "46");
                        cmd.Parameters.AddWithValue("@ContactPerson", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@Make", "Bluestar");
                        cmd.Parameters.AddWithValue("@ProductPincode", "0");
                        cmd.Parameters.AddWithValue("@BalSumAssured", "0.00");
                        cmd.Parameters.AddWithValue("@SumAssured", "60000.00");
                        cmd.Parameters.AddWithValue("@IsDelete", "0");
                        cmd.Parameters.AddWithValue("@ProjectId", "65");
                        
                        //  Service Calls Table
                        cmd.Parameters.AddWithValue("@ProblemNo", "1");
                        cmd.Parameters.AddWithValue("@CallSource", "19");
                        cmd.Parameters.AddWithValue("@CallPriority", "1");
                        cmd.Parameters.AddWithValue("@CallTypes", "18");
                        cmd.Parameters.AddWithValue("@ServiceType", "10");
                        cmd.Parameters.AddWithValue("@Symptoms", "0");
                        cmd.Parameters.AddWithValue("@ProblemReported", "Partner Registration Inquiry");
                        cmd.Parameters.AddWithValue("@Charges", "0");
                        cmd.Parameters.AddWithValue("@InfinityRemarks", "Partner Registration Inquiry");
                        cmd.Parameters.AddWithValue("@CallAction", "201");
                        cmd.Parameters.AddWithValue("@CallStatus", "19");
                        cmd.Parameters.AddWithValue("@ClaimReportedDate", DateTime.Now.ToString("MM/dd/yyyy"));
                        cmd.Parameters.AddWithValue("@ClaimReportedTime", DateTime.Now.ToString("hh:mm tt"));
                        cmd.Parameters.AddWithValue("@DeviceSwitchingOn", "2");
                        cmd.Parameters.AddWithValue("@SurverMailStatus", "No");
                        cmd.Parameters.AddWithValue("@lastupdateDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CallactionDate", DateTime.Now.ToString("MM/dd/yyyy"));
                        cmd.Parameters.AddWithValue("@CallActionTime", DateTime.Now.ToString("hh:mm tt"));
                        cmd.Parameters.AddWithValue("@NextActionDate", DateTime.Now.ToString("MM/dd/yyyy"));
                        cmd.Parameters.AddWithValue("@NextActionTime", DateTime.Now.ToString("hh:mm tt"));
                        cmd.Parameters.AddWithValue("@lastupdatedby", "System Generated / Updated by System");
                        cmd.Parameters.AddWithValue("@claimstatus", "Open");
                        cmd.Parameters.AddWithValue("@UserRole", "Admin");

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            string resultMessage = dt.Rows[0]["ResultMessage"]?.ToString() ?? string.Empty;
                            if (resultMessage.Contains("already available"))
                            {
                                lblMessage.Text = "Retailer is already registered with this mobile number.";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                return;
                            }
                            else
                            {
                                lblMessage.Text = "Registration successful.";
                                lblMessage.ForeColor = System.Drawing.Color.Green;
                            }
                        }
                    }
                
            }
            catch (Exception)
            {
            }
        }

        protected void UploadDocumentTicket(int insertedMid)
        {
            try
            {
                if (ViewState["DocumentData"] != null)
                {
                    DataTable dt = (DataTable)ViewState["DocumentData"];

                    foreach (DataRow row in dt.Rows)
                    {
                        string docName = row["DocumentName"].ToString();
                        string DocumentPath = row["DocumentPath"].ToString();
                        string DocId = row["DocId"].ToString();
                        
                        UploadImage1(docName, DocId, DocumentPath, insertedMid);
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        protected void UploadImage1(string docName, string docId, string tempFileName, int insertedMid)
        {
            try
            {
                string basePath = ConfigurationManager.AppSettings["FilePath3"];
                string ticketNo = GetTicketno(txtMobileNumber.Text != null ? txtMobileNumber.Text.ToString() : "");
                string yy = DateTime.Now.Year.ToString();
                string mn = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);

                string targetFolder = Path.Combine(basePath, "InfyShield", yy, mn);
                if (!Directory.Exists(targetFolder))
                    Directory.CreateDirectory(targetFolder);

                string fileExtension = Path.GetExtension(tempFileName);
                string sanitizedFileName = SanitizeFileName(docName).Replace(" ", "_");
                string fn = ticketNo.Replace("/", "") + "InfyShield" + sanitizedFileName + fileExtension;
                string sourcePath = Server.MapPath("~/UploadedDocuments/") + tempFileName;
                string destPath = Path.Combine(targetFolder, fn);
                if (System.IO.File.Exists(sourcePath))
                {
                    System.IO.File.Copy(sourcePath, destPath, true);
                }
                UploadDocuemt("0", docId, "InfyShield/" + yy + "/" + mn + "/" + fn, insertedMid);
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
        protected void UploadDocuemt(string mid, string documentNumber, string documentPath, int insertedMid)
        {
            try
            {
                string ticketNo = GetTicketno(txtMobileNumber.Text != null ? txtMobileNumber.Text.ToString() : "");
                if (!string.IsNullOrWhiteSpace(ticketNo))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@type", 94);
                        cmd.Parameters.AddWithValue("@Mid", mid);
                        cmd.Parameters.AddWithValue("@ticketno", ticketNo);
                        cmd.Parameters.AddWithValue("@documentNumber", documentNumber);
                        cmd.Parameters.AddWithValue("@DocumentPath", documentPath);
                        cmd.Parameters.AddWithValue("@CreatedBy", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@insertedMid", insertedMid);
                        cmd.Parameters.AddWithValue("@UserRole", "Admin");

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
                return;
            }
        }

        protected string GetTicketno(string mobileNo)
        {
            string ticketNo = string.Empty;
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 92);
                cmd.Parameters.AddWithValue("@mobileno", mobileNo);

                if (con.State != ConnectionState.Open)
                    con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ticketNo = reader["TicketNO"] != DBNull.Value ? reader["TicketNO"].ToString() : string.Empty;
                    }
                }
                con.Close();
            }
            return ticketNo;
        }
        protected void UpdateRetailerSKU()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@type", 93);
                    cmd.Parameters.AddWithValue("@mobileno", txtMobileNumber.Text != null ? txtMobileNumber.Text.ToString() : "");

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch 
            {
                return;
            }
        }


    }
}