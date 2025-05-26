using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Patner_Retailer_ADO
{
    public partial class Profile : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iaplConnectionString"].ConnectionString);
        static public void DisplayMessage(Control page, string msg)
        {
            string msg1 = String.Format("alert('{0}');", msg);
            ScriptManager.RegisterStartupScript(page, page.GetType(), "msg", msg1, true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RetailerUniqueID"] != null)
            {

            }
            else
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {

                LoadProfileData();
                BindBankDetails();
                BindUploadedDocuments();
                BindDealer();
            }
        }
        private void LoadProfileData()
        {
            SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", 3);
            cmd.Parameters.AddWithValue("@Mid", Session["RetailerUniqueID"].ToString());

            cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                lblNameValue.Text = dr["Name"].ToString();
                lblMobileNoValue.Text = dr["MobileNo"].ToString();
                lblWhatsappNoValue.Text = dr["WhatsappMobileNo"].ToString();
                lblEmailValue.Text = dr["emailID"].ToString();
                lblDOBValue.Text = dr["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateOfBirth"]).ToString("dd-MMM-yyyy") : null;
                lblGenderValue.Text = dr["Gender"].ToString();
                lblPincodeValue.Text = dr["Pincode"].ToString();
                lblCityValue.Text = dr["City"].ToString();
                lblStateValue.Text = dr["State"].ToString();
                lblAddressValue.Text = dr["Address"].ToString();
            }
            dr.Close();
            con.Close();
        }


        private void BindBankDetails()
        {
            SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", 18);
            cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString());
            cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                RepeaterBankDetails.DataSource = dt;
                RepeaterBankDetails.DataBind();
            }
            con.Close();
        }

        private void BindUploadedDocuments()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 19);
                cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString());
                cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {

                    rptDocuments.DataSource = dt;
                    rptDocuments.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void BindDealer()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 20);
                cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString());
                cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {

                    RepeaterEmployeeDetails.DataSource = dt;
                    RepeaterEmployeeDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnAddBank_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddBank.aspx");
        }
        protected void RepeaterBankDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EditBank")
            {
                string mid = e.CommandArgument.ToString();
                string bankId = Convert.ToBase64String(Encoding.UTF8.GetBytes(mid));
                Response.Redirect($"AddBank.aspx?qu="+ HttpUtility.UrlEncode(bankId));
            }
            else if (e.CommandName == "DeleteBank")
            {
                string mid = e.CommandArgument.ToString();
                DeleteBankRecord(mid);                
            }
        }

        private void DeleteBankRecord(string mid)
        {
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 26);
                cmd.Parameters.AddWithValue("@Mid", mid);                

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            BindBankDetails();
            DisplayMessage(this, "Bank record deleted successfully.");
        }
        protected void btnAddDealer_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddDealer.aspx");
        }

        protected void RepeaterDealerDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EditDealer")
            {
                string mid = e.CommandArgument.ToString();
                string bankId = Convert.ToBase64String(Encoding.UTF8.GetBytes(mid));
                Response.Redirect($"AddDealer.aspx?qu=" + HttpUtility.UrlEncode(bankId));
            }
            else if (e.CommandName == "DeleteDealer")
            {
                string mid = e.CommandArgument.ToString();
                DeleteDealerRecord(mid);
            }
        }

        private void DeleteDealerRecord(string mid)
        {
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 29);
                cmd.Parameters.AddWithValue("@Mid", mid);
                cmd.Parameters.AddWithValue("@UserRole", Session["Role"]?.ToString() ?? "");

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            BindDealer();
            DisplayMessage(this, "Dealer record deleted successfully.");
        }

        protected void btnUploadDocument_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddDocument.aspx");
        }
        protected void RepeaterDocumentDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Editdoc")
            {
                string mid = e.CommandArgument.ToString();
                string docid = Convert.ToBase64String(Encoding.UTF8.GetBytes(mid));
                Response.Redirect($"AddDocument.aspx?qu=" + HttpUtility.UrlEncode(docid));
            }
            else if (e.CommandName == "Deletedoc")
            {
                string mid = e.CommandArgument.ToString();
                DeleteDocumentRecord(mid);
            }
        }

        private void DeleteDocumentRecord(string mid)
        {
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 32);
                cmd.Parameters.AddWithValue("@Mid", mid);
                cmd.Parameters.AddWithValue("@UserRole", Session["Role"]?.ToString() ?? "");

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            BindUploadedDocuments();
            DisplayMessage(this, "Document deleted successfully.");
        }

        protected void btnEditProfile_Click(object sender, EventArgs e)
        {
            btnEditProfile.Visible = false;
            btnCancelProfile.Visible = true;
            btnUpdateProfile.Visible = true;

            BindEditValue();
        }
        protected void BindEditValue()
        {
            lblNameValue.Visible = false;
            lblMobileNoValue.Visible = false;
            lblWhatsappNoValue.Visible = false;
            lblEmailValue.Visible = false;
            lblDOBValue.Visible = false;
            lblGenderValue.Visible = false;
            lblPincodeValue.Visible = false;
            lblAddressValue.Visible = false;

            TextBox1.Attributes.Add("ReadOnly", "readonly");

            txtFirstName.Visible = true;
            txtMobileNumber.Visible = true;
            txtAlternateMobile.Visible = true;
            txtEmail.Visible = true;
            TextBox1.Visible = true;
            ddlGender.Visible = true;
            txtPinCode.Visible = true;
            txtAddress.Visible = true;

            txtFirstName.Text = lblNameValue.Text;
            txtMobileNumber.Text = lblMobileNoValue.Text;
            txtAlternateMobile.Text = lblWhatsappNoValue.Text;
            txtEmail.Text = lblEmailValue.Text;
            TextBox1.Text = lblDOBValue.Text;
            ddlGender.SelectedValue = lblGenderValue.Text;
            txtPinCode.Text = lblPincodeValue.Text;
            txtAddress.Text = lblAddressValue.Text;
        }
        protected void btnCancelProfile_Click(object sender, EventArgs e)
        {
            btnEditProfile.Visible = true;
            btnCancelProfile.Visible = false;
            btnUpdateProfile.Visible = false;

            lblNameValue.Visible = true;
            lblMobileNoValue.Visible = true;
            lblWhatsappNoValue.Visible = true;
            lblEmailValue.Visible = true;
            lblDOBValue.Visible = true;
            lblGenderValue.Visible = true;
            lblPincodeValue.Visible = true;
            lblAddressValue.Visible = true;

            txtFirstName.Visible = false;
            txtMobileNumber.Visible = false;
            txtAlternateMobile.Visible = false;
            txtEmail.Visible = false;
            TextBox1.Visible = false;
            ddlGender.Visible = false;
            txtPinCode.Visible = false;
            txtAddress.Visible = false;
        }
        protected void btnUpdateProfile_Click(object sender, EventArgs e)
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
                    lblErrorPincode.Visible = true;
                    txtPinCode.Focus();
                    count++;
                }
                if (txtPinCode.Text.Length != 6)
                {
                    lblErrorPincode.Visible = true;
                    lblErrorPincode.InnerText = "Enter a 6-digit Pincode";
                    txtPinCode.Focus();
                    count++;
                }
                else { lblErrorPincode.Visible = false; }

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
                    lblErrorMobileNo.Visible = true;
                    txtMobileNumber.Focus();
                    count++;
                }
                else { lblErrorMobileNo.Visible = false; }

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
                    return;
                }
                else
                {
                    if (Session["RetailerUniqueID"] != null)
                    {
                        SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Type", 33);
                        cmd.Parameters.AddWithValue("@Mid", Session["RetailerUniqueID"].ToString());
                        cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());
                        cmd.Parameters.AddWithValue("@CustomerName", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerMobileNo", txtMobileNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@WhatsappNo", txtAlternateMobile.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerEmailID", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Pincode", txtPinCode.Text.Trim());
                        cmd.Parameters.AddWithValue("@City", lblCityValue.Text.Trim());
                        cmd.Parameters.AddWithValue("@State", lblStateValue.Text.Trim());
                        cmd.Parameters.AddWithValue("@AddressLine1", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedItem.Text.ToString());
                        cmd.Parameters.AddWithValue("@DateOfBirth", TextBox1.Text.Trim());
                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        con.Close();

                        btnEditProfile.Visible = true;
                        btnCancelProfile.Visible = false;
                        btnUpdateProfile.Visible = false;

                        LoadProfileData();
                        lblNameValue.Visible = true;
                        lblMobileNoValue.Visible = true;
                        lblWhatsappNoValue.Visible = true;
                        lblEmailValue.Visible = true;
                        lblDOBValue.Visible = true;
                        lblGenderValue.Visible = true;
                        lblPincodeValue.Visible = true;
                        lblAddressValue.Visible = true;

                        txtFirstName.Visible = false;
                        txtMobileNumber.Visible = false;
                        txtAlternateMobile.Visible = false;
                        txtEmail.Visible = false;
                        TextBox1.Visible = false;
                        ddlGender.Visible = false;
                        txtPinCode.Visible = false;
                        txtAddress.Visible = false;
                    }
                    else
                    {
                        string script = $@"
                            <script type='text/javascript'>
                                alert('Session has been removed, Please re-login');
                                window.location.href = 'Login.aspx';
                            </script>";

                        ClientScript.RegisterStartupScript(this.GetType(), "LoginRedirect", script);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
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
                        lblCityValue.Text = dt.Rows[0]["CityName"].ToString();
                        lblStateValue.Text = dt.Rows[0]["statename"].ToString();
                    }
                }
                else
                {
                    lblErrorPincode.Visible = true;
                    lblErrorPincode.InnerText = "Enter a 6-digit Pincode";
                    txtPinCode.Focus();                    
                }
            }
            catch (Exception ex)
            {

                return;
            }
        }
    }
}