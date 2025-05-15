<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateAnAccount.aspx.cs" Inherits="Patner_Retailer_ADO.CreateAnAccount" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Signup</title>
    <meta name="Description" content="">
    <meta name="Author" content="">
    <meta name="keywords" content="">  
    <link id="style" href="assets/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <link href="assets/css/signup.css" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">
</head>
<body>
    <main>

        <form id="signupForm" runat="server">
            <div class="warpper-main">
                <div class="wrapper-content d-flex">
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
                        <div class="row" style="width: 200px; display: flex; flex-wrap: nowrap; margin-top: 3%; margin-bottom: 12px;">
                            <img src="assets/images/logo.png" alt="InfyShield" />

                            <div class="d-flex justify-content-end mt-3 logout_btn_top">
                                <asp:Button ID="btnLogout" runat="server" CssClass="skip-btn btn btn-danger" Text="Logout" OnClick="btnLogout_Click" />
                            </div>
                        </div>
                        <section class="signup-step-container">
                            <div class="container">
                                <div class="row d-flex justify-content-center">
                                    <div class="col-md-12">
                                        <div class="wizard">
                                            <div class="wizard-inner">
                                                <asp:HiddenField ID="hdnActiveTab" runat="server" />
                                                <div class="connecting-line"></div>
                                                <ul class="nav nav-tabs" role="tablist">
                                                    <li role="presentation" class="active">
                                                        <a href="#step1" data-toggle="tab" aria-controls="step1" role="tab"
                                                            aria-expanded="true"><span class="round-tab"></span><i>Personal
                                                        Info</i>
                                                        </a>
                                                    </li>
                                                    <li role="presentation" class="disabled">
                                                        <a href="#step2" data-toggle="tab" aria-controls="step2" role="tab"
                                                            aria-expanded="false"><span class="round-tab"></span><i>Bank
                                                        Detail</i>
                                                        </a>
                                                    </li>
                                                    <li role="presentation" class="disabled">
                                                        <a href="#step3" data-toggle="tab" aria-controls="step3"
                                                            role="tab"><span class="round-tab"></span><i>Upload
                                                        Document</i>
                                                        </a>
                                                    </li>
                                                    <li role="presentation" class="disabled">
                                                        <a href="#step4" data-toggle="tab" aria-controls="step4"
                                                            role="tab"><span class="round-tab"></span>
                                                            <i>Confirmation</i></a>
                                                    </li>
                                                </ul>
                                            </div>
                                            <form class="login-box">
                                                <div class="tab-content" id="main_form">
                                                    <div class="tab-pane active" role="tabpanel" id="step1">
                                                        <div class="row">

                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Full Name *</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtFirstName" placeholder="" MaxLength="30"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Mobile No.</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtMobileNumber" placeholder=""
                                                                        pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">
                                                                        Alternate Mobile No / WhatsApp
                                                                    No</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtAlternateMobile" placeholder=""
                                                                        pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Email Address*</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtEmail" TextMode="Email" placeholder="" MaxLength="50"></asp:TextBox>
                                                                     <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" ForeColor="Red" Display="Dynamic" />
                                                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Enter a valid email address"
                                                                        ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" ForeColor="Red" Display="Dynamic" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Alternate Email Address</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtAlternateEmail" TextMode="Email" placeholder="" MaxLength="50"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Date of Birth *</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="TextBox1" placeholder=""  TextMode="Date"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Gender</label>
                                                                     <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                                                         <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                                                         <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                                                         <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                                                         <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                                                                     </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvGender" runat="server" ControlToValidate="ddlGender"
                                                                        InitialValue="" ErrorMessage="Select Gender" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />                                                                    
                                                                </div>
                                                            </div>

                                                            <div class="col-md-6">
                                                                <div class="row mb-3">
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label class="mb-1">PIN Code*</label>
                                                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtPinCode" AutoPostBack="true" OnTextChanged="txtPinCode_TextChanged" placeholder=""
                                                                                maxlength="6" pattern="\d{6}" title="Enter a 6-digit Pincode" oninput="validatePincode(this)"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label class="mb-1">City</label>
                                                                            <asp:TextBox runat="server" CssClass="form-control" Enabled="false" ID="txtCity" placeholder="" MaxLength="50"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">State</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtState" Enabled="false" placeholder="" MaxLength="50"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="form-group">
                                                                    <label class="mb-1">Current Address (Full Postal Address)</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtAddress" TextMode="MultiLine" Rows="3" placeholder="" MaxLength="150"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <ul class="list-inline pull-right">
                                                            <li>
                                                                <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="default-btn next-step" OnClick="btnNext_Click" />

                                                            </li>
                                                        </ul>
                                                    </div>

                                                    <div class="tab-pane" role="tabpanel" id="step2">
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Account Number*</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtAccountNumber" placeholder="" MaxLength="20"></asp:TextBox>
                                                                    <asp:RegularExpressionValidator ID="revAccountNumber" runat="server" ControlToValidate="txtAccountNumber"
                                                                        ErrorMessage="Account number must be 6 to 20 digits" ValidationExpression="^\d{6,20}$" ForeColor="Red" Display="Dynamic" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Confirm Account Number*</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtConfirmAccountNumber" placeholder="" MaxLength="20"></asp:TextBox>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtConfirmAccountNumber"
                                                                        ErrorMessage="Account number must be 6 to 20 digits" ValidationExpression="^\d{6,20}$" ForeColor="Red" Display="Dynamic" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">IFSC Code*</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtIFSCCode" AutoPostBack="true" placeholder="" MaxLength="11" OnTextChanged="txtIFSC_TextChanged"></asp:TextBox>
                                                                    <asp:RegularExpressionValidator ID="revIFSC" runat="server" ControlToValidate="txtIFSCCode" ValidationExpression="^[A-Z]{4}0[A-Z0-9]{6}$"
                                                                        ErrorMessage="Enter a valid IFSC code" CssClass="text-danger" ValidationGroup="vgBank" Display="Dynamic" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Account Holder Name</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtAccountHolderName" placeholder=""></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Bank Name</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtBankName" placeholder=""></asp:TextBox>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Branch Name</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtBranchName" placeholder=""></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="form-group mb-3">
                                                                    <label class="mb-1">Branch Address</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtBranchAddress" TextMode="MultiLine" Rows="3" placeholder=""></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <ul class="list-inline pull-right">
                                                            <li>
                                                                <button type="button"
                                                                    class="default-btn prev-step bg-dark">
                                                                    <i
                                                                        class="fa fa-angle-double-left font14"></i>Back</button>
                                                            </li>
                                                            <li>

                                                                <asp:Button ID="btnnext2" runat="server" Text="Next" CssClass="default-btn next-step" OnClick="btnnext2_Click" />


                                                            </li>
                                                        </ul>
                                                    </div>
                                                    <div class="tab-pane" role="tabpanel" id="step3">
                                                        <div class="row">
                                                            <div class="col-md-6 mb-3">
                                                                <div class="form-group">
                                                                    <label class="mb-1">Document Name *</label>
                                                                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddlDocumentName">                                                                        
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6 mb-3">
                                                                <div class="form-group">
                                                                    <label class="mb-1">Document Number *</label>
                                                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtDocumentNumber" placeholder="" MaxLength="20"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="row">
                                                                    <div class="col-md-8">
                                                                        <div class="form-group">
                                                                            <label class="mb-1">File</label>
                                                                            <asp:FileUpload runat="server" ID="fuFrontSide" CssClass="form-control" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="form-group">
                                                                            <asp:Button runat="server" ID="btnUploadFront" Text="Upload" CssClass="default-btn next-step" OnClick="btnUploadFront_Click" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            
                                                            <div class="col-md-12">
                                                                <div class="DocumentData">
                                                                    <asp:GridView runat="server" ID="gvDocuments" CssClass="table table-striped table-bordered table-responsive" AutoGenerateColumns="false">
                                                                        <Columns>
                                                                            <asp:BoundField HeaderText="Sr.No." DataField="SrNo" />
                                                                            <asp:BoundField HeaderText="DocId" DataField="DocId" Visible="false" />
                                                                            <asp:BoundField HeaderText="Document Name" DataField="DocumentName" />
                                                                            <asp:BoundField HeaderText="Document Number" DataField="DocumentNumber" />
                                                                            <asp:BoundField HeaderText="Document Path" DataField="DocumentPath" Visible="false" />
                                                                            <asp:BoundField HeaderText="Status" DataField="Status" />
                                                                            <asp:BoundField HeaderText="Size" DataField="Size" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <ul class="list-inline pull-right">
                                                            <li>
                                                                <button type="button" class="default-btn prev-step bg-dark">
                                                                    <i class="fa fa-angle-double-left font14"></i>Back
                                                           
                                                                </button>
                                                            </li>
                                                            <li>

                                                                <asp:Button ID="btnnext3" runat="server" Text="Next" CssClass="default-btn next-step" OnClick="btnnext3_Click" />


                                                            </li>
                                                        </ul>
                                                    </div>
                                                    <div class="tab-pane" role="tabpanel" id="step4">
                                                        <div class="all-info-container">
                                                            <div class="CheckIcon">
                                                                <i class="fa fa-check-circle">&nbsp;</i>
                                                            </div>
                                                            <div class="confirmation-content">
                                                                <h3>Thanks for Your Confirmation</h3>
                                                                <p>Your coupon is on its way to your email... </p>
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


    </script>
</body>
</html>
