<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CreateSalesPerson.aspx.cs" Inherits="Patner_Retailer_ADO.CreateSalesPerson" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="assets/css/signup.css" rel="stylesheet">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid dashboard-content">
        <div class="row">
            <div class="col-12">
                <div class="card">


                    <div class="btn-group">
                        <a class="btn btn-primary" href="ViewSalesPerson.aspx"><i class="fa fa-list"></i>&nbsp View Sales Person List</a>
                    </div>

                    <h5 class="card-header">Personal Info</h5>
                    <div class="card-body">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="text-danger" ValidationGroup="vgPersonal" />

                        <div class="row">
                            <!-- First Name -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblFirstName" runat="server" Text="First Name"></asp:Label>
                                <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" CssClass="form-control" placeholder="First name"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                                    ErrorMessage="First Name is required" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                            </div>

                            <!-- Last Name -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblLastName" runat="server" Text="Last Name"></asp:Label>
                                <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" CssClass="form-control" placeholder="Last name"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                                    ErrorMessage="Last Name is required" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                            </div>

                            <!-- Mobile Number -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblMobile" runat="server" Text="Mobile Number"></asp:Label>
                                <asp:TextBox ID="txtMobile" runat="server" MaxLength="10" CssClass="form-control"
                                    pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ControlToValidate="txtMobile"
                                    ErrorMessage="Mobile number is required" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="txtMobile"
                                    ValidationExpression="^\d{10}$" ErrorMessage="Enter valid 10-digit mobile number"
                                    CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                            </div>

                            <!-- Alternate Mobile -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblAltMobile" runat="server" Text="Alternate Mobile No."></asp:Label>
                                <asp:TextBox ID="txtAltMobile" runat="server" MaxLength="10" CssClass="form-control"
                                    pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNumber(this)"></asp:TextBox>
                            </div>

                            <!-- Email -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblEmail" runat="server" Text="Email Address"></asp:Label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email address"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="Email is required" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ErrorMessage="Enter a valid email address" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                            </div>

                            <!-- Alternate Email -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblAltEmail" runat="server" Text="Alternate Email Address"></asp:Label>
                                <asp:TextBox ID="txtAltEmail" runat="server" CssClass="form-control" placeholder="Alternate Email Address"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAltEmail"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ErrorMessage="Enter a valid email address" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                            </div>

                            <!-- DOB -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblDOB" runat="server" Text="Date of Birth"></asp:Label>
                                <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>

                            <!-- Gender -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblGender" runat="server" Text="Gender"></asp:Label>
                                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                    <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvGender" runat="server" ControlToValidate="ddlGender"
                                    InitialValue="" ErrorMessage="Select Gender" CssClass="text-danger" ValidationGroup="vgPersonal" Display="Dynamic" />
                            </div>

                            <!-- PIN -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblPIN" runat="server" Text="PIN Code"></asp:Label>
                                <asp:TextBox ID="txtPIN" runat="server" CssClass="form-control" AutoPostBack="true" MaxLength="6" OnTextChanged="txtPIN_TextChanged"
                                    pattern="\d{6}" title="Enter a 6-digit Pincode" oninput="validatePincode(this)"></asp:TextBox>
                            </div>

                            <!-- City -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblCity" runat="server" Text="City"></asp:Label>
                                <asp:TextBox ID="txtCity" runat="server" Enabled="false" CssClass="form-control" placeholder="City"></asp:TextBox>
                            </div>

                            <!-- State -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblState" runat="server" Text="State"></asp:Label>
                                <asp:TextBox ID="txtState" runat="server" CssClass="form-control" placeholder="State" MaxLength="50"></asp:TextBox>
                            </div>

                            <!-- Address -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblCommAddress" runat="server" Text="Communication Address"></asp:Label>
                                <asp:TextBox ID="txtCommAddress" runat="server" CssClass="form-control" MaxLength="200" Rows="2"></asp:TextBox>
                            </div>
                        </div>

                        <hr />
                        <h5 class="card-header">Bank Detail</h5>
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="text-danger" ValidationGroup="vgBank" />

                        <div class="row">
                            <!-- Account Number -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblAccount" runat="server" Text="Account Number"></asp:Label>
                                <asp:TextBox ID="txtAccount" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revAccountNumber" runat="server" ControlToValidate="txtAccount"
                                    ErrorMessage="Account number must be 6 to 20 digits" ValidationExpression="^\d{6,20}$" ForeColor="Red" Display="Dynamic" />
                            </div>

                            <!-- Confirm Account Number -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblConfirmAccount" runat="server" Text="Confirm Account Number"></asp:Label>
                                <asp:TextBox ID="txtConfirmAccount" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                <asp:CompareValidator ID="cvAccount" runat="server" ControlToCompare="txtAccount" ControlToValidate="txtConfirmAccount"
                                    ErrorMessage="Account numbers do not match" CssClass="text-danger" ValidationGroup="vgBank" Display="Dynamic" />
                            </div>

                            <!-- IFSC -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblIFSC" runat="server" Text="IFSC Code"></asp:Label>
                                <asp:TextBox ID="txtIFSC" runat="server" AutoPostBack="true" MaxLength="11" OnTextChanged="txtIFSC_TextChanged" CssClass="form-control"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revIFSC" runat="server" ControlToValidate="txtIFSC"
                                    ValidationExpression="^[A-Z]{4}0[A-Z0-9]{6}$"
                                    ErrorMessage="Enter a valid IFSC code" CssClass="text-danger" ValidationGroup="vgBank" Display="Dynamic" />
                            </div>

                            <!-- Account Holder Name -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblHolder" runat="server" Text="Account Holder Name"></asp:Label>
                                <asp:TextBox ID="txtHolder" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <!-- Bank Name -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblBankName" runat="server" Text="Bank Name"></asp:Label>
                                <asp:TextBox ID="txtBankName" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                            </div>

                            <!-- Branch Name -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblBranch" runat="server" Enabled="false" Text="Branch Name"></asp:Label>
                                <asp:TextBox ID="txtBranch" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                            </div>

                            <!-- Branch Address -->
                            <div class="col-md-4 mb-2">
                                <asp:Label ID="lblBranchAddress" runat="server" Text="Branch Address"></asp:Label>
                                <asp:TextBox ID="txtBranchAddress" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <asp:Button ID="btnSubmit" runat="server" Text="Submit"
                            CssClass="btn btn-primary mt-3"
                            OnClick="btnSubmit_Click"
                            ValidationGroup="vgPersonal" />

                          <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary mt-3" OnClick="btnUpdate_Click"
                              ValidationGroup="vgPersonal" />

                        <hr />

                        <div id="DocumentPanel" class="row" runat="server">
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
                                    <asp:GridView runat="server" ID="gvDocuments" CssClass="table table-striped table-bordered table-responsive" AutoGenerateColumns="false"
                                        OnRowCommand="gvDocuments_RowCommand">
                                        <Columns>
                                            <asp:BoundField HeaderText="Sr.No." DataField="SrNo" />
                                            <asp:BoundField HeaderText="DocId" DataField="DocId" Visible="false" />
                                            <asp:BoundField HeaderText="Document Name" DataField="DocumentName" />
                                            <asp:BoundField HeaderText="Document Number" DataField="DocumentNumber" />
                                            <asp:BoundField HeaderText="Document Path" DataField="DocumentPath" Visible="false" />
                                            <asp:BoundField HeaderText="Status" DataField="Status" />
                                            <asp:BoundField HeaderText="Size" DataField="Size" />
                                            <asp:TemplateField HeaderText="Actions">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditDoc" CommandArgument='<%# Eval("MId") %>'
                                                        CssClass="btn btn-sm btn-primary">
                                                        <i class="fa fa-edit"></i>
                                                    </asp:LinkButton>

                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteRow" CommandArgument='<%# Eval("MId") %>'
                                                        CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('Are you sure you want to delete this Document?');">
                                                        <i class="fa fa-trash"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <ul class="list-inline pull-right">
                            <asp:Button ID="btnnext3" runat="server" Text="Submit" CssClass="default-btn next-step" OnClick="btnnext3_Click" />
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

