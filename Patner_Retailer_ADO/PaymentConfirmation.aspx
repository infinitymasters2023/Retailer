<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="PaymentConfirmation.aspx.cs" Inherits="Patner_Retailer_ADO.PaymentConfirmation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
      .slider-container {
        width: 100%;
        overflow: hidden;
        position: relative;
        height: 40px;
        background-color: #f0f0f0;
    }

    .slide-pingpong {
        position: absolute;
        white-space: nowrap;
        font-weight: bold;
        color: green;
        font-size: 18px;
        top: 50%;
        transform: translateY(-50%);
        animation: pingpongScroll 10s linear infinite alternate;
    }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid  dashboard-content">
        <div class="row">
            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                <div class="card">
                    <div class="slider-container">
                        <asp:Label id="lblSucess1" runat="server" class="slide-pingpong"></asp:Label>
                    </div>
                    <div class="card-body" style="display: flex;">
                        <div class="col-md-12">
                            <h4>Customer Details</h4>
                            <asp:Repeater ID="rptPlans" runat="server">
                                <HeaderTemplate>
                                    <table class="table-responsive table data-table table-striped table-bordered nowrap">
                                        <thead>
                                            <tr>
                                                <th>Customer Name</th>
                                                <th>Address Line 1</th>
                                                <th>City</th>
                                                <th>State</th>
                                                <th>Pin Code</th>
                                                <th>Mobile No</th>
                                                <th>Email ID Address</th>
                                                <th>Landmark</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("CustomerName") %></td>
                                        <td><%# Eval("AddressLine1") %></td>
                                        <td><%# Eval("City") %></td>
                                        <td><%# Eval("State") %></td>
                                        <td><%# Eval("Pincode") %></td>
                                        <td><%# Eval("MobileNo") %></td>
                                        <td><%# Eval("EmailIDAddress") %></td>
                                        <td><%# Eval("Landmark") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>

                            <hr />
                            <!-- Product Information -->
                            <h4>Product Information</h4>
                            <asp:Repeater ID="rptProductInfo" runat="server">
                                <HeaderTemplate>
                                    <table class="table-responsive table data-table table-striped table-bordered nowrap">
                                        <thead>
                                            <tr>
                                                <th>Product Name</th>
                                                <th>Subcategory</th>
                                                <th>Brand</th>
                                                <th>Model</th>
                                                <th>Serial No</th>
                                                <th>Device Price</th>
                                                <th>Date of Implementation</th>
                                                <th>Manufacturer Warranty</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Productname") %></td>
                                        <td><%# Eval("Productsubcategoryname") %></td>
                                        <td><%# Eval("Brand") %></td>
                                        <td><%# Eval("ModalName") %></td>
                                        <td><%# Eval("serialno") %></td>
                                        <td><%# Eval("DevicePurchasePrice") %></td>
                                        <td><%# Eval("DateofImplementation", "{0:dd-MM-yyyy}") %></td>
                                        <td><%# Eval("ManufacturerWarranty_yymmdd") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>

                            <hr />
                            <!-- Plan Information -->
                            <h4>Plan / Payment Information</h4>
                            <asp:Repeater ID="rptPlanInfo" runat="server">
                                <HeaderTemplate>
                                    <table class="table-responsive table data-table table-striped table-bordered nowrap">
                                        <thead>
                                            <tr>
                                                <th>Plan Name</th>
                                                <th>Plan Price</th>
                                                <th>Taxable Value</th>
                                                <th>Tax Amount</th>
                                                <th>Total Paid</th>
                                                <th>Transaction Status</th>
                                                <th>Payment Date</th>
                                                <th>Order ID</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("PlanName") %></td>
                                        <td><%# Eval("PlanPrice") %></td>
                                        <td><%# Eval("TaxableValue") %></td>
                                        <td><%# Eval("TaxAmout") %></td>
                                        <td><%# Eval("TotalAmountPay") %></td>
                                        <td><%# Eval("TranStatus") %></td>
                                        <td><%# Eval("PaymentDate", "{0:dd-MM-yyyy}") %></td>
                                        <td><%# Eval("OrderId") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
