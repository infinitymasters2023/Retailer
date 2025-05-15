using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Patner_Retailer_ADO
{
    public partial class Profile : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iaplConnectionString"].ConnectionString);

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
                lblName.Text += dr["Name"].ToString();
                lblMobileNo.Text += dr["MobileNo"].ToString();
                lblWhatsappNo.Text += dr["WhatsappMobileNo"].ToString();
                lblEmail.Text += dr["emailID"].ToString();
                // lblDOB.Text += Convert.ToDateTime(dr["DateOfBirth"]).ToString("dd-MM-yyyy");
                lblGender.Text += dr["Gender"].ToString();
                lblAddress.Text += dr["Address"].ToString();
                lblCity.Text += dr["City"].ToString();
                lblState.Text += dr["State"].ToString();
                lblPAN.Text += dr["PANNo"].ToString();
                lblAadhar.Text += dr["AdhaarNo"].ToString();
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
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                TableRow row = new TableRow();
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    TableCell cell = new TableCell();
                    cell.Text = rdr[i].ToString();
                    row.Cells.Add(cell);
                }
                tbodyData.Controls.Add(row);
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

    }
}