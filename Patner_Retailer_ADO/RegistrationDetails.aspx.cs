using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Patner_Retailer_ADO
{
    public partial class RegistrationDetails : System.Web.UI.Page
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
                string sku = Request.QueryString["qe"];
                if (sku != null)
                {
                    BindProductInfo(sku);
                }
            }
        }
        protected void BindProductInfo(string sku)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", 53);
                    cmd.Parameters.AddWithValue("@Retailer_FreelanceID", Session["MobileNo"].ToString());
                    cmd.Parameters.AddWithValue("@SKU", sku);

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ViewState["AllInfo"] = dt;
                    con.Close();

                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];

                        GvCustomerDetails.DataSource = dt;
                        GvCustomerDetails.DataBind();

                        GVProductDetails.DataSource = dt;
                        GVProductDetails.DataBind();

                        //GVPlanDetails.DataSource = dt;
                        //GVPlanDetails.DataBind();
                        
                        //GVCommissionDetails.DataSource = dt;
                        //GVCommissionDetails.DataBind();

                        GvCustomerDetails.CssClass = "table data-table table-striped nowrap";
                        GVProductDetails.CssClass = "table data-table table-striped nowrap";
                        //GVPlanDetails.CssClass = "table data-table table-striped nowrap";
                        //GVCommissionDetails.CssClass = "table data-table table-striped nowrap";
                        if (GvCustomerDetails.HeaderRow != null)
                        {
                            GvCustomerDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                        if (GVProductDetails.HeaderRow != null)
                        {
                            GVProductDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                        string commisionType = row["CommissionType"]?.ToString();
                        if (commisionType == "Percantage")
                        {
                            Commossiontag.InnerText = "Retailer Commision("+ row["CommissionPercentage"]?.ToString() + "%)";
                        }
                        txtPlanNicknameSelection.InnerText = row["PlanNicknameSelection"]?.ToString();
                        MRP.InnerText = FormatDecimal(row["MRP"]);
                        PlanPrice.InnerText = FormatDecimal(row["PlanPrice"]);
                        TaxableValue.InnerText = FormatDecimal(row["TaxableValue"]);
                        TaxAmout.InnerText = FormatDecimal(row["TaxAmout"]);
                        Commission.InnerText = FormatDecimal(row["Commission"]);
                        CommissionTaxableValue.InnerText = FormatDecimal(row["CommissionTaxableValue"]);
                        CommissionTaxValue.InnerText = FormatDecimal(row["CommissionTaxValue"]);
                        TotalAmountPay.InnerText = FormatDecimal(row["TotalAmountPay"]);

                        /*  if (GVPlanDetails.HeaderRow != null)
                          {
                              GVPlanDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                          }
                          if (GVCommissionDetails.HeaderRow != null)
                          {
                              GVCommissionDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                          }*/

                        /*                        lblNameValue.Text = row["CustomerName"]?.ToString();
                                                lblMobileNoValue.Text = row["MobileNo"]?.ToString();
                                                lblWhatsappNoValue.Text = row["WhatsappNo"]?.ToString();
                                                lblEmailValue.Text = row["EmailIDAddress"]?.ToString();
                                                lblPincodeValue.Text = row["Pincode"]?.ToString();
                                                lblCityValue.Text = row["City"]?.ToString();
                                                lblStateValue.Text = row["State"]?.ToString();
                                                lblAddressValue.Text = row["AddressLine1"]?.ToString();

                                                lblCategoryValue.Text = row["Productsubcategoryname"].ToString();
                                                lblProductValue.Text = row["Productname"].ToString();
                                                lblBrandValue.Text = row["Brand"].ToString();
                                                lblModelValue.Text = row["ModalName"].ToString();
                                                lblIMEI1Value.Text = row["imei"].ToString();
                                                lblIMEI2Value.Text = row["imei2"].ToString();
                                                lblDevicePriceValue.Text = row["DevicePurchasePrice"].ToString();
                                                lblDeviceDateValue.Text = row["ProductPurchaseDate"].ToString();
                                                lblProductStatusValue.Text = row["ProdStatus"].ToString();

                                                lblPlanValue.Text = row["PlanNicknameSelection"].ToString();
                                                lblDescriptionValue.Text = row["FinalPlanNameDescription"].ToString();
                                                lblSKUValue.Text = row["SKU"].ToString();
                                                lblOfferValue.Text = row["TaxableValue"].ToString();
                                                lblDiscountValue.Text = row["Discount"].ToString();
                                                lblMRPValue.Text = row["MRP"].ToString();
                                                lblPlanPeriodValue.Text = row["extendedwarrantystartdate"].ToString() + " to " + row["extendedwarrantyenddate"].ToString();
                                                lblPlanStatusValue.Text = row["planStatus"].ToString();

                                                lblCommissionTypeVale.Text = row["CommissionType"].ToString();
                                                lblCommissionPerValue.Text = row["CommissionPercentage"].ToString();
                                                lblCommissionAmountValue.Text = row["CommissionValue"].ToString();
                                                lblCommissionStatusValue.Text = row["PaymentStatus"].ToString();
                                                lblGSTCommissionPerValue.Text = row["GSTCommissionPercentage"].ToString();
                                                lblGSTCommissionAmountValue.Text = row["GSTCommissionValue"].ToString();
                                                lblGSTCommissionStatusValue.Text = row["GSTCommissionPaid"].ToString();*/

                    }
                    else
                    {

                        GvCustomerDetails.DataSource = null;
                        GvCustomerDetails.DataBind();

                        GVProductDetails.DataSource = null;
                        GVProductDetails.DataBind();

                       /* GVPlanDetails.DataSource = null;
                        GVPlanDetails.DataBind();

                        GVCommissionDetails.DataSource = null;
                        GVCommissionDetails.DataBind();*/

                        GvCustomerDetails.CssClass = "table table-striped nowrap";
                        GVProductDetails.CssClass = "table table-striped nowrap";
                        /*GVPlanDetails.CssClass = "table table-striped nowrap";
                        GVCommissionDetails.CssClass = "table table-striped nowrap";*/
                        /*lblNameValue.Text = "";
                        lblMobileNoValue.Text = "";
                        lblWhatsappNoValue.Text = "";
                        lblEmailValue.Text = "";
                        lblPincodeValue.Text = "";
                        lblCityValue.Text = "";
                        lblStateValue.Text = "";
                        lblAddressValue.Text = "";

                        lblProductValue.Text = "";
                        lblCategoryValue.Text = "";
                        lblBrandValue.Text = "";
                        lblModelValue.Text = "";
                        lblIMEI1Value.Text = "";
                        lblIMEI2Value.Text = "";
                        lblDevicePriceValue.Text = "";
                        lblDeviceDateValue.Text = "";
                        lblProductStatusValue.Text = "";

                        lblPlanValue.Text = "";
                        lblDescriptionValue.Text = "";
                        lblSKUValue.Text = "";
                        lblOfferValue.Text = "";
                        lblDiscountValue.Text = "";
                        lblMRPValue.Text = "";
                        lblPlanPeriodValue.Text = "";
                        lblPlanStatusValue.Text = "";

                        lblCommissionTypeVale.Text = "";
                        lblCommissionPerValue.Text = "";
                        lblCommissionAmountValue.Text = "";
                        lblCommissionStatusValue.Text = "";
                        lblGSTCommissionPerValue.Text = "";
                        lblGSTCommissionAmountValue.Text = "";
                        lblGSTCommissionStatusValue.Text = "";*/

                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        private string FormatDecimal(object value)
        {
            if (value == null || value == DBNull.Value)
                return "0.00";

            if (decimal.TryParse(value.ToString(), out decimal result))
                return result.ToString("0.00");

            return "0.00";
        }

    }
}