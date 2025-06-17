using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Patner_Retailer_ADO
{
    public partial class AddEditFavourite : System.Web.UI.Page
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
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                CalendarExtender3.EndDate = DateTime.Today;
                BindSubCategory();
                bindsubcatg();
                getallBrand();

                PlanPanel.Visible = false;
                txtPurchaseDate.Attributes.Add("ReadOnly", "readonly");
                btnEditPlan.Visible = false;
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
                    ddlsubcatg.DataValueField = "subcatid";
                    ddlsubcatg.DataTextField = "description";
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
            ddlProductType.Focus();
        }

        public void bindsubcatg()
        {
            try
            {
                SqlDataAdapter adp = new SqlDataAdapter("sp_iapl_PartnerRetailer", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@type", SqlDbType.Int).Value = 2;
                adp.SelectCommand.Parameters.Add("@description", SqlDbType.NVarChar).Value = ddlsubcatg.SelectedItem.Text;
                con.Open();
                DataSet ds = new DataSet();
                adp.Fill(ds);
                ddlProductType.DataSource = ds.Tables[0];
                ddlProductType.DataTextField = "subcategoryname";
                ddlProductType.DataValueField = "subcategoryid";
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
            txtMake.Text = "";
            getallBrand();
        }

        protected void getallBrand()
        {
            SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@type", SqlDbType.Int).Value = 3;
            cmd.Parameters.AddWithValue("@subcatgId", ddlProductType.SelectedValue);
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
        private void BindPlanData()
        {
            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", 4);
                cmd.Parameters.AddWithValue("@ProductType", ddlProductType.SelectedValue);
                cmd.Parameters.AddWithValue("@ProductSubCat", ddlsubcatg.SelectedValue);

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
        protected void AddToFavourite(object sender, EventArgs e)
        {
           
            string planName = string.Empty;
            string planPrice = string.Empty;
            string PlanId = string.Empty;
            foreach (RepeaterItem item in rptPlans.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                if (chkSelect.Checked)
                {
                    planName = ((Label)item.FindControl("lblPlanName"))?.Text ?? string.Empty;
                    planPrice = ((HiddenField)item.FindControl("hdnPlanPrice")).Value;
                    PlanId = ((HiddenField)item.FindControl("hdnPlanId")).Value;
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


            using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@type", 5);
                cmd.Parameters.AddWithValue("@ProfileId", Session["RetailerUniqueID"].ToString());
                cmd.Parameters.AddWithValue("@ProductSubCat", ddlsubcatg.SelectedValue);
                cmd.Parameters.AddWithValue("@Productname", ddlProductType.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@ProductType", ddlProductType.SelectedValue);
                cmd.Parameters.AddWithValue("@Productsubcategoryname", ddlsubcatg.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@Brand", ddlBrand.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@ModalName", txtModel.Text.ToString());
                cmd.Parameters.AddWithValue("@ProductPurchaseDate", !string.IsNullOrWhiteSpace(txtPurchaseDate.Text) ? Convert.ToDateTime(txtPurchaseDate.Text.ToString()) : DateTime.Now);
                cmd.Parameters.AddWithValue("@PlanName", planName);
                cmd.Parameters.AddWithValue("@PlanPrice", !string.IsNullOrWhiteSpace(planPrice) ? Convert.ToDecimal(planPrice) : 0);
                cmd.Parameters.AddWithValue("@ManufacturerWarranty_yymmdd", selectedWarranty);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Response.Redirect("Favourite.aspx");
                    }
                }
            }
            con.Close();
            Response.Redirect("Favourite.aspx");
        }

        protected void SubmitPlanInfo(object sender, EventArgs e)
        {

            int count = 0;
            if (ddlsubcatg.SelectedValue == "0")
            {
                ddlsubcatg.BorderColor = System.Drawing.Color.Red;
                count = count + 1;
            }
            else
                ddlsubcatg.BorderColor = System.Drawing.Color.LightGray;

            if (ddlProductType.SelectedValue == "0")
            {
                ddlProductType.BorderColor = System.Drawing.Color.Red;
                count = count + 1;
            }
            else
                ddlProductType.BorderColor = System.Drawing.Color.LightGray;
            if (string.IsNullOrWhiteSpace(ddlBrand.SelectedValue) || ddlBrand.SelectedValue == "0")
            {
                ddlBrand.BorderColor = System.Drawing.Color.Red;
                count = count + 1;
            }
            else
                ddlBrand.BorderColor = System.Drawing.Color.LightGray;

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


            if (count > 0)
            {
                return;
            }
            else
            {
                BindPlanData();
                btnSubmitPlan.Visible = false;
                btnEditPlan.Visible = true;
                ddlsubcatg.Enabled = false;
                ddlProductType.Enabled = false;
                txtPurchaseDate.Enabled = false;
                ddlBrand.Enabled = false;
                txtModel.Enabled = false;
                rb3M.Enabled = false;
                rb6M.Enabled = false;
                rb1Y.Enabled = false;
                rb2Y.Enabled = false;
                rb3Y.Enabled = false;
                ddlCustomYears.Enabled = false;
                ddlCustomMonths.Enabled = false;
                ddlCustomDays.Enabled = false;
            }
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void EditPlanInfo(object sender, EventArgs e)
        {
            btnEditPlan.Visible = false;
            btnSubmitPlan.Visible = true;
            ddlsubcatg.Enabled = true;
            ddlProductType.Enabled = true;
            txtPurchaseDate.Enabled = true;
            ddlBrand.Enabled = true;
            txtModel.Enabled = true;
            rb3M.Enabled = true;
            rb6M.Enabled = true;
            rb1Y.Enabled = true;
            rb2Y.Enabled = true;
            rb3Y.Enabled = true;
            ddlCustomYears.Enabled = true;
            ddlCustomMonths.Enabled = true;
            ddlCustomDays.Enabled = true;
            HideOtherPanels();
        }

        protected void HideOtherPanels()
        {
            PlanPanel.Visible = false;
            btnSubmitPlan.Visible = true;
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
    }
}