<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AddBank.aspx.cs" Inherits="Patner_Retailer_ADO.AddBank" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .product-colors input[type=checkbox], input[type=radio] {
            display: inline;
        }

        .JointAccountpnl tbody {
            display: flex;
            gap: 20px;
        }
        .prev-step, .next-step{
            margin-top:0px;
        }
        .prev-step:hover{
            margin-top:0px;
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
    .Error-Message{
    font-size:12px !important;
    color:red;
    display:none;
    font-weight:400 !important;
    margin-bottom:0px;
}
    </style>
    
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
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtAccountNumber" placeholder="" MaxLength="20" AutoComplete="off"
                            oninput="validateAccountNumber(this); validateConfirmAccountNumber(); restrictToDigits(this);" ClientIDMode="Static"
                            onpaste="return false;" oncopy="return false;" oncut="return false;" AutoPostBack="true" OnTextChanged="AccountNumberChange">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvAccountNumber" runat="server" ControlToValidate="txtAccountNumber"
                            ErrorMessage="Account Number is required." CssClass="text-danger" Display="Dynamic" />
                        <asp:Label ID="lblAccountNumber" runat="server" ClientIDMode="Static" Style="display: none; font-size: 12px" ForeColor="Red">Account Number is required.</asp:Label>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group mb-3">
                        <label class="mb-1">Confirm Account Number <span style="color: red">*</span></label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtConfirmAccountNumber" placeholder="" MaxLength="20" AutoComplete="off"
                            oninput="validateConfirmAccountNumber();" ClientIDMode="Static" onpaste="return false;" oncopy="return false;" oncut="return false;">
                        </asp:TextBox>
                          <asp:RequiredFieldValidator ID="rfvConfirmAccountNumber" runat="server" ControlToValidate="txtConfirmAccountNumber"
                              ErrorMessage="Confirm Account Number is required." CssClass="text-danger" Display="Dynamic" />
                        <asp:Label ID="lblConfirmAccountNumber" runat="server" ClientIDMode="Static" Style="display: none; font-size: 12px" ForeColor="Red">Confirm Account Number is required.</asp:Label>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group mb-3">
                        <label class="mb-1">IFSC Code <span style="color: red">*</span></label>
                        <asp:TextBox runat="server" CssClass="form-control text-uppercase" ID="txtIFSCCode" AutoPostBack="true" placeholder="" MaxLength="11" AutoComplete="off"
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
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtAccountHolderName" placeholder="" AutoComplete="off"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rfvAccountHolderName" runat="server" ControlToValidate="txtAccountHolderName"
                             ErrorMessage="Account Holder Name is required." CssClass="text-danger" Display="Dynamic" />
                        <asp:Label ID="lblAccountHoldername" runat="server" Visible="false" Style="color: red; font-size: 12px">Account Holder Name is required.</asp:Label>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group mb-3">
                        <label class="mb-1">Bank Name</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtBankName" placeholder="" AutoComplete="off"></asp:TextBox>

                    </div>
                </div>
                <div class="col-md-4">
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
                        <asp:Label ID="lblsupportingDocName" CssClass="lblDocumentName" runat="server"></asp:Label>
                        <asp:Label ID="lblSupportingDocumentError" CssClass="Error-Message" runat="server"></asp:Label>
                    </div>
                    <p class="text-danger mt-2 mb-2" style="font-size: 13px;"><strong>Note<sup>*</sup></strong> jpg, jpeg, png and pdf format is acceptable.</p>
                </div>
                <div class="col-md-12">
                    <div class="form-group mb-3 d-flex justify-content-center" style="text-align:center;">
                        <a href="Profile.aspx?qu=Bank" class="default-btn prev-step bg-dark"><i class="fa fa-angle-double-left font14"></i>&nbsp; Cancel</a>
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn next-step ml-2" OnClick="btnAddBank_Click" />
                        <asp:Button ID="btnEdit" runat="server" Text="Submit" CssClass="btn next-step ml-2" Visible="false" OnClick="btnEditBank_Click" />
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
