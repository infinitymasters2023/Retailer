using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Patner_Retailer_ADO
{
    public partial class BaseMargin : System.Web.UI.Page
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
                BindCommision();
            }
        }
        protected void BindCommision()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", 96);
                    cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"].ToString());                    
                    cmd.Parameters.AddWithValue("@UserRole", Session["Role"]?.ToString() ?? "");
                    

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        GVBaseCommisionDetails.CssClass = "table-responsive table table-striped nowrap";

                        GVBaseCommisionDetails.DataSource = dt;
                        GVBaseCommisionDetails.DataBind();
                        if (GVBaseCommisionDetails.HeaderRow != null)
                        {
                            GVBaseCommisionDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                    }
                    else
                    {
                        GVBaseCommisionDetails.CssClass = "table table-striped nowrap";
                        GVBaseCommisionDetails.DataSource = null;
                        GVBaseCommisionDetails.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}