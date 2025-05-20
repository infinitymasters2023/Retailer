using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Patner_Retailer_ADO
{
    public partial class CreateAnAccount : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iaplConnectionString"].ConnectionString);        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDocument();
                hdnActiveTab.Value = "#step1";
                txtBankName.Enabled = false;
                txtBranchName.Enabled = false;
                txtBranchAddress.Enabled = false;
                lblSellerState.Visible = false;
                lblSellerCity.Visible = false;
                TextBox1.Attributes.Add("ReadOnly", "readonly");
                txtGSTIN.Text = Session["SellerGSTIN"] != null ? Session["SellerGSTIN"].ToString() : null;
                txtGSTIN.Enabled = Session["SellerGSTIN"] != null ? false : true;
                Session.Remove("SellerGSTIN");
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

        protected void btnUploadFront_Click(object sender, EventArgs e)
        {
            if (fuFrontSide.HasFile)
            {
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
                    hdnActiveTab.Value = "#step4";
                    if (gvDocuments.HeaderRow != null)
                    {
                        gvDocuments.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                }
                catch (Exception ex)
                {
                    // Log or show error
                }
            }
        }

        protected void btnUploadBack_Click(object sender, EventArgs e)
        {

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {

            try
            {
                int count = 0;

                if (string.IsNullOrWhiteSpace(txtAddress.Text))
                {
                    lblCurrentAddress.Visible = true;
                    txtAddress.Focus();
                    count++;
                }
                else { lblCurrentAddress.Visible = false; }

                if (string.IsNullOrWhiteSpace(txtPinCode.Text))
                {
                    lblPincode.Visible = true;
                    txtPinCode.Focus();
                    count++;
                }
                if (txtPinCode.Text.Length != 6)
                {
                    lblPincode.Visible = true;
                    lblPincode.InnerText = "Enter a 6-digit Pincode";
                    txtPinCode.Focus();
                    count++;
                }
                else { lblPincode.Visible = false; }

                if (string.IsNullOrWhiteSpace(TextBox1.Text))
                {
                    lblDateOfBirth.Visible = true;
                    TextBox1.Focus();
                    count++;
                }
                else { lblDateOfBirth.Visible = false; }

                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    lblEmailAddress.Visible = true;
                    txtEmail.Focus();
                    count++;
                }
                else { lblEmailAddress.Visible = false; }

                if (string.IsNullOrWhiteSpace(txtMobileNumber.Text))
                {
                    lblMobileNo.Visible = true;
                    txtMobileNumber.Focus();
                    count++;
                }
                else { lblMobileNo.Visible = false; }

                if (!string.IsNullOrWhiteSpace(txtFirstName.Text) && txtFirstName.Text.Length < 3)
                {
                    lblFullName.Visible = true;
                    lblFullName.InnerText = "First Name must be between 3 and 30 characters.";
                    txtFirstName.Focus();
                    count++;
                }
                if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    lblFullName.Visible = true;
                    txtFirstName.Focus();                    
                    count++;
                }
                else
                {
                    lblFullName.Visible = false;
                }
              
                if (count > 0)
                {
                    hdnActiveTab.Value = "#step1";
                    return;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                    cmd.CommandType = CommandType.StoredProcedure;
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
                    cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedItem.Text.ToString());
                    cmd.Parameters.AddWithValue("@DateOfBirth", TextBox1.Text.Trim());

                    SqlParameter outputMid = new SqlParameter("@FreelanerIdd", SqlDbType.Int);
                    outputMid.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputMid);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (outputMid.Value != null)
                    {
                        Session["UniqueMid"] = Convert.ToInt32(outputMid.Value);
                        string script = "$('.nav-tabs > .active').next('li').find('a').click();";
                        ScriptManager.RegisterStartupScript(this, GetType(), "MoveToNextTab", script, true);
                        hdnActiveTab.Value = "#step2";

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
            if (!Regex.IsMatch(txtIFSCCode.Text, pattern))
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
            
            if (count > 0)
            {
                hdnActiveTab.Value = "#step3";
                return;
            }
            else
            {
                if (Session["UniqueMid"] != null)
                {
                    SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", 5);
                    cmd.Parameters.AddWithValue("@ProfileId", Session["UniqueMid"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@BankAccountNumber", txtAccountNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@IFSCCode", txtIFSCCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@AccountHolderName", txtAccountHolderName.Text.Trim());
                    cmd.Parameters.AddWithValue("@BankName", txtBankName.Text.Trim());
                    cmd.Parameters.AddWithValue("@BankBranch", txtBranchName.Text.Trim());
                    cmd.Parameters.AddWithValue("@BankBranchAddress", txtBranchAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Status", "Pending");

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        string script = "$('.nav-tabs > .active').next('li').find('a').click();";
                        ScriptManager.RegisterStartupScript(this, GetType(), "MoveToNextTab", script, true);
                    }
                    hdnActiveTab.Value = "#step4";
                }
            }
        }

        protected void btnnext3_Click(object sender, EventArgs e)
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
                    cmd.Parameters.AddWithValue("@Remarks", docNumber);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@IPAddress", Request.UserHostAddress);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            string script = "$('.nav-tabs > .active').next('li').find('a').click();";
            ScriptManager.RegisterStartupScript(this, GetType(), "MoveToNextTab", script, true);
            hdnActiveTab.Value = "#step5";

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
                        txtCity.Text = dt.Rows[0]["CityName"].ToString();
                        txtState.Text = dt.Rows[0]["statename"].ToString();
                        txtCity.Enabled = false;
                        txtState.Enabled = false;
                    }
                    else
                    {
                        txtCity.Enabled = true;
                        txtState.Enabled = true;
                    }
                }
                else
                {
                    lblPincode.Visible = true;
                    lblPincode.InnerText = "Enter a 6-digit Pincode";
                    txtPinCode.Focus();
                    hdnActiveTab.Value = "#step1";
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
                        lblSellerCity.Text = dt.Rows[0]["CityName"].ToString();
                        lblSellerState.Text = dt.Rows[0]["statename"].ToString();
                        lblSellerCity.Visible = true;
                        lblSellerState.Visible = true;

                    }
                    else
                    {
                        lblSellerCity.Text = null;
                        lblSellerState.Text = null;
                        lblSellerCity.Visible = false;
                        lblSellerState.Visible = false;
                    }
                }
                else
                {
                    lblSellerPINCode.Visible = true;
                    lblSellerPINCode.InnerText = "Enter a 6-digit Pincode";
                    txtSellerPincode.Focus();
                    hdnActiveTab.Value = "#step2";
                }
            }
            catch (Exception ex)
            {

                return;
            }
        }

        protected void txtIFSC_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^[A-Z]{4}0[A-Z0-9]{6}$";
            if (!Regex.IsMatch(txtIFSCCode.Text, pattern))
            {
                lblIFSCCode.Style["display"] = "block";
                lblIFSCCode.Text = "Invalid IFSC code format.";
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

        protected void btnSeller_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;

                if (string.IsNullOrWhiteSpace(txtSellerLandmark.Text))
                {
                    lblSellerLandMark.Visible = true;
                    txtSellerLandmark.Focus();                    
                    count++;
                }
                else
                {
                    lblSellerLandMark.Visible = false;
                }

                if (string.IsNullOrWhiteSpace(txtSellerAddressLine1.Text))
                {
                    lblSellerAddress.Visible = true;
                    txtSellerAddressLine1.Focus();                    
                    count++;
                }
                else
                {
                    lblSellerAddress.Visible = false;
                }

                if (string.IsNullOrWhiteSpace(txtSellerPincode.Text))
                {
                    lblSellerPINCode.Visible = true;
                    txtSellerPincode.Focus();                    
                    count++;
                }
                if (txtSellerPincode.Text.Length != 6)
                {
                    lblSellerPINCode.Visible = true;
                    lblSellerPINCode.InnerText = "Enter a 6-digit Pincode";
                    txtSellerPincode.Focus();
                    count++;
                }
                else
                {
                    lblSellerPINCode.Visible = false;
                }

                if (string.IsNullOrWhiteSpace(txtGSTIN.Text))
                {
                    lblSellerGSTIN.Visible = true;
                    txtGSTIN.Focus();
                    count++;
                }
                string gstinPattern = @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[A-Z0-9]{1}[Z]{1}[A-Z0-9]{1}$";
                if (!Regex.IsMatch(txtGSTIN.Text.Trim().ToUpper(), gstinPattern))
                {
                    lblSellerGSTIN.Visible = true;
                    lblSellerGSTIN.InnerText = "Invalid GSTIN format.";
                    txtGSTIN.Focus();
                    count++;
                }
                else
                {
                    lblSellerGSTIN.Visible = false;
                }
                if (txtSellerName.Text.Length < 3)
                {
                    lblSellerName.Visible = true;
                    lblSellerName.InnerText = "Seller Name must be between 3 and 30 characters.";
                    txtSellerName.Focus();
                    count++;
                }
                if (string.IsNullOrWhiteSpace(txtSellerName.Text))
                {
                    lblSellerName.Visible = true;
                    txtSellerName.Focus();
                    count++;
                }
                else
                {
                    lblSellerName.Visible = false;
                }  
                
                if (count > 0)
                {
                    hdnActiveTab.Value = "#step2";
                    return;
                }
                if (!string.IsNullOrWhiteSpace(txtSellerName.Text) && (txtSellerName.Text.Length >= 3) && !string.IsNullOrWhiteSpace(txtGSTIN.Text) && !string.IsNullOrWhiteSpace(txtSellerPincode.Text) &&
                    !string.IsNullOrWhiteSpace(txtSellerAddressLine1.Text) && !string.IsNullOrWhiteSpace(txtSellerLandmark.Text))
                {
                    SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", 16);
                    cmd.Parameters.AddWithValue("@CustomerName", txtSellerName.Text.Trim());
                    cmd.Parameters.AddWithValue("@SellerGSTINNo", txtGSTIN.Text.Trim());
                    cmd.Parameters.AddWithValue("@AddressLine1", txtSellerAddressLine1.Text.Trim());
                    cmd.Parameters.AddWithValue("@AddressLine2", txtSellerAddressLine2.Text.Trim());
                    cmd.Parameters.AddWithValue("@Landmark", txtSellerLandmark.Text.Trim());
                    cmd.Parameters.AddWithValue("@Pincode", txtSellerPincode.Text.Trim());
                    cmd.Parameters.AddWithValue("@City", lblSellerCity.Text.Trim());
                    cmd.Parameters.AddWithValue("@State", lblSellerState.Text.Trim());
                    cmd.Parameters.AddWithValue("@ProfileId", Session["UniqueMid"].ToString().Trim());

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();

                    string script = "$('.nav-tabs > .active').next('li').find('a').click();";
                    ScriptManager.RegisterStartupScript(this, GetType(), "MoveToNextTab", script, true);
                    hdnActiveTab.Value = "#step3";
                }
            }
            catch (Exception ex)
            {
                hdnActiveTab.Value = "#step2";
            }

        }

    }
}