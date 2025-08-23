<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RegistrationDetails.aspx.cs" Inherits="Patner_Retailer_ADO.RegistrationDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ValueClass {
            font-weight: 500;
            color: #000;
        }

        .TagClass {
            font-size: 13px;
        }

        .text-dark {
            color: #000000 !important;
            font-size: 1rem;
        }

        .flexContent {
            display: flex;
            gap: 5px;
            /* flex-direction: row; */
            flex-wrap: wrap;
            flex: 0 0 25%;
            padding: 0px 10px;
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
        .btnBackSalesPerson :hover{
            color:#71748d;
        }
    </style>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

    <script>
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
    </script>
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
    <div class="page-main-heading">
        <h2>
            <a href="SalesReports.aspx" class="btnBackSalesPerson" name="BackButton">
                <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" viewBox="0 0 18 18">
                    <path fill-rule="evenodd" d="M5.854 3.646a.5.5 0 0 1 0 .708L2.707 7.5H14.5a.5.5 0 0 1 0 1H2.707l3.147 3.146a.5.5 0 0 1-.708.708l-4-4a.5.5 0 0 1 0-.708l4-4a.5.5 0 0 1 .708 0z" />
                </svg>
            </a>
            View Details</h2>
    </div>
    <div class="row">
        <div class="col-md-8">
            <div class="card mb-3">
                <h5 class="card-header mt-0 text-dark">Customer Details</h5>
                <div class="card-body py-2 pr-0">
                    <%--  <div class="row">

                <div class="flexContent">
                    <asp:Label ID="lblName" CssClass="TagClass" runat="server" Text="Name : " />
                    <asp:Label ID="lblNameValue" CssClass="ValueClass" runat="server" />
                </div>


                <div class="flexContent">
                    <asp:Label ID="lblMobileNo" CssClass="TagClass" runat="server" Text="Mobile No : " />
                    <asp:Label ID="lblMobileNoValue" CssClass="ValueClass" runat="server" />
                </div>


                <div class="flexContent">
                    <asp:Label ID="lblWhatsappNo" runat="server" CssClass="TagClass" Text="WhatsApp No : " />
                    <asp:Label ID="lblWhatsappNoValue" CssClass="ValueClass" runat="server" />
                </div>


                <div class="flexContent">
                    <asp:Label ID="lblEmail" runat="server" CssClass="TagClass" Text="Email : " />
                    <asp:Label ID="lblEmailValue" CssClass="ValueClass" runat="server" />
                </div>


                <div class="flexContent">
                    <asp:Label ID="lblPincode" runat="server" CssClass="TagClass" Text="Pin Code : " />
                    <asp:Label ID="lblPincodeValue" CssClass="ValueClass" runat="server" />
                </div>


                <div class="flexContent">
                    <asp:Label ID="lblCity" runat="server" CssClass="TagClass" Text="City : " />
                    <asp:Label ID="lblCityValue" CssClass="ValueClass" runat="server" />
                </div>


                <div class="flexContent">
                    <asp:Label ID="lblState" runat="server" CssClass="TagClass" Text="State : " />
                    <asp:Label ID="lblStateValue" CssClass="ValueClass" runat="server" />
                </div>

                <div class="flexContent">
                    <asp:Label ID="lblAddress" runat="server" CssClass="TagClass" Text="Address : " />
                    <asp:Label ID="lblAddressValue" CssClass="ValueClass" runat="server" />
                </div>

            </div>--%>
                    <asp:GridView ID="GvCustomerDetails" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead"
                        EmptyDataText="No records available. Please refine your search.">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
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
            <div class="card mb-3">
                <h5 class="card-header mt-0 text-dark">Product Details</h5>
                <div class="card-body py-2 pr-0">
                    <asp:GridView ID="GVProductDetails" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead"
                        EmptyDataText="No records available. Please refine your search.">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PlanNicknameSelection" HeaderText="Plan" />
                            <asp:BoundField DataField="Productsubcategoryname" HeaderText="Category" />
                            <asp:BoundField DataField="Productname" HeaderText="Product" />
                            <asp:BoundField DataField="Brand" HeaderText="Brand" />
                            <asp:BoundField DataField="ModalName" HeaderText="Modal" />
                            <asp:BoundField DataField="imei" HeaderText="IMEI No 1" />
                            <asp:BoundField DataField="imei2" HeaderText="IMEI No 2" />
                            <asp:BoundField DataField="DevicePurchasePrice" HeaderText="Device Price" />
                            <asp:BoundField DataField="ProductPurchaseDate" HeaderText="Product Purchase Date" />
                            <asp:BoundField DataField="ProdStatus" HeaderText="Status" />
                        </Columns>
                    </asp:GridView>
                    <%--     <div class="row">
                <div class="flexContent">
                    <asp:Label ID="lblCategory" runat="server" CssClass="TagClass" Text="Category Name: " />
                    <asp:Label ID="lblCategoryValue" CssClass="ValueClass" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="lblProduct" runat="server" CssClass="TagClass" Text="Product Name: " />
                    <asp:Label ID="lblProductValue" CssClass="ValueClass" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="lblBrand" runat="server" CssClass="TagClass" Text="Brand Name: " />
                    <asp:Label ID="lblBrandValue" CssClass="ValueClass" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="lblModel" runat="server" CssClass="TagClass" Text="Modal: " />
                    <asp:Label ID="lblModelValue" CssClass="ValueClass" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="lblIMEI1" runat="server" CssClass="TagClass" Text="IMEI No 1: " />
                    <asp:Label ID="lblIMEI1Value" CssClass="ValueClass" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="lblIMEI2" runat="server" CssClass="TagClass" Text="IMEI No 2: " />
                    <asp:Label ID="lblIMEI2Value" CssClass="ValueClass" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="lblDevicePrice" runat="server" CssClass="TagClass" Text="Device Price: " />
                    <asp:Label ID="lblDevicePriceValue" CssClass="ValueClass" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="lblDeviceDate" runat="server" CssClass="TagClass" Text="Product Purchase Date: " />
                    <asp:Label ID="lblDeviceDateValue" CssClass="ValueClass" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="lblProductStatus" runat="server" CssClass="TagClass" Text="Status: " />
                    <asp:Label ID="lblProductStatusValue" CssClass="ValueClass" runat="server" />
                </div>
            </div>--%>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card bg-light mb-0" style="position: sticky; top: 80px;">
                <div class="card-header"><strong>Your Order Summary</strong></div>
                <div class="card-body bg-white order-calcs">
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        Plan Name : 
             <strong>
                 <p id="txtPlanNicknameSelection" runat="server"></p>
             </strong>
                    </div>
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        MRP :
             <strong>
                 <p id="MRP" runat="server"></p>
             </strong>
                    </div>
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        Offer Price :
             <strong>
                 <p id="PlanPrice" runat="server"></p>
             </strong>
                    </div>
                    <div class="d-flex justify-content-between align-items-center mb-2 pl-2">
                        Taxable Value :
             <strong>
                 <p id="TaxableValue" runat="server"></p>
             </strong>
                    </div>
                    <div class="d-flex justify-content-between align-items-center mb-2 pl-2">
                        Tax Amout(18%) :
             <strong>
                 <p id="TaxAmout" runat="server"></p>
             </strong>
                    </div>
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        Cess(0%) :
             <strong>
                 <p id="P1" runat="server">0</p>
             </strong>
                    </div>
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        Convenience Fee(0%) :
             <strong>
                 <p id="P2" runat="server">0</p>
             </strong>
                    </div>
                    <hr />
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <span style="color: green; font-weight: bold;">Retailer Commission Details</span>
                        <button class="btn btn-sm btn-link text-primary" type="button" data-bs-toggle="collapse" data-bs-target="#commissionSection" aria-expanded="false" aria-controls="commissionSection">
                            <i class="fa fa-angle-down"></i>
                        </button>
                    </div>
                    <div class="collapse" id="commissionSection">
                        <div class="d-flex justify-content-between align-items-center mb-2" style="color: green;">
                            <p class="mb-0" id="Commossiontag" runat="server">Retailer Commision :</p>
                            <strong>
                                <p id="Commission" runat="server"></p>
                            </strong>
                        </div>
                        <div class="d-flex justify-content-between align-items-center mb-2 pl-2" style="color: green;">
                            <p class="mb-0" id="P3" runat="server">Taxable Value :</p>
                            <strong>
                                <p id="CommissionTaxableValue" runat="server"></p>
                            </strong>
                        </div>
                        <div class="d-flex justify-content-between align-items-center mb-2 pl-2" style="color: green;">
                            <p class="mb-0" id="P5" runat="server">GST(18%) <span style="color: red">*</span> :</p>
                            <strong>
                                <p id="CommissionTaxValue" runat="server"></p>
                            </strong>
                        </div>
                    </div>
                    <hr>
                    <div class="d-flex justify-content-between align-items-center">
                        Total Net Value (Including Tax):
             <strong>
                 <p id="TotalAmountPay" runat="server"></p>
             </strong>
                    </div>
                </div>
                <p class="py-2 px-3" style="font-size: 12px">
                    <span style="color: red">*</span>
                    Retailer GST (18%) will be payable post Tax Invoice issued from Retailer
                </p>
            </div>
        </div>
    </div>
<%--    <div class="card mb-3">
        <h5 class="card-header mt-0 text-dark">Plan Details</h5>
        <div class="card-body py-2">
            <asp:GridView ID="GVPlanDetails" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead"
                EmptyDataText="No records available. Please refine your search.">
                <Columns>
                    <asp:TemplateField HeaderText="S.No.">
                        <ItemTemplate>
                            <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PlanNicknameSelection" HeaderText="Plan Name" />
                    <asp:BoundField DataField="FinalPlanNameDescription" HeaderText="Plan Description" />
                    <asp:BoundField DataField="MRP" HeaderText="MRP" DataFormatString="Rs. {0:N2}" HtmlEncode="false" />
                    <asp:BoundField DataField="TaxableValue" HeaderText="Offer Price" DataFormatString="Rs. {0:N2}" HtmlEncode="false" />
                    <asp:BoundField DataField="Discount" HeaderText="Discount" />
                    <asp:BoundField DataField="planStatus" HeaderText="Status" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="card mb-3">
        <h5 class="card-header mt-0 text-dark">Commission Details</h5>
        <div class="card-body py-2">
            <asp:GridView ID="GVCommissionDetails" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead"
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
                </Columns>
            </asp:GridView>
        </div>
    </div>--%>
</asp:Content>
