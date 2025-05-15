using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Patner_Retailer_ADO
{
    public partial class ViewClaims : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iaplConnectionString"].ConnectionString);
        static public void DisplayMessage(Control page, string msg)
        {
            string msg1 = String.Format("alert('{0}');", msg);
            ScriptManager.RegisterStartupScript(page, page.GetType(), "msg", msg1, true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ViewState["filePath"] = System.Configuration.ConfigurationManager.AppSettings["FilePath3"];
                ViewState["filePath2"] = System.Configuration.ConfigurationManager.AppSettings["FilePath2"];
                txtddate.Attributes.Add("ReadOnly", "readonly");
                txtdtime.Attributes.Add("ReadOnly", "readonly");               


                string TicketNO = "";



                if (Request.QueryString["TKT"] != null)
                {
                    TicketNO = Request.QueryString["TKT"].ToString();

                }

                else if (Request.QueryString["ticket"] != null)
                {
                    string TicketNO1 = Request.QueryString["ticket"].ToString();
                    string que = qu(TicketNO1);
                    Response.Redirect("ViewClaims.aspx?qu=" + que + "&sr=" + TicketNO1);

                    TicketNO = TicketNO1;




                }

                else if (Request.QueryString["qu"] != null && Request.QueryString["sr"] != null)
                {
                    string tick = "";
                    tick = tkt(Request.QueryString["qu"].ToString(), Request.QueryString["sr"].ToString());

                    if (tick == "")
                    {

                        this.Controls.Clear();
                        string html = "<iframe src='error.aspx' style='border: 0; width: 100%; height: 100%'></iframe>";


                    }


                    else
                    {
                        TicketNO = tick;
                        ViewState["TicketNo"] = TicketNO;


                        Label5.Visible = true;
                        Label2.Text = TicketNO;


                        bindticket(TicketNO);

                        linkGen(TicketNO);
                        getdocuments();
                        InsertFPMSLog();
                    }

                }


                else
                {
                    Label5.Visible = false;

                }




            }
        }



        protected void linkGen(string TicketNO)
        {
            try
            {
                divlinkgn.Visible = false;
                SqlCommand cmd = new SqlCommand("sp_Promoter_claim", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TicketNo", TicketNO);
                cmd.Parameters.AddWithValue("@Type", 62);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    divlinkgn.Visible = true;
                    string Total = dt.Rows[0]["Total"].ToString();
                    string ActionStatus = dt.Rows[0]["ActionStatus"].ToString();

                    if (Convert.ToDecimal(Total) < 1)
                    {
                        A1.InnerText = "https://connect.infyshield.com/SCMS/fgiclaimform.aspx?TicketNo=" + TicketNO.Trim();
                        A1.HRef = "https://connect.infyshield.com/SCMS/fgiclaimform.aspx?TicketNo=" + TicketNO.Trim();
                    }
                    else
                    {

                        Label4.Text = "Resubmission not required";
                    }

                    if (ActionStatus == "19")
                    {

                        if (lblClaim_settlement.Text == "Total Loss" || lblClaim_settlement.Text == "Total Loss With Device Collection")
                        {
                            A2.InnerText = "https://connect.infyshield.com/SCMS/fgineftform.aspx?TicketNo=" + TicketNO.Trim();
                            A2.HRef = "https://connect.infyshield.com/SCMS/fgineftform.aspx?TicketNo=" + TicketNO.Trim();
                        }
                        else
                        {
                            A2.InnerText = " https://connect.infyshield.com/SCMS/fgiclaimreimbursementrequest.aspx?ticketNo=" + TicketNO.Trim();
                            A2.HRef = " https://connect.infyshield.com/SCMS/fgiclaimreimbursementrequest.aspx?ticketNo=" + TicketNO.Trim();
                        }
                    }
                    if (ActionStatus == "50")
                    {
                        A4.InnerText = " https://connect.infyshield.com/SCMS/online_payment.aspx?TicketNo=" + TicketNO.Trim();
                        A4.HRef = " https://connect.infyshield.com/SCMS/online_payment.aspx?TicketNo=" + TicketNO.Trim();
                    }


                    if (ActionStatus == "68")
                    {

                        A3.InnerText = " https://connect.infyshield.com/SCMS/Consent.aspx?TicketNo=" + TicketNO.Trim();
                        A3.HRef = " https://connect.infyshield.com/SCMS/Consent.aspx?TicketNo=" + TicketNO.Trim();
                    }
                }
                else
                {
                    divlinkgn.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }

        }


        protected void InsertFPMSLog()
        {

            SqlCommand cmd = new SqlCommand("sp_Promoter_claim", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", 60);

            cmd.Parameters.AddWithValue("@TicketNo", Label2.Text.Trim().Replace("UPDATE", "").Replace("Update", "").Replace("INSERT", "").Replace("Insert", "").Replace("insert", "").Replace("Delete", "").Replace("delete", "").Replace("Drop", "").Replace("Truncate", "").Replace("--", "").Replace("update", "").Replace("='", ""));
            cmd.Parameters.AddWithValue("@P_MobileNo", Session["MobileNo"].ToString().Trim().Replace("UPDATE", "").Replace("Update", "").Replace("INSERT", "").Replace("Insert", "").Replace("insert", "").Replace("Delete", "").Replace("delete", "").Replace("Drop", "").Replace("Truncate", "").Replace("--", "").Replace("update", "").Replace("='", ""));
            //cmd.Parameters.AddWithValue("@P_EmailID", Session["P_EmailID"].ToString().Trim().Replace("UPDATE", "").Replace("Update", "").Replace("INSERT", "").Replace("Insert", "").Replace("insert", "").Replace("Delete", "").Replace("delete", "").Replace("Drop", "").Replace("Truncate", "").Replace("--", "").Replace("update", "").Replace("='", ""));
            cmd.Parameters.AddWithValue("@P_Name", Session["Name"].ToString().Trim());
            cmd.Parameters.AddWithValue("@P_Relationship", "1");
            con.Close();
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {



            }

        }



        public string qu(string tkt)
        {
            string msg = "";
            if (tkt != "")
            {
                SqlCommand cmd = new SqlCommand("sp_GetCustomerDetailsAgainstticket", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 11117;
                cmd.Parameters.AddWithValue("@TicketNo", SqlDbType.VarChar).Value = tkt.ToString();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    msg = dt.Rows[0]["qu"].ToString();

                }
            }
            return msg;

        }



        public string tkt(string qu, string tkt)
        {
            string msg = "";
            if (qu != "" && tkt != "")
            {

                SqlCommand cmd = new SqlCommand("sp_GetCustomerDetailsAgainstticket", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 11119;
                cmd.Parameters.AddWithValue("@TicketNo", SqlDbType.VarChar).Value = qu.ToString();
                cmd.Parameters.AddWithValue("@TicketNo1", SqlDbType.VarChar).Value = tkt.ToString();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    msg = dt.Rows[0]["TicketNO"].ToString();

                }
            }
            return msg;

        }



        public void GetLoanno(string tic)
        {

            if (Request.QueryString.Count > 0)
            {
                SqlCommand cmd = new SqlCommand("sp_iapl_chatbot_New", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ticketno", tic);
                cmd.Parameters.AddWithValue("@Type", 10);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {


                    ViewState["Crn_No"] = dt.Rows[0]["Crn_No"].ToString();
                    ViewState["loan"] = dt.Rows[0]["LoanNo"].ToString();

                }
            }
        }




        public void bindticket(string tic)
        {
            //

            if (Request.QueryString.Count > 0)
            {


                SqlCommand cmd = new SqlCommand("sp_Promoter_claim", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ticketno", tic);
                cmd.Parameters.AddWithValue("@Type", 5);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    ViewState["Client"] = dt.Rows[0]["NikeName"].ToString();
                    lbladdress.Text = dt.Rows[0]["Address"].ToString();
                    lblmobile.Text = dt.Rows[0]["mobileno"].ToString();
                    lblemail.Text = dt.Rows[0]["emailidaddress"].ToString();
                    lblcity.Text = dt.Rows[0]["City"].ToString();
                    lblpin.Text = dt.Rows[0]["pincode"].ToString();
                    //   lblname.Text = dt.Rows[0]["customername"].ToString();
                    lblname2.Text = dt.Rows[0]["customername"].ToString();
                    lblbrand.Text = dt.Rows[0]["make"].ToString() + ", " + dt.Rows[0]["Model"].ToString();
                    lblIMEINo.Text = dt.Rows[0]["IMEINo"].ToString();
                    lblsate.Text = dt.Rows[0]["state"].ToString();
                    lblloan.Text = dt.Rows[0]["loanno"].ToString();
                    lblcertificate.Text = dt.Rows[0]["certificateno"].ToString();
                    lblBranchcode.Text = dt.Rows[0]["BBCodename"].ToString();
                    lblBranchName.Text = dt.Rows[0]["EECodename"].ToString();

                    lblclamno.Text = dt.Rows[0]["CrnNo"].ToString();
                    lblSumAssured.Text = dt.Rows[0]["Sumassured"].ToString();

                    lblplan.Text = dt.Rows[0]["Plan_Name"].ToString();
                    lblplanperiod.Text = dt.Rows[0]["Policy_Start_Date"].ToString() + " To " + dt.Rows[0]["Policy_End_date"].ToString();
                    ViewState["loan"] = lblloan.Text;
                    if (lblclamno.Text == "&nbsp;")
                    {
                        ViewState["Crnno"] = "";
                    }
                    else
                    {
                        ViewState["Crnno"] = lblclamno.Text;
                    }

                    ViewState["CurrentOpenCall"] = tic;
                    bindticketstatus(tic);
                    doclist(tic);
                }


            }
        }





        public void bindticketstatus(string tic)
        {
            //


            SqlCommand cmd = new SqlCommand("sp_Promoter_claim", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ticketno", tic);
            cmd.Parameters.AddWithValue("@Type", 14);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {

                string est = dt.Rows[0]["totalestimate"].ToString();
                if (est == "0.00")
                {
                    lbleststatus.Text = "Estimate Pending";
                    lbleststatus.ForeColor = System.Drawing.Color.Red;
                }



                else
                {

                    lbleststatus.Text = " Rs. " + est;
                    lbleststatus.ForeColor = System.Drawing.Color.Green;
                }

                string doc = dt.Rows[0]["totaldoc"].ToString();
                if (doc == "0")
                {
                    lbldocstatus.Text = "(" + dt.Rows[0]["totaldoc"].ToString() + ") Documents ";
                    lbldocstatus.ForeColor = System.Drawing.Color.Red;

                }
                else
                {
                    lbldocstatus.Text = "(" + dt.Rows[0]["totaldoc"].ToString() + ") Documents ";
                    lbldocstatus.ForeColor = System.Drawing.Color.Green;
                }



                string ascd = "";
                ascd = dt.Rows[0]["ServiceCenterName"].ToString();
                if (ascd == "")
                {
                    lblascststus.Text = "Pending for Allocation";
                    lblascststus.ForeColor = System.Drawing.Color.Red;
                }

                else
                {
                    lblascststus.Text = ascd;

                    lblascststus.ForeColor = System.Drawing.Color.Green;
                }





                lblproblemreported.Text = dt.Rows[0]["ProblemReported"].ToString();
                lbltypeofdamage.Text = dt.Rows[0]["TypeofDamage"].ToString();
                string DeviceSwtich = dt.Rows[0]["DeviceSwitchingOn"].ToString();

                if (DeviceSwtich == "1")
                {
                    lbldeviceswitchon.Text = "Yes";
                }
                else if (DeviceSwtich == "2")
                {
                    lbldeviceswitchon.Text = "Yes";
                }
                else
                {
                    lbldeviceswitchon.Text = DeviceSwtich;

                }
                lblpartdamaged.Text = dt.Rows[0]["PartsDamaged"].ToString();
                lblplaceofdamage.Text = dt.Rows[0]["PlaceofDamage"].ToString();
                lbltouchscreenworking.Text = dt.Rows[0]["TouchWorking"].ToString();
                lbldamagedatae.Text = dt.Rows[0]["Date_of_Loss"].ToString() + "  " + dt.Rows[0]["Loss_Time"].ToString();
                lblClaim_settlement.Text = dt.Rows[0]["Claim_settlement"].ToString();
                string CallstatusT = dt.Rows[0]["CallStatus"].ToString();

                if (lblClaim_settlement.Text.Trim() == "Total Loss" && CallstatusT == "2")
                {

                    Label1.Text = "Please note that the claim settlement in Total Loss is done directly by FGI-HO into Customer Bank Account in 7-8 Business Days from the Date of Call Closed as shown here.";
                }
                else
                {
                    Label1.Text = "";
                }
                lblremarks.Text = dt.Rows[0]["ProblemRemarks_New"].ToString();  // Customer 
                Label9.Text = dt.Rows[0]["Remarksforclient"].ToString();  // Client 
                lblcallstatus.Text = dt.Rows[0]["Status"].ToString() + "&nbsp;&nbsp; /  &nbsp;&nbsp;" + " Last Updated  " + dt.Rows[0]["lastupdateDate"].ToString();

                if ((Session["MobileNo"].ToString() == "9619171733") || (Session["MobileNo"].ToString() == "9890115116") || (Session["MobileNo"].ToString() == "9082517504"))
                {
                    divrem1.Visible = true;
                    divrem2.Visible = true;
                }
                else
                {
                    divrem2.Visible = true;

                }
                if (dt.Rows[0]["claimstatus"].ToString() != "Open")
                {
                    lblcallstatus.Text = "Ticket Closed" + "&nbsp;&nbsp; /  &nbsp;&nbsp;" + "  Last Updated  " + dt.Rows[0]["lastupdateDate"].ToString() + "&nbsp;&nbsp; /  &nbsp;&nbsp;" + "  Call Closed  " + dt.Rows[0]["lastupdateDate"].ToString();
                }

                if (lblproblemreported.Text.Trim() == "")
                {
                    divproblemupdate.Visible = true;
                }
                else
                {
                    divproblemupdate.Visible = false;
                }



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

        private string SanitizeFileName(string fileName)
        {
            // Define a regular expression pattern to match unwanted characters
            string pattern = "[^a-zA-Z0-9-_\\. ]";
            // Replace unwanted characters with empty string
            string sanitizedFileName = Regex.Replace(fileName, pattern, "");

            return sanitizedFileName;
        }


        protected void UploadImage1(object sender, EventArgs e)
        {
            if (ddldocumentattached2.SelectedIndex == 0)
            {
                DisplayMessage(this, "Please select Documnts type");
                ddldocumentattached2.Focus();
                return;
            }

            if (ViewState["CurrentOpenCall"] != null)
            {
                string Ticket = ViewState["CurrentOpenCall"].ToString();

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




                        ViewState["docstatus"] = "VP";
                        ViewState["Mis_Report"] = "1";

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

                                    // Validate and sanitize file name
                                    string sanitizedFileName = SanitizeFileName(originalFileName);

                                    // Optionally, replace spaces with underscores or hyphens
                                    sanitizedFileName = sanitizedFileName.Replace(" ", "_");


                                    string fn = Ticket.ToString().Replace("/", "") + '_' + Nickname_doc + '_' + sanitizedFileName.Replace(" ", "_");
                                    //fupupload2.SaveAs(Server.MapPath("../Documents/" + fn.ToString()));
                                    fupupload2.SaveAs(ViewState["filePath"].ToString() + ViewState["Client"].ToString() + "/" + yy + "/" + mn + "/" + fn);
                                    ViewState["picnamenew"] = fn.ToString();

                                    SqlCommand cmd = new SqlCommand("Iapl_crm_usp_getProblemStatus", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 710;
                                    cmd.Parameters.AddWithValue("@TicketNo", SqlDbType.NVarChar).Value = Ticket;
                                    cmd.Parameters.AddWithValue("@UserName", SqlDbType.NVarChar).Value = Session["UserName"].ToString();
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
                                        bindticketstatus(Ticket);
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

                                DisplayMessage(this, "Please Upload .png or .jpeg format file");
                                return;

                            }
                        }
                        else
                        {
                            DisplayMessage(this, "Please Upload .png or .jpeg format file");
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
            ViewState["nick_name"] = "";
            string Ticket = ViewState["CurrentOpenCall"].ToString();
            string Checklist22 = ddldocumentattached2.SelectedItem.Value;

            // string TCT = ViewState["TicketNo"].ToString();
            string NA = "1";
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

        private bool UrlExists(string url)
        {
            try
            {
                // Force TLS 1.2
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;

                // Bypass SSL/TLS certificate validation (not recommended for production)
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
                if (File.Exists(FilePath4 + (string)e.CommandArgument))
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


                string FilePath11 = ConfigurationManager.AppSettings["FilePath1"].ToString();
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


                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + url + "');", true);

            }


        }

        public void doclist(string tic)
        {
            con.Open();
            SqlCommand cmd1 = new SqlCommand("IAPL_CRM_ServiceCallLog_CRUD", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@Type", 68);

            cmd1.Parameters.AddWithValue("@TicketNumber", SqlDbType.VarChar).Value = tic;


            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            con.Close();

            if (dt1.Rows.Count > 0)
            {
                if (dt1.Rows[0]["DocView_Controlled"].ToString() != "Y")
                {
                    Panelc.Visible = true;
                    SqlCommand cmd = new SqlCommand("sp_Promoter_claim", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 6;
                    cmd.Parameters.AddWithValue("@ticketno", SqlDbType.Int).Value = tic;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        Panelc.Visible = true;
                        GridView2.DataSource = dt;
                        GridView2.DataBind();

                    }
                    else
                    {
                        GridView2.DataSource = null;
                        GridView2.EmptyDataText = "No documents are available";
                        GridView2.DataBind();
                    }
                }
                else
                {
                    Panelc.Visible = false;
                    lbldoccontrolmsg.ForeColor = System.Drawing.Color.Red;
                    lbldoccontrolmsg.Text = "Status of already submitted documents is not available at this moment. Please contact customer care for any help and guidance.";

                }

            }
        }

        protected void btnsave_click(object sender, EventArgs e)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("sp_updateRegiastedData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 1;
                cmd.Parameters.AddWithValue("@ticketno", SqlDbType.Int).Value = Label2.Text.Trim().Replace("UPDATE", "").Replace("Update", "").Replace("INSERT", "").Replace("Insert", "").Replace("insert", "").Replace("Delete", "").Replace("delete", "").Replace("Drop", "").Replace("Truncate", "").Replace("--", "").Replace("update", "").Replace("='", "");
                cmd.Parameters.AddWithValue("@Problem_Reported", SqlDbType.Int).Value = txtpreported.Text.Trim().Replace("UPDATE", "").Replace("Update", "").Replace("INSERT", "").Replace("Insert", "").Replace("insert", "").Replace("Delete", "").Replace("delete", "").Replace("Drop", "").Replace("Truncate", "").Replace("--", "").Replace("update", "").Replace("='", "");
                cmd.Parameters.AddWithValue("@Typeof_damage", SqlDbType.Int).Value = rbltypeofdamage.SelectedValue.Trim();
                cmd.Parameters.AddWithValue("@device_switchon", SqlDbType.Int).Value = rblphoneswitchingonornot.SelectedValue.Trim();
                cmd.Parameters.AddWithValue("@Part_damage", SqlDbType.Int).Value = rblpartdamage.SelectedValue.Trim();
                cmd.Parameters.AddWithValue("@placeofdamage", SqlDbType.Int).Value = txtplacedamage.Text.Trim().Replace("UPDATE", "").Replace("Update", "").Replace("INSERT", "").Replace("Insert", "").Replace("insert", "").Replace("Delete", "").Replace("delete", "").Replace("Drop", "").Replace("Truncate", "").Replace("--", "").Replace("update", "").Replace("='", "");
                cmd.Parameters.AddWithValue("@touchworking", SqlDbType.Int).Value = rbltouchworking.SelectedValue.Trim();
                cmd.Parameters.AddWithValue("@damagedate", SqlDbType.Int).Value = txtddate.Text + " " + txtdtime.Text;
                cmd.Parameters.AddWithValue("@damagetime", SqlDbType.Int).Value = txtdtime.Text.Trim();
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {


                    bindticketstatus(Label2.Text.Trim());


                }

                else
                {
                    DisplayMessage(this, "Error!!");


                    return;

                }

            }
            catch (Exception ex)
            {


            }

        }

        protected void Deletefile(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnk = (LinkButton)sender;
                GridViewRow row = (GridViewRow)lnk.NamingContainer;

                Label id = (Label)row.FindControl("lblid");

                Label tit = (Label)row.FindControl("lbldocumentname");

                Label lblticketno = (Label)row.FindControl("lblticketno");
                TextBox txthold = (TextBox)row.FindControl("txthold");

                if (txthold.Text.Trim() != "")
                {
                    SqlCommand cmd = new SqlCommand("sp_iapl_chatbot_New", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 11;
                    cmd.Parameters.AddWithValue("@mid", SqlDbType.VarChar).Value = id.Text;
                    cmd.Parameters.AddWithValue("@docremarks", SqlDbType.VarChar).Value = txthold.Text.Trim();
                    cmd.Parameters.AddWithValue("@Deleted_By", SqlDbType.VarChar).Value = (string)Session["MobileNo"];
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

        public void checkall(object sender, EventArgs e)
        {
            foreach (GridViewRow gvrow in GridView2.Rows)
            {
                CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                chk.Checked = true;

            }

        }

        public void PROCEEDCHECKALL(object sender, EventArgs args)
        {

            CheckBox txt = (CheckBox)sender;
            GridViewRow rw = (GridViewRow)txt.NamingContainer;
            CheckBox checksAllprocess = (CheckBox)rw.FindControl("checkAllprocess");
            int count = 0;
            foreach (GridViewRow gvrow in GridView2.Rows)
            {
                CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");

                if (checksAllprocess.Checked == true)
                {
                    chk.Checked = true;
                    count++;
                }

                else
                {
                    chk.Checked = false;

                }

            }



        }



    






        protected void docclick(object sebder, EventArgs args)
        {

            string url = "";
            string sr = "";
            url = "uploaded_doc.aspx?ticket=" + ViewState["CurrentOpenCall"].ToString() + "&sr=" + sr;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + url + "', '_blank');", true);


        }



        protected void esimateclick(object sebder, EventArgs args)
        {

            string url = "";
            string sr = "";
            url = "estimate.aspx?ticket=" + ViewState["CurrentOpenCall"].ToString() + "&sr=" + sr;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + url + "', '_blank');", true);


        }


        protected void ascclick(object sebder, EventArgs args)
        {

            string url = "";
            string sr = "";
            url = "asc_details.aspx?ticket=" + ViewState["CurrentOpenCall"].ToString() + "&sr=" + sr;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + url + "', '_blank');", true);


        }


        public void ChangeColour(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDocStatus = (e.Row.FindControl("lblverify") as Label);
                Label lbldocumentname = (e.Row.FindControl("lbldocumentname") as Label);

                Label lblcreateddate = (e.Row.FindControl("lblcreateddate") as Label);



                if (lbldocumentname.Text == "Proof of Payment to Customer" || lbldocumentname.Text == "Proof of Payment to ASC")
                {
                    lblDocStatus.Text = "Approved";
                }

                if (lblDocStatus.Text == "Approved")
                {
                    e.Row.ForeColor = Color.Green;

                }
                else if (lblDocStatus.Text == "Verification Pending")
                {
                    e.Row.BackColor = Color.LightYellow;

                }
                else if (lblDocStatus.Text == "Reverification")
                {
                    //  e.Row.BackColor = Color.Yellow;

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
                    // e.Row.BackColor = Color.Yellow;

                }



            }



        }


        }
    }