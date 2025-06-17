using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Security.Policy;

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
                        GvSalePerson.CssClass = "table-responsive table data-table table-striped table-bordered nowrap";
                        GvSalePerson.DataSource = dt;
                        GvSalePerson.DataBind();
                        if (GvSalePerson.HeaderRow != null)
                        {
                            GvSalePerson.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                    }
                    else
                    {
                        GvSalePerson.CssClass = "table-responsive table table-striped table-bordered nowrap";
                        GvSalePerson.DataSource = null;
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
            if (e.CommandName == "ChangeStatusRow")
            {
                string[] args = e.CommandArgument.ToString().Split('|');
                string profileId = args[0];
                string currentStatus = args[1];
                ChagneStatus(profileId, currentStatus);
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
        protected void ChagneStatus(string profileId, string currentStatus)
        {
            using (SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con))
            {
                if (currentStatus == "Active")
                    currentStatus = "Pending";
                else
                    currentStatus = "Active";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 21);
                cmd.Parameters.AddWithValue("@ProfileId", profileId);
                cmd.Parameters.AddWithValue("@Status", currentStatus);
                cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());

                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }



    }
}