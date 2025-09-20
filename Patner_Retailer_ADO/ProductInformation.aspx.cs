using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Patner_Retailer_ADO
{
    public partial class ProductInformation : System.Web.UI.Page
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
                if (Session["ProductType"] != null)
                {
                    Session.Remove("ProductType");
                }
                BindProductCategories();
            }
        }
        private void BindProductCategories()
        {
            SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@type", 52);
            cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString());
            cmd.Parameters.AddWithValue("@UserRole", Session["Role"].ToString());

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            var grouped = dt.AsEnumerable().GroupBy(r => r["Category"].ToString()).Select(g => new
            {
                Category = g.Key,
                Products = g.CopyToDataTable()
            }).ToList();

            rptCategory.DataSource = grouped;
            rptCategory.DataBind();
        }
        protected void AddProduct_Click(object sender, CommandEventArgs e)
        {
            string selectedProductTypeID = e.CommandArgument.ToString();
            LinkButton btn = (LinkButton)sender;
            RepeaterItem item = (RepeaterItem)btn.NamingContainer;

            HiddenField hdnName = (HiddenField)item.FindControl("hdnProductTypeName");
            string selectedProductTypeName = hdnName?.Value;
            Session["ProductType"] = selectedProductTypeID;
            Session["ProductTypeName"] = selectedProductTypeName;

            if (Request.QueryString["AddProduct"] != null)
            {
                Response.Redirect("BuyInfySalePlan.aspx?AddProduct=Add");
            }
            else
            {
                Response.Redirect("BuyInfySalePlan.aspx");
            }
        }

    }

}