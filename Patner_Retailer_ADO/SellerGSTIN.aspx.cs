using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Patner_Retailer_ADO
{
    public partial class SellerGSTIN : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iaplConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            string token = Request.QueryString["qa"];
            if (token != null && !string.IsNullOrWhiteSpace(token))
            {
                bool flag = CheckToken(token);
                if (!flag)
                {
                    lblRegistrationErrorMessage.Text = "Invalid Token.";
                    lblRegistrationErrorMessage.Attributes.Add("style", "display:block");
                    return;
                }
            }
            if (Session["Message"] != null)
            {
                lblRegistrationErrorMessage.Text = Session["Message"].ToString();
                Session.Remove("Message");
            }
            //Session.Remove("SellerGSTIN");                
        }
        protected void btnGSTIN_Click(object sender, EventArgs e)
        {
            string gstinPattern = @"^\d{2}[A-Z]{5}\d{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$";

            SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", 38);
            cmd.Parameters.AddWithValue("@SellerGSTINNo", txtGSTIN.Text.Trim());

            if (con.State != ConnectionState.Open)
                con.Open();
            object result = cmd.ExecuteScalar();
            con.Close();
            if (result != null && result.ToString() == "0")
            {
                gstinError.Text = "Seller with this GSTIN already exists.";
                return;
            }            
            if (!string.IsNullOrWhiteSpace(txtGSTIN.Text) && !Regex.IsMatch(txtGSTIN.Text.ToUpper(), gstinPattern))
            {
                gstinError.Text = "Invalid GSTIN format.";
                return;
            }
            else
            {
                string token = Request.QueryString["qa"];
                if (token != null && !string.IsNullOrWhiteSpace(token))
                {
                    Session["SellerGSTIN"] = txtGSTIN.Text;
                    Response.Redirect("CreateAnAccount.aspx?qa=" + token);
                }
                else
                {
                    Session["SellerGSTIN"] = txtGSTIN.Text;
                    Response.Redirect("CreateAnAccount.aspx");
                }
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
    }
}