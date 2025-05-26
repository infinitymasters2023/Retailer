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
            margin-bottom:0px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid  dashboard-content">
        <div class="row">
            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                <div class="card">
                    <h5 class="card-header" style="text-align: center;">Customer Details</h5>
                    <div class="card-body">

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">First Name</label>
                                <asp:TextBox CssClass="form-control validate-group" ID="txtFirstName" runat="server" placeholder="First Name" MaxLength="50" Enabled="false"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                                    ErrorMessage="First Name is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Last Name</label>
                                <asp:TextBox CssClass="form-control validate-group" ID="txtLastName" runat="server" placeholder="Last Name" MaxLength="50" Enabled="false"> </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                                    ErrorMessage="Last Name is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Customer Registered Email</label>
                                <asp:TextBox CssClass="form-control validate-group" ID="txtEmail" runat="server" placeholder="Customer Registered Email" TextMode="Email" MaxLength="50" Enabled="false"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ErrorMessage="Invalid email format" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Customer Resistered Mobile No.</label>
                                <asp:TextBox CssClass="form-control validate-group" ID="txtCustomerMobileNo" runat="server" placeholder="Customer Resistered Mobile No."
                                    pattern="\d{10}" title="Please enter a valid 10-digit mobile number" TextMode="Number" oninput="validateMobileNumber(this)" MaxLength="10"
                                    Enabled="false"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCustomerMobile" runat="server" ControlToValidate="txtCustomerMobileNo"
                                    ErrorMessage="Mobile number is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                                <asp:RegularExpressionValidator ID="revCustomerMobile" runat="server" ControlToValidate="txtCustomerMobileNo" ValidationExpression="^[6-9]\d{9}$"
                                    ErrorMessage="Enter valid 10-digit mobile number" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Whatsapp Number</label>
                                <asp:TextBox CssClass="form-control validate-group" ID="txtWhatsappNo" runat="server" placeholder="Whatsapp Number" TextMode="Number"
                                    pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" MaxLength="10" Enabled="false"> </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvWhatsapp" runat="server" ControlToValidate="txtWhatsappNo"
                                    ErrorMessage="Whatsapp number is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                                <asp:RegularExpressionValidator ID="revWhatsapp" runat="server" ControlToValidate="txtWhatsappNo" ValidationExpression="^[6-9]\d{9}$"
                                    ErrorMessage="Enter valid 10-digit number" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                            </div>


                        </div>

                        <div class="row">
                            <div class="col-xl-2 col-lg-2 col-md-2 col-sm-2 col-2 mb-3">
                                <label for="validationCustom01">Pincode</label>
                                <asp:TextBox CssClass="form-control validate-group" ID="txtPincode" runat="server" placeholder="Pincode" MaxLength="6" pattern="\d{6}"
                                    title="Enter a 6-digit Pincode" oninput="validatePincode(this)" AutoPostBack="true" Enabled="false"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPincode" runat="server" ControlToValidate="txtPincode" ErrorMessage="Pincode is required" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="CustomerDetails" />
                                <asp:RegularExpressionValidator ID="revPincode" runat="server" ControlToValidate="txtPincode" ValidationExpression="^\d{6}$"
                                    ErrorMessage="Enter a valid 6-digit pincode" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                            </div>

                            <div class="col-xl-2 col-lg-2 col-md-2 col-sm-2 col-2 mb-3">
                                <label for="validationCustom01">City</label>
                                <asp:TextBox class="form-control validate-group" ID="txtCity" runat="server" placeholder="City" CssClass="form-control mb-2" MaxLength="20" Enabled="false"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity" ErrorMessage="City is required" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="CustomerDetails" />
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">State</label>
                                <asp:TextBox class="form-control validate-group" ID="txtState" runat="server" placeholder="State" CssClass="form-control mb-2" MaxLength="20" Enabled="false"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="txtState" ErrorMessage="State is required" CssClass="text-danger"
                                    Display="Dynamic" ValidationGroup="CustomerDetails" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                                <label for="validationCustom01">Correspondance Address</label>
                                <textarea class="form-control validate-group" id="txtAddressLine1" runat="server" placeholder="Correspondance Address"
                                    maxlength="150" disabled="disabled"></textarea>
                                <%--   <span id="addressError" style="color: red; font-size: 12px;"></span>
                                <asp:Label ID="lblAddressError" runat="server" ForeColor="Red" Visible="false" />
                                <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddressLine1" ErrorMessage="Address is required"
                                    CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />--%>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                                <label for="validationCustom01">Landmark</label>
                                <textarea class="form-control validate-group" id="txtLandmark" runat="server" placeholder="Landmark" maxlength="50"
                                    oninput="validateLandmarkWords()" disabled="disabled"></textarea>
                                <span id="landmarkError" style="color: red; font-size: 12px;"></span>
                                <asp:Label ID="lblLandmarkError" runat="server" ForeColor="Red" Visible="false" />
                                <asp:RequiredFieldValidator ID="rfvLandmark" runat="server" ControlToValidate="txtLandmark" ErrorMessage="Landmark is required"
                                    CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                            </div>
                        </div>

                        <hr />
                        <%--   <div class="form-row">
                        <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12" style="text-align: center;">
                            <asp:Button ID="btnContinuePayment" CssClass="btn btn-primary" runat="server" OnClick="ContinuePayment" Text="Continue To Payment"
                                OnClientClick="return validateGroupFields();" ValidationGroup="CustomerDetails" />
                        </div>
                    </div>--%>
                    </div>
                </div>

                <div class="card">
                    <h5 class="card-header" style="text-align: center;">Product Details</h5>
                    <div class="card-body">

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Product Name</label>
                                <asp:TextBox CssClass="form-control validate-group" ID="txtProductName" runat="server" placeholder="Product Name" MaxLength="50" Enabled="false"></asp:TextBox>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Product Sub Category Name</label>
                                <asp:TextBox CssClass="form-control validate-group" ID="txtProductsubcategoryname" runat="server" placeholder="Product Sub Category Name" MaxLength="50" Enabled="false"> </asp:TextBox>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Brand</label>
                                <asp:TextBox CssClass="form-control validate-group" ID="txtBrand" runat="server" placeholder="Brand" MaxLength="50" Enabled="false"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Model Name</label>
                                <asp:TextBox CssClass="form-control validate-group" ID="txtModelname" runat="server" placeholder="Model Name" Enabled="false"></asp:TextBox>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">IMEI/Serial No.</label>
                                <asp:TextBox CssClass="form-control validate-group" ID="txtIMEI" runat="server" placeholder="IMEI/Serial No" Enabled="false"> </asp:TextBox>
                            </div>
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Device Purchase Price</label>
                                <asp:TextBox CssClass="form-control validate-group" ID="txtDevicePurchasePrice" runat="server" placeholder="Device Purchase Price" Enabled="false"> </asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Product Purchase Date</label>
                                <asp:TextBox CssClass="form-control validate-group" ID="txtProductPurchaseDate" runat="server" placeholder="Product Purchase Date" Enabled="false"></asp:TextBox>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Plan Name</label>
                                <asp:TextBox ID="txtPlanName" runat="server" placeholder="Plan Name" CssClass="form-control mb-2" MaxLength="20" Enabled="false"></asp:TextBox>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Plan Price</label>
                                <asp:TextBox ID="txtPlanPrice" runat="server" placeholder="Plan Price" CssClass="form-control mb-2" MaxLength="20"
                                    Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <hr />
                    </div>
                </div>

                <div class="card">
                    <h5 class="card-header" style="text-align: center;">Register Complaint</h5>
                    <div class="card-body">

                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                                <label for="validationCustom01">Problem Reported (Detailed Description Required)<span style="color: red;"> *</span> </label>
                                <textarea class="form-control validate-group" id="txtProblemDesc" runat="server" placeholder="Problem Reported (Voice of Customer - as he / she reports)"
                                    maxlength="250" oninput="validateProblemWords()"></textarea>
                                <span id="problemError" style="color: red;font-size:13px;"></span>
                                <asp:Label ID="lblProblemDesc" runat="server" ForeColor="Red" Font-Size="13px" Visible="false" />                                
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12 mb-3">
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
                                <asp:Label ID="lblDamageType" runat="server" Visible="false"  style="color: red;font-size:13px;"></asp:Label>
                            </div>


                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 mb-3">
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
                                <asp:Label ID="lblDeviceSwitchOn" runat="server" Visible="false"  style="color: red;" Font-Size="13px"></asp:Label>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 mb-3">
                                <label for="validationCustom01" class="mb-0">Select Defective Parts: <span style="color: red;">*</span></label>
                                <asp:CheckBoxList ID="chkDefectiveParts" runat="server" CssClass="defective-part-lists">
                                    <asp:ListItem Text="Screen / Display"></asp:ListItem>
                                    <asp:ListItem Text="Camera"></asp:ListItem>
                                    <asp:ListItem Text="Button"></asp:ListItem>
                                    <asp:ListItem Text="Front Cover"></asp:ListItem>
                                    <asp:ListItem Text="Back Cover"></asp:ListItem>
                                    <asp:ListItem Text="Others"></asp:ListItem>
                                </asp:CheckBoxList>
                                 <asp:Label ID="lblDefectiveParts" runat="server" Visible="false"  Font-Size="13px" style="color: red;"></asp:Label>
                            </div>

                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 mb-3">
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
                                <asp:Label ID="lblTouchWorking" runat="server" Visible="false" Font-Size="13px" style="color: red;"></asp:Label>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 mb-3">
                                <label for="validationCustom01" class="mb-0">Damage Date / Time <span style="color: red;">*</span></label>
                                <div class="d-flex">
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 mb-3">
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
                                        <cc1:calendarextender id="CalendarExtender3" runat="server" format="dd-MMM-yyyy"
                                            targetcontrolid="txtDamageDate" enddate="<%# DateTime.Today %>">
                                        </cc1:calendarextender>
                                            </div>
                                         <asp:Label ID="lblDamageDate" runat="server" Visible="false" Font-Size="13px" style="color: red; "></asp:Label>
                                    </div>
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 mb-3">
                                         <asp:TextBox ID="txtDamageTime" data-provide="timepicker" placeholder="Damage Time" TextMode="Time"
                                             runat="server" autocomplete="off" CssClass="form-control timepicker" MaxLength="12">
                                         </asp:TextBox>   
                                        <asp:Label ID="lblDamageTime" runat="server" Visible="false" Font-Size="13px" style="color: red;"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 mb-3">
                                <label for="validationCustom01">Place of Damage <span style="color: red;">*</span></label>
                               <asp:TextBox ID="txtPlaceOfDamage" runat="server" CssClass="form-control" placeholder="Place of Damage" MaxLength="50"></asp:TextBox>
                                <asp:Label ID="lblPlaceOfDamage" runat="server" Visible="false" Font-Size="13px" style="color: red;"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                                <label for="validationCustom01">Remarks if Any</label>
                                <textarea class="form-control validate-group" id="txtRemarks" runat="server" placeholder="Remarks if Any"
                                    maxlength="150"></textarea>
                            </div>
                        </div>

                        <hr />
                        <div class="form-row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12" style="text-align: center;">
                                <asp:Button ID="btnRegisterClaim" CssClass="btn btn-primary" runat="server" OnClick="RegisterClaim" Text="Register Claim"
                                    OnClientClick="return validateGroupFields();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
