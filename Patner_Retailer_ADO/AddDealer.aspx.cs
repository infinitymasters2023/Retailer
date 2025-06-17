using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Text;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace Patner_Retailer_ADO
{
    public partial class AddDealer : System.Web.UI.Page
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
                    BindDealerDetails(decoded);
                    btnSubmit.Visible = false;
                    btnEdit.Visible = true;
                    hdrtext.InnerText = "Edit Dealer Details";
                }
            }
        }

        protected void BindDealerDetails(string decoded)
        {
            try
            {
                if (string.IsNullOrEmpty(decoded))
                    return;
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 27);
                    cmd.Parameters.AddWithValue("@Mid", decoded);
                    cmd.Parameters.AddWithValue("@UserRole", Session["Role"]?.ToString() ?? "");

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        txtSellerName.Text = dr["SellerName"] != DBNull.Value ? dr["SellerName"].ToString() : "";
                        txtGSTIN.Text = dr["SellerGSTINNo"] != DBNull.Value ? dr["SellerGSTINNo"].ToString() : "";
                        txtSellerPincode.Text = dr["PINCode"] != DBNull.Value ? dr["PINCode"].ToString() : "";
                        lblSellerCity.Text = dr["City"] != DBNull.Value ? dr["City"].ToString() : "";
                        lblSellerState.Text = dr["State"] != DBNull.Value ? dr["State"].ToString() : "";
                        txtSellerAddressLine1.Text = dr["AddressLine1"] != DBNull.Value ? dr["AddressLine1"].ToString() : "";
                        txtSellerAddressLine2.Text = dr["AddressLine2"] != DBNull.Value ? dr["AddressLine2"].ToString() : "";
                        txtSellerLandmark.Text = dr["LandMark"] != DBNull.Value ? dr["LandMark"].ToString() : "";
                    }
                    else
                    {
                        txtSellerName.Text = "";
                        txtGSTIN.Text = "";
                        txtSellerPincode.Text = "";
                        lblSellerCity.Text = "";
                        lblSellerState.Text = "";
                        txtSellerAddressLine1.Text = "";
                        txtSellerAddressLine2.Text = "";
                        txtSellerLandmark.Text = "";
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
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
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        protected void btnAddDealer_Click(object sender, EventArgs e)
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
                    cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString().Trim());

                    con.Open();
                    object result = cmd.ExecuteScalar();
                    con.Close();

                    if (result != null && result.ToString() == "1")
                    {
                        string script = @"<script type='text/javascript'>
                                            alert('Dealer Details has been saved successfully!');
                                            window.location.href = 'Profile.aspx';
                                        </script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ProfileRedirect", script);
                    }
                    else
                    {
                        DisplayMessage(this, "Dealer already exists with the same GSTIN and name.");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }

        }

        protected void btnEditDealer_Click(object sender, EventArgs e)
        {
            try
            {
                string qu = Request.QueryString["qu"];
                if (!string.IsNullOrWhiteSpace(qu))
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
                        return;
                    }
                    if (!string.IsNullOrWhiteSpace(txtSellerName.Text) && (txtSellerName.Text.Length >= 3) && !string.IsNullOrWhiteSpace(txtGSTIN.Text) && !string.IsNullOrWhiteSpace(txtSellerPincode.Text) &&
                        !string.IsNullOrWhiteSpace(txtSellerAddressLine1.Text) && !string.IsNullOrWhiteSpace(txtSellerLandmark.Text))
                    {
                        string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(qu));
                        SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Type", 28);
                        cmd.Parameters.AddWithValue("@Mid", decoded);
                        cmd.Parameters.AddWithValue("@CustomerName", txtSellerName.Text.Trim());
                        cmd.Parameters.AddWithValue("@SellerGSTINNo", txtGSTIN.Text.Trim());
                        cmd.Parameters.AddWithValue("@AddressLine1", txtSellerAddressLine1.Text.Trim());
                        cmd.Parameters.AddWithValue("@AddressLine2", txtSellerAddressLine2.Text.Trim());
                        cmd.Parameters.AddWithValue("@Landmark", txtSellerLandmark.Text.Trim());
                        cmd.Parameters.AddWithValue("@Pincode", txtSellerPincode.Text.Trim());
                        cmd.Parameters.AddWithValue("@City", lblSellerCity.Text.Trim());
                        cmd.Parameters.AddWithValue("@State", lblSellerState.Text.Trim());
                     
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        con.Close();

                        if (result != null && result.ToString() == "1")
                        {
                            string script = @"<script type='text/javascript'>
                                                alert('Dealer Details has been updated successfully!');
                                                window.location.href = 'Profile.aspx';
                                             </script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ProfileRedirect", script);
                        }
                        else
                        {
                            DisplayMessage(this, "Dealer already exists with the same GSTIN and name.");
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