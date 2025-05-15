using System;
using System.Collections.Generic;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Remove("SellerGSTIN");                
        }
        protected void btnGSTIN_Click(object sender, EventArgs e)
        {
            string gstinPattern = @"^\d{2}[A-Z]{5}\d{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$";

            if (!string.IsNullOrWhiteSpace(txtGSTIN.Text) && !Regex.IsMatch(txtGSTIN.Text.ToUpper(), gstinPattern))
            {
                gstinError.Text = "Invalid GSTIN format.";
                return;
            }
            else
            {
                Session["SellerGSTIN"] = txtGSTIN.Text;
                Response.Redirect("CreateAnAccount.aspx");
            }
        }
    }
}