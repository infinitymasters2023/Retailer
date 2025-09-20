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
    public partial class Index : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iaplConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string token = Request.QueryString["token"];
                if (token != null)
                {
                    string name = CheckToken(token);
                    lblWelcomeMessage.Text = "Welcome to :";
                    lblMessageName.Text = name;
                }
            }

        }
        protected string CheckToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return "";

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
                        string Name = reader["Name"].ToString();
                        return Name;
                    }
                }
                con.Close();
            }

            return "";
        }
    }
}