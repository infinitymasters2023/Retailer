using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Patner_Retailer_ADO
{
    public partial class Feedback : System.Web.UI.Page
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
                LodBind();
            }
        }
        private void LodBind()
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@type", 21);
                    //cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["RetailerUniqueID"].ToString());

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        ViewState["UrlTable"] = dt;
                        if (dt.Rows.Count > 0)
                        {
                            GVFeedback.CssClass = "table data-table table-striped nowrap";
                            GVFeedback.DataSource = dt;
                            GVFeedback.DataBind();
                            if (GVFeedback.HeaderRow != null)
                            {
                                GVFeedback.HeaderRow.TableSection = TableRowSection.TableHeader;
                            }
                        }
                        else
                        {
                            GVFeedback.DataSource = null;
                            GVFeedback.DataBind();
                            GVFeedback.CssClass = "table table-striped nowrap";
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
        protected void btnAddFeedback_ServerClick(object sender, EventArgs e)
        {
            if (feedbackSection.Visible == true)
                feedbackSection.Visible = false;
            else
                feedbackSection.Visible = true;
        }

        protected void btnSubmitFeedback_Click(object sender, EventArgs e)
        {
            string feedback = txtFeedback.Value;
            feedbackSection.Visible = false;
            txtFeedback.Value = "";
        }

        protected void btnCancelFeedback_Click(object sender, EventArgs e)
        {
            feedbackSection.Visible = false;
            txtFeedback.Value = "";
        }

    }
}