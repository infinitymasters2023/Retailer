<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Incentive.aspx.cs" Inherits="Patner_Retailer_ADO.Incentive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .incentive-chart {
            display: flex;
            justify-content: center;
            align-items: flex-end;
            gap: 15px;
            margin-bottom: 30px;
            width: 100%;
            margin-top: 60px;
        }

        .incentive-box {
            position: relative;
            padding: 20px 25px;
            border-radius: 8px;
            text-align: left;
            min-width: 250px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            height: 180px;
        }

            .incentive-box.blueBox {
                transform: translateY(0);
                border-left: 15px solid #3a7bd5;
                border-top: 15px solid #3a7bd5;
            }

            .incentive-box.greenBox {
                transform: translateY(-20px);
                border-left: 15px solid #3bd9b3;
                border-top: 15px solid #3bd9b3;
            }

            .incentive-box.darkgreenBox {
                transform: translateY(-40px);
                border-left: 15px solid #336600;
                border-top: 15px solid #336600;
            }

            .incentive-box::before {
                content: '';
                position: absolute;
                top: -15px;
                right: 15px;
                width: 0;
                height: 0;
                border-left: 15px solid transparent;
                border-right: 15px solid transparent;
                border-bottom: 20px solid rgba(255, 255, 255, 0.3);
            }

            .incentive-box.blue::before {
                border-bottom-color: #4a90e2;
            }

            .incentive-box.green::before {
                border-bottom-color: #50e3c2;
            }

            .incentive-box.dark-green::before {
                border-bottom-color: #417505;
            }

        .incentive-amount {
            font-size: 2.2em;
            font-weight: bold;
            margin-bottom: 10px;
            letter-spacing: -0.5px;
        }

        .incentive-text {
            font-size: 0.9em;
            line-height: 1.4;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-0">
        <div class="card">
            <div class="card-body">
                <div class="incentive-chart">
                    <div class="incentive-box blueBox">
                        <div class="incentive-amount">Rs.5,999/-</div>
                        <div class="incentive-text">
                            Worth Gift or<br />
                            En-cash for Rs.4,999/-<br />
                            on Sales Value of Rs.1.5 Lakhs+ in a Calendar Month*
   
                        </div>
                    </div>

                    <div class="incentive-box greenBox">
                        <div class="incentive-amount">Rs.10,999/-</div>
                        <div class="incentive-text">
                            Worth Gift or<br />
                            En-cash for Rs.9,999/-<br />
                            on Sales Value of Rs.2.00+ Lakhs in a Calendar Month*
   
                        </div>
                    </div>

                    <div class="incentive-box darkgreenBox">
                        <div class="incentive-amount">Rs.15,999/-</div>
                        <div class="incentive-text">
                            Worth Gift or<br />
                            En-cash for Rs.14,999/-<br />
                            on Sales Value of Rs.2.50+ Lakhs in a Calendar Month*
   
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="card mt-3">
    <div class="card-body mt-3">
        <div>
            <asp:GridView ID="GVIncentive" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead"
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
