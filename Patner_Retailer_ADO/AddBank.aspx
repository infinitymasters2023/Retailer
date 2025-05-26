<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AddBank.aspx.cs" Inherits="Patner_Retailer_ADO.AddBank" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function validateAccountNumber(input) {
            const accountNumber = input.value.trim();
            const errorLabel = document.getElementById('lblAccountNumber');

            // Check if empty
            if (accountNumber === "") {
                errorLabel.innerText = "Account Number is required.";
                errorLabel.style.display = "block";
                return false;
            }

            // Check if numeric
            if (!/^\d+$/.test(accountNumber)) {
                errorLabel.innerText = "Account Number must be numeric.";
                errorLabel.style.display = "block";
                return false;
            }

            // Check length (example: 9-18 digits)
            if (accountNumber.length < 9 || accountNumber.length > 20) {
                errorLabel.innerText = "Account Number must be between 9 and 20 digits.";
                errorLabel.style.display = "block";
                return false;
            }

            errorLabel.style.display = "none";
            return true;
        }

        function validateConfirmAccountNumber() {
            const accNumber = document.getElementById('txtAccountNumber').value.trim();
            const confirmAcc = document.getElementById('txtConfirmAccountNumber').value.trim();
            const errorLabel = document.getElementById('lblConfirmAccountNumber');

            if (confirmAcc === "") {
                errorLabel.innerText = "Confirm Account Number is required.";
                errorLabel.style.display = "block";
                return false;
            }

            if (accNumber !== confirmAcc) {
                errorLabel.innerText = "Account numbers do not match.";
                errorLabel.style.display = "block";
                return false;
            }

            errorLabel.style.display = "none";
            return true;
        }

        function validateIFSCCode(input) {
            debugger;
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
        function restrictToDigits(e) {
            e.value = e.value.replace(/\D/g, ''); // Remove all non-digit characters
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card">
        <h5 class="card-header" id="hdrtext" runat="server">Add Bank Details</h5>
        <div class="card-body">


            <div class="row">
                <div class="col-md-4">
                    <div class="form-group mb-3">
                        <label class="mb-1">Account Number <span style="color: red">*</span></label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtAccountNumber" placeholder="" MaxLength="20"
                            oninput="validateAccountNumber(this); validateConfirmAccountNumber(); restrictToDigits(this);" ClientIDMode="Static">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvAccountNumber" runat="server" ControlToValidate="txtAccountNumber"
                            ErrorMessage="Account Number is required." CssClass="text-danger" Display="Dynamic" />
                        <asp:Label ID="lblAccountNumber" runat="server" ClientIDMode="Static" Style="display: none; font-size: 12px" ForeColor="Red">Account Number is required.</asp:Label>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group mb-3">
                        <label class="mb-1">Confirm Account Number <span style="color: red">*</span></label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtConfirmAccountNumber" placeholder="" MaxLength="20"
                            oninput="validateConfirmAccountNumber();" ClientIDMode="Static">
                        </asp:TextBox>
                          <asp:RequiredFieldValidator ID="rfvConfirmAccountNumber" runat="server" ControlToValidate="txtConfirmAccountNumber"
                              ErrorMessage="Confirm Account Number is required." CssClass="text-danger" Display="Dynamic" />
                        <asp:Label ID="lblConfirmAccountNumber" runat="server" ClientIDMode="Static" Style="display: none; font-size: 12px" ForeColor="Red">Confirm Account Number is required.</asp:Label>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group mb-3">
                        <label class="mb-1">IFSC Code <span style="color: red">*</span></label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtIFSCCode" AutoPostBack="true" placeholder="" MaxLength="11"
                            OnTextChanged="txtIFSC_TextChanged" oninput="validateIFSCCode(this);" onblur="validateIFSCCode(this);">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvIFSCCode" runat="server" ControlToValidate="txtIFSCCode"
                            ErrorMessage="IFSC Code is required." CssClass="text-danger" Display="Dynamic" />
                        <asp:Label ID="lblIFSCCode" runat="server" ClientIDMode="Static" Style="display: none; font-size: 12px" ForeColor="Red">IFSC Code is required.</asp:Label>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group mb-3">
                        <label class="mb-1">Account Holder Name <span style="color: red">*</span></label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtAccountHolderName" placeholder=""></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rfvAccountHolderName" runat="server" ControlToValidate="txtAccountHolderName"
                             ErrorMessage="Account Holder Name is required." CssClass="text-danger" Display="Dynamic" />
                        <asp:Label ID="lblAccountHoldername" runat="server" Visible="false" Style="color: red; font-size: 12px">Account Holder Name is required.</asp:Label>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group mb-3">
                        <label class="mb-1">Bank Name</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtBankName" placeholder=""></asp:TextBox>

                    </div>
                </div>
                <div class="col-md-4">
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
                <div class="col-md-12">
                    <div class="form-group mb-3" style="text-align:center;">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnAddBank_Click" />
                        <asp:Button ID="btnEdit" runat="server" Text="Submit" CssClass="btn btn-primary" Visible="false" OnClick="btnEditBank_Click" />
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
