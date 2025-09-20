<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateAnAccount.aspx.cs" Inherits="Patner_Retailer_ADO.CreateAnAccount" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Retailer</title>
    <link rel="icon" type="image/png" href="../assets/images/Infyshield-logo.png" />
    <meta name="Description" content="">
    <meta name="Author" content="">
    <meta name="keywords" content="">
    <link id="style" href="assets/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <link href="assets/css/signup.css" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">

    <style>
        @media (max-width:767px) {
            .leftArea {
                width: 100%;
                height: auto;
                padding: 20px 0;
                display: none;
            }


            .RightArea {
                width: 100%;
                height: auto;
                padding: 20px 0;
            }

            .signup-step-container {
                padding: 0px;
            }
        }

        .text-uppercase {
            text-transform: uppercase;
        }

        .same-small-txt label {
            margin-left: 5px;
            font-size: 13px !important;
            font-weight: 400 !important;
        }

        .wizard label {
            font-size: 14px;
            font-weight: 500;
        }

        .wizard input, .wizard select, .wizard textarea {
            font-size: 13px;
        }

        .wizard .form-group span {
            font-size: 13px;
        }


        .next-step {
            text-decoration: none;
        }

        #btnLogin:hover {
            color: #fff;
        }

        .prev-step {
            text-decoration: none;
        }

        .skip-btn {
            border: 1px solid #dc3545;
            color: #fff;
            font-size: 13px;
            border-radius: 5px;
            padding: 9px 8px !important;
            letter-spacing: .5px;
            text-decoration: none;
            background: #dc3545 !important;
        }

        .Error-Message {
            font-size: 12px !important;
            color: red;
            display: none;
            font-weight: 400 !important;
            margin-bottom: 0px;
        }

        .chksameAddress label {
            font-size: 13px;
            font-weight: 400;
            padding-left: 5px;
        }

        .JointAccountpnl tbody {
            display: flex;
            gap: 20px;
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

        .prev-step, .next-step {
            margin-top: 0px;
        }
    </style>

</head>
<body>
    <main>

        <form id="signupForm" runat="server">

            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <div class="warpper-main">
                <div class="wrapper-content d-lg-flex">
                    <div class="leftArea">
                        <div class="Logo">
                            <img src="assets/images/logo.png" alt="" />
                        </div>
                        <div class="Content-area">
                            <h2>Welcome to the World of Hassle-free<br>
                                Assurance Solutions</h2>
                            <ul>
                                <li><i class="fa fa-shield-alt" aria-hidden="true">&nbsp;</i> Easy for the Customer to Understand</li>
                                <li><i class="fa fa-shield-alt" aria-hidden="true">&nbsp;</i> Easy for Our Partners to Sell</li>
                                <li><i class="fa fa-shield-alt" aria-hidden="true">&nbsp;</i> Healthy Margins</li>
                                <li><i class="fa fa-shield-alt" aria-hidden="true">&nbsp;</i> Easy for us to Manage</li>
                                <li><i class="fa fa-shield-alt" aria-hidden="true">&nbsp;</i> No Risk to Our Partners</li>
                            </ul>
                        </div>
                    </div>
                    <div class="RightArea">
                        <div class="d-flex justify-content-end mt-3 logout_btn_top">
                            <asp:LinkButton ID="btnLogout" runat="server" class="btn btn-danger" AutoPostBack="true" OnClick="btnLogout_Click"><i class="fa fa-power-off"></i></asp:LinkButton>
                        </div>
                        <section class="signup-step-container mt-5">
                            <div class="container">
                                <div class="row d-flex justify-content-center">
                                    <div class="col-md-12">
                                        <div class="wizard">
                                            <div class="wizard-inner">
                                                <asp:HiddenField ID="hdnActiveTab" runat="server" />
                                                <div class="connecting-line"></div>

                                                <ul class="nav nav-tabs" role="tablist">
                                                    <li role="presentation" class="active">
                                                        <a href="#step2" data-toggle="tab" aria-controls="step2" role="tab"
                                                            aria-expanded="true"><span class="round-tab"></span><i>Personal Info</i>
                                                        </a>
                                                    </li>
                                                    <%--  <li role="presentation" class="disabled">
                                                        <a href="#step1" data-toggle="tab" aria-controls="step1" role="tab"
                                                            aria-expanded="false"><span class="round-tab"></span><i>Address Info</i>
                                                        </a>
                                                    </li>--%>
                                                    <li role="presentation" class="disabled">
                                                        <a href="#step3" data-toggle="tab" aria-controls="step3" role="tab"
                                                            aria-expanded="false"><span class="round-tab"></span>
                                                            <i>Bank Detail</i>
                                                        </a>
                                                    </li>
                                                    <li role="presentation" class="disabled">
                                                        <a href="#step4" data-toggle="tab" aria-controls="step4"
                                                            role="tab"><span class="round-tab"></span>
                                                            <i>Upload Document</i>
                                                        </a>
                                                    </li>
                                                    <li role="presentation" class="disabled">
                                                        <a href="#step5" data-toggle="tab" aria-controls="step5"
                                                            role="tab"><span class="round-tab"></span>
                                                            <i>Confirmation</i></a>
                                                    </li>
                                                </ul>
                                            </div>
                                            <form class="login-box">
                                                <div class="tab-content" id="main_form">
                                                    <div class="tab-pane active" role="tabpanel" id="step2">
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2">
                                                                    <label class="mb-1">Legal / Firm Name as per GSTIN<span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control text-uppercase" ID="txtSellerName" AutoComplete="off" placeholder=""
                                                                        MaxLength="30" oninput="hideSellerName(this, document.getElementById('lblSellerName'))"></asp:TextBox>
                                                                    <p id="lblSellerName" runat="server" class="Error-Message">Legal Name is required.</p>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2">
                                                                    <label class="mb-1">GSTIN <span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtGSTIN" AutoComplete="off" placeholder="" MaxLength="15"></asp:TextBox>
                                                                    <p id="lblSellerGSTIN" runat="server" class="Error-Message">GSTIN is required.</p>
                                                                </div>
                                                            </div>


                                                            <%-- <h5>Personal Information</h5>--%>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2">
                                                                    <label class="mb-1">Name (CEO / MD / Main Partner / Proprietor) <span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" oninput="hideSellerName(this, document.getElementById('lblFullName'))" ID="txtFirstName" placeholder="" MaxLength="30" AutoComplete="off"></asp:TextBox>
                                                                    <p id="lblFullName" runat="server" class="Error-Message">Owner Name is required.</p>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2">
                                                                    <label class="mb-1">Firm Type <span style="color: red">*</span> </label>
                                                                    <asp:DropDownList ID="ddlFirmType" runat="server" CssClass="form-control"
                                                                        onchange="hideSellerName(this, document.getElementById('lblFirmTypeError'))">
                                                                        <asp:ListItem Text="--Select--" Value="Select"></asp:ListItem>
                                                                        <asp:ListItem Text="Proprietorship" Value="Proprietorship"></asp:ListItem>
                                                                        <asp:ListItem Text="Partnership" Value="Partnership"></asp:ListItem>
                                                                        <asp:ListItem Text="OPC" Value="OPC"></asp:ListItem>
                                                                        <asp:ListItem Text="LLP" Value="LLP"></asp:ListItem>
                                                                        <asp:ListItem Text="Private Limited" Value="Private Limited"></asp:ListItem>
                                                                        <asp:ListItem Text="Limited" Value="Limited"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <label id="lblFirmTypeError" runat="server" class="Error-Message">Firm Type is required.</label>
                                                                </div>

                                                            </div>

                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2" style="display: grid;">
                                                                    <label class="mb-1">Mobile No.<span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtMobileNumber" placeholder="Enter The Mobile No." MaxLength="10" AutoComplete="off"
                                                                        pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" AutoPostBack="true" OnTextChanged="txtCustomerMobile_TextChanged">
                                                                    </asp:TextBox>
                                                                    <asp:HiddenField ID="hdnMobileCountryCode" runat="server" />
                                                                    <asp:HiddenField ID="hdnMobile" runat="server" />
                                                                    <asp:CheckBox ID="chkMobileNumberWhatsApp" runat="server" CssClass="same-small-txt mt-1" Text="This Number is on WhatsApp" />
                                                                    <p id="lblMobileNo" runat="server" class="Error-Message">Mobile No is required.</p>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2" style="display: grid;">
                                                                    <label class="mb-1">Alternate Mobile No</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtAlternateMobile" placeholder="Enter Mobile No." MaxLength="10" AutoComplete="off"
                                                                        pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" AutoPostBack="true" OnTextChanged="txtAltCustomerMobile_TextChanged"></asp:TextBox>
                                                                    <asp:HiddenField ID="hdnAltCountryCode" runat="server" />
                                                                    <asp:HiddenField ID="hdnAltMobileNo" runat="server" />
                                                                    <p id="lblAltMobileNo" runat="server" class="Error-Message">Mobile No is required.</p>
                                                                    <asp:CheckBox ID="chkAlternativeMobileNumber" runat="server" CssClass="same-small-txt mt-1" Text="This Number is on WhatsApp" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2">
                                                                    <label class="mb-1">Email ID  <span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control text-lowercase" ID="txtEmail" TextMode="Email" placeholder="" MaxLength="50" AutoComplete="off"
                                                                        AutoPostBack="true" OnTextChanged="txtCustomerEmail_TextChanged"></asp:TextBox>
                                                                    <p id="lblEmailAddress" runat="server" class="Error-Message">Email ID is required.</p>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2">
                                                                    <label class="mb-1">Alternate Email ID</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control text-lowercase" ID="txtAlternateEmail" TextMode="Email" placeholder="" MaxLength="50" AutoComplete="off"
                                                                        AutoPostBack="true" OnTextChanged="txtAltEmail_TextChanged"></asp:TextBox>
                                                                    <p id="lblAtlEmail" runat="server" class="Error-Message">Email ID is required.</p>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2 position-relative">
                                                                    <label class="mb-1">Date of Birth  </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="TextBox1" placeholder=""
                                                                        AutoPostBack="true" AutoCompleteType="Disabled" AutoComplete="off">
                                                                    </asp:TextBox>
                                                                    <div class="input-group-append">
                                                                        <span class="input-group-text" style="cursor: pointer;"
                                                                            onclick="document.getElementById('<%= TextBox1.ClientID %>').focus();">
                                                                            <i class="fa fa-calendar"></i>
                                                                        </span>
                                                                    </div>
                                                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                                                        TargetControlID="TextBox1" EndDate="<%# DateTime.Today %>"></cc1:CalendarExtender>
                                                                    <p id="lblDateOfBirth" runat="server" class="Error-Message">Date of Birth is required.</p>
                                                                </div>

                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2">
                                                                    <label class="mb-1">Gender</label>
                                                                    <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                                        <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                                                        <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>

                                                            <%-- <h5>Business Information</h5>--%>

                                                            <div id="divcheck" runat="server" visible="false">
                                                                <div class="col-md-4">
                                                                    <div class="form-group mb-2">
                                                                        <label class="mb-1">Business Mobile No. <span style="color: red">*</span> </label>
                                                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtOfficialMobileNo" AutoComplete="off" placeholder="Enter The Mobile No." MaxLength="10"
                                                                            pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" AutoPostBack="true" OnTextChanged="txtBussinessMobile_TextChanged">
                                                                        </asp:TextBox>
                                                                        <asp:HiddenField ID="hdnCountryCode" runat="server" />
                                                                        <asp:HiddenField ID="hdnPhoneNumber" runat="server" />

                                                                        <asp:LinkButton ID="lnkResendOTP" runat="server" Text="Resend OTP" Visible="false" OnClick="btnSendOTP_Click" Style="text-decoration: none;"></asp:LinkButton>
                                                                        <p id="Label1" runat="server" class="Error-Message">Mobile No is required.</p>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <div class="form-group mb-2">
                                                                        <asp:LinkButton ID="lnkChangeBussinessMobileNo" runat="server" Text="" Visible="false" OnClick="lnkChangeBussinessMobileNo_Click"
                                                                            Style="text-decoration: none;"><i class="fas fa-edit" style="margin-top: 34px;"></i></asp:LinkButton>

                                                                        <asp:Button runat="server" CssClass="btn next-step" ID="btnSendOTP" Text="Send OTP" OnClick="btnSendOTP_Click"></asp:Button>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group mb-2">
                                                                        <label class="mb-1">Business Email ID  <span style="color: red">*</span> </label>
                                                                        <asp:TextBox runat="server" CssClass="form-control text-lowercase" ID="txtOfficialEmail" AutoComplete="off" TextMode="Email" placeholder="" MaxLength="50"
                                                                            AutoPostBack="true" OnTextChanged="txtBussinessEmail_TextChanged"></asp:TextBox>
                                                                        <p id="Label2" runat="server" class="Error-Message">Email ID is required.</p>
                                                                    </div>
                                                                </div>
                                                                <div id="otpdiv" runat="server" class="row" visible="false">
                                                                    <div class="col-md-4">
                                                                        <div class="form-group mb-2">
                                                                            <label class="mb-1">Enter OTP</label>
                                                                            <asp:TextBox ID="txtVerifyOTP" runat="server" CssClass="form-control" MaxLength="6" AutoComplete="off"
                                                                                pattern="\d{6}" title="Please enter a valid 6-digit OTP" oninput="validateMobileNumber(this)"></asp:TextBox>
                                                                            <asp:Label ID="lblMessage" runat="server" class="mb-2 text-success" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-2">
                                                                        <asp:Button ID="btnVerifyOTP" runat="server" CssClass="btn next-step" Text="Verifiy OTP" OnClick="btnVerifyOTP_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="row mb-12">
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <h6>Registered Office Address</h6>
                                                            <div class="col-12 col-lg-3">
                                                                <div class="form-group mb-2">
                                                                    <label class="mb-1">PIN Code  <span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtPinCode" AutoPostBack="true" OnTextChanged="txtPinCode_TextChanged" placeholder=""
                                                                        MaxLength="6" title="Enter a 6-digit Pincode" oninput="validatePincode(this)" AutoComplete="off">
                                                                    </asp:TextBox>
                                                                    <p id="lblPincode" runat="server" class="Error-Message">PIN Code is required.</p>
                                                                </div>
                                                            </div>
                                                            <div class="col-6 col-lg-4">
                                                                <div class="form-group">
                                                                    <label class="mb-1 d-block">City</label>
                                                                    <asp:Label runat="server" Enabled="false" ID="txtCity" placeholder="" MaxLength="50"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="col-6 col-lg-5">
                                                                <div class="form-group mb-0">
                                                                    <label class="mb-1 d-block">State</label>
                                                                    <asp:Label runat="server" ID="txtState" Enabled="false" placeholder="" MaxLength="50"></asp:Label>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12">
                                                                <div class="form-group mb-2">
                                                                    <label class="mb-1">Address Line 1 <span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtAddress" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="150" AutoComplete="off"
                                                                        onchange="validateAddress()"></asp:TextBox>
                                                                    <p id="lblCurrentAddress" runat="server" class="Error-Message"></p>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2">
                                                                    <label class="mb-1">Address Line 2 </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtAddress1" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="150" AutoComplete="off"
                                                                        onchange="validateAddress2()"></asp:TextBox>
                                                                    <p id="lblCurrentAddress1" runat="server" class="Error-Message"></p>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6 mb-3">
                                                                <div class="form-group mb-2">
                                                                    <label class="mb-1">Landmark <span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtLandmark" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="100" AutoComplete="off"
                                                                        onchange="validateLandmark()"></asp:TextBox>
                                                                    <p id="lblCurrentlandmark" runat="server" class="Error-Message">Landmark is required</p>
                                                                </div>
                                                            </div>

                                                            <h6>Corporate / Main Office Address</h6>
                                                            <div class="col-md-12 mb-2">
                                                                <asp:CheckBox ID="chkSameAddress" CssClass="chksameAddress" runat="server" Text="Same as Registered Office Address" AutoPostBack="true" OnCheckedChanged="SameAsPersonalAddress" />
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="form-group">
                                                                    <label class="mb-1">PIN Code <span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtSellerPincode" AutoPostBack="true" OnTextChanged="txtSellerPinCode_TextChanged" placeholder=""
                                                                        MaxLength="6" title="Enter a 6-digit Pincode" oninput="validatePincode(this)" AutoComplete="off">
                                                                    </asp:TextBox>
                                                                    <p id="lblSellerPINCode" runat="server" class="Error-Message">PIN Code is required.</p>
                                                                </div>
                                                            </div>
                                                            <div class="col-6 col-md-4">
                                                                <div class="form-group">
                                                                    <label class="mb-2 mt-2 mt-lg-0">City</label>
                                                                    <asp:Label ID="lblSellerCity" CssClass="d-block" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="col-6 col-md-5 pr-0">
                                                                <div class="form-group mb-0">
                                                                    <label class="mb-2 mt-0 mt-lg-0">State</label>
                                                                    <asp:Label ID="lblSellerState" CssClass="d-block" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="form-group mb-2 mb-lg-2 mt-lg-2">
                                                                    <label class="mb-1">Address Line 1 <span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtSellerAddressLine1" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="150" AutoComplete="off"
                                                                        onchange="validateSellerAddress()"></asp:TextBox>
                                                                    <p id="lblSellerAddress" runat="server" class="Error-Message">Address Line 1 is required</p>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2 mb-lg-2 mt-lg-2">
                                                                    <label class="mb-1">Address Line 2</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtSellerAddressLine2" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="150" AutoComplete="off"
                                                                        onchange="validateSellerAddress2()"></asp:TextBox>
                                                                    <p id="lblSellerAddress2" runat="server" class="Error-Message"></p>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2 mb-lg-2 mt-lg-2">
                                                                    <label class="mb-1">Landmark <span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtSellerLandmark" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="100" AutoComplete="off"
                                                                        onchange="validateSellerLandmark()"></asp:TextBox>
                                                                    <p id="lblSellerLandMark" runat="server" class="Error-Message">Landmark is required</p>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <ul class="list-inline pull-right mt-3">
                                                            <li>
                                                                <asp:LinkButton ID="btnSeller" runat="server" CssClass="default-btn next-step mt-3" OnClick="btnSeller_Click" UseSubmitBehavior="false"> 
                                                                    Save and Next  <i class='fa fa-angle-double-right font14'></i>
                                                                </asp:LinkButton>
                                                            </li>
                                                        </ul>
                                                    </div>

                                                    <%--<div class="tab-pane" role="tabpanel" id="step1">
                                                        <div class="row">
                                                            <h6>Registered Office Address</h6>
                                                            <div class="col-12 col-lg-3">
                                                                <div class="form-group mb-2">
                                                                    <label class="mb-1">PIN Code  <span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtPinCode" AutoPostBack="true" OnTextChanged="txtPinCode_TextChanged" placeholder=""
                                                                        MaxLength="6" title="Enter a 6-digit Pincode"  oninput="validatePincode(this)" AutoComplete="off">
                                                                    </asp:TextBox>
                                                                    <p id="lblPincode" runat="server" class="Error-Message">PIN Code is required.</p>
                                                                </div>
                                                            </div>
                                                            <div class="col-6 col-lg-4">
                                                                <div class="form-group">
                                                                    <label class="mb-1 d-block">City</label>
                                                                    <asp:Label runat="server" Enabled="false" ID="txtCity" placeholder="" MaxLength="50"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="col-6 col-lg-5">
                                                                <div class="form-group mb-0">
                                                                    <label class="mb-1 d-block">State</label>
                                                                    <asp:Label runat="server" ID="txtState" Enabled="false" placeholder="" MaxLength="50"></asp:Label>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-12">
                                                                <div class="form-group mb-2">
                                                                    <label class="mb-1">Address Line 1 <span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtAddress" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="150" AutoComplete="off"
                                                                      onchange="validateAddress()"></asp:TextBox>
                                                                    <p id="lblCurrentAddress" runat="server" class="Error-Message"></p>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2">
                                                                    <label class="mb-1">Address Line 2 </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtAddress1" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="150" AutoComplete="off"
                                                                        onchange="validateAddress2()"></asp:TextBox>
                                                                    <p id="lblCurrentAddress1" runat="server" class="Error-Message"></p>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6 mb-3">
                                                                <div class="form-group mb-2">
                                                                    <label class="mb-1">Landmark <span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtLandmark" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="100" AutoComplete="off"
                                                                        onchange="validateLandmark()"></asp:TextBox>
                                                                    <p id="lblCurrentlandmark" runat="server" class="Error-Message">Landmark is required</p>
                                                                </div>
                                                            </div>

                                                            <hr />
                                                            <h6>Corporate / Main Office Address</h6>
                                                            <div class="col-md-12 mb-2">
                                                                <asp:CheckBox ID="chkSameAddress" CssClass="chksameAddress" runat="server" Text="Same as Personal Address" AutoPostBack="true" OnCheckedChanged="SameAsPersonalAddress" />
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="form-group">
                                                                    <label class="mb-1">PIN Code <span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtSellerPincode" AutoPostBack="true" OnTextChanged="txtSellerPinCode_TextChanged" placeholder=""
                                                                        MaxLength="6" title="Enter a 6-digit Pincode"   oninput="validatePincode(this)" AutoComplete="off">
                                                                    </asp:TextBox>
                                                                    <p id="lblSellerPINCode" runat="server" class="Error-Message">PIN Code is required.</p>
                                                                </div>
                                                            </div>
                                                            <div class="col-6 col-md-4">
                                                                <div class="form-group">
                                                                    <label class="mb-2 mt-2 mt-lg-0">City</label>
                                                                    <asp:Label ID="lblSellerCity" CssClass="d-block" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="col-6 col-md-5 pr-0">
                                                                <div class="form-group mb-0">
                                                                    <label class="mb-2 mt-0 mt-lg-0">State</label>
                                                                    <asp:Label ID="lblSellerState" CssClass="d-block" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="form-group mb-2 mb-lg-2 mt-lg-2">
                                                                    <label class="mb-1">Address Line 1 <span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtSellerAddressLine1" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="150" AutoComplete="off"
                                                                        onchange="validateSellerAddress()"></asp:TextBox>
                                                                    <p id="lblSellerAddress" runat="server" class="Error-Message">Address Line 1 is required</p>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2 mb-lg-2 mt-lg-2">
                                                                    <label class="mb-1">Address Line 2</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtSellerAddressLine2" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="150" AutoComplete="off"
                                                                        onchange="validateSellerAddress2()"></asp:TextBox>
                                                                    <p id="lblSellerAddress2" runat="server" class="Error-Message"></p>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-2 mb-lg-2 mt-lg-2">
                                                                    <label class="mb-1">Landmark <span style="color: red">*</span> </label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtSellerLandmark" TextMode="MultiLine" Rows="2" placeholder="" MaxLength="100" AutoComplete="off"
                                                                        onchange="validateSellerLandmark()"></asp:TextBox>
                                                                    <p id="lblSellerLandMark" runat="server" class="Error-Message">Landmark is required</p>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <ul class="list-inline pull-right mt-3">
                                                            <li>
                                                                <asp:LinkButton ID="btnBackSellerInfo" runat="server" CssClass="default-btn prev-step bg-dark" OnClick="btnBackSellerInformationHidden"
                                                                    UseSubmitBehavior="false">
                                                                    <i class="fa fa-angle-double-left font14"></i>&nbsp; Back
                                                                </asp:LinkButton>

                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="btnNext" runat="server" CssClass="default-btn next-step" OnClick="btnNext_Click" UseSubmitBehavior="false"> 
                                                                    Save and Next <i class='fa fa-angle-double-right font14'></i>
                                                                </asp:LinkButton>


                                                                <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="default-btn next-step" OnClick="btnNext_Click" />

                                                            </li>
                                                        </ul>
                                                    </div>--%>

                                                    <div class="tab-pane" role="tabpanel" id="step3">
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Account Number <span style="color: red">*</span></label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtAccountNumber" placeholder="" MaxLength="20"
                                                                        oninput="validateAccountNumber(this); validateConfirmAccountNumber();" AutoComplete="off"
                                                                        onpaste="return false;" oncopy="return false;" oncut="return false;" TextMode="Password">
                                                                    </asp:TextBox>
                                                                    <p id="lblAccountNumber" runat="server" clientidmode="Static" class="Error-Message">Account Number is required.</p>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Confirm Account Number <span style="color: red">*</span></label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtConfirmAccountNumber" placeholder="" MaxLength="20" AutoComplete="off"
                                                                        oninput="validateConfirmAccountNumber();" onpaste="return false;" oncopy="return false;" oncut="return false;">
                                                                    </asp:TextBox>
                                                                    <p id="lblConfirmAccountNumber" runat="server" clientidmode="Static" class="Error-Message">Confirm Account Number is required.</p>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">IFSC Code <span style="color: red">*</span></label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtIFSCCode" AutoPostBack="true" placeholder="" MaxLength="11" AutoComplete="off"
                                                                        OnTextChanged="txtIFSC_TextChanged" oninput="validateIFSCCode(this);" onblur="validateIFSCCode(this);">
                                                                    </asp:TextBox>
                                                                    <p id="lblIFSCCode" runat="server" clientidmode="Static" class="Error-Message">IFSC Code is required.</p>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Account Holder Name <span style="color: red">*</span></label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtAccountHolderName" placeholder="" AutoComplete="off" MaxLength="30"
                                                                        oninput="hideSellerName(this, document.getElementById('lblAccountHoldername'))"></asp:TextBox>
                                                                    <p id="lblAccountHoldername" runat="server" class="Error-Message">Account Holder Name is required.</p>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Bank Name</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtBankName" placeholder="" AutoComplete="off"></asp:TextBox>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Branch Name</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtBranchName" placeholder="" AutoComplete="off"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Branch Address</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtBranchAddress" TextMode="MultiLine" Rows="3" placeholder="" AutoComplete="off"></asp:TextBox>
                                                                </div>
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
                                                                            <span class="tooltip-text">Any document containing complete Bank Account details such as Beneficiary Name, Account Number, IFSC Code, Bank Branch, etc.
                                                                            </span>
                                                                        </span>
                                                                        <span style="color: red">*</span>
                                                                    </label>
                                                                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddlSuppotingDoc" AutoPostBack="false">
                                                                    </asp:DropDownList>
                                                                    <asp:Label ID="lblsupportingDocError" CssClass="Error-Message" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">File<span style="color: red">*</span></label>
                                                                    <asp:FileUpload runat="server" ID="fuSuppotingDoc" CssClass="form-control" accept=".jpg,.jpeg,.png,.pdf" />
                                                                    <asp:Label ID="lblSupportingDocumentError" CssClass="Error-Message" runat="server"></asp:Label>
                                                                </div>
                                                                <p class="text-danger mt-2 mb-2" style="font-size: 13px;"><strong>Note<sup>*</sup></strong> jpg, jpeg, png and pdf format is acceptable.</p>
                                                            </div>
                                                        </div>
                                                        <div class="d-flex justify-content-between align-items-center my-3">
                                                            <div>
                                                                <asp:LinkButton ID="btnBackDealer" runat="server" CssClass="default-btn prev-step bg-dark" OnClick="btnBackDealerInfo" UseSubmitBehavior="false">
                                                                    <i class="fa fa-angle-double-left font14"></i>&nbsp; Back
                                                                </asp:LinkButton>
                                                            </div>
                                                            <div class="d-flex gap-2">
                                                                <asp:LinkButton ID="btnSkip" runat="server" CssClass="skip-btn bg-info" OnClick="btnSkip_Click" UseSubmitBehavior="false"> 
                                                                    Skip Now <i class='fa fa-angle-double-right font14'></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnnext2" runat="server" CssClass="default-btn next-step" OnClick="btnnext2_Click" UseSubmitBehavior="false"> 
                                                                   Save and Next  <i class='fa fa-angle-double-right font14'></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="tab-pane" role="tabpanel" id="step4">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <asp:Label ID="lblRequiredDocuments" runat="server" CssClass="text-danger" Style="font-size: 14px;" Text="Note: It is mandatory to submit Aadhaar, PAN card, and GST certificate details."></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-6 mb-3">
                                                                <div class="form-group">
                                                                    <label class="mb-1">Document Name *</label>
                                                                    <%--<asp:DropDownList runat="server" CssClass="form-control" ID="ddlDocumentName">
                                                                    </asp:DropDownList>--%>
                                                                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddlDocumentName" AutoPostBack="false" onchange="handleDocumentFormat(this.value)">
                                                                    </asp:DropDownList>
                                                                    <asp:Label ID="lblRequiredDocError" CssClass="Error-Message" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="row align-items-center">
                                                                    <div class="col-12 col-lg-7 col-xl-8 mb-3 mb-lg-0">
                                                                        <div class="">
                                                                            <label class="mb-1">File</label>
                                                                            <asp:FileUpload runat="server" ID="fuFrontSide" CssClass="form-control" accept=".jpg,.jpeg,.png,.pdf" />
                                                                            <asp:Label ID="lblDocument" runat="server" Text="Document is required." ForeColor="Red" Visible="false"></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-12 col-lg-5 mt-lg-4 mt-0 col-xl-4 px-lg-0">
                                                                        <asp:LinkButton ID="btnUploadFront" runat="server" CssClass="default-btn next-step" OnClick="btnUploadFront_Click" UseSubmitBehavior="false" Style="font-size: 14px; padding: 8px;"> 
                                                                            Add to Grid
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                                <p class="text-danger mt-2 mb-2" style="font-size: 13px;"><strong>Note<sup>*</sup></strong> jpg, jpeg, png and pdf format is acceptable.</p>
                                                            </div>
                                                            <div class="col-md-6 mb-3" id="lblDocumentNumber" runat="server">
                                                                <div class="form-group">
                                                                    <label class="mb-1">Document Number</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control text-uppercase" ID="txtDocumentNumber" MaxLength="15" placeholder="" AutoComplete="off" AutoPostBack="false" oninput="validateDocumentNumber()"></asp:TextBox>
                                                                    <%--<asp:HiddenField ID="hdnDocumentNumber" runat="server" />--%>
                                                                    <asp:CheckBox ID="chckGSTIN" CssClass="chckGSTINcls" Text="Same AS Personal GSTIN" runat="server" OnCheckedChanged="chkGSTIN_Change" AutoPostBack="true" Style="display: none;" />
                                                                    <p class="text-danger mt-2 mb-0" id="lblAdhaarDocumentNumber" style="font-size: 13px; display: none;"><strong>Note<sup>*</sup></strong> Please enter the last 4 digits of your Aadhar.</p>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group" style="margin-top: 32px;">
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <asp:Label ID="lblDocFormatError" runat="server" CssClass="alert alert-danger" Style="display: none;"></asp:Label>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="DocumentData table-responsive">
                                                                    <asp:GridView runat="server" ID="gvDocuments" CssClass="table table-striped text-nowrap table-bordered mb-0"
                                                                        AutoGenerateColumns="false" DataKeyNames="DocId" GridLines="None" OnRowCommand="gvDocuments_RowCommand" OnRowDataBound="gvDocuments_RowDataBound">
                                                                        <Columns>
                                                                            <asp:BoundField HeaderText="Sr.No." DataField="SrNo" />
                                                                            <asp:BoundField HeaderText="DocId" DataField="DocId" Visible="false" />
                                                                            <asp:BoundField HeaderText="Document Name" DataField="DocumentName" />
                                                                            <%--<asp:BoundField HeaderText="Document Number" DataField="DocumentNumber" />--%>
                                                                            <asp:TemplateField HeaderText="Document Number">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblMaskedDoc" runat="server" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField HeaderText="Document Path" DataField="DocumentPath" Visible="false" />
                                                                            <asp:BoundField HeaderText="Status" DataField="Status" />
                                                                            <asp:BoundField HeaderText="Size" DataField="Size" />
                                                                            <asp:TemplateField HeaderText="Actions">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkView" runat="server" CommandName="View" CommandArgument='<%# Container.DataItemIndex %>' CssClass="text-primary me-1" ToolTip="View" Style="text-decoration: none;">
                                                                                        <i class="fa fa-eye"></i>
                                                                                    </asp:LinkButton>
                                                                                    <asp:LinkButton ID="lnkDelete" Visible="false" runat="server" CommandName="DeleteDoc" CommandArgument='<%# Container.DataItemIndex %>' CssClass="text-danger" ToolTip="Delete">
                                                                                        <i class="fa fa-trash"></i>
                                                                                    </asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="my-3">
                                                        <ul class="list-inline pull-right mt-3">
                                                            <li>
                                                                <asp:LinkButton ID="btnBankDetails" runat="server" CssClass="default-btn prev-step bg-dark" OnClick="btnBackBankDetails" UseSubmitBehavior="false">
                                                                   <i class="fa fa-angle-double-left font14"></i>&nbsp; Back
                                                                </asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="btnnext3" runat="server" CssClass="default-btn next-step" OnClick="btnnext3_Click" Visible="false" UseSubmitBehavior="false"> 
                                                                   Save and Next  <i class='fa fa-angle-double-right font14'></i>
                                                                </asp:LinkButton>
                                                                <%--<asp:Button ID="btnnext3" runat="server" Text="Next" CssClass="default-btn next-step" OnClick="btnnext3_Click" />--%>

                                                            </li>
                                                        </ul>
                                                            </div>
                                                    </div>
                                                    <div class="tab-pane" role="tabpanel" id="step5">
                                                        <div class="all-info-container">
                                                            <div class="CheckIcon">
                                                                <i class="fa fa-check-circle">&nbsp;</i>
                                                            </div>
                                                            <div class="confirmation-content">
                                                                <h3>Acknowledgement</h3>
                                                                <p>Thank you for your interest and for sharing the details. Your account will be activated after verification. You may check your account status by logging in again.</p>
                                                                <div style="gap: 15px; display: flex; justify-content: center;">
                                                                    <asp:LinkButton ID="btnLogin" runat="server" class="btn next-step" AutoPostBack="true" OnClick="btnLogout_Click">Login</asp:LinkButton>
                                                                    <asp:LinkButton ID="btnDashboard" runat="server" class="btn next-step" AutoPostBack="true" OnClick="btnDashboard_Click">Let's Start My Journey</asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix"></div>
                                                </div>
                                            </form>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </div>
                </div>
            </div>
        </form>
    </main>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js"></script>
    <script>
        // ------------step-wizard-------------
        $(document).ready(function () {
            $('.nav-tabs > li a[title]').tooltip();

            //Wizard
            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {

                var target = $(e.target);

                if (target.parent().hasClass('disabled')) {
                    return false;
                }
            });

            $(".next-step").click(function (e) {

                var active = $('.wizard .nav-tabs li.active');
                active.next().removeClass('disabled');
                nextTab(active);

            });
            $(".prev-step").click(function (e) {

                var active = $('.wizard .nav-tabs li.active');
                prevTab(active);

            });
        });

        function nextTab(elem) {
            $(elem).next().find('a[data-toggle="tab"]').click();
        }
        function prevTab(elem) {
            $(elem).prev().find('a[data-toggle="tab"]').click();
        }


        $('.nav-tabs').on('click', 'li', function () {
            $('.nav-tabs li.active').removeClass('active');
            $(this).addClass('active');
        });


        $(document).ready(function () {
            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                var activeTab = $(e.target).attr('href');
                $('#<%= hdnActiveTab.ClientID %>').val(activeTab);
            });


            var lastTab = $('#<%= hdnActiveTab.ClientID %>').val();
            if (lastTab) {
                $('a[href="' + lastTab + '"]').tab('show');
                var currentLi = $('a[href="' + lastTab + '"]').parent();
                currentLi.removeClass('disabled').addClass('active');
                currentLi.prevAll().removeClass('disabled').addClass('done active');
            }
        });


        function validatePincode(input) {
            input.value = input.value.replace(/\D/g, '');
            if (input.value.length > 6) {
                input.value = input.value.slice(0, 6);
            }
        }

        function validateMobileNumber(input) {
            input.value = input.value.replace(/[^\d]/g, '').slice(0, 10);
        }

        document.addEventListener('DOMContentLoaded', function () {
            const accountInput = document.getElementById('<%= txtAccountNumber.ClientID %>');
            accountInput.addEventListener('input', function () {
                this.value = this.value.replace(/\D/g, '');
            });
        });
        document.addEventListener('DOMContentLoaded', function () {
            const accountInput = document.getElementById('<%= txtConfirmAccountNumber.ClientID %>');
            accountInput.addEventListener('input', function () {
                this.value = this.value.replace(/\D/g, '');
            });
        });



        function validateAccountNumber(input) {
            // debugger;
            const accNum = input.value.trim();
            const errorLabel = document.getElementById('lblAccountNumber');
            if (accNum.length >= 9 && accNum.length <= 20) {
                errorLabel.style.display = 'none';
            } else {
                errorLabel.innerText = "Account Number must be 9–20 digits.";
                errorLabel.style.display = 'block';
            }
        }

        function validateConfirmAccountNumber() {
            // debugger;
            const accNum = document.getElementById('<%= txtAccountNumber.ClientID %>').value.trim();
            const confirmAccNum = document.getElementById('<%= txtConfirmAccountNumber.ClientID %>').value.trim();
            const errorLabel = document.getElementById('lblConfirmAccountNumber');
            if (accNum === confirmAccNum) {
                errorLabel.style.display = 'none';
            }
            else if (confirmAccNum == '') {
                errorLabel.style.display = 'none';
            }
            else {
                errorLabel.innerText = "Account numbers do not match.";
                errorLabel.style.display = 'block';
            }
        }

        function validateIFSCCode(input) {
            //  debugger;
            const ifsc = input.value.trim().toUpperCase();
            const ifscRegex = /^[A-Z]{4}0[A-Z0-9]{6}$/;
            const errorLabel = document.getElementById('lblIFSCCode');

            input.value = ifsc;

            if (ifscRegex.test(ifsc)) {
                errorLabel.style.display = 'none';
            } else {
                errorLabel.innerText = "Incorrect IFSC Code format.";
                errorLabel.style.display = 'block';
            }
        }

        $(document).ready(function () {
            $('.nav-tabs li a').click(function (e) {
                e.preventDefault(); // Prevent navigation
                return false;
            });
        });

    </script>


    <%--    <script>
        document.addEventListener("DOMContentLoaded", function () {
            var txtDoc = document.getElementById('<%= txtDocumentNumber.ClientID %>');
            var hdnDoc = document.getElementById('<%= hdnDocumentNumber.ClientID %>');

            // Store full value on input
            txtDoc.addEventListener('input', function () {
                hdnDoc.value = txtDoc.value;
            });

            // Mask on blur
            txtDoc.addEventListener('blur', function () {
                var val = txtDoc.value;
                if (val.length > 4) {
                    hdnDoc.value = val; // Store full value
                    var last4 = val.slice(-4);
                    var masked = '*'.repeat(val.length - 4) + last4;
                    txtDoc.value = masked;
                }
            });

            // Restore on focus
            txtDoc.addEventListener('focus', function () {
                if (hdnDoc.value) {
                    txtDoc.value = hdnDoc.value;
                }
            });

            // Ensure full value is submitted
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
    </script>--%>

    <script type="text/javascript">
        let selectedDocType = "";

        function handleDocumentFormat(value) {
            selectedDocType = value;
            debugger;
            <%--var hdnDoc = document.getElementById('<%= hdnDocumentNumber.ClientID %>');--%>
            var docinput = document.getElementById('<%= txtDocumentNumber.ClientID %>');
            var doclbl = document.getElementById('<%= lblDocumentNumber.ClientID %>');
            <%--var gstCheckbox = document.getElementById('<%= chckGSTIN.ClientID %>');--%>
            var gstCheckbox = document.getElementsByClassName("chckGSTINcls");
            const errorAdharLabel = document.getElementById("lblAdhaarDocumentNumber");
            errorAdharLabel.style.display = "none";
            gstCheckbox[0].style.display = "none";
            docinput.readOnly = false; 
            docinput.innerText = "";
            docinput.value = "";
            if (selectedDocType === "13" || selectedDocType === "57" || selectedDocType === "58") {
                errorAdharLabel.style.display = "block";
            }
            if (selectedDocType === "278") {
                docinput.style.display = "none";
                doclbl.style.display = "none";
            }
            else if (selectedDocType === "146") {
                //gstCheckbox[0].style.display = "block";
                docinput.innerText = '<%= Session["SellerGSTIN"] != null ? Session["SellerGSTIN"].ToString() : "" %>';
                docinput.value = '<%= Session["SellerGSTIN"] != null ? Session["SellerGSTIN"].ToString() : "" %>';
                docinput.readOnly = true; 
                
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
                    errorLabel.style.display = "block";
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


    <script src="https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/js/intlTelInput.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/css/intlTelInput.css">

    <script type="text/javascript">
        window.onload = function () {
            // Get elements by their ASP.NET ClientID
            const mobileCodeInput = document.getElementById('<%= txtOfficialMobileNo.ClientID %>');
            const altMobileCodeInput = document.getElementById('<%= txtAlternateMobile.ClientID %>');
            const MobileNumber = document.getElementById('<%= txtMobileNumber.ClientID %>');

            const countryCodeHidden = document.getElementById('<%= hdnCountryCode.ClientID %>');
            const phoneNumberHidden = document.getElementById('<%= hdnPhoneNumber.ClientID %>');

            const altCountryCodeHidden = document.getElementById('<%= hdnAltCountryCode.ClientID %>');
            const altPhoneNumberHidden = document.getElementById('<%= hdnAltMobileNo.ClientID %>');

            const hdnMobileCountryCode = document.getElementById('<%= hdnMobileCountryCode.ClientID %>');
            const hdnMobile = document.getElementById('<%= hdnMobile.ClientID %>');


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

            if (MobileNumber && hdnMobileCountryCode && hdnMobile) {
                const itiMobileNumber = window.intlTelInput(MobileNumber, {
                    initialCountry: "in",
                    separateDialCode: true,
                    formatOnDisplay: false,
                    utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/js/utils.js",
                });

                function updateHiddenFields() {
                    const dialCode = itiMobileNumber.getSelectedCountryData().dialCode;
                    let nationalNumber = itiMobileNumber.getNumber(intlTelInputUtils.numberFormat.NATIONAL).replace(/\D/g, '');
                    if (nationalNumber.startsWith('0')) {
                        nationalNumber = nationalNumber.substring(1);
                    }
                    hdnMobileCountryCode.value = dialCode;
                    hdnMobile.value = nationalNumber;
                }

                MobileNumber.addEventListener('countrychange', updateHiddenFields);
                MobileNumber.addEventListener('blur', updateHiddenFields);
            }
        };
    </script>

    <script>
        function validateAddress() {
            //   debugger;
            var address = document.getElementById('<%= txtAddress.ClientID %>').value.trim();
            var label = document.getElementById('<%= lblCurrentAddress.ClientID %>');
            var regex = /^(\S+\s+){2,}\S+$/;
            if (!regex.test(address)) {
                label.innerText = 'Please enter at least 3 words in the address.';
                label.style.display = 'block';
            } else {
                label.innerText = "";
                label.style.display = 'none';
            }
        }
        function validateAddress2() {
            //    debugger;
            var address1 = document.getElementById('<%= txtAddress1.ClientID %>').value.trim();
            var label1 = document.getElementById('<%= lblCurrentAddress1.ClientID %>');
            var regex = /^(\S+\s+){2,}\S+$/;
            if (!regex.test(address1)) {
                label1.innerText = 'Please enter at least 3 words in the address.';
                label1.style.display = 'block';
            } else {
                label1.innerText = '';
                label1.style.display = 'none';
            }
        }
        function validateSellerAddress() {
            var Selleraddress = document.getElementById('<%= txtSellerAddressLine1.ClientID %>').value.trim();
            var Sellerlabel = document.getElementById('<%= lblSellerAddress.ClientID %>');
            var regex = /^(\S+\s+){2,}\S+$/;
            if (!regex.test(Selleraddress)) {
                Sellerlabel.innerText = 'Please enter at least 3 words in the address.';
                Sellerlabel.style.display = 'block';
            }
            else {
                Sellerlabel.innerText = "";
                Sellerlabel.style.display = 'none';
            }
        }

        function validateSellerAddress2() {
            //   debugger;
            var Selleraddress2 = document.getElementById('<%= txtSellerAddressLine2.ClientID %>').value.trim();
            var Sellerlabel2 = document.getElementById('<%= lblSellerAddress2.ClientID %>');
            var regex = /^(\S+\s+){2,}\S+$/;
            if (!regex.test(Selleraddress2)) {
                Sellerlabel2.innerText = 'Please enter at least 3 words in the address.';
                Sellerlabel2.style.display = 'block';
            }
            else {
                Sellerlabel2.innerText = '';
                Sellerlabel2.style.display = 'none';
            }
        }

        function validateLandmark() {
            var landmark1 = document.getElementById('<%= txtLandmark.ClientID %>').value.trim();
            var label1 = document.getElementById('<%= lblCurrentlandmark.ClientID %>');

            if (landmark1.length < 3) {
                label1.innerText = "Please enter at least 3 characters for the landmark.";
                label1.style.display = 'block';
            } else {
                label1.innerText = "";
                label1.style.display = 'none';
            }
        }

        function validateSellerLandmark() {
            var landmark = document.getElementById('<%= txtSellerLandmark.ClientID %>').value.trim();
            var label = document.getElementById('<%= lblSellerLandMark.ClientID %>');

            if (landmark.length < 3) {
                label.innerText = "Please enter at least 3 characters for the landmark.";
                label.style.display = 'block';
            } else {
                label.innerText = "";
                label.style.display = 'none';
            }
        }
    </script>

    <script>
        function hideSellerName(textbox, label) {
            debugger;

            if (textbox.value.trim().length > 0) {
                label.style.display = "none";
                label.innerText = "";
            } else {
                label.style.display = "block";
            }
        }
    </script>

</body>
</html>
