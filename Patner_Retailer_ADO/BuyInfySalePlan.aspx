<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BuyInfySalePlan.aspx.cs" Inherits="Patner_Retailer_ADO.BuyInfySalePlan" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/js/intlTelInput.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/css/intlTelInput.css">

    <script type="text/javascript">
        window.onload = function () {

            const mobileCodeInput = document.getElementById('<%= txtCustomerMobile.ClientID %>');
            const countryCodeHidden = document.getElementById('<%= hdnCountryCode.ClientID %>');
            const phoneNumberHidden = document.getElementById('<%= hdnPhoneNumber.ClientID %>');


            if (mobileCodeInput && countryCodeHidden && phoneNumberHidden) {
                const itiMobile = window.intlTelInput(mobileCodeInput, {
                    initialCountry: "in",
                    separateDialCode: true,
                    formatOnDisplay: false,
                    utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/js/utils.js",
                });

                function updatePrimaryHiddenFields() {
                    const dialCode = itiMobile.getSelectedCountryData().dialCode;
                    let nationalNumber = itiMobile.getNumber(intlTelInputUtils.numberFormat.NATIONAL).replace(/\D/g, '');
                    if (nationalNumber.startsWith('0')) {
                        nationalNumber = nationalNumber.substring(1);
                    }
                    countryCodeHidden.value = dialCode;
                    phoneNumberHidden.value = nationalNumber;
                }

                mobileCodeInput.addEventListener('countrychange', updatePrimaryHiddenFields);
                mobileCodeInput.addEventListener('blur', updatePrimaryHiddenFields);
            }
        };
    </script>

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

            .prices-txt {
                display: flex;
                gap: 43px;
            }

            .plan-card {
                background: #ffffff;
                border: 1px solid #e2e2e2;
                padding: 0px;
                border-radius: 10px;
                min-height: 240px;
                box-shadow: 0px 0px 5px #d1d1d1;
            }

            .plan-card:hover {
                box-shadow: 0px 0px 5px #64a7bd;
            }


            .plan-card h2 {
                font-size: 16px;
                font-weight: 500;
                padding: 10px 15px;
                margin-bottom: 0;
            }

            .plan-card h3 {
                font-size: 13px;
                font-weight: 400;
                padding: 0px 15px 10px 15px;
                margin-bottom: 0;
            }

        .logo-parts {
            background: #ceebe0;
            padding: 5px 15px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .plan-price {
            display: flex;
            justify-content: space-between;
            padding: 0px 15px 15px 15px;
        }

        .price-txt {
            font-size: 18px;
            font-weight: 600;
        }

        .price-amount {
            font-size: 15px;
            font-weight: 600;
            color: #00a500;
        }

        .form-check-input {
            position: relative;
            border: 1px solid #000 !important;
        }

        .selectWarranty {
            background: blue !important;
            color: white !important;
        }

        .selectCustomWarranty {
            background: #ffc108 !important;
            color: white !important;
        }

        .plan-name-txt {
            font-size: 12px;
        }

        .infyshield-logo {
            max-width: 36px;
        }

        img.infinity-logo {
            max-width: 85px;
        }

        .btn-submit {
            margin-top: 13px;
        }

        .marginTopsty {
            margin-top: 28px;
        }

        .resend-otp {
            font-size: 12px;
        }

        .send-mobile-txt {
            color: #1c9338;
            font-size: 14px;
            display: block;
        }

        .errorMessage {
            color: red;
            font-size: 12px;
        }

        .next-step {
            background: linear-gradient(90deg, rgba(78, 49, 170, 1) 0%, rgba(122, 28, 172, 1) 100%);
            color: #fff !important;
            font-size: 13px !important;
        }

        .select2-container .select2-selection--single {
            height: 37px !important;
        }

        .select2-container--default .select2-selection--single .select2-selection__arrow {
            top: 5px !important;
        }

        .select2-container--default .select2-selection--single .select2-selection__rendered {
            line-height: 34px !important;
            font-size: 13px;
        }

        .productTypeName {
            font-size: 14px;
        }

        .select2-results {
            display: block;
            font-size: 13px;
        }

        .select2-results__option {
            padding: 4px 10px;
            user-select: none;
            -webkit-user-select: none;
        }

        .form-control {
            font-size: 13px !important;
        }

        .warrantyDuration label {
            font-size: 13px;
        }

        .price-discounts {
            display: flex;
            gap: 9px;
        }

        .terms-conditions h5 {
            font-size: 16px;
        }

        .terms-conditions p {
            font-size: 13px;
        }

        .border-danger {
            border-color: red !important;
        }

        .iti {
            position: relative;
            display: inline-block;
            width: 100%;
        }
    </style>

    <!-- CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

    <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/timepicker/1.3.5/jquery.timepicker.min.css" />
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/timepicker/1.3.5/jquery.timepicker.min.js"></script>
    <script>
        function validateDecimalInput(input) {
            input.value = input.value.replace(/[^0-9.]/g, '');
            if ((input.value.match(/\./g) || []).length > 1) {
                input.value = input.value.replace(/\.(?=.*\.)/g, '');
            }
            if (input.value.includes('.') && input.value.split('.')[1].length > 2) {
                input.value = input.value.slice(0, input.value.indexOf('.') + 3);
            }
        }

       <%-- function validateYears() {
            var years = document.getElementById("<%= validationCustom09.ClientID %>").value;
            if (years > 10) {
                document.getElementById("<%= validationCustom09.ClientID %>").value = 10;
            }
        }
        function validateMonths() {
            var years = document.getElementById("<%= validationCustom010.ClientID %>").value;
            if (years > 12) {
                document.getElementById("<%=  validationCustom010.ClientID %>").value = 12;
            }
        }
        function validateDays() {
            var years = document.getElementById("<%= validationCustom011.ClientID %>").value;
            if (years > 31) {
                document.getElementById("<%= validationCustom011.ClientID %>").value = 31;
            }
        }--%>

        function validateMobileNo() {
            var mobileNo = document.getElementById("<%= txtCustomerMobile.ClientID %>").value;
            if (!/^\d{0,10}$/.test(mobileNo)) {
                document.getElementById("<%= txtCustomerMobile.ClientID %>").value = mobileNo.slice(0, -1);
            }
        }
        function validateOTP(input) {
            input.value = input.value.replace(/\D/g, '');
            if (input.value.length > 6) {
                input.value = input.value.slice(0, 6);
            }
        }

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

    <script type="text/javascript">
        function validateIMEI() {
            const imeiInput = document.getElementById('<%= txtimeiNo.ClientID %>');
            const imei = imeiInput.value.trim();
            const errorSpan = document.getElementById('imeiError');

            if (!/^\d{15}$/.test(imei)) {
                errorSpan.innerText = "IMEI must be exactly 15 digits.";
                imeiInput.style.borderColor = "red";
                return false;
            }
            // Valid IMEI
            errorSpan.innerText = "";
            imeiInput.style.borderColor = "";
            return true;
        }
    </script>

    <script>
        function onTermsCheckboxChanged(checkbox) {
            if (checkbox.checked) {
                var modal = new bootstrap.Modal(document.getElementById('termsModal'));
                modal.show();
                checkbox.checked = false; // uncheck after opening modal to force re-check
            }
        }

        function validateTerms() {
            //const chk1 = document.getElementById("chkAgree1");
            //const chk2 = document.getElementById("chkAgree2");

            //chk1.classList.remove("border-danger");
            //chk2.classList.remove("border-danger");
            //if (!chk1.checked) {
            //    alert("Please agree to the Terms and Conditions.");
            //    chk1.classList.add("border-danger");
            //    chk1.focus();
            //    return false;
            //}

            //if (!chk2.checked) {
            //    alert("Please confirm the information is accurate.");
            //    chk2.focus();
            //    chk2.classList.add("border-danger");
            //    return false;
            //}

            return true;
        }
    </script>
    <script src="https://code.jquery.com/jquery-3.5.1.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            ss();

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        ss();
                    }
                });
            }

            function ss() {
                <%--var ddlddlsubcatg = $('#<%= ddlsubcatg.ClientID %>');--%>
                var ddlddlProductType = $('#<%= ddlProductType.ClientID %>');
                var ddlddlBrand = $('#<%= ddlBrand.ClientID %>');

                if (typeof $.fn.select2 !== 'function') {
                    setTimeout(ss, 100);
                    return;
                }

                //if (ddlddlsubcatg.length) {
                //    ddlddlsubcatg.select2();
                //}
                if (ddlddlProductType.length) {
                    ddlddlProductType.select2();
                }
                if (ddlddlBrand.length) {
                    ddlddlBrand.select2();
                }
            }
        });
    </script>







</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




    <div class="card">
        <h5 class="card-header" style="display: flex; justify-content: space-between;">Sell InfyShield Plans
            <asp:Button ID="btnClearCart" runat="server" class="btn next-step mt-0" Text="Clear Cart" Visible="false" OnClick="btnClearCart_Click" />
        </h5>
        <div class="row">
            <div class="col-12 col-lg-6">
                <div class="card-body" style="background: #f7f7f7;">
                    <div class="row">
                        <div class="col-lg-12">
                            <label style="font-weight: 500">Product Type :</label>
                            <asp:Label ID="lblProductName" runat="server" CssClass="productTypeName"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12 col-lg-6 col-xl-6" runat="server" visible="false">
                            <label>Product Sub Category</label>
                            <div class="custom-select-with-caret">
                                <asp:DropDownList ID="ddlsubcatg" runat="server" Enabled="true" AutoPostBack="true"
                                    Visible="true" OnSelectedIndexChanged="ddlsubcatg_OnSelectedIndexChanged" CssClass="form-control input-sm mb-2">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlsubcatg" ErrorMessage="Product Sub Category is required."
                                    ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo" CssClass="errorMessage"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-12 col-lg-6 col-xl-6" id="ProductDiv" runat="server">
                            <label>Product Type</label>
                            <div class="custom-select-with-caret">
                                <asp:DropDownList ID="ddlProductType" OnSelectedIndexChanged="ddlProductType_OnSelectedIndexChanged"
                                    runat="server" Enabled="true" AutoPostBack="true" placeholder="Product Type"
                                    CssClass="form-control input-sm mb-2">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlProductType" ErrorMessage="Product Type is required."
                                    InitialValue="0" ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo" CssClass="errorMessage"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-12 col-lg-6 col-xl-6">
                            <label>Product Sub Type <span class="text-danger">*</span></label>
                            <div class="custom-select-with-caret">
                                <asp:DropDownList ID="ddlProductSubType" runat="server" Enabled="true" placeholder="Product Sub Type"
                                    CssClass="form-control input-sm mb-2">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="refProductSubType" runat="server" ControlToValidate="ddlProductSubType" ErrorMessage="Product Sub Type is required."
                                    InitialValue="0" ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo" CssClass="errorMessage"></asp:RequiredFieldValidator>
                                <asp:Label ID="lblProductSubType" ForeColor="Red" Visible="false" runat="server" CssClass="error-message" Text="Product Sub Type is required."></asp:Label>
                            </div>
                        </div>
                        <div class="col-12 col-lg-6 col-xl-6 mb-2">
                            <label>Make <span class="text-danger">*</span></label>
                            <div class="custom-select-with-caret">
                                <asp:DropDownList ID="ddlBrand" runat="server" Enabled="true" placeholder="Brand"
                                    CssClass="form-control input-sm mb-2">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlBrand" ErrorMessage="Make is required."
                                    InitialValue="0" ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo" CssClass="errorMessage"></asp:RequiredFieldValidator>
                                <asp:Label ID="lblBrand" ForeColor="Red" Visible="false" runat="server" CssClass="error-message" Text="Make is required."></asp:Label>
                            </div>
                            <asp:TextBox ID="txtMake" runat="server" Visible="false" placeholder="Make" CssClass="form-control input-sm mb-2"></asp:TextBox>
                        </div>
                        <div class="col-12 col-lg-6 col-xl-6">
                            <label>Model <span class="text-danger">*</span></label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtModel" runat="server" placeholder="Model" MaxLength="50" AutoComplete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtModel" ErrorMessage="Model is required."
                                ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo" CssClass="errorMessage"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-12 col-lg-6 col-xl-6">
                            <label>Model No.</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="validationCustom06" runat="server" MaxLength="30" placeholder="Model No." AutoComplete="off"></asp:TextBox>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="validationCustom06" ErrorMessage="Model No is required."
        ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo" CssClass="errorMessage"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-12 col-lg-6 col-xl-6">
                            <label>Product Purchase Price <span class="text-danger">*</span></label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtPrice" runat="server" placeholder="Product Purchase Price" MaxLength="10"
                                oninput="validateDecimalInput(this)" AutoComplete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPrice" ErrorMessage="Product Purchase Price is required."
                                ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo" CssClass="errorMessage"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-12 col-lg-6 col-xl-6 mb-2">
                            <label>Product Purchase Date <span class="text-danger">*</span></label>
                            <div class="input-group">
                                <asp:TextBox ID="txtPurchaseDate" runat="server" CssClass="form-control"
                                    AutoCompleteType="Disabled" AutoComplete="off" AutoPostBack="true" OnTextChanged="txtPurchaseDate_TextChanged"></asp:TextBox>
                                <div class="input-group-append">
                                    <span class="input-group-text" style="cursor: pointer;"
                                        onclick="document.getElementById('<%= txtPurchaseDate.ClientID %>').focus();">
                                        <i class="fa fa-calendar"></i>
                                    </span>
                                </div>
                            </div>
                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server"
                                Format="dd-MMM-yyyy"
                                TargetControlID="txtPurchaseDate"
                                EndDate="<%# DateTime.Today %>"></cc1:CalendarExtender>
                            <asp:RequiredFieldValidator ID="rfvPurchaseDate" runat="server" ControlToValidate="txtPurchaseDate" ErrorMessage="Purchase Date is required."
                                ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo" CssClass="errorMessage"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-12 col-lg-6 col-xl-6" id="serielNoDiv" runat="server">
                            <label>Serial No. <span class="text-danger"></span></label>
                            <asp:TextBox class="form-control mb-2" ID="txtSerialNo" CssClass="form-control mb-2" runat="server" placeholder="Serial No."
                                MaxLength="20" AutoComplete="off" AutoPostBack="true" OnTextChanged="SerialNoChange"></asp:TextBox>
                            <small id="serialNoError" style="color: red; display: block;" runat="server"></small>
                        </div>
                        <div class="col-12 col-lg-6 col-xl-6" id="imeiNoDiv" runat="server">
                            <label>IMEI No. 1<span class="text-danger">*</span></label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtimeiNo" MaxLength="15" onblur="validateIMEI()" runat="server" placeholder="IMEI No. 1" AutoComplete="off" AutoPostBack="true" OnTextChanged="IMEINoChange"></asp:TextBox>
                            <small id="imeiError" style="color: red; display: block;" runat="server"></small>
                            <asp:RequiredFieldValidator ID="rfvIMEINo" runat="server" ControlToValidate="txtimeiNo"
                                ErrorMessage="IMEI No. is required." ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo" CssClass="errorMessage" />
                        </div>
                        <div class="col-12 col-lg-6 col-xl-6" id="imeiNo2Div" runat="server">
                            <label>IMEI No. 2</label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtimeiNo2" MaxLength="15" onblur="validateIMEI()" runat="server" placeholder="IMEI No. 2" AutoComplete="off" AutoPostBack="true" OnTextChanged="IMEINo2Change"></asp:TextBox>
                            <small id="Small1" style="color: red; display: block;" runat="server"></small>
                        </div>


                        <div class="col-12 col-lg-6 col-xl-6 mb-2" id="dateOfImplementationDic" runat="server">
                            <label>Product Installation Date <span class="text-danger"></span></label>
                            <div class="input-group">
                                <asp:TextBox ID="txtDateOfImpl" runat="server" CssClass="form-control"
                                    AutoCompleteType="Disabled" AutoComplete="off"></asp:TextBox>
                                <div class="input-group-append">
                                    <span class="input-group-text" style="cursor: pointer;"
                                        onclick="document.getElementById('<%= txtDateOfImpl.ClientID %>').focus();">
                                        <i class="fa fa-calendar"></i>
                                    </span>
                                </div>
                            </div>

                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server"
                                Format="dd-MMM-yyyy"
                                TargetControlID="txtDateOfImpl"></cc1:CalendarExtender>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12 col-lg-6 col-xl-6">
                            <label>Manufacturer Warranty <span class="text-danger">*</span></label>
                            <div class="d-flex flex-wrap gap-2 mb-3 warrantyDuration">
                                <asp:CustomValidator ID="cvWarranty" runat="server" ErrorMessage="Please select a warranty option" Display="Dynamic" ForeColor="Red"
                                    OnServerValidate="ValidateWarranty" ValidationGroup="WarrantyGroup" />
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
                        <div class="col-12 col-lg-6 col-xl-6">
                            <div id="customWarrantyDiv" runat="server" visible="false" style="margin-top: 25px;">
                                <div class="row">
                                    <div class="col-12 col-lg-4">
                                        <asp:DropDownList ID="ddlCustomYears" runat="server" CssClass="form-control mb-2" AppendDataBoundItems="true">
                                            <asp:ListItem Text="Years" Value="" />
                                        </asp:DropDownList>
                                        <asp:Label ID="lblCustomYearsError" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                    <div class="col-12 col-lg-4">
                                        <asp:DropDownList ID="ddlCustomMonths" runat="server" CssClass="form-control mb-2" AppendDataBoundItems="true">
                                            <asp:ListItem Text="Months" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-12 col-lg-4">
                                        <asp:DropDownList ID="ddlCustomDays" runat="server" CssClass="form-control mb-2" AppendDataBoundItems="true">
                                            <asp:ListItem Text="Days" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row" id="customerInfoDiv" runat="server">
                        <div class="col-12 col-lg-6 col-xl-6">
                            <label>Customer Name <span class="text-danger">*</span></label>
                            <asp:TextBox CssClass="form-control mb-2" ID="txtCustomerName" MaxLength="50" runat="server" placeholder="Customer Name" AutoComplete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCustomerName" runat="server" ControlToValidate="txtCustomerName" ErrorMessage="Customer Name is required."
                                ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo" CssClass="errorMessage"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revCustomerName" runat="server" ControlToValidate="txtCustomerName" ValidationExpression="^.{3,}$"
                                ErrorMessage="Customer Name must be at least 3 characters." ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo" CssClass="errorMessage" />
                        </div>

                        <div class="col-12 col-lg-6 col-xl-6">
                            <label>Customer Email ID <span class="text-danger">*</span></label>
                            <asp:TextBox CssClass="form-control mb-2 text-lowercase" ID="txtCustomerEmail" MaxLength="50" runat="server" placeholder="Customer Email ID"
                                TextMode="Email" AutoComplete="off" AutoPostBack="true" OnTextChanged="txtCustomerEmail_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtCustomerEmail" ErrorMessage="Customer Email is required."
                                ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo" CssClass="errorMessage"></asp:RequiredFieldValidator>
                            <asp:Label ID="BlockCustomerEmailErrorMessage" runat="server" CssClass="errorMessage" ForeColor="Red"></asp:Label>
                        </div>

                        <div class="col-12 col-lg-6 col-xl-6">
                            <div class="row align-items-center">
                                <div class="col-12 col-lg-8 pr-0">
                                    <label>Customer Mobile No. <span class="text-danger">*</span></label>
                                    <asp:TextBox CssClass="form-control mb-2" ID="txtCustomerMobile" runat="server" placeholder="Mobile No." name="txtCustomerMobile"
                                        pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNo()" AutoComplete="off" AutoPostBack="true" OnTextChanged="txtCustomerMobile_TextChanged"></asp:TextBox>

                                    <asp:HiddenField ID="hdnCountryCode" runat="server" />
                                    <asp:HiddenField ID="hdnPhoneNumber" runat="server" />
                                </div>
                                <div class="col-12 col-lg-4 mt-0">
                                    <asp:Button ID="btnSubmitPlan" CssClass="btn next-step" OnClick="SubmitPlanInfo" ValidationGroup="ProductInfo" runat="server" Text="Submit" />
                                    <asp:Button ID="btnEditPlan" CssClass="btn next-step" OnClick="EditPlanInfo" Style="margin-top: 18px;" runat="server" Text="Edit" />
                                </div>
                                <asp:Label ID="BlockCustomerMobileErrorMessage" runat="server" class="errorMessage"></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtCustomerMobile" ErrorMessage="Customer Mobile is required."
                                    ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo" CssClass="errorMessage"></asp:RequiredFieldValidator>
                            </div>

                        </div>

                        <div class="col-xl-6 col-lg-6 col-12">
                            <div id="OTPPanel" class="row" runat="server" visible="false">
                                <div class="row">
                                    <div class="col-9 col-lg-9">
                                        <label>Enter OTP</label>
                                        <input type="text" class="form-control mb-2" id="txtOTP" runat="server" placeholder="Enter OTP"
                                            maxlength="6" pattern="\d{6}" title="Enter a 6-digit OTP" oninput="validateOTP(this)" autocomplete="off">
                                        <div class="text-right">
                                            <asp:LinkButton ID="lnkResendOTP" runat="server" CssClass="resend-otp" OnClick="lnkResendOTP_Click">Resend OTP</asp:LinkButton>
                                        </div>
                                        <asp:Label ID="lblOTPSend" CssClass="send-mobile-txt" Text="OTP Send to the Mobile No." runat="server" Visible="false"></asp:Label>
                                        <asp:RequiredFieldValidator ID="refOTP" runat="server" ControlToValidate="txtOTP" CssClass="errorMessage" ErrorMessage="OTP is required."
                                            ForeColor="Red" Display="Dynamic" ValidationGroup="OTPSubmission">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-3 col-lg-3">
                                        <asp:Button CssClass="btn next-step" ID="btnSubmitOTP" OnClick="SubmitOTP" runat="server" AutoPostBack="false" Text="Verify" ValidationGroup="OTPSubmission" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-12 col-lg-6">

                <div class="buyinfyshieldSlider">
                    <asp:Repeater ID="rptslider" runat="server">
                        <ItemTemplate>
                            <div>
                                <img src='<%# Eval("ImageURL") %>' alt='<%# Eval("ProductType") %>' style="width: 100%; height: auto;" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

            </div>
        </div>


        <div id="PlanPanel" class="mt-3" runat="server">
            <h4 class="text-center mb-3">Choose the Best Plan for Your Product</h4>
            <asp:Panel ID="pnlNoPlans" runat="server" Visible="false" CssClass="alert alert-info text-center mt-4 mx-4">
                <h5 class="mb-1">No Plans Available</h5>
                <p class="mb-0">Currently, there are no service plans available for your selection. Please check back later or contact support for assistance.</p>
            </asp:Panel>

            <div class="mx-4">
                <asp:Repeater ID="rptPlans" runat="server" OnItemDataBound="rptPlans_ItemDataBound">
                    <HeaderTemplate>
                        <div class="row">
                    </HeaderTemplate>

                    <ItemTemplate>
                        <div class="col-lg-4 col-12 mb-3">
                            <div class="plan-card">
                                <div class="form-check mb-2 px-3 pt-2">
                                    <asp:CheckBox ID="chkSelect" runat="server" CssClass="planCheckBox"
                                        AutoPostBack="true"
                                        OnCheckedChanged="chkSelect_CheckedChanged"
                                        CommandArgument='<%# Eval("Mid") %>' />
                                    <label class="form-check-label">Select</label>
                                </div>
                                <div class="logo-parts">
                                    <img src="assets/images/infinity-logo.png" class="infinity-logo" alt="" />
                                    <img src="assets/images/Infyshield-logo.png" class="infyshield-logo" alt="" />
                                </div>
                                <h2>
                                    <asp:Label ID="lblPlanName" runat="server" Text='<%# Eval("PlanNicknameSelection") %>'></asp:Label>
                                    <asp:HiddenField ID="hdnPlanId" runat="server" Value='<%# Eval("Mid") %>' />
                                </h2>
                                <h3>
                                    <div id="divpp" runat="server" visible="false">
                                        <asp:Label ID="lblPlanFullName" runat="server" Text='<%# Eval("PlanNickNameSCMS") %>'></asp:Label>
                                        <br />
                                    </div>
                                    <asp:Label ID="lblPlanSKU" runat="server" Text='<%# Eval("SKU") %>'></asp:Label>
                                </h3>
                                <h3 id="divpd" runat="server" visible="false">
                                    <asp:Label ID="lblProduct" runat="server" Text='<%# Session["ProductTypeName"].ToString() %>'></asp:Label>
                                </h3>
                                <div class="d-flex gap-3 mb-2 px-3">
                                    <%--<div class="plan-name-txt"><%# Eval("FinalPlanNameDescription") %></div>--%>
                                    <%--  <div class="plan-name-txt">SDP:  <%# Eval("SDP") %></div>
                                    <div class="plan-name-txt">ADP: <%# Eval("ADP") %></div>--%>
                                </div>
                                <div class="plan-price mt-4">
                                    <%--<div class="price-txt">Price:</div>--%>
                                    <div class="prices-txt">

                                        <div class="price-amount text-danger">MRP: <strike><%# Eval("MRP") %></strike></div>

                                        <div class="price-discounts">
                                            <div class="price-amount">Offer Price: <%# Eval("OfferPrice") %></div>
                                            <div class="price-amount" id="txtDiscountPer" runat="server"><%# Eval("DiscountPer") %></div>
                                        </div>

                                    </div>
                                </div>

                                <asp:HiddenField ID="hdnPlanPrice" runat="server" Value='<%# Eval("OfferPrice") %>' />
                                <asp:HiddenField ID="hdnSKU" runat="server" Value='<%# Eval("SKU") %>' />

                            </div>
                        </div>
                    </ItemTemplate>

                    <FooterTemplate>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
                <%-- <table class="table-responsive table data-table table-striped table-bordered nowrap">
                                    <thead class="thead-dark">
                                        <tr>
                                            <th>Select</th>
                                            <th>Plan Name</th>
                                            <th>EW (In Years)</th>
                                            <th>SDP (In Years)</th>
                                            <th>ADP (In Years)</th>
                                            <th>Description</th>
                                            <th>Plan Price (₹)</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptPlans" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="text-align:center;">
                                                        <asp:CheckBox ID="chkSelect" runat="server" CssClass="form-check-input planCheckBox"
                                                            AutoPostBack="true"
                                                            OnCheckedChanged="chkSelect_CheckedChanged"
                                                            CommandArgument='<%# Eval("Mid") %>' Style="margin-top: -5px;" />
                                                        <asp:HiddenField ID="hdnPlanPrice" runat="server" Value='<%# Eval("CustPriceINR") %>' />
                                                        <asp:HiddenField ID="hdnSKU" runat="server" Value='<%# Eval("SKU") %>' />
                                                        <asp:HiddenField ID="hdnPlanId" runat="server" Value='<%# Eval("Mid") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPlanName" runat="server" Text='<%# Eval("PlanNickName") %>'></asp:Label>
                                                    </td>
                                                    <td><%# Eval("EW") %></td>
                                                    <td><%# Eval("SDP") %></td>
                                                    <td><%# Eval("ADP") %></td>
                                                    <td><%# Eval("FinalPlanNameDescription") %></td>
                                                    <td><%# Eval("CustPriceINR") %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>--%>
            </div>
        </div>

        <div id="ApplyPromoCodePanel" runat="server">
            <div class="d-flex justify-content-center mt-3">
                <div class="col-xl-3 col-lg-3 col-8">
                    <input type="text" class="form-control" id="txtPromoDiscount" runat="server" placeholder="Apply Your Promo Code" autocomplete="off">
                    <asp:Label ID="lblErrorPromoCode" ForeColor="Red" runat="server"></asp:Label>
                </div>

                <div class="col-lg-2 col-4">
                    <asp:Button ID="btnApplyPromoCode" CssClass="btn next-step mt-0" runat="server" AutoPostBack="true" OnClick="ApplyPromoCode" Text="Apply" />
                </div>
            </div>

            <div runat="server" id="AddOnsDiv" visible="false">
                <div class="row">
                    <asp:Repeater ID="rptAddOns" runat="server">
                        <HeaderTemplate>
                            <div class="row">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="col-lg-3 col-12 mb-4">
                                <div class="plan-card">
                                    <div class="form-check mb-2">
                                        <asp:CheckBox ID="chkAddOnsSelect" runat="server" CssClass="form-check-input"
                                            AutoPostBack="true"
                                            Checked='<%# Eval("IsFree").ToString() == "1" %>'
                                            OnCheckedChanged="chkSelect_AddOnsChanged"
                                            CommandArgument='<%# Eval("Mid") %>' />
                                        <asp:HiddenField ID="hdnAddOnsId" runat="server" Value='<%# Eval("Mid") %>' />
                                        <label class="form-check-label">Select</label>
                                    </div>
                                    <h2>
                                        <asp:Label ID="lblAddOnsName" runat="server" Text='<%# Eval("AdsOnName") %>'></asp:Label>
                                    </h2>
                                    <div class="d-flex gap-3 mb-2">
                                        <div class="plan-name-txt"><strong>Add-Ons Code:</strong> <%# Eval("AdsOnCode") %></div>
                                    </div>
                                    <div class="plan-price">
                                        <div class="price-txt">Price: </div>
                                        <div class="price-amount">&nbsp; <%# Eval("DiscountLabel") %></div>
                                        <asp:Label ID="lblFinalPrice" runat="server" Text='<%# Eval("FinalPrice", "₹{0:N2}") %>' Visible="false"></asp:Label>
                                    </div>
                                    <div class="price-amount" style="color: green;">&nbsp; <%# Eval("DisplayText") %></div>

                                </div>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>
                    <%-- <table class="table-responsive table data-table table-striped table-bordered nowrap">
                                        <thead class="thead-dark">
                                            <tr>
                                                <th>Select</th>
                                                <th>Add-Ons Name</th>
                                                <th>Add-Ons Code</th>
                                                <th>Price (₹)</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptAddOns" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: center;">
                                                            <asp:CheckBox ID="chkAddOnsSelect" runat="server" CssClass="form-check-input"
                                                                AutoPostBack="true"
                                                                Checked='<%# Eval("IsFree").ToString() == "1" %>'
                                                                Enabled='<%# Eval("IsFree").ToString() != "1" %>'
                                                                OnCheckedChanged="chkSelect_AddOnsChanged"
                                                                CommandArgument='<%# Eval("Mid") %>' Style="margin-top: -5px;" />
                                                            <asp:HiddenField ID="hdnAddOnsId" runat="server" Value='<%# Eval("Mid") %>' />
                                                        </td>
                                                        <td><%# Eval("AdsOnName") %></td>
                                                        <td><%# Eval("AdsOnCode") %></td>
                                                        <td><%# Eval("DisplayText") %></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>--%>
                </div>
            </div>

            <div runat="server" id="calculationdiv" visible="false" class="mt-3 mx-5">
                <div class="row mb-2">
                    <div class="col-md-6 text-end">
                        <label style="font-size: 13px;">Offer Amount :</label>
                    </div>
                    <div class="col-md-6 text-start">
                        <strong style="font-size: 14px;">
                            <asp:Label ID="lblPlanPrice" runat="server"></asp:Label>
                        </strong>
                    </div>
                </div>

                <div class="row mb-2" id="PromoCodeDiv" runat="server">
                    <div class="col-md-6 text-end">
                        <label id="lblPromoCodeDiscountAmount" runat="server" style="font-size: 13px;">Promo Discount Amount :</label>
                    </div>
                    <div class="col-md-6 text-start">
                        <strong style="font-size: 14px;">
                            <asp:Label ID="lblDiscountAmount" runat="server"></asp:Label>
                        </strong>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6 text-end">
                        <label style="font-size: 13px;">Total Amount :</label>
                    </div>
                    <div class="col-md-6 text-start">
                        <strong style="font-size: 14px;">
                            <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                        </strong>
                    </div>
                </div>
            </div>

            <div class="form-row">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-12" style="text-align: center;">
                </div>
                <div class="col-xl-12 col-lg-12 col-12">
                    <div class="form-group px-3">
                        <div class="form-check d-flex justify-content-center gap-2">
                            <input class="form-check-input" type="checkbox" id="invalidCheck" onchange="onTermsCheckboxChanged(this)" runat="server" />

                            <label class="form-check-label" for="invalidCheck">
                                By proceeding, you agree to the Terms and Conditions
                            </label>
                        </div>

                        <div class="d-flex justify-content-center">
                            <asp:Label ID="lblErrorTermCondition" runat="server" Text="Please Accept Term and Condition to buy Plan." Font-Size="12px" ForeColor="Red" Visible="false"></asp:Label>
                        </div>

                    </div>
                </div>
                <div class="modal fade" id="termsModal" tabindex="-1" aria-labelledby="termsModalLabel" aria-hidden="true" style="--bs-modal-width: 990px; padding-right: 0px !important;">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content p-3">
                            <div class="modal-header pt-0 pl-0">
                                <h5 class="modal-title">Terms & Conditions</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>

                            </div>
                            <div class="modal-body px-0 pb-0">
                                <div style="max-height: 60vh; overflow: hidden auto;">
                                    <div class="row">
                                        <div class="col-xl-12">
                                            <div class="card custom-card">
                                                <div class=" p-0 product-checkout">
                                                    <div class="terms-conditions" style="padding: 14px;">
                                                        <div style="flex: 1 1 0%; overflow: hidden auto; font-size: 14px;">
                                                            <p>
                                                                The InfyShield Product Protection Plan provides extended warranty, accidental damage protection, and screen damage protection for various electronic products. Here's a summary:
                                                            </p>

                                                            <h5>Coverage:</h5>
                                                            <p class="mb-1">- Extended Warranty Protection Plan: Covers breakdowns or malfunctions after the manufacturer's warranty period has expired.</p>
                                                            <p class="mb-1">- Accidental Damage Protection: Covers sudden and unforeseen damage to the product due to external means.</p>
                                                            <p class="mb-1">- Screen Damage Protection: Covers damage to the product's screen due to accidental drops or impacts.</p>

                                                            <h5 class="mt-3">Eligibility:</h5>

                                                            <p class="mb-1">- Products must be brand new and not pre-owned or refurbished.</p>
                                                            <p class="mb-1">- Products must be purchased in India with a valid purchase bill/invoice.</p>
                                                            <p class="mb-1">- Maximum combined period of coverage, including manufacturer's warranty, is 3-5 years depending on the product type.</p>

                                                            <h5 class="mt-3">Services:</h5>

                                                            <p class="mb-1">- Repair or replacement of defective products.</p>
                                                            <p class="mb-1">- Pickup and delivery services for portable devices.</p>
                                                            <p class="mb-1">- On-site service for certain products.</p>

                                                            <h5 class="mt-3">Exclusions:</h5>

                                                            <p class="mb-1">- Products not listed in the document.</p>
                                                            <p class="mb-1">- Pre-existing defects or damages.</p>
                                                            <p class="mb-1">- Damage caused by misuse, neglect, or normal wear and tear.</p>
                                                            <p class="mb-1">- Cosmetic damage or damage to accessories.</p>

                                                            <h5 class="mt-3">Terms and Conditions:</h5>

                                                            <p class="mb-1">- Registration of the service plan is mandatory within 7 days of purchase.</p>
                                                            <p class="mb-1">- Service order charges apply for certain services.</p>
                                                            <p class="mb-1">- Customers are responsible for maintaining their products and providing proof of maintenance.</p>

                                                            <h5 class="mt-3">Cancellation and Termination:</h5>

                                                            <p class="mb-1">- Free-look period of 7 business days for cancellation with full refund.</p>
                                                            <p class="mb-1">- Plan can be terminated due to fraud, non-disclosure, or non-payment.</p>
                                                            <p class="mb-1">- Pro-rata refund applicable in certain cases.</p>

                                                            <p class="mb-1">
                                                                The InfyShield Product Protection Plan is a service plan offered by Infinity Assurance Solutions Private Limited that provides extended warranty, accidental damage protection, and screen damage protection for various electronic products. Here's a summary:
                                                            </p>

                                                            <h5 class="mt-3">Key Features:</h5>

                                                            <p class="mb-1">
                                                                1. Coverage: The plan covers various products, including consumer electronics, large appliances, home appliances, kitchen appliances, mobile phones, personal grooming devices, wearables, PCs, laptops, displays, office automation, security & surveillance, and more.
                                                            </p>
                                                            <p class="mb-1">
                                                                2. Types of Plans: The plan offers different types of coverage, including Extended Warranty Protection Plan, Accidental Damage Protection Plan, Screen Damage Protection Plan, Total Protection Plan, and Combo Plans.
                                                            </p>
                                                            <p class="mb-1">
                                                                3. Eligibility: Products must be brand new, not pre-owned or refurbished, and purchased in India with a valid purchase bill/invoice.
                                                            </p>
                                                            <p class="mb-1">4. Registration: Registration of the service plan is mandatory within 7 days of purchase.</p>

                                                            <h5 class="mt-3">Terms and Conditions:</h5>

                                                            <p class="mb-1">1. Service Order Charges: Applicable service order charges are payable at the time of registering a service request.</p>
                                                            <p class="mb-1">2. Pick-up and Delivery: Pick-up and delivery services are available for portable devices and gadgets.</p>
                                                            <p class="mb-1">3. On-site Service: On-site service is available for certain products.</p>
                                                            <p class="mb-1">4. Exclusions: The plan excludes damages caused by misuse, neglect, normal wear and tear, and certain other conditions.</p>

                                                            <h5 class="mt-3">Claims and Repairs:</h5>

                                                            <p class="mb-1">1. Claim Process: Customers need to inform Infinity Assurance Solutions Private Limited immediately after a defect or damage occurs.</p>
                                                            <p class="mb-1">2. Repair or Replacement: The company will repair or replace the product, or offer liquidated damages, depending on the situation.</p>

                                                            <h5 class="mt-3">Cancellation and Termination:</h5>

                                                            <p class="mb-1">1. Free-look Period: Customers can cancel the plan within 7 business days of purchase for a full refund.</p>
                                                            <p class="mb-1">2. Termination: The plan can be terminated due to fraud, non-disclosure, or non-payment.</p>

                                                            <h5 class="mt-3">Governing Law and Dispute Resolution:</h5>

                                                            <p class="mb-1">1. Governing Law: The plan is governed by the laws of India.</p>
                                                            <p class="mb-1">2. Dispute Resolution: Disputes will be resolved through the courts of Delhi.</p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <%--<asp:Button ID="btnConfirmBuy" runat="server" Text="I Accept all Terms & Conditions" CssClass="btn btn-success w-100 mt-3" OnClick="btnConfirmBuy_Click" OnClientClick="return validateTerms();" />--%>
                                </div>
                                <div class="modal-footer">


                                    <button type="button" class="btn btn-secondary px-2" fdprocessedid="ke02ty">Close</button>
                                    <%--  <button type="button" class="btn btn-primary px-2" fdprocessedid="zrhbji">I accept all terms and conditions</button>--%>


                                    <asp:Button ID="btnConfirmBuy" runat="server" Text="I Accept all Terms & Conditions" CssClass="btn btn-primary px-2" OnClick="btnConfirmBuy_Click" OnClientClick="return validateTerms();" />

                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-4 mt-2" style="text-align: center;">
                    <asp:Button ID="btnSubmitMain" CssClass="btn next-step mt-0" runat="server" AutoPostBack="true" OnClick="AddToCart" Text="Add to Cart" />
                </div>
            </div>
        </div>

    </div>




    <!-- ============================================================== -->
    <!-- end validation form -->
    <!-- ============================================================== -->


</asp:Content>
