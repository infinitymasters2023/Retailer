using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.IO;

namespace Patner_Retailer_ADO
{
    public partial class AddBank : System.Web.UI.Page
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
                BindBankSupportingDocument();
                fuSuppotingDoc.Attributes["accept"] = ".jpg,.jpeg,.png,.pdf";
                string qu = Request.QueryString["qu"];
                if (!string.IsNullOrWhiteSpace(qu))
                {
                    string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(qu));
                    BindBankDetails(decoded);
                    btnSubmit.Visible = false;
                    btnEdit.Visible = true;
                    hdrtext.InnerText = "Edit Bank Details";
                }

                txtBankName.Enabled = false;
                txtBranchName.Enabled = false;
                txtBranchAddress.Enabled = false;
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
        protected void txtIFSC_TextChanged(object sender, EventArgs e)
        {
            txtAccountNumber.Attributes["value"] = txtAccountNumber.Text;
            txtConfirmAccountNumber.Attributes["value"] = txtConfirmAccountNumber.Text;
            string pattern = @"^[A-Z]{4}0[A-Z0-9]{6}$";
            if (!Regex.IsMatch(txtIFSCCode.Text.ToUpper(), pattern))
            {
                lblIFSCCode.Style["display"] = "block";
                lblIFSCCode.Text = "Invalid IFSC code format.";
                txtBankName.Text = null;
                txtBranchName.Text = null;
                txtBranchAddress.Text = null;
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
                txtBankName.Text = null;
                txtBranchName.Text = null;
                txtBranchAddress.Text = null;
                lblIFSCCode.Style["display"] = "block";
                lblIFSCCode.Text = "No matching IFSC code found.";
            }
        }

        protected void BindBankDetails(string mid)
        {
            try
            {
                if (string.IsNullOrEmpty(mid))
                    return;
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 24);
                    cmd.Parameters.AddWithValue("@Mid", mid);
                    cmd.Parameters.AddWithValue("@UserRole", Session["Role"]?.ToString() ?? "");

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        txtAccountNumber.Attributes["value"] = dr["BankAccountNumber"] != DBNull.Value ? dr["BankAccountNumber"].ToString() : "";
                        txtConfirmAccountNumber.Attributes["value"] = dr["BankAccountNumber"] != DBNull.Value ? dr["BankAccountNumber"].ToString() : "";
                        txtIFSCCode.Text = dr["IFSCCode"] != DBNull.Value ? dr["IFSCCode"].ToString() : "";
                        txtAccountHolderName.Text = dr["AccountHolderName"] != DBNull.Value ? dr["AccountHolderName"].ToString() : "";
                        txtBankName.Text = dr["BankName"] != DBNull.Value ? dr["BankName"].ToString() : "";
                        txtBranchName.Text = dr["BankBranch"] != DBNull.Value ? dr["BankBranch"].ToString() : "";
                        txtBranchAddress.Text = dr["BankBranchAddress"] != DBNull.Value ? dr["BankBranchAddress"].ToString() : "";
                        txtUPIID.Text = dr["UPIID"] != DBNull.Value ? dr["UPIID"].ToString() : "";
                        ddlTypeOfBank.SelectedValue = dr["TypeofBankAccount"] != DBNull.Value ? dr["TypeofBankAccount"].ToString() : "";
                        chkJointAccount.SelectedValue = dr["IsThisYourJointAccount"] != DBNull.Value ? dr["IsThisYourJointAccount"].ToString() : "";
                        txtJointHolderName.Text = dr["JointAccountHolderName"] != DBNull.Value ? dr["JointAccountHolderName"].ToString() : "";
                        ddlSuppotingDoc.SelectedValue = dr["SupportingDocuments"] != DBNull.Value ? dr["SupportingDocuments"].ToString() : "";
                        lblsupportingDocName.Text = dr["SupportingDocumentsPath"] != DBNull.Value ? dr["SupportingDocumentsPath"].ToString() : "";
                    }
                    else
                    {
                        txtAccountNumber.Text = "";
                        txtConfirmAccountNumber.Text = "";
                        txtIFSCCode.Text = "";
                        txtAccountHolderName.Text = "";
                        txtBankName.Text = "";
                        txtBranchName.Text = "";
                        txtBranchAddress.Text = "";
                        txtUPIID.Text = "";
                        ddlTypeOfBank.SelectedValue = "";
                        chkJointAccount.SelectedValue ="";
                        txtJointHolderName.Text = "";
                        ddlSuppotingDoc.SelectedValue = "";
                        lblsupportingDocName.Text = "";
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }

        protected void btnAddBank_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;

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
                if (ddlSuppotingDoc.SelectedItem.Value == "")
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
                if (!string.IsNullOrWhiteSpace(txtAccountHolderName.Text) && txtAccountHolderName.Text.Length < 3)
                {
                    lblAccountHoldername.Visible = true;
                    lblAccountHoldername.Text = "Account Holder Name must be between 3 and 30 characters.";
                    txtAccountHolderName.Focus();
                    count++;
                }
                if (string.IsNullOrWhiteSpace(txtAccountHolderName.Text))
                {
                    lblAccountHoldername.Visible = true;
                    txtAccountHolderName.Focus();
                    count++;
                }
                else { lblAccountHoldername.Visible = false; }

                string pattern = @"^[A-Z]{4}0[A-Z0-9]{6}$";
                if (!Regex.IsMatch(txtIFSCCode.Text.ToUpper(), pattern))
                {
                    lblIFSCCode.Style["display"] = "block";
                    lblIFSCCode.Text = "Invalid IFSC code format.";
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
                    lblConfirmAccountNumber.Text = "Account numbers do not match.";
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
                bool validatedAccount = AccountNumberValidation(txtAccountNumber.Text, "0");
                if (validatedAccount)
                {
                    count++;
                }
                if (count > 0)
                {
                    return;
                }
                else
                {
                    if (Session["RetailerUniqueID"] != null)
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
                        
                        string fileName = Path.GetFileName(fuSuppotingDoc.FileName);
                        string folderPath = Server.MapPath("~/UploadedDocuments/");
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        string uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName;
                        string fullPath = Path.Combine(folderPath, uniqueFileName);
                        fuSuppotingDoc.SaveAs(fullPath);


                        SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (Session["Role"].ToString() == "Admin")
                            cmd.Parameters.AddWithValue("@Type", 27);
                        else
                            cmd.Parameters.AddWithValue("@Type", 26);
                        cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString().Trim());
                        cmd.Parameters.AddWithValue("@BankAccountNumber", txtAccountNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@IFSCCode", txtIFSCCode.Text.ToUpper().Trim());
                        cmd.Parameters.AddWithValue("@AccountHolderName", txtAccountHolderName.Text.Trim());
                        cmd.Parameters.AddWithValue("@BankName", txtBankName.Text.Trim());
                        cmd.Parameters.AddWithValue("@BankBranch", txtBranchName.Text.Trim());
                        cmd.Parameters.AddWithValue("@BankBranchAddress", txtBranchAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@Status", "Secondary");
                        cmd.Parameters.AddWithValue("@UPIID", txtUPIID.Text.ToString());
                        cmd.Parameters.AddWithValue("@TypeofBankAccount", ddlTypeOfBank.SelectedValue == "" ? null : ddlTypeOfBank.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@IsThisYourJointAccount", chkJointAccount.SelectedValue == "" ? null : chkJointAccount.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@JointAccountHolderName", txtJointHolderName.Text.ToString());
                        cmd.Parameters.AddWithValue("@SupportingDocuments", ddlSuppotingDoc.SelectedValue == "" ? null : ddlSuppotingDoc.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@SupportingDocumentsPath", uniqueFileName);

                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        con.Close();

                        string script = $@"
                            <script type='text/javascript'>
                                alert('Bank Details has been saved successfully!');
                                window.location.href = 'Profile.aspx?qu=Bank';
                            </script>";

                        ClientScript.RegisterStartupScript(this.GetType(), "ProfileRedirect", script);
                        return;

                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }
        protected void btnEditBank_Click(object sender, EventArgs e)
        {
            try
            {
                string qu = Request.QueryString["qu"];
                if (!string.IsNullOrWhiteSpace(qu))
                {
                    string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(qu));
                    int count = 0;
                    if (fuSuppotingDoc.HasFile || !string.IsNullOrWhiteSpace(lblsupportingDocName.Text))
                    {
                        lblsupportingDocError.Attributes.Add("style", "display: none;");
                    }
                    else
                    {
                        lblSupportingDocumentError.Attributes.Add("style", "display: block;");
                        lblSupportingDocumentError.Text = "Please upload the Supporting Document.";
                        lblSupportingDocumentError.Focus();
                        count++;
                    }
                    if (ddlSuppotingDoc.SelectedItem.Value == "")
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
                    if (!string.IsNullOrWhiteSpace(txtAccountHolderName.Text) && txtAccountHolderName.Text.Length < 3)
                    {
                        lblAccountHoldername.Visible = true;
                        lblAccountHoldername.Text = "Account Holder Name must be between 3 and 30 characters.";
                        txtAccountHolderName.Focus();
                        count++;
                    }
                    if (string.IsNullOrWhiteSpace(txtAccountHolderName.Text))
                    {
                        lblAccountHoldername.Visible = true;
                        txtAccountHolderName.Focus();
                        count++;
                    }
                    else { lblAccountHoldername.Visible = false; }

                    string pattern = @"^[A-Z]{4}0[A-Z0-9]{6}$";
                    if (!Regex.IsMatch(txtIFSCCode.Text.ToUpper(), pattern))
                    {
                        lblIFSCCode.Style["display"] = "block";
                        lblIFSCCode.Text = "Invalid IFSC code format.";
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
                        lblConfirmAccountNumber.Text = "Account numbers do not match.";
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
                        lblAccountNumber.Text = "Account Number is required.";
                        txtAccountNumber.Focus();
                        count++;
                    }
                    else { lblAccountNumber.Style["display"] = "none"; }
                    bool validatedAccount = AccountNumberValidation(txtAccountNumber.Text, decoded);
                    if (!string.IsNullOrWhiteSpace(txtAccountNumber.Text) && validatedAccount)
                    {
                        count++;
                    }
                    if (count > 0)
                    {
                        return;
                    }
                    else
                    {
                        if (Session["RetailerUniqueID"] != null)
                        {
                            string uniqueFileName = string.Empty;                            
                            if (fuSuppotingDoc.HasFile)
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

                                string fileName = Path.GetFileName(fuSuppotingDoc.FileName);
                                string folderPath = Server.MapPath("~/UploadedDocuments/");
                                if (!Directory.Exists(folderPath))
                                    Directory.CreateDirectory(folderPath);

                                uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName;
                                string fullPath = Path.Combine(folderPath, uniqueFileName);
                                fuSuppotingDoc.SaveAs(fullPath);
                            }
                            else if(!string.IsNullOrWhiteSpace(lblsupportingDocName.Text))
                                uniqueFileName = lblsupportingDocName.Text;
                            else
                            {
                                lblSupportingDocumentError.Text = "Please upload only jpg, jpeg, png and pdf format.";
                                lblSupportingDocumentError.Attributes.Add("style", "display:block");
                                return;
                            }

                            SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Type", 25);
                            cmd.Parameters.AddWithValue("@UserRole", Session["Role"]?.ToString() ?? "");
                            cmd.Parameters.AddWithValue("@Mid", decoded);
                            cmd.Parameters.AddWithValue("@BankAccountNumber", txtAccountNumber.Text.Trim());
                            cmd.Parameters.AddWithValue("@IFSCCode", txtIFSCCode.Text.ToUpper().Trim());
                            cmd.Parameters.AddWithValue("@AccountHolderName", txtAccountHolderName.Text.Trim());
                            cmd.Parameters.AddWithValue("@BankName", txtBankName.Text.Trim());
                            cmd.Parameters.AddWithValue("@BankBranch", txtBranchName.Text.Trim());
                            cmd.Parameters.AddWithValue("@BankBranchAddress", txtBranchAddress.Text.Trim());
                            cmd.Parameters.AddWithValue("@IPAddress", Request.UserHostAddress);
                            cmd.Parameters.AddWithValue("@UPIID", txtUPIID.Text.ToString());
                            cmd.Parameters.AddWithValue("@TypeofBankAccount", ddlTypeOfBank.SelectedValue == "" ? null : ddlTypeOfBank.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@IsThisYourJointAccount", chkJointAccount.SelectedValue == "" ? null : chkJointAccount.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@JointAccountHolderName", txtJointHolderName.Text.ToString());
                            cmd.Parameters.AddWithValue("@SupportingDocuments", ddlSuppotingDoc.SelectedValue == "" ? null : ddlSuppotingDoc.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@SupportingDocumentsPath", uniqueFileName);

                            con.Open();
                            int i = cmd.ExecuteNonQuery();
                            con.Close();

                            string script = $@"
                            <script type='text/javascript'>
                                alert('Bank Details has been updated successfully!');
                                window.location.href = 'Profile.aspx?qu=Bank';
                            </script>";

                            ClientScript.RegisterStartupScript(this.GetType(), "ProfileRedirect", script);
                            return;

                        }
                    }
                }
                else
                {
                    string script = $@"
                            <script type='text/javascript'>
                                alert('Invalid Link');
                                window.location.href = 'Profile.aspx?qu=Bank';
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
                txtJointHolderName.Focus();
            }
            else
            {
                jointAcountHolderpnl.Visible = false;
                ddlSuppotingDoc.Focus();
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
                    lblAccountNumber.Style.Add("display", "block");
                    lblAccountNumber.Text = "This bank account number already exists in our records.";
                    return true;
                }
                else
                {
                    lblAccountNumber.Style.Add("display", "none");
                    lblAccountNumber.Text = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
                return false;
            }
        }
    }
}