<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="Patner_Retailer_ADO.Reports" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .custom-select {
            line-height: 1 !important;
        }

        .logo-parts {
            background: #dff0d8;
            padding: 5px 15px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        img.infinity-logo {
            max-width: 90px;
        }

        .infyshield-logo {
            max-width: 42px;
        }

        .plan-card {
            background: #ffffff;
            border: 1px solid #e2e2e2;
            padding: 0px;
            border-radius: 10px;
            min-height: 215px;
            box-shadow: 0px 0px 5px #d1d1d1;
            position: relative;
            transition: all 0.3s ease;
        }

            .plan-card h2 {
                font-size: 14px;
                font-weight: 500;
                padding: 5px 15px 0px 15px;
                text-align: center;
                color: #228b22;
                margin: 0;
            }

            .plan-card h3 {
                font-size: 13px;
                font-weight: 400;
                margin-bottom: 0;
                display: flex;
                align-items: center;
                width: 100%;
                padding: 0px 1rem 5px 1rem;
                color: #228b22;
                letter-spacing: .3px;
            }

        .plan-name-txt {
            font-size: 11px;
            letter-spacing: .5px;
            font-weight: 400;
        }
        /*
          .plan-name-txt strong {
              font-weight: 600;
          }*/

        .multiselect {
            overflow: hidden;
            text-overflow: ellipsis;
            text-align: left !important;
            height: 34px;
        }

        .card-btn {
            display: flex;
            align-items: center;
            gap: 6px;
        }

            .card-btn svg {
                font-size: 18px;
            }

        .eye-icon {
            font-size: 11px;
            color: #000000;
            background: #f7f7f7;
            border: 1px solid #dfdfdf;
            border-radius: 5px;
            padding: 5px;
            position: absolute;
            bottom: 20px;
            right: 20px;
        }

            .eye-icon a {
                color: #000;
            }

                .eye-icon a:hover {
                    text-decoration: none;
                    color: #000;
                }


        .plan-name-txt strong {
            font-weight: 600;
        }

        .thead {
            background: #4397a7 !important;
            color: #fff !important;
        }

            .thead th {
                background: #4397a7 !important;
                color: #fff !important;
            }

        #ContentPlaceHolder1_GvReport_filter {
            display: none;
        }

        #ContentPlaceHolder1_GvReport_length {
            display: none;
        }

        .multiselect-container .multiselect-filter > .fa-search {
            font-size: 12px !important;
        }
    </style>
    <script>
        setTimeout(function () {
            const rows = document.querySelectorAll('#ContentPlaceHolder1_GvReport_wrapper .row');
            if (rows.length > 1) {
                rows[1].classList.add('table-responsive');
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
    <div class="card mb-3">
        <div class="card-body">
            <h5 class="card-header mb-3" style="display: flex; justify-content: space-between; padding-top: 0px !important; padding-left: 0px !important;">Reports</h5>
            <div class="d-flex justify-content-start mb-3">

                <div class="row">
                    <div class="col-12 col-lg-2 pr-0 mb-2 mb-lg-0">
                        <label>From Date</label>
                        <asp:TextBox ID="txtfromDate" runat="server" CssClass="form-control" AutoCompleteType="Disabled" AutoComplete="off"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtfromDate"></cc1:CalendarExtender>
                    </div>
                    <div class="col-12 col-lg-2 pr-0 mb-2 mb-lg-0">
                        <label>To Date</label>
                        <asp:TextBox ID="txttodate" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy" TargetControlID="txttodate"></cc1:CalendarExtender>
                    </div>
                    <div class="col-12 col-lg-2 pr-0 mb-2 mb-lg-0">
                        <label>Brand</label>
                        <asp:ListBox ID="ddlBrand" CssClass="form-control multiselect" SelectionMode="Multiple" runat="server"></asp:ListBox>
                    </div>
                    <div class="col-12 col-lg-2 pr-0 mb-2 mb-lg-0">
                        <label>Model</label>
                        <asp:TextBox ID="txtModel" runat="server" CssClass="form-control" placeholder="Model Name"></asp:TextBox>
                    </div>
                    <div class="col-12 col-lg-2 pr-0 mb-2 mb-lg-0">
                        <label>Customer Name</label>
                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" placeholder="Customer Name"></asp:TextBox>
                    </div>
                    <div class="col-12 col-lg-2 pr-0 mb-2 mb-lg-0">
                        <label>Customer Mobile No</label>
                        <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control" placeholder="Mobile No"></asp:TextBox>
                    </div>
                    <div class="col-12 col-lg-2 pr-0 mb-2 mb-lg-0">
                        <label>Product Type</label>
                        <asp:ListBox ID="ddlproductType" CssClass="form-control multiselect" SelectionMode="Multiple" runat="server"></asp:ListBox>
                    </div>
                    <div class="col-12 col-lg-2 pr-0 mb-3 mb-lg-0">
                        <label>Plan Name</label>
                        <asp:ListBox ID="ddlplan" CssClass="form-control multiselect" SelectionMode="Multiple" runat="server"></asp:ListBox>
                    </div>

                    <div class="col-12 col-lg-2 pt-4">
                        <div class="d-flex justify-content-between gap-2">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn next-step mt-0" OnClick="SubmitReport" />
                            <asp:Button ID="btnExportExcel" runat="server" Text="Export to Excel" CssClass="btn btn-info ml-2" OnClick="btnExportExcel_Click" />
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div class="card">
        <div class="card-body">
            <div>
                <div class="mb-3 d-flex justify-content-end gap-2">
                    <button id="btnCardView" style="border-radius: 7px 0 0 7px;" runat="server" onserverclick="btnCardView_Click">
                        <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 576 512" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                            <path d="M512 80c8.8 0 16 7.2 16 16l0 32L48 128l0-32c0-8.8 7.2-16 16-16l448 0zm16 144l0 192c0 8.8-7.2 16-16 16L64 432c-8.8 0-16-7.2-16-16l0-192 480 0zM64 32C28.7 32 0 60.7 0 96L0 416c0 35.3 28.7 64 64 64l448 0c35.3 0 64-28.7 64-64l0-320c0-35.3-28.7-64-64-64L64 32zm56 304c-13.3 0-24 10.7-24 24s10.7 24 24 24l48 0c13.3 0 24-10.7 24-24s-10.7-24-24-24l-48 0zm128 0c-13.3 0-24 10.7-24 24s10.7 24 24 24l112 0c13.3 0 24-10.7 24-24s-10.7-24-24-24l-112 0z"></path>
                        </svg>
                        Card View
                    </button>
                    <button id="btnListView" style="border-radius: 0 7px 7px 0;" runat="server" onserverclick="btnListView_Click">
                        <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 512 512" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                            <path d="M0 96C0 60.7 28.7 32 64 32l384 0c35.3 0 64 28.7 64 64l0 320c0 35.3-28.7 64-64 64L64 480c-35.3 0-64-28.7-64-64L0 96zm64 0l0 64 64 0 0-64L64 96zm384 0L192 96l0 64 256 0 0-64zM64 224l0 64 64 0 0-64-64 0zm384 0l-256 0 0 64 256 0 0-64zM64 352l0 64 64 0 0-64-64 0zm384 0l-256 0 0 64 256 0 0-64z"></path>
                        </svg>
                        List View
                    </button>
                </div>
                <asp:MultiView ID="mvViewType" runat="server" ActiveViewIndex="0">
                    <asp:View ID="viewList" runat="server">
                        <asp:GridView ID="GvReport" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead" EmptyDataText="No records available. Please refine your search.">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Claim">
                                    <ItemTemplate>
                                        <div style="text-align: center;">
                                            <asp:LinkButton ID="lnkClaim" runat="server" OnClick="RegisterClaim" ToolTip='<%#Bind("TicketNO")%>'
                                                CommandArgument='<%# Eval("TicketNO") %>' Text="View">
                                            <i class="fa fa-eye" aria-hidden="true" style="font-size: large;"></i>
                                            </asp:LinkButton>
                                            <asp:HiddenField ID="hdnSkuNo" runat="server" Value='<%# Eval("skuandserialno") %>' />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" />
                                <asp:BoundField DataField="EmailIDAddress" HeaderText="Email Id" />
                                <asp:BoundField DataField="WhatsappNo" HeaderText="Whatsapp No" />
                                <asp:BoundField DataField="AddressLine1" HeaderText="Address Line 1" />
                                <asp:BoundField DataField="City" HeaderText="City" />
                                <asp:BoundField DataField="State" HeaderText="State" />
                                <asp:BoundField DataField="Pincode" HeaderText="Pincode" />
                                <asp:BoundField DataField="Landmark" HeaderText="Landmark" />
                                <asp:BoundField DataField="TaxableValue" HeaderText="Taxable Value" DataFormatString="Rs. {0:N2}" HtmlEncode="false" />
                                <asp:BoundField DataField="TaxAmout" HeaderText="Tax Amout" DataFormatString="Rs. {0:N2}" HtmlEncode="false" />
                                <asp:BoundField DataField="TotalAmountPay" HeaderText="Total Amount Pay" DataFormatString="Rs. {0:N2}" HtmlEncode="false" />
                                <asp:BoundField DataField="TranStatus" HeaderText="Tran Status" />
                                <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date" />
                                <asp:BoundField DataField="OrderId" HeaderText="Order Id" />
                                <asp:BoundField DataField="Productname" HeaderText="Product Name" />
                                <asp:BoundField DataField="Productsubcategoryname" HeaderText="Product Sub Category Name" />
                                <asp:BoundField DataField="Brand" HeaderText="Brand" />
                                <asp:BoundField DataField="ModalName" HeaderText="Modal Name" />
                                <asp:BoundField DataField="serialno" HeaderText="Serial No" />
                                <asp:BoundField DataField="DevicePurchasePrice" HeaderText="Device Purchase Price" />
                                <asp:BoundField DataField="PlanName" HeaderText="Plan Name" />
                                <asp:BoundField DataField="PlanPrice" HeaderText="Plan Price" DataFormatString="Rs. {0:N2}" HtmlEncode="false" />
                                <asp:BoundField DataField="DateofImplementation" HeaderText="Date of Implementation" />
                                <asp:BoundField DataField="ManufacturerWarranty_yymmdd" HeaderText="Manufacturer Warranty(yy/mm/dd)" />
                                <asp:BoundField DataField="ProductPurchaseDate" HeaderText="Product Purchase Date" />
                                <asp:BoundField DataField="imei" HeaderText="IMEI" />
                            </Columns>
                        </asp:GridView>
                    </asp:View>

                    <!-- Card View -->
                    <asp:View ID="viewCard" runat="server">
                        <div class="row">
                            <asp:Repeater ID="rptIncompletePurchase" runat="server">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="col-lg-4 col-12 mb-3">
                                        <div class="plan-card">
                                            <div class="logo-parts  mt-3">
                                                <img src="assets/images/infinity-logo.png" class="infinity-logo" alt="" />
                                                <img src="assets/images/Infyshield-logo.png" class="infyshield-logo" alt="" />
                                            </div>
                                            <h2>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Brand") %>'></asp:Label>
                                            </h2>
                                            <h3>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Productname") %>'></asp:Label>(
                                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("Brand") %>'></asp:Label>)
                                            </h3>
                                            <div class="container-fluid mb-2 px-3 d-flex gap-1 flex-column mt-0">
                                                <div class="row mb-0">
                                                    <div class="col-12 plan-name-txt">Plan: <strong><%# Eval("PlanName") %> </strong></div>
                                                </div>
                                                <div class="row mb-0">
                                                    <div class="col-12 plan-name-txt">Plan Valid From: <strong style='<%# GetPlanStatusBackground(Eval("PlanStatus").ToString()) %>'><%# Eval("manufacturewarrantystartdate") %> to  <%# Eval("manufacturewarrantyenddate") %> (<%# Eval("PlanStatus") %>) </strong></div>
                                                </div>
                                                <div class="row mb-0">
                                                    <div class="col-12 plan-name-txt">IMEI No: <strong><%# FormatSerialWithSpaces(Eval("imei")) %></strong></div>
                                                </div>
                                                <div class="row mb-0">
                                                    <div class="col-12 plan-name-txt">
                                                        InfyShield No: <strong><%# FormatSerialWithSpaces(Eval("skuandserialno")) %></strong>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between px-3">
                                                    <div class="plan-name-txt">Loan No:<strong> <%# Eval("LoanNo") %> </strong></div>
                                                    <button class="eye-icon">
                                                        <asp:LinkButton ID="lnkCardClaim" runat="server" OnClick="RegisterClaim" ToolTip='<%#Bind("skuandserialno")%>'
                                                            CommandArgument='<%# Eval("TicketNO") %>' Text="View">
                                                            <i class="fa fa-eye" aria-hidden="true"></i> View Details
                                                        </asp:LinkButton>
                                                        <asp:HiddenField ID="hdnSkuNo" runat="server" Value='<%# Eval("skuandserialno") %>' />
                                                    </button>
                                                </div>
                                                <div class="row mb-1">
                                                    <div class="col-12 plan-name-txt d-flex">
                                                        Registration Status:
                                                        <p class="ml-2" style='<%# GetStatusBackground(Eval("Status").ToString()) %>'><%# Eval("Status") %></p>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
</asp:Content>
