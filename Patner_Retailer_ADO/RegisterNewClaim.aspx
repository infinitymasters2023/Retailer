<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RegisterNewClaim.aspx.cs" Inherits="Patner_Retailer_ADO.RegisterNewClaim" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />

    <script>

        function validateProblemWords() {
            const addressInput = document.getElementById('<%= txtProblemDesc.ClientID %>');
            const errorDiv = document.getElementById('problemError');
            const wordCount = addressInput.value.trim().split(/\s+/).filter(w => w).length;

            if (wordCount < 3) {
                errorDiv.textContent = "Problem must contain at least 3 words.";
            } else {
                errorDiv.textContent = "";
            }
        }
    </script>

    <style>
        .dashboard-content input[type=checkbox], input[type=radio] {
            box-sizing: border-box;
            padding: 0;
            display: block;
        }

        .defective-part-lists tr {
            display: inline-flex;
            margin-right: 10px;
            align-items: center;
        }

        .defective-part-lists td {
            display: flex;
            align-items: center;
            gap: 10px;
            justify-content: start;
        }

            .defective-part-lists td label {
                margin-bottom: 0px;
                font-size: 13px;
            }

        .customer-details-txt {
            display: flex;
            flex-direction: column;
            flex-wrap: nowrap;
        }

            .customer-details-txt span {
                font-size: 13px;
                color: #000;
            }

            .customer-details-txt label {
                font-size: 13px;
                color: #646464;
            }

        .card-body span {
            font-size: 14px;
        }

        .card-body input, .card-body textarea, .card-body select {
            font-size: 13px;
        }
    </style>

      <script type="text/javascript">

        function ShowPreview(input) {
            debugger;
            if (input.files && input.files[0]) {

                var ImageDir = new FileReader();

                ImageDir.onload = function (e) {
                    $('#impPrev').attr('src', e.target.result);

                }
                ImageDir.readAsDataURL(input.files[0]);

            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<div class="container-fluid  dashboard-content">
        <div class="row">
            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">--%>
    <div class="card mb-3">
        <h5 class="card-header" style="text-align: left;">Customer Details</h5>
        <div class="card-body pt-0 px-3 pb-3">
            <div id="divcert" runat="server" style="margin-bottom: 0px;" class=" form-group row">
                <div class="col-md-3">
                    <asp:Label ID="Label5" runat="server" Text="Ticket No: "></asp:Label>
                    <asp:Label ID="Label2" runat="server" Font-Bold="true"></asp:Label>
                </div>
                <div class=" col-md-3 ">
                    <asp:Label ID="lbl" runat="server"> Certificate No:</asp:Label>
                    <asp:Label ID="lblcertificate" Font-Bold="true" Text="NA" runat="server"></asp:Label>
                </div>
                <div class=" col-md-3 ">
                    <asp:Label ID="Label4" runat="server"> Loan No:</asp:Label>
                    <asp:Label ID="lblloan" Font-Bold="true" Text="NA" runat="server"></asp:Label>
                </div>
                <div class=" col-md-3 ">
                    <asp:Label ID="Label6" runat="server"> Claim No:</asp:Label>
                    <asp:Label ID="lblclamno" Font-Bold="true" Text="NA" runat="server"></asp:Label>
                </div>
            </div>
            <div class="d-flex align-items-center flex-wrap row">
                 <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">Customer Name </label>
                    <asp:Label ID="txtFirstName" runat="server"></asp:Label>
                    <%--<asp:TextBox CssClass="form-control validate-group" ID="txtFirstName" runat="server" placeholder="Customer Name" MaxLength="50" Enabled="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                        ErrorMessage="Customer Name is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />--%>
                </div>
                 <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">Customer Registered Email</label>
                    <asp:Label ID="txtEmail" runat="server"></asp:Label>
                    <%--<asp:TextBox CssClass="form-control validate-group" ID="txtEmail" runat="server" placeholder="Customer Registered Email" TextMode="Email" MaxLength="50" Enabled="false"></asp:TextBox>--%>
                    <%-- <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        ErrorMessage="Invalid email format" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />--%>
                </div>
                 <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">Customer Resistered Mobile No.</label>
                    <asp:Label ID="txtCustomerMobileNo" runat="server"></asp:Label>
                    <%--<asp:TextBox CssClass="form-control validate-group" ID="txtCustomerMobileNo" runat="server" placeholder="Customer Resistered Mobile No."
                        pattern="\d{10}" title="Please enter a valid 10-digit mobile number" TextMode="Number" oninput="validateMobileNumber(this)" MaxLength="10"
                        Enabled="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCustomerMobile" runat="server" ControlToValidate="txtCustomerMobileNo"
                        ErrorMessage="Mobile number is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                    <asp:RegularExpressionValidator ID="revCustomerMobile" runat="server" ControlToValidate="txtCustomerMobileNo" ValidationExpression="^[6-9]\d{9}$"
                        ErrorMessage="Enter valid 10-digit mobile number" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />--%>
                </div>
                 <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">Whatsapp Number</label>
                    <asp:Label ID="txtWhatsappNo" runat="server"></asp:Label>
                    <%--<asp:TextBox CssClass="form-control validate-group" ID="txtWhatsappNo" runat="server" placeholder="Whatsapp Number" TextMode="Number"
                        pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" MaxLength="10" Enabled="false"> </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvWhatsapp" runat="server" ControlToValidate="txtWhatsappNo"
                        ErrorMessage="Whatsapp number is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                    <asp:RegularExpressionValidator ID="revWhatsapp" runat="server" ControlToValidate="txtWhatsappNo" ValidationExpression="^[6-9]\d{9}$"
                        ErrorMessage="Enter valid 10-digit number" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />--%>
                </div>


                 <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">Pincode</label>
                    <asp:Label ID="txtPincode" runat="server"></asp:Label>
                    <%--                    <asp:TextBox CssClass="form-control validate-group" ID="txtPincode" runat="server" placeholder="Pincode" MaxLength="6" pattern="\d{6}"
                        title="Enter a 6-digit Pincode" oninput="validatePincode(this)" AutoPostBack="true" Enabled="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPincode" runat="server" ControlToValidate="txtPincode" ErrorMessage="Pincode is required" CssClass="text-danger"
                        Display="Dynamic" ValidationGroup="CustomerDetails" />
                    <asp:RegularExpressionValidator ID="revPincode" runat="server" ControlToValidate="txtPincode" ValidationExpression="^\d{6}$"
                        ErrorMessage="Enter a valid 6-digit pincode" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />--%>
                </div>
                 <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">City</label>
                    <asp:Label ID="txtCity" runat="server"></asp:Label>
                    <%--                    <asp:TextBox class="form-control validate-group" ID="txtCity" runat="server" placeholder="City" CssClass="form-control mb-2" MaxLength="20" Enabled="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity" ErrorMessage="City is required" CssClass="text-danger"
                        Display="Dynamic" ValidationGroup="CustomerDetails" />--%>
                </div>
                 <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">State</label>
                    <asp:Label ID="txtState" runat="server"></asp:Label>
                    <%--                    <asp:TextBox class="form-control validate-group" ID="txtState" runat="server" placeholder="State" CssClass="form-control mb-2" MaxLength="20" Enabled="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="txtState" ErrorMessage="State is required" CssClass="text-danger"
                        Display="Dynamic" ValidationGroup="CustomerDetails" />--%>
                </div>

                 <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">Correspondance Address</label>
                    <asp:Label ID="txtAddressLine1" runat="server"></asp:Label>
                    <%--                    <textarea class="form-control validate-group" id="txtAddressLine1" runat="server" placeholder="Correspondance Address"
                        maxlength="150" disabled="disabled"></textarea>--%>
                </div>
                 <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">Landmark</label>
                    <asp:Label ID="txtLandmark" runat="server"></asp:Label>
                    <%--                    <textarea class="form-control validate-group" id="txtLandmark" runat="server" placeholder="Landmark" maxlength="50"
                        oninput="validateLandmarkWords()" disabled="disabled"></textarea>
                    <span id="landmarkError" style="color: red; font-size: 12px;"></span>
                    <asp:Label ID="lblLandmarkError" runat="server" ForeColor="Red" Visible="false" />
                    <asp:RequiredFieldValidator ID="rfvLandmark" runat="server" ControlToValidate="txtLandmark" ErrorMessage="Landmark is required"
                        CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />--%>
                </div>
            </div>

        </div>
    </div>
    <div class="card mb-3">
        <h5 class="card-header" style="text-align: left;">Product Details</h5>
        <div class="card-body pt-2 px-3 pb-2">
            <div class="d-flex align-items-center flex-wrap row">
                <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">Product Name</label>
                    <asp:Label ID="txtProductName" runat="server"></asp:Label>
                    <%--<asp:TextBox CssClass="form-control validate-group" ID="txtProductName" runat="server" placeholder="Product Name" MaxLength="50" Enabled="false"></asp:TextBox>--%>
                </div>
                <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">Product Sub Category Name</label>
                    <asp:Label ID="txtProductsubcategoryname" runat="server"></asp:Label>
                    <asp:HiddenField ID="prodcuCat" runat="server" />
                    <%--<asp:TextBox CssClass="form-control validate-group" ID="txtProductsubcategoryname" runat="server" placeholder="Product Sub Category Name" MaxLength="50" Enabled="false"> </asp:TextBox>--%>
                </div>
                <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">Brand</label>
                    <asp:Label ID="txtBrand" runat="server"></asp:Label>
                    <%--<asp:TextBox CssClass="form-control validate-group" ID="txtBrand" runat="server" placeholder="Brand" MaxLength="50" Enabled="false"></asp:TextBox>--%>
                </div>

                <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">Model Name</label>
                    <asp:Label ID="txtModelname" runat="server"></asp:Label>
                    <%--<asp:TextBox CssClass="form-control validate-group" ID="txtModelname" runat="server" placeholder="Model Name" Enabled="false"></asp:TextBox>--%>
                </div>
                <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">IMEI/Serial No.</label>
                    <asp:Label ID="txtIMEI" runat="server"></asp:Label>
                    <%--<asp:TextBox CssClass="form-control validate-group" ID="txtIMEI" runat="server" placeholder="IMEI/Serial No" Enabled="false"> </asp:TextBox>--%>
                </div>
                <div class="customer-details-txt col-12 col-lg-3 nb-1">
                    <label for="validationCustom01" class="mb-0">Device Purchase Price</label>
                    <asp:Label ID="txtDevicePurchasePrice" runat="server"></asp:Label>
                    <%--<asp:TextBox CssClass="form-control validate-group" ID="txtDevicePurchasePrice" runat="server" placeholder="Device Purchase Price" Enabled="false"> </asp:TextBox>--%>
                </div>

                <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">Product Purchase Date</label>
                    <asp:Label ID="txtProductPurchaseDate" runat="server"></asp:Label>
                    <%--<asp:TextBox CssClass="form-control validate-group" ID="txtProductPurchaseDate" runat="server" placeholder="Product Purchase Date" Enabled="false"></asp:TextBox>--%>
                </div>
                <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">Plan Name</label>
                    <asp:Label ID="txtPlanName" runat="server"></asp:Label>
                    <asp:HiddenField ID="hdnPlanId" runat="server" />
                    <%--<asp:TextBox ID="txtPlanName" runat="server" placeholder="Plan Name" CssClass="form-control mb-2" MaxLength="20" Enabled="false"></asp:TextBox>--%>
                </div>
                <div class="customer-details-txt col-12 col-lg-3 mb-1">
                    <label for="validationCustom01" class="mb-0">Plan Price</label>
                    <asp:Label ID="txtPlanPrice" runat="server"></asp:Label>
                    <%--<asp:TextBox ID="txtPlanPrice" runat="server" placeholder="Plan Price" CssClass="form-control mb-2" MaxLength="20" Enabled="false"></asp:TextBox>--%>
                </div>
            </div>

        </div>
    </div>
    <div class="card mb-3">
        <h5 class="card-header" style="text-align: left;">Register Complaint</h5>
        <div class="card-body px-3">
            <div class="row">
                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-12 mb-3">
                    <label for="ProblemCategory">Problem Category</label>
                    <asp:DropDownList ID="ddlProblemCategory" runat="server" CssClass="form-control" OnSelectedIndexChanged="problemCategoryChange" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtproblemcategory" Visible="false" runat="server" autocomplete="off" CssClass="form-control" MaxLength="150"></asp:TextBox>
                </div>
                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-12 mb-3">
                    <label for="SymptomsProblem">Symptoms / Problem</label>
                    <asp:DropDownList ID="ddlSymptomsProblem" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-12 mb-3">
                    <label for="suspectedParts">Suspected Parts</label>
                    <asp:TextBox ID="txtSuspectedParts" runat="server"  CssClass="form-control" placeholder="Suspected Parts" MaxLength="250" AutoComplete="off"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                    <label for="validationCustom01">Problem Reported (Detailed Description Required)<span style="color: red;"> *</span> </label>
                    <textarea class="form-control validate-group" id="txtProblemDesc" runat="server" placeholder="Problem Reported (Voice of Customer - as he / she reports)"
                        maxlength="450" oninput="validateProblemWords()" AutoComplete="off"></textarea>
                    <span id="problemError" style="color: red; font-size: 13px;"></span>
                    <asp:Label ID="lblProblemDesc" runat="server" ForeColor="Red" Font-Size="13px" Visible="false" />
                </div>
            </div>
            <div id="mobileDiv" runat="server" visible="false">
                <div class="row">
                    <div class="col-xl-3 col-lg-3 col-md-3 col-sm-6 col-12 mb-3">
                        <label>Type of Damage <span style="color: red;">*</span></label>
                        <div class="d-flex" style="gap: 10px">
                            <div class="form-check">
                                <asp:RadioButton ID="rdoPhysical" runat="server" GroupName="DamageType" CssClass="form-check-input" />
                                <label class="form-check-label" for="rdoPhysical">Physical</label>
                            </div>
                            <div class="form-check">
                                <asp:RadioButton ID="rdoLiquid" runat="server" GroupName="DamageType" CssClass="form-check-input" />
                                <label class="form-check-label" for="rdoLiquid">Liquid</label>
                            </div>
                            <div class="form-check">
                                <asp:RadioButton ID="rdoBoth" runat="server" GroupName="DamageType" CssClass="form-check-input" />
                                <label class="form-check-label" for="rdoBoth">Both</label>
                            </div>
                        </div>
                        <asp:Label ID="lblDamageType" runat="server" Visible="false" Style="color: red; font-size: 13px;"></asp:Label>
                    </div>
                    <div class="col-xl-3 col-lg-3 col-md-3 col-sm-6 col-6 mb-3">
                        <label for="validationCustom01">Is the device switching on <span style="color: red;">*</span></label>
                        <div class="d-flex" style="gap: 10px">
                            <div class="form-check">
                                <asp:RadioButton ID="rblphoneswitchingon" runat="server" GroupName="SwitchingOn" CssClass="form-check-input" />
                                <label class="form-check-label" for="rdoPhysical">Yes</label>
                            </div>
                            <div class="form-check">
                                <asp:RadioButton ID="rblphoneswitchingnot" runat="server" GroupName="SwitchingOn" CssClass="form-check-input" />
                                <label class="form-check-label" for="rdoPhysical">No</label>
                            </div>
                        </div>
                        <asp:Label ID="lblDeviceSwitchOn" runat="server" Visible="false" Style="color: red;" Font-Size="13px"></asp:Label>
                    </div>
                    <div class="col-xl-4 col-lg-4 col-md-4 col-sm-6 col-6 mb-3">
                        <label for="validationCustom01" class="mb-0">Select Defective Parts: <span style="color: red;">*</span></label>
                        <asp:CheckBoxList ID="chkDefectiveParts" runat="server" CssClass="defective-part-lists">
                            <asp:ListItem Text="Screen / Display"></asp:ListItem>
                            <asp:ListItem Text="Camera"></asp:ListItem>
                            <asp:ListItem Text="Button"></asp:ListItem>
                            <asp:ListItem Text="Front Cover"></asp:ListItem>
                            <asp:ListItem Text="Back Cover"></asp:ListItem>
                            <asp:ListItem Text="Others"></asp:ListItem>
                        </asp:CheckBoxList>
                        <asp:Label ID="lblDefectiveParts" runat="server" Visible="false" Font-Size="13px" Style="color: red;"></asp:Label>
                    </div>
                    <div class="col-xl-2 col-lg-2 col-md-2 col-sm-6 col-6 mb-3">
                        <label for="validationCustom01">Touch Screen Working <span style="color: red;">*</span></label>
                        <div class="d-flex" style="gap: 10px">
                            <div class="form-check">
                                <asp:RadioButton ID="touchworking" runat="server" GroupName="TouchWorking" CssClass="form-check-input" />
                                <label class="form-check-label" for="rdoPhysical">Yes</label>
                            </div>
                            <div class="form-check">
                                <asp:RadioButton ID="touchworkingnot" runat="server" GroupName="TouchWorking" CssClass="form-check-input" />
                                <label class="form-check-label" for="rdoPhysical">No</label>
                            </div>
                        </div>
                        <asp:Label ID="lblTouchWorking" runat="server" Visible="false" Font-Size="13px" Style="color: red;"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xl-3 col-lg-3 col-md-3 col-sm-6 col-6 mb-3">
                    <label for="validationCustom01">Damage Date / Time <span style="color: red;">*</span></label>
                    <div class="d-flex row">
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 mb-3 pr-0">
                            <div class="d-flex">
                                <asp:TextBox ID="txtDamageDate" runat="server" CssClass="form-control" placeholder=""
                                    AutoCompleteType="Disabled" AutoComplete="off">
                                </asp:TextBox>
                                <div class="input-group-append">
                                    <span class="input-group-text" style="cursor: pointer;"
                                        onclick="document.getElementById('<%= txtDamageDate.ClientID %>').focus();">
                                        <i class="fa fa-calendar"></i>
                                    </span>
                                </div>
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtDamageDate" EndDate="<%# DateTime.Today %>"></cc1:CalendarExtender>
                            </div>
                            <asp:Label ID="lblDamageDate" runat="server" Visible="false" Font-Size="13px" Style="color: red;"></asp:Label>
                        </div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 mb-3">
                            <asp:TextBox ID="txtDamageTime" data-provide="timepicker" placeholder="Damage Time" TextMode="Time"
                                runat="server" autocomplete="off" CssClass="form-control timepicker" MaxLength="12">
                            </asp:TextBox>
                            <asp:Label ID="lblDamageTime" runat="server" Visible="false" Font-Size="13px" Style="color: red;"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-3 col-sm-6 col-6 mb-3">
                    <label for="validationCustom01">Preferred Date / Time <span style="color: red;">*</span></label>
                    <div class="d-flex row">
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 mb-3 pr-0">
                            <div class="d-flex">
                                <asp:TextBox ID="txtPreferredDate" runat="server" CssClass="form-control" placeholder=""
                                    AutoCompleteType="Disabled" AutoComplete="off">
                                </asp:TextBox>
                                <div class="input-group-append">
                                    <span class="input-group-text" style="cursor: pointer;"
                                        onclick="document.getElementById('<%= txtPreferredDate.ClientID %>').focus();">
                                        <i class="fa fa-calendar"></i>
                                    </span>
                                </div>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtPreferredDate" EndDate="<%# DateTime.Today %>"></cc1:CalendarExtender>
                            </div>
                            <asp:Label ID="Label1" runat="server" Visible="false" Font-Size="13px" Style="color: red;"></asp:Label>
                        </div>
                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 mb-3">
                            <asp:TextBox ID="txtPreferredTime" data-provide="timepicker" placeholder="Damage Time" TextMode="Time"
                                runat="server" autocomplete="off" CssClass="form-control timepicker" MaxLength="12">
                            </asp:TextBox>
                            <asp:Label ID="Label3" runat="server" Visible="false" Font-Size="13px" Style="color: red;"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 mb-3">
                    <label for="validationCustom01">Place of Damage <span style="color: red;">*</span></label>
                    <asp:TextBox ID="txtPlaceOfDamage" runat="server" CssClass="form-control" placeholder="Place of Damage" MaxLength="50" AutoComplete="off"></asp:TextBox>
                    <asp:Label ID="lblPlaceOfDamage" runat="server" Visible="false" Font-Size="13px" Style="color: red;"></asp:Label>
                </div>
            </div>
            <div class="row">
            </div>

            <div class="row">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                    <label for="validationCustom01">Remarks if Any</label>
                    <textarea class="form-control validate-group" id="txtRemarks" runat="server" placeholder="Remarks if Any"
                        maxlength="150" AutoComplete="off"></textarea>
                </div>
            </div>
            <div class="form-row">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12" style="text-align: center;">
                    <asp:Button ID="btnRegisterClaim" CssClass="btn btn-primary" runat="server" OnClick="RegisterClaim" Text="Register Claim"
                        OnClientClick="return validateGroupFields();" />
                </div>
                <div class=" col-md-12" style="text-align: center; margin-top: 20px; font-size: 18px;">
                    <asp:Label ID="lbltkt" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    <div class="card mb-3" id="divdoc" runat="server" visible="false">
        <h5 class="card-header" style="text-align: left;">Upload Documents</h5>
        <div class="card-body py-2">
            <div class="row">
                <div id="UploadDocumentPanel" runat="server">
                    <div class=" col-md-12">
                        <p class="text-left">
                            Please submit supporting documents by selecting Correct Document Title.
                       <br />
                            Please submit each document file separately. Files containing multiple images may
                       be rejected.
                       <br />
                            Before submitting, please check that the image quality is good, readable and relevant
                       to the claim. This will help us to serve you better.
                        </p>
                    </div>
                    <div class="row UploadDocumentCls">
                        <div class=" col-md-3">
                            <label>
                                <asp:DropDownList ID="ddldocumentattached2" runat="server" CssClass="form-control ">
                                </asp:DropDownList>
                            </label>
                        </div>
                        <div class=" col-md-3">
                            <asp:FileUpload runat="server" name="ImageUpload" accept=".jpg,.jpeg,.png,.pdf"
                                ID="fupupload2" onchange="ShowPreview(this)" />
                        </div>
                        <div class=" col-md-2">
                            <asp:Button ID="btnfuupload" OnClick="UploadImage1" runat="server" CssClass="btn next-step mt-0"
                                Text="Submit Document" />
                        </div>
                        <asp:Label ID="lblMessage" runat="server" CssClass="text-success"></asp:Label>
                    </div>
                    <div class="col-md-4 hidden" visible="false" runat="server">
                        <span>Image Preview</span><br />
                        <asp:Image ID="impPrev" runat="server" Width="300px" Height="330px" ImageUrl="../Document/not_available.jpg" />
                    </div>
                </div>
                <div class="col-md-12">
                    <asp:GridView ID="GVSupportingDoc" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead" EmptyDataText="No records available. Please refine your search.">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MID" Visible="false">
                                <ItemTemplate>
                                    <asp:HiddenField ID="lblMid" runat="server" Value='<%# Eval("Mid") %>'></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="View Document">
                                <ItemTemplate>
                                    <a href='<%# Eval("FullDocumentPath") %>' target="_blank" title="View Document">
                                        <i class="fa fa-eye" aria-hidden="true"></i>
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Document Name">
                                <ItemTemplate>
                                    <%# Eval("DocumentName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Uploaded Date">
                                <ItemTemplate>
                                    <%# Eval("UploadedDate") %>'
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblDocStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <%--<div class="panel panel-default form-group row" style="margin-left: 10px">
            <div class="card-body px-3">
                <div class="panel-body">
                    <div id="Div1" runat="server" class=" form-group row">
                        <div class="col-md-12">
                            <div>
                                <div class="col-md-12">
                                    <div class=" col-md-12">
                                        <p class="text-left">
                                            Please submit supporting documents by selecting Correct Document Title.
                                                                <br />
                                            Please submit each document file separately. Files containing multiple images may
                                                                be rejected.
                                                                <br />
                                            Before submitting, please check that the image quality is good, readable and relevant
                                                                to the claim. This will help us to serve you better.
                                        </p>
                                    </div>
                                    <div class="row" style="text-align: center; margin-top: 10px;">
                                        <div class=" col-md-3">
                                            <label>
                                                <asp:DropDownList ID="ddldocumentattached2" runat="server" CssClass="form-control ">
                                                </asp:DropDownList>
                                            </label>
                                        </div>
                                        <div class=" col-md-3">
                                            <asp:FileUpload runat="server" name="ImageUpload" accept=".jpg,.jpeg,.png,.pdf"
                                                ID="fupupload2" onchange="ShowPreview(this)" />
                                        </div>
                                        <div class=" col-md-2">
                                            <asp:Button ID="btnfuupload" OnClick="UploadImage1" runat="server" CssClass="btn btn-success"
                                                Text="Submit Document" />
                                        </div>
                                    </div>
                                    <div class="col-md-4 hidden" Visible="false" runat="server">
                                        <span>Image Preview</span><br />
                                        <asp:Image ID="impPrev" runat="server" Width="300px" Height="330px" ImageUrl="../Document/not_available.jpg" />
                                    </div>
                                </div>
                                <div class=" col-md-12">
                                    <hr />
                                </div>
                                <div class="col-md-12">
                                    <asp:GridView ID="GridView1" Visible="true" OnRowCommand="gvFiles_RowCommand" Width="100%"
                                        OnRowDataBound="ChangeColour" AutoGenerateColumns="false" runat="server" RowStyle-CssClass="rows"
                                        HeaderStyle-CssClass="header" CssClass="mydatagrid">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr. No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex+ 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false" HeaderText="View">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkview" Enabled='<%#Bind("ststus") %>' ForeColor=" green" ToolTip="View Uploaded Documents"
                                                        Width="40px" CommandName="ViewDOC" Text="View DOC" CommandArgument='<%#Eval("DocumentPath") %>'
                                                        runat="server"> <i class="fa fa-share" aria-hidden="true" style="font-size: 28px;"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="View" Visible="true">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkopen" Visible='<%#Bind("ststus") %>' ForeColor="green" ToolTip="Open Uploaded Documents in tab"
                                                        Width="40px" CommandName="ViewDOCopen" CommandArgument='<%# Eval("DocumentPath") %>'
                                                        runat="server"> <i class="fa fa-eye" aria-hidden="true" style="font-size: 28px;"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false" HeaderText="Ticket No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblticketno" runat="server" Text='<%#Bind("TicketNo") %>'></asp:Label>
                                                    <asp:Label ID="lblid" runat="server" Text='<%#Bind("mid") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Document Title">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldocumentname" runat="server" Text='<%#Bind("DocumentName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false" HeaderText="Document Attached">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDocumentPath" runat="server" Text='<%#Bind("DocumentPath") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblverify" Width="75px" runat="server" Text='<%#Bind("DocStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Uploaded By" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbluoloadedby" Width="75px" runat="server" Text='<%#Bind("CreatedBy") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Uploaded Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcreateddate" Width="75px" runat="server" Text='<%#Bind("CreateDate") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldocremarks" runat="server" Text='<%#Bind("docremarks") %>' Width="250px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="250px" Wrap="true" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false" HeaderText="Request To Ignore">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txthold" TextMode="MultiLine" MaxLength="200" class="form-control input-sm"
                                                        Text="" runat="server"></asp:TextBox>
                                                    <div style="margin-left: 78px;">
                                                        <asp:LinkButton ID="lnkdelete" ToolTip="Ignore Uploaded Documents" Width="40px" Text="View DOC"
                                                            OnClick="Deletefile" OnClientClick="return confirm('Are you sure you want to Request To Ignore this file?');"
                                                            runat="server"> <i class="fa fa-eraser" aria-hidden="true" style="font-size: 21px;color: red;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle ForeColor="White" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <asp:Button Text="Download Selcted Files" ID="btndownload" CssClass="btn-info btn-sm"
                                Visible="false" OnClick="DownloadAll" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>
    </div>
</asp:Content>
