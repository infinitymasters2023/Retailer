<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CartDetails.aspx.cs" Inherits="Patner_Retailer_ADO.CartDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/js/intlTelInput.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/css/intlTelInput.css">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

    <script type="text/javascript">
        window.onload = function () {
            // Get elements by their ASP.NET ClientID
            const mobileCodeInput = document.getElementById('<%= txtCustomerMobileNo.ClientID %>');
          const altMobileCodeInput = document.getElementById('<%= txtAlternativeMobile.ClientID %>');
          const waahatsMobileCodeInput = document.getElementById('<%= txtWhatsappNo.ClientID %>');

          const countryCodeHidden = document.getElementById('<%= hdnCountryCode.ClientID %>');
          const phoneNumberHidden = document.getElementById('<%= hdnPhoneNumber.ClientID %>');
          const altCountryCodeHidden = document.getElementById('<%= hdnAltCountryCode.ClientID %>');
          const altPhoneNumberHidden = document.getElementById('<%= hdnAltMobile.ClientID %>');
          const whatsCountryCodeHidden = document.getElementById('<%= hdnWhatsCountryCode.ClientID %>');
          const whatsPhoneNumberHidden = document.getElementById('<%= hdnWhatsMobile.ClientID %>');

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

            if (altMobileCodeInput && whatsCountryCodeHidden && altPhoneNumberHidden) {
                const itiAltMobile = window.intlTelInput(altMobileCodeInput, {
                    initialCountry: "in",
                    separateDialCode: true,
                    formatOnDisplay: false,
                    utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/js/utils.js",
                });

                function updateAltHiddenFields() {
                    const dialCode = itiAltMobile.getSelectedCountryData().dialCode;
                    let nationalNumber = itiAltMobile.getNumber(intlTelInputUtils.numberFormat.NATIONAL).replace(/\D/g, '');
                    if (nationalNumber.startsWith('0')) {
                        nationalNumber = nationalNumber.substring(1);
                    }
                    altCountryCodeHidden.value = dialCode;
                    altPhoneNumberHidden.value = nationalNumber;
                }

                altMobileCodeInput.addEventListener('countrychange', updateAltHiddenFields);
                altMobileCodeInput.addEventListener('blur', updateAltHiddenFields);
            }

            if (waahatsMobileCodeInput && whatsCountryCodeHidden && whatsPhoneNumberHidden) {
                const itiWhatsMobile = window.intlTelInput(waahatsMobileCodeInput, {
                    initialCountry: "in",
                    separateDialCode: true,
                    formatOnDisplay: false,
                    utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/js/utils.js",
                });

                function updateWhatsHiddenFields() {
                    const dialCode = itiWhatsMobile.getSelectedCountryData().dialCode;
                    let nationalNumber = itiWhatsMobile.getNumber(intlTelInputUtils.numberFormat.NATIONAL).replace(/\D/g, '');
                    if (nationalNumber.startsWith('0')) {
                        nationalNumber = nationalNumber.substring(1);
                    }
                    whatsCountryCodeHidden.value = dialCode;
                    whatsPhoneNumberHidden.value = nationalNumber;
                }

                waahatsMobileCodeInput.addEventListener('countrychange', updateWhatsHiddenFields);
                waahatsMobileCodeInput.addEventListener('blur', updateWhatsHiddenFields);
            }
        };
    </script>

    <style>
        .prev-step, .next-step {
            font-size: 14px;
            padding: 7px 10px;
            border: none;
            border-radius: 5px;
            margin-top: 28px;
            color: #fff;
        }
        .order-calcs {
            font-size: 13px;
        }
        .iti {
            width: 100%;
        }
        .chkWhatsAppcls label {
            font-size: 12px;
            margin-bottom:0px;
        }
          .Error-Message {
              font-size: 12px !important;
              color: red;
              display: none;
              font-weight: 400 !important;
              margin-bottom: 0px;
          }
          .ddlPlanCls{
              font-size: 12px !important;
          }
    </style>
    <script>
        function validatePincode(input) {
            input.value = input.value.replace(/\D/g, '');
            if (input.value.length > 6) {
                input.value = input.value.slice(0, 6);
            }
        }
        function validateMobileNumber(input) {
            input.value = input.value.replace(/[^\d]/g, '').slice(0, 10);
        }

        function validateGroupFields() {
            const fields = document.querySelectorAll('.validate-group');
            let allValid = true;

            for (let i = 0; i < fields.length; i++) {
                if (!fields[i].checkValidity()) {
                    fields[i].reportValidity();
                    allValid = false;
                    break;
                }
            }
            return allValid;
        }

        function copyAddressIfChecked(checkbox) {
            const corresPincode = document.getElementById('<%= txtPincode.ClientID %>');
            const corresCity = document.getElementById('<%= txtCity.ClientID %>');
            const corresState = document.getElementById('<%= txtState.ClientID %>');
            const corresAddress = document.getElementById('<%= txtAddressLine1.ClientID %>');
            const corresLandmark = document.getElementById('<%= txtLandmark.ClientID %>');

            const installPincode = document.getElementById('<%= txtProductInstalledPincode.ClientID %>');
            const installCity = document.getElementById('<%= txtProductInstalledCity.ClientID %>');
            const installState = document.getElementById('<%= txtProductInstalledState.ClientID %>');
            const installAddress = document.getElementById('<%= txtAvaility.ClientID %>');
            const installLandmark = document.getElementById('<%= txtinstalledLandmark.ClientID %>');

            if (checkbox.checked) {
                installPincode.value = corresPincode.value;
                installCity.value = corresCity.value;
                installState.value = corresState.value;
                installAddress.value = corresAddress.value;
                installLandmark.value = corresLandmark.value;
            } else {
                installPincode.value = '';
                installCity.value = '';
                installState.value = '';
                installAddress.value = '';
                installLandmark.value = '';
            }
        }


        function validateAddressWords() {
            const addressInput = document.getElementById('<%= txtAddressLine1.ClientID %>');
            const errorDiv = document.getElementById('addressError');
            const wordCount = addressInput.value.trim().split(/\s+/).filter(w => w).length;

            if (wordCount < 3) {
                errorDiv.textContent = "Address must contain at least 3 words.";
            } else {
                errorDiv.textContent = "";
            }
        }

        function validateLandmarkWords() {
            const landmarkInput = document.getElementById('<%= txtLandmark.ClientID %>');
            const errorDiv = document.getElementById('landmarkError');
            const value = landmarkInput.value.trim();

            if (value.length < 3) {
                errorDiv.textContent = "Landmark must contain at least 3 characters.";
            } else {
                errorDiv.textContent = "";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid px-0 ">

        <div class="row">
            <div class="col-12 col-lg-8">
                <div class="card mb-3 mb-lg-0">
                    <%-- <h5 class="card-header">Cart Details</h5>--%>
                    <div class="card-body">
                        <h4>Cart Details</h4>
                        <div class="table-responsive">
                        <asp:Repeater ID="rptPlans" runat="server">
                            <HeaderTemplate>
                                <table class="table table-striped table-bordered" style="white-space:nowrap">
                                    <thead>
                                        <tr>
                                            <th>Product Name</th>
                                            <th>Plan Selected</th>
                                             <th>Product Price</th>
                                            <th>Quantity</th>
                                            <th>Value</th>
                                            <th>Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPlanName" runat="server" Text='<%# Eval("Productname") %>'></asp:Label>
                                    </td>
                                  
                                    <td>
                                        <asp:Label ID="lblSelectedPlan" runat="server" Text='<%# Eval("PlanNicknameSelection") %>'></asp:Label>
                                        <asp:DropDownList ID="ddlPlan" runat="server" CssClass="form-control ddlPlanCls" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlPlanChange" Visible="false"></asp:DropDownList>
                                    </td>
                                      <td>
                                          <asp:Label ID="lblDevicePurchasePrice" runat="server" Text='<%# Eval("DevicePurchasePrice") %>'></asp:Label>
                                      </td>
                                    <td>
                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>

                                    </td>
                                    <td>
                                        <asp:Label ID="lblPlanPrice" runat="server" Text='<%# Eval("PlanPrice") %>'></asp:Label>
                                        <asp:HiddenField ID="hdnUID" Value='<%#Eval("UID") %>' runat="server" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnlEdit" runat="server" ToolTip="Edit Plan" CommandName="Edit" CommandArgument='<%# Eval("Mid") + "|" + Eval("cartItemId") + "|" + Eval("UID") %>' OnClick="EditPlanInfo" CssClass="text-primary">
                                            <i class="fa fa-edit"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkBack" runat="server" ToolTip="Back" Visible="false" CommandName="Back" CommandArgument='<%# Eval("Mid") + "|" + Eval("cartItemId") + "|" + Eval("UID") %>' OnClick="CancelPlanInfo" CssClass="text-primary">
                                            <i class="fas fa-arrow-left"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkSave" runat="server" ToolTip="Save Plan" Visible="false" CommandName="Save" CommandArgument='<%# Eval("Mid") + "|" + Eval("cartItemId") + "|" + Eval("UID") %>' OnClick="SavePlanInfo" CssClass="text-primary">
                                            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" style="height: 15px;fill: #5969ff;margin: -2px;">
                                                <path d="M160 96C124.7 96 96 124.7 96 160L96 480C96 515.3 124.7 544 160 544L480 544C515.3 544 544 515.3 544 480L544 237.3C544 220.3 537.3 204 525.3 192L448 114.7C436 102.7 419.7 96 402.7 96L160 96zM192 192C192 174.3 206.3 160 224 160L384 160C401.7 160 416 174.3 416 192L416 256C416 273.7 401.7 288 384 288L224 288C206.3 288 192 273.7 192 256L192 192zM320 352C355.3 352 384 380.7 384 416C384 451.3 355.3 480 320 480C284.7 480 256 451.3 256 416C256 380.7 284.7 352 320 352z"/>
                                            </svg>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" ToolTip="Delete Plan" CommandName="Delete"  CommandArgument='<%# Eval("Mid") + "|" + Eval("cartItemId") + "|" + Eval("UniqueID") %>'  OnClick="DeletePlanInfo" CssClass="text-danger">
                                          <i class="fa fa-trash"></i>
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>

                            <FooterTemplate>
                                </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        </div>
                        <button id="btnAddMore" runat="server" class="btn next-step mt-3"
                            causesvalidation="false" usesubmitbehavior="true" onserverclick="AddMoreProducts">
                            <i class="fa fa-plus">&nbsp; </i>Add More Products
                        </button>
                        <%--   <asp:Button ID="btnAddMore" runat="server" class="btn btn-primary" Text="Add More Products" OnClick="AddMoreProducts"
                          CausesValidation="false" UseSubmitBehavior="true" OnClientClick="this.form.noValidate = true;" />
                          <i class="fa fa-plus"></i>           --%>
                    </div>
                </div>

                <div class="row mt-3">
    <!-- ============================================================== -->
    <!-- validation form -->
    <!-- ============================================================== -->
    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
        <div class="card">
            <h5 class="card-header">Customer Details</h5>
            <div class="card-body">
                <div class="row">
                    <div class="col-12 col-lg-6 mb-3">
                        <label for="validationCustom01">Full Name <span class="text-danger">*</span></label>
                        <asp:TextBox CssClass="form-control validate-group" ID="txtFirstName" runat="server" placeholder="First Name" MaxLength="50" Enabled="false" AutoComplete="off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                            ErrorMessage="First Name is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                    </div>

                    <%-- <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                        <label for="validationCustom01">Last Name</label>
                        <asp:TextBox CssClass="form-control validate-group" ID="txtLastName" runat="server" placeholder="Last Name" MaxLength="50"> </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                            ErrorMessage="Last Name is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                    </div>--%>
                    <div class="col-12 col-lg-6 mb-3">
                        <label for="validationCustom01">Customer Resistered Mobile No. <span class="text-danger">*</span></label>
                        <asp:TextBox CssClass="form-control validate-group" ID="txtCustomerMobileNo" runat="server" placeholder="Customer Resistered Mobile No." Enabled="false" name="txtCustomerMobileNo"
                            pattern="\d{10}" title="Please enter a valid 10-digit mobile number" TextMode="Number" oninput="validateMobileNumber(this)" MaxLength="10" AutoComplete="off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCustomerMobile" runat="server" ControlToValidate="txtCustomerMobileNo"
                            ErrorMessage="Mobile number is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                        <asp:RegularExpressionValidator ID="revCustomerMobile" runat="server" ControlToValidate="txtCustomerMobileNo" ValidationExpression="^[6-9]\d{9}$"
                            ErrorMessage="Enter valid 10-digit mobile number" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                        <asp:HiddenField ID="hdnCountryCode" runat="server" />
                        <asp:HiddenField ID="hdnPhoneNumber" runat="server" />
                        <asp:CheckBox ID="chkWhatsAppNo" CssClass="chkWhatsAppcls" Font-Size="12px" runat="server" Text="This Number is on WhatsApp" AutoPostBack="true" OnCheckedChanged="chkWhatsAppChange" />
                    </div>
                    <div class="col-12 col-lg-6 mb-3">
                        <label for="validationCustom01">Alternative Mobile No.</label>
                        <asp:TextBox CssClass="form-control validate-group" ID="txtAlternativeMobile" runat="server" placeholder="Alternative Mobile No." AutoComplete="off" name="txtAlternativeMobile"
                            pattern="\d{10}" title="Please enter a valid 10-digit mobile number" TextMode="Number" oninput="validateMobileNumber(this)" MaxLength="10"
                            AutoPostBack="true" OnTextChanged="txtAltCustomerMobile_TextChanged"> </asp:TextBox>
                        <asp:RegularExpressionValidator ID="revAltMobile" runat="server" ControlToValidate="txtAlternativeMobile" ValidationExpression="^[6-9]\d{9}$"
                            ErrorMessage="Enter valid 10-digit mobile number" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                        <asp:HiddenField ID="hdnAltCountryCode" runat="server" />
                        <asp:HiddenField ID="hdnAltMobile" runat="server" />
                        <p id="lblAltMobileNo" runat="server" class="Error-Message">Mobile No is required.</p>
                    </div>
                    <div class="col-12 col-lg-6 mb-1">
                        <label for="validationCustom01">WhatsApp Number <span class="text-danger">*</span></label>
                        <asp:TextBox CssClass="form-control validate-group" ID="txtWhatsappNo" runat="server" placeholder="WhatsApp Number" TextMode="Number" name="txtWhatsappNo"
                            AutoComplete="off" pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" MaxLength="10"
                            AutoPostBack="true" OnTextChanged="txtWhatsCustomerMobile_TextChanged"> </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvWhatsapp" runat="server" ControlToValidate="txtWhatsappNo"
                            ErrorMessage="WhatsApp number is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                        <asp:RegularExpressionValidator ID="revWhatsapp" runat="server" ControlToValidate="txtWhatsappNo" ValidationExpression="^[6-9]\d{9}$"
                            ErrorMessage="Enter valid 10-digit number" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                        <asp:HiddenField ID="hdnWhatsCountryCode" runat="server" />
                        <asp:HiddenField ID="hdnWhatsMobile" runat="server" />
                        <p id="lblWhatsAppError" runat="server" class="Error-Message">Mobile No is required.</p>
                    </div>
                </div>

                <div class="row">
                    
                    <div class="col-12 col-lg-6 mb-3">
                        <label for="validationCustom01">Customer Registered Email <span class="text-danger">*</span></label>
                        <asp:TextBox CssClass="form-control validate-group text-lowercase" ID="txtEmail" runat="server" placeholder="Customer Registered Email" TextMode="Email" MaxLength="50" Enabled="false" AutoComplete="off"
                            AutoPostBack="true" OnTextChanged="txtCustomerEmail_TextChanged"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            ErrorMessage="Invalid email format" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                        <p id="lblEmailAddress" runat="server" class="Error-Message">Email ID is required.</p>
                    </div>
                    <div class="col-12 col-lg-6 mb-3">
                        <label for="validationCustom01">Alternative Email</label>
                        <asp:TextBox CssClass="form-control validate-group text-lowercase" ID="txtAlternativeEmail" runat="server" placeholder="Alternative Email" TextMode="Email" MaxLength="50" AutoComplete="off"
                                AutoPostBack="true" OnTextChanged="txtAltEmail_TextChanged"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revAltEmail" runat="server" ControlToValidate="txtAlternativeEmail"
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Invalid email format" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                        <p id="lblAtlEmail" runat="server" class="Error-Message">Email ID is required.</p>
                    </div>

                    <div class="col-12 col-lg-4 mb-3">
                        <label for="validationCustom01">Pincode <span class="text-danger">*</span></label>
                        <asp:TextBox CssClass="form-control validate-group" ID="txtPincode" runat="server" placeholder="Pincode" MaxLength="6" pattern="\d{6}" AutoComplete="off"
                            title="Enter a 6-digit Pincode" oninput="validatePincode(this)" AutoPostBack="true" OnTextChanged="txtPIN_TextChanged"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPincode" runat="server" ControlToValidate="txtPincode" ErrorMessage="Pincode is required" CssClass="text-danger"
                            Display="Dynamic" ValidationGroup="CustomerDetails" />
                        <asp:RegularExpressionValidator ID="revPincode" runat="server" ControlToValidate="txtPincode" ValidationExpression="^\d{6}$"
                            ErrorMessage="Enter a valid 6-digit pincode" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                        <asp:Label ID="lblPincodeError" runat="server" CssClass="text-danger" Font-Size="12px" ForeColor="Red" Visible="false"></asp:Label>
                    </div>

                    <div class="col-12 col-lg-4 mb-3">
                        <label for="validationCustom01">City <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtCity" runat="server" Enabled="false" placeholder="City" CssClass="form-control validate-group mb-2" MaxLength="20" AutoComplete="off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity" ErrorMessage="City is required" CssClass="text-danger"
                            Display="Dynamic" ValidationGroup="CustomerDetails" />
                    </div>

                    <div class="col-12 col-lg-4 mb-3">
                        <label for="validationCustom01">State <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtState" runat="server" Enabled="false" placeholder="State" CssClass="form-control validate-group mb-2" MaxLength="20" AutoComplete="off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="txtState" ErrorMessage="State is required" CssClass="text-danger"
                            Display="Dynamic" ValidationGroup="CustomerDetails" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-xl-6 col-lg-6 col-12 mb-3">
                        <label for="validationCustom01">Correspondance Address <span class="text-danger">*</span></label>
                        <textarea class="form-control validate-group" id="txtAddressLine1" runat="server" placeholder="Correspondance Address" maxlength="150" oninput="validateAddressWords()" autocomplete="off" rows="2"></textarea>
                        <span id="addressError" style="color: red; font-size: 12px;"></span>
                        <asp:Label ID="lblAddressError" runat="server" ForeColor="Red" Visible="false" />
                        <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddressLine1" ErrorMessage="Address is required"
                            CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                    </div>
                
                    <div class="col-xl-6 col-lg-6 col-12 mb-3">
                        <label for="validationCustom01">Landmark <span class="text-danger">*</span></label>
                        <textarea class="form-control validate-group" id="txtLandmark" runat="server" placeholder="Landmark" maxlength="50" oninput="validateLandmarkWords()" autocomplete="off" rows="2"></textarea>
                        <span id="landmarkError" style="color: red; font-size: 12px;"></span>
                        <asp:Label ID="lblLandmarkError" runat="server" ForeColor="Red" Visible="false" />
                        <asp:RequiredFieldValidator ID="rfvLandmark" runat="server" ControlToValidate="txtLandmark" ErrorMessage="Landmark is required"
                            CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <div class="form-group">
                            <div class="form-check">
                                <input class="form-check-input" type="checkbox" value="" id="invalidCheck" onchange="copyAddressIfChecked(this)">
                                <label class="form-check-label" for="invalidCheck">
                                    Same as Correspondance Address
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12 col-lg-4 mb-3">
                        <label for="validationCustom01">Product Installed Pincode <span class="text-danger">*</span></label>
                        <asp:TextBox CssClass="form-control validate-group" ID="txtProductInstalledPincode" runat="server" placeholder="Pincode" MaxLength="6" pattern="\d{6}" AutoComplete="off"
                            title="Enter a 6-digit Pincode" oninput="validatePincode(this)" AutoPostBack="true" OnTextChanged="txtInstalledPIN_TextChanged">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtProductInstalledPincode" ErrorMessage=" Product InstalledPincode is required" CssClass="text-danger"
                            Display="Dynamic" ValidationGroup="CustomerDetails" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtProductInstalledPincode" ValidationExpression="^\d{6}$"
                            ErrorMessage="Enter a valid 6-digit pincode" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                        <asp:Label ID="lblProductInstalledPincodeError" runat="server" CssClass="text-danger" Font-Size="12px" ForeColor="Red" Visible="false"></asp:Label>
                    </div>

                    <div class="col-12 col-lg-4 mb-3">
                        <label for="validationCustom01">Product Installed City <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtProductInstalledCity" runat="server" Enabled="false" placeholder="City" CssClass="form-control validate-group mb-2" MaxLength="20" AutoComplete="off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtProductInstalledCity" ErrorMessage="Product Installed City is required" CssClass="text-danger"
                            Display="Dynamic" ValidationGroup="CustomerDetails" />
                    </div>

                    <div class="col-12 col-lg-4 mb-3">
                        <label for="validationCustom01">Product Installed State <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtProductInstalledState" runat="server" Enabled="false" placeholder="State" CssClass="form-control validate-group mb-2" MaxLength="20" AutoComplete="off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtProductInstalledState" ErrorMessage="Product Installed State is required" CssClass="text-danger"
                            Display="Dynamic" ValidationGroup="CustomerDetails" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-xl-6 col-lg-6 col-6 mb-3">
                        <label for="validationCustom01">Product Installed Address</label>
                        <textarea class="form-control validate-group" id="txtAvaility" runat="server" placeholder="Product Installed Address" autocomplete="off"
                            maxlength="150" rows="2"> </textarea>
                    </div>
                    <div class="col-xl-6 col-lg-6 col-12 mb-3">
                        <label for="validationCustom01">Product Installed Landmark</label>
                        <textarea class="form-control validate-group" id="txtinstalledLandmark" runat="server" placeholder="Product Installed Landmark" autocomplete="off"
                            maxlength="50" rows="2"></textarea>
                    </div>
                </div>

                <div class="form-row">
                    <div class="col-xl-12 col-lg-12 col-12 text-center">
                        <asp:Button ID="btnContinuePayment" CssClass="btn next-step mt-0" runat="server" OnClick="ContinuePayment" Text="Continue To Payment"
                            OnClientClick="return validateGroupFields();" ValidationGroup="CustomerDetails" />
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

            <div class="col-12 col-lg-4 pl-lg-0">
                <div class="card bg-light mb-0" style="position:sticky; top:80px;">
                    <div class="card-header"><strong>Your Order Summary</strong></div>
                    <div class="card-body bg-white order-calcs">
                        <div class="d-flex justify-content-between align-items-center mb-2">
                            Total Item : 
                            <strong>
                                <p id="txtQuantity" runat="server"></p>
                            </strong>
                        </div>
                        <div class="d-flex justify-content-between align-items-center mb-2">
                            Total Value :
                            <strong>
                                <p id="TotalValueAmount" runat="server"></p>
                            </strong>
                            </div>
                        <div class="d-flex justify-content-between align-items-center mb-2 pl-2">
                            Taxable Value :
                            <strong>
                                <p id="TaxableValue" runat="server"></p>
                            </strong>
                        </div>
                        <div class="d-flex justify-content-between align-items-center mb-2 pl-2">
                            GST(18%) :
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
                            <strong><p id="P2" runat="server">0</p></strong>
                        </div>

                        <div class="d-flex justify-content-between align-items-center mb-2">
                            Total Amount (Incl. Tax) :
                            <strong>
                                <p id="TotalValue" runat="server">0</p>
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
                                    <p id="TotalCommisionValue" runat="server"></p>
                                </strong>
                            </div>
                            <div class="d-flex justify-content-between align-items-center mb-2 pl-2" style="color: green;">
                                <p class="mb-0" id="P3" runat="server">Taxable Value :</p>
                                <strong>
                                    <p id="CommisionValue" runat="server"></p>
                                </strong>
                            </div>
                            <div class="d-flex justify-content-between align-items-center mb-2 pl-2" style="color: green;">
                                <p class="mb-0" id="P5" runat="server">GST(18%) <span style="color: red">*</span> :</p>
                                <strong>
                                    <p id="ComminsionTax" runat="server"></p>
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
                <p class="py-2 px-3" style="font-size:12px">
                    <span style="color:red">*</span>
                    Retailer GST (18%) will be payable post Tax Invoice issued from Retailer
                </p>
                </div>
            </div>

        </div>
        

    </div>
</asp:Content>
