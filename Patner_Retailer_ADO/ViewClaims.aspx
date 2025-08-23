<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewClaims.aspx.cs" Inherits="Patner_Retailer_ADO.ViewClaims" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
            flex: 0 0 32%;
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
        .UploadDocumentCls{
            text-align: center;
    margin-top: 10px;
    justify-content: space-around;
        }
    </style>


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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="page-main-heading">
        <h2> 
           <a id="anchorBackButton" runat="server" class="btnBackSalesPerson" name="BackButton">
                <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" viewBox="0 0 18 18">
                    <path fill-rule="evenodd" d="M5.854 3.646a.5.5 0 0 1 0 .708L2.707 7.5H14.5a.5.5 0 0 1 0 1H2.707l3.147 3.146a.5.5 0 0 1-.708.708l-4-4a.5.5 0 0 1 0-.708l4-4a.5.5 0 0 1 .708 0z" />
                </svg>
            </a>
            View Details</h2>
    </div>
    <div class="card mb-3">
        <h5 class="card-header mt-0 text-dark">Registered Owner Details</h5>
        <div class="card-body py-2">
            <div class="row">



                <div class="flexContent">
                    <asp:Label ID="Label1" CssClass="ValueClass" runat="server" Text="Ticket No. : " />
                    <asp:Label ID="lblticketNo" CssClass="" runat="server" />
                </div>


                <div class="flexContent">
                    <asp:Label ID="Label3" CssClass="ValueClass" runat="server" Text="Certificate of InfyShield No.: " />
                    <asp:Label ID="lblcoi" CssClass="" runat="server" />
                </div>


                <div class="flexContent">
                    <asp:Label ID="Label5" runat="server" CssClass="ValueClass" Text="Product Status : " />
                    <asp:Label ID="lblstatus" CssClass="" runat="server" />
                </div>

                <div class="flexContent">
                    <asp:Label ID="lblName" CssClass="ValueClass" runat="server" Text="Name : " />
                    <asp:Label ID="lblNameValue" CssClass="" runat="server" />
                </div>


                <div class="flexContent">
                    <asp:Label ID="lblMobileNo" CssClass="ValueClass" runat="server" Text="Mobile No : " />
                    <asp:Label ID="lblMobileNoValue" CssClass="" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="lblAltModileNo" CssClass="ValueClass" runat="server" Text="Alter Mobile No : " />
                    <asp:Label ID="lblAltModileNoValue" CssClass="" runat="server" />
                </div>

                <div class="flexContent">
                    <asp:Label ID="lblWhatsappNo" runat="server" CssClass="ValueClass" Text="WhatsApp No : " />
                    <asp:Label ID="lblWhatsappNoValue" CssClass="" runat="server" />
                </div>


                <div class="flexContent">
                    <asp:Label ID="lblEmail" runat="server" CssClass="ValueClass" Text="Email : " />
                    <asp:Label ID="lblEmailValue" CssClass="" runat="server" />
                </div>


                <div class="flexContent">
                    <asp:Label ID="lblPincode" runat="server" CssClass="ValueClass" Text="Pin Code : " />
                    <asp:Label ID="lblPincodeValue" CssClass="" runat="server" />
                </div>


                <div class="flexContent">
                    <asp:Label ID="lblCity" runat="server" CssClass="ValueClass" Text="City : " />
                    <asp:Label ID="lblCityValue" CssClass="" runat="server" />
                </div>


                <div class="flexContent">
                    <asp:Label ID="lblState" runat="server" CssClass="ValueClass" Text="State : " />
                    <asp:Label ID="lblStateValue" CssClass="" runat="server" />
                </div>

                <div class="flexContent">
                    <asp:Label ID="lblAddress" runat="server" CssClass="ValueClass" Text="Address Line 1 : " />
                    <asp:Label ID="lblAddressValue" CssClass="" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="Label11" runat="server" CssClass="ValueClass" Text="Address Line 2 : " />
                    <asp:Label ID="lblAddressValue2" CssClass="" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="Label13" runat="server" CssClass="ValueClass" Text="LandMark : " />
                    <asp:Label ID="lblAddressValue3" CssClass="" runat="server" />
                </div>
            </div>

        </div>

    </div>
    <div class="card mb-3">
        <h5 class="card-header mt-0 text-dark">Product Details / Service Plan</h5>
        <div class="card-body py-2">

            <div class="row">
                <div class="flexContent">
                    <asp:Label ID="lblCategory" runat="server" CssClass="ValueClass" Text="Category Name: " />
                    <asp:Label ID="lblCategoryValue" CssClass="" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="lblProduct" runat="server" CssClass="ValueClass" Text="Product Name: " />
                    <asp:Label ID="lblProductValue" CssClass="" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="lblBrand" runat="server" CssClass="ValueClass" Text="Brand Name: " />
                    <asp:Label ID="lblBrandValue" CssClass="" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="lblModel" runat="server" CssClass="ValueClass" Text="Modal: " />
                    <asp:Label ID="lblModelValue" CssClass="" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="lblIMEI1" runat="server" CssClass="ValueClass" Text="IMEI No. 1: " />
                    <asp:Label ID="lblIMEI1Value" CssClass="" runat="server" />
                </div>
                <div class="flexContent">
                    <asp:Label ID="lblIMEI2" runat="server" CssClass="ValueClass" Text="IMEI No. 2: " />
                    <asp:Label ID="lblIMEI2Value" CssClass="" runat="server" />
                </div>
            <div class="flexContent">
                <asp:Label ID="Label9" runat="server" CssClass="ValueClass" Text="Serial No. : " />
                <asp:Label ID="lblserialNo" CssClass="" runat="server" />
            </div>
            <div class="flexContent">
                <asp:Label ID="lblDevicePrice" runat="server" CssClass="ValueClass" Text="Device Price: " />
                <asp:Label ID="lblDevicePriceValue" CssClass="" runat="server" />
            </div>
            <div class="flexContent">
                <asp:Label ID="lblDeviceDate" runat="server" CssClass="ValueClass" Text="Product Purchase Date: " />
                <asp:Label ID="lblDeviceDateValue" CssClass="" runat="server" />
            </div>
            <div class="flexContent">
                <asp:Label ID="lblProductStatus" runat="server" CssClass="ValueClass" Text="Status: " />
                <asp:Label ID="lblProductStatusValue" CssClass="" runat="server" />
            </div>

            <div class="flexContent">
                <asp:Label ID="lblPlan" runat="server" CssClass="ValueClass" Text="Plan Name: " />
                <asp:Label ID="lblPlanValue" CssClass="" runat="server" />
            </div>
            <div class="flexContent" id="divdes" runat="server" visible="false">
                <asp:Label ID="lblDescription" runat="server" CssClass="ValueClass" Text="Plan Description: " />
                <asp:Label ID="lblDescriptionValue" CssClass="" runat="server" />
            </div>
            <div class="flexContent">
                <asp:Label ID="lblSKU" runat="server" CssClass="ValueClass" Text="Plan SKU: " />
                <asp:Label ID="lblSKUValue" CssClass="" runat="server" />
            </div>
            <div class="flexContent">
                <asp:Label ID="lblOffer" runat="server" CssClass="ValueClass" Text="Offer Price: " />
                <asp:Label ID="lblOfferValue" CssClass="" runat="server" />
            </div>
            <div class="flexContent">
                <asp:Label ID="lblDiscount" runat="server" CssClass="ValueClass" Text="Discount: " />
                <asp:Label ID="lblDiscountValue" CssClass="" runat="server" />
            </div>
            <div class="flexContent">
                <asp:Label ID="lblMRP" runat="server" CssClass="ValueClass" Text="MRP: " />
                <asp:Label ID="lblMRPValue" CssClass="" runat="server" />
            </div>
            <div class="flexContent" style="flex: auto;">
                <asp:Label ID="lblPlanPeriod" runat="server" CssClass="ValueClass" Text="Plan Period: " />
                <asp:Label ID="lblPlanPeriodValue" CssClass="" runat="server" />
            </div>
            <div class="flexContent" runat="server" visible="false">
                <asp:Label ID="lblPlanStatus" runat="server" CssClass="ValueClass" Text="Status: " />
                <asp:Label ID="lblPlanStatusValue" CssClass="" runat="server" />
            </div>

        </div>
    </div>
    </div>

    <div class="card mb-3">
        <h5 class="card-header mt-0 text-dark">Supporting Documents</h5>
        <div class="card-body py-2">
            <div class="row">
                <div id="UploadDocumentPanel" runat="server" >
                <div class=" col-md-12">
                    <p class="text-left">
                        Please submit supporting documents by selecting Correct Document Title.
                            <br />
                        Please submit each document file separately. Files containing multiple images may
                            be rejected.
                            <br />
                        Before submitting, please check that the image quality is good, readable and relevant
                            to the claim. This will help us to serve you better.
                    </p>
                </div>
                <div class="row UploadDocumentCls">
                    <div class=" col-md-3">
                        <label>
                            <asp:DropDownList ID="ddldocumentattached2" runat="server" CssClass="form-control ">
                            </asp:DropDownList>
                        </label>
                    </div>
                    <div class=" col-md-3">
                        <asp:FileUpload runat="server" name="ImageUpload" accept=".jpg,.jpeg,.png,.pdf"
                            ID="fupupload2" onchange="ShowPreview(this)" />
                    </div>
                    <div class=" col-md-2">
                        <asp:Button ID="btnfuupload" OnClick="UploadImage1" runat="server" CssClass="btn next-step mt-0"
                            Text="Submit Document" />
                    </div>
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-success"></asp:Label>
                </div>
                <div class="col-md-4 hidden" visible="false" runat="server">
                    <span>Image Preview</span><br />
                    <asp:Image ID="impPrev" runat="server" Width="300px" Height="330px" ImageUrl="../Document/not_available.jpg" />
                </div>
                </div>
                <div class="col-md-12">
                    <asp:GridView ID="GVSupportingDoc" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead" EmptyDataText="No records available. Please refine your search."
                         OnRowDataBound="GVSupportingDoc_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MID" Visible="false">
                                <ItemTemplate>
                                    <asp:HiddenField ID="lblMid" runat="server" Value='<%# Eval("Mid") %>'></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="View Document">
                                <ItemTemplate>
                                    <a href='<%# Eval("FullDocumentPath") %>' target="_blank" title="View Document">
                                        <i class="fa fa-eye" aria-hidden="true"></i>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Document Name">
                                <ItemTemplate>
                                    <%# Eval("DocumentName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Uploaded Date">
                                <ItemTemplate>
                                    <%# Eval("UploadedDate") %>'
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                       <asp:Label ID="lblDocStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
