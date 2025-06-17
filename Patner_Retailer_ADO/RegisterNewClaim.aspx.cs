using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Drawing;
using Ionic.Zip;

namespace Patner_Retailer_ADO
{
    public partial class RegisterNewClaim : System.Web.UI.Page
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
                txtDamageDate.Attributes.Add("ReadOnly", "readonly");
                this.DataBind();
                BindInformation();
            }
        }
        private void BindInformation()
        {
            string sku = Request.QueryString["sku"];
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 19);
                cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"].ToString());
                cmd.Parameters.AddWithValue("@SKU", sku);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    txtFirstName.Text = dr["CustomerName"].ToString();
                    txtLastName.Text = "";
                    txtEmail.Text = dr["EmailID"].ToString();
                    txtCustomerMobileNo.Text = dr["MobileNo"].ToString();
                    txtWhatsappNo.Text = dr["WhatsAppNo"].ToString();
                    txtPincode.Text = dr["PINCode"].ToString();
                    txtCity.Text = dr["City"].ToString();
                    txtState.Text = dr["State"].ToString();
                    txtAddressLine1.Value = dr["AddressLine1"].ToString();
                    txtLandmark.Value = dr["AddressLine2"].ToString();
                    txtProductName.Text = dr["Productname"].ToString();
                    txtProductsubcategoryname.Text = dr["Productsubcategoryname"].ToString();
                    txtBrand.Text = dr["Brand"].ToString();
                    txtModelname.Text = dr["Model"].ToString();
                    txtIMEI.Text = dr["IMEI_No"].ToString();
                    txtDevicePurchasePrice.Text = dr["DevicePurchasePrice"].ToString();
                    txtProductPurchaseDate.Text = dr["ProductPurchaseDate"].ToString();
                    txtPlanName.Text = dr["PlanName"].ToString();
                    txtPlanPrice.Text = dr["PlanPrice"].ToString();
                    ViewState["sku"] = dr["InfyShieldRefNo"].ToString();
                    ViewState["reg"] = dr["registrationno"].ToString();
                    lblcertificate.Text = dr["certificateno"].ToString();
                    lblloan.Text = dr["LoanNo"].ToString();
                    lblclamno.Text = dr["CrnNo"].ToString();
                }
                else
                {
                    txtFirstName.Text = "";
                    txtLastName.Text = "";
                    txtEmail.Text = "";
                    txtCustomerMobileNo.Text = "";
                    txtWhatsappNo.Text = "";
                    txtPincode.Text = "";
                    txtCity.Text = "";
                    txtState.Text = "";
                    txtAddressLine1.Value = "";
                    txtLandmark.Value = "";
                    txtProductName.Text = "";
                    txtProductsubcategoryname.Text = "";
                    txtBrand.Text = "";
                    txtModelname.Text = "";
                    txtIMEI.Text = "";
                    txtDevicePurchasePrice.Text = "";
                    txtProductPurchaseDate.Text = "";
                    txtPlanName.Text = "";
                    txtPlanPrice.Text = "";
                }
            }
        }
        protected void RegisterClaim(object sender, EventArgs args)
        {
            int count = 0;
            if (string.IsNullOrWhiteSpace(Request.Form[txtProblemDesc.UniqueID]))
            {
                lblProblemDesc.Text = "Problem Description is required.";
                lblProblemDesc.Visible = true;
                count++;
            }
            if (!rdoPhysical.Checked && !rdoLiquid.Checked && !rdoBoth.Checked)
            {
                lblDamageType.Text = "Please select the Type of Damage.";
                lblDamageType.Visible = true;
                count++;
            }
            if (!rblphoneswitchingon.Checked && !rblphoneswitchingnot.Checked)
            {
                lblDeviceSwitchOn.Text = "Please specify whether the device is switching on.";
                lblDeviceSwitchOn.Visible = true;
                count++;
            }
            if (!chkDefectiveParts.Items.Cast<ListItem>().Any(i => i.Selected))
            {
                lblDefectiveParts.Text = "Please select at least one defective part.";
                lblDefectiveParts.Visible = true;
                count++;
            }
            if (!touchworking.Checked && !touchworkingnot.Checked)
            {
                lblTouchWorking.Text = "Please specify whether the touch screen is working.";
                lblTouchWorking.Visible = true;
                count++;
            }
            if (string.IsNullOrWhiteSpace(txtDamageDate.Text))
            {
                lblDamageDate.Text = "Damage date is required.";
                lblDamageDate.Visible = true;
                count++;
            }
            if (string.IsNullOrWhiteSpace(txtDamageTime.Text))
            {
                lblDamageTime.Text = "Damage time is required.";
                lblDamageTime.Visible = true;
                count++;
            }
            if (string.IsNullOrWhiteSpace(txtPlaceOfDamage.Text))
            {
                lblPlaceOfDamage.Text = "Place of damage is required.";
                lblPlaceOfDamage.Visible = true;
                count++;
            }
            if (count > 0)
            {
                return;
            }

            string ticketno = "";
            if (Label2.Text != "")
            {
                lbltkt.Text = "Thank you for submitting the details. <span style='color:red'> Claim is Registered against Ticket No.: " + ticketno.Trim() + ". <span> Please mention this Ticket No. in all future communications.";
                btnRegisterClaim.Text = "Edit";
                btnRegisterClaim.Enabled = false;
                string url = "";
                url = "RegisterNewClaim.aspx?TKT=" + ticketno.Trim() + "&sr=Newcall" + "";
                Response.Redirect(url, false);
            }
            else
            {
                if (lblloan.Text == "")
                {
                    ticketno = LogComplaint(ViewState["sku"].ToString(), ViewState["reg"].ToString());
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("Iapl_crm_usp_getProblemStatus", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 712;
                    cmd.Parameters.AddWithValue("@LoanNo", lblloan.Text.Trim().Replace("UPDATE", "").Replace("Update", "").Replace("INSERT", "").Replace("Insert", "").Replace("insert", "").Replace("Delete", "").Replace("delete", "").Replace("Drop", "").Replace("Truncate", "").Replace("--", "").Replace("update", "").Replace("='", ""));

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        string TCKT = dt.Rows[0]["TicketNO"].ToString();
                        if (TCKT == string.Empty)
                        {
                            ticketno = LogComplaint(ViewState["sku"].ToString(), ViewState["reg"].ToString());
                        }
                        else
                        {
                            Label5.Text = "Ticket No. : " + TCKT.Trim();
                            ticketno = TCKT;
                        }
                    }
                    else
                    {
                        ticketno = LogComplaint(ViewState["sku"].ToString(), ViewState["reg"].ToString());
                    }

                    if (ticketno != "")
                    {
                        SaveProxyinfo();
                        Label5.Visible = true;
                        Label2.Text = ticketno.Trim();
                        lbltkt.Text = "Thank you for submitting the details. <span style='color:red'> Claim is Registered against Ticket No.: " + ticketno.Trim() + ". <span> Please mention this Ticket No. in all future communications.";
                        btnRegisterClaim.Text = "Edit";
                        btnRegisterClaim.Enabled = false;
                        textboxdisable();
                        divdoc.Visible = true;
                        btndownload.Visible = true;

                        string url = "";
                        url = "RegisterNewClaim.aspx?TKT=" + ticketno.Trim() + "&sr=Newcall" + "";
                        Response.Redirect(url, false);
                    }
                    else
                    {
                        DisplayMessage(this, "server Error !!");
                        return;
                    }
                }
            }
        }

        protected string LogComplaint(string SKU, string Regno)
        {
            string tic = string.Empty;
            string ret = "";
            tic = GenerateTicketno();
            ViewState["NewTicket"] = tic.ToString();

            int status = 0;
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["dtvalue"];

            int checkReocrd = 0;
            bool flag = false;
            DateTime Todaydate = DateTime.Now;
        
            string damagedate = Convert.ToDateTime(txtDamageDate.Text).ToString("MM/dd/yyyy");
            string txtFormClaimDatereg = Convert.ToDateTime(DateTime.Now).ToString("MM/dd/yyyy");
            string txtFormClaimTimeR = Convert.ToDateTime(DateTime.Now).ToString("hh:mm tt");

            string[] array = SKU.Split(new char[] { ' ' }, 1);
            string firstItem = array[0];
            string remainingItems = string.Join(" ", array.Skip(0).ToList());
            SqlCommand cmds = new SqlCommand("sp_iapl_PartnerRetailer", con);
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 35;
            cmds.Parameters.AddWithValue("@SKU", SqlDbType.VarChar).Value = remainingItems;
            cmds.Parameters.AddWithValue("@ticketno", SqlDbType.VarChar).Value = tic;
            cmds.Parameters.AddWithValue("@RegistrationNo", SqlDbType.VarChar).Value = Regno;
            cmds.Parameters.AddWithValue("@ProblemNo", SqlDbType.Int).Value = 1;
            cmds.Parameters.AddWithValue("@CallSource", SqlDbType.Int).Value = 15;
            cmds.Parameters.AddWithValue("@CallPriority", SqlDbType.Int).Value = 1;
            cmds.Parameters.AddWithValue("@CallType", SqlDbType.Int).Value = 28;
            cmds.Parameters.AddWithValue("@CallTypes", SqlDbType.Int).Value = 9;
            cmds.Parameters.AddWithValue("@OtherProblemDescription", SqlDbType.VarChar).Value = txtProblemDesc.Value;
            cmds.Parameters.AddWithValue("@Symptoms", SqlDbType.Int).Value = 0;
            cmds.Parameters.AddWithValue("@otherSymptoms", SqlDbType.VarChar).Value = txtProblemDesc.Value;
            cmds.Parameters.AddWithValue("@ProblemReported", SqlDbType.VarChar).Value = txtProblemDesc.Value;
            if (txtRemarks.Value == "")
            {
                txtRemarks.Value = "NEW CALL";
            }
            cmds.Parameters.AddWithValue("@InfinityRemarks", SqlDbType.VarChar).Value = txtRemarks.Value;
            cmds.Parameters.AddWithValue("@CallAction", SqlDbType.Int).Value = 201;
            cmds.Parameters.AddWithValue("@CallStatus", SqlDbType.Int).Value = 19;
            cmds.Parameters.AddWithValue("@ServiceType", SqlDbType.Int).Value = 1;
            cmds.Parameters.AddWithValue("@DamageDateTime", SqlDbType.VarChar).Value = damagedate.Replace('-', '/') + " " + txtDamageTime.Text;
            cmds.Parameters.AddWithValue("@Claim_settlement", SqlDbType.VarChar).Value = "ASC Not Yet Assigned";
            cmds.Parameters.AddWithValue("@MIS_status", SqlDbType.VarChar).Value = "Documents Pending, Telephonic Follow-up Stage";
            cmds.Parameters.AddWithValue("@claimstatus", SqlDbType.VarChar).Value = "Open";
            cmds.Parameters.AddWithValue("@claimcalltype", SqlDbType.VarChar).Value = "Claim";
            cmds.Parameters.AddWithValue("@FG_emailid", SqlDbType.VarChar).Value = "";
            cmds.Parameters.AddWithValue("@FG_Resourceid", SqlDbType.Int).Value = 0;
            cmds.Parameters.AddWithValue("@CauseRemark", SqlDbType.VarChar).Value = "104";
            cmds.Parameters.AddWithValue("@ActionStatus", SqlDbType.VarChar).Value = "59";
            cmds.Parameters.AddWithValue("@FunctionareaID", SqlDbType.Int).Value = 15;
            cmds.Parameters.AddWithValue("@userID", SqlDbType.Int).Value = Session["RetailerUniqueID"].ToString();

            string TypeDamage = string.Empty;

            if (rdoPhysical.Checked)
            {
                TypeDamage = "Physical";
            }
            else if (rdoLiquid.Checked)
            {
                TypeDamage = "Liquid";
            }
            else if (rdoBoth.Checked)
            {
                TypeDamage = "Both";
            }
            TypeDamage = TypeDamage.TrimEnd(',');
            cmds.Parameters.AddWithValue("@TypeofDamage", SqlDbType.VarChar).Value = TypeDamage;
            string deviceSwitchingOnValue = "";

            if (rblphoneswitchingon.Checked)
            {
                deviceSwitchingOnValue = "Yes";
            }
            else if (rblphoneswitchingnot.Checked)
            {
                deviceSwitchingOnValue = "No";
            }
            cmds.Parameters.AddWithValue("@DeviceSwitchingOn", deviceSwitchingOnValue);

            string partsname = string.Empty;
            for (int i = 0; i < chkDefectiveParts.Items.Count; i++)
            {
                if (chkDefectiveParts.Items[i].Selected == true)
                {
                    partsname += chkDefectiveParts.Items[i].Text + ",";
                }
            }
            partsname = partsname.TrimEnd(',');
            cmds.Parameters.AddWithValue("@PartsDamaged", SqlDbType.VarChar).Value = partsname;
            cmds.Parameters.AddWithValue("@PlaceofDamage", SqlDbType.VarChar).Value = txtPlaceOfDamage.Text;
            cmds.Parameters.AddWithValue("@UserName", SqlDbType.VarChar).Value = Session["Name"].ToString();
            string touchWorkingValue = "";

            if (touchworking.Checked)
            {
                touchWorkingValue = "Yes";
            }
            else if (touchworkingnot.Checked)
            {
                touchWorkingValue = "No";
            }

            cmds.Parameters.AddWithValue("@TouchWorking", touchWorkingValue);

            string DateCurr = DateTime.Now.ToShortDateString();
            DateTime TimeCurr = DateTime.Now.ToLocalTime();
            string Cureent = Convert.ToString(DateCurr);
            cmds.Parameters.AddWithValue("@ClaimDate", SqlDbType.VarChar).Value = txtFormClaimDatereg.Replace('-', '/');
            cmds.Parameters.AddWithValue("@ClaimTime", SqlDbType.VarChar).Value = txtFormClaimTimeR.Trim();

            con.Open();
            checkReocrd = (int)cmds.ExecuteNonQuery();
            con.Close();

            string Ticket2 = string.Empty;
            string regno = string.Empty;
            if (checkReocrd > 0)
            {
                flag = true;
                ret = tic;
            }
            return ret;
        }

        protected string GenerateTicketno()
        {
            SqlCommand cmd = new SqlCommand("IAPL_CRM_stockgenerate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 5;
            cmd.Parameters.Add("@ticketno", SqlDbType.VarChar, 100);
            cmd.Parameters["@ticketno"].Direction = ParameterDirection.Output;
            con.Open();
            int check = cmd.ExecuteNonQuery();
            con.Close();
            ViewState["tickeno"] = cmd.Parameters["@ticketno"].Value.ToString();
            string checkticket = ViewState["tickeno"].ToString();
            return ViewState["tickeno"].ToString();
        }
        protected void SaveProxyinfo()
        {
            string type = "10";
            SqlCommand cmd = new SqlCommand("sp_Promoter_claim", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", type);
            cmd.Parameters.AddWithValue("@ProxyMobile1", Session["P_MobileNo"].ToString());
            cmd.Parameters.AddWithValue("@P_Name", Session["P_Name"].ToString());
            cmd.Parameters.AddWithValue("@P_EmailID", Session["P_EmailID"].ToString());
            cmd.Parameters.AddWithValue("@P_Relationship", Session["P_Relationship"].ToString());
            cmd.Parameters.AddWithValue("@skuandserialno", ViewState["sku"].ToString().ToString());

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
        }
        protected void Deletefile(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnk = (LinkButton)sender;
                GridViewRow row = (GridViewRow)lnk.NamingContainer;
                System.Web.UI.WebControls.Label id = (System.Web.UI.WebControls.Label)row.FindControl("lblid");
                System.Web.UI.WebControls.Label tit = (System.Web.UI.WebControls.Label)row.FindControl("lbldocumentname");
                System.Web.UI.WebControls.Label lblticketno = (System.Web.UI.WebControls.Label)row.FindControl("lblticketno");
                TextBox txthold = (TextBox)row.FindControl("txthold");
                if (txthold.Text.Trim() != "")
                {
                    SqlCommand cmd = new SqlCommand("sp_iapl_chatbot_New", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 11;
                    cmd.Parameters.AddWithValue("@mid", SqlDbType.VarChar).Value = id.Text;
                    cmd.Parameters.AddWithValue("@docremarks", SqlDbType.VarChar).Value = txthold.Text.Trim();
                    cmd.Parameters.AddWithValue("@Deleted_By", SqlDbType.VarChar).Value = (string)Session["ClientIdnameclient"];

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        doclist(lblticketno.Text.Trim());
                        DisplayMessage(this, "Document updated  Successfully for Request To Ignore (" + tit.Text + ")");
                        return;
                    }
                    else
                    {
                        DisplayMessage(this, "Error!!");
                        return;
                    }
                }
                else
                {
                    txthold.Focus();
                    DisplayMessage(this, "Please submit resion  for request !!");
                    return;
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
                return;
            }
        }
        public void doclist(string tic)
        {
            SqlCommand cmd = new SqlCommand("sp_Promoter_claim", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 6;
            cmd.Parameters.AddWithValue("@ticketno", SqlDbType.Int).Value = tic;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.Visible = true;
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.EmptyDataText = "No documents are available";
                GridView1.DataBind();
            }
        }
        protected void DownloadAll(object sender, EventArgs e)
        {
            string CRNONO = txtFirstName.Text.Trim().Replace(" ", "_") + "_" + txtIMEI.Text.Trim();
            string Mfilename = "";
            string filename = txtIMEI.Text.Trim();
            string custname = txtFirstName.Text;
            string TicketNo = ViewState["TicketNo"].ToString();
            if (CRNONO != "")
            {
                Mfilename = CRNONO;
            }
            else
            {
                Mfilename = TicketNo;
            }
            int i = 0;
            using (ZipFile zip = new ZipFile())
            {
                foreach (GridViewRow gvrow in GridView1.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                    System.Web.UI.WebControls.Label doc = (System.Web.UI.WebControls.Label)gvrow.FindControl("lbldownload");
                    if (chk.Checked)
                    {
                        i++;
                        string fileName = doc.Text;
                        string filePath = Server.MapPath("../Documents/" + fileName);
                        zip.AddFile(filePath, filename);
                    }
                }
                if (i > 0)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Mfilename + ".zip");
                    Response.ContentType = "application/zip";
                    zip.Save(Response.OutputStream);
                    Response.End();
                }
                else
                {
                    DisplayMessage(this, "Please select file");
                    return;
                }
            }
        }
        protected void gvFiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string FilePath2 = ConfigurationManager.AppSettings["FilePath2"].ToString();
            string FilePath4 = ConfigurationManager.AppSettings["FilePath4"].ToString();
            if (e.CommandName == "Download")
            {
                if (File.Exists(FilePath2 + (string)e.CommandArgument))
                {
                    Response.Clear();
                    Response.ContentType = "image/pdf";
                    Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
                    string docs = FilePath2 + e.CommandArgument;
                    Response.TransmitFile(FilePath2 + e.CommandArgument);
                    Response.End();
                }
                else if (File.Exists(FilePath4 + (string)e.CommandArgument))
                {
                    Response.Clear();
                    Response.ContentType = "image/pdf";
                    Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
                    string docs = FilePath4 + e.CommandArgument;
                    Response.TransmitFile(FilePath4 + e.CommandArgument);
                    Response.End();
                }
                else
                {
                    DisplayMessage(this, "File is not available for download");
                    return;
                }
            }
            else if (e.CommandName == "ViewDOCopen")
            {
                string url = "";
                if (UrlExists(FilePath2 + (string)e.CommandArgument))
                {
                    url = FilePath2 + (string)e.CommandArgument;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + url + "');", true);
                }
                else if (UrlExists(FilePath4 + e.CommandArgument))
                {
                    url = FilePath4 + e.CommandArgument;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), Guid.NewGuid().ToString(), "window.open('" + url + "');", true);
                }
                else
                {
                    string redirectUrl = ("https://azureblob.infyshield.com/Get_Docblob.aspx?filename=" + (string)e.CommandArgument);
                    Response.Write("<script>window.open('" + redirectUrl + "', '_blank');</script>");
                }
            }
        }
        private bool UrlExists(string url)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "HEAD";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return (response.StatusCode == HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        protected void getdocuments()
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            SqlCommand cmd = new SqlCommand("sp_Promoter_claim", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 13;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ddldocumentattached2.DataSource = dt;
                ddldocumentattached2.DataTextField = "DocumentName";
                ddldocumentattached2.DataValueField = "mid";
                ddldocumentattached2.DataBind();
                ddldocumentattached2.Items.Insert(0, "Select Document Title");
            }
        }
        protected void UploadImage1(object sender, EventArgs e)
        {
            if (ddldocumentattached2.SelectedIndex == 0)
            {
                DisplayMessage(this, "Please select Documnts type");
                ddldocumentattached2.Focus();
                return;
            }
            if (Label2.Text != null)
            {
                string Ticket = Label2.Text;
                try
                {
                    nickname();
                    if (ddldocumentattached2.SelectedIndex == 0)
                    {
                        DisplayMessage(this, "Please select Documnts type");
                        ddldocumentattached2.Focus();
                        return;
                    }
                    else
                    {
                        ViewState["Mis_Report"] = "1";
                        ViewState["docstatus"] = "VP";

                        string FilePath1 = ConfigurationManager.AppSettings["FilePath1"].ToString();
                        string Nickname_doc = ViewState["nick_name"].ToString();
                        if (fupupload2.HasFile)
                        {
                            string ext = System.IO.Path.GetExtension(this.fupupload2.PostedFile.FileName);
                            String yy = DateTime.Now.Year.ToString();
                            String mn = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);

                            bool existsClient = System.IO.Directory.Exists(ViewState["filePath"].ToString() + "/" + ViewState["Client"].ToString());
                            if (!existsClient)
                                System.IO.Directory.CreateDirectory(ViewState["filePath"].ToString() + "/" + ViewState["Client"].ToString());
                            bool existsYear = System.IO.Directory.Exists(ViewState["filePath"].ToString() + "/" + ViewState["Client"].ToString() + "/" + yy);
                            if (!existsYear)
                                System.IO.Directory.CreateDirectory(ViewState["filePath"].ToString() + "/" + ViewState["Client"].ToString() + "/" + yy);
                            bool existsMonth = System.IO.Directory.Exists(ViewState["filePath"].ToString() + "/" + ViewState["Client"].ToString() + "/" + yy + "/" + mn);
                            if (!existsMonth)
                                System.IO.Directory.CreateDirectory(ViewState["filePath"].ToString() + "/" + ViewState["Client"].ToString() + "/" + yy + "/" + mn);
                            if (ext.ToLower() == ".jpeg" || ext.ToLower() == ".jpg" || ext.ToLower() == ".png" || ext.ToLower() == ".pdf" || ext.ToLower() == ".mp4")
                            {
                                if (fupupload2.HasFile)
                                {
                                    string originalFileName = fupupload2.PostedFile.FileName;
                                    string sanitizedFileName = SanitizeFileName(originalFileName);
                                    sanitizedFileName = sanitizedFileName.Replace(" ", "_");
                                    string fn = Ticket.ToString().Replace("/", "") + '_' + Nickname_doc + '_' + sanitizedFileName.Replace(" ", "_");

                                    fupupload2.SaveAs(ViewState["filePath"].ToString() + ViewState["Client"].ToString() + "/" + yy + "/" + mn + "/" + fn);
                                    ViewState["picnamenew"] = fn.ToString();

                                    SqlCommand cmd = new SqlCommand("Iapl_crm_usp_getProblemStatus", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 710;
                                    cmd.Parameters.AddWithValue("@TicketNo", SqlDbType.NVarChar).Value = Ticket;
                                    cmd.Parameters.AddWithValue("@UserName", SqlDbType.NVarChar).Value = Session["Name"].ToString();
                                    cmd.Parameters.AddWithValue("@doc", ddldocumentattached2.SelectedItem.Value);
                                    cmd.Parameters.AddWithValue("@path", ViewState["Client"].ToString() + "/" + yy + "/" + mn + "/" + fn);
                                    cmd.Parameters.AddWithValue("@DocStatus", SqlDbType.VarChar).Value = ViewState["docstatus"].ToString();
                                    cmd.Parameters.AddWithValue("@Mis_Report", SqlDbType.VarChar).Value = ViewState["Mis_Report"].ToString();

                                    con.Open();
                                    int check1 = cmd.ExecuteNonQuery();
                                    con.Close();
                                    if (check1 > 0)
                                    {
                                        doclist(Ticket);
                                        ddldocumentattached2.SelectedIndex = 0;
                                        DisplayMessage(this, "Document uploaded Successfully");
                                        return;
                                    }
                                    else
                                    {
                                        ViewState["picname"] = null;
                                        DisplayMessage(this, "Error !!! in Document upload");
                                        return;
                                    }
                                }
                                else
                                {
                                    DisplayMessage(this, "Please Upload a file");
                                    return;
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisplayMessage(this, ex.Message);
                    return;
                }
            }
            else
            {
                DisplayMessage(this, "Ticket number not open ");
                return;
            }
        }

        protected void nickname()
        {
            string Ticket = Label2.Text;
            string Checklist22 = ddldocumentattached2.SelectedItem.Value;

            SqlCommand cmd = new SqlCommand("sp_iapl_chatbot_New", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 13;
            cmd.Parameters.AddWithValue("@mid", SqlDbType.NVarChar).Value = Checklist22;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ViewState["nick_name"] = dt.Rows[0]["Doc_nickName"].ToString().Trim();
            }
        }
        private string SanitizeFileName(string fileName)
        {
            string pattern = "[^a-zA-Z0-9-_\\. ]";
            string sanitizedFileName = Regex.Replace(fileName, pattern, "");
            return sanitizedFileName;
        }
        public void ChangeColour(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.Label lblDocStatus = (e.Row.FindControl("lblverify") as System.Web.UI.WebControls.Label);
                if (lblDocStatus.Text == "Approved")
                {
                    e.Row.ForeColor = Color.Green;
                }
                else if (lblDocStatus.Text == "Verification Pending")
                {
                    e.Row.BackColor = Color.Yellow;
                }
                else if (lblDocStatus.Text == "Reverification")
                {
                }
                else if (lblDocStatus.Text == "Reject")
                {
                    e.Row.ForeColor = Color.Red;
                }
                else if (lblDocStatus.Text == "Not-Verifiable")
                {
                    e.Row.ForeColor = Color.Red;
                }
                else if (lblDocStatus.Text == "")
                {
                }
            }
        }
        protected void textboxdisable()
        {
            txtProblemDesc.Attributes.Add("Readonly", "readonly");
            rdoPhysical.Enabled = false;
            rdoLiquid.Enabled = false;
            rdoBoth.Enabled = false;
            rblphoneswitchingon.Enabled = false;
            rblphoneswitchingnot.Enabled = false;
            chkDefectiveParts.Enabled = false;
            touchworking.Enabled = false;
            touchworkingnot.Enabled = false;
            txtDamageDate.Enabled = false;
            txtDamageTime.Enabled = false;
            txtPlaceOfDamage.Enabled = false;
            txtRemarks.Attributes.Add("Readonly", "readonly");
        }

    }
}