using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Patner_Retailer_ADO
{
    public partial class CreateSalesPerson : System.Web.UI.Page
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
                BindDocument();
                DocumentPanel.Visible = false;
                btnUpdate.Visible = false;
                btnnext3.Visible = false;
                txtDOB.Attributes.Add("Readonly", "readonly");
                CalendarExtender3.EndDate = DateTime.Today;
                string querymid = Request.QueryString["Mid"];
                if (!string.IsNullOrWhiteSpace(querymid))
                {
                    btnSubmit.Visible = false;
                    BindSalesPersonInfo(querymid);
                    btnUpdate.Visible = true;
                    Session["UpdateSalesPersonMid"] = querymid;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 7);
                cmd.Parameters.AddWithValue("@RetailerAdminID", Session["RetailerUniqueID"].ToString().Trim());
                cmd.Parameters.AddWithValue("@Name", txtFirstName.Text.Trim());
                cmd.Parameters.AddWithValue("@MobileNo", txtMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@MobileNo_2", txtAltMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@EmailID", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@EmailID_2", txtAltEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", ddlGender.Text.Trim());
                cmd.Parameters.AddWithValue("@DateOfBirth", txtDOB.Text.Trim());
                cmd.Parameters.AddWithValue("@PinCode", txtPIN.Text.Trim());
                cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                cmd.Parameters.AddWithValue("@Address", txtCommAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@BankAccountNumber", txtAccount.Text.Trim());
                cmd.Parameters.AddWithValue("@IFSCCode", txtIFSC.Text.Trim());
                cmd.Parameters.AddWithValue("@AccountHolderName", txtHolder.Text.Trim());
                cmd.Parameters.AddWithValue("@BankName", txtBankName.Text.Trim());
                cmd.Parameters.AddWithValue("@BankBranch", txtBranch.Text.Trim());
                cmd.Parameters.AddWithValue("@BankBranchAddress", txtBranchAddress.Text.Trim());

                con.Open();
                //int i = cmd.ExecuteNonQuery();
                SqlDataReader reader = cmd.ExecuteReader();
                int insertedProfileId = 0;

                if (reader.Read())
                {
                    insertedProfileId = Convert.ToInt32(reader["InsertedProfileId"]);
                }

                reader.Close();
                con.Close();

                DocumentPanel.Visible = true;
                btnnext3.Visible = true;
                Session["InsertedProfileId"] = insertedProfileId;
                ddlDocumentName.Focus();
                //Response.Redirect("ViewSalesPerson.aspx", false);
            }
            catch (SqlException ex)
            {
                //  Check the error number to specifically handle the duplicate mobile number error.
                if (ex.Number == 50001) //  50001 is the error number we threw in the SP.
                {
                    //  Display a user-friendly message.  DO NOT expose the raw SQL error message to the user.
                    //lblError.Text = "Error: A record with this Mobile Number already exists. Please enter a different Mobile Number.";
                    //    lblError.Visible = true; // Make sure the label is visible.
                }
                else
                {
                    //  Handle other SQL Server errors (log them, show a generic error message, etc.).
                    LogError(ex); //  Call a method to log the error.
                                  //  lblError.Text = "An unexpected database error occurred. Please contact support.";
                                  //   lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                //  Handle general exceptions (e.g., connection errors, other unexpected issues).
                LogError(ex);
                //   lblError.Text = "An unexpected error occurred. Please contact support.";
                // lblError.Visible = true;
            }
            finally
            {
                //  Ensure the connection is closed, even if an exception occurs.
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void LogError(Exception ex)
        {
            //  Implement robust error logging here.  
            //  Consider logging to a file, the Windows Event Log, a database table, or a logging framework (like log4net).
            //  Example (basic logging to a file):
            string filePath = Server.MapPath("~/logs/ErrorLog.txt"); //  Use Server.MapPath for file paths in web applications.
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Message);
                    sw.WriteLine("Stack Trace: " + ex.StackTrace);
                    sw.WriteLine("--------------------------------------------------");
                }
            }
            catch (Exception logEx)
            {
                //  Handle any errors that occur during logging.
                //  You might not be able to log to the original destination, so consider an alternative (e.g., writing to the Application Event Log).
                System.Diagnostics.EventLog.WriteEntry("Application", "Error logging failed: " + logEx.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        protected void txtIFSC_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("sp_iapl_crm_newsrvcall", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 11;
            cmd.Parameters.AddWithValue("@IFSC", SqlDbType.NVarChar).Value = txtIFSC.Text;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {

                txtBankName.Text = dt.Rows[0]["BANK"].ToString();
                txtBranch.Text = dt.Rows[0]["BRANCH"].ToString();
                txtBranchAddress.Text = dt.Rows[0]["ADDRESS"].ToString();
                txtBranchAddress.Focus();
                lblIFSCError.Text = "";
            }

            else
            {
                txtBankName.Text = "";
                txtBranch.Text = "";
                txtBranchAddress.Text = "";
                lblIFSCError.Text = "Enter a valid IFSC code";
                txtIFSC.Focus();
            }
        }

        protected void txtPIN_TextChanged(object sender, EventArgs e)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pincode", SqlDbType.Int).Value = txtPIN.Text.Trim();
                cmd.Parameters.AddWithValue("@Type", SqlDbType.Int).Value = 4;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    txtCity.Text = dt.Rows[0]["CityName"].ToString();
                    txtState.Text = dt.Rows[0]["statename"].ToString();
                    txtCity.Focus();
                }
                else
                {


                }
            }
            catch (Exception ex)
            {

                return;
            }
        }
        protected void BindDocument()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 10);

                ddlDocumentName.Items.Clear();
                con.Open();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlDocumentName.DataSource = ds.Tables[0];
                    ddlDocumentName.DataTextField = "DocumentName";
                    ddlDocumentName.DataValueField = "mid";
                    ddlDocumentName.DataBind();

                    ddlDocumentName.Items.Insert(0, new ListItem("-- Select Document --", ""));
                }
                con.Close();
            }
            catch (Exception)
            {
            }
        }

        protected void btnUploadFront_Click(object sender, EventArgs e)
        {
            if (fuFrontSide.HasFile)
            {
                try
                {
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };
                    string fileExtension = Path.GetExtension(fuFrontSide.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "scrollToBottom", "window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' });", true);
                        return;
                    }
                    string docName = ddlDocumentName.SelectedItem.Text;
                    string docNumber = txtDocumentNumber.Text.Trim();
                    string fileName = Path.GetFileName(fuFrontSide.FileName);
                    string folderPath = Server.MapPath("~/UploadedDocuments/");
                    string DocId = ddlDocumentName.SelectedValue.ToString();
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string uniqueFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName;
                    string fullPath = Path.Combine(folderPath, uniqueFileName);
                    fuFrontSide.SaveAs(fullPath);

                    DataTable dt;
                    if (ViewState["DocumentData"] == null)
                    {
                        dt = new DataTable();
                        dt.Columns.Add("SrNo");
                        dt.Columns.Add("Mid");
                        dt.Columns.Add("DocId");
                        dt.Columns.Add("DocumentName");
                        dt.Columns.Add("DocumentNumber");
                        dt.Columns.Add("DocumentPath");
                        dt.Columns.Add("Status");
                        dt.Columns.Add("Size");
                    }
                    else
                    {
                        dt = (DataTable)ViewState["DocumentData"];
                    }

                    bool isDuplicate = dt.AsEnumerable().Any(row => row.Field<string>("DocumentName") == docName);
                    if (isDuplicate)
                    {
                        DisplayMessage(this, "Document already exists.");
                        return;
                    }

                    DataRow existingRow = dt.AsEnumerable().FirstOrDefault(row => row["DocId"].ToString() == DocId);
                    if (existingRow != null)
                    {
                        existingRow["DocumentName"] = docName;
                        existingRow["DocumentNumber"] = docNumber;
                        existingRow["DocumentPath"] = uniqueFileName;
                        existingRow["Status"] = "Updated";
                        existingRow["Size"] = (fuFrontSide.PostedFile.ContentLength / 1024.0).ToString("0.00") + " KB";
                    }
                    else
                    {
                        // Add new row
                        DataRow dr = dt.NewRow();
                        dr["SrNo"] = dt.Rows.Count + 1;
                        dr["Mid"] = 0;
                        dr["DocId"] = DocId;
                        dr["DocumentName"] = docName;
                        dr["DocumentNumber"] = docNumber;
                        dr["DocumentPath"] = uniqueFileName;
                        dr["Status"] = "Uploaded";
                        dr["Size"] = (fuFrontSide.PostedFile.ContentLength / 1024.0).ToString("0.00") + " KB";
                        dt.Rows.Add(dr);
                    }
                    ViewState["DocumentData"] = dt;
                    gvDocuments.DataSource = dt;
                    gvDocuments.DataBind();
                    lblDocument.Visible = false;
                    ddlDocumentName.SelectedIndex = 0;
                    txtDocumentNumber.Text = null;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "scrollToBottom", "window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' });", true);
                }
                catch (Exception ex)
                {
                }
            }
        }
        protected void btnnext3_Click(object sender, EventArgs e)
        {
            if (ViewState["DocumentData"] != null)
            {
                DataTable dt = (DataTable)ViewState["DocumentData"];

                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        string docName = row["DocumentName"].ToString();
                        string docNumber = row["DocumentNumber"].ToString();
                        string status = row["Status"].ToString();
                        string size = row["Size"].ToString();
                        string DocumentPath = row["DocumentPath"].ToString();
                        string DocId = row["DocId"].ToString();
                        string MId = row["Mid"].ToString();

                        SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Type", 14);
                        cmd.Parameters.AddWithValue("@ProfileId", Session["InsertedProfileId"] != null ? Session["InsertedProfileId"].ToString().Trim() : Session["UpdateSalesPersonMid"].ToString());
                        cmd.Parameters.AddWithValue("@DocID", DocId);
                        cmd.Parameters.AddWithValue("@Mid", MId);
                        cmd.Parameters.AddWithValue("@DocumentPath", DocumentPath);
                        cmd.Parameters.AddWithValue("@documentNumber", docNumber);
                        cmd.Parameters.AddWithValue("@Remarks", docNumber);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@IPAddress", Request.UserHostAddress);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 50000)
                        {
                            DisplayMessage(this, ex.Message);
                            return;
                        }
                        throw;
                    }
                }
            }
            Response.Redirect("ViewSalesPerson.aspx", false);
        }

        protected void BindSalesPersonInfo(string querymid)
        {
            SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", 12);
            cmd.Parameters.AddWithValue("@Mid", querymid);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txtFirstName.Text = dr["Name"].ToString();
                txtMobile.Text = dr["MobileNo"].ToString();
                txtAltMobile.Text = dr["MobileNo_2"].ToString();
                txtEmail.Text = dr["EmailID"].ToString();
                txtAltEmail.Text = dr["EmailID_2"].ToString();
                txtDOB.Text = Convert.ToDateTime(dr["DateOfBirth"]).ToString("dd-MMM-yyyy");// dr["DateOfBirth"].ToString();
                ddlGender.SelectedValue = dr["Gender"].ToString();
                txtPIN.Text = dr["Pincode"].ToString();
                txtCity.Text = dr["City"].ToString();
                txtState.Text = dr["State"].ToString();
                txtCommAddress.Text = dr["Address"].ToString();
                txtAccount.Text = dr["BankAccountNumber"].ToString();
                txtConfirmAccount.Text = dr["BankAccountNumber"].ToString();
                txtIFSC.Text = dr["IFSCCode"].ToString();
                txtHolder.Text = dr["AccountHolderName"].ToString();
                txtBankName.Text = dr["BankName"].ToString();
                txtBranch.Text = dr["BankBranch"].ToString();
                txtBranchAddress.Text = dr["BankBranchAddress"].ToString();               
            }
            dr.Close();            
            con.Close();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 13);
                cmd.Parameters.AddWithValue("@Mid", Session["UpdateSalesPersonMid"].ToString().Trim());
                cmd.Parameters.AddWithValue("@Name", txtFirstName.Text.Trim());
                cmd.Parameters.AddWithValue("@MobileNo", txtMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@MobileNo_2", txtAltMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@EmailID", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@EmailID_2", txtAltEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", ddlGender.Text.Trim());
                cmd.Parameters.AddWithValue("@DateOfBirth", txtDOB.Text.Trim());
                cmd.Parameters.AddWithValue("@PinCode", txtPIN.Text.Trim());
                cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                cmd.Parameters.AddWithValue("@Address", txtCommAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@BankAccountNumber", txtAccount.Text.Trim());
                cmd.Parameters.AddWithValue("@IFSCCode", txtIFSC.Text.Trim());
                cmd.Parameters.AddWithValue("@AccountHolderName", txtHolder.Text.Trim());
                cmd.Parameters.AddWithValue("@BankName", txtBankName.Text.Trim());
                cmd.Parameters.AddWithValue("@BankBranch", txtBranch.Text.Trim());
                cmd.Parameters.AddWithValue("@BankBranchAddress", txtBranchAddress.Text.Trim());

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();


                DocumentPanel.Visible = true;
                btnnext3.Visible = true;
                BindDocumentInf();
                ddlDocumentName.Focus();
            }
            catch (Exception ex) 
            {
            }
        }

        protected void gvDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditDoc")
            {
                int docId = Convert.ToInt32(e.CommandArgument);
                LoadDocumentForEdit(docId);
            }
            else if (e.CommandName == "ViewDoc")
            {
                string[] args = e.CommandArgument.ToString().Split('|');
                string docId = args[0];
                string documentName = args[1];
                string documentNumber = args[2];
                string documentPath = args[3];

                ddlDocumentName.ClearSelection();
                ListItem item = ddlDocumentName.Items.FindByText(documentName);
                if (item != null) item.Selected = true;
                txtDocumentNumber.Text = documentNumber;

                string fileExt = Path.GetExtension(documentPath).ToLower();
                string relativePath = "~/UploadedDocuments/" + documentPath;

                if (fileExt == ".jpg" || fileExt == ".jpeg" || fileExt == ".png")
                {
                    imgPreview.ImageUrl = relativePath;
                    imgPreview.Visible = true;
                    litPdfPreview.Visible = false;
                }
                else if (fileExt == ".pdf")
                {
                    litPdfPreview.Text = $"<iframe src='{ResolveUrl(relativePath)}' width='100%' height='400px'></iframe>";
                    litPdfPreview.Visible = true;
                    imgPreview.Visible = false;
                }
            }
            else if (e.CommandName == "DeleteRow")
            {
                string docId = e.CommandArgument.ToString();
                DeleteDocument(docId);
            }
        }

        protected void BindDocumentInf()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 15);
                cmd.Parameters.AddWithValue("@ProfileId", Session["UpdateSalesPersonMid"]);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Add a SrNo column dynamically
                dt.Columns.Add("SrNo", typeof(int));
                dt.Columns.Add("Size", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["SrNo"] = i + 1;
                    dt.Rows[i]["Size"] = "";
                }
                ViewState["DocumentData"] = dt;
                gvDocuments.DataSource = dt;
                gvDocuments.DataBind();
            }
            catch (Exception ex)
            {
            }
        }
        private void LoadDocumentForEdit(int docId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", 16);
                cmd.Parameters.AddWithValue("@Mid", docId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    ddlDocumentName.SelectedValue = dt.Rows[0]["DocID"].ToString();
                    txtDocumentNumber.Text = dt.Rows[0]["DocumentNumber"].ToString();

                    ViewState["EditDocID"] = docId;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void DeleteDocument(string mid)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_IAPL_Retailer_Auth", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Type", 17);
                cmd.Parameters.AddWithValue("@Mid", mid);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                BindDocumentInf();
            }
            catch { }
        }

    }
}