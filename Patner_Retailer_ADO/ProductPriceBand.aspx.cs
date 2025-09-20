using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Patner_Retailer_ADO
{
    public partial class ProductPriceBand : System.Web.UI.Page
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
                BindProductPriceBand();
            }
        }

        private void BindProductPriceBand()
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_iapl_PartnerRetailer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@type", 95);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            gvPlans.DataSource = dt;
                            gvPlans.DataBind();

                        }
                        else
                        {
                            gvPlans.DataSource = null;
                            gvPlans.DataBind();
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


        protected string lastProductType = string.Empty;

        protected void gvPlans_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView gv = (GridView)sender;


                // Second row: EW merged columns
                GridViewRow headerRow2 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                headerRow2.Cells.Add(new TableHeaderCell { Text = "PriceBand", RowSpan = 1, ColumnSpan = 2, HorizontalAlign = HorizontalAlign.Center });

                string[] ewYears = { "1Y EW", "2Y EW", "3Y EW", "4Y EW" };
                foreach (string year in ewYears)
                {
                    TableHeaderCell ewCell = new TableHeaderCell
                    {
                        Text = year,
                        ColumnSpan = 2,
                        HorizontalAlign = HorizontalAlign.Center
                    };
                    headerRow2.Cells.Add(ewCell);
                }

                gv.Controls[0].Controls.AddAt(0, headerRow2);

            }
        }

        protected void gvPlans_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string currentProduct = DataBinder.Eval(e.Row.DataItem, "ProductType").ToString();
                GridView gv = (GridView)sender;

                if (lastProductType != currentProduct)
                {

                    Table gridTable = (Table)gv.Controls[0];

                    int rowIndex = gridTable.Rows.GetRowIndex(e.Row);
                    lastProductType = currentProduct;

                    GridViewRow headerRow3 = new GridViewRow(rowIndex, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    headerRow3.Cells.Add(new TableHeaderCell { Text = currentProduct, RowSpan = 1, HorizontalAlign = HorizontalAlign.Center });
                    headerRow3.Cells.Add(new TableHeaderCell { Text = "Price Band", RowSpan = 1, HorizontalAlign = HorizontalAlign.Center });
                    for (int i = 0; i < 4; i++)
                    {
                        headerRow3.Cells.Add(new TableHeaderCell { Text = "MRP", RowSpan = 1, HorizontalAlign = HorizontalAlign.Center });
                        headerRow3.Cells.Add(new TableHeaderCell { Text = "Discounted Price", RowSpan = 1, HorizontalAlign = HorizontalAlign.Center });
                    }

                    gv.Controls[0].Controls.AddAt(rowIndex, headerRow3);
                }

            }
        }


    }
}