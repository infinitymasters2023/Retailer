<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CreateSalesPerson.aspx.cs" Inherits="Patner_Retailer_ADO.CreateSalesPerson" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">



    <script src="https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/js/intlTelInput.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/css/intlTelInput.css">

    <script type="text/javascript">
        window.onload = function () {
            // Get elements by their ASP.NET ClientID
            const mobileCodeInput = document.getElementById('<%= txtMobile.ClientID %>');
            const altMobileCodeInput = document.getElementById('<%= txtAltMobile.ClientID %>');

            const countryCodeHidden = document.getElementById('<%= hdnCountryCode.ClientID %>');
            const phoneNumberHidden = document.getElementById('<%= hdnPhoneNumber.ClientID %>');

            const altCountryCodeHidden = document.getElementById('<%= hdnAltCountryCode.ClientID %>');
            const altPhoneNumberHidden = document.getElementById('<%= hdnAltMobile.ClientID %>');

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

            if (altMobileCodeInput && altCountryCodeHidden && altPhoneNumberHidden) {
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
        };
    </script>

    <script>
        function validateMobileNumber(input) {
            input.value = input.value.replace(/[^\d]/g, '').slice(0, 10);
        }

        function validatePincode(input) {
            input.value = input.value.replace(/\D/g, '');
            if (input.value.length > 6) {
                input.value = input.value.slice(0, 6);
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            const accountInput = document.getElementById('<%= txtAccount.ClientID %>');
            accountInput.addEventListener('input', function () {
                this.value = this.value.replace(/\D/g, '');
            });
        });
        document.addEventListener('DOMContentLoaded', function () {
            const accountInput = document.getElementById('<%= txtConfirmAccount.ClientID %>');
            accountInput.addEventListener('input', function () {
                this.value = this.value.replace(/\D/g, '');
            });
        });
    </script>
    <script>
        window.addEventListener('DOMContentLoaded', function () {
            var ifsc = document.getElementById('<%= txtIFSC.ClientID %>');
            if (ifsc) {
                ifsc.addEventListener('input', function () {
                    this.value = this.value.toUpperCase();
                });
            }
        });
    </script>


    <style>
        .head-title {
            border-bottom: 1px solid #e6e6f2;
            padding-bottom: 10px;
        }

        .selectWarranty {
            background: blue !important;
            color: white !important;
        }

        .same-small-txt label {
            margin-left: 5px;
            font-size: 13px !important;
            font-weight: 400 !important;
        }
    </style>
    <%--<script>
        document.addEventListener("DOMContentLoaded", function () {
            var txtDoc = document.getElementById('<%= txtDocumentNumber.ClientID %>');
            var hdnDoc = document.getElementById('<%= hdnDocumentNumber.ClientID %>');

            txtDoc.addEventListener('input', function () {
                hdnDoc.value = txtDoc.value;
            });

            txtDoc.addEventListener('blur', function () {
                var val = txtDoc.value;
                if (val.length > 4) {
                    hdnDoc.value = val;
                    var last4 = val.slice(-4);
                    var masked = '*'.repeat(val.length - 4) + last4;
                    txtDoc.value = masked;
                }
            });

            txtDoc.addEventListener('focus', function () {
                if (hdnDoc.value) {
                    txtDoc.value = hdnDoc.value;
                }
            });

            var btnSubmit = document.getElementById('<%= btnUploadFront.ClientID %>');
            if (btnSubmit) {
                btnSubmit.addEventListener('click', function () {
                    txtDoc.value = hdnDoc.value;
                });
            }

            var btnEdit = document.getElementById('<%= btnUploadFront.ClientID %>');
            if (btnEdit) {
                btnEdit.addEventListener('click', function () {
                    txtDoc.value = hdnDoc.value;
                });
            }
            if (txtDoc && txtDoc.value.length > 4) {
                hdnDoc.value = txtDoc.value;
                var last4 = txtDoc.value.slice(-4);
                var masked = '*'.repeat(txtDoc.value.length - 4) + last4;
                txtDoc.value = masked;
            }
        });

        function syncAccountFields() {
            document.getElementById('<%= hdnAccount.ClientID %>').value = document.getElementById('<%= txtAccount.ClientID %>').value;
             document.getElementById('<%= hdnConfirmAccount.ClientID %>').value = document.getElementById('<%= txtConfirmAccount.ClientID %>').value;
    }

    // Attach to IFSC textbox onchange so it runs before postback
    document.addEventListener("DOMContentLoaded", function () {
        const ifscTextbox = document.getElementById('<%= txtIFSC.ClientID %>');
        if (ifscTextbox) {
            ifscTextbox.addEventListener('change', syncAccountFields);
        }
    });
    </script>--%>


    <style>
        .product-colors input[type=checkbox], input[type=radio] {
            display: inline;
        }

        .JointAccountpnl tbody {
            display: flex;
            gap: 20px;
        }

        .prev-step, .next-step {
            margin-top: 0px;
        }

            .prev-step:hover {
                margin-top: 0px;
            }

        .tooltip-wrapper {
            position: relative;
            display: inline-block;
            cursor: pointer;
        }

            .tooltip-wrapper .tooltip-text {
                visibility: hidden;
                width: 250px;
                background-color: #333;
                color: #fff;
                text-align: left;
                border-radius: 4px;
                padding: 8px;
                position: absolute;
                z-index: 10;
                bottom: 125%;
                left: 50%;
                transform: translateX(-50%);
                opacity: 0;
                transition: opacity 0.3s;
                font-size: 13px;
            }

            .tooltip-wrapper:hover .tooltip-text {
                visibility: visible;
                opacity: 1;
            }

        .rounded-circle {
            color: #0d6efd;
            border: 1px solid #0d6efd;
        }

        .Error-Message {
            font-size: 12px !important;
            color: red;
            display: none;
            font-weight: 400 !important;
            margin-bottom: 0px;
        }
    </style>

    <script type="text/javascript">
        let selectedDocType = "";

        function handleDocumentFormat(value) {
            selectedDocType = value;
            var hdnDoc = document.getElementById('<%= hdnDocumentNumber.ClientID %>');
            var docinput = document.getElementById('<%= txtDocumentNumber.ClientID %>');
            var doclbl = document.getElementById('<%= lblDocumentNumber.ClientID %>');
            const errorAdharLabel = document.getElementById("lblAdhaarDocumentNumber");
            errorAdharLabel.style.display = "none";
            if (selectedDocType === "13" || selectedDocType === "57" || selectedDocType === "58") {
                errorAdharLabel.style.display = "block";
            }
            if (selectedDocType === "278") {
                docinput.style.display = "none";
                doclbl.style.display = "none";
            }
            else {
                docinput.style.display = "block";
                doclbl.style.display = "block";
            }
        }

        function validateDocumentNumber() {
            const input = document.getElementById("<%= txtDocumentNumber.ClientID %>");
            const errorLabel = document.getElementById("<%= lblDocFormatError.ClientID %>");

            let val = input.value.toUpperCase();
            errorLabel.style.display = "none";
            errorLabel.innerText = "";

            if (selectedDocType === "13" || selectedDocType === "57" || selectedDocType === "58") {
                input.maxLength = 4;
                val = val.replace(/\D/g, '');
                if (val.length > 4) {
                    val = val.substring(val.length - 4);
                }
                input.value = val;

                if (val.length < 4) {
                    errorLabel.innerText = "Please enter last 4 digits only.";
                } else {
                    errorLabel.style.display = "none";
                    errorLabel.innerText = "";
                }
            }
            else if (selectedDocType === "19") {
                input.maxLength = 10;
                val = val.replace(/[^A-Z0-9]/g, '');
                if (val.length > 10) {
                    val = val.substring(0, 10);
                }
                let panFormatted = '';
                for (let i = 0; i < val.length; i++) {
                    let char = val.charAt(i);
                    if (i < 5) {
                        if (char.match(/[A-Z]/)) {
                            panFormatted += char;
                        }
                    }
                    else if (i >= 5 && i < 9) {
                        if (char.match(/[0-9]/)) {
                            panFormatted += char;
                        }
                    }
                    else if (i === 9) {
                        if (char.match(/[A-Z]/)) {
                            panFormatted += char;
                        }
                    }
                }
                input.value = panFormatted;
                const isValidPAN = /^[A-Z]{5}[0-9]{4}[A-Z]$/.test(panFormatted);
                if (isValidPAN && panFormatted.length === 10) {
                    errorLabel.style.display = "none";
                    errorLabel.innerText = "";
                }
            }
            else {
                // Reset maxlength for other document types, if needed
                input.maxLength = 15; // or your default max length
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card p-3">
        <div class="btn-group">
            <a class="btn btn-primary" href="ViewSalesPerson.aspx"><i class="fa fa-list"></i>&nbsp View Sales Executive List</a>
        </div>
    </div>


    <div class="container-fluid px-0">
        <div class="my-3 d-flex justify-content-end" style="gap: 10px">
            <button id="btnPersonalInformationView" style="border-radius: 7px" runat="server" onserverclick="btnPersonalInformation_Click">
                <svg stroke="currentColor" fill="currentColor" stroke-width="0" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="1em" width="1em">
                    <path d="M240 192C240 147.8 275.8 112 320 112C364.2 112 400 147.8 400 192C400 236.2 364.2 272 320 272C275.8 272 240 236.2 240 192zM448 192C448 121.3 390.7 64 320 64C249.3 64 192 121.3 192 192C192 262.7 249.3 320 320 320C390.7 320 448 262.7 448 192zM144 544C144 473.3 201.3 416 272 416L368 416C438.7 416 496 473.3 496 544L496 552C496 565.3 506.7 576 520 576C533.3 576 544 565.3 544 552L544 544C544 446.8 465.2 368 368 368L272 368C174.8 368 96 446.8 96 544L96 552C96 565.3 106.7 576 120 576C133.3 576 144 565.3 144 552L144 544z" />
                </svg>
                Personal Information View
            </button>
            <button id="btnBandDetailsView" runat="server" visible="false" onserverclick="btnBandDetailsView_Click" style="border-radius: 7px;">
                <svg stroke="currentColor" fill="currentColor" stroke-width="0" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="1em" width="1em">
                    <path d="M335.9 84.2C326.1 78.6 314 78.6 304.1 84.2L80.1 212.2C67.5 219.4 61.3 234.2 65 248.2C68.7 262.2 81.5 272 96 272L128 272L128 480L128 480L76.8 518.4C68.7 524.4 64 533.9 64 544C64 561.7 78.3 576 96 576L544 576C561.7 576 576 561.7 576 544C576 533.9 571.3 524.4 563.2 518.4L512 480L512 272L544 272C558.5 272 571.2 262.2 574.9 248.2C578.6 234.2 572.4 219.4 559.8 212.2L335.8 84.2zM464 272L464 480L400 480L400 272L464 272zM352 272L352 480L288 480L288 272L352 272zM240 272L240 480L176 480L176 272L240 272zM320 160C337.7 160 352 174.3 352 192C352 209.7 337.7 224 320 224C302.3 224 288 209.7 288 192C288 174.3 302.3 160 320 160z" />
                </svg>
                Bank Details View
            </button>
            <button id="btnUploadedDocumentListView" runat="server" onserverclick="btnUploadedDocumentListView_Click" style="border-radius: 7px;">
                <svg stroke="currentColor" fill="currentColor" stroke-width="0" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="1em" width="1em">
                    <path d="M352 173.3L352 384C352 401.7 337.7 416 320 416C302.3 416 288 401.7 288 384L288 173.3L246.6 214.7C234.1 227.2 213.8 227.2 201.3 214.7C188.8 202.2 188.8 181.9 201.3 169.4L297.3 73.4C309.8 60.9 330.1 60.9 342.6 73.4L438.6 169.4C451.1 181.9 451.1 202.2 438.6 214.7C426.1 227.2 405.8 227.2 393.3 214.7L352 173.3zM320 464C364.2 464 400 428.2 400 384L480 384C515.3 384 544 412.7 544 448L544 480C544 515.3 515.3 544 480 544L160 544C124.7 544 96 515.3 96 480L96 448C96 412.7 124.7 384 160 384L240 384C240 428.2 275.8 464 320 464zM464 488C477.3 488 488 477.3 488 464C488 450.7 477.3 440 464 440C450.7 440 440 450.7 440 464C440 477.3 450.7 488 464 488z" />
                </svg>
                Uploaded Document List View
            </button>
            <button id="btnCommisionDetailsView" visible="false" style="border-radius: 7px;" runat="server" onserverclick="btnCommisionDetailsView_Click">
                <svg stroke="currentColor" fill="currentColor" stroke-width="0" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="1em" width="1em">
                    <path d="M160 128C160 110.3 174.3 96 192 96L456 96C469.3 96 480 106.7 480 120C480 133.3 469.3 144 456 144L379.3 144C397 163.8 409.4 188.6 414 216L456 216C469.3 216 480 226.7 480 240C480 253.3 469.3 264 456 264L414 264C403.6 326.2 353.2 374.9 290.2 382.9L434.6 486C449 496.3 452.3 516.3 442 530.6C431.7 544.9 411.7 548.3 397.4 538L173.4 378C162.1 370 157.3 355.5 161.5 342.2C165.7 328.9 178.1 320 192 320L272 320C307.8 320 338.1 296.5 348.3 264L184 264C170.7 264 160 253.3 160 240C160 226.7 170.7 216 184 216L348.3 216C338.1 183.5 307.8 160 272 160L192 160C174.3 160 160 145.7 160 128z" />
                </svg>
                Commision Details View
            </button>
            <button id="btnAccountHistoryView" visible="false" style="border-radius: 7px;" runat="server" onserverclick="btnAccountHistoryView_Click">
                <svg stroke="currentColor" fill="currentColor" stroke-width="0" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512" height="1em" width="1em">
                    <path d="M288 64c106 0 192 86 192 192S394 448 288 448c-65.2 0-122.9-32.5-157.6-82.3-10.1-14.5-30.1-18-44.6-7.9s-18 30.1-7.9 44.6C124.1 468.6 201 512 288 512 429.4 512 544 397.4 544 256S429.4 0 288 0C202.3 0 126.5 42.1 80 106.7L80 80c0-17.7-14.3-32-32-32S16 62.3 16 80l0 112c0 17.7 14.3 32 32 32l24.6 0c.5 0 1 0 1.5 0l86 0c17.7 0 32-14.3 32-32s-14.3-32-32-32l-38.3 0C154.9 102.6 217 64 288 64zm24 88c0-13.3-10.7-24-24-24s-24 10.7-24 24l0 104c0 6.4 2.5 12.5 7 17l72 72c9.4 9.4 24.6 9.4 33.9 0s9.4-24.6 0-33.9l-65-65 0-94.1z" />
                </svg>
                Account History View
            </button>
        </div>
        <asp:MultiView ID="mvViewType" runat="server" ActiveViewIndex="0">
            <asp:View ID="PersonalInformationList" runat="server">

                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-4 mb-3">
                                <asp:Label ID="lblFirstName" runat="server" Text="Full Name"> Full Name<span style="color:red;">*</span></asp:Label>
                                <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" CssClass="form-control" placeholder="Full Name" AutoComplete="off" OnTextChanged="AccountHolder_Change" AutoPostBack="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" Font-Size="12px"
                                    ErrorMessage="Full Name is required" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                                <asp:Label ID="lblFirstNameError" CssClass="ErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                            </div>



                            <!-- Mobile Number -->
                            <div class="col-md-4 mb-1" style="display: grid;">
                                <asp:Label ID="lblMobile" runat="server" Text="Mobile Number">Mobile Number<span style="color:red;">*</span></asp:Label>
                                <asp:TextBox ID="txtMobile" runat="server" MaxLength="10" CssClass="form-control" AutoComplete="off" name="txtMobile" placeholder="Enter Mobile No"
                                    pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" AutoPostBack="true" OnTextChanged="txtCustomerMobile_TextChanged"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ControlToValidate="txtMobile" Font-Size="12px"
                                    ErrorMessage="Mobile number is required" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="txtMobile" Font-Size="12px"
                                    ValidationExpression="^\d{10}$" ErrorMessage="Enter valid 10-digit mobile number"
                                    CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                                <asp:Label ID="BlockCustomerMobileErrorMessage" runat="server" ForeColor="Red" Font-Size="12px"></asp:Label>
                                <asp:HiddenField ID="hdnCountryCode" runat="server" />
                                <asp:HiddenField ID="hdnPhoneNumber" runat="server" />
                                <asp:CheckBox ID="chkMobileNumberWhatsApp" runat="server" CssClass="same-small-txt mt-1" Text="This Number is on WhatsApp" />
                            </div>

                            <!-- Alternate Mobile -->
                            <div class="col-md-4 mb-1" style="display: grid;">
                                <asp:Label ID="lblAltMobile" runat="server" Text="Alternate Mobile No."></asp:Label>
                                <asp:TextBox ID="txtAltMobile" runat="server" MaxLength="10" CssClass="form-control" AutoComplete="off" name="txtAltMobile" placeholder="Enter Mobile No"
                                    pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" AutoPostBack="true" OnTextChanged="txtCustomerAltMobile_TextChanged"></asp:TextBox>
                                <asp:Label ID="BlockAltCustomerMobileErrorMessage" runat="server" ForeColor="Red" Font-Size="12px"></asp:Label>
                                <asp:HiddenField ID="hdnAltCountryCode" runat="server" />
                                <asp:HiddenField ID="hdnAltMobile" runat="server" />
                                <asp:CheckBox ID="chkAltMobileNumberWhatsApp" runat="server" CssClass="same-small-txt mt-1" Text="This Number is on WhatsApp" />
                            </div>

                            <!-- Email -->
                            <div class="col-md-4 mb-3">
                                <asp:Label ID="lblEmail" runat="server" Text="Email Address">Email Address<span style="color:red;">*</span></asp:Label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control text-lowercase" placeholder="Email address" AutoComplete="off"
                                    AutoPostBack="true" OnTextChanged="txtCustomerEmail_TextChanged"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" Font-Size="12px"
                                    ErrorMessage="Email is required" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" Font-Size="12px"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ErrorMessage="Enter a valid email address" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                                <asp:Label ID="BlockCustomerEmailErrorMessage" runat="server" ForeColor="Red" Font-Size="12px"></asp:Label>
                            </div>

                            <!-- Alternate Email -->
                            <div class="col-md-4 mb-3">
                                <asp:Label ID="lblAltEmail" runat="server" Text="Alternate Email Address"></asp:Label>
                                <asp:TextBox ID="txtAltEmail" runat="server" CssClass="form-control text-lowercase" placeholder="Alternate Email Address" AutoComplete="off"
                                    AutoPostBack="true" OnTextChanged="txtCustomerAltEmail_TextChanged"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAltEmail" Font-Size="12px"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ErrorMessage="Enter a valid email address" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                                <asp:Label ID="BlockAltCustomerEmailErrorMessage" runat="server" ForeColor="Red" Font-Size="12px"></asp:Label>
                            </div>

                            <!-- DOB -->
                            <div class="col-md-4 mb-3">
                                <asp:Label ID="lblDOB" runat="server" Text="Date of Birth"></asp:Label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control"
                                        AutoCompleteType="Disabled" AutoComplete="off"></asp:TextBox>
                                    <div class="input-group-append">
                                        <span class="input-group-text" style="cursor: pointer;"
                                            onclick="document.getElementById('<%= txtDOB.ClientID %>').focus();">
                                            <i class="fa fa-calendar"></i>
                                        </span>
                                    </div>
                                </div>
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtDOB"></cc1:CalendarExtender>
                            </div>

                            <!-- Gender -->
                            <div class="col-md-4 mb-3">
                                <asp:Label ID="lblGender" runat="server" Text="Gender"></asp:Label>
                                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                    <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <!-- PIN -->
                            <div class="col-md-2 mb-3">
                                <asp:Label ID="lblPIN" runat="server" Text="PIN Code">PIN Code<span style="color:red;">*</span></asp:Label>
                                <asp:TextBox ID="txtPIN" runat="server" CssClass="form-control" AutoPostBack="true" MaxLength="6" OnTextChanged="txtPIN_TextChanged" Font-Size="12px"
                                    pattern="\d{6}" title="Enter a 6-digit Pincode" oninput="validatePincode(this)" AutoComplete="off"></asp:TextBox>
                                <asp:Label ID="lblPinCodeError" runat="server" Visible="false" CssClass="error-message"></asp:Label>
                                <asp:RequiredFieldValidator ID="rfvGender" runat="server" ControlToValidate="txtPIN" Font-Size="12px"
                                    InitialValue="" ErrorMessage="PIN Code is required" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                            </div>

                            <!-- State -->
                            <div class="col-md-2 mb-3">
                                <asp:Label ID="lblState" runat="server" Text="State"></asp:Label>
                                <asp:TextBox ID="txtState" runat="server" CssClass="form-control" placeholder="State" Enabled="false" MaxLength="50" AutoComplete="off"></asp:TextBox>
                            </div>

                            <!-- City -->
                            <div class="col-md-4 mb-3">
                                <asp:Label ID="lblCity" runat="server" Text="City"></asp:Label>
                                <asp:TextBox ID="txtCity" runat="server" Enabled="false" CssClass="form-control" placeholder="City" AutoComplete="off"></asp:TextBox>
                            </div>

                            <!-- Address -->
                            <div class="col-md-12 mb-3">
                                <asp:Label ID="lblCommAddress" runat="server" Text="Communication Address"></asp:Label>
                                <asp:TextBox ID="txtCommAddress" runat="server" CssClass="form-control" MaxLength="200" Rows="2" AutoComplete="off" TextMode="MultiLine"></asp:TextBox>
                            </div>

                            <div class="row" runat="server" visible="false">
                                <div class="col-md mb-3">
                                    <asp:CheckBox ID="chkCopyRetailerBank" runat="server" Text="Use Retailer Bank Details" OnCheckedChanged="BindBankDetails" AutoPostBack="true" />
                                    <br />
                                    <asp:Label ID="lblCopyRetailerBankErrorMessage" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
                                </div>
                            </div>


                            <!-- Account Number -->
                            <div class="col-md-4 mb-3">
                                <asp:Label ID="lblAccount" runat="server" Text="Account Number">Account Number<span style="color:red;">*</span></asp:Label>
                                <asp:HiddenField ID="hdnAccount" runat="server" />
                                <asp:TextBox ID="txtAccount" runat="server" CssClass="form-control" MaxLength="20" onpaste="return false;" oncopy="return false;" oncut="return false;" AutoComplete="off"
                                    AutoPostBack="true" OnTextChanged="AccountNumberChange" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAccountNumber" runat="server" ControlToValidate="txtAccount" Font-Size="12px"
                                    ErrorMessage="Account Number is required" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revAccountNumber" runat="server" ControlToValidate="txtAccount" Font-Size="12px"
                                    ErrorMessage="Account number must be 6 to 20 digits" ValidationExpression="^\d{6,20}$" ForeColor="Red" Display="Dynamic" ValidationGroup="vgPersonal" />
                                <asp:Label ID="lblAccountError" CssClass="ErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                            </div>

                            <!-- Confirm Account Number -->
                            <div class="col-md-4 mb-3">
                                <asp:Label ID="lblConfirmAccount" runat="server" Text="Confirm Account Number">Confirm Account Number<span style="color:red;">*</span></asp:Label>
                                <asp:HiddenField ID="hdnConfirmAccount" runat="server" />
                                <asp:TextBox ID="txtConfirmAccount" runat="server" CssClass="form-control" MaxLength="20" onpaste="return false;" oncopy="return false;" oncut="return false;" AutoComplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvConfirmAccountNumber" runat="server" ControlToValidate="txtConfirmAccount" Font-Size="12px"
                                    ErrorMessage="Confirm Account Number is required" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revConfirmAccountNumber" runat="server" ControlToValidate="txtConfirmAccount" Font-Size="12px"
                                    ErrorMessage="Account number must be 6 to 20 digits" ValidationExpression="^\d{6,20}$" ForeColor="Red" Display="Dynamic" ValidationGroup="vgPersonal" />
                                <asp:CompareValidator ID="cvAccount" runat="server" ControlToCompare="txtAccount" ControlToValidate="txtConfirmAccount" Font-Size="12px"
                                    ErrorMessage="Account numbers do not match" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                                <asp:Label ID="lblConfirmAccountError" CssClass="ErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                            </div>

                            <!-- IFSC -->
                            <div class="col-md-4 mb-3">
                                <asp:Label ID="lblIFSC" runat="server" Text="IFSC Code">IFSC Code<span style="color:red;">*</span></asp:Label>
                                <asp:TextBox ID="txtIFSC" runat="server" AutoPostBack="true" MaxLength="11" OnTextChanged="txtIFSC_TextChanged" CssClass="form-control text-uppercase" AutoComplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvIFSCCode" runat="server" ControlToValidate="txtIFSC" Font-Size="12px"
                                    ErrorMessage="IFSC Code is required" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revIFSC" runat="server" ControlToValidate="txtIFSC" Font-Size="12px"
                                    ValidationExpression="^[A-Z]{4}0[A-Z0-9]{6}$"
                                    ErrorMessage="Enter a valid IFSC code" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                                <asp:Label ID="lblIFSCError" CssClass="ErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                            </div>

                            <!-- Account Holder Name -->
                            <div class="col-md-4 mb-3">
                                <asp:Label ID="lblHolder" runat="server" Text="Account Holder Name">Account Holder Name<span style="color:red;">*</span></asp:Label>
                                <asp:TextBox ID="txtHolder" runat="server" CssClass="form-control" AutoComplete="off" OnTextChanged="AccountHolder_Change" AutoPostBack="true"></asp:TextBox>
                                <asp:Label ID="lblAccountHolderNameError" CssClass="Error-Message" runat="server"></asp:Label>
                                <asp:RequiredFieldValidator ID="rfvAcountHolder" runat="server" ControlToValidate="txtHolder" Font-Size="12px"
                                    ErrorMessage="Account Holder Name is required" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                            </div>

                            <!-- Bank Name -->
                            <div class="col-md-4 mb-3">
                                <asp:Label ID="lblBankName" runat="server" Text="Bank Name"></asp:Label>
                                <asp:TextBox ID="txtBankName" runat="server" Enabled="false" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                            </div>

                            <!-- Branch Name -->
                            <div class="col-md-4 mb-3">
                                <asp:Label ID="lblBranch" runat="server" Enabled="false" Text="Branch Name"></asp:Label>
                                <asp:TextBox ID="txtBranch" runat="server" Enabled="false" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                            </div>

                            <!-- Branch Address -->
                            <div class="col-md-12 mb-3">
                                <asp:Label ID="lblBranchAddress" runat="server" Text="Branch Address"></asp:Label>
                                <asp:TextBox ID="txtBranchAddress" runat="server" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label class="mb-1">UPI ID</label>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtUPIID" placeholder="" AutoComplete="off" MaxLength="50"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regexUPI" runat="server" ControlToValidate="txtUPIID"
                                        ValidationExpression="^[a-zA-Z0-9.\-_]{2,256}@[a-zA-Z]{2,64}$" ErrorMessage="Invalid UPI ID format."
                                        CssClass="text-danger" Display="Dynamic">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label class="mb-1">Type of Bank Account</label>
                                    <asp:DropDownList ID="ddlTypeOfBank" runat="server" CssClass="form-control"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlTypeOfBank_SelectedIndexChanged">
                                        <asp:ListItem Text="-- Select Account Type --" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Savings" Value="Savings"></asp:ListItem>
                                        <asp:ListItem Text="Current" Value="Current"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-6" id="jointAcountpnl" runat="server" visible="false">
                                <div class="form-group mb-3">
                                    <label class="mb-1">Is this your joint account?</label>
                                    <asp:RadioButtonList ID="chkJointAccount" runat="server" CssClass="JointAccountpnl"
                                        AutoPostBack="true" OnSelectedIndexChanged="chkJointAccount_Change">
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="col-md-6" id="jointAcountHolderpnl" runat="server" visible="false">
                                <div class="form-group mb-3">
                                    <label class="mb-1">Joint Account Holder Name</label>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtJointHolderName" placeholder="" AutoComplete="off" MaxLength="50"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label class="mb-1">
                                        Supporting Documents
                                            <span class="tooltip-wrapper">
                                                <span class="badge rounded-circle px-2 py-1">i</span>
                                                <span class="tooltip-text">Any document containing complete Bank Account details such as Beneficiary Name, Account Number, IFSC Code, Bank Branch, etc.</span>
                                            </span>
                                        <span style="color: red">*</span>
                                    </label>
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddlSuppotingDoc" AutoPostBack="false">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblsupportingDocError" CssClass="Error-Message" runat="server"></asp:Label>
                                    <asp:RequiredFieldValidator ID="rfvSuppotingDoc" runat="server" ControlToValidate="ddlSuppotingDoc" InitialValue="" ErrorMessage="Please select a supporting document."
                                        CssClass="Error-Message" Display="Dynamic" ForeColor="Red" ValidationGroup="vgPersonal" />

                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label class="mb-1">File<span style="color: red">*</span></label>
                                    <asp:FileUpload runat="server" ID="fuSuppotingDoc" CssClass="form-control" accept=".jpg,.jpeg,.png,.pdf" />
                                    <asp:Label ID="lblsupportingDocName" CssClass="lblDocumentName" runat="server"></asp:Label>
                                    <asp:Label ID="lblSupportingDocumentError" CssClass="Error-Message" runat="server"></asp:Label>
                                    <asp:RequiredFieldValidator ID="rfvSupportingDoc" runat="server" ControlToValidate="ddlSuppotingDoc" InitialValue="" ErrorMessage="Please select a supporting document."
                                        CssClass="Error-Message" Display="Dynamic" ForeColor="Red" ValidationGroup="vgPersonal"></asp:RequiredFieldValidator>
                                </div>
                                <p class="text-danger mt-2 mb-2" style="font-size: 13px;"><strong>Note<sup>*</sup></strong> jpg, jpeg, png and pdf format is acceptable.</p>
                            </div>

                        </div>

                        <div class="d-flex justify-content-end">
                            <asp:Button ID="btnSubmit" runat="server" Text="Save & Next" CssClass="btn next-step mt-0 mt-lg-3" OnClick="btnSubmit_Click" ValidationGroup="vgPersonal" />

                            <asp:Button ID="btnUpdate" runat="server" Text="Update & Next" CssClass="btn btn-primary next-step mt-0 mt-lg-3" OnClick="btnUpdate_Click"
                                ValidationGroup="vgPersonal" />
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="BandDetails" runat="server">



                <div class="card">
                    <div class="card-body">
                    </div>
                </div>
            </asp:View>


            <asp:View ID="UploadedDocumentList" runat="server">

                <div class="card">
                    <div class="card-body">
                        <div id="DocumentPanel" class="row" runat="server">

                            <div class="col-md-12">
                                <asp:Label ID="lblRequiredDocuments" runat="server" CssClass="text-danger" Text="Note: It is mandatory to submit both Aadhar and PAN card details."></asp:Label>
                            </div>

                            <div class="col-md-4 mb-3">
                                <div class="form-group">
                                    <label class="mb-1">Document Name <span style="color: red;">*</span></label>
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddlDocumentName" AutoPostBack="false" onchange="handleDocumentFormat(this.value)">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4 mb-3" id="lblDocumentNumber" runat="server">
                                <div class="form-group">
                                    <label class="mb-1">Document Number</label>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtDocumentNumber" placeholder="" MaxLength="20" AutoComplete="off" oninput="validateDocumentNumber()"></asp:TextBox>
                                    <asp:HiddenField ID="hdnDocumentNumber" runat="server" />
                                    <p class="text-danger mt-2 mb-0" id="lblAdhaarDocumentNumber" style="font-size: 12px; display: none;"><strong>Note<sup>*</sup></strong> Please enter the last 4 digits of your Aadhar.</p>
                                </div>
                            </div>
                            <div class="col-md-4 mb-3">
                                <div class="row">
                                    <div class="col-12 col-lg-9">
                                        <div class="form-group">
                                            <label class="mb-1">File</label>
                                            <asp:FileUpload runat="server" ID="fuFrontSide" CssClass="form-control" />
                                            <p class="text-danger mt-2"><strong>Note<sup>*</sup></strong> jpg, jpeg, png and pdf format is acceptable.</p>
                                            <asp:Label ID="lblDocument" runat="server" Text="Document is required." ForeColor="Red" Visible="false" Font-Size="12px"></asp:Label>
                                            <asp:Image ID="imgPreview" runat="server" CssClass="img-fluid mt-3" Visible="false" Width="200" />
                                            <asp:Literal ID="litPdfPreview" runat="server" Visible="false"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col-12 col-lg-3 mt-4">
                                        <div class="form-group">
                                            <asp:Button runat="server" ID="btnUploadFront" Text="Upload" CssClass="default-btn next-step" OnClick="btnUploadFront_Click" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="col-md-12">
                                <asp:Label ID="lblDocFormatError" runat="server" CssClass="alert alert-danger" Style="display: none;"></asp:Label>
                            </div>

                            <div class="col-md-12">
                                <div class="DocumentData table-responsive">
                                    <asp:GridView runat="server" ID="gvDocuments" CssClass="table table-striped table-bordered" AutoGenerateColumns="false"
                                        OnRowCommand="gvDocuments_RowCommand" DataKeyNames="DocId" OnRowDataBound="gvDocuments_RowDataBound">
                                        <Columns>
                                            <asp:BoundField HeaderText="Sr.No." DataField="SrNo" />
                                            <asp:BoundField HeaderText="DocId" DataField="DocId" Visible="false" />
                                            <asp:BoundField HeaderText="Document Name" DataField="DocumentName" />
                                            <asp:TemplateField HeaderText="Document Number">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMaskedDoc" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Document Path" DataField="DocumentPath" Visible="false" />
                                            <asp:BoundField HeaderText="Remarks" DataField="ActionRemarks" />
                                            <asp:BoundField HeaderText="Status" DataField="ActionStatus" />
                                            <asp:BoundField HeaderText="Action By" DataField="ActionBy" />
                                            <asp:BoundField HeaderText="Action Date" DataField="ActionDate" />
                                            <asp:TemplateField HeaderText="Actions">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkView" runat="server" CommandName="ViewDoc" CssClass="text-info"
                                                        CommandArgument='<%# Eval("DocId") + "|" + Eval("DocumentName") + "|" + Eval("DocumentNumber") + "|" + Eval("DocumentPath") %>'>
                                                        <i class="fa fa-eye"></i>
                                                    </asp:LinkButton>

                                                    <%--  <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditDoc" CommandArgument='<%# Eval("MId") %>'
                                             CssClass="btn btn-sm btn-primary">
                                             <i class="fa fa-edit"></i>
                                         </asp:LinkButton>--%>

                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteRow" CommandArgument='<%# Eval("MId") %>'
                                                        CssClass="text-danger" OnClientClick="return confirm('Are you sure you want to delete this Document?');">
                                             <i class="fa fa-trash"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <ul class="list-inline pull-right mt-3">
                            <asp:Button ID="btnnext3" runat="server" Text="Submit" CssClass="btn next-step" OnClick="btnnext3_Click" />
                        </ul>

                    </div>
                </div>
            </asp:View>

            <asp:View ID="CommisionDetailsList" runat="server">
            </asp:View>
            <asp:View ID="AccountHistoryList" runat="server">
            </asp:View>
        </asp:MultiView>
    </div>

</asp:Content>

