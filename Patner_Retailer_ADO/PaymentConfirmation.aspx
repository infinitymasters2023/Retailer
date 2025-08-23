<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="PaymentConfirmation.aspx.cs" Inherits="Patner_Retailer_ADO.PaymentConfirmation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ValueClass {
            font-weight: 500;
            color: #000;
        }

        .dashboard-main-wrapper {
            min-height: 100%;
            padding-top: 30px;
            position: relative;
        }

        .TagClass {
            font-size: 13px;
        }

        .text-dark {
            color: #000000 !important;
            font-size: 1rem;
        }

        .page-main-heading {
            display: flex;
            align-items: center;
            margin-bottom: 10px;
        }

            .page-main-heading h2 {
                font-size: 18px;
                color: #000;
                margin-bottom: 0px;
            }

                .page-main-heading h2 svg {
                    vertical-align: middle;
                }

        .thead {
            background: #4397a7 !important;
            color: #fff !important;
        }

            .thead th {
                background: #4397a7 !important;
                color: #fff !important;
            }

        .dataTables_filter {
            display: none;
        }

        .dataTables_length {
            display: none;
        }

        .dataTables_info {
            display: none;
        }

        .dataTables_paginate {
            display: none;
        }

        .btnBackSalesPerson :hover {
            color: #71748d;
        }

        a.InfyVaultLink {
            color: #1947f9;
            text-decoration: underline;
        }

        .MainMessage, .TicketNoPanel, .greentingMessage {
            font-weight: 600;
        }
    </style>


    <%--    <script>
        setTimeout(function () {
            const rows1 = document.querySelectorAll('#ContentPlaceHolder1_GvCustomerDetails_wrapper .row');
            const rows2 = document.querySelectorAll('#ContentPlaceHolder1_GVProductDetails_wrapper .row');
            const rows3 = document.querySelectorAll('#ContentPlaceHolder1_GVPlanDetails_wrapper .row');
            const rows4 = document.querySelectorAll('#ContentPlaceHolder1_GVCommissionDetails_wrapper .row');
            if (rows1.length > 1) {
                rows1[1].classList.add('table-responsive');
            }
            if (rows2.length > 1) {
                rows2[1].classList.add('table-responsive');
            }
            if (rows3.length > 1) {
                rows3[1].classList.add('table-responsive');
            }
            if (rows4.length > 1) {
                rows4[1].classList.add('table-responsive');
            }
        }, 500);
    </script>--%>
    <script type="text/javascript">
        function pageLoad() {
            $('.multiselect').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                filterPlaceholder: 'Search',
                buttonWidth: '100%',
                nonSelectedText: 'Select',
                selectAllText: 'Select All',
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card mb-3">
        <div class="slider-container">
            <asp:Label ID="lblSucess1" runat="server" class="slide-pingpong"></asp:Label>
        </div>
        <div class="card-body mb-2">
            <div class="TicketInfoPanelCSS" id="TicketInfoPanel" runat="server" visible="false">
                <p class="firstLine">
                    <strong>Dear
                    <asp:Label ID="lblCustomerName" runat="server"></asp:Label>,</strong>
                </p>
                <p class="greentingMessage mb-2">Greetings from InfyShield!</p>
                <p class="mb-2">
                    Thank you for choosing to protect your
                    <asp:Label ID="lblProductName" runat="server" CssClass="MainMessage"></asp:Label>
                    with the
                    <asp:Label ID="lblPlanName" runat="server" CssClass="MainMessage"></asp:Label>
                </p>
                <p class="mb-2">We are pleased to inform you that we have successfully received your service plan registration request.</p>
                <p class="mb-2">Your plan is currently under review, and we will notify you once it has been verified and activated.</p>

                <hr />
                <p class="firstLine mb-2"><strong>Track Your Request</strong> </p>
                <div class="d-flex">
                    <p class="mb-2">You can track the status of your service plan using the <b>Ticket No:</b> &nbsp;</p>
                    <p id="ticketNo" runat="server" class="TicketNoPanel mb-2"></p>
                </div>
                <p class="mb-2">To track or manage your request, including uploading any supporting documents, please visit: <a href="https://infyvault.com/" target="_blank" class="InfyVaultLink">https://infyvault.com/ </a></p>
                <hr />
                <p class="text-danger mb-2"><strong>Important Reminder:</strong></p>
                <p>Please ensure that all required supporting documents (such as Invoice, Serial Number proof,) are uploaded to avoid any delays in verification and activation.</p>
                <%--<div style="font-family: Arial, sans-serif; font-size: 14px; color: #333; line-height: 1.6; max-width: 600px;">
                    <p>To ensure smooth verification and activation of your protection plan, please upload the following documents:</p>

                    <ul>
                        <li><strong>Purchase Invoice</strong>: Clearly showing the product details, date of purchase, and seller information.</li>
                        <li><strong>IMEI Number / Serial Number Proof</strong>:
                        <ul>
                            <li><strong>For mobiles/tablets:</strong> IMEI number (can be found in device settings or on the box).</li>
                            <li><strong>For appliances (e.g., AC, TV):</strong> Serial number (usually on a sticker on the device or packaging).</li>
                        </ul>
                        </li>
                    </ul>

                    <p style="color: #d9534f;"><strong>⚠️ Please ensure all documents are clear and legible. Incomplete or incorrect uploads may lead to delays in processing your plan.</strong></p>

                    <p>Thank you for choosing our protection services.</p>
                </div>--%>
            </div>
        </div>

    </div>

    <div class="card mb-3">
        <h5 class="card-header mt-0 text-dark">Customer Details</h5>
        <div class="card-body py-2">

            <div class="table-responsive">
                <asp:GridView ID="GvCustomerDetails" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead"
                    EmptyDataText="No records available. Please refine your search.">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No.">
                            <ItemTemplate>
                                <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />--%>
                        <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" />
                        <asp:BoundField DataField="WhatsappNo" HeaderText="Whatsapp No" />
                        <asp:BoundField DataField="EmailIDAddress" HeaderText="Email Id" />
                        <asp:BoundField DataField="Pincode" HeaderText="Pincode" />
                        <asp:BoundField DataField="City" HeaderText="City" />
                        <asp:BoundField DataField="State" HeaderText="State" />
                        <asp:BoundField DataField="AddressLine1" HeaderText="Address" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <div class="card mb-3">
        <h5 class="card-header mt-0 text-dark">Product Details</h5>
        <div class="card-body py-2">
            <div class="table-responsive">
                <asp:GridView ID="GVProductDetails" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead"
                    EmptyDataText="No records available. Please refine your search.">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No.">
                            <ItemTemplate>
                                <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Productname" HeaderText="Product Name" />
                        <asp:BoundField DataField="Productsubcategoryname" HeaderText="Category Name" />
                        <asp:BoundField DataField="Brand" HeaderText="Brand Name" />
                        <asp:BoundField DataField="ModalName" HeaderText="Modal" />
                        <asp:BoundField DataField="serialno" HeaderText="Serial No" />
                        <asp:BoundField DataField="DevicePurchasePrice" HeaderText="Device Price" />
                        <asp:BoundField DataField="DateofImplementation" HeaderText="Product Installation Date" />
                        <asp:BoundField DataField="ManufacturerWarranty_yymmdd" HeaderText="Manufacturer Warranty" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <div class="card mb-3">
        <h5 class="card-header mt-0 text-dark">Plan Details</h5>
        <div class="card-body py-2">
            <div class="table-responsive">
                <asp:GridView ID="GVPlanDetails" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead"
                    EmptyDataText="No records available. Please refine your search.">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No.">
                            <ItemTemplate>
                                <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PlanNicknameSelection" HeaderText="Plan Name" />
                        <asp:BoundField DataField="PlanSKU" HeaderText="Plan SKU" />
                        <asp:BoundField DataField="MRP" HeaderText="MRP" DataFormatString="Rs.{0:N2}" HtmlEncode="false" />
                        <asp:BoundField DataField="PlanPrice" HeaderText="Offer Price" DataFormatString="Rs. {0:N2}" HtmlEncode="false" />
                        <asp:BoundField DataField="TaxableValue" HeaderText="Taxable Value" DataFormatString="Rs. {0:N2}" HtmlEncode="false" />
                        <asp:BoundField DataField="TaxAmout" HeaderText="Tax Amout(18%)" DataFormatString="Rs. {0:N2}" HtmlEncode="false" />
                        <asp:BoundField DataField="Commission" HeaderText="Retailer Commision" DataFormatString="Rs. {0:N2}" HtmlEncode="false" />
                        <asp:BoundField DataField="CommissionTaxableValue" HeaderText="Retailer Commision Taxable Value" DataFormatString="Rs. {0:N2}" HtmlEncode="false" />
                        <asp:BoundField DataField="CommissionTaxValue" HeaderText="Retailer Commision Tax(18%)" DataFormatString="Rs. {0:N2}" HtmlEncode="false" />
                        <asp:BoundField DataField="TotalAmountPay" HeaderText="Total Amount Pay" DataFormatString="Rs. {0:N2}" HtmlEncode="false" />
                        <asp:BoundField DataField="TranStatus" HeaderText="Transaction Status" />
                        <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="false" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <%--    <div class="container-fluid  dashboard-content">
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
                                    <table class="table-responsive table data-table table-striped table-bordered nowrap"  EmptyDataText="No records available.">
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
                                    <table class="table-responsive table data-table table-striped table-bordered nowrap"  EmptyDataText="No records available.">
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
                                    <table class="table-responsive table data-table table-striped table-bordered nowrap"  EmptyDataText="No records available.">
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
                                        <td><%# Eval("PlanPrice", "{0:N2}") %></td>
                                        <td><%# Eval("TaxableValue", "{0:N2}") %></td>
                                        <td><%# Eval("TaxAmout", "{0:N2}") %></td>
                                        <td><%# Eval("TotalAmountPay", "{0:N2}") %></td>
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
    </div>--%>
</asp:Content>
