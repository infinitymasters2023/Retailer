using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Patner_Retailer_ADO
{
    public partial class CreateSalesPerson : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iaplConnectionString"].ConnectionString);
        static public void DisplayMessage(Control page, string msg)
        {
            string msg1 = String.Format("alert('{0}');", msg);
            ScriptManager.RegisterStartupScript(page, page.GetType(), "msg", msg1, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

                mvViewType.ActiveViewIndex = 0;
                btnPersonalInformationView.Attributes["class"] = "btn btn-primary card-btn";
                btnBandDetailsView.Attributes["class"] = "btn btn-outline-primary card-btn";
                btnUploadedDocumentListView.Attributes["class"] = "btn btn-outline-primary card-btn";
                btnCommisionDetailsView.Attributes["class"] = "btn btn-outline-primary card-btn";
                btnAccountHistoryView.Attributes["class"] = "btn btn-outline-primary card-btn";
                BindBankSupportingDocument();
                BindDocument();
                DocumentPanel.Visible = true;
                btnUpdate.Visible = false;
                btnnext3.Visible = false;
                txtDOB.Attributes.Add("Readonly", "readonly");
                CalendarExtender3.EndDate = DateTime.Today.AddYears(-19);
                string querymid = Request.QueryString["Mid"];
                if (!string.IsNullOrWhiteSpace(querymid))
                {
                    btnSubmit.Visible = false;
                    BindSalesPersonInfo(querymid);
                    btnUpdate.Visible = true;
                    Session["UpdateSalesPersonMid"] = querymid;
                }
            }
            if (IsPostBack)
            {
                txtAccount.Attributes["value"] = hdnAccount.Value;
                txtConfirmAccount.Attributes["value"] = hdnConfirmAccount.Value;
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
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                bool checkcusotmer = CheckBlockCustomer(txtEmail.Text, txtMobile.Text);
                bool checkSalesPerson = CheckSalesPersonOrRetailer(txtEmail.Text, txtMobile.Text);
                bool checkAltcusotmer = CheckBlockCustomer(txtAltEmail.Text, txtAltMobile.Text);
                bool checkAltSalesPerson = CheckSalesPersonOrRetailer(txtAltEmail.Text, txtAltMobile.Text);
                if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    lblFirstNameError.Text = "Full Name is required";
                    lblFirstNameError.Attributes.Add("style", "display:block");
                    txtFirstName.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtFirstName.Text) || txtFirstName.Text.Trim().Length < 3)
                {
                    lblFirstNameError.Text = "Full Name must contain at least 3 words.";
                    lblFirstNameError.Attributes.Add("style", "display:block");
                    txtFirstName.Focus();
                    return;
                }
                else
                {
                    lblFirstNameError.Text = "";
                    lblFirstNameError.Attributes.Add("style", "display:none");
                }
                if (!string.IsNullOrWhiteSpace(txtAltMobile.Text) && Regex.IsMatch(txtAltMobile.Text, @"^[0-5]"))
                {
                    BlockAltCustomerMobileErrorMessage.Text = "Invalid number";
                    txtAltMobile.Focus();
                    return;
                }
                if (!string.IsNullOrWhiteSpace(txtAltMobile.Text) && Regex.IsMatch(txtAltMobile.Text, @"^(\d)\1{9}$"))
                {
                    BlockAltCustomerMobileErrorMessage.Text = "Invalid number.";
                    txtAltMobile.Focus();
                    return;
                }
                if (!string.IsNullOrWhiteSpace(txtMobile.Text) && Regex.IsMatch(txtMobile.Text, @"^[0-5]"))
                {
                    BlockCustomerMobileErrorMessage.Text = "Invalid number";
                    txtMobile.Focus();
                    return;
                }

                if (Regex.IsMatch(txtMobile.Text, @"^(\d)\1{9}$"))
                {
                    BlockCustomerMobileErrorMessage.Text = "Invalid number.";
                    txtMobile.Focus();
                    return;
                }
                string mobile = txtMobile.Text.Trim();
                string altMobile = txtAltMobile.Text.Trim();

                if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
                {
                    BlockCustomerMobileErrorMessage.Text = "Mobile No and alternate mobile numbers cannot be the same.";
                    btnSubmit.Enabled = false;
                    btnUpdate.Enabled = false;
                    txtAltMobile.Focus();
                    return;
                }
                if (!string.IsNullOrWhiteSpace(txtAltEmail.Text) && !Regex.IsMatch(txtAltEmail.Text.Trim(), @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
                {
                    BlockAltCustomerEmailErrorMessage.Text = "Invalid email format.";
                    txtAltEmail.Focus();
                    return;
                }
                if (!Regex.IsMatch(txtEmail.Text.Trim(), @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
                {
                    BlockCustomerEmailErrorMessage.Text = "Invalid email format.";
                    txtEmail.Focus();
                    return;
                }
                string email = txtEmail.Text.Trim();
                string altemail = txtAltEmail.Text.Trim();

                if (!string.IsNullOrEmpty(email) && email == altemail)
                {
                    BlockCustomerEmailErrorMessage.Text = "Email ID and alternate Email ID cannot be the same.";
                    btnSubmit.Enabled = false;
                    btnUpdate.Enabled = false;
                    txtAltEmail.Focus();
                    return;
                }
                if (checkcusotmer || checkSalesPerson || checkAltcusotmer || checkAltSalesPerson)
                {
                    return;
                }
                if(string.IsNullOrWhiteSpace(txtPIN.Text))
                {
                    lblPinCodeError.Text = "PIN Code is required";
                    lblPinCodeError.Visible = true;
                    txtPIN.Focus();
                    return;
                }
                else if(txtPIN.Text.Length != 6)
                {
                    lblPinCodeError.Text = "Invalid PIN Code";
                    lblPinCodeError.Visible  = true;
                    txtPIN.Focus();
                    return;
                }
                else
                {
                    lblPinCodeError.Text = "";
                    lblPinCodeError.Visible = false;
                }
                if (string.IsNullOrWhiteSpace(txtAccount.Text))
                {
                    lblAccountError.Text = "Account Number is required";
                    lblAccountError.Attributes.Add("style", "display:block");
                    txtAccount.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtConfirmAccount.Text))
                {
                    lblConfirmAccountError.Text = "Confirm Account Number is required";
                    lblConfirmAccountError.Attributes.Add("style", "display:block");
                    txtConfirmAccount.Focus();
                    return;
                }
                else if (txtConfirmAccount.Text != txtAccount.Text)
                {
                    lblConfirmAccountError.Text = "Account numbers do not match";
                    lblConfirmAccountError.Attributes.Add("style", "display:block");
                    txtConfirmAccount.Focus();
                    return;
                }
                else
                {
                    lblConfirmAccountError.Text = "";
                    lblConfirmAccountError.Attributes.Add("style", "display:none");
                }
                bool validatedAccount = AccountNumberValidation(txtAccount.Text, "0");
                if (!string.IsNullOrWhiteSpace(txtAccount.Text) && validatedAccount)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtIFSC.Text))
                {
                    lblIFSCError.Text = "IFSC Code is required";
                    lblIFSCError.Attributes.Add("style", "display:block");
                    txtIFSC.Focus();
                    return;
                }
                else
                {
                    lblIFSCError.Text = "";
                    lblIFSCError.Attributes.Add("style", "display:none");
                }
                if (string.IsNullOrWhiteSpace(txtHolder.Text))
                {
                    lblAccountHolderNameError.Text = "Account Holder Name is required";
                    lblAccountHolderNameError.Attributes.Add("style", "display:block");
                    txtHolder.Focus();
                    return;
                }
                else
                {
                    lblAccountHolderNameError.Text = "";
                    lblAccountHolderNameError.Attributes.Add("style", "display:none");
                }
                if (ddlSuppotingDoc.SelectedIndex == -1)
                {
                    lblsupportingDocError.Text = "Supporting Document is required";
                    lblsupportingDocError.Attributes.Add("style", "display:block");
                    ddlSuppotingDoc.Focus();
                    return;
                }
                else
                {
                    lblsupportingDocError.Text = "";
                    lblsupportingDocError.Attributes.Add("style", "display:none");
                }
                if (!fuSuppotingDoc.HasFile)
                {
                    lblSupportingDocumentError.Text = "Supporting Document is required";
                    lblSupportingDocumentError.Attributes.Add("style", "display:block");
                    fuSuppotingDoc.Focus();
                    return;
                }
                else
                {

                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
                    string fileExtension = Path.GetExtension(fuSuppotingDoc.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        lblSupportingDocumentError.Text = "Please upload only jpg, jpeg, png and pdf format.";
                        lblSupportingDocumentError.Attributes.Add("style", "display:block");
                        return;
                    }
                    lblSupportingDocumentError.Attributes.Add("style", "display:none");
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

                    Session["FileName"] = uniqueFileName;

                    lblSupportingDocumentError.Text = "";
                    lblSupportingDocumentError.Attributes.Add("style", "display:none");
                }


                DocumentPanel.Visible = true;
                //btnnext3.Visible = true;
                ddlDocumentName.Focus();
                btnSubmit.Visible = false;
                btnUploadedDocumentListView_Click(sender, e);
                //Response.Redirect("ViewSalesPerson.aspx", false);
            }
            catch (SqlException ex)
            {
                //  Check the error number to specifically handle the duplicate mobile number error.
                if (ex.Number == 50001) //  50001 is the error number we threw in the SP.
                {
                    //  Display a user-friendly message.  DO NOT expose the raw SQL error message to the user.
                    //lblError.Text = "Error: A record with this Mobile Number already exists. Please enter a different Mobile Number.";
                    //    lblError.Visible = true; // Make sure the label is visible.
                }
                else
                {
                    //  Handle other SQL Server errors (log them, show a generic error message, etc.).
                    LogError(ex); //  Call a method to log the error.
                                  //  lblError.Text = "An unexpected database error occurred. Please contact support.";
                                  //   lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                //  Handle general exceptions (e.g., connection errors, other unexpected issues).
                LogError(ex);
                //   lblError.Text = "An unexpected error occurred. Please contact support.";
                // lblError.Visible = true;
            }
            finally
            {
                //  Ensure the connection is closed, even if an exception occurs.
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        protected void SaveSalesPerson()
        {
            try
            {

                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 7);
                cmd.Parameters.AddWithValue("@RetailerAdminID", Session["RetailerUniqueID"].ToString().Trim());
                cmd.Parameters.AddWithValue("@Name", txtFirstName.Text.Trim());
                cmd.Parameters.AddWithValue("@MobileNo", txtMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@MobileNo_2", txtAltMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@EmailID", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@EmailID_2", txtAltEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", ddlGender.Text.Trim());
                cmd.Parameters.AddWithValue("@DateOfBirth", txtDOB.Text.Trim());
                cmd.Parameters.AddWithValue("@PinCode", txtPIN.Text.Trim());
                cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                cmd.Parameters.AddWithValue("@Address", txtCommAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@BankAccountNumber", !string.IsNullOrWhiteSpace(txtAccount.Text) ? txtAccount.Text.Trim() : hdnAccount.Value);
                cmd.Parameters.AddWithValue("@IFSCCode", txtIFSC.Text.ToUpper().Trim());
                cmd.Parameters.AddWithValue("@AccountHolderName", txtHolder.Text.Trim());
                cmd.Parameters.AddWithValue("@BankName", txtBankName.Text.Trim());
                cmd.Parameters.AddWithValue("@BankBranch", txtBranch.Text.Trim());
                cmd.Parameters.AddWithValue("@BankBranchAddress", txtBranchAddress.Text.Trim());

                cmd.Parameters.AddWithValue("@MobileNo_CheckWhatsapp", chkMobileNumberWhatsApp.Checked ? txtMobile.Text.Trim() : null);
                cmd.Parameters.AddWithValue("@MobileNo2_CheckWhatsapp", chkAltMobileNumberWhatsApp.Checked ? txtAltMobile.Text.Trim() : null);
                cmd.Parameters.AddWithValue("@UPIID", txtUPIID.Text.ToString());
                cmd.Parameters.AddWithValue("@TypeofBankAccount", ddlTypeOfBank.SelectedValue == "" ? null : ddlTypeOfBank.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@IsThisYourJointAccount", chkJointAccount.SelectedValue == "" ? null : chkJointAccount.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@JointAccountHolderName", txtJointHolderName.Text.ToString());
                cmd.Parameters.AddWithValue("@SupportingDocuments", ddlSuppotingDoc.SelectedValue == "" ? null : ddlSuppotingDoc.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@SupportingDocumentsPath", Session["FileName"] != null ? Session["FileName"].ToString() : null);

                con.Open();
                //int i = cmd.ExecuteNonQuery();
                SqlDataReader reader = cmd.ExecuteReader();
                int insertedProfileId = 0;

                if (reader.Read())
                {
                    insertedProfileId = Convert.ToInt32(reader["InsertedProfileId"]);
                }

                reader.Close();
                con.Close();
                Session["InsertedProfileId"] = insertedProfileId;
            }
            catch (SqlException sqlEx)
            {
                DisplayMessage(this, sqlEx.Message);
            }
        }
        private void LogError(Exception ex)
        {
            //  Implement robust error logging here.  
            //  Consider logging to a file, the Windows Event Log, a database table, or a logging framework (like log4net).
            //  Example (basic logging to a file):
            string filePath = Server.MapPath("~/logs/ErrorLog.txt"); //  Use Server.MapPath for file paths in web applications.
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Message);
                    sw.WriteLine("Stack Trace: " + ex.StackTrace);
                    sw.WriteLine("--------------------------------------------------");
                }
            }
            catch (Exception logEx)
            {
                //  Handle any errors that occur during logging.
                //  You might not be able to log to the original destination, so consider an alternative (e.g., writing to the Application Event Log).
                System.Diagnostics.EventLog.WriteEntry("Application", "Error logging failed: " + logEx.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        protected void chkJointAccount_Change(object sender, EventArgs e)
        {
            if (chkJointAccount.SelectedValue == "Yes")
            {
                jointAcountHolderpnl.Visible = true;
                txtJointHolderName.Focus();
            }
            else
            {
                jointAcountHolderpnl.Visible = false;
                ddlSuppotingDoc.Focus();
            }
            //if (!string.IsNullOrWhiteSpace(txtAccountNumber.Text))
            //{
            //    lblAccountNumber.Style["display"] = "none";
            //}
            //if (!string.IsNullOrWhiteSpace(txtConfirmAccountNumber.Text))
            //{
            //    lblConfirmAccountNumber.Style["display"] = "none";
            //}
            //if (!string.IsNullOrWhiteSpace(txtAccountHolderName.Text))
            //{
            //    lblAccountHoldername.Style["display"] = "none";
            //}
            if (ddlSuppotingDoc.SelectedItem.Value != "")
            {
                lblsupportingDocError.Style["display"] = "none";
            }
            if (!fuSuppotingDoc.HasFile)
            {
                lblSupportingDocumentError.Style["display"] = "none";
            }
        }
        protected void ddlTypeOfBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTypeOfBank.SelectedValue == "Savings")
            {
                jointAcountpnl.Visible = true;
                chkJointAccount.Focus();
            }
            else
            {
                jointAcountpnl.Visible = false;
                jointAcountHolderpnl.Visible = false;
                ddlTypeOfBank.Focus();
            }
            //if (!string.IsNullOrWhiteSpace(txtAccountNumber.Text))
            //{
            //    lblAccountNumber.Style["display"] = "none";
            //}
            //if (!string.IsNullOrWhiteSpace(txtConfirmAccountNumber.Text))
            //{
            //    lblConfirmAccountNumber.Style["display"] = "none";
            //}
            //if (!string.IsNullOrWhiteSpace(txtAccountHolderName.Text))
            //{
            //    lblAccountHoldername.Style["display"] = "none";
            //}
            if (ddlSuppotingDoc.SelectedItem.Value != "")
            {
                lblsupportingDocError.Style["display"] = "none";
            }
            if (!fuSuppotingDoc.HasFile)
            {
                lblSupportingDocumentError.Style["display"] = "none";
            }
        }
        protected void txtIFSC_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("sp_iapl_crm_newsrvcall", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 11;
            cmd.Parameters.AddWithValue("@IFSC", SqlDbType.NVarChar).Value = txtIFSC.Text;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {

                txtBankName.Text = dt.Rows[0]["BANK"].ToString();
                txtBranch.Text = dt.Rows[0]["BRANCH"].ToString();
                txtBranchAddress.Text = dt.Rows[0]["ADDRESS"].ToString();
                txtHolder.Focus();
                lblIFSCError.Text = "";
            }

            else
            {
                txtBankName.Text = "";
                txtBranch.Text = "";
                txtBranchAddress.Text = "";
                lblIFSCError.Text = "Enter a valid IFSC code";
                txtIFSC.Focus();
            }
        }

        protected void txtPIN_TextChanged(object sender, EventArgs e)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pincode", SqlDbType.Int).Value = txtPIN.Text.Trim();
                cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 4;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    txtCity.Text = dt.Rows[0]["CityName"].ToString();
                    txtState.Text = dt.Rows[0]["statename"].ToString();
                    txtCity.Focus();
                    txtCity.Enabled = false;
                    txtState.Enabled = false;
                    lblPinCodeError.Text = "";
                    lblPinCodeError.Visible = false;
                }
                else
                {
                    lblPinCodeError.Text = "The entered pincode does not exist. Please verify and try again.";
                    lblPinCodeError.Visible = true;
                }
            }
            catch (Exception ex)
            {

                return;
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

        protected void btnUploadFront_Click(object sender, EventArgs e)
        {
            if (fuFrontSide.HasFile)
            {
                try
                {
                    if (ddlDocumentName.SelectedValue == "13" || ddlDocumentName.SelectedValue == "57" || ddlDocumentName.SelectedValue == "58")
                    {
                        string cleanedDocNumber = txtDocumentNumber.Text.Replace("-", "");
                        if (!string.IsNullOrWhiteSpace(txtDocumentNumber.Text) && !System.Text.RegularExpressions.Regex.IsMatch(cleanedDocNumber, @"^\d{4}$"))
                        {
                            lblDocFormatError.Text = "Invalid Aadhaar number.";
                            lblDocFormatError.CssClass = "text-danger";
                            lblDocFormatError.Attributes.Add("style", "display:block");
                            lblDocFormatError.Visible = true;
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
                            lblDocFormatError.Visible = true;
                            return;
                        }
                    }
                    lblDocFormatError.Attributes.Add("style", "display:none");

                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
                    string fileExtension = Path.GetExtension(fuFrontSide.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "scrollToBottom", "window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' });", true);
                        return;
                    }
                   
                    string docName = ddlDocumentName.SelectedItem.Text;
                    string docNumber = txtDocumentNumber.Text.Replace("-", "").Trim();
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
                        dt.Columns.Add("Mid");
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

                    var existingDoc = dt.AsEnumerable().FirstOrDefault(row => row.Field<string>("DocumentName") == docName);

                    //bool isDuplicate = dt.AsEnumerable().Any(row => row.Field<string>("DocumentName") == docName);
                    if (existingDoc != null)
                    {
                        string status = existingDoc.Field<string>("ActionStatus");
                        if (status == "Approved")
                        {
                            DisplayMessage(this, "Document already exists.");
                            return;
                        }
                    }

                    //DataRow existingRow = dt.AsEnumerable().FirstOrDefault(row => row["DocId"].ToString() == DocId);
                    //if (existingRow != null)
                    //{
                    //    existingRow["DocumentName"] = docName;
                    //    existingRow["DocumentNumber"] = docNumber;
                    //    existingRow["DocumentPath"] = uniqueFileName;
                    //    existingRow["Status"] = "Updated";
                    //    existingRow["Size"] = (fuFrontSide.PostedFile.ContentLength / 1024.0).ToString("0.00") + " KB";
                    //}
                    //else
                    //{
                    // Add new row
                    DataRow dr = dt.NewRow();
                        dr["SrNo"] = dt.Rows.Count + 1;
                        dr["Mid"] = 0;
                        dr["DocId"] = DocId;
                        dr["DocumentName"] = docName;
                        dr["DocumentNumber"] = docNumber;
                        dr["DocumentPath"] = uniqueFileName;
                        dr["Status"] = "Uploaded";
                        dr["Size"] = (fuFrontSide.PostedFile.ContentLength / 1024.0).ToString("0.00") + " KB";
                        dt.Rows.Add(dr);
                    //}
                    ViewState["DocumentData"] = dt;
                    gvDocuments.DataSource = dt;
                    gvDocuments.DataBind();
                    lblDocument.Visible = false;
                    ddlDocumentName.SelectedIndex = 0;
                    txtDocumentNumber.Text = null;
                    hdnDocumentNumber.Value = "";
                    txtDocumentNumber.Text = "";
                    CheckIfBothDocumentsUploaded();
                    var itemToRemove = ddlDocumentName.Items.FindByValue(DocId);
                    if (itemToRemove != null)
                    {
                        ddlDocumentName.Items.Remove(itemToRemove);
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "scrollToBottom", "window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' });", true);
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                lblDocument.Visible = true;
                ddlDocumentName.Focus();
            }
        }
        protected void btnnext3_Click(object sender, EventArgs e)
        {
            if (ViewState["DocumentData"] != null)
            {
                if (Session["UpdateSalesPersonMid"] == null)
                {
                    SaveSalesPerson();
                }
                DataTable dt = (DataTable)ViewState["DocumentData"];

                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        string docName = row["DocumentName"].ToString();
                        string docNumber = row["DocumentNumber"].ToString();
                        string status = row["Status"].ToString();
                        string size = row["Size"].ToString();
                        string DocumentPath = row["DocumentPath"].ToString();
                        string DocId = row["DocId"].ToString();
                        string MId = row["Mid"].ToString();

                        SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Type", 14);
                        cmd.Parameters.AddWithValue("@ProfileId", Session["InsertedProfileId"] != null ? Session["InsertedProfileId"].ToString().Trim() : Session["UpdateSalesPersonMid"].ToString());
                        cmd.Parameters.AddWithValue("@DocID", DocId);
                        cmd.Parameters.AddWithValue("@Mid", MId);
                        cmd.Parameters.AddWithValue("@DocumentPath", DocumentPath);
                        cmd.Parameters.AddWithValue("@documentNumber", docNumber);
                        cmd.Parameters.AddWithValue("@Remarks", docNumber);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@IPAddress", Request.UserHostAddress);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
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
                Response.Redirect("ViewSalesPerson.aspx");
            }
            else
            {
                lblRequiredDocuments.Text = "Note* Submission of both Aadhaar and PAN card details is required for the creation of a Salesperson account.";
                lblDocument.Visible = true;
                ddlDocumentName.Focus();
            }
        }

        protected void BindSalesPersonInfo(string querymid)
        {
            SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", 12);
            cmd.Parameters.AddWithValue("@Mid", querymid);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txtFirstName.Text = dr["Name"].ToString();
                txtMobile.Text = dr["MobileNo"].ToString();
                txtAltMobile.Text = dr["MobileNo_2"].ToString();
                txtEmail.Text = dr["EmailID"].ToString();
                txtAltEmail.Text = dr["EmailID_2"].ToString();
                txtDOB.Text = Convert.ToDateTime(dr["DateOfBirth"]).ToString("dd-MMM-yyyy");// dr["DateOfBirth"].ToString();
                ddlGender.SelectedValue = dr["Gender"].ToString();
                txtPIN.Text = dr["Pincode"].ToString();
                txtCity.Text = dr["City"].ToString();
                txtState.Text = dr["State"].ToString();
                txtCommAddress.Text = dr["Address"].ToString();
                txtAccount.Attributes["value"] = dr["BankAccountNumber"].ToString();
                txtConfirmAccount.Attributes["value"] = dr["BankAccountNumber"].ToString();
                txtIFSC.Text = dr["IFSCCode"].ToString();
                txtHolder.Text = dr["AccountHolderName"].ToString();
                txtBankName.Text = dr["BankName"].ToString();
                txtBranch.Text = dr["BankBranch"].ToString();
                txtBranchAddress.Text = dr["BankBranchAddress"].ToString();
                txtUPIID.Text = dr["UPIID"] != null ? dr["UPIID"].ToString() : "";
                ddlTypeOfBank.SelectedValue = dr["TypeofBankAccount"] != null ? dr["TypeofBankAccount"].ToString() : "";
                if(ddlTypeOfBank.SelectedValue == "Savings") 
                {
                    jointAcountpnl.Visible = true;
                }
                chkJointAccount.SelectedValue = dr["IsThisYourJointAccount"] != null ? dr["IsThisYourJointAccount"].ToString() : "";
                if (chkJointAccount.SelectedValue == "Yes")
                {
                    jointAcountHolderpnl.Visible = true;
                }
                txtJointHolderName.Text = dr["JointAccountHolderName"] != null ? dr["JointAccountHolderName"].ToString() : "";
                ddlSuppotingDoc.SelectedValue = dr["SupportingDocuments"] != null ? dr["SupportingDocuments"].ToString() : "";
                lblsupportingDocName.Text = dr["SupportingDocumentsPath"] != null ? dr["SupportingDocumentsPath"].ToString() : "";
            }
            dr.Close();            
            con.Close();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string uniqueFileName = "";
                if (!fuSuppotingDoc.HasFile)
                {
                    uniqueFileName = lblsupportingDocName.Text;
                }
                else
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

                    uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName;
                    string fullPath = Path.Combine(folderPath, uniqueFileName);
                    fuSuppotingDoc.SaveAs(fullPath);
                }

                bool checkcusotmer = CheckBlockCustomer(txtEmail.Text, txtMobile.Text);
                bool checkSalesPerson = CheckSalesPersonOrRetailer(txtEmail.Text, txtMobile.Text);
                bool checkAltcusotmer = CheckBlockCustomer(txtAltEmail.Text, txtAltMobile.Text);
                bool checkAltSalesPerson = CheckSalesPersonOrRetailer(txtAltEmail.Text, txtAltMobile.Text);
                if (checkcusotmer || checkSalesPerson || checkAltcusotmer || checkAltSalesPerson)
                {
                    return;
                }
                string mid = Session["UpdateSalesPersonMid"] != null ? Session["UpdateSalesPersonMid"].ToString() : "";
                bool validatedAccount = AccountNumberValidation(txtAccount.Text, mid);
                if (!string.IsNullOrWhiteSpace(txtAccount.Text) && validatedAccount)
                {
                    return;
                }
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 13);
                cmd.Parameters.AddWithValue("@Mid", Session["UpdateSalesPersonMid"].ToString().Trim());
                cmd.Parameters.AddWithValue("@Name", txtFirstName.Text.Trim());
                cmd.Parameters.AddWithValue("@MobileNo", txtMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@MobileNo_2", txtAltMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@EmailID", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@EmailID_2", txtAltEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", ddlGender.Text.Trim());
                cmd.Parameters.AddWithValue("@DateOfBirth", txtDOB.Text.Trim());
                cmd.Parameters.AddWithValue("@PinCode", txtPIN.Text.Trim());
                cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                cmd.Parameters.AddWithValue("@Address", txtCommAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@BankAccountNumber", !string.IsNullOrWhiteSpace(txtAccount.Text) ? txtAccount.Text.Trim() : hdnAccount.Value);
                cmd.Parameters.AddWithValue("@IFSCCode", txtIFSC.Text.ToUpper().Trim());
                cmd.Parameters.AddWithValue("@AccountHolderName", txtHolder.Text.Trim());
                cmd.Parameters.AddWithValue("@BankName", txtBankName.Text.Trim());
                cmd.Parameters.AddWithValue("@BankBranch", txtBranch.Text.Trim());
                cmd.Parameters.AddWithValue("@BankBranchAddress", txtBranchAddress.Text.Trim());

                cmd.Parameters.AddWithValue("@MobileNo_CheckWhatsapp", chkMobileNumberWhatsApp.Checked ? txtMobile.Text.Trim() : null);
                cmd.Parameters.AddWithValue("@MobileNo2_CheckWhatsapp", chkAltMobileNumberWhatsApp.Checked ? txtAltMobile.Text.Trim() : null);
                cmd.Parameters.AddWithValue("@UPIID", txtUPIID.Text.ToString());
                cmd.Parameters.AddWithValue("@TypeofBankAccount", ddlTypeOfBank.SelectedValue == "" ? null : ddlTypeOfBank.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@IsThisYourJointAccount", chkJointAccount.SelectedValue == "" ? null : chkJointAccount.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@JointAccountHolderName", txtJointHolderName.Text.ToString());
                cmd.Parameters.AddWithValue("@SupportingDocuments", ddlSuppotingDoc.SelectedValue == "" ? null : ddlSuppotingDoc.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@SupportingDocumentsPath", uniqueFileName);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();


                DocumentPanel.Visible = true;
                btnnext3.Visible = true;
                BindDocumentInf();
                ddlDocumentName.Focus();
                btnUploadedDocumentListView_Click(sender, e);
            }
            catch (Exception ex) 
            {
            }
        }

        protected void gvDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditDoc")
            {
                int docId = Convert.ToInt32(e.CommandArgument);
                LoadDocumentForEdit(docId);
            }
            else if (e.CommandName == "ViewDoc")
            {
                string[] args = e.CommandArgument.ToString().Split('|');
                string docId = args[0];
                string documentName = args[1];
                string documentNumber = args[2];
                string documentPath = args[3];

                ddlDocumentName.ClearSelection();
                ListItem item = ddlDocumentName.Items.FindByText(documentName);
                if (item != null) item.Selected = true;
                txtDocumentNumber.Text = documentNumber;

                string fileExt = Path.GetExtension(documentPath).ToLower();
                string relativePath = "~/UploadedDocuments/" + documentPath;

                if (fileExt == ".jpg" || fileExt == ".jpeg" || fileExt == ".png")
                {
                    imgPreview.ImageUrl = relativePath;
                    imgPreview.Visible = true;
                    litPdfPreview.Visible = false;
                }
                else if (fileExt == ".pdf")
                {
                    litPdfPreview.Text = $"<iframe src='{ResolveUrl(relativePath)}' width='100%' height='400px'></iframe>";
                    litPdfPreview.Visible = true;
                    imgPreview.Visible = false;
                }
            }
            else if (e.CommandName == "DeleteRow")
            {
                hdnDocumentNumber.Value = "";
                txtDocumentNumber.Text = "";
                //string docId = e.CommandArgument.ToString();
                //DeleteDocument(docId);
                DataTable dt = (DataTable)ViewState["DocumentData"];
                string mid = e.CommandArgument.ToString();
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                string docId = gvDocuments.DataKeys[row.RowIndex]["DocId"].ToString();
                string documentName = row.Cells[2].Text;
                string quer = Request.QueryString["Mid"];
                if (!string.IsNullOrWhiteSpace(quer))
                {
                    DeleteDocument(mid);
                }
                else
                {
                    string filePath = dt.Rows[index]["DocumentPath"].ToString();
                    string fullPath = Server.MapPath("~/UploadedDocuments/" + filePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    dt.Rows.RemoveAt(index);
                    for (int i = 0; i < dt.Rows.Count; i++)
                        dt.Rows[i]["SrNo"] = i + 1;

                    ViewState["DocumentData"] = dt;
                    gvDocuments.DataSource = dt;
                    gvDocuments.DataBind();
                }
                if (!string.IsNullOrEmpty(docId) && !string.IsNullOrEmpty(documentName))
                {
                    ListItem item = new ListItem(documentName, docId);
                    if (ddlDocumentName.Items.FindByValue(docId) == null)
                    {
                        ddlDocumentName.Items.Add(item);
                    }
                }
                CheckIfBothDocumentsUploaded();
            }
        }

        protected void BindDocumentInf()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 15);
                cmd.Parameters.AddWithValue("@ProfileId", Session["UpdateSalesPersonMid"]);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Add a SrNo column dynamically
                dt.Columns.Add("SrNo", typeof(int));
                dt.Columns.Add("Size", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["SrNo"] = i + 1;
                    dt.Rows[i]["Size"] = "";
                }
                ViewState["DocumentData"] = dt;
                gvDocuments.DataSource = dt;
                gvDocuments.DataBind();
            }
            catch (Exception ex)
            {
            }
        }
        private void LoadDocumentForEdit(int docId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 16);
                cmd.Parameters.AddWithValue("@Mid", docId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    ddlDocumentName.SelectedValue = dt.Rows[0]["DocID"].ToString();
                    txtDocumentNumber.Text = dt.Rows[0]["DocumentNumber"].ToString();

                    ViewState["EditDocID"] = docId;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void DeleteDocument(string mid)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Type", 17);
                cmd.Parameters.AddWithValue("@Mid", mid);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                BindDocumentInf();
            }
            catch { }
        }
        protected void BindBankDetails(object sender, EventArgs e)
        {
            if (chkCopyRetailerBank.Checked)
            {
                string profileId = Session["RetailerUniqueID"]?.ToString();
                if (!string.IsNullOrEmpty(profileId))
                {
                    SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", 22);
                    cmd.Parameters.AddWithValue("@ProfileId", profileId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];

                        txtAccount.Attributes["value"] = dr["BankAccountNumber"].ToString();
                        hdnAccount.Value = dr["BankAccountNumber"].ToString();
                        txtConfirmAccount.Attributes["value"] = dr["BankAccountNumber"].ToString();
                        txtIFSC.Text = dr["IFSCCode"].ToString();
                        txtHolder.Text = dr["AccountHolderName"].ToString();
                        txtBankName.Text = dr["BankName"].ToString();
                        txtBranch.Text = dr["BankBranch"].ToString();
                        txtBranchAddress.Text = dr["BankBranchAddress"].ToString();

                        txtAccount.Enabled = false;
                        txtConfirmAccount.Enabled = false;
                        txtIFSC.Enabled = false;
                        txtHolder.Enabled = false;
                        txtBranchAddress.Enabled = false;
                    }
                    else 
                        lblCopyRetailerBankErrorMessage.Text = "Please set a primary bank account before proceeding.";

                }
            }
            else
            {
                txtAccount.Enabled = true;
                txtConfirmAccount.Enabled = true;
                txtIFSC.Enabled = true;
                txtHolder.Enabled = true;
                txtBranchAddress.Enabled = true;

                txtAccount.Attributes["value"] = "";
                txtConfirmAccount.Attributes["value"] = "";
                txtIFSC.Text = "";
                txtHolder.Text = "";
                txtBankName.Text = "";
                txtBranch.Text = "";
                txtBranchAddress.Text = "";
                lblCopyRetailerBankErrorMessage.Text = "";
            }
        }
        //protected void CommissionChanged(object sender, EventArgs e)
        //{
        //    lblCommisionPer.CssClass = "btn btn-outline-primary";
        //    lblCommissionAmt.CssClass = "btn btn-outline-primary";


        //    if (rbCommisionPer.Checked)
        //    {
        //        lblCommisionPer.CssClass += " selectWarranty";
        //    }
        //    else if (rbCommissionAmt.Checked)
        //    {
        //        lblCommissionAmt.CssClass += " selectWarranty";
        //    }
        //    txtCommission.Focus();
        //}
        //protected void ValidateCommission(object source, ServerValidateEventArgs args)
        //{
        //    if (rbCommisionPer.Checked || rbCommissionAmt.Checked)
        //    {
        //        args.IsValid = true;
        //    }
        //    else
        //    {
        //        args.IsValid = false;
        //    }
        //}
        protected void txtCustomerEmail_TextChanged(object sender, EventArgs e)
        {
            rfvEmail.Validate();
            revEmail.Validate();

            if (!rfvEmail.IsValid || !revEmail.IsValid)
            {
                return;
            }
            bool blockuser = CheckBlockCustomer(txtEmail.Text, "");
            bool existuser = CheckSalesPersonOrRetailer(txtEmail.Text, "");
            if (existuser)
            {
                BlockCustomerEmailErrorMessage.Text = "This Email is already registered";
                BlockCustomerMobileErrorMessage.Text = "";
                btnSubmit.Enabled = false;
                btnUpdate.Enabled = false;
                return;
            }
            else if (blockuser)
            {
                BlockCustomerEmailErrorMessage.Text = BlockCustomerMobileErrorMessage.Text;
                BlockCustomerMobileErrorMessage.Text = "";
                btnSubmit.Enabled = false;
                btnUpdate.Enabled = false;
                return;
            }
            if (!Regex.IsMatch(txtEmail.Text.Trim(), @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
            {
                BlockCustomerEmailErrorMessage.Text = "Invalid email format.";
                txtEmail.Focus();
                return;
            }
            string email = txtEmail.Text.Trim();
            string altemail = txtAltEmail.Text.Trim();

            if (!string.IsNullOrEmpty(email) && email == altemail)
            {
                BlockCustomerEmailErrorMessage.Text = "Email ID and alternate Email ID cannot be the same.";
                btnSubmit.Enabled = false;
                btnUpdate.Enabled = false;
                txtEmail.Focus();
                return;
            }
            BlockCustomerEmailErrorMessage.Text = "";
            BlockAltCustomerEmailErrorMessage.Text = "";
            txtAltEmail.Focus();
            btnSubmit.Enabled = true;
            btnUpdate.Enabled = true;
        }
        protected void txtCustomerMobile_TextChanged(object sender, EventArgs e)
        {
            bool blockuser = CheckBlockCustomer("", txtMobile.Text);
            bool existuser = CheckSalesPersonOrRetailer("", txtMobile.Text);
            if (existuser)
            {
                BlockCustomerMobileErrorMessage.Text = "This Mobile No is already registered";
                btnSubmit.Enabled = false;
                btnUpdate.Enabled = false;
                return;
            }
            else if (blockuser)
            {
                BlockCustomerMobileErrorMessage.Text = BlockCustomerMobileErrorMessage.Text;
                btnSubmit.Enabled = false;
                btnUpdate.Enabled = false;
                return;
            }
            if (Regex.IsMatch(txtMobile.Text, @"^[0-5]"))
            {
                BlockCustomerMobileErrorMessage.Text = "Invalid number";
                txtMobile.Focus();
                return;
            }

            if (Regex.IsMatch(txtMobile.Text, @"^(\d)\1{9}$"))
            {
                BlockCustomerMobileErrorMessage.Text = "Invalid number.";
                txtMobile.Focus();
                return;
            }
            string mobile = txtMobile.Text.Trim();
            string altMobile = txtAltMobile.Text.Trim();

            if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
            {
                BlockCustomerMobileErrorMessage.Text = "Mobile No and alternate mobile numbers cannot be the same.";
                btnSubmit.Enabled = false;
                btnUpdate.Enabled = false;
                txtAltMobile.Focus();
                return;
            }
            BlockCustomerMobileErrorMessage.Text = "";
            BlockAltCustomerMobileErrorMessage.Text = "";
            txtAltMobile.Focus();
            btnSubmit.Enabled = true;
            btnUpdate.Enabled = true;
        }
        protected void txtCustomerAltEmail_TextChanged(object sender, EventArgs e)
        {
            bool blockuser = CheckBlockCustomer(txtAltEmail.Text, "");
            bool existuser = CheckSalesPersonOrRetailer(txtAltEmail.Text, "");
            if (existuser)
            {
                BlockAltCustomerEmailErrorMessage.Text = "This Email is already registered";
                BlockCustomerMobileErrorMessage.Text = "";
                btnSubmit.Enabled = false;
                btnUpdate.Enabled = false;
                return;
            }
            else if (blockuser)
            {
                BlockAltCustomerEmailErrorMessage.Text = BlockCustomerMobileErrorMessage.Text;                
                BlockCustomerMobileErrorMessage.Text = "";
                btnSubmit.Enabled = false;
                btnUpdate.Enabled = false;
                return;
            }
            if (!Regex.IsMatch(txtAltEmail.Text.Trim(), @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
            {
                BlockAltCustomerEmailErrorMessage.Text = "Invalid email format.";
                txtAltEmail.Focus();
                return;
            }
            string email = txtEmail.Text.Trim();
            string altemail = txtAltEmail.Text.Trim();

            if (!string.IsNullOrEmpty(email) && email == altemail)
            {
                BlockAltCustomerEmailErrorMessage.Text = "Email ID and alternate Email ID cannot be the same.";
                btnSubmit.Enabled = false;
                btnUpdate.Enabled = false;
                txtAltEmail.Focus();
                return;
            }
            BlockCustomerEmailErrorMessage.Text = "";
            BlockAltCustomerEmailErrorMessage.Text = "";
            txtDOB.Focus();
            btnSubmit.Enabled = true;
            btnUpdate.Enabled = true;
        }
        protected void txtCustomerAltMobile_TextChanged(object sender, EventArgs e)
        {
            bool blockuser = CheckBlockCustomer("", txtAltMobile.Text);
            bool existuser = CheckSalesPersonOrRetailer("", txtAltMobile.Text);
            if (existuser)
            {
                BlockAltCustomerMobileErrorMessage.Text = "This Mobile No is already registered";
                btnSubmit.Enabled = false;
                btnUpdate.Enabled = false;
                return;
            }
            else if (blockuser)
            {
                BlockAltCustomerMobileErrorMessage.Text = BlockAltCustomerMobileErrorMessage.Text;
                btnSubmit.Enabled = false;
                btnUpdate.Enabled = false;
                return;
            }
            if (Regex.IsMatch(txtAltMobile.Text, @"^[0-5]"))
            {
                BlockAltCustomerMobileErrorMessage.Text = "Invalid number";
                txtAltMobile.Focus();
                return;
            }

            if (Regex.IsMatch(txtAltMobile.Text, @"^(\d)\1{9}$"))
            {
                BlockAltCustomerMobileErrorMessage.Text = "Invalid number.";
                txtAltMobile.Focus();
                return;
            }
            string mobile = txtMobile.Text.Trim();
            string altMobile = txtAltMobile.Text.Trim();

            if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
            {
                BlockAltCustomerMobileErrorMessage.Text = "Mobile No and alternate mobile numbers cannot be the same.";
                btnSubmit.Enabled = false;
                btnUpdate.Enabled = false;
                txtAltMobile.Focus();
                return;
            }
            BlockCustomerMobileErrorMessage.Text = "";
            BlockAltCustomerMobileErrorMessage.Text = "";
            txtEmail.Focus();
            btnSubmit.Enabled = true;
            btnUpdate.Enabled = true;
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
                        BlockCustomerMobileErrorMessage.Text = "This Customer is Under Watch.Please contact your Manager";
                        return true;
                    }
                    else if (suspicious == "2")
                    {
                        BlockCustomerMobileErrorMessage.Text = "This Customer is Black Listed..Please contact your Manager";
                        return true;
                    }
                    else if (suspicious == "")
                    {
                        BlockCustomerMobileErrorMessage.Text = "";
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
                    cmd.Parameters.AddWithValue("@CustomerMobileNo", !string.IsNullOrWhiteSpace(mobileno) ? mobileno: null);
                    cmd.Parameters.AddWithValue("@CustomerEmailID", !string.IsNullOrWhiteSpace(email) ? email : null);
                    cmd.Parameters.AddWithValue("@Mid", Session["UpdateSalesPersonMid"] != null ? Session["UpdateSalesPersonMid"].ToString() : "0");
                    cmd.Parameters.AddWithValue("@type", 50);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        BlockCustomerMobileErrorMessage.Text = "This Mobile No or Email is already registered";
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

        protected void gvDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string fullDocNumber = DataBinder.Eval(e.Row.DataItem, "DocumentNumber")?.ToString();
                Label lblMaskedDoc = (Label)e.Row.FindControl("lblMaskedDoc");


                //if (!string.IsNullOrEmpty(fullDocNumber) && fullDocNumber.Length > 4)
                //{
                //    string masked = new string('*', fullDocNumber.Length - 4) + fullDocNumber.Substring(fullDocNumber.Length - 4);
                //    if (lblMaskedDoc != null)
                //    {
                //        lblMaskedDoc.Text = masked;
                //    }
                //}
                //else
                //{
                    lblMaskedDoc.Text = fullDocNumber;
                // }

                string status = DataBinder.Eval(e.Row.DataItem, "ActionStatus")?.ToString()?.ToLower();

                    switch (status)
                    {
                        case "rejected":
                            e.Row.BackColor = System.Drawing.Color.LightCoral;
                            break;
                        case "approved":
                            e.Row.BackColor = System.Drawing.Color.LightGreen;
                            break;
                        default:
                            e.Row.BackColor = System.Drawing.Color.LightYellow;
                            break;
                    }
                
            }
        }
        private void CheckIfBothDocumentsUploaded()
        {
            bool hasAadhaar = false;
            bool hasPAN = false;

            foreach (GridViewRow row in gvDocuments.Rows)
            {
                string docName = ((Label)row.FindControl("lblMaskedDoc"))?.Text.ToLower();
                string docId = gvDocuments.DataKeys[row.RowIndex]["DocId"].ToString();

                if (docId == "13" || docId == "57" || docId == "58")
                    hasAadhaar = true;
                else if (docId == "19")
                    hasPAN = true;

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

            btnnext3.Enabled = hasAadhaar && hasPAN;
            if (btnnext3.Enabled)
            {
                lblRequiredDocuments.Visible = false;
                btnnext3.Visible = true;
            }
            else
            {
                lblRequiredDocuments.Visible = true;
                btnnext3.Visible = false;
            }
        }


        protected void btnPersonalInformation_Click(object sender, EventArgs e)
        {
            mvViewType.ActiveViewIndex = 0;
            btnPersonalInformationView.Attributes["class"] = "btn btn-primary card-btn";
            btnBandDetailsView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnUploadedDocumentListView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnCommisionDetailsView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnAccountHistoryView.Attributes["class"] = "btn btn-outline-primary card-btn";
        }
        protected void btnBandDetailsView_Click(object sender, EventArgs e)
        {
            mvViewType.ActiveViewIndex = 1;
            btnBandDetailsView.Attributes["class"] = "btn btn-primary card-btn";
            btnPersonalInformationView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnUploadedDocumentListView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnCommisionDetailsView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnAccountHistoryView.Attributes["class"] = "btn btn-outline-primary card-btn";
        }
        protected void btnUploadedDocumentListView_Click(object sender, EventArgs e)
        {
            mvViewType.ActiveViewIndex = 2;
            btnUploadedDocumentListView.Attributes["class"] = "btn btn-primary card-btn";
            btnPersonalInformationView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnBandDetailsView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnCommisionDetailsView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnAccountHistoryView.Attributes["class"] = "btn btn-outline-primary card-btn";
            BindDocumentInf();
        }
        protected void btnCommisionDetailsView_Click(object sender, EventArgs e)
        {
            mvViewType.ActiveViewIndex = 3;
            btnCommisionDetailsView.Attributes["class"] = "btn btn-primary card-btn";
            btnPersonalInformationView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnBandDetailsView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnUploadedDocumentListView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnAccountHistoryView.Attributes["class"] = "btn btn-outline-primary card-btn";
            BindDocumentInf();
        }
        protected void btnAccountHistoryView_Click(object sender, EventArgs e)
        {
            mvViewType.ActiveViewIndex = 4;
            btnAccountHistoryView.Attributes["class"] = "btn btn-primary card-btn";
            btnCommisionDetailsView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnPersonalInformationView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnBandDetailsView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnUploadedDocumentListView.Attributes["class"] = "btn btn-outline-primary card-btn";
        }

        protected void AccountHolder_Change(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFirstName.Text) && !string.IsNullOrWhiteSpace(txtHolder.Text) && txtHolder.Text.ToLower() != txtFirstName.Text.ToLower())
            {
                lblAccountHolderNameError.Attributes.Add("style", "display:block");
                lblAccountHolderNameError.Text = "Full Name and Account Holder name must be same";
                txtHolder.Focus();
            }
            else
            {
                lblAccountHolderNameError.Attributes.Add("style", "display:none");
                lblAccountHolderNameError.Text = "";
            }
        }
        protected void AccountNumberChange(object sender, EventArgs e)
        {
            try
            {
                string qu = Session["UpdateSalesPersonMid"] != null ? Session["UpdateSalesPersonMid"].ToString() : "";
                string decoded = "0";
                if (!string.IsNullOrWhiteSpace(qu))
                {
                    decoded = qu;
                }
                bool AccountNumber = AccountNumberValidation(txtAccount.Text, decoded);
                txtAccount.Text = txtAccount.Text;
                txtAccount.Attributes["value"] = txtAccount.Text;
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
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
                    lblAccountError.Style.Add("display", "block");
                    lblAccountError.Text = "This bank account number already exists in our records.";
                    return true;
                }
                else
                {
                    lblAccountError.Style.Add("display", "none");
                    lblAccountError.Text = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
                return false;
            }
        }

        protected void CreateTicket()
        {
            try
            {
                string mobile = Session["MobileNo1"] != null ? Session["MobileNo1"].ToString() : "";
                if (!string.IsNullOrWhiteSpace(mobile) && mobile == txtMobile.Text.Trim())
                {
                    return;
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@type", 90);
                        cmd.Parameters.AddWithValue("@PlanID", "1");
                        cmd.Parameters.AddWithValue("@ProCat", "3");
                        cmd.Parameters.AddWithValue("@userID", Session["UniqueMid"] != null ? Session["UniqueMid"].ToString() : "");

                        cmd.Parameters.AddWithValue("@customername", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@addressline1", txtCommAddress.Text);
                        cmd.Parameters.AddWithValue("@addressline2", "");
                        cmd.Parameters.AddWithValue("@addressline3", "");
                        cmd.Parameters.AddWithValue("@city", txtCity.Text);
                        cmd.Parameters.AddWithValue("@state", txtState.Text);
                        cmd.Parameters.AddWithValue("@pincode", txtPIN.Text);
                        cmd.Parameters.AddWithValue("@mobileno", txtMobile.Text);
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

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            string resultMessage = dt.Rows[0]["ResultMessage"]?.ToString() ?? string.Empty;
                            if (resultMessage.Contains("already available"))
                            {
                                //lblMessage.Text = "Retailer is already registered with this mobile number.";
                                //lblMessage.ForeColor = System.Drawing.Color.Red;
                                return;
                            }
                            else
                            {
                                //lblMessage.Text = "Registration successful.";
                                //lblMessage.ForeColor = System.Drawing.Color.Green;
                            }
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
                        string docNumber = row["DocumentNumber"].ToString();
                        string status = row["Status"].ToString();
                        string size = row["Size"].ToString();
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
                string ticketNo = GetTicketno(txtMobile.Text != null ? txtMobile.Text.ToString() : "");
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
                //lblMessage.Text = "Document uploaded and saved successfully!";
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Upload failed: " + ex.Message;
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
                string ticketNo = GetTicketno(txtMobile.Text != null ? txtMobile.Text.ToString() : "");
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
                        cmd.Parameters.AddWithValue("@UserRole", Session["Role"] != null ? Session["Role"].ToString() : "");

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
                    cmd.Parameters.AddWithValue("@mobileno", txtMobile.Text != null ? txtMobile.Text.ToString() : "");

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