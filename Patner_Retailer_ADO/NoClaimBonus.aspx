<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="NoClaimBonus.aspx.cs" Inherits="Patner_Retailer_ADO.NoClaimBonus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ncb-box {
            position: relative;
            display: flex;
            flex-direction: column;
            max-width: 300px;
            height: 250px;
            margin: 60px auto 40px auto;
            border: 1px solid #c8d3e6;
            background-color: #eaf1f9;
            box-shadow: 0 4px 15px rgba(0,0,0,0.1);
        }

        .ncb-percent {
            position: absolute;
            top: -40px;
            left: -70px;
            width: 100px;
            height: 100px;
            background: #3498db;
            border-radius: 50%;
            border: 2px solid #2980b9;
            display: flex;
            justify-content: center;
            align-items: center;
            color: #ffeb3b;
            font-size: 36px;
            font-weight: bold;
            box-shadow: 0 4px 10px rgba(0,0,0,0.2);
        }

        .ncb-section {
            flex-grow: 1;
            display: flex;
            justify-content: center;
            align-items: center;
            text-align: center;
            font-size: 28px;
            font-weight: 500;
            color: #000;
            padding: 10px;
            line-height: 32px;
        }

        .ncb-top {
            border-bottom: 1px solid #c8d3e6;
        }

        .ncb-note {
            font-size: 10px;
            color: var(--text-light);
            text-align: center;
            margin-top: 15px;
        }

        .total-margin-box.ncb-margin {
            max-width: 400px;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-0">
        <div class="card">
            <div class="card-body">
                <div class="ncb-box">
                    <div class="ncb-percent">2%</div>
                    <div class="ncb-section ncb-top">No Claim Bonus*</div>
                    <div class="ncb-section">Direct Benefits to the Service Centre</div>
                </div>
                <p class="ncb-note">*NCB: No Claim Bonus, will be due post plan expiry, if no claim is made by customer</p>
            </div>
        </div>
        <div class="card mt-3">
    <div class="card-body mt-3">
        <div>
            <asp:GridView ID="GVNCB" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead"
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
