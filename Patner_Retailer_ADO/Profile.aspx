<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Patner_Retailer_ADO.Profile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/js/intlTelInput.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/css/intlTelInput.css">

    <script type="text/javascript">
        window.onload = function () {
            const mobileCodeInput = document.getElementById('<%= txtMobileNumber.ClientID %>');
            const altMobileCodeInput = document.getElementById('<%= txtAlternateMobile.ClientID %>');

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

            //WhatsApp Numbers

            const WAmobileCodeInput = document.getElementById('<%= txtWhatsAppNo.ClientID %>');
            const WAaltMobileCodeInput = document.getElementById('<%= txtWhatsAppNo2.ClientID %>');

            const WAcountryCodeHidden = document.getElementById('<%= hdnWhatsAppNo.ClientID %>');
            const WAphoneNumberHidden = document.getElementById('<%= hdnWhatsAppNo2.ClientID %>');

            const WAaltCountryCodeHidden = document.getElementById('<%= hdnWhatsAppNoCountry.ClientID %>');
            const WAaltPhoneNumberHidden = document.getElementById('<%= hdnWhatsAppNoCountry2.ClientID %>');
            if (WAmobileCodeInput && WAcountryCodeHidden && WAphoneNumberHidden) {
                const WAitiMobile = window.intlTelInput(WAmobileCodeInput, {
                    initialCountry: "in",
                    separateDialCode: true,
                    formatOnDisplay: false,
                    utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/js/utils.js",
                });

                function WAupdatePrimaryHiddenFields() {
                    const dialCode = WAitiMobile.getSelectedCountryData().dialCode;
                    let nationalNumber = WAitiMobile.getNumber(intlTelInputUtils.numberFormat.NATIONAL).replace(/\D/g, '');
                    if (nationalNumber.startsWith('0')) {
                        nationalNumber = nationalNumber.substring(1);
                    }
                    WAcountryCodeHidden.value = dialCode;
                    WAphoneNumberHidden.value = nationalNumber;
                }

                WAmobileCodeInput.addEventListener('countrychange', WAupdatePrimaryHiddenFields);
                WAmobileCodeInput.addEventListener('blur', WAupdatePrimaryHiddenFields);
            }

            if (WAaltMobileCodeInput && WAaltCountryCodeHidden && WAaltPhoneNumberHidden) {
                const WAitiAltMobile = window.intlTelInput(WAaltMobileCodeInput, {
                    initialCountry: "in",
                    separateDialCode: true,
                    formatOnDisplay: false,
                    utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/js/utils.js",
                });

                function WAupdateAltHiddenFields() {
                    const dialCode = WAitiAltMobile.getSelectedCountryData().dialCode;
                    let nationalNumber = WAitiAltMobile.getNumber(intlTelInputUtils.numberFormat.NATIONAL).replace(/\D/g, '');
                    if (nationalNumber.startsWith('0')) {
                        nationalNumber = nationalNumber.substring(1);
                    }
                    WAaltCountryCodeHidden.value = dialCode;
                    WAaltPhoneNumberHidden.value = nationalNumber;
                }

                WAaltMobileCodeInput.addEventListener('countrychange', WAupdateAltHiddenFields);
                WAaltMobileCodeInput.addEventListener('blur', WAupdateAltHiddenFields);
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
    </script>

      <script>
          setTimeout(function () {
              const rows = document.querySelectorAll('#ContentPlaceHolder1_GVAccountHistory .row');
              if (rows.length > 1) {
                  rows[1].classList.add('table-responsive');
              }
          }, 500);
      </script>

    <style>
        .profile-panel .row .col-md-3 label {
            font-weight: bold;
            display: block;
            margin-bottom: 0.25rem;
            color: #495057;
        }

        .profile-panel .row .col-md-3 span {
            display: block;
            color: #212529;
        }

        .profile-panel .card-header + .card-body h5 {
            margin-top: 0;
        }

        .profile-panel .table {
            width: 100%;
            margin-bottom: 1rem;
            color: #212529;
            border-collapse: collapse;
        }

            .profile-panel .table th,
            .profile-panel .table td {
                padding: 0.75rem;
                vertical-align: top;
                border-top: 1px solid #dee2e6;
                text-align: left;
            }

            .profile-panel .table thead th {
                vertical-align: bottom;
                border-bottom: 2px solid #dee2e6;
            }

            .profile-panel .table tbody tr:nth-of-type(odd) {
                background-color: rgba(0, 0, 0, 0.05);
            }

        .profile-panel .table-sm th,
        .profile-panel .table-sm td {
            padding: 0.5rem;
        }

        .profile-panel .card {
            border: 1px solid rgba(0, 0, 0, 0.125);
            border-radius: 0.25rem;
            margin-bottom: 1.5rem;
        }

        .profile-panel .card-header {
            background-color: #f9feff !important;
            padding: 0.75rem 1.25rem;
            margin-bottom: 0;
            border-bottom: 1px solid rgba(0, 0, 0, 0.125);
        }

        .profile-panel .card-body {
            padding: 1.25rem;
        }

        .AccountPendingErrorMessage {
            color: red;
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

        .dataTables_info, .dataTables_paginate {
            display: none;
        }

        .lblTagName {
            font-weight: 600;
            font-size: 14px;
        }

        .profile-txt-area {
            display: flex;
            flex-wrap: wrap;
            flex-direction: column;
            margin-bottom: 7px;
        }

        .regs-text-address {
            font-size: 16px;
            color: #333;
            border-top: 1px solid #e2e2e2;
            padding-top: 10px;
            margin-top: 10px;
        }

        .prev-step, .next-step {
            font-size: 14px !important;
            padding: 7px 15px;
            border: none;
            border-radius: 5px;
            margin-top: 0px;
            color: #fff;
        }

            .prev-step:hover {
                font-size: 16px;
                padding: 7px 15px;
                border: none;
                border-radius: 5px;
                margin-top: 0px;
                color: #fff;
            }

        .submit_btn2 {
            color: #fff !important;
            background: #000 !important;
            font-size: 13px;
            letter-spacing: .5px;
        }
        .hideMessagecls {
            display: flex;
            justify-content: end;
            position: absolute;
            right: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlProfile" runat="server" CssClass="profile-panel">
        <div runat="server" id="hdrMessage">
            <div id="AccountPendingMessage" runat="server" class="alert alert-danger" role="alert">
                <asp:LinkButton CssClass="hideMessagecls" runat="server" ID="hideMessage" OnClick="hideMessageClick">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="20">
                        <path d="M320 112C434.9 112 528 205.1 528 320C528 434.9 434.9 528 320 528C205.1 528 112 434.9 112 320C112 205.1 205.1 112 320 112zM320 576C461.4 576 576 461.4 576 320C576 178.6 461.4 64 320 64C178.6 64 64 178.6 64 320C64 461.4 178.6 576 320 576zM231 231C221.6 240.4 221.6 255.6 231 264.9L286 319.9L231 374.9C221.6 384.3 221.6 399.5 231 408.8C240.4 418.1 255.6 418.2 264.9 408.8L319.9 353.8L374.9 408.8C384.3 418.2 399.5 418.2 408.8 408.8C418.1 399.4 418.2 384.2 408.8 374.9L353.8 319.9L408.8 264.9C418.2 255.5 418.2 240.3 408.8 231C399.4 221.7 384.2 221.6 374.9 231L319.9 286L264.9 231C255.5 221.6 240.3 221.6 231 231z"/>
                    </svg>
                </asp:LinkButton>
                <div class="mb-1">Dear Partner,</div>
                <ul class="mb-0 ms-0 ps-3">
                    <li>Your basic details have been received and are under review. We will revert shortly.</li>
                    <li>You may review the submitted details here as under and submit changes if any.</li>
                </ul>
            </div>
            <div id="AccountMoreDocumentRequired" runat="server" class="alert alert-danger" role="alert">
                <asp:LinkButton CssClass="hideMessagecls" runat="server" ID="LinkButton1" OnClick="hideMessageClick">
     <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="20">
         <path d="M320 112C434.9 112 528 205.1 528 320C528 434.9 434.9 528 320 528C205.1 528 112 434.9 112 320C112 205.1 205.1 112 320 112zM320 576C461.4 576 576 461.4 576 320C576 178.6 461.4 64 320 64C178.6 64 64 178.6 64 320C64 461.4 178.6 576 320 576zM231 231C221.6 240.4 221.6 255.6 231 264.9L286 319.9L231 374.9C221.6 384.3 221.6 399.5 231 408.8C240.4 418.1 255.6 418.2 264.9 408.8L319.9 353.8L374.9 408.8C384.3 418.2 399.5 418.2 408.8 408.8C418.1 399.4 418.2 384.2 408.8 374.9L353.8 319.9L408.8 264.9C418.2 255.5 418.2 240.3 408.8 231C399.4 221.7 384.2 221.6 374.9 231L319.9 286L264.9 231C255.5 221.6 240.3 221.6 231 231z"/>
     </svg>
 </asp:LinkButton>
                <div class="mb-1">Dear Partner,</div>
                <ul class="mb-0 ms-0 ps-3">
                    <li>We have received your basic details. However, additional documents are required to proceed with the verification.</li>
                    <li>Please review your submitted details below and upload the necessary documents or make changes if needed.</li>
                </ul>
            </div>
            <div id="AccountRejectMessage" runat="server" class="alert alert-danger" role="alert">
                <asp:LinkButton CssClass="hideMessagecls" runat="server" ID="LinkButton2" OnClick="hideMessageClick">
     <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="20">
         <path d="M320 112C434.9 112 528 205.1 528 320C528 434.9 434.9 528 320 528C205.1 528 112 434.9 112 320C112 205.1 205.1 112 320 112zM320 576C461.4 576 576 461.4 576 320C576 178.6 461.4 64 320 64C178.6 64 64 178.6 64 320C64 461.4 178.6 576 320 576zM231 231C221.6 240.4 221.6 255.6 231 264.9L286 319.9L231 374.9C221.6 384.3 221.6 399.5 231 408.8C240.4 418.1 255.6 418.2 264.9 408.8L319.9 353.8L374.9 408.8C384.3 418.2 399.5 418.2 408.8 408.8C418.1 399.4 418.2 384.2 408.8 374.9L353.8 319.9L408.8 264.9C418.2 255.5 418.2 240.3 408.8 231C399.4 221.7 384.2 221.6 374.9 231L319.9 286L264.9 231C255.5 221.6 240.3 221.6 231 231z"/>
     </svg>
 </asp:LinkButton>
                <div class="mb-1">Dear Partner,</div>
                <ul class="mb-0 ms-0 ps-3">
                    <li>Unfortunately, your application has been rejected due to incomplete or invalid information.</li>
                    <li>Please review your submitted details below and make the necessary corrections or upload the required documents to reapply.</li>
                </ul>
            </div>
            <div id="PendingWithdrawApplicationMessage" runat="server" class="alert alert-danger" role="alert">
               <asp:LinkButton CssClass="hideMessagecls" runat="server" ID="LinkButton3" OnClick="hideMessageClick">
     <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="20">
         <path d="M320 112C434.9 112 528 205.1 528 320C528 434.9 434.9 528 320 528C205.1 528 112 434.9 112 320C112 205.1 205.1 112 320 112zM320 576C461.4 576 576 461.4 576 320C576 178.6 461.4 64 320 64C178.6 64 64 178.6 64 320C64 461.4 178.6 576 320 576zM231 231C221.6 240.4 221.6 255.6 231 264.9L286 319.9L231 374.9C221.6 384.3 221.6 399.5 231 408.8C240.4 418.1 255.6 418.2 264.9 408.8L319.9 353.8L374.9 408.8C384.3 418.2 399.5 418.2 408.8 408.8C418.1 399.4 418.2 384.2 408.8 374.9L353.8 319.9L408.8 264.9C418.2 255.5 418.2 240.3 408.8 231C399.4 221.7 384.2 221.6 374.9 231L319.9 286L264.9 231C255.5 221.6 240.3 221.6 231 231z"/>
     </svg>
 </asp:LinkButton>
                <div class="mb-1">Dear Partner,</div>
                <ul class="mb-0 ms-0 ps-3">
                    <li>Your request to withdraw your application has been received and is currently under review.</li>
                    <li>Please wait while our team verifies the request. You will be notified once the process is completed.</li>
                </ul>
            </div>
            <div id="TerminateApplication" runat="server" class="alert alert-danger" role="alert">
                <asp:LinkButton CssClass="hideMessagecls" runat="server" ID="LinkButton4" OnClick="hideMessageClick">
     <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="20">
         <path d="M320 112C434.9 112 528 205.1 528 320C528 434.9 434.9 528 320 528C205.1 528 112 434.9 112 320C112 205.1 205.1 112 320 112zM320 576C461.4 576 576 461.4 576 320C576 178.6 461.4 64 320 64C178.6 64 64 178.6 64 320C64 461.4 178.6 576 320 576zM231 231C221.6 240.4 221.6 255.6 231 264.9L286 319.9L231 374.9C221.6 384.3 221.6 399.5 231 408.8C240.4 418.1 255.6 418.2 264.9 408.8L319.9 353.8L374.9 408.8C384.3 418.2 399.5 418.2 408.8 408.8C418.1 399.4 418.2 384.2 408.8 374.9L353.8 319.9L408.8 264.9C418.2 255.5 418.2 240.3 408.8 231C399.4 221.7 384.2 221.6 374.9 231L319.9 286L264.9 231C255.5 221.6 240.3 221.6 231 231z"/>
     </svg>
 </asp:LinkButton>
                <div class="mb-1">Dear Partner,</div>
                <ul class="mb-0 ms-0 ps-3">
                    <li>Your application has been terminated due to non-compliance or failure to meet the required criteria.</li>
                    <li>If you believe this was a mistake or would like to reapply, please contact support or follow the reapplication process.</li>
                </ul>
            </div>
            <div id="ApprovedWithdrawApplication" runat="server" class="alert alert-success" role="alert">
               <asp:LinkButton CssClass="hideMessagecls" runat="server" ID="LinkButton5" OnClick="hideMessageClick">
     <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="20">
         <path d="M320 112C434.9 112 528 205.1 528 320C528 434.9 434.9 528 320 528C205.1 528 112 434.9 112 320C112 205.1 205.1 112 320 112zM320 576C461.4 576 576 461.4 576 320C576 178.6 461.4 64 320 64C178.6 64 64 178.6 64 320C64 461.4 178.6 576 320 576zM231 231C221.6 240.4 221.6 255.6 231 264.9L286 319.9L231 374.9C221.6 384.3 221.6 399.5 231 408.8C240.4 418.1 255.6 418.2 264.9 408.8L319.9 353.8L374.9 408.8C384.3 418.2 399.5 418.2 408.8 408.8C418.1 399.4 418.2 384.2 408.8 374.9L353.8 319.9L408.8 264.9C418.2 255.5 418.2 240.3 408.8 231C399.4 221.7 384.2 221.6 374.9 231L319.9 286L264.9 231C255.5 221.6 240.3 221.6 231 231z"/>
     </svg>
 </asp:LinkButton>
                <div class="mb-1">Dear Partner,</div>
                <ul class="mb-0 ms-0 ps-3">
                    <li>Your request to withdraw your application has been approved successfully.</li>
                    <li>If you wish to reapply in the future, you may initiate a new application at any time.</li>
                </ul>
            </div>


            <div id="AccountApprovedMessage" runat="server" class="alert alert-success" role="alert">
                <asp:LinkButton CssClass="hideMessagecls" runat="server" ID="LinkButton6" OnClick="hideMessageClick">
     <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="20">
         <path d="M320 112C434.9 112 528 205.1 528 320C528 434.9 434.9 528 320 528C205.1 528 112 434.9 112 320C112 205.1 205.1 112 320 112zM320 576C461.4 576 576 461.4 576 320C576 178.6 461.4 64 320 64C178.6 64 64 178.6 64 320C64 461.4 178.6 576 320 576zM231 231C221.6 240.4 221.6 255.6 231 264.9L286 319.9L231 374.9C221.6 384.3 221.6 399.5 231 408.8C240.4 418.1 255.6 418.2 264.9 408.8L319.9 353.8L374.9 408.8C384.3 418.2 399.5 418.2 408.8 408.8C418.1 399.4 418.2 384.2 408.8 374.9L353.8 319.9L408.8 264.9C418.2 255.5 418.2 240.3 408.8 231C399.4 221.7 384.2 221.6 374.9 231L319.9 286L264.9 231C255.5 221.6 240.3 221.6 231 231z"/>
     </svg>
 </asp:LinkButton>
                <div class="mb-1">Dear Partner,</div>
                <ul class="mb-0 ms-0 ps-3">
                    <li>Your profile has been approved successfully.</li>
                    <li>You may now access all features and continue managing your services.</li>
                </ul>
            </div>
        </div>
        <div class="container-fluid px-0">
            <div class="mb-3 d-flex justify-content-end" style="gap: 10px">
                <button id="btnPersonalInformationView" style="border-radius: 7px" runat="server" onserverclick="btnPersonalInformation_Click">
                    <svg stroke="currentColor" fill="currentColor" stroke-width="0" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="1em" width="1em">
                        <path d="M240 192C240 147.8 275.8 112 320 112C364.2 112 400 147.8 400 192C400 236.2 364.2 272 320 272C275.8 272 240 236.2 240 192zM448 192C448 121.3 390.7 64 320 64C249.3 64 192 121.3 192 192C192 262.7 249.3 320 320 320C390.7 320 448 262.7 448 192zM144 544C144 473.3 201.3 416 272 416L368 416C438.7 416 496 473.3 496 544L496 552C496 565.3 506.7 576 520 576C533.3 576 544 565.3 544 552L544 544C544 446.8 465.2 368 368 368L272 368C174.8 368 96 446.8 96 544L96 552C96 565.3 106.7 576 120 576C133.3 576 144 565.3 144 552L144 544z" />
                    </svg>
                    Personal Information View
                </button>
                <button id="btnBandDetailsView" runat="server" onserverclick="btnBandDetailsView_Click" style="border-radius: 7px;">
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
                <button id="btnCommisionDetailsView" style="border-radius: 7px;" runat="server" onserverclick="btnCommisionDetailsView_Click">
                    <svg stroke="currentColor" fill="currentColor" stroke-width="0" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="1em" width="1em">
                        <path d="M160 128C160 110.3 174.3 96 192 96L456 96C469.3 96 480 106.7 480 120C480 133.3 469.3 144 456 144L379.3 144C397 163.8 409.4 188.6 414 216L456 216C469.3 216 480 226.7 480 240C480 253.3 469.3 264 456 264L414 264C403.6 326.2 353.2 374.9 290.2 382.9L434.6 486C449 496.3 452.3 516.3 442 530.6C431.7 544.9 411.7 548.3 397.4 538L173.4 378C162.1 370 157.3 355.5 161.5 342.2C165.7 328.9 178.1 320 192 320L272 320C307.8 320 338.1 296.5 348.3 264L184 264C170.7 264 160 253.3 160 240C160 226.7 170.7 216 184 216L348.3 216C338.1 183.5 307.8 160 272 160L192 160C174.3 160 160 145.7 160 128z" />
                    </svg>
                    Commision Details View
                </button>
                <button id="btnAccountHistoryView" style="border-radius: 7px;" runat="server" onserverclick="btnAccountHistoryView_Click">
                    <svg stroke="currentColor" fill="currentColor" stroke-width="0" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512" height="1em" width="1em">
                        <path d="M288 64c106 0 192 86 192 192S394 448 288 448c-65.2 0-122.9-32.5-157.6-82.3-10.1-14.5-30.1-18-44.6-7.9s-18 30.1-7.9 44.6C124.1 468.6 201 512 288 512 429.4 512 544 397.4 544 256S429.4 0 288 0C202.3 0 126.5 42.1 80 106.7L80 80c0-17.7-14.3-32-32-32S16 62.3 16 80l0 112c0 17.7 14.3 32 32 32l24.6 0c.5 0 1 0 1.5 0l86 0c17.7 0 32-14.3 32-32s-14.3-32-32-32l-38.3 0C154.9 102.6 217 64 288 64zm24 88c0-13.3-10.7-24-24-24s-24 10.7-24 24l0 104c0 6.4 2.5 12.5 7 17l72 72c9.4 9.4 24.6 9.4 33.9 0s9.4-24.6 0-33.9l-65-65 0-94.1z" />
                    </svg>
                    Account History View
                </button>
            </div>
            <asp:MultiView ID="mvViewType" runat="server" ActiveViewIndex="0">
                <asp:View ID="PersonalInformationList" runat="server">
                    <div class="card">
                        <h5 class="card-header mt-0 text-dark">Profile Details</h5>
                        <div id="RetailerPersonalInfoPanel" runat="server">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblName" CssClass="lblTagName" runat="server" Text="Legal / Firm Name as per GSTIN: " />
                                        <asp:Label ID="lblNameValue" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control text-uppercase" ID="txtFirstName" placeholder="" MaxLength="100" Visible="false" AutoComplete="off" />
                                        <label id="lblFullName" runat="server" visible="false" style="color: red; font-size: 12px">Legal Name as per GSTIN is required.</label>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblGSTIN" CssClass="lblTagName" runat="server" Text="GSTIN: " />
                                        <asp:Label ID="lblGSTINValue" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control text-uppercase" ID="txtGSTIN" placeholder="" MaxLength="15" Visible="false" AutoComplete="off" Enabled="false" />
                                        <label id="lblGSTINError" runat="server" visible="false" style="color: red; font-size: 12px">GSTIN is required.</label>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblCEOName" CssClass="lblTagName" runat="server" Text="Name (CEO / MD / Main Partner / Proprietor): " />
                                        <asp:Label ID="lblCEONameValue" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtCEOName" placeholder="" MaxLength="30" Visible="false" AutoComplete="off" />
                                        <label id="lblCEONameEror" runat="server" visible="false" style="color: red; font-size: 12px">Name (CEO / MD / Main Partner / Proprietor) is required.</label>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblFirmType" CssClass="lblTagName" runat="server" Text="Firm Type: " />
                                        <asp:Label ID="lblFirmTypeValue" runat="server" />
                                        <asp:DropDownList ID="ddlFirmType" runat="server" CssClass="form-control" Visible="false">
                                            <asp:ListItem Text="--Select--" Value="Select"></asp:ListItem>
                                            <asp:ListItem Text="Proprietorship" Value="Proprietorship"></asp:ListItem>
                                            <asp:ListItem Text="Partnership" Value="Partnership"></asp:ListItem>
                                            <asp:ListItem Text="OPC" Value="OPC"></asp:ListItem>
                                            <asp:ListItem Text="LLP" Value="LLP"></asp:ListItem>
                                            <asp:ListItem Text="Private Limited" Value="Private Limited"></asp:ListItem>
                                            <asp:ListItem Text="Limited" Value="Limited"></asp:ListItem>
                                        </asp:DropDownList>
                                        <label id="lblFirmTypeEror" runat="server" visible="false" style="color: red; font-size: 12px">Firm Type is required.</label>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblMobileNo" CssClass="lblTagName" runat="server" Text="Mobile No: " />
                                        <asp:Label ID="lblMobileNoValue" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtMobileNumber" placeholder=" " MaxLength="10" Visible="false" AutoComplete="off"
                                            pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" Enabled="false" />
                                        <label id="lblErrorMobileNo" runat="server" visible="false" style="color: red; font-size: 12px">Mobile No is required.</label>
                                    </div>
                                    <asp:HiddenField ID="hdnCountryCode" runat="server" />
                                    <asp:HiddenField ID="hdnPhoneNumber" runat="server" />
                                </div>
                                <div class="col-md-3">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblAlternateMobileNo" CssClass="lblTagName" runat="server" Text="Alternate Mobile No: " />
                                        <asp:Label ID="lblAlternateMobileNoValue" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtAlternateMobile" placeholder=" " MaxLength="10" Visible="false"
                                            pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" AutoComplete="off"
                                            AutoPostBack="true" OnTextChanged="txtAltCustomerMobile_TextChanged" />
                                        <asp:HiddenField ID="hdnAltCountryCode" runat="server" />
                                        <asp:HiddenField ID="hdnAltMobile" runat="server" />
                                        <label id="lblAlternateMobileNoError" runat="server" visible="false" style="color: red; font-size: 12px">Invalid Alternate Mobile No.</label>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblEmail" CssClass="lblTagName" runat="server" Text="Email ID: " />
                                        <asp:Label ID="lblEmailValue" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control text-lowercase" ID="txtEmail" TextMode="Email" placeholder="" MaxLength="50" Visible="false" AutoComplete="off"
                                            AutoPostBack="true" OnTextChanged="txtCustomerEmail_TextChanged" />
                                        <label id="lblEmailAddress" runat="server" visible="false" style="color: red; font-size: 12px">Email Id is required.</label>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblAltEmail" CssClass="lblTagName" runat="server" Text="Alternate Email ID: " />
                                        <asp:Label ID="lblAltEmailValue" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control text-lowercase" ID="txtAltEmail" TextMode="Email" placeholder="" MaxLength="50" Visible="false" AutoComplete="off"
                                            AutoPostBack="true" OnTextChanged="txtCustomerAltEmail_TextChanged" />
                                        <label id="lblAltEmailError" runat="server" visible="false" style="color: red; font-size: 12px"></label>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblDOB" CssClass="lblTagName" runat="server" Text="Date of Birth: " />
                                        <asp:Label ID="lblDOBValue" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control" ID="TextBox1" placeholder="" Visible="false"
                                            AutoPostBack="true" AutoCompleteType="Disabled" AutoComplete="off" />
                                        <div class="input-group-append" runat="server" visible="false">
                                            <span class="input-group-text" style="cursor: pointer;"
                                                onclick="document.getElementById('<%= TextBox1.ClientID %>').focus();">
                                                <i class="fa fa-calendar"></i>
                                            </span>
                                        </div>
                                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" TargetControlID="TextBox1"
                                            EndDate="<%# DateTime.Today %>"></cc1:CalendarExtender>
                                        <label id="lblDateOfBirth" runat="server" visible="false" style="color: red; font-size: 12px">Date of Birth is required.</label>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblGender" CssClass="lblTagName" runat="server" Text="Gender: " />
                                        <asp:Label ID="lblGenderValue" runat="server" />
                                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control" Visible="false">
                                            <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                            <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblWhatsAppNo" CssClass="lblTagName" runat="server" Text="WhatsApp No 1: " />
                                        <asp:Label ID="lblWhatsAppNoValue" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtWhatsAppNo" placeholder=" " MaxLength="10" Visible="false" AutoComplete="off"
                                            pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)"
                                            AutoPostBack="true" OnTextChanged="txtWhatsAppCustomerMobile_TextChanged" />
                                        <label id="Label3" runat="server" visible="false" style="color: red; font-size: 12px"></label>
                                        <asp:HiddenField ID="hdnWhatsAppNo" runat="server" />
                                        <asp:HiddenField ID="hdnWhatsAppNoCountry" runat="server" />
                                        <label id="lblhdnWhatsAppNo1Error" runat="server" visible="false" style="color: red; font-size: 12px">WhatsApp Mobile No 1 is required.</label>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblWhatsAppNo2" CssClass="lblTagName" runat="server" Text="WhatsApp Mobile No 2: " />
                                        <asp:Label ID="lblWhatsAppNo2Value" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtWhatsAppNo2" placeholder=" " MaxLength="10" Visible="false"
                                            pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" AutoComplete="off"
                                            AutoPostBack="true" OnTextChanged="txtWhatsAppAltCustomerMobile_TextChanged" />
                                        <asp:HiddenField ID="hdnWhatsAppNo2" runat="server" />
                                        <asp:HiddenField ID="hdnWhatsAppNoCountry2" runat="server" />
                                        <label id="lblhdnWhatsAppNo2Error" runat="server" visible="false" style="color: red; font-size: 12px">Invalid WhatsApp Mobile No 2.</label>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <h4 class="regs-text-address">Registered Office Address</h4>
                                </div>
                                <div class="col-md-2">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblPincode" CssClass="lblTagName" runat="server" Text="Pin Code: " />
                                        <asp:Label ID="lblPincodeValue" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtPinCode" AutoPostBack="true" OnTextChanged="txtPinCode_TextChanged" placeholder=""
                                            MaxLength="6" title="Enter a 6-digit Pincode" oninput="validatePincode(this)" Visible="false" AutoComplete="off" />
                                        <label id="lblErrorPincode" runat="server" visible="false" style="color: red; font-size: 12px">PIN Code is required.</label>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblCity" CssClass="lblTagName" runat="server" Text="City: " />
                                        <asp:Label ID="lblCityValue" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblState" CssClass="lblTagName" runat="server" Text="State: " />
                                        <asp:Label ID="lblStateValue" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblAddress" CssClass="lblTagName" runat="server" Text="Address Line 1: " />
                                        <asp:Label ID="lblAddressValue" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtAddress" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="150" Visible="false" AutoComplete="off" />
                                        <label id="lblCurrentAddress" runat="server" visible="false" style="color: red; font-size: 12px">Address Line 1 is required.</label>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblAddress2" CssClass="lblTagName" runat="server" Text="Address Line 2: " />
                                        <asp:Label ID="lblAddress2Value" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtAddress2" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="150" Visible="false" AutoComplete="off" />
                                        <label id="lblAddress2Error" runat="server" visible="false" style="color: red; font-size: 12px"></label>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblLandMark" CssClass="lblTagName" runat="server" Text="Landmark: " />
                                        <asp:Label ID="lblLandMarkValue" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control mb-3" ID="txtLandmark" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="150" Visible="false" AutoComplete="off" />
                                        <label id="lblLandMarkError" runat="server" visible="false" style="color: red; font-size: 12px">Landmark is required.</label>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <h4 class="regs-text-address">Corporate / Main Office Address</h4>
                                </div>
                                <div class="col-md-2">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblDealerPincode" CssClass="lblTagName" runat="server" Text="Pin Code: " />
                                        <asp:Label ID="lblDealerPincodeValue" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtDealerPincode" AutoPostBack="true" OnTextChanged="txtPinCode_TextChanged" placeholder=""
                                            MaxLength="6" title="Enter a 6-digit Pincode" oninput="validatePincode(this)" Visible="false" AutoComplete="off" />
                                        <label id="lblDealerPincodeError" runat="server" visible="false" style="color: red; font-size: 12px">PIN Code is required.</label>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblDealerCity" CssClass="lblTagName" runat="server" Text="City: " />
                                        <asp:Label ID="lblDealerCityValue" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblDealerState" CssClass="lblTagName" runat="server" Text="State: " />
                                        <asp:Label ID="lblDealerStateValue" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblDealerAddressLine1" CssClass="lblTagName" runat="server" Text="Address Line 1: " />
                                        <asp:Label ID="lblDealerAddressLine1Value" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtDealerAddressLine1" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="150" Visible="false" AutoComplete="off" />
                                        <label id="lblDealerAddressLine1Error" runat="server" visible="false" style="color: red; font-size: 12px">Address Line 1 is required.</label>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblDealerAddressLine2" CssClass="lblTagName" runat="server" Text="Address Line 2: " />
                                        <asp:Label ID="lblDealerAddressLine2Value" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtDealerAddressLine2" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="150" Visible="false" AutoComplete="off" />
                                        <label id="lblDealerAddressLine2Error" runat="server" visible="false" style="color: red; font-size: 12px"></label>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblDealerLandmark" CssClass="lblTagName" runat="server" Text="Landmark: " />
                                        <asp:Label ID="lblDealerLandmarkValue" runat="server" />
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtDealerLandmark" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="150" Visible="false" AutoComplete="off" />
                                        <label id="lblDealerLandmarkError" runat="server" visible="false" style="color: red; font-size: 12px">Landmark is required.</label>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="profile-txt-area">
                                         <asp:Label ID="lblProfileImage" CssClass="lblTagName" runat="server" Text="Profile Image:" />
                                        <asp:Image ID="imgProfile" runat="server" CssClass="img-thumbnail mt-2" Width="120px" Height="120px" ImageUrl="../assets/images/avatar5.png" />
                                        <asp:FileUpload ID="fuProfileImage" runat="server" CssClass="form-control mt-2" Visible="false" accept=".jpg,.jpeg,.png" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="profile-txt-area">
                                        <asp:Label ID="lblCompanyLogo" CssClass="lblTagName" runat="server" Text="Company Logo: " />
                                        <asp:Image ID="imgCompanyLogo" runat="server" CssClass="img-thumbnail mt-2" Width="30%" Height="30%" ImageUrl="../assets/images/avatar5.png" />
                                        <asp:FileUpload ID="fuCompanyLogo" runat="server" CssClass="form-control mt-2" Visible="false" accept=".jpg,.jpeg,.png" />
                                    </div>
                                </div>
                            </div>
                            <div class="row mt-3 px-2">
                                <div class="w-100 d-flex justify-content-end">
                                    <asp:Button ID="btnEditProfile" runat="server" Text="Edit Profile" CssClass="btn btn-primary next-step" OnClick="btnEditProfile_Click" />
                                    <asp:Button ID="btnCancelProfile" runat="server" Text="Cancel" CssClass="btn prev-step bg-dark mr-3" Visible="false" OnClick="btnCancelProfile_Click" />
                                    <asp:Button ID="btnUpdateProfile" runat="server" Text="Update Profile" CssClass="btn btn-primary next-step" Visible="false" OnClick="btnUpdateProfile_Click" />
                                </div>
                            </div>
                        </div>
                        </div>
                        <div id="SalesPersonPersonalInfoPanel" runat="server">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonName" CssClass="lblTagName" runat="server" Text="Name : " />
                                            <asp:Label ID="lblSalesPersonNameValue" runat="server" />
                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtSalesPersonName" placeholder="" MaxLength="100" Visible="false" AutoComplete="off" />
                                            <label id="lblSalesPersonNameError" runat="server" visible="false" style="color: red; font-size: 12px">Name is required.</label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonMobileNo" CssClass="lblTagName" runat="server" Text="Mobile No: " />
                                            <asp:Label ID="lblSalesPersonMobileNoValue" runat="server" />
                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtSalesPersonMobileNo" placeholder=" " MaxLength="10" Visible="false" AutoComplete="off"
                                                pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" Enabled="false" />
                                            <label id="lblSalesPersonMobileNoError" runat="server" visible="false" style="color: red; font-size: 12px">Mobile No is required.</label>
                                        </div>
                                        <asp:HiddenField ID="hdnSalesPersonCountry" runat="server" />
                                        <asp:HiddenField ID="hdnSalesPersonCode" runat="server" />
                                    </div>
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonAltMobileNo" CssClass="lblTagName" runat="server" Text="Alternate Mobile No: " />
                                            <asp:Label ID="lblSalesPersonAltMobileNoValue" runat="server" />
                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtSalesPersonAltMobileNo" placeholder=" " MaxLength="10" Visible="false"
                                                pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" AutoComplete="off"
                                                AutoPostBack="true" OnTextChanged="txtAltSalesPersonMobile_TextChanged" />
                                            <asp:HiddenField ID="hdnSalesPersonAltCountry" runat="server" />
                                            <asp:HiddenField ID="hdnSalesPersonAltCode" runat="server" />
                                            <label id="lblSalesPersonAltMobileNoError" runat="server" visible="false" style="color: red; font-size: 12px">Invalid Alternate Mobile No.</label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonDOB" CssClass="lblTagName" runat="server" Text="Date of Birth: " />
                                            <asp:Label ID="lblSalesPersonDOBValue" runat="server" />
                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtSalesPersonDOB" placeholder="" Visible="false"
                                                AutoPostBack="true" AutoCompleteType="Disabled" AutoComplete="off" />
                                            <div class="input-group-append" runat="server" visible="false">
                                                <span class="input-group-text" style="cursor: pointer;"
                                                    onclick="document.getElementById('<%= TextBox1.ClientID %>').focus();">
                                                    <i class="fa fa-calendar"></i>
                                                </span>
                                            </div>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtSalesPersonDOB"
                                                EndDate="<%# DateTime.Today %>"></cc1:CalendarExtender>
                                            <label id="lblSalesPersonDOBError" runat="server" visible="false" style="color: red; font-size: 12px">Date of Birth is required.</label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonEmail" CssClass="lblTagName" runat="server" Text="Email ID: " />
                                            <asp:Label ID="lblSalesPersonEmailValue" runat="server" />
                                            <asp:TextBox runat="server" CssClass="form-control text-lowercase" ID="txtSalesPersonEmail" TextMode="Email" placeholder="" MaxLength="50" Visible="false" AutoComplete="off"
                                                AutoPostBack="true" OnTextChanged="txtSalesPersonEmail_TextChanged" />
                                            <label id="lblSalesPersonEmailError" runat="server" visible="false" style="color: red; font-size: 12px">Email Id is required.</label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonAltEmail" CssClass="lblTagName" runat="server" Text="Alternate Email ID: " />
                                            <asp:Label ID="lblSalesPersonAltEmailValue" runat="server" />
                                            <asp:TextBox runat="server" CssClass="form-control text-lowercase" ID="txtSalesPersonAltEmail" TextMode="Email" placeholder="" MaxLength="50" Visible="false" AutoComplete="off"
                                                AutoPostBack="true" OnTextChanged="txtSalesPersonAltEmail_TextChanged" />
                                            <label id="lblSalesPersonAltEmailError" runat="server" visible="false" style="color: red; font-size: 12px"></label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonWhatsapp1" CssClass="lblTagName" runat="server" Text="WhatsApp No 1: " />
                                            <asp:Label ID="lblSalesPersonWhatsapp1Value" runat="server" />
                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtSalesPersonWhatsapp1" placeholder=" " MaxLength="10" Visible="false" AutoComplete="off"
                                                pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)"
                                                AutoPostBack="true" OnTextChanged="txtWhatsAppSalerPersonMobile_TextChanged" />
                                            <label id="Label33" runat="server" visible="false" style="color: red; font-size: 12px"></label>
                                            <asp:HiddenField ID="hdnSalesPersonWhatsapp1Country" runat="server" />
                                            <asp:HiddenField ID="hdnSalesPersonWhatsapp1Code" runat="server" />
                                            <label id="lblSalesPersonWhatsapp1Error" runat="server" visible="false" style="color: red; font-size: 12px">WhatsApp Mobile No 1 is required.</label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonWhatsap2" CssClass="lblTagName" runat="server" Text="WhatsApp Mobile No 2: " />
                                            <asp:Label ID="lblSalesPersonWhatsap2Value" runat="server" />
                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtSalesPersonWhatsapp2" placeholder=" " MaxLength="10" Visible="false"
                                                pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" AutoComplete="off"
                                                AutoPostBack="true" OnTextChanged="txtWhatsAppAltSalesPersonMobile_TextChanged" />
                                            <asp:HiddenField ID="hdnSalesPersonWhatsapp2Country" runat="server" />
                                            <asp:HiddenField ID="hdnSalesPersonWhatsapp2Code" runat="server" />
                                            <label id="lblSalesPersonWhatsap2Error" runat="server" visible="false" style="color: red; font-size: 12px">Invalid WhatsApp Mobile No 2.</label>
                                        </div>
                                    </div>
                                    
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonGender" CssClass="lblTagName" runat="server" Text="Gender: " />
                                            <asp:Label ID="lblSalesPersoGenderValue" runat="server" />
                                            <asp:DropDownList ID="ddlSalesPersonGender" runat="server" CssClass="form-control" Visible="false">
                                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                                <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonPincode" CssClass="lblTagName" runat="server" Text="Pin Code: " />
                                            <asp:Label ID="lblSalesPersonPincodeValue" runat="server" />
                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtSalesPersonPincode" AutoPostBack="true" OnTextChanged="txtSalesPersonPinCode_TextChanged" placeholder=""
                                                MaxLength="6" title="Enter a 6-digit Pincode" oninput="validatePincode(this)" Visible="false" AutoComplete="off" />
                                            <label id="lblSalesPersonPincodeError" runat="server" visible="false" style="color: red; font-size: 12px">PIN Code is required.</label>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonCity" CssClass="lblTagName" runat="server" Text="City: " />
                                            <asp:Label ID="lblSalesPersonCityValue" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonState" CssClass="lblTagName" runat="server" Text="State: " />
                                            <asp:Label ID="lblSalesPersonStateValue" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonAddress" CssClass="lblTagName" runat="server" Text="Address: " />
                                            <asp:Label ID="lblSalesPersonAddressValue" runat="server" />
                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtSalesPersonAddress" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="150" Visible="false" AutoComplete="off" />
                                            <label id="lblSalesPersonAddressError" runat="server" visible="false" style="color: red; font-size: 12px">Address is required.</label>
                                        </div>
                                    </div>
                                  
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonAdminName" CssClass="lblTagName" runat="server" Text="Associate Name: " />
                                            <asp:Label ID="lblSalesPersonAdminNameValue" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonAdminEmail" CssClass="lblTagName" runat="server" Text="Associate Email ID: " />
                                            <asp:Label ID="lblSalesPersonAdminEmailValue" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonAdminPhoneNo" CssClass="lblTagName" runat="server" Text="Associate Mobile No: " />
                                            <asp:Label ID="lblSalesPersonAdminPhoneNoValue" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="profile-txt-area">
                                            <asp:Label ID="lblSalesPersonProfileImage" CssClass="lblTagName" runat="server" Text="Profile Image:" />
                                            <asp:Image ID="imgSalesPerson" runat="server" CssClass="img-thumbnail mt-2" Width="120px" Height="120px" ImageUrl="../assets/images/avatar5.png" />
                                            <asp:FileUpload ID="fuSalesPersonProfileImage" runat="server" CssClass="form-control mt-2" Visible="false" accept=".jpg,.jpeg,.png" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row mt-3 px-2">
                                    <div class="w-100 d-flex justify-content-end">
                                        <asp:Button ID="btnSalesPersonEdit" runat="server" Text="Edit Profile" CssClass="btn btn-primary next-step" OnClick="btnEditSalesPersonProfile_Click" />
                                        <asp:Button ID="btnSalesPersonCancel" runat="server" Text="Cancel" CssClass="btn prev-step bg-dark mr-3" Visible="false" OnClick="btnCancelSalesPersonProfile_Click" />
                                        <asp:Button ID="btnSalesPersonSave" runat="server" Text="Update Profile" CssClass="btn btn-primary next-step" Visible="false" OnClick="btnUpdateSalesPersonProfile_Click" />
                                    </div>
                                </div>
                          
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="BandDetails" runat="server">
                    <div class="row">
                        <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                            <div class="card">
                                <div class="card-header">
                                    <div class="d-flex justify-content-between">
                                        <h5 class="mb-0">Bank Details </h5>
                                        <div class="col-md-1">
                                            <asp:Button ID="btnAddBank" runat="server" class="btn next-step" Text="Add Bank" OnClick="btnAddBank_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="table-responsive">
                                        <asp:Repeater ID="RepeaterBankDetails" runat="server" OnItemCommand="RepeaterBankDetails_ItemCommand">
                                            <HeaderTemplate>
                                                <table id="example44" class="table-responsive table data-table table-striped table-bordered nowrap" style="width: 100%">
                                                    <thead class="thead">
                                                        <tr>
                                                            <th>S.No.</th>
                                                            <th>View</th>
                                                            <th>Make Primary</th>
                                                            <th>Status</th>
                                                            <th>Bank Account Number</th>
                                                            <th>IFSC Code</th>
                                                            <th>Bank Name</th>
                                                            <th>Bank Branch</th>
                                                            <th>Branch Address</th>
                                                            <th>Account Holder Name</th>
                                                            <th>UPIID</th>
                                                            <th>Type of Bank Account</th>
                                                            <th>Is This Joint Account</th>
                                                            <th>Joint Account Holder Name</th>
                                                            <th>Supporting Documents</th>
                                                            <th>Created Date</th>
                                                            <th>IP Address</th>
                                                            <th>Action</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr class='<%# Eval("Status").ToString() == "Primary" ? "table-success" : "" %>'>
                                                    <td><%# Container.ItemIndex + 1 %></td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkViewDocument" runat="server" CommandArgument='<%# Eval("SupportingDocumentsPath") %>' CommandName="ViewDocument"
                                                            ToolTip="View" OnClientClick='<%# !String.IsNullOrEmpty(Eval("SupportingDocumentsPath")?.ToString()) 
                                                        ? "window.open(\"" + ResolveUrl("~/UploadedDocuments/") + Eval("SupportingDocumentsPath") + "\", \"_blank\"); return false;" : "return false;" %>'
                                                            Enabled='<%# !String.IsNullOrEmpty(Eval("SupportingDocumentsPath")?.ToString()) %>'>
                                                     <i class='<%# !String.IsNullOrEmpty(Eval("SupportingDocumentsPath")?.ToString()) ? "fa fa-eye text-success" : "fa fa-eye-slash text-danger" %>' style="font-size:18px;"></i>
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:LinkButton ID="lnkMakeActive" runat="server" CommandName="MakeActive" CommandArgument='<%# Eval("Mid") %>'
                                                            ToolTip="Make Primary" OnClientClick="return confirm('Set this as the Primary account?');">
                                                    <i class="fa fa-check-circle text-success" style="font-size:18px;"></i>
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td><asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' Visible="false"></asp:Label><%# Eval("Status") %></td>
                                                    <td><%# Eval("BankAccountNumber") %></td>
                                                    <td><%# Eval("IFSCCode") %></td>
                                                    <td><%# Eval("BankName") %></td>
                                                    <td><%# Eval("BankBranch") %></td>
                                                    <td><%# Eval("BankBranchAddress") %></td>
                                                    <td><%# Eval("AccountHolderName") %></td>
                                                    <td><%# Eval("UPIID") %></td>
                                                    <td><%# Eval("TypeofBankAccount") %></td>
                                                    <td><%# Eval("IsThisYourJointAccount") %></td>
                                                    <td><%# Eval("JointAccountHolderName") %></td>
                                                    <td><%# Eval("DocumentName") %></td>
                                                    <td><%# Eval("CreatedDate", "{0:dd-MMM-yyyy}") %></td>
                                                    <td><%# Eval("IPAddress") %></td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditBank" CommandArgument='<%# Eval("Mid") %>'
                                                            ToolTip="Edit">
                                                <i class="fa fa-edit text-primary mr-2"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" Visible="false" CommandName="DeleteBank" CommandArgument='<%# Eval("Mid") %>'
                                                            ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                                    <i class="fa fa-trash text-danger"></i>
                                                        </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                                        </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        <asp:Label ID="lblNoBankDetails" runat="server" CssClass="text-center text-danger fw-bold" Visible="false">
                                   <b>Important:</b> Please ensure that you provide complete and accurate Bank details, including your Account Number, IFSC Code, and Bank Name. Submission of these details is mandatory.
                                        </asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>

                <%--     <div class="row">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <div class="card">
                        <div class="card-header">
                            <div class="d-flex justify-content-between">
                                <h5 class="mb-0">Dealer Details</h5>
                                <div class="col-md-1">
                                    <asp:Button ID="btnAddDealer" runat="server" class="btn btn-primary" Text="Add Dealer" OnClick="btnAddDealer_Click" Visible="false" />
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:Repeater ID="RepeaterEmployeeDetails" runat="server" OnItemCommand="RepeaterDealerDetails_ItemCommand">
                                    <HeaderTemplate>
                                        <div class="table-responsive">
                                            <table id="example45" class="table data-table table-striped table-bordered nowrap" style="width: 100%">
                                                <thead class="thead">
                                                    <tr>
                                                        <th>S.No.</th>
                                                        <th>Firm Name</th>
                                                        <th>GSTIN</th>
                                                        <th>Address</th>
                                                        <th>City</th>
                                                        <th>State</th>
                                                        <th>Action</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Container.ItemIndex + 1 %></td>
                                            <td><%# Eval("SellerName") %></td>
                                            <td><%# Eval("SellerGSTINNo") %></td>
                                            <td><%# Eval("AddressLine1") %>  <%# Eval("AddressLine2") %></td>
                                            <td><%# Eval("City") %></td>
                                            <td><%# Eval("State") %></td>
                                            <td>
                                                <asp:LinkButton ID="lnkEditDealer" runat="server" CommandName="EditDealer" CommandArgument='<%# Eval("Mid") %>'
                                                    ToolTip="Edit" >
                                                    <i class="fa fa-edit text-primary"></i>
                                                </asp:LinkButton>
                                                &nbsp;
                                                <asp:LinkButton ID="lnkDeleteDealer" runat="server" Visible="false" CommandName="DeleteDealer" CommandArgument='<%# Eval("Mid") %>'
                                                    ToolTip="Delete" CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                                    <i class="fa fa-trash"></i>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                        </table>
                                        </div>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>
                <asp:View ID="UploadedDocumentList" runat="server">
                    <div class="row">
                        <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                            <div class="card">
                                <div class="card-header">
                                    <div class="d-flex justify-content-between">
                                        <h5 class="mb-0">Uploaded Documents</h5>
                                        <div>
                                            <asp:Button ID="btnUploadDocument" runat="server" class="btn next-step" Text="Add Documents" OnClick="btnUploadDocument_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="table-responsive">
                                        <asp:Repeater ID="rptDocuments" runat="server" OnItemCommand="RepeaterDocumentDetails_ItemCommand">
                                            <HeaderTemplate>
                                                <div class="table-responsive">
                                                    <table class="table data-table table-bordered nowrap">
                                                        <thead class="thead">
                                                            <tr>
                                                                <th>S.No.</th>
                                                                <th>View</th>
                                                                <th>Document Number</th>
                                                                <th>Document Name</th>
                                                                <th>Status</th>
                                                                <th>Remarks</th>
                                                                <th>Action By</th>
                                                                <th>Action Date</th>
                                                                <%--<th>Action</th>--%>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <tr style='<%# GetRowStyle(Eval("Status")?.ToString()) %>'>
                                                    <td><%# Container.ItemIndex + 1 %></td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkViewDocument" runat="server" CommandArgument='<%# Eval("DocumentPath") %>' CommandName="ViewDocument"
                                                            ToolTip="View" OnClientClick='<%# "window.open(\"" + ResolveUrl("~/UploadedDocuments/") + Eval("DocumentPath") + "\", \"_blank\"); return false;" %>'
                                                            Enabled='<%# !String.IsNullOrEmpty(Eval("DocumentPath")?.ToString()) %>'>
                                                    <i class="fa fa-eye text-success" style="font-size:18px;"></i>
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td><%# Eval("documentNumber") %></td>
                                                    <td><%# Eval("DocumentName") %></td>
                                                    <%--<td><%# Eval("Status") %></td>--%>
                                                    <td><%# string.IsNullOrEmpty(Eval("Status")?.ToString()) ? "Under Verification" : Eval("Status") %></td>
                                                    <td><%# Eval("Remarks") %></td>
                                                    <td><%# Eval("ActionBy") %></td>
                                                    <td><%# Eval("ActionDate") %></td>
                                                    <%-- <td>
                                                <asp:LinkButton ID="lnkEditDoc" runat="server" CommandName="Editdoc" CommandArgument='<%# Eval("Mid") %>'
                                                    ToolTip="Edit">
                                                    <i class="fa fa-edit text-primary"></i>
                                                </asp:LinkButton>
                                                &nbsp;
                                                <asp:LinkButton ID="lnkDeleteDoc" runat="server" CommandName="Deletedoc" CommandArgument='<%# Eval("Mid") %>'
                                                    ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                                    <i class="fa fa-trash text-danger"></i>
                                                </asp:LinkButton>
                                            </td>--%>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                                    </table>
                                        </div>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        <asp:Label ID="lblNoDocuments" runat="server" CssClass="text-center text-danger fw-bold" Visible="false">
                                    <b>Important</b>: Please upload your Aadhaar card, PAN card, and GST certificate. Submission of all three documents is mandatory. Your request may not be processed if any of the required documents are missing.
                                        </asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>

                <asp:View ID="CommisionDetailsList" runat="server">
                    <div class="row">
                        <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                            <div class="card">
                                <div class="card-header">
                                    <div class="d-flex justify-content-between">
                                        <h5 class="mb-0">Commission Details </h5>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="table-responsive">
                                        <asp:Repeater ID="rptCommissionDetails" runat="server" OnItemCommand="RepeaterBankDetails_ItemCommand">
                                            <HeaderTemplate>
                                                <table id="example44" class="table data-table table-striped table-bordered nowrap" style="width: 100%">
                                                    <thead class="thead">
                                                        <tr>
                                                            <th>S.No.</th>
                                                            <th>Type</th>
                                                            <th>Commission Percentage</th>
                                                            <th>Commission Value</th>
                                                            <th>Max Commission Value</th>
                                                            <th>Min Commission Value</th>
                                                            <th>Plan Category</th>
                                                            <th>Plan Qty Condition</th>
                                                            <th>Plan Qty</th>
                                                            <th>Plan Status</th>
                                                            <th>Category</th>
                                                            <th>Sub Category</th>
                                                            <th>Product Type</th>
                                                            <th>Brands</th>
                                                            <th>Commission Status</th>
                                                            <th>Recurring Status</th>
                                                            <th>Internal Remark</th>
                                                            <th>External Remark</th>
                                                            <th>Plan Qty Start Slab</th>
                                                            <th>Plan Qty End Slab</th>
                                                            <th>Valid From</th>
                                                            <th>Valid To</th>
                                                            <th>Created At</th>
                                                            <th>Created By</th>
                                                            <th>Created By Designation</th>
                                                            <th>Updated At</th>
                                                            <th>Updated By</th>
                                                            <th>Updated By Designation</th>
                                                            <th>Status</th>
                                                            <th>check_status</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%# Container.ItemIndex + 1 %></td>
                                                    <td><%# Eval("Type") %></td>
                                                    <td><%# Eval("CommissionPercentage") %></td>
                                                    <td><%# Eval("CommissionValue") %></td>
                                                    <td><%# Eval("MaxCommissionValue") %></td>
                                                    <td><%# Eval("MinCommissionValue") %></td>
                                                    <td><%# Eval("PlanCategory") %></td>
                                                    <td><%# Eval("PlanQtyCondition") %></td>
                                                    <td><%# Eval("PlanQty") %></td>
                                                    <td><%# Eval("PlanStatus") %></td>
                                                    <td><%# Eval("Category") %></td>
                                                    <td><%# Eval("SubCategory") %></td>
                                                    <td><%# Eval("ProductType") %></td>
                                                    <td><%# Eval("Brands") %></td>
                                                    <td><%# Eval("CommissionStatus") %></td>
                                                    <td><%# Eval("RecurringStatus") %></td>
                                                    <td><%# Eval("InternalRemark") %></td>
                                                    <td><%# Eval("ExternalRemark") %></td>
                                                    <td><%# Eval("PlanQtyStartSlab") %></td>
                                                    <td><%# Eval("PlanQtyEndSlab") %></td>
                                                    <td><%# Eval("ValidFrom", "{0:dd-MMM-yyyy}") %></td>
                                                    <td><%# Eval("ValidTo", "{0:dd-MMM-yyyy}") %></td>
                                                    <td><%# Eval("CreatedAt", "{0:dd-MMM-yyyy}") %></td>
                                                    <td><%# Eval("CreatedBy") %></td>
                                                    <td><%# Eval("CreatedByDesignation") %></td>
                                                    <td><%# Eval("UpdatedAt", "{0:dd-MMM-yyyy}") %></td>
                                                    <td><%# Eval("UpdatedBy") %></td>
                                                    <td><%# Eval("UpdatedByDesignation") %></td>
                                                    <td><%# Eval("Status") %></td>
                                                    <td><%# Eval("check_status") %></td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                             </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        <asp:Label ID="lblNoCommission" runat="server" CssClass="text-center text-danger fw-bold" Visible="false">
                                    <b>Note</b>: Commission details will be available once the approval process is completed.
                                        </asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="AccountHistoryList" runat="server">
                    <div class="card">
                        <div class="card-header">
                            <div class="d-flex justify-content-between">
                                <h5 class="mb-0">Account History </h5>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="row justify-content-end mb-3">
                                <button id="btnwithdrawalRequest" runat="server" class="btn btn-info d-flex bg-blue" visible="false" onserverclick="btnwithdrawalRequest_ServerClick">
                                    <%--<svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24" height="18" width="18" xmlns="http://www.w3.org/2000/svg">
                                        <path fill="none" d="M0 0h24v24H0V0z"></path>
                                        <path d="M20 2H4c-1.1 0-1.99.9-1.99 2L2 22l4-4h14c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2zm0 14H5.17l-.59.59-.58.58V4h16v12zm-9-4h2v2h-2zm0-6h2v4h-2z"></path>
                                    </svg>--%>
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24"  height="18" width="18" xmlns="http://www.w3.org/2000/svg">
                                        <path d="M14 14.252V16.3414C13.3744 16.1203 12.7013 16 12 16C8.68629 16 6 18.6863 6 22H4C4 17.5817 7.58172 14 12 14C12.6906 14 13.3608 14.0875 14 14.252ZM12 13C8.685 13 6 10.315 6 7C6 3.685 8.685 1 12 1C15.315 1 18 3.685 18 7C18 10.315 15.315 13 12 13ZM12 11C14.21 11 16 9.21 16 7C16 4.79 14.21 3 12 3C9.79 3 8 4.79 8 7C8 9.21 9.79 11 12 11ZM19 17.5858L21.1213 15.4645L22.5355 16.8787L20.4142 19L22.5355 21.1213L21.1213 22.5355L19 20.4142L16.8787 22.5355L15.4645 21.1213L17.5858 19L15.4645 16.8787L16.8787 15.4645L19 17.5858Z"></path>
                                    </svg>
                                    Withdrawal Request
                                </button>
                            </div>
                            <div id="feedbackSection" runat="server" visible="false">
                                <label>Current Status: <asp:Label ID="lblCurruntStatus" runat="server" Font-Bold="true"></asp:Label></label>
                                <div class="form-group">
                                    <textarea class="form-control" rows="4" placeholder="Write your Remarks here..." runat="server" id="txtRemarks"></textarea>
                                </div>
                                <div class="mt-2 d-flex justify-content-center">
                                    <%--<button type="submit" id="btnCancelFeedback" class="submit_btn2 btn mr-3" onserverclick="btnCancelFeedback_Click" runat="server"><i class="far fa-times-circle mr-1"></i>Cancel</button>
                                <button type="submit" id="btnSubmitFeedback" class="submit_btn btn" runat="server" onserverclick="btnSubmitFeedback_Click"><i class="fas fa-paper-plane mr-1"></i>Submit</button>--%>
                                    <asp:Button ID="btnCancelWithdrawProfile" runat="server" Text="Cancel Withdraw" OnClientClick="return confirm('Are you sure you want to Cancel Withdraw Account?');" CssClass="btn btn-primary next-step mr-2" OnClick="btnCancelWithdrawProfile_Click" />
                                    <asp:Button ID="btnWithdrawProfile" runat="server" Text="Withdraw Account" OnClientClick="return confirm('Are you sure you want to Withdraw Account?');" CssClass="btn btn-primary next-step mr-2" OnClick="btnWithdrawProfile_Click" />
                                    <button type="submit" id="btnCancelFeedback" class="submit_btn2 btn mr-3" onserverclick="btnCancel_Click" runat="server"><i class="far fa-times-circle mr-1"></i>Cancel</button>
                                </div>
                            </div>
                            <div class="mt-3">
                               <%-- <asp:GridView ID="GVAccountHistory" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead" EmptyDataText="No records available. Please refine your search.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MobileNumber" HeaderText="Mobile No" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Action By" />
                                        <asp:BoundField DataField="CreatedAt" HeaderText="Action Date" />
                                    </Columns>
                                </asp:GridView>--%>
                                <div class="table-responsive">
                                    <asp:Repeater ID="GVAccountHistory" runat="server">
                                        <HeaderTemplate>
                                            <table id="example44" class="table data-table table-striped table-bordered nowrap" style="width: 100%">
                                                <thead class="thead">
                                                    <tr>
                                                        <th>S.No.</th>
                                                        <%--<th>Mobile No</th>--%>
                                                        <th>Status</th>
                                                        <th>Remarks</th>
                                                        <%--<th>Action By</th>--%>
                                                        <th>Action Date</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Container.ItemIndex + 1 %></td>
                                                <%--<td><%# Eval("MobileNumber") %></td>--%>
                                                <td><%# Eval("Status") %></td>
                                                <td><%# Eval("Remarks") %></td>
                                                <%--<td><%# Eval("CreatedBy") %></td>--%>
                                                <td><%# Eval("CreatedAt", "{0:dd-MMM-yyyy}") %></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>

    </asp:Panel>
</asp:Content>
