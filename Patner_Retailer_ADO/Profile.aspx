<%@ Page Title="Retailer Profile" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Patner_Retailer_ADO.Profile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

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

    <style>
        /* Custom CSS for the profile page */
        .profile-panel .row .col-md-3 {
            margin-bottom: 0.75rem; /* Add some vertical spacing between label-value pairs */
        }

        .profile-panel .row .col-md-3 label {
            font-weight: bold; /* Make labels stand out */
            display: block; /* Ensure label takes full width of its column */
            margin-bottom: 0.25rem; /* Add a little space between label and value */
            color: #495057; /* Slightly darker text for labels */
        }

        .profile-panel .row .col-md-3 span {
            display: block; /* Display value on the next line */
            color: #212529; /* Standard text color for values */
        }

        /* Adjust margin-top for the first heading (Retailer Profile Details) if needed */
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
            text-align: left; /* Align text to the left for better readability */
        }

        .profile-panel .table thead th {
            vertical-align: bottom;
            border-bottom: 2px solid #dee2e6;
            background-color: #343a40; /* Dark background for header */
            color: white;
        }

        .profile-panel .table tbody tr:nth-of-type(odd) {
            background-color: rgba(0, 0, 0, 0.05); /* Add subtle background for odd rows */
        }

        .profile-panel .table-sm th,
        .profile-panel .table-sm td {
            padding: 0.5rem; /* Reduce padding for a more compact table */
        }

        .profile-panel .card {
            border: 1px solid rgba(0, 0, 0, 0.125);
            border-radius: 0.25rem;
            margin-bottom: 1.5rem; /* Add some space below the profile card */
        }

        .profile-panel .card-header {
            background-color: #f8f9fa; /* Light background for header */
            padding: 0.75rem 1.25rem;
            margin-bottom: 0;
            border-bottom: 1px solid rgba(0, 0, 0, 0.125);
        }

        .profile-panel .card-body {
            padding: 1.25rem;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlProfile" runat="server" CssClass="profile-panel">
        <div class="container-fluid px-0">
            <div class="card">
                <h5 class="card-header mt-0 text-dark">Retailer Profile Details</h5>
                <div class="card-body">

                    <div class="row">
                        <div class="col-md-3">
                            <div style="display: flex;">
                                <asp:Label ID="lblName" runat="server" Text="Name: " />&nbsp;
                                <asp:Label ID="lblNameValue" runat="server" />
                            </div>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtFirstName" placeholder="" MaxLength="30" Visible="false" />
                            <label id="lblFullName" runat="server" visible="false" style="color: red;font-size:12px">First Name is required.</label>
                        </div>
                        <div class="col-md-3">
                            <div style="display: flex;">
                                <asp:Label ID="lblMobileNo" runat="server" Text="Mobile No: " />&nbsp;
                                <asp:Label ID="lblMobileNoValue" runat="server" />
                            </div>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtMobileNumber" placeholder="" MaxLength="10" Visible="false"
                                pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" Enabled="false" />
                            <label id="lblErrorMobileNo" runat="server" visible="false" style="color: red;font-size:12px">Mobile No is required.</label>
                        </div>
                        <div class="col-md-3">
                            <div style="display: flex;">
                                <asp:Label ID="lblWhatsappNo" runat="server" Text="WhatsApp No: " />&nbsp;
                                <asp:Label ID="lblWhatsappNoValue" runat="server" />
                            </div>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtAlternateMobile" placeholder="" MaxLength="10" Visible="false"
                                pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)" />
                        </div>
                        <div class="col-md-3">
                            <div style="display: flex;">
                                <asp:Label ID="lblEmail" runat="server" Text="Email: " />&nbsp;
                                <asp:Label ID="lblEmailValue" runat="server" />
                            </div>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtEmail" TextMode="Email" placeholder="" MaxLength="50" Visible="false" />
                            <label id="lblEmailAddress" runat="server" visible="false" style="color:red; font-size:12px">Email Id is required.</label>
                        </div>
                        <div class="col-md-3">
                            <div style="display: flex;">
                                <asp:Label ID="lblDOB" runat="server" Text="Date of Birth: " />&nbsp;
                                <asp:Label ID="lblDOBValue" runat="server" />
                            </div>
                            <asp:TextBox runat="server" CssClass="form-control" ID="TextBox1" placeholder="" Visible="false"
                                AutoPostBack="true" AutoCompleteType="Disabled" AutoComplete="off" />
                            <div class="input-group-append" runat="server" Visible="false">
                                <span class="input-group-text" style="cursor: pointer;"
                                    onclick="document.getElementById('<%= TextBox1.ClientID %>').focus();">
                                    <i class="fa fa-calendar"></i>
                                </span>
                            </div>
                            <label id="lblDateOfBirth" runat="server" visible="false" style="color: red; font-size: 12px">Date of Birth is required.</label>

                        </div>
                        <div class="col-md-3">
                            <div style="display: flex;">
                                <asp:Label ID="lblGender" runat="server" Text="Gender: " />&nbsp;
                                <asp:Label ID="lblGenderValue" runat="server" />
                            </div>
                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control" Visible="false">
                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <div style="display: flex;">
                                <asp:Label ID="lblPincode" runat="server" Text="Pin Code: " />&nbsp;
                                <asp:Label ID="lblPincodeValue" runat="server" />
                            </div>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtPinCode" AutoPostBack="true" OnTextChanged="txtPinCode_TextChanged" placeholder=""
                                MaxLength="6" title="Enter a 6-digit Pincode" oninput="validatePincode(this)" Visible="false" />
                            <label id="lblErrorPincode" runat="server" visible="false" style="color: red; font-size:12px">PIN Code is required.</label>
                        </div>
                        <div class="col-md-2">
                            <div style="display: flex; margin-top:28px;">
                                <asp:Label ID="lblCity" runat="server" Text="City: " />&nbsp;
                                <asp:Label ID="lblCityValue" runat="server" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div style="display: flex;margin-top:28px;">
                                <asp:Label ID="lblState" runat="server" Text="State: " />&nbsp;
                                <asp:Label ID="lblStateValue" runat="server" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div style="display: flex;">
                                <asp:Label ID="lblAddress" runat="server" Text="Address: " />&nbsp;
                                <asp:Label ID="lblAddressValue" runat="server" />
                            </div>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtAddress" TextMode="MultiLine" Rows="3" placeholder="" MaxLength="150" Visible="false" />
                            <label id="lblCurrentAddress" runat="server" visible="false" style="color:red; font-size:12px">Address is required.</label>
                        </div>
                    </div>
                    <div class="row mt-3">
                        <div class="w-100 d-flex justify-content-center">
                            <asp:Button ID="btnEditProfile" runat="server" Text="Edit Profile" CssClass="btn btn-primary" OnClick="btnEditProfile_Click" />
                            <asp:Button ID="btnCancelProfile" runat="server" Text="Cancel" CssClass="btn btn-danger mr-3" Visible="false" OnClick="btnCancelProfile_Click" />
                            <asp:Button ID="btnUpdateProfile" runat="server" Text="Update Profile" CssClass="btn btn-primary" Visible="false" OnClick="btnUpdateProfile_Click" />
                        </div>
                    </div>
                </div>

            </div>
           
            <div class="row">
                <!-- ============================================================== -->
                <!-- fixed header  -->
                <!-- ============================================================== -->
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <div class="card">
                        <div class="card-header">
                            <div class="d-flex justify-content-between">
                                <h5 class="mb-0">Bank Details </h5>
                                <div class="col-md-1">
                                    <asp:Button ID="btnAddBank" runat="server" class="btn btn-primary" Text="Add Bank" OnClick="btnAddBank_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:Repeater ID="RepeaterBankDetails" runat="server" OnItemCommand="RepeaterBankDetails_ItemCommand">
                                    <HeaderTemplate>
                                        <table id="example44" class="table-responsive table data-table table-striped table-bordered nowrap" style="width: 100%">
                                            <thead>
                                                <tr>
                                                    <th>S.No.</th>
                                                    <th>Make Primary</th>
                                                    <th>Bank Account Number</th>
                                                    <th>IFSC Code</th>
                                                    <th>Bank Name</th>
                                                    <th>Bank Branch</th>
                                                    <th>Branch Address</th>
                                                    <th>Account Holder Name</th>
                                                    <th>Created Date</th>
                                                    <th>IP Address</th>
                                                    <th>Status</th>
                                                    <th>Action</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class='<%# Eval("Status").ToString() == "Active" ? "table-success" : "" %>'>
                                            <td><%# Container.ItemIndex + 1 %></td>
                                            <td>
                                                <asp:LinkButton ID="lnkMakeActive" runat="server" CommandName="MakeActive" CommandArgument='<%# Eval("Mid") %>'
                                                    ToolTip="Make Active" CssClass="btn btn-sm btn-success mr-2" OnClientClick="return confirm('Set this as the active account?');">
                                                    <i class="fa fa-check"></i>
                                                </asp:LinkButton>
                                            </td>
                                            <td><%# Eval("BankAccountNumber") %></td>
                                            <td><%# Eval("IFSCCode") %></td>
                                            <td><%# Eval("BankName") %></td>
                                            <td><%# Eval("BankBranch") %></td>
                                            <td><%# Eval("BankBranchAddress") %></td>
                                            <td><%# Eval("AccountHolderName") %></td>
                                            <td><%# Eval("CreatedDate", "{0:dd-MMM-yyyy}") %></td>
                                            <td><%# Eval("IPAddress") %></td>
                                            <td><asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' Visible="false"></asp:Label><%# Eval("Status") %></td>
                                            <td>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditBank" CommandArgument='<%# Eval("Mid") %>'
                                                    ToolTip="Edit" CssClass="btn btn-sm btn-warning mr-2">
                                                <i class="fa fa-edit"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteBank" CommandArgument='<%# Eval("Mid") %>'
                                                    ToolTip="Delete" CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('Are you sure you want to delete this record?');">
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
                        </div>
                    </div>
                </div>
                <!-- ============================================================== -->
                <!-- end fixed header  -->
                <!-- ============================================================== -->
            </div>


            <div class="row">
                <!-- ============================================================== -->
                <!-- fixed header  -->
                <!-- ============================================================== -->
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
                                                <thead>
                                                    <tr>
                                                        <th>S.No.</th>
                                                        <th>Firm Name</th>
                                                        <th>GSTIN</th>
                                                        <th>Address Line 1</th>
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
                                                    ToolTip="Edit" CssClass="btn btn-sm btn-warning">
                                                    <i class="fa fa-edit"></i>
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
                <!-- ============================================================== -->
                <!-- end fixed header  -->
                <!-- ============================================================== -->
            </div>
            <div class="row">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <div class="card">
                        <div class="card-header">
                            <div class="d-flex justify-content-between">
                                <h5 class="mb-0">Uploaded Documents</h5>
                                <div class="col-md-1">
                                    <asp:Button ID="btnUploadDocument" runat="server" class="btn btn-primary" Text="Add Documents" OnClick="btnUploadDocument_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:Repeater ID="rptDocuments" runat="server" OnItemCommand="RepeaterDocumentDetails_ItemCommand">
                                    <HeaderTemplate>
                                        <div class="table-responsive">
                                        <table class="table data-table table-striped table-bordered nowrap">
                                            <thead class="thead-dark">
                                                <tr>
                                                    <th>S.No.</th>
                                                    <th>Document Number</th>
                                                    <th>Document Path</th>
                                                    <th>Remarks</th>
                                                    <th>Status</th>
                                                    <th>Action By</th>
                                                    <th>Action Date</th>
                                                    <th>Action</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Container.ItemIndex + 1 %></td>
                                            <td><%# Eval("documentNumber") %></td>
                                            <td><%# Eval("DocumentPath") %></td>
                                            <td><%# Eval("Remarks") %></td>
                                            <td><%# Eval("Status") %></td>
                                            <td><%# Eval("ActionBy") %></td>
                                            <td><%# Eval("ActionDate") %></td>
                                            <td>
                                                <asp:LinkButton ID="lnkEditDoc" runat="server" CommandName="Editdoc" CommandArgument='<%# Eval("Mid") %>'
                                                    ToolTip="Edit" CssClass="btn btn-sm btn-warning">
                                                    <i class="fa fa-edit"></i>
                                                </asp:LinkButton>
                                                &nbsp;
                                                <asp:LinkButton ID="lnkDeleteDoc" runat="server" CommandName="Deletedoc" CommandArgument='<%# Eval("Mid") %>'
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
            </div>
        </div>

    </asp:Panel>
</asp:Content>
