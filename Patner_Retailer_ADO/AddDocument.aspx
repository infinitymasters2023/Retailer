<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AddDocument.aspx.cs" Inherits="Patner_Retailer_ADO.AddDocument" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .prev-step, .next-step {
            margin-top: 0px;
        }

            .prev-step:hover {
                margin-top: 0px;
            }

        .thead {
            background: #4397a7 !important;
            color: #fff !important;
        }

            .thead th {
                background: #4397a7 !important;
                color: #fff !important;
            }

        .dataTables_length {
            display: none;
        }

        .dataTables_filter {
            display: none;
        }

        .dataTables_paginate {
            display: none;
        }

        .dataTables_info {
            display: none;
        }

        .data-table {
            min-width: 100% !important;
        }
    </style>

    <%-- <script>
      document.addEventListener("DOMContentLoaded", function () {
          var txtDoc = document.getElementById('<%= txtDocumentNumber.ClientID %>');
          var hdnDoc = document.getElementById('<%= hdnDocumentNumber.ClientID %>');

          txtDoc.addEventListener('input', function () {
              hdnDoc.value = txtDoc.value;
          });

          // Mask on blur
          txtDoc.addEventListener('blur', function () {
              var val = txtDoc.value;
              if (val.length > 4) {
                  hdnDoc.value = val;
                  var last4 = val.slice(-4);
                  var masked = '*'.repeat(val.length - 4) + last4;
                  txtDoc.value = masked;
              }
          });
          txtDoc.addEventListener('focus', function () {
              if (hdnDoc.value) {
                  txtDoc.value = hdnDoc.value;
              }
          });
          var btnSubmit = document.getElementById('<%= btnSubmit.ClientID %>');
          if (btnSubmit) {
              btnSubmit.addEventListener('click', function () {
                  txtDoc.value = hdnDoc.value;
              });
          }

          var btnEdit = document.getElementById('<%= btnEdit.ClientID %>');
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
    <script>
        setTimeout(function () {
            const rows = document.querySelectorAll('#ContentPlaceHolder1_gvDocuments_wrapper .row');
            if (rows.length > 1) {
                rows[1].classList.add('table-responsive');
            }
        }, 500);
    </script>

    <script type="text/javascript">
        let selectedDocType = "";

        function handleDocumentFormat(value) {
            selectedDocType = value;
            const input = document.getElementById("<%= txtDocumentNumber.ClientID %>");
         var hdnDoc = document.getElementById('<%= hdnDocumentNumber.ClientID %>');
         const errorLabel = document.getElementById("<%= lblDocFormatError.ClientID %>");
            input.value = "";
            hdnDoc.value = "";
            errorLabel.style.display = "none";
            errorLabel.innerText = "";
            const errorAdharLabel = document.getElementById("lblAdhaarDocumentNumber");
            errorAdharLabel.style.display = "none";
            if (selectedDocType === "13" || selectedDocType === "57" || selectedDocType === "58") {
                errorAdharLabel.style.display = "block";
            }
            if (selectedDocType === "278") {
                docinput.style.display = "none";
                doclbl.style.display = "none";
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
            input.maxLength = 15;

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
        }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card">
        <h5 class="card-header" id="hdrtext" runat="server">Add Document</h5>
        <div class="card-body">
            <div class="row">
                <div class="col-md-4 mb-0">
                    <div class="form-group">
                        <label class="mb-1">Document Name <span style="color: red;">*</span></label>
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlDocumentName" AutoPostBack="false" onchange="handleDocumentFormat(this.value); toggleDocumentNumberValidator(this.value)">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvDocumentName" runat="server" ControlToValidate="ddlDocumentName"
                            InitialValue="" ErrorMessage="Document Name is required." CssClass="text-danger" Display="Dynamic" SetFocusOnError="true" />
                    </div>
                </div>
                <div class="col-md-4 mb-0">
                    <div class="form-group">
                        <label class="mb-1">Document Number</label>
                        <asp:TextBox runat="server" CssClass="form-control text-uppercase" ID="txtDocumentNumber" MaxLength="20" oninput="validateDocumentNumber()" placeholder="" AutoComplete="off"></asp:TextBox>
                        <asp:HiddenField ID="hdnDocumentNumber" runat="server" />
                        <p class="text-danger mt-2 mb-0" id="lblAdhaarDocumentNumber" style="font-size: 13px; display: none;"><strong>Note<sup>*</sup></strong> Please enter the last 4 digits of your Aadhar.</p>
                    </div>
                </div>
                <div class="col-md-4 mb-0">
                    <div class="form-group">
                        <label class="mb-1">File<span style="color: red;">*</span></label>
                        <asp:FileUpload runat="server" ID="fuFrontSide" CssClass="form-control" accept=".jpg,.jpeg,.png,.pdf" />
                        <asp:Label ID="lblDocument" runat="server" Text="Document is required." ForeColor="Red" Visible="false"></asp:Label>
                        <p class="text-danger mt-2"><strong>Note<sup>*</sup></strong> jpg, jpeg, png and pdf format is acceptable.</p>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="col-md-12">
                        <asp:Label ID="lblDocFormatError" runat="server" CssClass="alert alert-danger" Style="display: none;"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group mt-4" style="text-align: center;">
                            <asp:Button runat="server" ID="btnUploadFront" Text="Upload" CssClass="btn btn-primary" OnClick="btnUploadFront_Click" />
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="form-group mb-3 d-flex justify-content-center" style="text-align: center;">
                        <a href="Profile.aspx?qu=Document" class="default-btn prev-step bg-dark"><i class="fa fa-angle-double-left font14"></i>&nbsp; Cancel</a>
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn next-step ml-2" OnClick="btnAddDocument_Click" />
                        <asp:Button ID="btnEdit" runat="server" Text="Submit" CssClass="btn next-step ml-2" Visible="false" OnClick="btnEditDocument_Click" />
                    </div>
                </div>

                <div class="col-md-12">
                    <div class="DocumentData">
                        <asp:GridView runat="server" ID="gvDocuments" CssClass="data-table table-striped table-bordered nowrap"
                            AutoGenerateColumns="false" GridLines="None" HeaderStyle-CssClass="thead">
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
        </div>
    </div>
</asp:Content>
