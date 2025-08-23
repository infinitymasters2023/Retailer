using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.WebRequestMethods;

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
                CalendarExtender1.StartDate = DateTime.Today;
            }
        }
        protected void btnConfirmExpiry_Click(object sender, EventArgs e)
        {
            string serverUrl = ConfigurationManager.AppSettings["ServerURL"];
            string expiryString = expiryDate.Text + " " + expiryTime.Value;
            string encodedExpiry = Convert.ToBase64String(Encoding.UTF8.GetBytes(expiryString));
            string mobile = Session["MobileNo"].ToString();
            string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(mobile));
            string datetime = DateTime.Now.ToString();
            string dd = Convert.ToBase64String(Encoding.UTF8.GetBytes(datetime));
            string url = serverUrl + "/RetailerBuyInfySalePlan.aspx?qu=" + HttpUtility.UrlEncode(encoded) + "&dd=" + HttpUtility.UrlEncode(dd) + "&ed=" + HttpUtility.UrlEncode(encodedExpiry);

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
                        ViewState["UrlTable"] = dt;
                        if (dt.Rows.Count > 0)
                        {
                            GvURL.CssClass = "table data-table table-striped nowrap";
                            GvURL.DataSource = dt;
                            GvURL.DataBind();
                            if (GvURL.HeaderRow != null)
                            {
                                GvURL.HeaderRow.TableSection = TableRowSection.TableHeader;
                            }
                        }
                        else
                        {
                            GvURL.DataSource = null;
                            GvURL.DataBind();
                            GvURL.CssClass = "table table-striped nowrap";
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
        protected void btnSendEmailAndWhatsapp_Click(object sender, EventArgs e)
        {
            var email = hdnEmailList.Value;
            var mobile = hdnWhatsappList.Value;
            if (!string.IsNullOrWhiteSpace(email))
            {
                SendEmail(email);
            }
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                string[] numbers = mobile.Split(',').Select(n => n.Trim()).Where(n => !string.IsNullOrEmpty(n)).ToArray();
                foreach (string mobilenumber in numbers)
                {
                    SendWhatsapp(mobilenumber);
                }
            }
            LodBind();
        }
        protected void SendEmail(string email)
        {
            try
            {
                string url = urlToCopy.Value;
                string logoPath = Server.MapPath("~/assets/images/logo.png");

                string emailBody = @"<table cellpadding='0' cellspacing='0' style='width:100%; font-family: Arial, sans-serif; font-size: 14px; color: #333;'>
                                        <tr>
                                            <td colspan='2' style='padding: 10px 0;'>
                                                Dear Customer,
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan='2' style='padding: 10px 0;'>
                                                Please use the following secure link to complete your action:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan='2' style='padding: 10px 0;'>
                                                <strong><a href='" + url + @"' target='_blank' style='color: #1a73e8; text-decoration: underline;'>" + url + @"</a></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan='2' style='padding: 20px 0;'>
                                                Thanking you,<br/>
                                                <strong>Team Infinity</strong><br/>
                                                InfyShield
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan='2'>
                                                 <img src='cid:companylogo' alt='Company Logo' style='height: 50px;' />
                                            </td>
                                        </tr>
                                    </table>";
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress("no-reply@infinityassurance.com");
                Msg.To.Add(email.ToString());
                Msg.Subject = "Complete Your Process with This Secure InfyShield Link";
                Msg.IsBodyHtml = true;
                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(emailBody, null, MediaTypeNames.Text.Html);

                LinkedResource inlineLogo = new LinkedResource(logoPath, MediaTypeNames.Image.Jpeg);
                inlineLogo.ContentId = "companylogo";
                inlineLogo.TransferEncoding = TransferEncoding.Base64;
                avHtml.LinkedResources.Add(inlineLogo);

                Msg.AlternateViews.Add(avHtml);

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("no-reply@infinityassurance.com", "mlas jsej cdzd fmdc");
                smtp.EnableSsl = true;
                smtp.Send(Msg);
                Msg = null;
            }
            catch (Exception ex)
            {
                DisplayMessage(this, "" + ex.Message.ToString() + "");
                return;
            }
        }
        protected void SendWhatsapp(string mobileNo)
        {

        }
    }
}