<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CartDetails.aspx.cs" Inherits="Patner_Retailer_ADO.CartDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            const corresAddress = document.getElementById('<%= txtAddressLine1.ClientID %>');
              const corresLandmark = document.getElementById('<%= txtLandmark.ClientID %>');
        const installAddress = document.getElementById('<%= txtAvaility.ClientID %>');
              const installLandmark = document.getElementById('<%= txtinstalledLandmark.ClientID %>');

              if (checkbox.checked) {
                  installAddress.value = corresAddress.value;
                  installLandmark.value = corresLandmark.value;
              } else {
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
            const wordCount = landmarkInput.value.trim().split(/\s+/).filter(w => w).length;

            if (wordCount < 3) {
                errorDiv.textContent = "Landmark must contain at least 3 words.";
            } else {
                errorDiv.textContent = "";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid  dashboard-content">

        <div class="row">
            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                <div class="card">
                    <%-- <h5 class="card-header">Cart Details</h5>--%>
                    <div class="card-body" style="display: flex;">
                        <div class="col-md-8">
                            <h4>Cart Details</h4>

                            <asp:Repeater ID="rptPlans" runat="server">
                                <HeaderTemplate>
                                    <table class="table table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Product Name</th>
                                                <th>Product Price</th>
                                                <th>Plan Selected</th>
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
                                            <asp:Label ID="lblDevicePurchasePrice" runat="server" Text='<%# Eval("DevicePurchasePrice") %>'></asp:Label>
                                        </td>
                                        <td><%# Eval("PlanName") %></td>
                                        <td>
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>

                                        </td>
                                        <td>
                                            <asp:Label ID="lblPlanPrice" runat="server" Text='<%# Eval("PlanPrice") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Mid") %>' OnClick="DeletePlanInfo" CssClass="btn btn-sm btn-danger">
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
                            <hr />
                            <asp:Button ID="btnAddMore" runat="server" class="btn btn-primary" Text="➕ Add More Products" OnClick="AddMoreProducts"
                                CausesValidation="false" UseSubmitBehavior="true" OnClientClick="this.form.noValidate = true;" />

                        </div>
                        <div class="col-md-4">
                            <div class="card bg-light mb-3">
                                <div class="card-header">Your Order Summary</div>
                                <div class="card-body">
                                    <div class="d-flex justify-content-between align-items-center mb-2">
                                        Total Item : 
                                        <strong><p id="txtQuantity" runat="server"></p></strong>
                                    </div>
                                    <div class="d-flex justify-content-between align-items-center mb-2">
                                        Taxable Value:
                                        <strong><p id="TaxableValue" runat="server"></p></strong>
                                    </div>
                                    <div class="d-flex justify-content-between align-items-center mb-2">
                                        GST(18%) :
                                        <strong><p id="TaxAmout" runat="server"></p></strong>
                                    </div>
                                    <hr>
                                    <div class="d-flex justify-content-between align-items-center">
                                        Total Net Value (Including Tax):
                                        <strong><p id="TotalAmountPay" runat="server"></p></strong>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <!-- ============================================================== -->
            <!-- validation form -->
            <!-- ============================================================== -->
            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                <div class="card">
                    <h5 class="card-header" style="text-align: center;">Customer Details</h5>
                    <div class="card-body">

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">First Name</label>
                                <asp:TextBox class="form-control validate-group" ID="txtFirstName" runat="server" placeholder="First Name" maxlength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                                    ErrorMessage="First Name is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Last Name</label>
                                <asp:TextBox class="form-control validate-group" ID="txtLastName" runat="server" placeholder="Last Name" maxlength="50"> </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                                    ErrorMessage="Last Name is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="CustomerDetails" />
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">                                
                                <label for="validationCustom01">Customer Registered Email</label>
                                <asp:TextBox class="form-control validate-group" ID="txtEmail" runat="server" placeholder="Customer Registered Email" TextMode="Email" maxlength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ErrorMessage="Invalid email format" CssClass="text-danger" Display="Dynamic"  ValidationGroup="CustomerDetails" />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Alternative Email</label>
                                <asp:TextBox class="form-control validate-group" ID="txtAlternativeEmail" runat="server" placeholder="Alternative Email" TextMode="Email" maxlength="50"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revAltEmail" runat="server" ControlToValidate="txtAlternativeEmail" 
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Invalid email format" CssClass="text-danger" Display="Dynamic"  ValidationGroup="CustomerDetails" />
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Customer Resistered Mobile No.</label>
                                <asp:TextBox class="form-control validate-group" ID="txtCustomerMobileNo" runat="server" placeholder="Customer Resistered Mobile No."
                                    pattern="\d{10}" title="Please enter a valid 10-digit mobile number" TextMode="Number" oninput="validateMobileNumber(this)" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCustomerMobile" runat="server" ControlToValidate="txtCustomerMobileNo" 
                                    ErrorMessage="Mobile number is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                                <asp:RegularExpressionValidator ID="revCustomerMobile" runat="server" ControlToValidate="txtCustomerMobileNo" ValidationExpression="^[6-9]\d{9}$"
                                    ErrorMessage="Enter valid 10-digit mobile number" CssClass="text-danger" Display="Dynamic"  ValidationGroup="CustomerDetails" />
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Alternative Mobile No.</label>
                                <asp:TextBox class="form-control validate-group" ID="txtAlternativeMobile" runat="server" placeholder="Alternative Mobile No."
                                    pattern="\d{10}" title="Please enter a valid 10-digit mobile number" TextMode="Number" oninput="validateMobileNumber(this)" MaxLength="10"> </asp:TextBox>
                                <asp:RegularExpressionValidator ID="revAltMobile" runat="server" ControlToValidate="txtAlternativeMobile" ValidationExpression="^[6-9]\d{9}$" 
                                    ErrorMessage="Enter valid 10-digit mobile number" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">Whatsapp Number</label>
                                <asp:TextBox class="form-control validate-group" ID="txtWhatsappNo" runat="server" placeholder="Whatsapp Number" TextMode="Number"
                                    pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" MaxLength="10"> </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvWhatsapp" runat="server" ControlToValidate="txtWhatsappNo"
                                    ErrorMessage="Whatsapp number is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                                <asp:RegularExpressionValidator ID="revWhatsapp" runat="server" ControlToValidate="txtWhatsappNo" ValidationExpression="^[6-9]\d{9}$"
                                    ErrorMessage="Enter valid 10-digit number" CssClass="text-danger" Display="Dynamic"  ValidationGroup="CustomerDetails" />
                            </div>

                            <div class="col-xl-2 col-lg-2 col-md-2 col-sm-2 col-2 mb-3">
                                <label for="validationCustom01">Pincode</label>
                                <asp:TextBox class="form-control validate-group" ID="txtPincode" runat="server" placeholder="Pincode" maxlength="6" pattern="\d{6}"
                                    title="Enter a 6-digit Pincode" oninput="validatePincode(this)" AutoPostBack="true" OnTextChanged="txtPIN_TextChanged"></asp:TextBox>
                                  <asp:RequiredFieldValidator ID="rfvPincode" runat="server" ControlToValidate="txtPincode" ErrorMessage="Pincode is required" CssClass="text-danger" 
                                      Display="Dynamic" ValidationGroup="CustomerDetails" />
                                <asp:RegularExpressionValidator ID="revPincode" runat="server" ControlToValidate="txtPincode" ValidationExpression="^\d{6}$"
                                    ErrorMessage="Enter a valid 6-digit pincode" CssClass="text-danger" Display="Dynamic" ValidationGroup="CustomerDetails" />
                            </div>

                            <div class="col-xl-2 col-lg-2 col-md-2 col-sm-2 col-2 mb-3">
                                <label for="validationCustom01">City</label>
                                <asp:TextBox class="form-control validate-group" ID="txtCity" runat="server" placeholder="City" CssClass="form-control mb-2" maxlength="20"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity" ErrorMessage="City is required" CssClass="text-danger" 
                                    Display="Dynamic"  ValidationGroup="CustomerDetails" />
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 mb-3">
                                <label for="validationCustom01">State</label>
                                <asp:TextBox class="form-control validate-group" ID="txtState" runat="server" placeholder="State" CssClass="form-control mb-2" MaxLength="20"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="txtState" ErrorMessage="State is required" CssClass="text-danger"
                                    Display="Dynamic"  ValidationGroup="CustomerDetails" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                                <label for="validationCustom01">Correspondance Address</label>                                
                                <textarea class="form-control validate-group" id="txtAddressLine1" runat="server" placeholder="Correspondance Address" maxlength="150" oninput="validateAddressWords()"></textarea>
                                <span id="addressError" style="color: red; font-size: 12px;"></span>
                                <asp:Label ID="lblAddressError" runat="server" ForeColor="Red" Visible="false" />
                                <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddressLine1" ErrorMessage="Address is required" 
                                    CssClass="text-danger" Display="Dynamic"  ValidationGroup="CustomerDetails" />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                                <label for="validationCustom01">Landmark</label>
                                <textarea class="form-control validate-group" id="txtLandmark" runat="server" placeholder="Landmark" maxlength="50" oninput="validateLandmarkWords()"></textarea>
                                <span id="landmarkError" style="color: red; font-size: 12px;"></span>
                                <asp:Label ID="lblLandmarkError" runat="server" ForeColor="Red" Visible="false" />
                                 <asp:RequiredFieldValidator ID="rfvLandmark" runat="server" ControlToValidate="txtLandmark" ErrorMessage="Landmark is required" 
                                     CssClass="text-danger" Display="Dynamic"  ValidationGroup="CustomerDetails" />
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
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                                <label for="validationCustom01">Product Install Address</label>
                                <textarea class="form-control validate-group" id="txtAvaility" runat="server" placeholder="Product Install Address"
                                    maxlength="150"> </textarea>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                                <label for="validationCustom01">Product Install Landmark</label>
                                <textarea class="form-control validate-group" id="txtinstalledLandmark" runat="server" placeholder="Product Install Landmark"
                                    maxlength="50"> </textarea>
                            </div>
                        </div>
                        <hr />
                        <div class="form-row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12" style="text-align: center;">
                                <asp:Button ID="btnContinuePayment" CssClass="btn btn-primary" runat="server" OnClick="ContinuePayment" Text="Continue To Payment"
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
</asp:Content>
