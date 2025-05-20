<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RetailerPaymentConfirmation.aspx.cs" Inherits="Patner_Retailer_ADO.RetailerPaymentConfirmation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Retailer Payment Confirmation</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.4/css/dataTables.bootstrap4.min.css">
    <link rel="stylesheet" href="https://cdn.datatables.net/responsive/2.4.1/css/responsive.bootstrap4.min.css">
    <link rel="stylesheet" href="../assets/vendor/fonts/fontawesome/css/fontawesome-all.css">
    <link rel="stylesheet" href="../assets/vendor/bootstrap/css/bootstrap.min.css">
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
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid  dashboard-content">
            <div class="row">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <div class="card">
                        <div class="slider-container">
                            <asp:Label ID="lblSucess1" runat="server" class="slide-pingpong"></asp:Label>
                        </div>
                        <div class="card-body" style="display: flex;">
                            <div class="col-md-12">
                                <h4>Customer Details</h4>
                                <asp:Repeater ID="rptPlans" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-bordered">
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
                                <h4>Product Information</h4>
                                <asp:Repeater ID="rptProductInfo" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-bordered">
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
                                <h4>Plan / Payment Information</h4>
                                <asp:Repeater ID="rptPlanInfo" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-bordered">
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
    </form>
</body>
</html>