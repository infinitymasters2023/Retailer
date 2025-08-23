<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="InCompletePurchased.aspx.cs" Inherits="Patner_Retailer_ADO.InCompletePurchased" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
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
            width: 350px;
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

        .thead {
            background: #4397a7 !important;
            color: #fff !important;
        }

            .thead th {
                background: #4397a7 !important;
                color: #fff !important;
            }

        #ContentPlaceHolder1_GvReport_length {
            display: none;
        }

        #ContentPlaceHolder1_GvReport_filter {
            display: none;
        }
        .multiselect-container .multiselect-filter > .fa-search {
            font-size: 12px !important;
        }
        .fw-600 {
            font-weight: 600;
        }

        @media (max-width:576px){
            .plan-card {
                width: 290px;
                min-height: 170px;
            }
            .infyshield-logo {
                max-width: 32px;
            }
            img.infinity-logo {
                max-width: 80px;
            }
        }
        .backToCartcls{
            text-align:end;
        }
        .plan-card a:hover {
            color: #71748d;
        }
/*
        .plan-card:hover {
            border: 1px solid #8ad1e5;
        }*/
    </style>
  <%--  <script>
        setTimeout(function () {
            const rows = document.querySelectorAll('#ContentPlaceHolder1_GvReport_wrapper .row');
            if (rows.length > 1) {
                rows[1].classList.add('table-responsive');
            }
        }, 500);
    </script>--%>
    <script type="text/javascript">
        //function pageLoad() {
        //    $('.multiselect').multiselect({
        //        includeSelectAllOption: true,
        //        enableFiltering: true,
        //        enableCaseInsensitiveFiltering: true,
        //        filterPlaceholder: 'Search',
        //        buttonWidth: '100%',
        //        nonSelectedText: 'Select',
        //        selectAllText: 'Select All',
        //    });
        //}

        function validateMobileNumber(input) {
            input.value = input.value.replace(/[^\d]/g, '').slice(0, 10);
        }
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
                 allSelectedText: 'All selected ({0})',
                 buttonText: function (options, select) {
                     if (options.length === 0) {
                         return 'Select';
                     } else if (options.length === select.find('option').length) {
                         return 'All selected (' + options.length + ')';
                     } else if (options.length >= 1) {
                         return 'selected ' + options.length;
                     } else {
                         return options.map(function () { return $(this).text(); }).get().join(', ');
                     }
                 }
             });
         }
     </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card mb-3">
        <div class="card-body">
            <h5 class="card-header mb-3" style="display: flex; justify-content: space-between; padding-top: 0px !important; padding-left: 0px !important;">Incomplete Purchased</h5>
            <div class="d-flex justify-content-start mb-0">
                <div class="row" style="width: 100%;">
                    <div class="col-12 col-lg-2 mb-2 mb-lg-2">
                        <label>From Date</label>
                        <asp:TextBox ID="txtfromDate" runat="server" CssClass="form-control" AutoCompleteType="Disabled" AutoComplete="off"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtfromDate"></cc1:CalendarExtender>
                    </div>
                    <div class="col-12 col-lg-2 mb-2 mb-lg-2">
                        <label>To Date</label>
                        <asp:TextBox ID="txttodate" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy" TargetControlID="txttodate"></cc1:CalendarExtender>
                    </div>
                    <div class="col-12 col-lg-2 mb-3 mb-lg-2">
                        <%--<asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control">
                            </asp:DropDownList>--%>
                        <label>Product Type</label>
                        <asp:ListBox ID="ddlProduct" CssClass="form-control multiselect" SelectionMode="Multiple" runat="server"></asp:ListBox>
                        <%--<asp:TextBox ID="txtProductname" runat="server" CssClass="form-control" placeholder="Product Name"></asp:TextBox>--%>
                    </div>
                    <div class="col-12 col-lg-2 mb-2 mb-lg-2">
                        <%--<asp:DropDownList ID="ddlBrand" runat="server" CssClass="form-control">
                            </asp:DropDownList>--%>
                        <label>Brand</label>
                        <asp:ListBox ID="ddlBrand" CssClass="form-control multiselect" SelectionMode="Multiple" runat="server"></asp:ListBox>
                        <%--<asp:TextBox ID="txtBrnad" runat="server" CssClass="form-control" placeholder="Brand Name"></asp:TextBox>--%>
                    </div>
                    <div class="col-12 col-lg-2 mb-2 mb-lg-2">
                        <%--<asp:DropDownList ID="ddlPlan" runat="server" CssClass="form-control">
                            </asp:DropDownList>--%>
                        <label>Plan Name</label>
                        <asp:ListBox ID="ddlPlan" CssClass="form-control multiselect" SelectionMode="Multiple" runat="server"></asp:ListBox>
                        <%--<asp:TextBox ID="txtPlanName" runat="server" CssClass="form-control" placeholder="Plan Name"></asp:TextBox>--%>
                    </div>
                    <div id="mainDiv" runat="server" visible="false">
                        <div class="col-12 col-lg-2 mb-2 mb-lg-2">
                            <label>Model</label>
                            <asp:TextBox ID="txtModel" runat="server" CssClass="form-control" placeholder="Model Name" MaxLength="50"></asp:TextBox>
                        </div>
                        <div class="col-12 col-lg-2 mb-2 mb-lg-2">
                            <label>Customer Name</label>
                            <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" placeholder="Customer Name" MaxLength="50"></asp:TextBox>
                        </div>
                        <div class="col-12 col-lg-2 mb-2 mb-lg-2">
                            <label>Customer Mobile No</label>
                            <asp:TextBox ID="txtMobileNo" runat="server" oninput="validateMobileNumber(this)" CssClass="form-control" placeholder="Mobile No"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-12 col-lg-2 pt-2 pt-lg-4">
                        <div class="d-flex justify-content-between gap-2">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn next-step mt-0" Style="font-size: 12px;" OnClick="SubmitReport" />
                            <asp:Button ID="btnExportExcel" runat="server" Text="Excel to Excel" CssClass="btn btn-success" OnClick="btnExportExcel_Click" Style="font-size: 12px;" />
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
                    <%--<asp:Button ID="btnCardView" runat="server" Text="Card View" OnClick="btnCardView_Click" style="border-radius: 7px 0 0 7px;" />--%>
                    <%--<asp:Button ID="btnListView" runat="server" Text="List View" OnClick="btnListView_Click" style="border-radius: 0 7px 7px 0;" />--%>
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
                    </asp:View>

                    <!-- Card View -->
                    <asp:View ID="viewCard" runat="server">
                        <div class="d-flex align-items-center flex-wrap"  style="gap:20px;">
                            <asp:Repeater ID="rptIncompletePurchase" runat="server" OnItemCommand="rptIncompletePurchase_ItemCommand">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%--<div class="col-lg-4 col-12 mb-3">--%>
                                        <div class="plan-card">
                                             <%--<asp:LinkButton ID="LinkButton1" runat="server" CommandName="BackToCart" CommandArgument='<%# Eval("incMid") %>'>--%>
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
                                                    <div class="col-12 plan-name-txt">Plan: <span class="fw-600"><%# Eval("PlanNicknameSelection") %> </span></div>
                                                </div>
                                                <div class="row mb-0">
                                                    <div class="col-12 plan-name-txt">IMEI No. / Serial No.: <span class="fw-600"><%# FormatSerialWithSpaces(Eval("imei")) %>      <strong><%# FormatSerialWithSpaces(Eval("serialno")) %></strong></span></div>
                                                </div>
                                                <div class="row mb-1">
                                                    <div class="col-12 plan-name-txt d-flex">
                                                        Transaction Status:
                                                        <p class="ml-2" style='<%# GetStatusBackground(Eval("Status").ToString()) %>'><%# Eval("Status") %></p>
                                                    </div>
                                                </div>
                                                <div class="backToCartcls">
                                                    <asp:LinkButton ID="backToCart" runat="server" CommandName="BackToCart" CommandArgument='<%# Eval("incMid") %>'>
                                                        <svg viewBox="0 0 100 100" xmlns="http://www.w3.org/2000/svg" style="height: 22px;">
                                                        <g id="shopping-cart-group">
                                                            <circle cx="34" cy="86" r="5" fill="#3D5AFE" />
                                                            <circle cx="76" cy="86" r="5" fill="#3D5AFE" />
                                                            <path d="M 34 86 L 30 70 M 76 86 L 80 70 L 30 70 L 22 35" fill="none" stroke="#8C9EFF" stroke-width="3.5" stroke-linecap="round" stroke-linejoin="round" />
                                                            <path d="M 22 35 L 85 35 L 92 63 L 29 63 Z" fill="none" stroke="#8C9EFF" stroke-width="3.5" stroke-linecap="round" stroke-linejoin="round" />
                                                            <g stroke="#8C9EFF" stroke-width="2.5" stroke-linecap="round">
                                                                <line x1="25" y1="43" x2="87" y2="43" />
                                                                <line x1="27" y1="51" x2="89" y2="51" />
                                                                <line x1="38" y1="35" x2="40" y2="63" />
                                                                <line x1="51" y1="35" x2="53" y2="63" />
                                                                <line x1="64" y1="35" x2="66" y2="63" />
                                                                <line x1="77" y1="35" x2="79" y2="63" />
                                                            </g>
                                                            <path d="M 22 35 L 18 25 H 10" fill="none" stroke="#8C9EFF" stroke-width="3.5" stroke-linecap="round" />
                                                            <rect x="5" y="22" width="12" height="6" fill="#F44336" rx="2" />
                                                        </g>
                                                        <g id="checkout-symbol-group">
                                                            <circle cx="83" cy="45" r="17" fill="#FFC107" />
                                                            <path d="M 75 45 H 91 M 85 39 L 91 45 L 85 51" stroke="#FFFFFF" stroke-width="3" fill="none" stroke-linecap="round" stroke-linejoin="round" />
                                                        </g>
                                                    </svg>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                                 <%--</asp:LinkButton>--%>
                                        </div>
                                    <%--</div>--%>
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
