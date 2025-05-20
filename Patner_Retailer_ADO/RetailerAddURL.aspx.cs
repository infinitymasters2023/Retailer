using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Patner_Retailer_ADO
{
    public partial class RetailerAddURL : System.Web.UI.Page
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
                expiryDate.Attributes.Add("ReadOnly", "readonly");                
            }
        }
        protected void btnConfirmExpiry_Click(object sender, EventArgs e)
        {
            string expiryString = expiryDate.Text +" "+ expiryTime.Value;
            string encodedExpiry = Convert.ToBase64String(Encoding.UTF8.GetBytes(expiryString));
            string mobile = Session["MobileNo"].ToString();
            string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(mobile));
            string datetime = DateTime.Now.ToString();
            string dd = Convert.ToBase64String(Encoding.UTF8.GetBytes(datetime));
            string url = "http://localhost:44302/RetailerBuyInfySalePlan.aspx?qu=" + HttpUtility.UrlEncode(encoded)+"&dd="+ HttpUtility.UrlEncode(dd) + "&ed=" + HttpUtility.UrlEncode(encodedExpiry);

            DataTable dt = ViewState["UrlTable"] as DataTable;
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("CreatedDate");
                dt.Columns.Add("CreatedTime");
                dt.Columns.Add("ExpiredDate");
                dt.Columns.Add("ExpiredTime");
                dt.Columns.Add("URL");
            }
            DataRow row = dt.NewRow();
            row["CreatedDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            row["CreatedTime"] = DateTime.Now.ToString("HH:mm:ss");
            row["ExpiredDate"] = expiryDate.Text;
            row["ExpiredTime"] = expiryTime.Value;
            row["URL"] = url;
            dt.Rows.Add(row);
            GvURL.DataSource = dt;
            GvURL.DataBind();
            if (GvURL.HeaderRow != null)
            {
                GvURL.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            AddURL(url, expiryDate.Text, expiryTime.Value);
            string script = $@"
                <script type='text/javascript'>
                $(function() {{
                    showUrlModal('{url}');
                }});
                </script>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowUrlModal", script, false);
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
                    cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["RetailerUniqueID"].ToString());

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        GvURL.DataSource = dt;
                        GvURL.DataBind();
                        if (GvURL.HeaderRow != null)
                        {
                            GvURL.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                        ViewState["UrlTable"] = dt;

                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }
        protected void AddURL(string url, string expiryDate, string expiryTime)
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@type", 22);
                    cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["RetailerUniqueID"].ToString());
                    cmd.Parameters.AddWithValue("@Retailer_MobileNo", Session["MobileNo"] != null ? Session["MobileNo"].ToString() : null);
                    cmd.Parameters.AddWithValue("@LinkCreatedDate", DateTime.Now.ToString("dd-MMM-yyyy"));
                    cmd.Parameters.AddWithValue("@LinkCreatedTime", DateTime.Now.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@LinkExpiredDate", !string.IsNullOrWhiteSpace(expiryDate) ? expiryDate : null);
                    cmd.Parameters.AddWithValue("@LinkExpiredTime", !string.IsNullOrWhiteSpace(expiryTime) ? expiryTime : null);
                    cmd.Parameters.AddWithValue("@Link", url);
                    cmd.Parameters.AddWithValue("@CreatedBy", Session["Name"] != null ? Session["Name"].ToString() : null);


                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (Exception ex)
            {
            }
        }
    }
}