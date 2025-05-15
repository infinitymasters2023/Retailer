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
                                <div class="card-header"><strong>Your Order Summary</strong></div>
                                <div class="card-body">
                                    <div>
                                        <strong>Total Item : </strong>
                                        <p id="txtQuantity" runat="server"></p>
                                    </div>
                                    <div>
                                        <strong>Taxable Value:</strong>
                                        <p id="TaxableValue" runat="server"></p>
                                    </div>
                                    <div>
                                        <strong>GST(18%) :</strong>
                                        <p id="TaxAmout" runat="server"></p>
                                    </div>
                                    <hr>
                                    <div>
                                        <strong>Total Net Value (Including Tax):</strong><p id="TotalAmountPay" runat="server"></p>
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
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">First Name</label>
                                <asp:TextBox class="form-control validate-group" id="txtFirstName" runat="server" placeholder="First Name" required="required" maxlength="50" />
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Last Name</label>
                                <asp:TextBox class="form-control validate-group" id="txtLastName" runat="server" placeholder="Last Name" required="required" maxlength="50" />
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Customer Registered Email</label>
                                <input type="email" class="form-control validate-group" id="txtEmail" runat="server" placeholder="Customer Registered Email" required maxlength="50">
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Alternative Email</label>
                                <input type="email" class="form-control validate-group" id="txtAlternativeEmail" runat="server" placeholder="Alternative Email" maxlength="50">
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Customer Resistered Mobile No.</label>
                                <asp:TextBox class="form-control validate-group" id="txtCustomerMobileNo" runat="server" placeholder="Customer Resistered Mobile No." required="required"
                                    pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" />
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Alternative Mobile No.</label>
                                <asp:TextBox class="form-control validate-group" id="txtAlternativeMobile" runat="server" placeholder="Alternative Mobile No."
                                    pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Whatsapp Number</label>
                                <asp:TextBox class="form-control validate-group" id="txtWhatsappNo" runat="server" placeholder="Whatsapp Number" required="required"
                                    pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" />
                            </div>

                            <div class="col-xl-2 col-lg-2 col-md-2 col-sm-2 col-2 ">
                                <label for="validationCustom01">Pincode</label>
                                <asp:TextBox class="form-control validate-group" id="txtPincode" runat="server" placeholder="Pincode" required="required"
                                    maxlength="6" pattern="\d{6}" title="Enter a 6-digit Pincode" oninput="validatePincode(this)" AutoPostBack="true" OnTextChanged="txtPIN_TextChanged" />
                            </div>

                            <div class="col-xl-2 col-lg-2 col-md-2 col-sm-2 col-2 ">
                                <label for="validationCustom01">City</label>
                                <asp:TextBox class="form-control validate-group" id="txtCity" runat="server" placeholder="City" CssClass="form-control mb-2" required="required"
                                    maxlength="20" />
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">State</label>
                                <asp:TextBox class="form-control validate-group" id="txtState" runat="server" placeholder="State" CssClass="form-control mb-2" required="required"
                                    maxlength="20" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 ">
                                <label for="validationCustom01">Correspondance Address</label>
                                <textarea class="form-control validate-group" id="txtAddressLine1" runat="server" placeholder="Correspondance Address" required
                                    maxlength="150"></textarea>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 ">
                                <label for="validationCustom01">Landmark</label>
                                <textarea class="form-control validate-group" id="txtLandmark" runat="server" placeholder="Landmark" required
                                    maxlength="50"></textarea>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                                <div class="form-group">
                                    <div class="form-check">
                                        <input class="form-check-input" type="checkbox" value="" id="invalidCheck">
                                        <label class="form-check-label" for="invalidCheck">
                                            Same as Correspondance Address
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 ">
                                <label for="validationCustom01">Product Install Address</label>
                                <textarea class="form-control validate-group" id="txtAvaility" runat="server" placeholder="Product Install Address"
                                    maxlength="150"> </textarea>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 ">
                                <label for="validationCustom01">Product Install Landmark</label>
                                <textarea class="form-control validate-group" id="txtinstalledLandmark" runat="server" placeholder="Product Install Landmark"
                                    maxlength="50"> </textarea>
                            </div>
                        </div>
                        <hr />
                        <div class="form-row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12" style="text-align: center;">
                                <asp:Button ID="btnContinuePayment" CssClass="btn btn-primary" runat="server" OnClick="ContinuePayment" Text="Continue To Payment"
                                    OnClientClick="return validateGroupFields();" />
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
