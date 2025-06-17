<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AddEditFavourite.aspx.cs" Inherits="Patner_Retailer_ADO.AddEditFavourite" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .custom-select-with-caret {
            position: relative;
        }

        .custom-select-with-caret select {
            appearance: none;
            -webkit-appearance: none;
            -moz-appearance: none;
            padding-right: 2rem; /* space for caret */
            background-image: url('data:image/svg+xml;utf8,<svg fill="black" height="16" viewBox="0 0 24 24" width="16" xmlns="http://www.w3.org/2000/svg"><path d="M7 10l5 5 5-5z"/></svg>');
            background-repeat: no-repeat;
            background-position: right 0.75rem center;
            background-size: 1.8rem;
        }

        .plan-card {
            background: #f6f6f6;
            border: 1px solid #e2e2e2;
            padding: 15px;
            border-radius: 10px;
        }

        .plan-card h2 {
            font-size: 18px;
            font-weight: 500;
        }

        .plan-price {
            display: flex;
            justify-content: space-between;
        }

        .price-txt, .price-amount {
            font-size: 16px;
            font-weight: 700;
        }

        .form-check-input {
            position: relative;
        }

        .selectWarranty {
            background: blue !important;
            color: white !important;
        }

        .selectCustomWarranty {
            background: #ffc108 !important;
            color: white !important;
        }
    </style>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/timepicker/1.3.5/jquery.timepicker.min.css" />
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/timepicker/1.3.5/jquery.timepicker.min.js"></script>
    <script>
  
        (function () {
            $("#<%= txtPurchaseDate.ClientID %>").datepicker({
                dateFormat: "dd/mm/yy"
            });
        });
    </script>
    <script type="text/javascript">        
        document.addEventListener("DOMContentLoaded", function () {
            const checkboxes = document.querySelectorAll(".planCheckBox input[type='checkbox']");
            checkboxes.forEach(function (checkbox) {
                checkbox.addEventListener("change", function () {
                    if (this.checked) {
                        checkboxes.forEach(cb => {
                            if (cb !== this) cb.checked = false;
                        });
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card">
        <h5 class="card-header">Favourite InfyShield Plans</h5>
        <div class="card-body">
            <div class="row">
                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12">
                    <label>Product Sub Category</label>
                    <div class="custom-select-with-caret">
                        <asp:DropDownList ID="ddlsubcatg" runat="server" Enabled="true" AutoPostBack="true"
                            Visible="true" OnSelectedIndexChanged="ddlsubcatg_OnSelectedIndexChanged" CssClass="form-control input-sm mb-2">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlsubcatg" ErrorMessage="Product Sub Category is required."
                            ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12">
                    <label>Product Type</label>
                    <div class="custom-select-with-caret">
                        <asp:DropDownList ID="ddlProductType" OnSelectedIndexChanged="ddlProductType_OnSelectedIndexChanged"
                            runat="server" Enabled="true" AutoPostBack="true" placeholder="Product Type"
                            CssClass="form-control input-sm mb-2">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlProductType" ErrorMessage="Product Type is required."
                            InitialValue="0" ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                    <label>Make</label>
                    <div class="custom-select-with-caret">
                        <asp:DropDownList ID="ddlBrand" runat="server" Enabled="true" placeholder="Brand"
                            CssClass="form-control input-sm mb-2">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlBrand" ErrorMessage="Brand is required."
                            InitialValue="0" ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                    </div>
                    <asp:TextBox ID="txtMake" runat="server" Visible="false" placeholder="Make" CssClass="form-control input-sm mb-2"></asp:TextBox>
                </div>
            </div>

            <div class="row">
                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12">
                    <label>Product Purchase Date</label>
                    <div class="input-group">
                        <asp:TextBox ID="txtPurchaseDate" runat="server" CssClass="form-control"
                            AutoCompleteType="Disabled" AutoComplete="off"></asp:TextBox>
                        <div class="input-group-append">
                            <span class="input-group-text" style="cursor: pointer;"
                                onclick="document.getElementById('<%= txtPurchaseDate.ClientID %>').focus();">
                                <i class="fa fa-calendar"></i>
                            </span>
                        </div>
                    </div>
                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtPurchaseDate" EndDate="<%# DateTime.Today %>"></cc1:CalendarExtender>
                    <asp:RequiredFieldValidator ID="rfvPurchaseDate" runat="server" ControlToValidate="txtPurchaseDate" ErrorMessage="Purchase Date is required."
                        ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                </div>
               
                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                    <label>Model</label>
                    <asp:TextBox CssClass="form-control mb-2" ID="txtModel" runat="server" placeholder="Model" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtModel" ErrorMessage="Model is required."
                        ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-12">
                    <label>Manufacturer Warranty</label>
                    <div class="d-flex flex-wrap gap-2 mb-3">
                        <asp:CustomValidator ID="cvWarranty" runat="server" ErrorMessage="Please select a warranty option" Display="Dynamic" ForeColor="Red"
                            OnServerValidate="ValidateWarranty" ValidationGroup="ProductInfo" />
                        <asp:RadioButton ID="rb3M" runat="server" GroupName="Warranty" Text="3 Months" CssClass="btn-check" AutoPostBack="true" OnCheckedChanged="WarrantyChanged" />
                        <asp:Label ID="lbl3M" runat="server" AssociatedControlID="rb3M" CssClass="btn btn-outline-primary">3 Months</asp:Label>

                        <asp:RadioButton ID="rb6M" runat="server" GroupName="Warranty" Text="6 Months" CssClass="btn-check" AutoPostBack="true" OnCheckedChanged="WarrantyChanged" />
                        <asp:Label ID="lbl6M" runat="server" AssociatedControlID="rb6M" CssClass="btn btn-outline-primary">6 Months</asp:Label>

                        <asp:RadioButton ID="rb1Y" runat="server" GroupName="Warranty" Text="1 Year" CssClass="btn-check" AutoPostBack="true" OnCheckedChanged="WarrantyChanged" />
                        <asp:Label ID="lbl1Y" runat="server" AssociatedControlID="rb1Y" CssClass="btn btn-outline-primary">1 Year</asp:Label>

                        <asp:RadioButton ID="rb2Y" runat="server" GroupName="Warranty" Text="2 Years" CssClass="btn-check" AutoPostBack="true" OnCheckedChanged="WarrantyChanged" />
                        <asp:Label ID="lbl2Y" runat="server" AssociatedControlID="rb2Y" CssClass="btn btn-outline-primary">2 Years</asp:Label>

                        <asp:RadioButton ID="rb3Y" runat="server" GroupName="Warranty" Text="3 Years" CssClass="btn-check" AutoPostBack="true" OnCheckedChanged="WarrantyChanged" />
                        <asp:Label ID="lbl3Y" runat="server" AssociatedControlID="rb3Y" CssClass="btn btn-outline-primary">3 Years</asp:Label>

                        <asp:RadioButton ID="rbCustom" runat="server" GroupName="Warranty" Text="Custom" CssClass="btn-check" AutoPostBack="true" OnCheckedChanged="WarrantyChanged" />
                        <asp:Label ID="lblCustom" runat="server" AssociatedControlID="rbCustom" CssClass="btn btn-outline-warning">Custom</asp:Label>

                        <asp:Label ID="lblWarrantyError" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-12">

                    <div id="customWarrantyDiv" runat="server" visible="false" style="margin-top: 25px;">
                        <div class="row">
                            <div class="col-4">
                                <asp:DropDownList ID="ddlCustomYears" runat="server" CssClass="form-control mb-2" AppendDataBoundItems="true">
                                    <asp:ListItem Text="Years" Value="" />
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlCustomYears" ErrorMessage="Year is required."
                                    InitialValue="0" ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                                <asp:Label ID="lblCustomYearsError" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                            <div class="col-4">
                                <asp:DropDownList ID="ddlCustomMonths" runat="server" CssClass="form-control mb-2" AppendDataBoundItems="true">
                                    <asp:ListItem Text="Months" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="col-4">
                                <asp:DropDownList ID="ddlCustomDays" runat="server" CssClass="form-control mb-2" AppendDataBoundItems="true">
                                    <asp:ListItem Text="Days" Value="" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 mt-4">
                    <asp:Button ID="btnSubmitPlan" class="btn btn-primary" OnClick="SubmitPlanInfo" ValidationGroup="ProductInfo" runat="server" Text="Submit" />
                    <asp:Button ID="btnEditPlan" class="btn btn-primary" OnClick="EditPlanInfo" runat="server" Text="Edit" />
                </div>
            </div>
            <div id="PlanPanel" runat="server">
                <div class="row"></div>
                <h3 class="text-center mb-3">Choose the Best Plan for Your Product</h3>
                <asp:Panel ID="pnlNoPlans" runat="server" Visible="false" CssClass="alert alert-info text-center mt-4">
                    <h5 class="mb-1">No Plans Available</h5>
                    <p class="mb-0">Currently, there are no service plans available for your selection. Please check back later or contact support for assistance.</p>
                </asp:Panel>
                <div class="row">
                    <asp:Repeater ID="rptPlans" runat="server">
                        <HeaderTemplate>
                            <div class="row">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="col-lg-3 col-12 mb-4">
                                <div class="plan-card">
                                    <div class="form-check mb-2 pl-0">
                                        <asp:CheckBox ID="chkSelect" runat="server" CssClass="planCheckBox"
                                            AutoPostBack="true"
                                            OnCheckedChanged="chkSelect_CheckedChanged"
                                            CommandArgument='<%# Eval("Mid") %>' />
                                        <label class="form-check-label">Select</label>
                                    </div>
                                    <h2>
                                        <asp:Label ID="lblPlanName" runat="server" Text='<%# Eval("PlanNickName") %>'></asp:Label>
                                    </h2>
                                    <div class="d-flex gap-3 mb-2">
                                        <div class="plan-name-txt"><strong>EW:</strong> <%# Eval("EW") %></div>
                                        <div class="plan-name-txt"><strong>SDP:</strong>  <%# Eval("SDP") %></div>
                                        <div class="plan-name-txt"><strong>ADP:</strong> <%# Eval("ADP") %></div>
                                    </div>
                                    <div class="plan-price">
                                        <div class="price-txt">Price:</div>
                                        <div class="price-amount">₹<%# Eval("CustPriceINR") %></div>
                                    </div>
                                    <asp:HiddenField ID="hdnPlanPrice" runat="server" Value='<%# Eval("CustPriceINR") %>' />
                                    <asp:HiddenField ID="hdnSKU" runat="server" Value='<%# Eval("SKU") %>' />
                                </div>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <hr />
            <div id="AddToFavouritePanel" runat="server">
                <div class="row">
                    <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 mt-4">
                        <asp:Button ID="btnAddToFavourite" class="btn btn-primary" OnClick="AddToFavourite" ValidationGroup="ProductInfo" runat="server" Text="Submit" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
