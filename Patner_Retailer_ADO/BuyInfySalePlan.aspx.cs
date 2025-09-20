using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Runtime.Remoting.Messaging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Reflection;

namespace Patner_Retailer_ADO
{
    public partial class BuyInfySalePlan : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iaplConnectionString"].ConnectionString);
        static public void DisplayMessage(Control page, string msg)
        {
            string msg1 = String.Format("alert('{0}');", msg);
            ScriptManager.RegisterStartupScript(page, page.GetType(), "msg", msg1, true);
        }
        private const int MaxOTPAttempts = 3;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MobileNo"] == null)
            {
                Response.Redirect("index.aspx");
                return;
            }
            if (!IsPostBack)
            {
                CalendarExtender3.EndDate = DateTime.Today;
                CalendarExtender3.StartDate = DateTime.Today.AddMonths(-11);
                CalendarExtender1.EndDate = DateTime.Today;
                CalendarExtender1.StartDate = DateTime.Today.AddMonths(-11);

                BindSubCategory();
                bindsubcatg();
                getallBrand();
                Session.Remove("cartItemId");
                Session.Remove("UniqueID");
                
                PlanPanel.Visible = false;
                ApplyPromoCodePanel.Visible = false;
                txtPurchaseDate.Attributes.Add("ReadOnly", "readonly");
                txtDateOfImpl.Attributes.Add("ReadOnly", "readonly");
                btnEditPlan.Visible = false;
               
                if (Session["salesOrderID"] != null)
                {
                    var req = Request.QueryString["AddProduct"];
                    if (req == "Add")
                    {

                        txtCustomerName.Text = Session["CustomerName"].ToString();
                        txtCustomerEmail.Text = Session["CustomerEmailId"].ToString();
                        txtCustomerMobile.Text = Session["CustomerMobileNo"].ToString();

                        txtCustomerName.Enabled = false;
                        txtCustomerEmail.Enabled = false;
                        txtCustomerMobile.Enabled = false;
                    }
                    btnClearCart.Visible = true;
                }
                if (Session["ProductType"] != null)
                {
                    ProductDiv.Visible = false;
                    ddlProductType.SelectedItem.Value = Session["ProductType"].ToString();
                    lblProductName.Text = Session["ProductTypeName"].ToString();
                    ddlProductType_OnSelectedIndexChanged(sender, e);
                }
                bindSerielNo();
                BindProductSubType();
                ddlProductSubType.Focus();
             
                string checkedPlan = Request.QueryString["qu"];
                if (!string.IsNullOrWhiteSpace(checkedPlan))
                {
                    var status = Request.QueryString["st"];
                    if(status == "Checked")
                    {
                        BindIncompleteInfo();
                    }
                    else if (status == "NotChecked")
                    {
                        BindIncompleteInfo();
                    }
                }
                BindSlider();
            }
        }



        private void BindSlider()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 70);
                    cmd.Parameters.AddWithValue("@ProductType", lblProductName.Text);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        rptslider.DataSource = dt;
                        rptslider.DataBind();
                    }
                    else
                    {
                        rptslider.DataSource = null;
                        rptslider.DataBind();
                        // Optionally show a "no records" message or hide the control
                        // lblNoRecords.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                // LogError(ex); 
                // Or show error on screen (avoid in production)
                Response.Write("Error: " + ex.Message);
            }
        }





        protected void BindIncompleteInfo()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 67);
                    cmd.Parameters.AddWithValue("@Mid", Session["IncompleteMid"] != null ? Session["IncompleteMid"].ToString() : "0");

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ddlProductType.SelectedValue = reader["ProductTypeID"]?.ToString();
                            ddlProductSubType.SelectedValue = reader["SubCategoryID"]?.ToString();
                            ddlBrand.SelectedValue = reader["Brand"]?.ToString();
                            txtModel.Text = reader["ModalName"]?.ToString();
                            validationCustom06.Text = reader["ModalName"]?.ToString();
                            txtPrice.Text = reader["DevicePurchasePrice"]?.ToString();
                            if (reader["ProductPurchaseDate"] != DBNull.Value)
                            {
                                DateTime purchaseDate = Convert.ToDateTime(reader["ProductPurchaseDate"]);
                                txtPurchaseDate.Text = purchaseDate.ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                txtPurchaseDate.Text = string.Empty;
                            }

                            txtSerialNo.Text = reader["serialno"]?.ToString();
                            txtimeiNo.Text = reader["imei"]?.ToString();
                            txtimeiNo2.Text = "";
                            txtDateOfImpl.Text = reader["DateofImplementation"]?.ToString();
                            string warrantyType = reader["ManufacturerWarranty_yymmdd"]?.ToString();
                            if(warrantyType == "00/03/00")
                            {
                                rb3M.Checked = true;
                                lbl3M.CssClass += " selectWarranty";
                            }
                            else if (warrantyType == "00/06/00")
                            {
                                rb6M.Checked = true;
                                lbl6M.CssClass += " selectWarranty";
                            }
                            else if (warrantyType == "01/00/00")
                            {
                                rb1Y.Checked = true;
                                lbl1Y.CssClass += " selectWarranty";
                            }
                            else if (warrantyType == "02/00/00")
                            {
                                rb2Y.Checked = true;
                                lbl2Y.CssClass += " selectWarranty";
                            }
                            else if (warrantyType == "03/00/00")
                            {
                                rb3Y.Checked = true;
                                lbl3Y.CssClass += " selectWarranty";
                            }
                            else
                            {
                                rbCustom.Checked = true;
                                lblCustom.CssClass += " selectCustomWarranty";
                                customWarrantyDiv.Visible = true;
                                string[] parts = warrantyType.Split('/');

                                if (ddlCustomYears.Items.Count <= 1)
                                {
                                    for (int i = 1; i <= 10; i++)
                                    {
                                        string value = i.ToString("D2");
                                        ddlCustomYears.Items.Add(new ListItem(value, value));
                                    }
                                }
                                else
                                {
                                    ddlCustomYears.SelectedIndex = 0;
                                }

                                if (ddlCustomMonths.Items.Count <= 1)
                                {
                                    for (int i = 0; i <= 12; i++)
                                    {
                                        string value = i.ToString("D2");
                                        ddlCustomMonths.Items.Add(new ListItem(value, value));
                                    }
                                }
                                else
                                    ddlCustomMonths.SelectedIndex = 0;

                                if (ddlCustomDays.Items.Count <= 1)
                                {
                                    for (int i = 0; i <= 31; i++)
                                    {
                                        string value = i.ToString("D2");
                                        ddlCustomDays.Items.Add(new ListItem(value, value));
                                    }
                                }
                                else
                                    ddlCustomDays.SelectedIndex = 0;
                                if (parts.Length == 3)
                                {
                                    ddlCustomYears.SelectedValue = parts[0];
                                    ddlCustomMonths.SelectedValue = parts[1];
                                    ddlCustomDays.SelectedValue = parts[2];
                                }
                            }

                            txtCustomerName.Text = reader["CustomerName"]?.ToString();
                            txtCustomerEmail.Text = reader["CustomerEmailID"]?.ToString();
                            txtCustomerMobile.Text = reader["CustomerMobileNo"]?.ToString();
                            ViewState["PlanId"] = reader["PlanID"]?.ToString();
                            string price = reader["PlanPrice"]?.ToString();
                            con.Close();
                            btnSubmitPlan.Visible = false;
                            btnEditPlan.Visible = true;
                            rptPlans.Visible = true;
                            PlanPanel.Visible = true;
                            
                            ddlProductType.Enabled = false;
                            ddlProductSubType.Enabled = false;
                            txtPrice.Enabled = false;
                            txtPurchaseDate.Enabled = false;
                            ddlBrand.Enabled = false;
                            txtModel.Enabled = false;
                            validationCustom06.Enabled = false;
                            txtSerialNo.Enabled = false;
                            txtimeiNo.Enabled = false;
                            txtimeiNo2.Enabled = false;
                            txtDateOfImpl.Enabled = false;
                            rb3M.Enabled = false;
                            rb6M.Enabled = false;
                            rb1Y.Enabled = false;
                            rb2Y.Enabled = false;
                            rb3Y.Enabled = false;
                            ddlCustomYears.Enabled = false;
                            ddlCustomMonths.Enabled = false;
                            ddlCustomDays.Enabled = false;
                            txtCustomerName.Enabled = false;
                            txtCustomerEmail.Enabled = false;
                            txtCustomerMobile.Enabled = false;
                            ApplyPromoCodePanel.Visible = true;
                            txtPromoDiscount.Value = "";
                            lblDiscountAmount.Text = "0";
                            lblTotalAmount.Text = "";
                            calculationdiv.Visible = false;
                            txtPromoDiscount.Focus();
                            lblErrorPromoCode.Visible = false;

                            ApplyPromoCode(this, EventArgs.Empty);
                            lblErrorPromoCode.Visible = false;
                            PromoCodeDiv.Visible = true;
                            lblPlanPrice.Text = "Rs. " + price;
                            lblTotalAmount.Text = "Rs. " + price;
                            BindPlanData();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, ex.Message);
            }
        }
        protected void rptPlans_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string planIdFromRow = DataBinder.Eval(e.Item.DataItem, "Mid")?.ToString();

                string selectedPlanId = ViewState["PlanId"]?.ToString();

                CheckBox chkSelect = (CheckBox)e.Item.FindControl("chkSelect");

                if (chkSelect != null && planIdFromRow == selectedPlanId)
                {
                    chkSelect.Checked = true;
                }
            }
        }


        protected void BindSubCategory()
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 1);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    ddlsubcatg.DataSource = dt;
                    ddlsubcatg.DataValueField = "SubCategoryID";
                    ddlsubcatg.DataTextField = "SubCategory";
                    ddlsubcatg.DataBind();

                    ddlsubcatg.Items.Insert(0, new ListItem("Select Product", "0"));
                    ListItem item = ddlsubcatg.Items.FindByValue("2");
                    if (item != null)
                    {
                        ddlsubcatg.SelectedValue = "2";
                    }
                }
            }
            con.Close();
        }
        protected void ddlsubcatg_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            bindsubcatg();
            bindSerielNo();
            //HideOtherPanels();
            ddlProductType.Focus();
        }

        public void bindsubcatg()
        {
            try
            {
                SqlDataAdapter adp = new SqlDataAdapter("sp_iapl_PartnerRetailer", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@type", SqlDbType.Int).Value = 2;
                adp.SelectCommand.Parameters.Add("@ProfileId", Session["RetailerUniqueID"].ToString());
                //adp.SelectCommand.Parameters.Add("@description", SqlDbType.NVarChar).Value = ddlsubcatg.SelectedItem.Text;
                con.Open();
                DataSet ds = new DataSet();
                adp.Fill(ds);
                ddlProductType.DataSource = ds.Tables[0];
                ddlProductType.DataTextField = "ProductType";
                ddlProductType.DataValueField = "ProductTypeID";
                ddlProductType.DataBind();
                ddlProductType.Items.Insert(0, new ListItem("Select Product Type", "0"));

                con.Close();
            }
            catch (SqlException se)
            {
                DisplayMessage(this, "" + se.Message + "");
                return;
            }
        }

        protected void ddlProductType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 1);
                cmd.Parameters.AddWithValue("@ProductType", ddlProductType.SelectedValue);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    ddlsubcatg.SelectedValue = dt.Rows[0]["SubCategoryID"].ToString();
                    bindSerielNo();
                }
            }
            con.Close();

            txtMake.Text = "";
            getallBrand();
            //HideOtherPanels();
            txtPrice.Focus();
        }

        protected void getallBrand()
        {
            SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 3;
            cmd.Parameters.AddWithValue("@subcatgId", ddlProductType.SelectedValue);
            cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString());
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            ddlBrand.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtMake.Visible = false;
                ddlBrand.Visible = true;
                string s = Convert.ToString(ds.Tables[0].Rows.Count + 1);
                ddlBrand.DataTextField = "Brand";
                ddlBrand.DataValueField = "Brand";
                ddlBrand.DataSource = ds.Tables[0];
                ddlBrand.DataBind();
                ddlBrand.Items.Insert(0, new ListItem("Select Make", string.Empty));
                ddlBrand.Items.Add(new ListItem("My Brand is not in the List", s));
            }
            else
            {
                ddlBrand.Items.Insert(0, new ListItem("Select Make", "0"));
            }
            con.Close();
        }
        protected void BindProductSubType()
        {
            SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 54;
            cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString());
            cmd.Parameters.AddWithValue("@ProductType", ddlProductType.SelectedValue);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            ddlProductSubType.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                string s = Convert.ToString(ds.Tables[0].Rows.Count + 1);
                ddlProductSubType.DataTextField = "SubProductType";
                ddlProductSubType.DataValueField = "SubProductTypeID";
                ddlProductSubType.DataSource = ds.Tables[0];
                ddlProductSubType.DataBind();
                ddlProductSubType.Items.Insert(0, new ListItem("Select Product Sub Type", string.Empty));
                if (ds.Tables[0].Rows.Count == 1)
                {
                    ddlProductSubType.SelectedIndex = 1;
                }
            }
            else
            {
                ddlProductSubType.Items.Insert(0, new ListItem("Select Product Sub Type", "0"));
            }
            con.Close();
        }
        private void BindPlanData()
        {
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 4);
                cmd.Parameters.AddWithValue("@ProductType", ddlProductType.SelectedValue);
                cmd.Parameters.AddWithValue("@subcatgId", ddlsubcatg.SelectedValue);
                cmd.Parameters.AddWithValue("@ProductPrice", txtPrice.Text.ToString());
                cmd.Parameters.AddWithValue("@SubProductType", ddlProductSubType.SelectedValue);
                cmd.Parameters.AddWithValue("@ProductPurchaseDate", !string.IsNullOrWhiteSpace(txtPurchaseDate.Text) ? Convert.ToDateTime(txtPurchaseDate.Text.ToString()) : DateTime.Now);
                cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString());

                if (con.State != ConnectionState.Open)
                    con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    pnlNoPlans.Visible = false;
                    rptPlans.DataSource = dt;
                    rptPlans.DataBind();
                }
                else
                {
                    pnlNoPlans.Visible = true;
                    rptPlans.DataSource = null;
                    rptPlans.DataBind();
                }
            }
        }
        protected void AddToCart(object sender, EventArgs e)
        {
            if (invalidCheck.Checked == false)
            {
                lblErrorTermCondition.Visible = true;
                return;
            }
            string planName = string.Empty;
            string planPrice = string.Empty;
            string SKU = string.Empty;
            foreach (RepeaterItem item in rptPlans.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                if (chkSelect.Checked)
                {
                    planName = ((Label)item.FindControl("lblPlanFullName"))?.Text ?? string.Empty;
                    planPrice = ((HiddenField)item.FindControl("hdnPlanPrice")).Value;
                    SKU = ((HiddenField)item.FindControl("hdnSKU")).Value;
                    break;
                }
            }

            string selectedWarranty = "";
            if (rb3M.Checked)
                selectedWarranty = "00/03/00";
            else if (rb6M.Checked)
                selectedWarranty = "00/06/00";
            else if (rb1Y.Checked)
                selectedWarranty = "01/00/00";
            else if (rb2Y.Checked)
                selectedWarranty = "02/00/00";
            else if (rb3Y.Checked)
                selectedWarranty = "03/00/00";
            else if (rbCustom.Checked)
            {
                string year = string.IsNullOrEmpty(ddlCustomYears.SelectedValue) ? "00" : ddlCustomYears.SelectedValue;
                string month = string.IsNullOrEmpty(ddlCustomMonths.SelectedValue) ? "00" : ddlCustomMonths.SelectedValue;
                string day = string.IsNullOrEmpty(ddlCustomDays.SelectedValue) ? "00" : ddlCustomDays.SelectedValue;

                selectedWarranty = $"{year}/{month}/{day}";
            }

            string categoryId = "";
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 10);
                cmd.Parameters.AddWithValue("@SKU", SKU);

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        categoryId = reader["categoryid"].ToString();
                    }
                }
            }
            var req = Request.QueryString["AddProduct"];
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@type", 5);
                cmd.Parameters.AddWithValue("@Saleschannel", "Retailer");
                cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"].ToString());
                cmd.Parameters.AddWithValue("@ProductSubCat", ddlsubcatg.SelectedValue);
                cmd.Parameters.AddWithValue("@Productname", lblProductName.Text);
                cmd.Parameters.AddWithValue("@ProductType", ddlProductType.SelectedValue);
                cmd.Parameters.AddWithValue("@Productsubcategoryname", ddlsubcatg.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@Brand", ddlBrand.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@SubProductType", ddlProductSubType.SelectedValue);
                cmd.Parameters.AddWithValue("@ModalName", txtModel.Text.ToString());
                cmd.Parameters.AddWithValue("@imei", !string.IsNullOrWhiteSpace(txtimeiNo.Text) ? txtimeiNo.Text.ToString() : null);
                cmd.Parameters.AddWithValue("@imei2", !string.IsNullOrWhiteSpace(txtimeiNo2.Text) ? txtimeiNo2.Text.ToString() : null);
                cmd.Parameters.AddWithValue("@serialno", !string.IsNullOrWhiteSpace(txtSerialNo.Text) ? txtSerialNo.Text.ToString() : null);
                cmd.Parameters.AddWithValue("@DevicePurchasePrice", !string.IsNullOrWhiteSpace(txtPrice.Text) ? Convert.ToDecimal(txtPrice.Text.ToString()) : 0);
                cmd.Parameters.AddWithValue("@ProductPurchaseDate", !string.IsNullOrWhiteSpace(txtPurchaseDate.Text) ? Convert.ToDateTime(txtPurchaseDate.Text.ToString()) : DateTime.Now);
                cmd.Parameters.AddWithValue("@CustomerEmailID", txtCustomerEmail.Text.ToLower().ToString());
                cmd.Parameters.AddWithValue("@CustomerMobileNo", txtCustomerMobile.Text.ToString());
                cmd.Parameters.AddWithValue("@PlanName", planName);
                cmd.Parameters.AddWithValue("@PlanPrice", !string.IsNullOrWhiteSpace(planPrice) ? Convert.ToDecimal(planPrice) : 0);
                cmd.Parameters.AddWithValue("@Promocode", txtPromoDiscount.Value.ToString());
                cmd.Parameters.AddWithValue("@PromoDiscount", !string.IsNullOrWhiteSpace(lblDiscountAmount.Text) ? Convert.ToDecimal(lblDiscountAmount.Text.Replace("Rs. ", "").Trim().ToString()) : 0);
                cmd.Parameters.AddWithValue("@PlanTotalValue", !string.IsNullOrWhiteSpace(lblTotalAmount.Text) ? Convert.ToDecimal(lblTotalAmount.Text.Replace("Rs. ", "").Trim().ToString()) : 0);
                cmd.Parameters.AddWithValue("@DateofImplementation", !string.IsNullOrWhiteSpace(txtDateOfImpl.Text) ? Convert.ToDateTime(txtDateOfImpl.Text.ToString()) : DateTime.Now);
                cmd.Parameters.AddWithValue("@ManufacturerWarranty_yymmdd", selectedWarranty);
                cmd.Parameters.AddWithValue("@categoryid", categoryId);
                cmd.Parameters.AddWithValue("@PlanSKU", SKU);
                cmd.Parameters.AddWithValue("@SalesOrderID", (Session["salesOrderID"] != null && req == "Add") ? Session["salesOrderID"].ToString() : null);
                cmd.Parameters.AddWithValue("@Customer_OrderID", (Session["Customerorder"] != null && req == "Add") ? Session["Customerorder"].ToString() : null);

                if (con.State != ConnectionState.Open)
                    con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string salesOrderID = reader["salesorderid"].ToString();
                        string Customerorder = reader["Customerorder"].ToString();
                        Session["salesOrderID"] = salesOrderID;
                        Session["Customerorder"] = Customerorder;
                        Session["CustomerName"] = txtCustomerName.Text;
                        Session["CustomerEmailId"] = txtCustomerEmail.Text.ToLower().ToString();
                        Session["CustomerMobileNo"] = txtCustomerMobile.Text.ToString();
                        reader.Close();

                        AddIncomepletePurchase("Add to cart, not purchase");
                        Response.Redirect("CartDetails.aspx?salesorder=" + salesOrderID);
                    }
                }
            }
            con.Close();
            Response.Redirect("CartDetails.aspx");
        }

        protected void SubmitPlanInfo(object sender, EventArgs e)
        {

            int count = 0;
            //if (ddlsubcatg.SelectedValue == "0")
            //{
            //    ddlsubcatg.BorderColor = System.Drawing.Color.Red;
            //    count = count + 1;
            //}
            //else
            //    ddlsubcatg.BorderColor = System.Drawing.Color.LightGray;

            if (ddlProductType.SelectedValue == "0")
            {
                ddlProductType.BorderColor = System.Drawing.Color.Red;
                count = count + 1;
            }
            else
                ddlProductType.BorderColor = System.Drawing.Color.LightGray;
            if (ddlProductSubType.SelectedValue == "0")
            {
                ddlProductSubType.BorderColor = System.Drawing.Color.Red;
                lblProductSubType.Visible = true;
                count = count + 1;
            }
            else
            {
                ddlProductType.BorderColor = System.Drawing.Color.LightGray;
                lblProductSubType.Visible = false;
            }
            if (string.IsNullOrWhiteSpace(ddlBrand.SelectedValue) || ddlBrand.SelectedValue == "0")
            {
                ddlBrand.BorderColor = System.Drawing.Color.Red;
                lblBrand.Visible = true;
                count = count + 1;
            }
            else
            {
                ddlBrand.BorderColor = System.Drawing.Color.LightGray;
                lblBrand.Visible = false;
            }

            if (rb3M.Checked || rb6M.Checked || rb1Y.Checked || rb2Y.Checked || rb3Y.Checked)
                lblWarrantyError.Text = "";
            else if (rbCustom.Checked)
            {
                if (ddlCustomYears.SelectedValue == "")
                {
                    lblCustomYearsError.Text = "Custom Year is required";
                    lblWarrantyError.Text = "";
                    count = count + 1;
                }
            }
            else
            {
                lblWarrantyError.Text = "Manufacture Warranty is required";
                count = count + 1;
            }
            if(!string.IsNullOrWhiteSpace(txtSerialNo.Text))
            {
                SerialNoChange(sender, e);
                if (!string.IsNullOrWhiteSpace(serialNoError.InnerText))
                {
                    count++;
                }
            }
            if (txtimeiNo.Visible == true && !System.Text.RegularExpressions.Regex.IsMatch(txtimeiNo.Text.Trim(), @"^\d{15}$"))
            {
                txtimeiNo.BorderColor = System.Drawing.Color.Red;
                imeiError.InnerText = "IMEI must be exactly 15 digits.";
                count++;
            }
            else if (txtimeiNo.Visible == true && !IsValidIMEI(txtimeiNo.Text.Trim()))
            {

                txtimeiNo.BorderColor = System.Drawing.Color.Red;
                imeiError.InnerText = "Invalid IMEI number.";
                count++;
            }
            else
            {
                txtimeiNo.BorderColor = System.Drawing.Color.LightGray;
                imeiError.InnerText = "";
                if(txtimeiNo.Visible == true)
                {
                    IMEINoChange(sender, new EventArgs());
                    if(!string.IsNullOrWhiteSpace(imeiError.InnerText))
                    {
                        count++;
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(txtimeiNo2.Text) && txtimeiNo2.Visible == true && !System.Text.RegularExpressions.Regex.IsMatch(txtimeiNo2.Text.Trim(), @"^\d{15}$"))
            {
                txtimeiNo2.BorderColor = System.Drawing.Color.Red;
                imeiError.InnerText = "IMEI must be exactly 15 digits.";
                count++;
            }
            else if (!string.IsNullOrWhiteSpace(txtimeiNo2.Text) && txtimeiNo2.Visible == true && !IsValidIMEI(txtimeiNo2.Text.Trim()))
            {
                txtimeiNo2.BorderColor = System.Drawing.Color.Red;
                imeiError.InnerText = "Invalid IMEI number.";
                count++;
            }
            else if (!string.IsNullOrWhiteSpace(txtimeiNo.Text) && !string.IsNullOrWhiteSpace(txtimeiNo.Text) && txtimeiNo.Text == txtimeiNo2.Text)
            {
                Small1.InnerText = "IMEI No. 1 and IMEI No. 2 cannot be same";
                count++;
            }
            else
            {
                IMEINo2Change(sender, new EventArgs());
                if (!string.IsNullOrWhiteSpace(Small1.InnerText))
                {
                    count++;
                }
                txtimeiNo2.BorderColor = System.Drawing.Color.LightGray;
                Small1.InnerText = "";
            }

            if (count > 0)
            {
                return;
            }
            bool checkcusotmer = CheckBlockCustomer();
            bool checkSalesPerson = CheckSalesPersonOrRetailer();
            if (checkcusotmer || checkSalesPerson)
            {
                return;
            }
            if (Session["salesOrderID"] != null)
            {
                var req = Request.QueryString["AddProduct"];
                if (req == "Add")
                {

                    PlanPanel.Visible = true;
                    btnSubmitOTP.Visible = false;
                    lblOTPSend.Visible = false;
                    lnkResendOTP.Visible = false;
                    OTPPanel.Visible = false;
                }
                else
                {
                    string newopt = newotp();
                    Session.Remove("OTP");
                    Session["OTP"] = newopt;
                    sendSMSOTP(txtCustomerMobile.Text.Trim(), Session["otp"].ToString());
                    sendotpmail(txtCustomerEmail.Text.Trim(), Session["otp"].ToString());
                    btnSubmitPlan.Visible = false;
                    OTPPanel.Visible = true;
                    btnSubmitOTP.Visible = true;
                    txtOTP.Value = null;
                    Session["OTPAttemptSessionKey"] = 0;
                    btnEditPlan.Visible = true;
                }
                BindPlanData();

                if (rptPlans.Items.Count > 0)
                {
                    CheckBox chkFirst = (CheckBox)rptPlans.Items[0].FindControl("chkSelect");
                    if (chkFirst != null)
                    {
                        chkFirst.Focus();
                    }
                }
                ddlProductType.Enabled = false;
                ddlProductSubType.Enabled = false;
                txtPrice.Enabled = false;
                txtPurchaseDate.Enabled = false;
                ddlBrand.Enabled = false;
                txtModel.Enabled = false;
                validationCustom06.Enabled = false;
                txtSerialNo.Enabled = false;
                txtimeiNo.Enabled = false;
                txtimeiNo2.Enabled = false;
                txtDateOfImpl.Enabled = false;
                rb3M.Enabled = false;
                rb6M.Enabled = false;
                rb1Y.Enabled = false;
                rb2Y.Enabled = false;
                rb3Y.Enabled = false;
                ddlCustomYears.Enabled = false;
                ddlCustomMonths.Enabled = false;
                ddlCustomDays.Enabled = false;
                txtCustomerName.Enabled = false;
                txtCustomerEmail.Enabled = false;
                txtCustomerMobile.Enabled = false;
            }
            else if (txtCustomerMobile.Text != "")
            {
                string newopt = newotp();
                //string newopt = "999999";
                Session.Remove("OTP");
                Session["OTP"] = newopt;
                sendSMSOTP(txtCustomerMobile.Text.Trim(), Session["otp"].ToString());
                sendotpmail(txtCustomerEmail.Text.Trim(), Session["otp"].ToString());
                btnSubmitPlan.Visible = false;
                OTPPanel.Visible = true;
                btnSubmitOTP.Visible = true;
                txtOTP.Value = null;
                Session["OTPAttemptSessionKey"] = 0;
                btnEditPlan.Visible = true;

                //ddlsubcatg.Enabled = false;
                ddlProductType.Enabled = false;
                ddlProductSubType.Enabled = false;
                txtPrice.Enabled = false;
                txtPurchaseDate.Enabled = false;
                ddlBrand.Enabled = false;
                txtModel.Enabled = false;
                validationCustom06.Enabled = false;
                txtSerialNo.Enabled = false;
                txtimeiNo.Enabled = false;
                txtimeiNo2.Enabled = false;
                txtDateOfImpl.Enabled = false;
                //validationCustom09.Enabled = false;
                //validationCustom010.Enabled = false;
                //validationCustom011.Enabled = false;
                rb3M.Enabled = false;
                rb6M.Enabled = false;
                rb1Y.Enabled = false;
                rb2Y.Enabled = false;
                rb3Y.Enabled = false;
                ddlCustomYears.Enabled = false;
                ddlCustomMonths.Enabled = false;
                ddlCustomDays.Enabled = false;
                txtCustomerName.Enabled = false;
                txtCustomerEmail.Enabled = false;
                txtCustomerMobile.Enabled = false;

                txtOTP.Focus();
                lblOTPSend.Visible = true;
                lblOTPSend.Text = $"OTP sent to {txtCustomerMobile.Text} and {txtCustomerEmail.Text.ToLower()} ";
            }
            //AddIncomepletePurchase("OTP Send Not Verified");
        }

        protected void lnkResendOTP_Click(object sender, EventArgs e)
        {
            int attempts = (int)Session["OTPAttemptSessionKey"];
            if (attempts < MaxOTPAttempts)
            {
                if (!string.IsNullOrEmpty(txtCustomerMobile.Text))
                {
                    string generatedOTP = newotp();
                    Session["OTP"] = generatedOTP;
                    lblOTPSend.Text = $"OTP sent to {txtCustomerMobile.Text} ";
                    sendSMSOTP(txtCustomerMobile.Text, generatedOTP);
                    Session["OTPAttemptSessionKey"] = attempts + 1;
                    txtOTP.Focus();
                }
                else
                {
                    lblOTPSend.Text = "Please enter a valid phone number.";
                }
            }
            else
            {
                lblOTPSend.Text = "Too many OTP resend attempts. Please try again later.";
                lnkResendOTP.Enabled = false;
            }
        }

        protected string newotp()
        {
            string otp1 = "";
            string numbers = "1234567890";
            string characters = numbers;
            int length = 6;
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            otp1 = otp;
            return otp1;
        }

        protected void sendSMSOTP(string mobileno, string otp)
        {
            try
            {
                string responseString = "";
                string message = "Welcome to Infinity, Your OTP to Login to Infinity TechCare Lounge is " + otp + ". For Help, Call Infinity 8447882424. 9AM-6PM Mon-Sat";
                string content_temID = "1107162426891569578";
                string no = mobileno;
                string sender12 = "ISHILD";
                
                string url1 = "https://api.mobilnxt.in/api/push?accesskey=uW9h2HHRlctDRlGwOQKEicLgsgBi2V&to=" + no + "&text=" + message + "&from=" + sender12 + "&tid=" + content_temID;

                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url1);
                HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
                StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                responseString = respStreamReader.ReadToEnd();
                string s1 = responseString.Substring(0, 17);
                respStreamReader.Close();
                myResp.Close();
            }
            catch (Exception e1)
            {
                DisplayMessage(this, e1.Message);
                return;
            }
        }
        public void sendotpmail(string email, string otp)
        {
            try
            {
                Session["email"] = email;
                string s = Server.MapPath("infySign.png");
                string MSG1 = "<table align='left' cellpadding='5' cellspacing='1' style='width:100%; '><tr><td style='font-size: 14px;text-align: justify; font-family: Arial;' colspan='2'>Dear Customer," + "\t\t" +
                "</td></tr>" +
                  "<tr><td style='font-size: 14px;text-align: justify; font-family: Arial;' colspan='2'>" + "\t\t<b><u>" + otp + "</b></u> is your OTP. " + "</td></tr>" +
                "<tr><td style='font-size: 14px;text-align: justify; font-family: Arial;' colspan='2'>Thanking you,<br/><br/>Team Infinity<br/>InfyShield<br/><br/><img src=cid:companylogo></td></tr></table>";

                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress("no-reply@infinityassurance.com");
                Msg.To.Add(email.ToString());
                Msg.Subject = "Welcome to InfyShield - Your OTP is here";
                Msg.IsBodyHtml = true;
                Msg.Body = MSG1;

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

        protected void SubmitOTP(object sender, EventArgs e)
        {
            lblOTPSend.Visible = false;
            if (txtOTP.Value.Trim() != null)
            {
                if (txtOTP.Value.Trim() == Session["OTP"].ToString())
                {
                    PlanPanel.Visible = true;
                    btnSubmitOTP.Visible = false;
                    lnkResendOTP.Visible = false;
                    OTPPanel.Visible = false;
                    BindPlanData();
                    //AddIncomepletePurchase("Plan checked but didn't buy");
                    if (rptPlans.Items.Count > 0)
                    {
                        CheckBox chkFirst = (CheckBox)rptPlans.Items[0].FindControl("chkSelect");
                        if (chkFirst != null)
                        {
                            chkFirst.Focus();
                        }
                    }
                }
            }
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            ApplyPromoCodePanel.Visible = true;
            txtPromoDiscount.Value = "";
            lblDiscountAmount.Text = "0";
            lblTotalAmount.Text = "";
            calculationdiv.Visible = false;
            txtPromoDiscount.Focus();
            lblErrorPromoCode.Visible = false;

            ApplyPromoCode(sender, e);
            lblErrorPromoCode.Visible = false;
            PromoCodeDiv.Visible = true;
            //BindAddOnsData();
            //AddOnsDiv.Visible = true;
            AddIncomepletePurchase("Plan checked but didn't buy");
        }

        protected void bindSerielNo()
        {
            if (ddlsubcatg.SelectedValue == "46" || ddlsubcatg.SelectedValue == "2")
            {
                txtimeiNo.Enabled = true;
                txtimeiNo2.Enabled = true;
                //txtSerialNo.Enabled = false;
                txtDateOfImpl.Enabled = false;
                txtDateOfImpl.Text = null;
                //txtSerialNo.Text = null;
                serielNoDiv.Visible = false;
                dateOfImplementationDic.Visible = false;
                imeiNoDiv.Visible = true;
                imeiNo2Div.Visible = true;
                rfvIMEINo.Enabled = true;
                //rfvSerialNo.Enabled = false;
                //rfvDateOfImpl.Enabled = false;
            }
            else
            {
                txtimeiNo.Enabled = false;
                txtimeiNo2.Enabled = false;
                txtSerialNo.Enabled = true;
                txtDateOfImpl.Enabled = true;
                txtimeiNo.Text = null;
                txtimeiNo2.Text = null;
                serielNoDiv.Visible = true;
                dateOfImplementationDic.Visible = true;
                imeiNoDiv.Visible = false;
                imeiNo2Div.Visible = false;
                rfvIMEINo.Enabled = false;
                //rfvSerialNo.Enabled = true;
                //rfvDateOfImpl.Enabled = true;
            }
        }

        protected void EditPlanInfo(object sender, EventArgs e)
        {
            btnEditPlan.Visible = false;
            btnSubmitPlan.Visible = true;
            //ddlsubcatg.Enabled = true;
            ddlProductType.Enabled = true;
            txtPrice.Enabled = true;
            txtPurchaseDate.Enabled = true;
            ddlBrand.Enabled = true;
            ddlProductSubType.Enabled = true;
            txtModel.Enabled = true;
            validationCustom06.Enabled = true;
            //validationCustom09.Enabled = true;
            //validationCustom010.Enabled = true;
            //validationCustom011.Enabled = true;
            rb3M.Enabled = true;
            rb6M.Enabled = true;
            rb1Y.Enabled = true;
            rb2Y.Enabled = true;
            rb3Y.Enabled = true;
            ddlCustomYears.Enabled = true;
            ddlCustomMonths.Enabled = true;
            ddlCustomDays.Enabled = true;
            txtCustomerName.Enabled = true;
            txtCustomerEmail.Enabled = true;
            txtCustomerMobile.Enabled = true;
            lblOTPSend.Visible = false;
            HideOtherPanels();
            bindSerielNo();
        }

        protected void HideOtherPanels()
        {
            PlanPanel.Visible = false;
            OTPPanel.Visible = false;
            ApplyPromoCodePanel.Visible = false;
            btnSubmitPlan.Visible = true;
        }

        protected void ApplyPromoCode(object sender, EventArgs e)
        {
            try
            {
                decimal planPrice = 0;
                int planId = 0;
                foreach (RepeaterItem item in rptPlans.Items)
                {
                    CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                    if (chkSelect != null && chkSelect.Checked)
                    {
                        HiddenField hdnPlanPrice = (HiddenField)item.FindControl("hdnPlanPrice");
                        HiddenField hdnPlanId = (HiddenField)item.FindControl("hdnPlanId");
                        if (hdnPlanPrice != null)
                        {
                            planPrice = Convert.ToDecimal(hdnPlanPrice.Value);

                        }
                        if (hdnPlanId != null)
                        {
                            planId = Convert.ToInt32(hdnPlanId.Value);
                        }
                        break;
                    }
                }
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 20);
                    cmd.Parameters.AddWithValue("@Promocode", txtPromoDiscount.Value.Trim());
                    cmd.Parameters.AddWithValue("@PlanID", planId);
                    cmd.Parameters.AddWithValue("@ProdMid", ddlProductType.SelectedValue.Trim());
                    cmd.Parameters.AddWithValue("@ProCat", ddlsubcatg.SelectedValue.Trim());
                    cmd.Parameters.AddWithValue("@Brand", ddlBrand.SelectedValue.Trim());
                    cmd.Parameters.AddWithValue("@ProductPrice", txtPrice.Text.Trim());
                    cmd.Parameters.AddWithValue("@imei", !string.IsNullOrWhiteSpace(txtimeiNo.Text) ? txtimeiNo.Text.Trim() : txtSerialNo.Text);
                    cmd.Parameters.AddWithValue("@Retailer_MobileNo", Session["MobileNo"].ToString());

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        decimal discountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                        decimal discountPercent = Convert.ToDecimal(reader["DiscountPer"]);

                        if (planPrice > 0)
                        {
                            if (discountAmount > 0)
                            {
                                decimal total = planPrice - discountAmount;
                                lblPlanPrice.Text = "Rs. " + planPrice.ToString("0.00");
                                lblDiscountAmount.Text = "Rs. " + discountAmount.ToString("0.00");
                                lblTotalAmount.Text = "Rs. " + total.ToString("0.00");
                                calculationdiv.Visible = true;
                                lblPromoCodeDiscountAmount.InnerText = $"Promo Discount Amount :";
                                lblErrorPromoCode.Visible = false;
                                PromoCodeDiv.Visible = true;
                            }
                            else if (discountPercent > 0)
                            {
                                discountAmount = (planPrice * discountPercent) / 100;
                                decimal total = planPrice - discountAmount;
                                lblPlanPrice.Text = "Rs. " + planPrice.ToString("0.00");
                                lblDiscountAmount.Text = "Rs.  " + discountAmount.ToString("0.00");
                                lblTotalAmount.Text = "Rs.  " + total.ToString("0.00");
                                calculationdiv.Visible = true;
                                lblPromoCodeDiscountAmount.InnerText = $"Promo Discount Amount({discountPercent}%) :";
                                lblErrorPromoCode.Visible = false;
                                PromoCodeDiv.Visible = true;
                            }
                        }
                        else
                        {
                            lblErrorPromoCode.Visible = true;
                            lblErrorPromoCode.Text = "No plan selected.";
                        }
                    }
                    else
                    {

                        lblPlanPrice.Text = "Rs. " + planPrice.ToString("0.00");
                        lblDiscountAmount.Text = "Rs. " + "0.00";
                        lblTotalAmount.Text = "Rs. " + planPrice.ToString("0.00");
                        calculationdiv.Visible = true;
                        lblPromoCodeDiscountAmount.InnerText = $"Promo Discount Amount :";
                        lblErrorPromoCode.Visible = true;
                        lblErrorPromoCode.Text = "Invalid promo code.";
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessage(this, "Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void BindAddOnsData()
        {
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 34);
                cmd.Parameters.AddWithValue("@ProductType", ddlProductType.SelectedValue);
                cmd.Parameters.AddWithValue("@ProductSubCat", ddlsubcatg.SelectedValue);
                cmd.Parameters.AddWithValue("@Brand", ddlBrand.SelectedValue.ToString());

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    rptAddOns.DataSource = dt;
                    rptAddOns.DataBind();
                }
                else
                {
                    AddOnsDiv.Visible = false;
                    rptAddOns.DataSource = null;
                    rptAddOns.DataBind();
                }
            }
        }

        protected void chkSelect_AddOnsChanged(object sender, EventArgs e)
        {
            decimal totalAddOnPrice = 0;

            foreach (RepeaterItem item in rptAddOns.Items)
            {
                CheckBox chk = (CheckBox)item.FindControl("chkAddOnsSelect");
                HiddenField hdnAddOnsId = (HiddenField)item.FindControl("hdnAddOnsId");

                if (chk.Checked)
                {
                    Label lblAddOnFinalPrice = (Label)item.FindControl("lblFinalPrice");
                    if (lblAddOnFinalPrice != null)
                    {
                        decimal price = 0;
                        decimal.TryParse(lblAddOnFinalPrice.Text.Replace("Rs. ", "").Trim(), out price);
                        totalAddOnPrice += price;
                    }
                }
            }

            decimal basePlanPrice = 0;
            decimal.TryParse(lblPlanPrice.Text.Replace("Rs. ", "").Trim(), out basePlanPrice);
            decimal promoDiscount = 0;
            decimal.TryParse(lblDiscountAmount.Text.Replace("Rs. ", "").Trim(), out promoDiscount);

            decimal total = basePlanPrice + totalAddOnPrice - promoDiscount;
            lblTotalAmount.Text = "Rs. " + total.ToString("N2");

            calculationdiv.Visible = true;
        }
        protected void WarrantyChanged(object sender, EventArgs e)
        {
            lbl3M.CssClass = "btn btn-outline-primary";
            lbl6M.CssClass = "btn btn-outline-primary";
            lbl1Y.CssClass = "btn btn-outline-primary";
            lbl2Y.CssClass = "btn btn-outline-primary";
            lbl3Y.CssClass = "btn btn-outline-primary";
            lblCustom.CssClass = "btn btn-outline-warning";

            if (rb3M.Checked)
            {
                lbl3M.CssClass += " selectWarranty";
                customWarrantyDiv.Visible = false;
            }
            else if (rb6M.Checked)
            {
                lbl6M.CssClass += " selectWarranty";
                customWarrantyDiv.Visible = false;
            }
            else if (rb1Y.Checked)
            {
                lbl1Y.CssClass += " selectWarranty";
                customWarrantyDiv.Visible = false;
            }
            else if (rb2Y.Checked)
            {
                lbl2Y.CssClass += " selectWarranty";
                customWarrantyDiv.Visible = false;
            }
            else if (rb3Y.Checked)
            {
                lbl3Y.CssClass += " selectWarranty";
                customWarrantyDiv.Visible = false;
            }
            else if (rbCustom.Checked)
            {
                lblCustom.CssClass += " selectCustomWarranty";
                customWarrantyDiv.Visible = true;

                if (ddlCustomYears.Items.Count <= 1)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        string value = i.ToString("D2");
                        ddlCustomYears.Items.Add(new ListItem(value, value));
                    }
                }
                else
                {
                    ddlCustomYears.SelectedIndex = 0;
                }

                if (ddlCustomMonths.Items.Count <= 1)
                {
                    for (int i = 0; i <= 12; i++)
                    {
                        string value = i.ToString("D2");
                        ddlCustomMonths.Items.Add(new ListItem(value, value));
                    }
                }
                else
                    ddlCustomMonths.SelectedIndex = 0;

                if (ddlCustomDays.Items.Count <= 1)
                {
                    for (int i = 0; i <= 31; i++)
                    {
                        string value = i.ToString("D2");
                        ddlCustomDays.Items.Add(new ListItem(value, value));
                    }
                }
                else
                    ddlCustomDays.SelectedIndex = 0;
            }
            txtCustomerName.Focus();
        }
        protected void ValidateWarranty(object source, ServerValidateEventArgs args)
        {
            if (rb3M.Checked || rb6M.Checked || rb1Y.Checked || rb2Y.Checked || rb3Y.Checked)
            {
                args.IsValid = true;
            }
            else if (rbCustom.Checked)
            {
                args.IsValid = !string.IsNullOrEmpty(ddlCustomYears.SelectedValue);
            }
            else
            {
                args.IsValid = false;
            }
        }


        protected void btnConfirmBuy_Click(object sender, EventArgs e)
        {
            invalidCheck.Checked = true;
            lblErrorTermCondition.Visible = false;
            btnSubmitMain.Focus();
        }

        protected bool CheckBlockCustomer()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerMobileNo", SqlDbType.VarChar).Value = !string.IsNullOrWhiteSpace(txtCustomerMobile.Text) ? txtCustomerMobile.Text.ToString() : null;
                cmd.Parameters.AddWithValue("@CustomerEmailID", SqlDbType.VarChar).Value = !string.IsNullOrWhiteSpace(txtCustomerEmail.Text.ToLower()) ? txtCustomerEmail.Text.ToLower().ToString() : null;
                cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 39;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string suspicious = dt.Rows[0]["suspicious"].ToString();
                    if (suspicious == "1")
                    {
                        BlockCustomerMobileErrorMessage.Text = "This Customer is Under Watch.Please contact your Manager";
                        //customerInfoDiv.Attributes.Add("style", "background:gray");
                        return true;
                    }
                    else if (suspicious == "2")
                    {
                        BlockCustomerMobileErrorMessage.Text = "This Customer is Black Listed..Please contact your Manager";
                        //customerInfoDiv.Attributes.Add("style", "background:pink");
                        return true;

                    }
                    else if (suspicious == "")
                    {
                        BlockCustomerMobileErrorMessage.Text = "";
                        //customerInfoDiv.Attributes.Add("style", "background:white");
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
        }
        bool IsValidIMEI(string imeiNumber)
        {
            int sum = 0;
            for (int i = 0; i < 15; i++)
            {
                int digit = int.Parse(imeiNumber[i].ToString());
                if (i % 2 == 1)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }
                sum += digit;
            }
            return sum % 10 == 0;
        }

        protected bool CheckSalesPersonOrRetailer()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerMobileNo", !string.IsNullOrWhiteSpace(txtCustomerMobile.Text) ? txtCustomerMobile.Text.Trim() : null);
                    cmd.Parameters.AddWithValue("@CustomerEmailID", !string.IsNullOrWhiteSpace(txtCustomerEmail.Text.ToLower()) ? txtCustomerEmail.Text.ToLower().Trim() : null);
                    cmd.Parameters.AddWithValue("@Mid", "");
                    cmd.Parameters.AddWithValue("@type", 50);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        BlockCustomerMobileErrorMessage.Text = "This Mobile No or Email is already registered";
                        //customerInfoDiv.Attributes.Add("style", "background:gray");
                        return true;
                    }
                    else
                    {
                      //  customerInfoDiv.Attributes.Add("style", "background:white");
                        //BlockCustomerErrorMessage.Text = "";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        protected void btnClearCart_Click(object sender, EventArgs e)
        {
            btnClearCart.Visible = false;
            if (Session["salesOrderID"] != null)
            {
                Session.Remove("salesOrderID");
                txtCustomerMobile.Enabled = true;
                txtCustomerEmail.Enabled = true;
                txtCustomerName.Enabled = true;

                ddlProductType.Enabled = true;
                ddlProductSubType.Enabled = true;
                txtPrice.Enabled = true;
                txtPurchaseDate.Enabled = true;
                ddlBrand.Enabled = true;
                txtModel.Enabled = true;
                validationCustom06.Enabled = true;
                txtSerialNo.Enabled = true;
                txtimeiNo.Enabled = true;
                txtimeiNo2.Enabled = true;
                txtDateOfImpl.Enabled = true;
                rb3M.Enabled = true;
                rb6M.Enabled = true;
                rb1Y.Enabled = true;
                rb2Y.Enabled = true;
                rb3Y.Enabled = true;
                ddlCustomYears.Enabled = true;
                ddlCustomMonths.Enabled = true;
                ddlCustomDays.Enabled = true;
                txtCustomerName.Enabled = true;
                txtCustomerEmail.Enabled = true;
                txtCustomerMobile.Enabled = true;
            }

        }

        protected void txtCustomerEmail_TextChanged(object sender, EventArgs e)
        {
            bool blockuser = CheckBlockCustomer();
            bool existuser = CheckSalesPersonOrRetailer();
            if (existuser)
            {
                BlockCustomerEmailErrorMessage.Text = "This Email is already registered";
                BlockCustomerMobileErrorMessage.Text = "";
                btnSubmitPlan.Enabled = false;
                return;
            }
            else if (blockuser)
            {
                BlockCustomerEmailErrorMessage.Text = BlockCustomerMobileErrorMessage.Text;
                BlockCustomerMobileErrorMessage.Text = "";
                btnSubmitPlan.Enabled = false;
                return;
            }
            BlockCustomerEmailErrorMessage.Text = "";
            btnSubmitPlan.Enabled = true;
            txtCustomerMobile.Focus();
        }
        protected void txtCustomerMobile_TextChanged(object sender, EventArgs e)
        {
            bool blockuser = CheckBlockCustomer();
            bool existuser = CheckSalesPersonOrRetailer();
            if (existuser)
            {
                BlockCustomerMobileErrorMessage.Text = "This Mobile No is already registered";
                BlockCustomerEmailErrorMessage.Text = "";
                btnSubmitPlan.Enabled = false;
                return;
            }
            else if (blockuser)
            {
                BlockCustomerMobileErrorMessage.Text = BlockCustomerMobileErrorMessage.Text;
                BlockCustomerEmailErrorMessage.Text = "";
                btnSubmitPlan.Enabled = false;
                return;
            }
            BlockCustomerMobileErrorMessage.Text = "";
            btnSubmitPlan.Enabled = true;
            btnSubmitPlan.Focus();
        }

        protected void txtPurchaseDate_TextChanged(object sender, EventArgs e)
        {
            DateTime purchaseDate;

            if (DateTime.TryParse(txtPurchaseDate.Text, out purchaseDate))
            {
                CalendarExtender1.StartDate = purchaseDate;
            }
            else
            {
                CalendarExtender1.StartDate = DateTime.Today.AddMonths(-11);
            }
            if (!string.IsNullOrEmpty(txtDateOfImpl.Text))
            {
                DateTime installDate;
                if (DateTime.TryParse(txtDateOfImpl.Text, out installDate))
                {
                    if (installDate < purchaseDate || installDate > DateTime.Today)
                    {
                        txtDateOfImpl.Text = string.Empty;
                    }
                }
            }
            txtSerialNo.Focus();
        }
        protected void AddIncomepletePurchase(string status)
        {
            string planName = string.Empty;
            string planPrice = string.Empty;
            string SKU = string.Empty;
            foreach (RepeaterItem item in rptPlans.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                if (chkSelect.Checked)
                {
                    planName = ((Label)item.FindControl("lblPlanFullName"))?.Text ?? string.Empty;
                    planPrice = ((HiddenField)item.FindControl("hdnPlanPrice")).Value;
                    SKU = ((HiddenField)item.FindControl("hdnSKU")).Value;
                    break;
                }
            }

            string selectedWarranty = "";
            if (rb3M.Checked)
                selectedWarranty = "00/03/00";
            else if (rb6M.Checked)
                selectedWarranty = "00/06/00";
            else if (rb1Y.Checked)
                selectedWarranty = "01/00/00";
            else if (rb2Y.Checked)
                selectedWarranty = "02/00/00";
            else if (rb3Y.Checked)
                selectedWarranty = "03/00/00";
            else if (rbCustom.Checked)
            {
                string year = string.IsNullOrEmpty(ddlCustomYears.SelectedValue) ? "00" : ddlCustomYears.SelectedValue;
                string month = string.IsNullOrEmpty(ddlCustomMonths.SelectedValue) ? "00" : ddlCustomMonths.SelectedValue;
                string day = string.IsNullOrEmpty(ddlCustomDays.SelectedValue) ? "00" : ddlCustomDays.SelectedValue;

                selectedWarranty = $"{year}/{month}/{day}";
            }

            string categoryId = "";
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 10);
                cmd.Parameters.AddWithValue("@SKU", SKU);

                if (con.State != ConnectionState.Open)
                    con.Open();

                using (SqlDataReader CatReader = cmd.ExecuteReader())
                {
                    if (CatReader.Read())
                    {
                        categoryId = CatReader["categoryid"].ToString();
                    }
                    CatReader.Close();
                }
            }
            int planId = 0;
            foreach (RepeaterItem item in rptPlans.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                if (chkSelect != null && chkSelect.Checked)
                {
                    HiddenField hdnPlanId = (HiddenField)item.FindControl("hdnPlanId");
                   
                    if (hdnPlanId != null)
                    {
                        planId = Convert.ToInt32(hdnPlanId.Value);
                    }
                    break;
                }
            }
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if ((Session["cartItemId"] != null && Session["UniqueID"] != null) || Session["IncompleteMid"] != null)
                {
                    cmd.Parameters.AddWithValue("@type", 57);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@type", 56);
                }
                cmd.Parameters.AddWithValue("@Saleschannel", "Retailer");
                cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"].ToString());
                cmd.Parameters.AddWithValue("@ProductSubCat", ddlsubcatg.SelectedValue);
                cmd.Parameters.AddWithValue("@Productname", lblProductName.Text);
                cmd.Parameters.AddWithValue("@ProductType", ddlProductType.SelectedValue);
                cmd.Parameters.AddWithValue("@Productsubcategoryname", ddlsubcatg.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@Brand", ddlBrand.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@SubProductType", ddlProductSubType.SelectedValue);
                cmd.Parameters.AddWithValue("@ModalName", txtModel.Text.ToString());
                cmd.Parameters.AddWithValue("@imei", !string.IsNullOrWhiteSpace(txtimeiNo.Text) ? txtimeiNo.Text.ToString() : null);
                cmd.Parameters.AddWithValue("@imei2", !string.IsNullOrWhiteSpace(txtimeiNo2.Text) ? txtimeiNo2.Text.ToString() : null);//sfasdfsdfsd
                cmd.Parameters.AddWithValue("@serialno", !string.IsNullOrWhiteSpace(txtSerialNo.Text) ? txtSerialNo.Text.ToString() : null);
                cmd.Parameters.AddWithValue("@DevicePurchasePrice", !string.IsNullOrWhiteSpace(txtPrice.Text) ? Convert.ToDecimal(txtPrice.Text.ToString()) : 0);
                cmd.Parameters.AddWithValue("@ProductPurchaseDate", !string.IsNullOrWhiteSpace(txtPurchaseDate.Text) ? Convert.ToDateTime(txtPurchaseDate.Text.ToString()) : DateTime.Now);
                cmd.Parameters.AddWithValue("@CustomerEmailID", txtCustomerEmail.Text.ToLower().ToString());
                cmd.Parameters.AddWithValue("@CustomerMobileNo", txtCustomerMobile.Text.ToString());
                cmd.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text.ToString());
                cmd.Parameters.AddWithValue("@PlanName", planName);
                cmd.Parameters.AddWithValue("@PlanPrice", !string.IsNullOrWhiteSpace(planPrice) ? Convert.ToDecimal(planPrice) : 0);
                cmd.Parameters.AddWithValue("@Promocode", txtPromoDiscount.Value.ToString());
                cmd.Parameters.AddWithValue("@PromoDiscount", !string.IsNullOrWhiteSpace(lblDiscountAmount.Text) ? Convert.ToDecimal(lblDiscountAmount.Text.Replace("Rs. ", "").Trim().ToString()) : 0);
                cmd.Parameters.AddWithValue("@PlanTotalValue", !string.IsNullOrWhiteSpace(lblTotalAmount.Text) ? Convert.ToDecimal(lblTotalAmount.Text.Replace("Rs. ", "").Trim().ToString()) : 0);
                cmd.Parameters.AddWithValue("@DateofImplementation", !string.IsNullOrWhiteSpace(txtDateOfImpl.Text) ? Convert.ToDateTime(txtDateOfImpl.Text.ToString()) : DateTime.Now);
                cmd.Parameters.AddWithValue("@ManufacturerWarranty_yymmdd", selectedWarranty);
                cmd.Parameters.AddWithValue("@categoryid", categoryId);
                cmd.Parameters.AddWithValue("@PlanSKU", SKU);
                cmd.Parameters.AddWithValue("@subcatgId", ddlsubcatg.SelectedValue);
                cmd.Parameters.AddWithValue("@PlanID", planId);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@customer_status", status);
                cmd.Parameters.AddWithValue("@SalesOrderID", Session["salesOrderID"] != null ? Session["salesOrderID"].ToString() : null);
                cmd.Parameters.AddWithValue("@Customer_OrderID", Session["Customerorder"] != null ? Session["Customerorder"].ToString() : null);
                cmd.Parameters.AddWithValue("@cartItemId", Session["cartItemId"] != null ? Session["cartItemId"].ToString() : null);
                cmd.Parameters.AddWithValue("@UniqueID", Session["UniqueID"] != null ? Session["UniqueID"].ToString() : null);
                cmd.Parameters.AddWithValue("@Mid", Session["IncompleteMid"] != null ? Session["IncompleteMid"].ToString() : null);

                if (con.State != ConnectionState.Open)
                    con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string cartItemId = reader["cartItemId"].ToString();
                        string UniqueID = reader["UniqueID"].ToString();

                        Session["cartItemId"] = cartItemId;
                        Session["UniqueID"] = UniqueID;

                    }
                }
            }
            con.Close();
        }

        protected void IMEINoChange(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@imei", txtimeiNo.Text);
                cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 79;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    imeiError.InnerText = "IMEI No. already exists please contact the supervisor";
                    return;
                }
                else
                {
                    imeiError.InnerText = "";
                    if (!string.IsNullOrWhiteSpace(txtimeiNo.Text) && !string.IsNullOrWhiteSpace(txtimeiNo2.Text) && txtimeiNo.Text == txtimeiNo2.Text)
                    {
                        imeiError.InnerText = "IMEI No. 1 and IMEI No. 2 cannot be same";
                        return;
                    }
                    else
                    {
                        imeiError.InnerText = "";
                    }
                }
            }
            catch (Exception ex)
            {
                imeiError.InnerText = "An error occurred. Please try again.";
            }
        }
        protected void IMEINo2Change(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@imei", txtimeiNo2.Text);
                cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 80;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    Small1.InnerText = "IMEI No. already exists please contact the supervisor";
                    return;
                }
                else
                {
                    Small1.InnerText = "";
                    if (!string.IsNullOrWhiteSpace(txtimeiNo2.Text) && !string.IsNullOrWhiteSpace(txtimeiNo.Text) && txtimeiNo.Text == txtimeiNo2.Text)
                    {
                        Small1.InnerText = "IMEI No. 1 and IMEI No. 2 cannot be same";
                        return;
                    }
                    else
                    {
                        Small1.InnerText = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Small1.InnerText = "An error occurred. Please try again.";
            }
        }
        protected void SerialNoChange(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@serialno", txtSerialNo.Text);
                cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 81;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    serialNoError.InnerText = "Serial No. already exists please contact the supervisor";
                    return;
                }
                else
                {
                    serialNoError.InnerText = "";
                }
            }
            catch (Exception ex)
            {
                serialNoError.InnerText = "An error occurred. Please try again.";
            }
        }

    }
}