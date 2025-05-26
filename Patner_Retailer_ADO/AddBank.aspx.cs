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
                        txtAccountNumber.Text = dr["BankAccountNumber"] != DBNull.Value ? dr["BankAccountNumber"].ToString() : "";
                        txtConfirmAccountNumber.Text = dr["BankAccountNumber"] != DBNull.Value ? dr["BankAccountNumber"].ToString() : "";
                        txtIFSCCode.Text = dr["IFSCCode"] != DBNull.Value ? dr["IFSCCode"].ToString() : "";
                        txtAccountHolderName.Text = dr["AccountHolderName"] != DBNull.Value ? dr["AccountHolderName"].ToString() : "";
                        txtBankName.Text = dr["BankName"] != DBNull.Value ? dr["BankName"].ToString() : "";
                        txtBranchName.Text = dr["BankBranch"] != DBNull.Value ? dr["BankBranch"].ToString() : "";
                        txtBranchAddress.Text = dr["BankBranchAddress"] != DBNull.Value ? dr["BankBranchAddress"].ToString() : "";
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
                    return;
                }
                else
                {
                    if (Session["RetailerUniqueID"] != null)
                    {
                        SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Type", 5);
                        cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString().Trim());
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

                        string script = $@"
                            <script type='text/javascript'>
                                alert('Bank Details has been saved successfully!');
                                window.location.href = 'Profile.aspx';
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
                        return;
                    }
                    else
                    {

                        string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(qu));
                        if (Session["RetailerUniqueID"] != null)
                        {
                            SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Type", 25);
                            cmd.Parameters.AddWithValue("@UserRole", Session["Role"]?.ToString() ?? "");
                            cmd.Parameters.AddWithValue("@Mid", decoded);
                            cmd.Parameters.AddWithValue("@BankAccountNumber", txtAccountNumber.Text.Trim());
                            cmd.Parameters.AddWithValue("@IFSCCode", txtIFSCCode.Text.Trim());
                            cmd.Parameters.AddWithValue("@AccountHolderName", txtAccountHolderName.Text.Trim());
                            cmd.Parameters.AddWithValue("@BankName", txtBankName.Text.Trim());
                            cmd.Parameters.AddWithValue("@BankBranch", txtBranchName.Text.Trim());
                            cmd.Parameters.AddWithValue("@BankBranchAddress", txtBranchAddress.Text.Trim());
                            cmd.Parameters.AddWithValue("@IPAddress", Request.UserHostAddress);
                            

                            con.Open();
                            int i = cmd.ExecuteNonQuery();
                            con.Close();

                            string script = $@"
                            <script type='text/javascript'>
                                alert('Bank Details has been updated successfully!');
                                window.location.href = 'Profile.aspx';
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