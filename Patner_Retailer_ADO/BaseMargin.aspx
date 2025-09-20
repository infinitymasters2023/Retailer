<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BaseMargin.aspx.cs" Inherits="Patner_Retailer_ADO.BaseMargin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        :root {
            --primary-green: #27ae60;
            --secondary-green: #2ecc71;
            --orange: #f39c12;
            --blue: #2980b9;
            --purple: #8e44ad;
            --light-gray: #f9f9f9;
            --text-dark: #333;
            --text-light: #666;
        }

        body, p, h2, h3, h4 {
            margin: 0;
            padding: 0;
        }

            h2.section-title {
                text-align: center;
                margin: 50px 0 10px;
                font-size: 30px;
                font-weight: 700;
                color: var(--text-dark);
                display: flex;
            }

            p.section-subtitle {
                color: var(--text-light);
                margin-bottom: 40px;
                font-size: 16px;
            }

        .base-margin-container {
            display: flex;
            justify-content: center;
            align-items: flex-end;
            gap: 40px;
            flex-wrap: nowrap;
            margin-bottom: 60px;
        }

        .step-box {
            position: relative;
            background: linear-gradient(135deg, #eafaf1, #d7f5e3);
            border: 1px solid #bfe8d0;
            border-radius: 5px;
            padding: 20px;
            width: 160px;
            height: 120px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.12);
            text-align: center;
            transition: transform 0.3s;
            box-shadow: -18px -11px 0px 0px #ed7d31;
        }

            .step-box:hover {
                transform: translateY(-6px);
            }

            .step-box:nth-child(odd) {
                background: linear-gradient(135deg, #d0f0db, #bfe8ce);
            }

        .step-header {
            position: absolute;
            top: -22px;
            left: 50%;
            transform: translateX(-50%) skewX(-15deg);
            background: var(--orange);
            padding: 6px 16px;
            border-radius: 6px;
            color: #fff;
            font-weight: bold;
            font-size: 15px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.2);
        }

            .step-header span {
                transform: skewX(15deg);
                display: inline-block;
            }

        .step-box p {
            margin-top: 42px;
            font-size: 15px;
            font-weight: 600;
            color: var(--text-dark);
        }

        .base-margin-container .step-box {
            margin-bottom: calc(var(--i) * 25px);
        }

            .base-margin-container .step-box:nth-child(1) {
                --i: 0;
            }

            .base-margin-container .step-box:nth-child(2) {
                --i: 1;
            }

            .base-margin-container .step-box:nth-child(3) {
                --i: 2;
            }

            .base-margin-container .step-box:nth-child(4) {
                --i: 3;
            }

            .base-margin-container .step-box:nth-child(5) {
                --i: 4;
            }

            .base-margin-container .step-box:nth-child(6) {
                --i: 5;
            }

            .base-margin-container .step-box:nth-child(7) {
                --i: 6;
            }
        .section-title-Para {
            font-size: 20px !important;
            font-weight: 400 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-0">
        <div class="card">
            <div class="card-body">

                <!-- Base Margin -->
                <h2 class="section-title">Slab Based Margin &nbsp;<p class="section-title-Para">- in % of Net Sales Value</p>
                </h2>
                <p class="section-subtitle">(Grows with respect to the sales volumes in a Calendar month)</p>
                <div class="base-margin-container">
                    <div class="step-box">
                        <div class="step-header"><span>20%</span></div>
                        <p>Up to 20 Plans</p>
                    </div>
                    <div class="step-box">
                        <div class="step-header"><span>21%</span></div>
                        <p>Up to 25 Plans</p>
                    </div>
                    <div class="step-box">
                        <div class="step-header"><span>22%</span></div>
                        <p>Up to 30 Plans</p>
                    </div>
                    <div class="step-box">
                        <div class="step-header"><span>23%</span></div>
                        <p>Up to 37 Plans</p>
                    </div>
                    <div class="step-box">
                        <div class="step-header"><span>24%</span></div>
                        <p>Up to 45 Plans</p>
                    </div>
                    <div class="step-box">
                        <div class="step-header"><span>25%</span></div>
                        <p>Up to 55 Plans</p>
                    </div>
                    <div class="step-box">
                        <div class="step-header"><span>27%</span></div>
                        <p>Above 56 Plans</p>
                    </div>
                </div>
            </div>
        </div>
        <div class="card mt-3">
            <div class="card-body mt-3">
                <div>
                    <asp:GridView ID="GVBaseCommisionDetails" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead"
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
