using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Patner_Retailer_ADO
{
    public partial class ViewSalesPerson : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iaplConnectionString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LodBind();
            }

        }

        private void LodBind()
        {
            try
            {

                using (SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", 8); // Fetch All Data
                    cmd.Parameters.AddWithValue("@RetailerAdminID", Session["RetailerUniqueID"].ToString());

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        GvSalePerson.DataSource = dt;
                        GvSalePerson.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
        protected void GvSalePerson_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                string profileId = e.CommandArgument.ToString();
                // Redirect to edit page or open a modal
                Response.Redirect("CreateSalesPerson.aspx?Mid=" + profileId);
            }
            else if (e.CommandName == "DeleteRow")
            {
                string profileId = e.CommandArgument.ToString();
                DeleteProfile(profileId);
                LodBind();
            }
        }

        private void DeleteProfile(string profileId)
        {
            using (SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 11);
                cmd.Parameters.AddWithValue("@Mid", profileId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }



    }
}