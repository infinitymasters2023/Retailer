<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AdditionalMargin.aspx.cs" Inherits="Patner_Retailer_ADO.AdditionalMargin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .promo-box {
            width: 500px;
            max-width: 90%;
            margin: 40px auto;
            border-radius: 15px;
            overflow: hidden;
            box-shadow: 0 8px 20px rgba(0,0,0,0.2);
        }

        .promo-header {
            background: linear-gradient(135deg, var(--blue), #4a69bd);
            color: #fff;
            text-align: center;
            padding: 20px;
        }

            .promo-header h3 {
                margin: 0;
                font-size: 26px;
                font-weight: bold;
                color: #ffeb3b;
            }

            .promo-header p {
                margin: 8px 0 0;
                font-size: 20px;
                font-weight: bold;
            }

        .promo-body {
            background: #eef2fa;
            padding: 20px;
            text-align: center;
        }

            .promo-body h4 {
                margin: 0 0 15px;
                font-size: 18px;
                font-weight: bold;
            }

        .promo-criteria {
            text-align: left;
            display: inline-block;
        }

            .promo-criteria p {
                margin: 8px 0;
                font-size: 16px;
                font-weight: 600;
            }

                .promo-criteria p::before {
                    content: "✦ ";
                    color: #f39c12;
                    font-size: 18px;
                    margin-right: 4px;
                }

        .promo-note {
            font-size: 12px;
            color: var(--text-light);
            margin-top: 10px;
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-0">
        <div class="card">
            <div class="card-body">
                <div class="promo-box">
                    <div class="promo-header">
                        <h3>Sell More!</h3>
                        <p>Earn More + More!</p>
                    </div>
                    <div class="promo-body">
                        <h4>"3%" additional margin when you achieve</h4>
                        <div class="promo-criteria">
                            <p>> 100 Service Plans*</p>
                            <span style="display: flex; justify-content: center; font-weight: 600;">or</span>
                            <p>> Rs.2 Lakhs in Sales Value*</p>
                        </div>
                        <p class="promo-note">(*In a Calendar month)</p>
                    </div>
                </div>
            </div>
        </div>
        <div class="card mt-3">
            <div class="card-body mt-3">
                <div>
                    <asp:GridView ID="GVAdditionalMargin" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead"
                        EmptyDataText="No records available. Please refine your search." BorderStyle="None">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                            <asp:BoundField DataField="emailID" HeaderText="Customer Email Id" />
                            <asp:BoundField DataField="CustomerMobileNo" HeaderText="Customer Mobile No" />
                            <asp:BoundField DataField="Status" HeaderText="Tran Status" />
                            <asp:BoundField DataField="TaxableValue" HeaderText="Taxable Value" DataFormatString="{0:N2}" HtmlEncode="false" />
                            <asp:BoundField DataField="TaxAmout" HeaderText="Tax Amout" DataFormatString="{0:N2}" HtmlEncode="false" />
                            <asp:BoundField DataField="TotalAmountPay" HeaderText="Total Amount Pay" DataFormatString="{0:N2}" HtmlEncode="false" />
                            <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date" />
                            <asp:BoundField DataField="OrderId" HeaderText="Order Id" />
                            <asp:BoundField DataField="Productname" HeaderText="Product Name" />
                            <asp:BoundField DataField="Productsubcategoryname" HeaderText="Product Sub Category Name" />
                            <asp:BoundField DataField="Brand" HeaderText="Brand" />
                            <asp:BoundField DataField="ModalName" HeaderText="Modal Name" />
                            <asp:BoundField DataField="serialno" HeaderText="Serial No" />
                            <asp:BoundField DataField="DevicePurchasePrice" HeaderText="Device Purchase Price" />
                            <asp:BoundField DataField="PlanNicknameSelection" HeaderText="Plan Name" />
                            <asp:BoundField DataField="PlanPrice" HeaderText="Plan Price" />
                            <asp:BoundField DataField="DateofImplementation" HeaderText="Date of Implementation" />
                            <asp:BoundField DataField="ManufacturerWarranty_yymmdd" HeaderText="Manufacturer Warranty(yy/mm/dd)" />
                            <asp:BoundField DataField="ProductPurchaseDate" HeaderText="Product Purchase Date" />
                            <asp:BoundField DataField="imei" HeaderText="IMEI" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
