<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BuyInfySalePlan.aspx.cs" Inherits="Patner_Retailer_ADO.BuyInfySalePlan" %>


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

        function validateYears() {
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
        }

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

            let sum = 0;
            for (let i = 0; i < 15; i++) {
                let digit = parseInt(imei.charAt(i), 10);
                if (i % 2 === 1) {
                    digit *= 2;
                    if (digit > 9) digit -= 9;
                }
                sum += digit;
            }

            if (sum % 10 !== 0) {
                errorSpan.innerText = "Invalid IMEI number";
                imeiInput.style.borderColor = "red";
                return false;
            }

            // Valid IMEI
            errorSpan.innerText = "";
            imeiInput.style.borderColor = "";
            return true;
        }
    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid  dashboard-content">
        <div class="row">
            <!-- ============================================================== -->
            <!-- validation form -->
            <!-- ============================================================== -->
            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                <div class="card">
                    <h5 class="card-header">Buy InfyShield Plans</h5>
                    <div class="card-body">

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12">
                                <label>Product Sub Category</label>
                                <div class="custom-select-with-caret">
                                    <asp:DropDownList ID="ddlsubcatg" runat="server" Enabled="true" AutoPostBack="true"
                                        Visible="true" OnSelectedIndexChanged="ddlsubcatg_OnSelectedIndexChanged" CssClass="form-control input-sm mb-2">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12">
                                <label>Product Type</label>
                                <div class="custom-select-with-caret">
                                    <asp:DropDownList ID="ddlProductType" OnSelectedIndexChanged="ddlProductType_OnSelectedIndexChanged"
                                        runat="server" Enabled="true" AutoPostBack="true" placeholder="Product Type"
                                        CssClass="form-control input-sm mb-2" required>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                <label>Product Purchase Price</label>
                                <asp:TextBox class="form-control mb-2" ID="txtPrice" runat="server" placeholder="Product Purchase Price" maxlength="10" required
                                    oninput="validateDecimalInput(this)" AutoPostBack="true" OnTextChanged="txtModel_TextChanged"></asp:TextBox>                                
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12">
                                <label>Product Purchase Date</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtPurchaseDate" runat="server" CssClass="form-control"
                                        AutoPostBack="true" AutoCompleteType="Disabled" AutoComplete="off" OnTextChanged="txtModel_TextChanged"></asp:TextBox>
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
                            </div>


                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                <label>Brand</label>
                                <div class="custom-select-with-caret">
                                    <asp:DropDownList ID="ddlBrand" runat="server" Enabled="true" placeholder="Brand"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged" CssClass="form-control input-sm mb-2">
                                    </asp:DropDownList>
                                </div>
                                <asp:TextBox ID="txtMake" runat="server" Visible="false" placeholder="Make" CssClass="form-control input-sm mb-2"></asp:TextBox>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                <label>Model</label>
                                <asp:TextBox class="form-control mb-2" ID="txtModel" runat="server" placeholder="Model" maxlength="50" required 
                                    AutoPostBack="true" OnTextChanged="txtModel_TextChanged"></asp:TextBox>                                
                            </div>

                        </div>

                        <div class="row">

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                <label>Model No.</label>
                                <asp:TextBox class="form-control mb-2" ID="validationCustom06" runat="server" maxlength="30" placeholder="Model No."
                                    AutoPostBack="true" OnTextChanged="txtModel_TextChanged"></asp:TextBox>                                
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                <label>Serial No.</label>
                                <asp:TextBox class="form-control mb-2" ID="txtSerialNo" CssClass="form-control mb-2" runat="server" placeholder="Serial No."
                                    AutoPostBack="true" OnTextChanged="txtModel_TextChanged" MaxLength="20"></asp:TextBox>
                            </div>
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                <label>IMEI No.</label>
                                <asp:TextBox class="form-control mb-2" id="txtimeiNo" maxlength="15" onblur="validateIMEI()" CssClass="form-control mb-2" runat="server" placeholder="IMEI No."
                                    AutoPostBack="true" OnTextChanged="txtModel_TextChanged"></asp:TextBox>
                                <small id="imeiError" style="color: red; display: block;"></small>

                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12">
                                <label>Product Installation Date</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtDateOfImpl" runat="server" CssClass="form-control"
                                        AutoPostBack="true" AutoCompleteType="Disabled" AutoComplete="off" OnTextChanged="txtModel_TextChanged"></asp:TextBox>
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
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-12">

                                <div class="row">
                                    <div class="col-xl-4 col-lg-4 col-md-1 col-sm-4 col-12 ">
                                        <label>Years</label>
                                        <asp:TextBox  class="form-control mb-2" id="validationCustom09" runat="server" placeholder="Years" required max="10" min="1"
                                            oninput="validateYears()" AutoPostBack="true" OnTextChanged="txtModel_TextChanged" TextMode="Number"></asp:TextBox>                                        
                                    </div>
                                    <div class="col-xl-4 col-lg-4 col-md-1 col-sm-4 col-12 ">
                                        <label>Months</label>
                                        <asp:TextBox class="form-control mb-2" id="validationCustom010" runat="server" value="0" placeholder="Months" required max="12" min="0"
                                            oninput="validateMonths()" AutoPostBack="true" OnTextChanged="txtModel_TextChanged" TextMode="Number"></asp:TextBox>                                        
                                    </div>
                                    <div class="col-xl-4 col-lg-4 col-md-1 col-sm-4 col-12 ">
                                        <label>Days</label>
                                        <asp:TextBox class="form-control mb-2" id="validationCustom011" runat="server" value="0" placeholder="Days" required max="31" min="0"
                                            oninput="validateDays()" AutoPostBack="true" OnTextChanged="txtModel_TextChanged" TextMode="Number"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <label>Manufacturer Warranty</label>

                            </div>

                        </div>


                        <div class="row">



                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                <label>Customer Email ID</label>
                                <asp:TextBox class="form-control mb-2" ID="txtCustomerEmail" maxlength="50" runat="server" placeholder="Customer Email ID" required
                                    AutoPostBack="true" OnTextChanged="txtModel_TextChanged" TextMode="Email"></asp:TextBox>                                
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                <label>Customer Mobile No.</label>
                                <asp:TextBox  class="form-control mb-2" ID="txtCustomerMobile" runat="server" placeholder="Customer Mobile No." required
                                    pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNo()" AutoPostBack="true" OnTextChanged="txtModel_TextChanged"></asp:TextBox>                                
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 mt-4">
                                <asp:Button ID="btnSubmitPlan" class="btn btn-primary" OnClick="SubmitPlanInfo" runat="server" Text="Submit" />
                            </div>
                        </div>

                       

                        <div id="OTPPanel" class="row" runat="server" visible="false">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-12" style="text-align: center; display: flex;">
                                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 mb-4"></div>
                                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 mb-4">
                                    <label>OTP</label>
                                    <input type="text" class="form-control mb-2" id="txtOTP" runat="server" placeholder="Enter OTP" required
                                        maxlength="6" pattern="\d{6}" title="Enter a 6-digit OTP" oninput="validateOTP(this)">
                                </div>
                            </div>
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-12" style="text-align: center;">
                                <asp:Button class="btn btn-primary" OnClick="SubmitOTP" runat="server" AutoPostBack="false" Text="Submit OTP" />
                            </div>
                        </div>
                        <div id="PlanPanel" runat="server">
                            <div class="row"></div>
                            <h3 style="text-align: center;">Choose the Best Plan for Your Product
                            </h3>
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
                                        <div class="col-md-4 mb-4">
                                            <div class="card h-100">
                                                <div class="card-body">
                                                    <div class="form-check mb-2">
                                                        <asp:CheckBox ID="chkSelect" runat="server" CssClass="form-check-input planCheckBox"
                                                            AutoPostBack="true"
                                                            OnCheckedChanged="chkSelect_CheckedChanged"
                                                            CommandArgument='<%# Eval("Mid") %>' />
                                                        <label class="form-check-label">Select</label>
                                                    </div>

                                                    <asp:HiddenField ID="hdnPlanPrice" runat="server" Value='<%# Eval("CustPriceINR") %>' />
                                                    <asp:HiddenField ID="hdnSKU" runat="server" Value='<%# Eval("SKU") %>' />

                                                    <h5 class="card-title">
                                                        <asp:Label ID="lblPlanName" runat="server" Text='<%# Eval("PlanNickName") %>'></asp:Label>
                                                    </h5>
                                                    <ul class="list-group list-group-flush">
                                                        <li class="list-group-item"><strong>EW:</strong> <%# Eval("EW") %> <strong>SDP:</strong>  <%# Eval("SDP") %>  <strong>ADP:</strong> <%# Eval("ADP") %></li>
                                                        <li class="list-group-item"><strong>Plan Price:</strong> ₹<%# Eval("CustPriceINR") %></li>

                                                    </ul>
                                                </div>
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
                        <div id="ApplyPromoCodePanel" runat="server">
                            <div class="row">
                                <div class="col-xl-2 col-lg-2 col-md-2 col-sm-2 col-2 ">
                                </div>
                                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                    <input type="text" class="form-control" id="txtPromoDiscount" runat="server" placeholder="Apply Your Promo Code">
                                </div>
                                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                    <button class="btn btn-primary" type="submit">Apply</button>
                                </div>

                            </div>
                            <div class="form-row">
                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-12" style="text-align: center;">
                                    <label for="validationCustom03" style="width: 35%;">
                                        By paying just an additional ₹1600, you can upgrade to the
                                    2 Years Extended Warranty plan!
                                    </label>
                                </div>
                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                                    <div class="form-group">
                                        <div class="form-check">
                                            <input class="form-check-input" type="checkbox" value="" id="invalidCheck" required>
                                            <label class="form-check-label" for="invalidCheck">
                                                By proceeding, you agree to the Terms and Conditions
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 " style="text-align: center;">
                                    <asp:Button ID="btnSubmitMain" class="btn btn-primary" runat="server" AutoPostBack="true" OnClick="AddToCart" Text="Add to Cart" />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <!-- ============================================================== -->
            <!-- end validation form -->
            <!-- ============================================================== -->
        </div>

    </div>

</asp:Content>
