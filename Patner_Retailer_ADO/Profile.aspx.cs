using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
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
                if (Session["HiddenMessages"] != null)
                {
                    hdrMessage.Visible = false;
                }
                SalesPersonPersonalInfoPanel.Visible = false;
                RetailerPersonalInfoPanel.Visible = false;
                CalendarExtender3.EndDate = DateTime.Now.AddYears(-19);
                if (Session["Role"].ToString() == "Admin")
                {
                    RetailerPersonalInfoPanel.Visible = true;
                    LoadProfileData();
                }
                else
                {
                    SalesPersonPersonalInfoPanel.Visible = true;
                    LoadSalesPersonProfileData();
                }
                BindBankDetails();
                BindUploadedDocuments();
                BindCommisionDetails();
                BindAccountHistory();

                btnPersonalInformation_Click(sender, e);
                CheckTheStatusMessage();

                string value = Request.QueryString["qu"];
                if(value =="Bank")
                {
                    btnBandDetailsView_Click(sender, e);
                }
                if (value == "Document")
                {
                    btnUploadedDocumentListView_Click(sender, e);
                }
            }
        }

        protected void CheckTheStatusMessage()
        {
            ApprovedWithdrawApplication.Visible = false;
            TerminateApplication.Visible = false;
            PendingWithdrawApplicationMessage.Visible = false;
            AccountRejectMessage.Visible = false;
            AccountMoreDocumentRequired.Visible = false;
            AccountPendingMessage.Visible = false;
            AccountApprovedMessage.Visible = false;
            btnWithdrawProfile.Visible = false;
            btnCancelWithdrawProfile.Visible = false;

            if (Session["Status"].ToString() == "1")
            {
                AccountPendingMessage.Visible = true;
                btnWithdrawProfile.Visible = true;
            }
           else if (Session["Status"].ToString() == "2")
            {
                AccountPendingMessage.Visible = true;
                btnWithdrawProfile.Visible = true;
            }
           else if (Session["Status"].ToString() == "3")
            {
                AccountPendingMessage.Visible = true;
                btnWithdrawProfile.Visible = true;
            }
            else if (Session["Status"].ToString() == "4")
            {
                AccountMoreDocumentRequired.Visible = true;
                //btnWithdrawProfile.Visible = true;
            }
            else if (Session["Status"].ToString() == "6")
            {
                AccountRejectMessage.Visible = true;
                //btnWithdrawProfile.Visible = true;
            }
            else if (Session["Status"].ToString() == "7")
            {
                PendingWithdrawApplicationMessage.Visible = true;
                btnCancelWithdrawProfile.Visible = true;
            }
            else if (Session["Status"].ToString() == "8")
            {
                TerminateApplication.Visible = true;
                //btnWithdrawProfile.Visible = true;
            }
            else if (Session["Status"].ToString() == "9")
            {
                ApprovedWithdrawApplication.Visible = true;
                //btnWithdrawProfile.Visible = true;
            }
            else
            {
                AccountApprovedMessage.Visible = true;
                btnWithdrawProfile.Visible = true;
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
                lblNameValue.Text = dr["SellerName"].ToString();
                lblGSTINValue.Text = dr["SellerGSTINNo"].ToString();
                lblCEONameValue.Text = dr["Name"].ToString();
                lblFirmTypeValue.Text = dr["FirmType"].ToString();
                lblMobileNoValue.Text = dr["MobileNo"].ToString();
                lblAlternateMobileNoValue.Text = dr["MobileNo_2"].ToString();
                lblEmailValue.Text = dr["emailID"].ToString();
                lblAltEmailValue.Text = dr["emailID_2"].ToString();
                lblDOBValue.Text = dr["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateOfBirth"]).ToString("dd-MMM-yyyy") : null;
                lblGenderValue.Text = dr["Gender"].ToString();
                lblWhatsAppNoValue.Text = dr["MobileNo_CheckWhatsapp"].ToString();
                lblWhatsAppNo2Value.Text = dr["MobileNo2_CheckWhatsapp"].ToString();
                lblPincodeValue.Text = dr["perPincode"].ToString();
                lblCityValue.Text = dr["perCity"].ToString();
                lblStateValue.Text = dr["perState"].ToString();
                lblAddressValue.Text = dr["perAddressLine1"].ToString();
                lblAddress2Value.Text = dr["perAddressLine2"].ToString();
                lblLandMarkValue.Text = dr["perLandmark"].ToString();

                lblDealerPincodeValue.Text = dr["DealerPincode"].ToString();
                lblDealerCityValue.Text = dr["DealerCity"].ToString();
                lblDealerStateValue.Text = dr["DealerState"].ToString();
                lblDealerAddressLine1Value.Text = dr["DealerAddressLine1"].ToString();
                lblDealerAddressLine2Value.Text = dr["DealerAddressLine2"].ToString();
                lblDealerLandmarkValue.Text = dr["DealerLandmark"].ToString();

                imgProfile.ImageUrl = dr["ProfileImage"] != DBNull.Value ? "../uploadeddocuments/" + dr["ProfileImage"].ToString() : "../assets/images/avatar5.png";
                imgCompanyLogo.ImageUrl = dr["CompanyLogoImage"] != DBNull.Value ? "../uploadeddocuments/" + dr["CompanyLogoImage"].ToString() : "../assets/images/avatar5.png";
                //lblWhatsappNoValue.Text = dr["WhatsappMobileNo"].ToString();

            }
            dr.Close();
            con.Close();
        }
        private void LoadSalesPersonProfileData()
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
                lblSalesPersonNameValue.Text = dr["Name"].ToString();
                lblSalesPersonMobileNoValue.Text = dr["MobileNo"].ToString();
                lblSalesPersonAltMobileNoValue.Text = dr["MobileNo_2"].ToString();
                lblSalesPersonEmailValue.Text = dr["EmailID"].ToString();
                lblSalesPersonAltEmailValue.Text = dr["EmailID2"].ToString();
                lblSalesPersonDOBValue.Text = dr["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateOfBirth"]).ToString("dd-MMM-yyyy") : null;
                lblSalesPersoGenderValue.Text = dr["Gender"].ToString();
                lblSalesPersonWhatsapp1Value.Text = dr["MobileNo_CheckWhatsapp"].ToString();
                lblSalesPersonWhatsap2Value.Text = dr["MobileNo2_CheckWhatsapp"].ToString();
                lblSalesPersonPincodeValue.Text = dr["Pincode"].ToString();
                lblSalesPersonCityValue.Text = dr["City"].ToString();
                lblSalesPersonStateValue.Text = dr["State"].ToString();
                lblSalesPersonAddressValue.Text = dr["Address"].ToString();
                lblSalesPersonAdminNameValue.Text = dr["AdminName"].ToString();
                lblSalesPersonAdminEmailValue.Text = dr["AdminEmail"].ToString();
                lblSalesPersonAdminPhoneNoValue.Text = dr["AdminMobile"].ToString();
                imgSalesPerson.ImageUrl = dr["ProfileImage"] != DBNull.Value ? "../uploadeddocuments/" + dr["ProfileImage"].ToString() : "../assets/images/avatar5.png";
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
                lblNoBankDetails.Visible = false;
            }
            else
            {
                RepeaterBankDetails.DataSource = null;
                RepeaterBankDetails.DataBind();
                lblNoBankDetails.Visible = true;
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
                    lblNoDocuments.Visible = false;
                }
                else
                {
                    rptDocuments.DataSource = null;
                    rptDocuments.DataBind();
                    lblNoDocuments.Visible = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        //private void BindDealer()
        //{
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Type", 20);
        //        cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString());
        //        cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());

        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {

        //            RepeaterEmployeeDetails.DataSource = dt;
        //            RepeaterEmployeeDetails.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

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
                Response.Redirect($"AddBank.aspx?qu=" + HttpUtility.UrlEncode(bankId));
            }
            else if (e.CommandName == "DeleteBank")
            {
                string mid = e.CommandArgument.ToString();
                string status = DataBinder.Eval(e.Item.DataItem, "Status")?.ToString();
                string statusFromCell = ((System.Web.UI.WebControls.Literal)e.Item.FindControl("litStatus"))?.Text;
                System.Web.UI.WebControls.Label lblStatus = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblStatus");
                if (lblStatus != null)
                {
                    status = lblStatus.Text;
                }
                if (status == "Primary")
                {
                    DisplayMessage(this, "This account is set as primary and cannot be deleted.");
                    return;
                }
                DeleteBankRecord(mid);
            }
            else if (e.CommandName == "ViewDocument")
            {
                string documentPath = e.CommandArgument.ToString();
                if (!string.IsNullOrWhiteSpace(documentPath))
                {
                    string fullPath = Server.MapPath("~/UploadedDocuments/" + documentPath);

                    if (System.IO.File.Exists(fullPath))
                    {
                        string domain = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                        string url = domain + ResolveUrl("~/UploadedDocuments/" + documentPath);
                        //Response.Redirect(url);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "openDocument", $"window.open('{url}', '_blank');", true);
                    }
                }
            }
            else if (e.CommandName == "MakeActive")
            {
                string mid = e.CommandArgument.ToString();
                MakeActiveBankRecord(mid);
            }
        }

        private void DeleteBankRecord(string mid)
        {
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 26);
                cmd.Parameters.AddWithValue("@Mid", mid);
                cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            BindBankDetails();
            DisplayMessage(this, "Bank record deleted successfully.");
        }
        private void MakeActiveBankRecord(string mid)
        {
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 36);
                cmd.Parameters.AddWithValue("@Mid", mid);
                cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString());
                cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            BindBankDetails();
            DisplayMessage(this, "Selected bank account has been set as Primary successfully.");
        }
        //protected void btnAddDealer_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("AddDealer.aspx");
        //}

        /*protected void RepeaterDealerDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
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

            //BindDealer();
            DisplayMessage(this, "Dealer record deleted successfully.");
        }*/

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
            else if (e.CommandName == "ViewDocument")
            {
                string documentPath = e.CommandArgument.ToString();
                if (!string.IsNullOrWhiteSpace(documentPath))
                {
                    string fullPath = Server.MapPath("~/UploadedDocuments/" + documentPath);

                    if (System.IO.File.Exists(fullPath))
                    {
                        string domain = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                        string url = domain + ResolveUrl("~/UploadedDocuments/" + documentPath);
                        //Response.Redirect(url);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "openDocument", $"window.open('{url}', '_blank');", true);
                    }
                }
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
            btnWithdrawProfile.Visible = false;

            BindEditValue();
        }
        protected void BindEditValue()
        {
            lblNameValue.Visible = false;
            lblGSTINValue.Visible = false;
            lblCEONameValue.Visible = false;
            lblFirmTypeValue.Visible = false;
            lblMobileNoValue.Visible = false;
            lblAlternateMobileNoValue.Visible = false;
            lblEmailValue.Visible = false;
            lblAltEmailValue.Visible = false;
            lblDOBValue.Visible = false;
            lblGenderValue.Visible = false;
            lblWhatsAppNoValue.Visible = false;
            lblWhatsAppNo2Value.Visible = false;
            lblPincodeValue.Visible = false;
            lblAddressValue.Visible = false;
            lblAddress2Value.Visible = false;
            lblLandMarkValue.Visible = false;
            lblDealerPincodeValue.Visible = false;
            lblDealerAddressLine1Value.Visible = false;
            lblDealerAddressLine2Value.Visible = false;
            lblDealerLandmarkValue.Visible = false;
            imgProfile.Visible = false;
            imgCompanyLogo.Visible = false;

            TextBox1.Attributes.Add("ReadOnly", "readonly");

            txtFirstName.Visible = true;
            txtGSTIN.Visible = true;
            txtCEOName.Visible = true;
            ddlFirmType.Visible = true;
            txtMobileNumber.Visible = true;
            txtAlternateMobile.Visible = true;
            txtEmail.Visible = true;
            txtAltEmail.Visible = true;
            TextBox1.Visible = true;
            ddlGender.Visible = true;
            txtWhatsAppNo.Visible = true;
            txtWhatsAppNo2.Visible = true;
            txtPinCode.Visible = true;
            txtAddress.Visible = true;
            txtAddress2.Visible = true;
            txtLandmark.Visible = true;
            txtDealerPincode.Visible = true;
            txtDealerAddressLine1.Visible = true;
            txtDealerAddressLine2.Visible = true;
            txtDealerLandmark.Visible = true;
            fuCompanyLogo.Visible = true;
            fuProfileImage.Visible = true;

            txtFirstName.Text = lblNameValue.Text;
            txtGSTIN.Text = lblGSTINValue.Text;
            txtCEOName.Text = lblCEONameValue.Text;
            ddlFirmType.SelectedValue = lblFirmTypeValue.Text;
            txtMobileNumber.Text = lblMobileNoValue.Text;
            txtAlternateMobile.Text = lblAlternateMobileNoValue.Text;
            txtEmail.Text = lblEmailValue.Text;
            txtAltEmail.Text = lblAltEmailValue.Text;
            TextBox1.Text = lblDOBValue.Text;
            ddlGender.SelectedValue = lblGenderValue.Text.Contains("Select") ? null : lblGenderValue.Text;
            txtWhatsAppNo.Text = lblWhatsAppNoValue.Text;
            txtWhatsAppNo2.Text = lblWhatsAppNo2Value.Text;
            txtPinCode.Text = lblPincodeValue.Text;
            txtAddress.Text = lblAddressValue.Text;
            txtAddress2.Text = lblAddress2Value.Text;
            txtLandmark.Text = lblLandMarkValue.Text;
            txtDealerPincode.Text = lblDealerPincodeValue.Text;
            txtDealerAddressLine1.Text = lblDealerAddressLine1Value.Text;
            txtDealerAddressLine2.Text = lblDealerAddressLine2Value.Text;
            txtDealerLandmark.Text = lblDealerLandmarkValue.Text;


        }
        protected void btnCancelProfile_Click(object sender, EventArgs e)
        {
            btnEditProfile.Visible = true;
            btnCancelProfile.Visible = false;
            btnUpdateProfile.Visible = false;
            btnWithdrawProfile.Visible = true;

            lblNameValue.Visible = true;
            lblGSTINValue.Visible = true;
            lblCEONameValue.Visible = true;
            lblFirmTypeValue.Visible = true;
            lblMobileNoValue.Visible = true;
            lblAlternateMobileNoValue.Visible = true;
            lblEmailValue.Visible = true;
            lblAltEmailValue.Visible = true;
            lblDOBValue.Visible = true;
            lblGenderValue.Visible = true;
            lblWhatsAppNoValue.Visible = true;
            lblWhatsAppNo2Value.Visible = true;
            lblPincodeValue.Visible = true;
            lblAddressValue.Visible = true;
            lblAddress2Value.Visible = true;
            lblLandMarkValue.Visible = true;
            lblDealerPincodeValue.Visible = true;
            lblDealerAddressLine1Value.Visible = true;
            lblDealerAddressLine2Value.Visible = true;
            lblDealerLandmarkValue.Visible = true;
            imgProfile.Visible = true;
            imgCompanyLogo.Visible = true;

            txtFirstName.Visible = false;
            txtGSTIN.Visible = false;
            txtCEOName.Visible = false;
            ddlFirmType.Visible = false;
            txtMobileNumber.Visible = false;
            txtAlternateMobile.Visible = false;
            txtEmail.Visible = false;
            txtAltEmail.Visible = false;
            TextBox1.Visible = false;
            ddlGender.Visible = false;
            txtWhatsAppNo.Visible = false;
            txtWhatsAppNo2.Visible = false;
            txtPinCode.Visible = false;
            txtAddress.Visible = false;
            txtAddress2.Visible = false;
            txtLandmark.Visible = false;
            txtDealerPincode.Visible = false;
            txtDealerAddressLine1.Visible = false;
            txtDealerAddressLine2.Visible = false;
            txtDealerLandmark.Visible = false;
            fuProfileImage.Visible = false;
            fuCompanyLogo.Visible = false;
        }
        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;

                if (string.IsNullOrEmpty(txtDealerLandmark.Text))
                {
                    lblDealerLandmarkError.Visible = true;
                    txtDealerLandmark.Focus();
                    count++;
                }
                if (txtDealerLandmark.Text.Trim().Length < 3)
                {
                    lblDealerLandmarkError.Visible = true;
                    lblDealerLandmarkError.InnerText = "Corporate / Main Office Landmark must be at least 3 characters.";
                    txtDealerLandmark.Focus();
                    count++;
                }
                else { lblDealerLandmarkError.Visible = false; }

                if (string.IsNullOrEmpty(txtDealerAddressLine1.Text))
                {
                    lblDealerAddressLine1Error.Visible = true;
                    txtDealerAddressLine1.Focus();
                    count++;
                }
                else
                {
                    var words = txtDealerAddressLine1.Text.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length < 3)
                    {
                        lblDealerAddressLine1Error.Visible = true;
                        lblDealerAddressLine1Error.InnerText = "Corporate / Main Office Address must contain at least 3 words.";
                        txtDealerAddressLine1.Focus();
                        count++;
                    }
                    else
                    {
                        lblDealerAddressLine1Error.Visible = false;
                    }
                }
                if (string.IsNullOrWhiteSpace(txtDealerPincode.Text))
                {
                    lblDealerPincodeError.Visible = true;
                    txtDealerPincode.Focus();
                    count++;
                }
                else if (txtDealerPincode.Text.Length != 6)
                {
                    lblDealerPincodeError.Visible= true;
                    lblDealerPincodeError.InnerText = "Invalid Pincode";
                    txtDealerPincode.Focus();
                    count++;
                }
                else
                {
                    lblDealerPincodeError.Visible = false;
                }

                if (string.IsNullOrEmpty(txtLandmark.Text))
                {
                    lblLandMarkError.Visible = true;
                    txtLandmark.Focus();
                    count++;
                }
                if (txtLandmark.Text.Trim().Length < 3)
                {
                    lblLandMarkError.Visible = true;
                    lblLandMarkError.InnerText = "Corporate / Main Office Landmark must be at least 3 characters.";
                    txtLandmark.Focus();
                    count++;
                }
                else { lblLandMarkError.Visible = false; }

                if (string.IsNullOrEmpty(txtAddress.Text))
                {
                    lblCurrentAddress.Visible = true;
                    txtAddress.Focus();
                    count++;
                }
                else
                {
                    var words = txtAddress.Text.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length < 3)
                    {
                        lblCurrentAddress.Visible = true;
                        lblCurrentAddress.InnerText = "Corporate / Main Office Address must contain at least 3 words.";
                        txtAddress.Focus();
                        count++;
                    }
                    else
                    {
                        lblCurrentAddress.Visible = false;
                    }
                }
                if (string.IsNullOrWhiteSpace(txtPinCode.Text))
                {
                    lblErrorPincode.Visible = true;
                    txtPinCode.Focus(); 
                    count++;
                }
                else if (txtPinCode.Text.Length != 6)
                {
                    lblErrorPincode.Visible = true;
                    lblErrorPincode.InnerText = "Invalid Pincode";
                    txtPinCode.Focus();
                    count++;
                }
                else
                {
                    lblErrorPincode.Visible = false;
                }

                if (!string.IsNullOrWhiteSpace(txtWhatsAppNo2.Text) && txtWhatsAppNo2.Text.Length != 10)
                {
                    lblhdnWhatsAppNo2Error.Visible = true;
                    txtWhatsAppNo2.Focus();
                    count++;
                }
                else
                {
                    lblhdnWhatsAppNo2Error.Visible= false;
                }
                if (string.IsNullOrWhiteSpace(txtWhatsAppNo.Text) || txtWhatsAppNo.Text.Length != 10)
                {
                    if (txtWhatsAppNo.Text.Length != 10)
                        lblhdnWhatsAppNo2Error.InnerText = "Invalid WhatsApp Mobile No 1.";
                    lblhdnWhatsAppNo1Error.Visible = true;
                    txtWhatsAppNo.Focus();
                    count++;
                }
                else
                {
                    lblhdnWhatsAppNo2Error.Visible = false;
                }
                string wamobile = txtWhatsAppNo2.Text.Trim();
                string altwamobile = txtWhatsAppNo.Text.Trim();

                if (!string.IsNullOrEmpty(wamobile) && wamobile == altwamobile)
                {
                    lblhdnWhatsAppNo1Error.InnerText = "WhatsApp Mobile No and alternate WhatsApp mobile numbers cannot be the same.";
                    lblhdnWhatsAppNo1Error.Visible = true;
                    txtWhatsAppNo.Focus();
                    count++;
                }
                if (!string.IsNullOrWhiteSpace(txtAltEmail.Text) && !Regex.IsMatch(txtAltEmail.Text.Trim(), @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    lblAltEmailError.Visible = true;
                    lblAltEmailError.InnerText = "Invalid Alternate Email ID.";
                    txtAltEmail.Focus();
                    count++;
                }
                else
                {
                    lblAltEmailError.Visible = false;
                }

                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    lblEmailAddress.Visible = true;
                    txtEmail.Focus();
                    count++;
                }
                else if(!Regex.IsMatch(txtEmail.Text.Trim(), @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    lblEmailAddress.Visible = true;
                    lblEmailAddress.InnerText = "Invalid Email ID.";
                    txtEmail.Focus();
                    count++;
                }
                string email = txtEmail.Text.Trim();
                string altemail = txtAltEmail.Text.Trim();

                if (!string.IsNullOrEmpty(email) && email == altemail)
                {
                    lblEmailAddress.InnerText = "Email ID and alternate Email ID cannot be the same.";
                    lblEmailAddress.Visible = true;
                    txtEmail.Focus();
                    count++;
                }
                else { lblEmailAddress.Visible = false; }


                if (!string.IsNullOrWhiteSpace(txtAlternateMobile.Text) && txtAlternateMobile.Text.Length != 10)
                {
                    lblAlternateMobileNoError.Visible = true;
                    txtAlternateMobile.Focus();
                    count++;
                }
                string mobile = txtMobileNumber.Text.Trim();
                string altMobile = txtAlternateMobile.Text.Trim();

                if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
                {
                    lblAlternateMobileNoError.InnerText = "Mobile No and alternate mobile numbers cannot be the same.";
                    lblAlternateMobileNoError.Visible = true;
                    txtAlternateMobile.Focus();
                    count++;
                }
                else
                {
                    lblAlternateMobileNoError.Visible = false;
                }

                if (string.IsNullOrWhiteSpace(txtMobileNumber.Text))
                {
                    lblErrorMobileNo.Visible = true;
                    txtMobileNumber.Focus();
                    count++;
                }
                else if (txtMobileNumber.Text.Length != 10)
                {
                    lblErrorMobileNo.Visible = true;
                    lblErrorMobileNo.InnerText = "Invalid Mobile No.";
                    txtMobileNumber.Focus();
                    count++;
                }
                else { lblErrorMobileNo.Visible = false; }

                if (ddlFirmType.SelectedValue == "" || ddlFirmType.SelectedValue == "Select")
                {
                    lblFirmTypeEror.Visible = true;
                    ddlFirmType.Focus();
                    count++;
                }
                else
                {
                    lblFirmTypeEror.Visible= false;
                }

                if (!string.IsNullOrWhiteSpace(txtCEOName.Text) && txtCEOName.Text.Length < 3)
                {
                    lblCEONameEror.Visible = true;
                    lblCEONameEror.InnerText = "First Name must be between 3 and 30 characters.";
                    txtCEOName.Focus();
                    count++;
                }
                if (string.IsNullOrWhiteSpace(txtCEOName.Text))
                {
                    lblCEONameEror.Visible = true;
                    txtCEOName.Focus();
                    count++;
                }
                else
                {
                    lblCEONameEror.Visible = false;
                }
                if (string.IsNullOrWhiteSpace(txtGSTIN.Text.Trim()))
                {
                    lblGSTINError.Visible = true;
                    lblGSTINError.InnerText = "GSTIN is required.";
                    txtGSTIN.Focus();
                    count++;
                }
                else if (!Regex.IsMatch(txtGSTIN.Text.Trim(), @"^\d{2}[A-Z]{5}\d{4}[A-Z]{1}[A-Z\d]{1}Z[A-Z\d]{1}$"))
                {
                    lblGSTINError.Visible = true;
                    lblGSTINError.InnerText = "Please enter a valid 15-character GSTIN.";
                    txtGSTIN.Focus();
                    count++;
                }
                else
                {
                    lblGSTINError.Visible = false;
                }

                if (!string.IsNullOrWhiteSpace(txtFirstName.Text) && txtFirstName.Text.Length < 3)
                {
                    lblFullName.Visible = true;
                    lblFullName.InnerText = "Legal Name as per GSTIN must be between 3 and 30 characters.";
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
                    string profileImg = imgProfile.ImageUrl.Replace("/UploadedDocuments/", "");
                    string companyLogo = imgCompanyLogo.ImageUrl.Replace("../uploadeddocuments/", "");
                    if (fuProfileImage.HasFile)
                    {
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                        string fileExtension = Path.GetExtension(fuProfileImage.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            return;
                        }
                        string fileName = Path.GetFileName(fuProfileImage.FileName);
                        string folderPath = Server.MapPath("~/UploadedDocuments/");
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        string uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName;
                        string fullPath = Path.Combine(folderPath, uniqueFileName);
                        fuProfileImage.SaveAs(fullPath);
                        profileImg = uniqueFileName;
                    }
                    if (fuCompanyLogo.HasFile)
                    {
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                        string fileExtension = Path.GetExtension(fuCompanyLogo.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            return;
                        }
                        string fileName = Path.GetFileName(fuCompanyLogo.FileName);
                        string folderPath = Server.MapPath("~/UploadedDocuments/");
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        string uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName;
                        string fullPath = Path.Combine(folderPath, uniqueFileName);
                        fuCompanyLogo.SaveAs(fullPath);
                        companyLogo = uniqueFileName;
                    }
                    if (Session["RetailerUniqueID"] != null)
                    {
                        SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Type", 33);
                        cmd.Parameters.AddWithValue("@Mid", Session["RetailerUniqueID"].ToString());
                        cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());

                        cmd.Parameters.AddWithValue("@SellerName", txtFirstName.Text.ToUpper().Trim());
                        cmd.Parameters.AddWithValue("@SellerGSTINNo", txtGSTIN.Text.ToUpper().Trim());
                        cmd.Parameters.AddWithValue("@CustomerName", txtCEOName.Text.Trim());
                        cmd.Parameters.AddWithValue("@FirmType", ddlFirmType.SelectedValue);
                        cmd.Parameters.AddWithValue("@CustomerMobileNo", txtMobileNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@AlternativeMobile", txtAlternateMobile.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerEmailID", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@AlternativeEmail", txtAltEmail.Text.Trim());
                        DateTime dob;
                        if (DateTime.TryParse(TextBox1.Text.Trim(), out dob))
                            cmd.Parameters.AddWithValue("@DateOfBirth", dob.Date);
                        else
                            cmd.Parameters.AddWithValue("@DateOfBirth", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@WhatsappNo", txtWhatsAppNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@AltWhatsappNo", txtWhatsAppNo2.Text.Trim());
                        cmd.Parameters.AddWithValue("@Pincode", txtPinCode.Text.Trim());
                        cmd.Parameters.AddWithValue("@City", lblCityValue.Text.Trim());
                        cmd.Parameters.AddWithValue("@State", lblStateValue.Text.Trim());
                        cmd.Parameters.AddWithValue("@AddressLine1", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@AddressLine2", txtAddress2.Text.Trim());
                        cmd.Parameters.AddWithValue("@Landmark", txtLandmark.Text.Trim());
                        cmd.Parameters.AddWithValue("@DealerPincode", txtDealerPincode.Text.Trim());
                        cmd.Parameters.AddWithValue("@DealerCity", lblDealerCityValue.Text.Trim());
                        cmd.Parameters.AddWithValue("@DealerState", lblDealerStateValue.Text.Trim());
                        cmd.Parameters.AddWithValue("@DealerAddressLine1", txtDealerAddressLine1.Text.Trim());
                        cmd.Parameters.AddWithValue("@DealerAddressLine2", txtDealerAddressLine2.Text.Trim());
                        cmd.Parameters.AddWithValue("@DealerLandmark", txtDealerLandmark.Text.Trim());
                        cmd.Parameters.AddWithValue("@CompanyLogoImage", companyLogo);
                        cmd.Parameters.AddWithValue("@ProfileImage", profileImg);

                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        con.Close();

                        btnEditProfile.Visible = true;
                        btnWithdrawProfile.Visible = true;
                        btnCancelProfile.Visible = false;
                        btnUpdateProfile.Visible = false;

                        LoadProfileData();
                        btnCancelProfile_Click(sender, e);
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
        protected string MaskDocumentNumber(object docNumberObj)
        {
            string docNumber = docNumberObj?.ToString();
            if (!string.IsNullOrEmpty(docNumber) && docNumber.Length > 4)
            {
                string masked = new string('*', docNumber.Length - 4) + docNumber.Substring(docNumber.Length - 4);
                return masked;
            }
            return docNumber; // Return as-is if null, empty, or too short
        }

        private void BindCommisionDetails()
        {
            SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@type", 83);
            cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString());
            cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                rptCommissionDetails.DataSource = dt;
                rptCommissionDetails.DataBind();
                lblNoCommission.Visible = false;
            }
            else
            {
                rptCommissionDetails.DataSource = null;
                rptCommissionDetails.DataBind();
                lblNoCommission.Visible = true;
            }
            con.Close();
        }
        protected void txtAltCustomerMobile_TextChanged(object sender, EventArgs e)
        {
            string blockuser = CheckBlockCustomer("", txtAlternateMobile.Text);
            string existuser = CheckSalesPersonOrRetailer("", txtAlternateMobile.Text);
            if (!string.IsNullOrWhiteSpace(existuser))
            {
                lblAlternateMobileNoError.InnerText = "This Mobile No is already registered";
                btnUpdateProfile.Enabled = false;
                lblAlternateMobileNoError.Visible= true;
                return;
            }
            else if (!string.IsNullOrWhiteSpace(blockuser))
            {
                 lblAlternateMobileNoError.InnerText = blockuser;
                btnUpdateProfile.Enabled = false;
                lblAlternateMobileNoError.Visible= true;
                return;
            }
            if (!string.IsNullOrEmpty(txtAlternateMobile.Text) && txtAlternateMobile.Text.Replace("+91", "").Length != 10)
            {
                lblAlternateMobileNoError.InnerText = "Please enter your phone number.";
                lblAlternateMobileNoError.Visible = true;
                txtAlternateMobile.Focus();
                btnUpdateProfile.Enabled = false;
                return;
            }

            if (!string.IsNullOrEmpty(txtAlternateMobile.Text) && Regex.IsMatch(txtAlternateMobile.Text, @"^[0-5]"))
            {
                lblAlternateMobileNoError.InnerText = "Invalid number";
                lblAlternateMobileNoError.Visible = true;
                txtAlternateMobile.Focus();
                btnUpdateProfile.Enabled = false;
                return;
            }

            if (!string.IsNullOrEmpty(txtAlternateMobile.Text) && Regex.IsMatch(txtAlternateMobile.Text, @"^(\d)\1{9}$"))
            {
                lblAlternateMobileNoError.InnerText = "Invalid number.";
                lblAlternateMobileNoError.Visible = true;
                txtAlternateMobile.Focus();
                btnUpdateProfile.Enabled = false;
                return;
            }
            string mobile = txtMobileNumber.Text.Trim();
            string altMobile = txtAlternateMobile.Text.Trim();

            if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
            {
                lblAlternateMobileNoError.InnerText = "Mobile No and alternate mobile numbers cannot be the same.";
                lblAlternateMobileNoError.Visible = true;
                txtAlternateMobile.Focus();
                btnUpdateProfile.Enabled = false;
                return;
            }
            else { lblAlternateMobileNoError.Visible = false; }
            btnUpdateProfile.Enabled = true;
            lblAlternateMobileNoError.InnerText = "";
            lblAlternateMobileNoError.Visible = false;

        }
        protected void txtWhatsAppCustomerMobile_TextChanged(object sender, EventArgs e)
        {
            string blockuser = CheckBlockCustomer("", txtWhatsAppNo.Text);
            string existuser = CheckSalesPersonOrRetailer("", txtWhatsAppNo.Text);
            if (!string.IsNullOrWhiteSpace(existuser))
            {
                lblhdnWhatsAppNo1Error.InnerText = "This Mobile No is already registered";
                btnUpdateProfile.Enabled = false;
                lblAlternateMobileNoError.Visible = true;
                return;
            }
            else if (!string.IsNullOrWhiteSpace(blockuser))
            {
                lblhdnWhatsAppNo1Error.InnerText = blockuser;
                btnUpdateProfile.Enabled = false;
                lblhdnWhatsAppNo1Error.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(txtWhatsAppNo.Text) && txtWhatsAppNo.Text.Replace("+91", "").Length != 10)
            {
                lblhdnWhatsAppNo1Error.InnerText = "Please enter your phone number.";
                lblhdnWhatsAppNo1Error.Visible = true;
                txtWhatsAppNo.Focus();
                btnUpdateProfile.Enabled = false;
                return;
            }

            if (!string.IsNullOrEmpty(txtWhatsAppNo.Text) && Regex.IsMatch(txtWhatsAppNo.Text, @"^[0-5]"))
            {
                lblhdnWhatsAppNo1Error.InnerText = "Invalid number";
                lblhdnWhatsAppNo1Error.Visible = true;
                txtWhatsAppNo.Focus();
                btnUpdateProfile.Enabled = false;
                return;
            }

            if (!string.IsNullOrEmpty(txtWhatsAppNo.Text) && Regex.IsMatch(txtWhatsAppNo.Text, @"^(\d)\1{9}$"))
            {
                lblhdnWhatsAppNo1Error.InnerText = "Invalid number.";
                lblhdnWhatsAppNo1Error.Visible = true;
                txtWhatsAppNo.Focus();
                btnUpdateProfile.Enabled = false;
                return;
            }
            string mobile = txtWhatsAppNo2.Text.Trim();
            string altMobile = txtWhatsAppNo.Text.Trim();

            if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
            {
                lblhdnWhatsAppNo1Error.InnerText = "WhatsApp Mobile No and alternate WhatsApp mobile numbers cannot be the same.";
                lblhdnWhatsAppNo1Error.Visible = true;
                txtWhatsAppNo.Focus();
                btnUpdateProfile.Enabled = false;
                return;
            }
            else { lblhdnWhatsAppNo1Error.Visible = false; }
            btnUpdateProfile.Enabled = true;
            lblhdnWhatsAppNo1Error.InnerText = "";
            lblhdnWhatsAppNo1Error.Visible = false;
            lblhdnWhatsAppNo2Error.InnerText = "";
            lblhdnWhatsAppNo2Error.Visible = false;
        }
        protected void txtWhatsAppAltCustomerMobile_TextChanged(object sender, EventArgs e)
        {
            string blockuser = CheckBlockCustomer("", txtWhatsAppNo2.Text);
            string existuser = CheckSalesPersonOrRetailer("", txtWhatsAppNo2.Text);
            if (!string.IsNullOrWhiteSpace(existuser))
            {
                lblhdnWhatsAppNo2Error.InnerText = "This Mobile No is already registered";
                btnUpdateProfile.Enabled = false;
                lblAlternateMobileNoError.Visible = true;
                return;
            }
            else if (!string.IsNullOrWhiteSpace(blockuser))
            {
                lblhdnWhatsAppNo2Error.InnerText = blockuser;
                btnUpdateProfile.Enabled = false;
                lblhdnWhatsAppNo2Error.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(txtWhatsAppNo2.Text) && txtWhatsAppNo2.Text.Replace("+91", "").Length != 10)
            {
                lblhdnWhatsAppNo2Error.InnerText = "Please enter your phone number.";
                lblhdnWhatsAppNo2Error.Visible = true;
                txtWhatsAppNo2.Focus();
                btnUpdateProfile.Enabled = false;
                return;
            }

            if (!string.IsNullOrEmpty(txtWhatsAppNo2.Text) && Regex.IsMatch(txtWhatsAppNo2.Text, @"^[0-5]"))
            {
                lblhdnWhatsAppNo2Error.InnerText = "Invalid number";
                lblhdnWhatsAppNo2Error.Visible = true;
                txtWhatsAppNo2.Focus();
                btnUpdateProfile.Enabled = false;
                return;
            }

            if (!string.IsNullOrEmpty(txtWhatsAppNo2.Text) && Regex.IsMatch(txtWhatsAppNo2.Text, @"^(\d)\1{9}$"))
            {
                lblhdnWhatsAppNo2Error.InnerText = "Invalid number.";
                lblhdnWhatsAppNo2Error.Visible = true;
                txtWhatsAppNo2.Focus();
                btnUpdateProfile.Enabled = false;
                return;
            }
            string mobile = txtWhatsAppNo2.Text.Trim();
            string altMobile = txtWhatsAppNo.Text.Trim();

            if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
            {
                lblhdnWhatsAppNo2Error.InnerText = "WhatsApp Mobile No and alternate WhatsApp mobile numbers cannot be the same.";
                lblhdnWhatsAppNo2Error.Visible = true;
                txtWhatsAppNo2.Focus();
                btnUpdateProfile.Enabled = false;
                return;
            }
            else { lblhdnWhatsAppNo2Error.Visible = false; }
            lblhdnWhatsAppNo2Error.InnerText = "";
            lblhdnWhatsAppNo2Error.Visible = false;
            lblhdnWhatsAppNo1Error.InnerText = "";
            lblhdnWhatsAppNo1Error.Visible = false;
            btnUpdateProfile.Enabled = true;
        }
        protected void txtCustomerEmail_TextChanged(object sender, EventArgs e)
        {
            string blockuser = CheckBlockCustomer(txtEmail.Text, "");
            string existuser = CheckSalesPersonOrRetailer(txtEmail.Text, "");
            if (!string.IsNullOrEmpty(existuser))
            {
                lblEmailAddress.InnerText = "This Email is already registered";
                btnUpdateProfile.Enabled = true;
                lblEmailAddress.Visible = true;
                txtEmail.Focus();
                return;
            }
            else if (!string.IsNullOrWhiteSpace(blockuser))
            {
                lblEmailAddress.InnerText = blockuser;
                btnUpdateProfile.Enabled = true;
                lblEmailAddress.Visible = true;
                txtEmail.Focus();
                return;
            }
            string email = txtEmail.Text.Trim();
            string altemail = txtAltEmail.Text.Trim();

            if (!string.IsNullOrEmpty(email) && email == altemail)
            {
                lblEmailAddress.InnerText = "Email ID and alternate Email ID cannot be the same.";
                lblEmailAddress.Visible = true;
                txtEmail.Focus();
                return;
            }
            lblEmailAddress.InnerText = "";
            lblAltEmailError.InnerText = "";
            btnUpdateProfile.Enabled = true;
            lblEmailAddress.Visible = false;
            txtAltEmail.Focus();
        }

        protected void txtCustomerAltEmail_TextChanged(object sender, EventArgs e)
        {
            string blockuser = CheckBlockCustomer(txtAltEmail.Text, "");
            string existuser = CheckSalesPersonOrRetailer(txtAltEmail.Text, "");
            if (!string.IsNullOrEmpty(existuser))
            {
                lblAltEmailError.InnerText = "This Email is already registered";
                btnUpdateProfile.Enabled = true;
                lblAltEmailError.Visible = true;
                txtAltEmail.Focus();
                return;
            }
            else if (!string.IsNullOrWhiteSpace(blockuser))
            {
                lblAltEmailError.InnerText = blockuser;
                btnUpdateProfile.Enabled = true;
                lblAltEmailError.Visible = true;
                txtAltEmail.Focus();
                return;
            }
            string mobile = txtEmail.Text.Trim();
            string altMobile = txtAltEmail.Text.Trim();

            if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
            {
                lblAltEmailError.InnerText = "Email ID and alternate Email ID cannot be the same.";
                lblAltEmailError.Visible = true;
                txtAltEmail.Focus();
                return;
            }
            lblAltEmailError.InnerText = "";
            lblEmailAddress.InnerText = "";
            btnUpdateProfile.Enabled = true;
            lblAltEmailError.Visible = false;
            TextBox1.Focus();
        }
        protected string CheckBlockCustomer(string email, string mobileno)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerMobileNo", SqlDbType.VarChar).Value = !string.IsNullOrWhiteSpace(mobileno) ? mobileno.ToString() : null;
                cmd.Parameters.AddWithValue("@CustomerEmailID", SqlDbType.VarChar).Value = !string.IsNullOrWhiteSpace(email) ? email.ToString() : null;
                cmd.Parameters.AddWithValue("@Mid", Session["RetailerUniqueID"].ToString());
                cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 39;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string suspicious = dt.Rows[0]["suspicious"].ToString();
                    if (suspicious == "1")
                    {
                        return "This Customer is Under Watch.Please contact your Manager";
                    }
                    else if (suspicious == "2")
                    {
                        return "This Customer is Black Listed..Please contact your Manager";
                    }
                    else if (suspicious == "")
                    {
                        return "";
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return "Something went wrong!";
            }
        }
        protected string CheckSalesPersonOrRetailer(string email, string mobileno)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerMobileNo", !string.IsNullOrWhiteSpace(mobileno) ? mobileno : null);
                    cmd.Parameters.AddWithValue("@CustomerEmailID", !string.IsNullOrWhiteSpace(email) ? email : null);
                    cmd.Parameters.AddWithValue("@Mid", Session["RetailerUniqueID"].ToString());
                    cmd.Parameters.AddWithValue("@type", 50);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return "This Mobile No or Email is already registered";
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong!";
            }
        }

        protected void btnWithdrawProfile_Click(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(txtRemarks.InnerText))
                {
                    txtRemarks.Focus();
                    DisplayMessage(this, "Remarks is Required");
                    return;
                }
                SqlCommand cmd1 = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@Type", 62);
                cmd1.Parameters.AddWithValue("@Status", "7");
                cmd1.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString().Trim());
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd1.ExecuteNonQuery();
                con.Close();

                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 63);
                cmd.Parameters.AddWithValue("@Retailer_MobileNo", Session["MobileNo"].ToString().Trim());
                cmd.Parameters.AddWithValue("@Status", "7");
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.InnerText);
                cmd.Parameters.AddWithValue("@CreatedBy", Session["Name"].ToString().Trim());
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                feedbackSection.Visible = false;
                txtRemarks.InnerText = "";
                txtRemarks.Value = "";
                Session["Status"] = "7";
                BindAccountHistory();
                CheckTheStatusMessage();
                DisplayMessage(this, "Your account withdrawal request has been submitted!");
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }
        protected void btnCancelWithdrawProfile_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtRemarks.InnerText))
                {
                    txtRemarks.Focus();
                    DisplayMessage(this, "Remarks is Required");
                    return;
                }
                SqlCommand cmd1 = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@Type", 25);
                cmd1.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString().Trim());
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd1.ExecuteNonQuery();
                con.Close();


                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 63);
                cmd.Parameters.AddWithValue("@Retailer_MobileNo", Session["MobileNo"].ToString().Trim());
                cmd.Parameters.AddWithValue("@Status", "3");
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.InnerText);
                cmd.Parameters.AddWithValue("@CreatedBy", Session["Name"].ToString().Trim());
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                feedbackSection.Visible = false;
                txtRemarks.InnerText = "";
                txtRemarks.Value = "";
                Session["Status"] = "3";
                BindAccountHistory();
                CheckTheStatusMessage();
                DisplayMessage(this, "Your account withdrawal request has been canceled. Please wait for verification.");
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
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
        }
        protected void btnCommisionDetailsView_Click(object sender, EventArgs e)
        {
            mvViewType.ActiveViewIndex = 3;
            btnCommisionDetailsView.Attributes["class"] = "btn btn-primary card-btn";
            btnPersonalInformationView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnBandDetailsView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnUploadedDocumentListView.Attributes["class"] = "btn btn-outline-primary card-btn";
            btnAccountHistoryView.Attributes["class"] = "btn btn-outline-primary card-btn";
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

        private void BindAccountHistory()
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@type", 64);
                    cmd.Parameters.AddWithValue("@Retailer_MobileNo", Session["MobileNo"].ToString().Trim());

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            lblCurruntStatus.Text = dt.Rows[0]["Status"].ToString();
                            GVAccountHistory.DataSource = dt;
                            GVAccountHistory.DataBind();
                           
                        }
                        else
                        {
                            GVAccountHistory.DataSource = null;
                            GVAccountHistory.DataBind();
                        }
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }
        protected void btnwithdrawalRequest_ServerClick(object sender, EventArgs e)
        {
            if (feedbackSection.Visible == true)
                feedbackSection.Visible = false;
            else
                feedbackSection.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            feedbackSection.Visible = false;
        }
        protected void hideMessageClick(object sender, EventArgs e)
        {
            ApprovedWithdrawApplication.Visible = false;
            TerminateApplication.Visible = false;
            PendingWithdrawApplicationMessage.Visible = false;
            AccountRejectMessage.Visible = false;
            AccountMoreDocumentRequired.Visible = false;
            AccountPendingMessage.Visible = false;
            AccountApprovedMessage.Visible = false;
            Session["HiddenMessages"] = "hideMessage";
        }

        protected void btnEditSalesPersonProfile_Click(object sender, EventArgs e)
        {
            btnSalesPersonEdit.Visible = false;
            btnSalesPersonCancel.Visible = true;
            btnSalesPersonSave.Visible = true;

            BindSalesPersonEditValue();
        }

        protected void BindSalesPersonEditValue()
        {
            lblSalesPersonNameValue.Visible = false;
            lblSalesPersonMobileNoValue.Visible = false;
            lblSalesPersonAltMobileNoValue.Visible = false;
            lblSalesPersonEmailValue.Visible = false;
            lblSalesPersonAltEmailValue.Visible = false;
            lblSalesPersonDOBValue.Visible = false;
            lblSalesPersoGenderValue.Visible = false;
            lblSalesPersonWhatsapp1Value.Visible = false;
            lblSalesPersonWhatsap2Value.Visible = false;
            lblSalesPersonPincodeValue.Visible = false;
            lblSalesPersonAddressValue.Visible = false;
            imgSalesPerson.Visible = false;

            lblSalesPersonDOBValue.Attributes.Add("ReadOnly", "readonly");

            txtSalesPersonName.Visible = true;
            txtSalesPersonMobileNo.Visible = true;
            txtSalesPersonAltMobileNo.Visible = true;
            txtSalesPersonDOB.Visible = true;
            txtSalesPersonEmail.Visible = true;
            txtSalesPersonAltEmail.Visible = true;
            txtSalesPersonWhatsapp1.Visible = true;
            txtSalesPersonWhatsapp2.Visible = true;
            ddlSalesPersonGender.Visible = true;
            txtSalesPersonPincode.Visible = true;
            txtSalesPersonAddress.Visible = true;
            fuSalesPersonProfileImage.Visible = true;

            txtSalesPersonName.Text = lblSalesPersonNameValue.Text;
            txtSalesPersonMobileNo.Text = lblSalesPersonMobileNoValue.Text;
            txtSalesPersonAltMobileNo.Text = lblSalesPersonAltMobileNoValue.Text;
            txtSalesPersonDOB.Text = lblSalesPersonDOBValue.Text;
            txtSalesPersonEmail.Text = lblSalesPersonEmailValue.Text;
            txtSalesPersonAltEmail.Text = lblSalesPersonAltEmailValue.Text;
            txtSalesPersonWhatsapp1.Text = lblSalesPersonWhatsapp1Value.Text;
            txtSalesPersonWhatsapp2.Text = lblSalesPersonWhatsap2Value.Text;
            ddlSalesPersonGender.Text = lblSalesPersoGenderValue.Text.Contains("Select") ? null : lblSalesPersoGenderValue.Text;
            txtSalesPersonPincode.Text = lblSalesPersonPincodeValue.Text;
            txtSalesPersonAddress.Text = lblSalesPersonAddressValue.Text;
        }

        protected void btnCancelSalesPersonProfile_Click(object sender, EventArgs e)
        {
            btnSalesPersonEdit.Visible = true;
            btnSalesPersonCancel.Visible = false;
            btnSalesPersonSave.Visible = false;

            lblSalesPersonNameValue.Visible = true;
            lblSalesPersonMobileNoValue.Visible = true;
            lblSalesPersonAltMobileNoValue.Visible = true;
            lblSalesPersonEmailValue.Visible = true;
            lblSalesPersonAltEmailValue.Visible = true;
            lblSalesPersonDOBValue.Visible = true;
            lblSalesPersoGenderValue.Visible = true;
            lblSalesPersonWhatsapp1Value.Visible = true;
            lblSalesPersonWhatsap2Value.Visible = true;
            lblSalesPersonPincodeValue.Visible = true;
            lblSalesPersonCityValue.Visible = true;
            lblSalesPersonStateValue.Visible = true;
            lblSalesPersonAddressValue.Visible = true;
            imgSalesPerson.Visible = true;

            txtSalesPersonName.Visible = false;
            txtSalesPersonMobileNo.Visible = false;
            txtSalesPersonAltMobileNo.Visible = false;
            txtSalesPersonDOB.Visible = false;
            txtSalesPersonEmail.Visible = false;
            txtSalesPersonAltEmail.Visible = false;
            txtSalesPersonWhatsapp1.Visible = false;
            txtSalesPersonWhatsapp2.Visible = false;
            ddlSalesPersonGender.Visible = false;
            txtSalesPersonPincode.Visible = false;
            txtSalesPersonAddress.Visible = false;
            fuSalesPersonProfileImage.Visible = false;
        }

        protected void btnUpdateSalesPersonProfile_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;

                if (string.IsNullOrEmpty(txtSalesPersonAddress.Text))
                {
                    lblSalesPersonAddressError.Visible = true;
                    txtSalesPersonAddress.Focus();
                    count++;
                }
                else
                {
                    var words = txtSalesPersonAddress.Text.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length < 3)
                    {
                        lblSalesPersonAddressError.Visible = true;
                        lblSalesPersonAddressError.InnerText = "Address must contain at least 3 words.";
                        txtSalesPersonAddress.Focus();
                        count++;
                    }
                    else
                    {
                        lblSalesPersonAddressError.Visible = false;
                    }
                }
                if (string.IsNullOrWhiteSpace(txtSalesPersonPincode.Text))
                {
                    lblSalesPersonPincodeError.Visible = true;
                    txtSalesPersonPincode.Focus();
                    count++;
                }
                else if (txtSalesPersonPincode.Text.Length != 6)
                {
                    lblSalesPersonPincodeError.Visible = true;
                    lblSalesPersonPincodeError.InnerText = "Invalid Pincode";
                    txtSalesPersonPincode.Focus();
                    count++;
                }
                else
                {
                    lblSalesPersonPincodeError.Visible = false;
                }

                if (!string.IsNullOrWhiteSpace(txtSalesPersonWhatsapp2.Text) && txtSalesPersonWhatsapp2.Text.Length != 10)
                {
                    lblSalesPersonWhatsap2Error.Visible = true;
                    txtSalesPersonWhatsapp2.Focus();
                    count++;
                }
                else
                {
                    lblSalesPersonWhatsap2Error.Visible = false;
                }
                if (!string.IsNullOrWhiteSpace(txtSalesPersonWhatsapp1.Text) && txtSalesPersonWhatsapp1.Text.Length != 10)
                {
                    if (txtSalesPersonWhatsapp1.Text.Length != 10)
                        lblSalesPersonWhatsapp1Error.InnerText = "Invalid WhatsApp Mobile No 1.";
                    lblSalesPersonWhatsapp1Error.Visible = true;
                    txtSalesPersonWhatsapp1.Focus();
                    count++;
                }
                else
                {
                    lblSalesPersonWhatsapp1Error.Visible = false;
                }
                string wamobile = txtSalesPersonWhatsapp2.Text.Trim();
                string altwamobile = txtSalesPersonWhatsapp1.Text.Trim();

                if (!string.IsNullOrEmpty(wamobile) && wamobile == altwamobile)
                {
                    lblSalesPersonWhatsapp1Error.InnerText = "WhatsApp Mobile No and alternate WhatsApp mobile numbers cannot be the same.";
                    lblSalesPersonWhatsapp1Error.Visible = true;
                    txtSalesPersonWhatsapp1.Focus();
                    count++;
                }
                if (!string.IsNullOrWhiteSpace(txtSalesPersonAltEmail.Text) && !Regex.IsMatch(txtSalesPersonAltEmail.Text.Trim(), @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    lblSalesPersonAltEmailError.Visible = true;
                    lblSalesPersonAltEmailError.InnerText = "Invalid Alternate Email ID.";
                    txtSalesPersonAltEmail.Focus();
                    count++;
                }
                else
                {
                    lblSalesPersonAltEmailError.Visible = false;
                }

                if (string.IsNullOrWhiteSpace(txtSalesPersonEmail.Text))
                {
                    lblSalesPersonEmailError.Visible = true;
                    txtSalesPersonEmail.Focus();
                    count++;
                }
                else if (!Regex.IsMatch(txtSalesPersonEmail.Text.Trim(), @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    lblSalesPersonEmailError.Visible = true;
                    lblSalesPersonEmailError.InnerText = "Invalid Email ID.";
                    txtSalesPersonEmail.Focus();
                    count++;
                }
                string email = txtSalesPersonEmail.Text.Trim();
                string altemail = txtSalesPersonAltEmail.Text.Trim();

                if (!string.IsNullOrEmpty(email) && email == altemail)
                {
                    lblSalesPersonEmailError.InnerText = "Email ID and alternate Email ID cannot be the same.";
                    lblSalesPersonEmailError.Visible = true;
                    txtSalesPersonEmail.Focus();
                    count++;
                }
                else { lblSalesPersonEmailError.Visible = false; }


                if (!string.IsNullOrWhiteSpace(txtSalesPersonAltMobileNo.Text) && txtSalesPersonAltMobileNo.Text.Length != 10)
                {
                    lblSalesPersonAltMobileNoError.Visible = true;
                    txtSalesPersonAltMobileNo.Focus();
                    count++;
                }
                string mobile = txtSalesPersonMobileNo.Text.Trim();
                string altMobile = txtSalesPersonAltMobileNo.Text.Trim();

                if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
                {
                    lblSalesPersonAltMobileNoError.InnerText = "Mobile No and alternate mobile numbers cannot be the same.";
                    lblSalesPersonAltMobileNoError.Visible = true;
                    txtSalesPersonAltMobileNo.Focus();
                    count++;
                }
                else
                {
                    lblSalesPersonAltMobileNoError.Visible = false;
                }
           

                if (!string.IsNullOrWhiteSpace(txtSalesPersonName.Text) && txtSalesPersonName.Text.Length < 3)
                {
                    lblSalesPersonNameError.Visible = true;
                    lblSalesPersonNameError.InnerText = "Legal Name as per GSTIN must be between 3 and 30 characters.";
                    txtSalesPersonName.Focus();
                    count++;
                }
                if (string.IsNullOrWhiteSpace(txtSalesPersonName.Text))
                {
                    lblSalesPersonNameError.Visible = true;
                    txtSalesPersonName.Focus();
                    count++;
                }
                else
                {
                    lblSalesPersonNameError.Visible = false;
                }

                if (count > 0)
                {
                    return;
                }
                else
                {
                    string profileImg = imgSalesPerson.ImageUrl.Replace("/UploadedDocuments/", "");
                    if (fuSalesPersonProfileImage.HasFile)
                    {
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                        string fileExtension = Path.GetExtension(fuSalesPersonProfileImage.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            return;
                        }
                        string fileName = Path.GetFileName(fuSalesPersonProfileImage.FileName);
                        string folderPath = Server.MapPath("~/UploadedDocuments/");
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        string uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName;
                        string fullPath = Path.Combine(folderPath, uniqueFileName);
                        fuSalesPersonProfileImage.SaveAs(fullPath);
                        profileImg = uniqueFileName;
                    }
                    if (Session["RetailerUniqueID"] != null)
                    {
                        SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Type", 33);
                        cmd.Parameters.AddWithValue("@Mid", Session["RetailerUniqueID"].ToString());
                        cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());
                        cmd.Parameters.AddWithValue("@CustomerName", txtSalesPersonName.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerMobileNo", txtSalesPersonMobileNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@AlternativeMobile", txtSalesPersonAltMobileNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerEmailID", txtSalesPersonEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@AlternativeEmail", txtSalesPersonAltEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@AddressLine1", txtSalesPersonAddress.Text.Trim());
                        DateTime dob;
                        if (DateTime.TryParse(txtSalesPersonDOB.Text.Trim(), out dob))
                            cmd.Parameters.AddWithValue("@DateOfBirth", dob.Date);
                        else
                            cmd.Parameters.AddWithValue("@DateOfBirth", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Gender", ddlSalesPersonGender.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@WhatsappNo", txtSalesPersonWhatsapp1.Text.Trim());
                        cmd.Parameters.AddWithValue("@AltWhatsappNo", txtSalesPersonWhatsapp2.Text.Trim());
                        cmd.Parameters.AddWithValue("@Pincode", txtSalesPersonPincode.Text.Trim());
                        cmd.Parameters.AddWithValue("@City", lblSalesPersonCityValue.Text.Trim());
                        cmd.Parameters.AddWithValue("@State", lblSalesPersonStateValue.Text.Trim());
                        cmd.Parameters.AddWithValue("@ProfileImage", profileImg);

                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        con.Close();

                        btnSalesPersonEdit.Visible = true;
                        btnSalesPersonCancel.Visible = false;
                        btnSalesPersonSave.Visible = false;

                        LoadSalesPersonProfileData();
                        btnCancelSalesPersonProfile_Click(sender, e);
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
        protected void txtAltSalesPersonMobile_TextChanged(object sender, EventArgs e)
        {
            string blockuser = CheckBlockCustomer("", txtSalesPersonAltMobileNo.Text);
            string existuser = CheckSalesPersonOrRetailer("", txtSalesPersonAltMobileNo.Text);
            if (!string.IsNullOrWhiteSpace(existuser))
            {
                lblSalesPersonAltMobileNoError.InnerText = "This Mobile No is already registered";
                lblSalesPersonAltMobileNoError.Visible = true;
                return;
            }
            else if (!string.IsNullOrWhiteSpace(blockuser))
            {
                lblSalesPersonAltMobileNoError.InnerText = blockuser;
                lblSalesPersonAltMobileNoError.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(txtSalesPersonAltMobileNo.Text) && txtSalesPersonAltMobileNo.Text.Replace("+91", "").Length != 10)
            {
                lblSalesPersonAltMobileNoError.InnerText = "Please enter your phone number.";
                lblSalesPersonAltMobileNoError.Visible = true;
                txtSalesPersonAltMobileNo.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtSalesPersonAltMobileNo.Text) && Regex.IsMatch(txtSalesPersonAltMobileNo.Text, @"^[0-5]"))
            {
                lblSalesPersonAltMobileNoError.InnerText = "Invalid number";
                lblSalesPersonAltMobileNoError.Visible = true;
                txtSalesPersonAltMobileNo.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtSalesPersonAltMobileNo.Text) && Regex.IsMatch(txtSalesPersonAltMobileNo.Text, @"^(\d)\1{9}$"))
            {
                lblSalesPersonAltMobileNoError.InnerText = "Invalid number.";
                lblSalesPersonAltMobileNoError.Visible = true;
                txtSalesPersonAltMobileNo.Focus();
                return;
            }
            string mobile = txtSalesPersonMobileNo.Text.Trim();
            string altMobile = txtSalesPersonAltMobileNo.Text.Trim();

            if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
            {
                lblSalesPersonAltMobileNoError.InnerText = "Mobile No and alternate mobile numbers cannot be the same.";
                lblSalesPersonAltMobileNoError.Visible = true;
                txtSalesPersonAltMobileNo.Focus();
                return;
            }
            else { lblSalesPersonAltMobileNoError.Visible = false; }
            lblSalesPersonAltMobileNoError.InnerText = "";
            lblSalesPersonAltMobileNoError.Visible = false;
        }
        protected void txtSalesPersonEmail_TextChanged(object sender, EventArgs e)
        {
            string blockuser = CheckBlockCustomer(txtSalesPersonEmail.Text, "");
            string existuser = CheckSalesPersonOrRetailer(txtSalesPersonEmail.Text, "");
            if (!string.IsNullOrEmpty(existuser))
            {
                lblSalesPersonEmailError.InnerText = "This Email is already registered";
                lblSalesPersonEmailError.Visible = true;
                txtSalesPersonEmail.Focus();
                return;
            }
            else if (!string.IsNullOrWhiteSpace(blockuser))
            {
                lblSalesPersonEmailError.InnerText = blockuser;
                lblSalesPersonEmailError.Visible = true;
                txtSalesPersonEmail.Focus();
                return;
            }
            string email = txtSalesPersonEmail.Text.Trim();
            string altemail = txtSalesPersonAltEmail.Text.Trim();

            if (!string.IsNullOrEmpty(email) && email == altemail)
            {
                lblSalesPersonEmailError.InnerText = "Email ID and alternate Email ID cannot be the same.";
                lblSalesPersonEmailError.Visible = true;
                txtSalesPersonEmail.Focus();
                return;
            }
            lblSalesPersonEmailError.InnerText = "";
            lblSalesPersonAltEmailError.InnerText = "";
            lblSalesPersonEmailError.Visible = false;
            txtSalesPersonAltEmail.Focus();
        }
        protected void txtSalesPersonAltEmail_TextChanged(object sender, EventArgs e)
        {
            string blockuser = CheckBlockCustomer(txtSalesPersonAltEmail.Text, "");
            string existuser = CheckSalesPersonOrRetailer(txtSalesPersonAltEmail.Text, "");
            if (!string.IsNullOrEmpty(existuser))
            {
                lblSalesPersonAltEmailError.InnerText = "This Email is already registered";
                lblSalesPersonAltEmailError.Visible = true;
                txtSalesPersonAltEmail.Focus();
                return;
            }
            else if (!string.IsNullOrWhiteSpace(blockuser))
            {
                lblSalesPersonAltEmailError.InnerText = blockuser;
                lblSalesPersonAltEmailError.Visible = true;
                txtSalesPersonAltEmail.Focus();
                return;
            }
            string email = txtSalesPersonEmail.Text.Trim();
            string altemail = txtSalesPersonAltEmail.Text.Trim();

            if (!string.IsNullOrEmpty(email) && email == altemail)
            {
                lblSalesPersonAltEmailError.InnerText = "Email ID and alternate Email ID cannot be the same.";
                lblSalesPersonAltEmailError.Visible = true;
                txtSalesPersonAltEmail.Focus();
                return;
            }
            lblSalesPersonEmailError.InnerText = "";
            lblSalesPersonAltEmailError.InnerText = "";
            lblSalesPersonEmailError.Visible = false;
            txtSalesPersonWhatsapp1.Focus();
        }
        protected void txtWhatsAppSalerPersonMobile_TextChanged(object sender, EventArgs e)
        {

            string blockuser = CheckBlockCustomer("", txtSalesPersonWhatsapp1.Text);
            string existuser = CheckSalesPersonOrRetailer("", txtSalesPersonWhatsapp1.Text);
            if (!string.IsNullOrWhiteSpace(existuser))
            {
                lblSalesPersonWhatsapp1Error.InnerText = "This Mobile No is already registered";
                lblSalesPersonWhatsapp1Error.Visible = true;
                return;
            }
            else if (!string.IsNullOrWhiteSpace(blockuser))
            {
                lblSalesPersonWhatsapp1Error.InnerText = blockuser;
                lblSalesPersonWhatsapp1Error.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(txtSalesPersonWhatsapp1.Text) && txtSalesPersonWhatsapp1.Text.Replace("+91", "").Length != 10)
            {
                lblSalesPersonWhatsapp1Error.InnerText = "Please enter your phone number.";
                lblSalesPersonWhatsapp1Error.Visible = true;
                txtSalesPersonWhatsapp1.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtSalesPersonWhatsapp1.Text) && Regex.IsMatch(txtSalesPersonWhatsapp1.Text, @"^[0-5]"))
            {
                lblSalesPersonWhatsapp1Error.InnerText = "Invalid number";
                lblSalesPersonWhatsapp1Error.Visible = true;
                txtSalesPersonWhatsapp1.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtSalesPersonWhatsapp1.Text) && Regex.IsMatch(txtSalesPersonWhatsapp1.Text, @"^(\d)\1{9}$"))
            {
                lblSalesPersonWhatsapp1Error.InnerText = "Invalid number.";
                lblSalesPersonWhatsapp1Error.Visible = true;
                txtSalesPersonWhatsapp1.Focus();
                return;
            }
            string mobile = txtSalesPersonWhatsapp1.Text.Trim();
            string altMobile = txtSalesPersonWhatsapp2.Text.Trim();

            if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
            {
                lblSalesPersonWhatsapp1Error.InnerText = "Whatsapp Mobile No 1 and Whatsapp Mobile No 2 cannot be the same.";
                lblSalesPersonWhatsapp1Error.Visible = true;
                txtSalesPersonWhatsapp1.Focus();
                return;
            }
            else { lblSalesPersonWhatsapp1Error.Visible = false; }
            lblSalesPersonWhatsapp1Error.InnerText = "";
            lblSalesPersonWhatsap2Error.InnerText = "";
            lblSalesPersonWhatsapp1Error.Visible = false;
            lblSalesPersonWhatsap2Error.Visible = false;
            txtSalesPersonWhatsapp2.Focus();
        }
        protected void txtWhatsAppAltSalesPersonMobile_TextChanged(object sender, EventArgs e)
        {

            string blockuser = CheckBlockCustomer("", txtSalesPersonWhatsapp2.Text);
            string existuser = CheckSalesPersonOrRetailer("", txtSalesPersonWhatsapp2.Text);
            if (!string.IsNullOrWhiteSpace(existuser))
            {
                lblSalesPersonWhatsap2Error.InnerText = "This Mobile No is already registered";
                lblSalesPersonWhatsap2Error.Visible = true;
                return;
            }
            else if (!string.IsNullOrWhiteSpace(blockuser))
            {
                lblSalesPersonWhatsap2Error.InnerText = blockuser;
                lblSalesPersonWhatsap2Error.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(txtSalesPersonWhatsapp2.Text) && txtSalesPersonWhatsapp2.Text.Replace("+91", "").Length != 10)
            {
                lblSalesPersonWhatsap2Error.InnerText = "Please enter your phone number.";
                lblSalesPersonWhatsap2Error.Visible = true;
                txtSalesPersonWhatsapp2.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtSalesPersonWhatsapp2.Text) && Regex.IsMatch(txtSalesPersonWhatsapp2.Text, @"^[0-5]"))
            {
                lblSalesPersonWhatsap2Error.InnerText = "Invalid number";
                lblSalesPersonWhatsap2Error.Visible = true;
                txtSalesPersonWhatsapp2.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(txtSalesPersonWhatsapp2.Text) && Regex.IsMatch(txtSalesPersonWhatsapp2.Text, @"^(\d)\1{9}$"))
            {
                lblSalesPersonWhatsap2Error.InnerText = "Invalid number.";
                lblSalesPersonWhatsap2Error.Visible = true;
                txtSalesPersonWhatsapp2.Focus();
                return;
            }
            string mobile = txtSalesPersonMobileNo.Text.Trim();
            string altMobile = txtSalesPersonWhatsapp2.Text.Trim();

            if (!string.IsNullOrEmpty(mobile) && mobile == altMobile)
            {
                lblSalesPersonWhatsap2Error.InnerText = "Whatsapp Mobile No 1 and Whatsapp Mobile No 2 cannot be the same.";
                lblSalesPersonWhatsap2Error.Visible = true;
                txtSalesPersonWhatsapp2.Focus();
                return;
            }
            else { lblSalesPersonWhatsap2Error.Visible = false; }
            lblSalesPersonWhatsapp1Error.InnerText = "";
            lblSalesPersonWhatsap2Error.InnerText = "";
            lblSalesPersonWhatsapp1Error.Visible = false;
            lblSalesPersonWhatsap2Error.Visible = false;
            ddlSalesPersonGender.Focus();
        }
        protected void txtSalesPersonPinCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSalesPersonPincode.Text.Length == 6)
                {
                    SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pincode", SqlDbType.Int).Value = txtSalesPersonPincode.Text.Trim();
                    cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 4;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        lblSalesPersonCityValue.Text = dt.Rows[0]["CityName"].ToString();
                        lblSalesPersonStateValue.Text = dt.Rows[0]["statename"].ToString();
                    }
                }
                else
                {
                    lblSalesPersonPincodeError.Visible = true;
                    lblSalesPersonPincodeError.InnerText = "Enter a 6-digit Pincode";
                    txtSalesPersonPincode.Focus();
                }
            }
            catch (Exception ex)
            {

                return;
            }
        }
        protected string GetRowStyle(string status)
        {
            switch (status?.ToLower())
            {
                case "rejected":
                    return "background-color: #f8d7da;";
                case "approved":
                    return "background-color: #d4edda;";
                default:
                    return "background-color: #f1f16b;";
            }
        }




    }
}