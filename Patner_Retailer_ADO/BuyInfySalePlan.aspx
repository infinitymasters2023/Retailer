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
                                <label>Product Purchase Price</label>
                                <asp:TextBox CssClass="form-control mb-2" ID="txtPrice" runat="server" placeholder="Product Purchase Price" MaxLength="10"
                                    oninput="validateDecimalInput(this)" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPrice" ErrorMessage="Product Purchase Price is required."
                                    ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12">
                                <label>Product Purchase Date</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtPurchaseDate" runat="server" CssClass="form-control"
                                        AutoCompleteType="Disabled" AutoComplete="off" ></asp:TextBox>
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
                                    ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                            </div>


                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                <label>Brand</label>
                                <div class="custom-select-with-caret">
                                    <asp:DropDownList ID="ddlBrand" runat="server" Enabled="true" placeholder="Brand"
                                       CssClass="form-control input-sm mb-2">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlBrand" ErrorMessage="Brand is required."
                                        InitialValue="0" ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                                </div>
                                <asp:TextBox ID="txtMake" runat="server" Visible="false" placeholder="Make" CssClass="form-control input-sm mb-2"></asp:TextBox>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                <label>Model</label>
                                <asp:TextBox CssClass="form-control mb-2" ID="txtModel" runat="server" placeholder="Model" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtModel" ErrorMessage="Model is required."
                                    ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                            </div>

                        </div>

                        <div class="row">

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                <label>Model No.</label>
                                <asp:TextBox CssClass="form-control mb-2" ID="validationCustom06" runat="server" MaxLength="30" placeholder="Model No."></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="validationCustom06" ErrorMessage="Model No is required."
                                    ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                <label>Serial No.</label>
                                <asp:TextBox class="form-control mb-2" ID="txtSerialNo" CssClass="form-control mb-2" runat="server" placeholder="Serial No."
                                    MaxLength="20"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvSerialNo" runat="server" ControlToValidate="txtSerialNo"
                                    ErrorMessage="Serial No. is required." ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo" />
                            </div>
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                <label>IMEI No.</label>
                                <asp:TextBox CssClass="form-control mb-2" ID="txtimeiNo" MaxLength="15" onblur="validateIMEI()" runat="server" placeholder="IMEI No."></asp:TextBox>
                                <small id="imeiError" style="color: red; display: block;"></small>
                                <asp:RequiredFieldValidator ID="rfvIMEINo" runat="server" ControlToValidate="txtimeiNo"
                                    ErrorMessage="IMEI No. is required." ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo" />
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12">
                                <label>Product Installation Date</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtDateOfImpl" runat="server" CssClass="form-control"
                                        AutoCompleteType="Disabled" AutoComplete="off" ></asp:TextBox>
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
                                <asp:RequiredFieldValidator ID="rfvDateOfImpl" runat="server" ControlToValidate="txtDateOfImpl" ErrorMessage="Product Installation Date is required."
                                    ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"> </asp:RequiredFieldValidator>                                
                            </div>
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-12 col-12">
                                <label>Manufacturer Warranty</label>
                                <div class="row">
                                    <div class="col-xl-4 col-lg-4 col-md-1 col-sm-4 col-12">
                                        <asp:TextBox CssClass="form-control position-relative mb-2" ID="validationCustom09" runat="server" placeholder="Years" max="10" min="1"
                                            oninput="validateYears()" TextMode="Number"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="validationCustom09" ErrorMessage="Years is required."
                                            CssClass="error-message" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-xl-4 col-lg-4 col-md-1 col-sm-4 col-12 ">
                                        <asp:TextBox CssClass="form-control mb-2" ID="validationCustom010" runat="server" value="0" placeholder="Months" max="12" min="0"
                                            oninput="validateMonths()" TextMode="Number"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="validationCustom010" ErrorMessage="Months is required."
                                            ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-xl-4 col-lg-4 col-md-1 col-sm-4 col-12 ">
                                        <asp:TextBox CssClass="form-control mb-2" ID="validationCustom011" runat="server" value="0" placeholder="Days" max="31" min="0"
                                            oninput="validateDays()" TextMode="Number"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="validationCustom011" ErrorMessage="Days is required."
                                            ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                

                            </div>

                        </div>


                        <div class="row">



                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                <label>Customer Email ID</label>
                                <asp:TextBox CssClass="form-control mb-2" ID="txtCustomerEmail" MaxLength="50" runat="server" placeholder="Customer Email ID"
                                    TextMode="Email"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtCustomerEmail" ErrorMessage="Customer Email is required."
                                    ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12 ">
                                <label>Customer Mobile No.</label>
                                <asp:TextBox CssClass="form-control mb-2" ID="txtCustomerMobile" runat="server" placeholder="Customer Mobile No."
                                    pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNo()"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtCustomerMobile" ErrorMessage="Customer Mobile is required."
                                    ForeColor="Red" Display="Dynamic" ValidationGroup="ProductInfo"></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 mt-4">
                                <asp:Button ID="btnSubmitPlan" class="btn btn-primary" OnClick="SubmitPlanInfo" ValidationGroup="ProductInfo" runat="server" Text="Submit" />
                                <asp:Button ID="btnEditPlan" class="btn btn-primary" OnClick="EditPlanInfo" runat="server" Text="Edit" />
                            </div>
                        </div>



                        <div id="OTPPanel" class="row" runat="server" visible="false">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-12" style="text-align: center; display: flex;">
                                <div class="col-xl-5 col-lg-5 col-md-5 col-sm-5 col-12 mb-5"></div>
                                <div class="col-xl-2 col-lg-2 col-md-2 col-sm-2 col-12 mb-2">
                                    <label>Enter OTP</label>
                                    <input type="text" class="form-control mb-2" id="txtOTP" runat="server" placeholder="Enter OTP"
                                        maxlength="6" pattern="\d{6}" title="Enter a 6-digit OTP" oninput="validateOTP(this)">
                                     <asp:RequiredFieldValidator ID="refOTP" runat="server" ControlToValidate="txtOTP" ErrorMessage="OTP is required."
                                         ForeColor="Red" Display="Dynamic" ValidationGroup="OTPSubmission"></asp:RequiredFieldValidator>
                                    <span>
                                        <asp:LinkButton ID="lnkResendOTP" runat="server" CssClass="resend-otp" OnClick="lnkResendOTP_Click">Resend OTP</asp:LinkButton></span>

                                </div>
                            </div>
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-12" style="text-align: center;">
                            </div>
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-12" style="text-align: center;">
                                <asp:Button class="btn btn-primary" ID="btnSubmitOTP" OnClick="SubmitOTP" runat="server" AutoPostBack="false" Text="Submit OTP" ValidationGroup="OTPSubmission" />
                            </div>
                            <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />

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
                                                    <div class="price-txt"> Price:</div> 
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
                                <%--  <table class="table table-bordered table-striped">
                                    <thead class="thead-dark">
                                        <tr>
                                            <th>Select</th>
                                            <th>Plan Name</th>
                                            <th>1</th>
                                            <th>2</th>
                                            <th>3</th>
                                            <th>4</th>
                                            <th>5</th>
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
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPlanName" runat="server" Text='<%# Eval("PlanNickName") %>'></asp:Label>
                                                    </td>
                                                    <td>P</td>
                                                    <td>P</td>
                                                    <td>E</td>
                                                    <td>E</td>
                                                    <td></td>                                                    
                                                    <td><%# Eval("CustPriceINR") %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>--%>
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
                                    <asp:Button ID="btnApplyPromoCode" class="btn btn-primary" runat="server" AutoPostBack="true" OnClick="ApplyPromoCode" Text="Apply" />
                                </div>

                            </div>
                            <div runat="server" id="calculationdiv" visible="false">
                                <div class="row mt-3" style="text-align: right;">
                                    <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12"></div>
                                    <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12"></div>
                                    <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12">
                                        <label>Plan Amount : </label>
                                        <strong>
                                            <asp:Label ID="lblPlanPrice" runat="server"></asp:Label></strong>
                                    </div>
                                </div>
                                <div class="row mt-3" style="text-align: right;">
                                    <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12"></div>
                                    <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12"></div>
                                    <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12">
                                        <label>Promo Discount Amount : </label>
                                        <strong>
                                            <asp:Label ID="lblDiscountAmount" runat="server"></asp:Label></strong>
                                    </div>
                                </div>
                                <hr />
                                <div class="row mt-3" style="text-align: right;">
                                    <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12"></div>
                                    <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12"></div>
                                    <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-12">
                                        <label>Total Amount : </label>
                                        <strong>
                                            <asp:Label ID="lblTotalAmount" runat="server"></asp:Label></strong>
                                    </div>
                                </div>
                                <hr />
                            </div>
                            <div class="form-row">
                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-12" style="text-align: center;">
                                  <%--  <label for="validationCustom03" style="width: 35%;">
                                        By paying just an additional ₹1600, you can upgrade to the
                                    2 Years Extended Warranty plan!
                                    </label>--%>
                                </div>
                                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                                    <div class="form-group">
                                        <div class="form-check">
                                            <input class="form-check-input" type="checkbox" value="" id="invalidCheck" runat="server" required>
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
           
            <!-- ============================================================== -->
            <!-- end validation form -->
            <!-- ============================================================== -->
      

</asp:Content>
