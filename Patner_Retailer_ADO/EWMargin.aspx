<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EWMargin.aspx.cs" Inherits="Patner_Retailer_ADO.EWMargin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ew-circles {
            display: flex;
            justify-content: center;
            align-items: center;
            margin: 60px auto;
            height: 380px;
            position: relative;
        }

        .ew-circle1 {
            width: 180px;
            height: 180px;
            border-radius: 50%;
            background: rgb(165 195 221 / 50%);
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            color: #ffff00;
            font-weight: 600;
            font-size: 24px;
            text-align: center;
            box-shadow: 0 0px 3px rgb(91 145 191);
            transition: transform 0.3s;
            position: absolute;
            top: 45%;
            left: 25%;
        }

        .ew-circle2 {
            width: 180px;
            height: 180px;
            border-radius: 50%;
            background: rgba(143 183 218 / 80%);
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            color: #ffff00;
            font-weight: 600;
            font-size: 24px;
            text-align: center;
            transition: transform 0.3s;
            position: absolute;
            top: 23%;
            left: 37%;
        }

        .ew-circle3 {
            width: 180px;
            height: 180px;
            border-radius: 50%;
            background: rgb(58 152 231 / 60%);
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            color: #ffff00;
            font-weight: 600;
            font-size: 24px;
            text-align: center;
            transition: transform 0.3s;
            position: absolute;
            top: 0%;
            left: 49%;
        }

        .ew-circle:hover {
            transform: scale(1.1);
        }

        .ew-text {
            font-size: 14px;
            font-weight: 400;
            margin-top: 6px;
            color: #000;
            line-height: 20px;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-0">
        <div class="card">
            <div class="card-body">
                <div class="ew-circles">
                    <div class="ew-circle1">
                        <span>3%</span>
                        <div class="ew-text">
                            on sales of
                <br />
                            2Y EW Plan
                        </div>
                    </div>
                    <div class="ew-circle2">
                        <span>4%</span>
                        <div class="ew-text">
                            on sales of
                <br />
                            3Y EW Plan
                        </div>
                    </div>
                    <div class="ew-circle3">
                        <span>5%</span>
                        <div class="ew-text">
                            on sales of
                <br />
                            4Y EW Plan
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="card mt-3">
    <div class="card-body mt-3">
        <div>
            <asp:GridView ID="GVEWMargin" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead"
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
