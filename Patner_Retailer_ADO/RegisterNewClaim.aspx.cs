using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Patner_Retailer_ADO
{
    public partial class RegisterNewClaim : System.Web.UI.Page
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
                txtDamageDate.Attributes.Add("ReadOnly", "readonly");
                this.DataBind();
                BindInformation();
            }
        }
        private void BindInformation()
        {
            string ticketNo = Request.QueryString["TicketNumber"];
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 19);
                cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"].ToString());
                cmd.Parameters.AddWithValue("@ticketno", ticketNo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    txtFirstName.Text = dr["CustomerName"].ToString();
                    txtLastName.Text = "";                    
                    txtEmail.Text = dr["EmailID"].ToString();
                    txtCustomerMobileNo.Text = dr["MobileNo"].ToString();
                    txtWhatsappNo.Text = dr["WhatsAppNo"].ToString();
                    txtPincode.Text = dr["PINCode"].ToString();
                    txtCity.Text = dr["City"].ToString();
                    txtState.Text = dr["State"].ToString();
                    txtAddressLine1.Value = dr["AddressLine1"].ToString();
                    txtLandmark.Value = dr["AddressLine2"].ToString();

                    txtProductName.Text = dr["Productname"].ToString();
                    txtProductsubcategoryname.Text = dr["Productsubcategoryname"].ToString();
                    txtBrand.Text = dr["Brand"].ToString();
                    txtModelname.Text = dr["Model"].ToString();
                    txtIMEI.Text = dr["IMEI_No"].ToString();
                    txtDevicePurchasePrice.Text = dr["DevicePurchasePrice"].ToString();
                    txtProductPurchaseDate.Text = dr["ProductPurchaseDate"].ToString();
                    txtPlanName.Text = dr["PlanName"].ToString();
                    txtPlanPrice.Text = dr["PlanPrice"].ToString();                    
                }
                else
                {
                    txtFirstName.Text ="";
                    txtLastName.Text = "";
                    txtEmail.Text = "";
                    txtCustomerMobileNo.Text = "";
                    txtWhatsappNo.Text = "";
                    txtPincode.Text = "";
                    txtCity.Text = "";
                    txtState.Text = "";
                    txtAddressLine1.Value = "";
                    txtLandmark.Value = "";

                    txtProductName.Text = "";
                    txtProductsubcategoryname.Text = "";
                    txtBrand.Text = "";
                    txtModelname.Text = "";
                    txtIMEI.Text = "";
                    txtDevicePurchasePrice.Text = "";
                    txtProductPurchaseDate.Text = "";
                    txtPlanName.Text = "";
                    txtPlanPrice.Text = "";
                }
            }
          

        }
        protected void RegisterClaim(object sender, EventArgs args)
        {
            int count =0;
            if (string.IsNullOrWhiteSpace(Request.Form[txtProblemDesc.UniqueID]))
            {
                lblProblemDesc.Text = "Problem Description is required.";
                lblProblemDesc.Visible = true;
                count++;
            }
            if (!rdoPhysical.Checked && !rdoLiquid.Checked && !rdoBoth.Checked)
            {
                lblDamageType.Text = "Please select the Type of Damage.";
                lblDamageType.Visible = true;
                count++;
            }
            if (!rblphoneswitchingon.Checked && !rblphoneswitchingnot.Checked)
            {
                lblDeviceSwitchOn.Text = "Please specify whether the device is switching on.";
                lblDeviceSwitchOn.Visible = true;
                count++;
            }
            if (!chkDefectiveParts.Items.Cast<ListItem>().Any(i => i.Selected))
            {
                lblDefectiveParts.Text = "Please select at least one defective part.";
                lblDefectiveParts.Visible = true;
                count++;
            }
            if (!touchworking.Checked && !touchworkingnot.Checked)
            {
                lblTouchWorking.Text = "Please specify whether the touch screen is working.";
                lblTouchWorking.Visible = true;
                count++;
            }
            if (string.IsNullOrWhiteSpace(txtDamageDate.Text))
            {
                lblDamageDate.Text = "Damage date is required.";
                lblDamageDate.Visible = true;
                count++;
            }
            if (string.IsNullOrWhiteSpace(txtDamageTime.Text))
            {
                lblDamageTime.Text = "Damage time is required.";
                lblDamageTime.Visible = true;
                count++;
            }
            if (string.IsNullOrWhiteSpace(txtPlaceOfDamage.Text))
            {
                lblPlaceOfDamage.Text = "Place of damage is required.";
                lblPlaceOfDamage.Visible = true;
                count++;
            }

            if (count > 0)
            {
                return;
            }
        }

    }
}