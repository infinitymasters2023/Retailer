<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RetailerAddURL.aspx.cs" Inherits="Patner_Retailer_ADO.RetailerAddURL" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function showUrlModal(url) {
            document.getElementById("<%= urlToCopy.ClientID %>").value = url;
            $('#urlModal').modal('show');
        }

        function copyURL() {
            var copyText = document.getElementById("<%= urlToCopy.ClientID %>").value;
            var msg = document.getElementById("copyMsg");

            copyText.select();
            copyText.setSelectionRange(0, 99999);
            document.execCommand("copy");

            msg.classList.remove("d-none");

            setTimeout(function () {
                msg.classList.add("d-none");
            }, 3000);
        }

        function toggleInput(checkbox, inputId) {
            const inputDiv = document.getElementById(inputId);
            if (checkbox.checked) {
                inputDiv.classList.remove('d-none');
            } else {
                inputDiv.classList.add('d-none');
            }
        }
        function validateMobileNo() {
            var mobileNo = document.getElementById("<%= txtSendUrlWhatsapp.ClientID %>").value;
            if (!/^\d{0,10}$/.test(mobileNo)) {
                document.getElementById("<%= txtSendUrlWhatsapp.ClientID %>").value = mobileNo.slice(0, -1);
            }
        }

        function addEmail() {
            var inputElement = document.getElementById("<%= txtSendUrlEmail.ClientID %>");
            var label = document.getElementById("<%= lblEmailAddress.ClientID %>");
            var hidden = document.getElementById("<%= hdnEmailList.ClientID %>");
            var checkbox = document.getElementById("sendToEmail");

            var emailValue = inputElement.value.trim();
            if (emailValue !== "") {
                if (label.innerText === "") {
                    label.innerText = emailValue;
                    hidden.value = emailValue;
                } else {
                    label.innerText += ", " + emailValue;
                    hidden.value += "," + emailValue;
                }
                inputElement.value = "";
            }
            const sendBtn = document.getElementById("<%= btnSendEmailAndWhatsapp.ClientID %>");
            if (checkbox && checkbox.checked) {
                sendBtn.classList.remove("d-none");
            }
        }
        function addWhatsapp() {
            var input = document.getElementById("<%= txtSendUrlWhatsapp.ClientID %>");
            var label = document.getElementById("<%= lblWhatsappNo.ClientID %>");
            var hidden = document.getElementById("<%= hdnWhatsappList.ClientID %>");
            var checkbox = document.getElementById("sendToWhatsApp");

            var whatsappValue = input.value.trim();
            if (whatsappValue !== "") {
                if (label.innerText === "") {
                    label.innerText = whatsappValue;
                    hidden.value = whatsappValue;
                } else {
                    label.innerText += ", " + whatsappValue;
                    hidden.value += "," + whatsappValue;
                }
                input.value = "";
            }
            const sendBtn = document.getElementById("<%= btnSendEmailAndWhatsapp.ClientID %>");
            if (checkbox && checkbox.checked) {
                sendBtn.classList.remove("d-none");
            }
        }


        function ClearModel() {
            var lblWhat = document.getElementById("<%= lblWhatsappNo.ClientID %>");
            var hdnWhats = document.getElementById("<%= hdnWhatsappList.ClientID %>");
            var lblEmail = document.getElementById("<%= lblEmailAddress.ClientID %>");
            var hdnEmail = document.getElementById("<%= hdnEmailList.ClientID %>");
            const sendBtn = document.getElementById("<%= btnSendEmailAndWhatsapp.ClientID %>");
            lblWhat.innerText = "";
            hdnWhats.innerText = "";
            lblEmail.innerText = "";
            hdnEmail.innerText = "";
            sendBtn.classList.add('d-none');
        }


    </script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />

    <script>
        setTimeout(function () {
            const rows = document.querySelectorAll('#ContentPlaceHolder1_GvURL_wrapper .row');
            if (rows.length > 1) {
                rows[1].classList.add('table-responsive');
            }
        }, 500);
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="card">
        <div class="card-body">
            <h4 class="filter-txt">Generate URL</h4>
            <div class="row justify-content-start mb-3">
                <asp:Button ID="btnAddUrl" runat="server" Enabled="false" Text="Generate URL" CssClass="btn btn-info generate-url-button" OnClientClick="$('#expiryModal').modal('show'); return false;" />
            </div>
            <div>
                <asp:GridView ID="GvURL" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead" EmptyDataText="No records available. Please refine your search.">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No.">
                            <ItemTemplate>
                                <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" />
                        <asp:BoundField DataField="CreatedTime" HeaderText="Created Time" />
                        <asp:BoundField DataField="ExpiredDate" HeaderText="Expired Date" />
                        <asp:BoundField DataField="ExpiredTime" HeaderText="Expired Time" />
                        <asp:BoundField DataField="URL" HeaderText="URL" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" Text="View URL"
                                    OnClientClick='<%# "showUrlModal(\"" + Eval("URL").ToString().Replace("\"", "\\\"") + "\"); return false;" %>'
                                    CssClass="btn btn-sm btn-primary" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <div class="modal fade" id="urlModal" tabindex="-1" role="dialog" aria-labelledby="urlModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">View URL</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="ClearModel()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <input type="text" id="urlToCopy" class="form-control" runat="server" readonly />
                    <small id="copyMsg" class="text-success mt-2 d-none">URL copied!</small>

                    <hr />
                    <div class="form-check">
                        <input type="checkbox" class="form-check-input" id="sendToEmail" onchange="toggleInput(this, 'emailInput')" />
                        <label class="form-check-label" for="sendToEmail">Send to Email</label>
                    </div>
                    <div class="form-group mt-2 d-none" id="emailInput">
                        <div class="row">
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSendUrlEmail" runat="server" AutoComplete="off" CssClass="form-control text-lowercase" placeholder="Enter email address" TextMode="Email" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <button type="button" class="btn next-step text-white" onclick="addEmail()" style="margin-top: 0;">Add</button>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 ml-0">
                                <asp:Label ID="lblEmailAddress" runat="server" Font-Size="13px"></asp:Label>
                                <asp:Label ID="lblEmailAddressErrorMessage" runat="server" Font-Size="13px"></asp:Label>
                                <asp:HiddenField ID="hdnEmailList" runat="server" />
                            </div>
                        </div>
                    </div>

                    <div class="form-check mt-2">
                        <input type="checkbox" class="form-check-input" id="sendToWhatsApp" onchange="toggleInput(this, 'whatsappInput')" />
                        <label class="form-check-label" for="sendToWhatsApp">Send to WhatsApp</label>
                    </div>
                    <div class="form-group mt-2 d-none" id="whatsappInput">
                        <div class="row">
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSendUrlWhatsapp" runat="server" AutoComplete="off" CssClass="form-control" placeholder="Enter WhatsApp number"
                                    pattern="\d{10}" title="Please enter a valid 10-digit mobile number" oninput="validateMobileNo()"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <button type="button" class="btn next-step text-white" onclick="addWhatsapp()" style="margin-top: 0;">Add</button>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 ml-0">
                                <asp:Label ID="lblWhatsappNo" runat="server" Font-Size="13px"></asp:Label>
                                <asp:HiddenField ID="hdnWhatsappList" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSendEmailAndWhatsapp" runat="server" Text="Send Message" CssClass="btn next-step text-white d-none" OnClick="btnSendEmailAndWhatsapp_Click" UseSubmitBehavior="false" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal" onclick="ClearModel()">Close</button>
                    <button type="button" class="btn btn-primary" onclick="copyURL()">Copy</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="expiryModal" tabindex="-1" role="dialog" aria-labelledby="expiryModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Set Expiry Date and Time</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="expiryDate">Expiry Date</label>
                        <%--<input type="date" id="expiryDate" class="form-control" runat="server" />--%>
                        <div class="input-group">
                            <asp:TextBox ID="expiryDate" runat="server" CssClass="form-control"
                                AutoCompleteType="Disabled" AutoComplete="off"></asp:TextBox>
                            <div class="input-group-append">
                                <span class="input-group-text" style="cursor: pointer;"
                                    onclick="document.getElementById('<%= expiryDate.ClientID %>').focus();">
                                    <i class="fa fa-calendar"></i>
                                </span>
                            </div>
                        </div>
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server"
                            Format="dd-MMM-yyyy" StartDate="<%# DateTime.Today %>"
                            TargetControlID="expiryDate"></cc1:CalendarExtender>
                    </div>
                    <div class="form-group">
                        <label for="expiryTime">Expiry Time</label>
                        <input type="time" id="expiryTime" class="form-control" runat="server" />
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnConfirmExpiry" runat="server" Text="Generate URL" CssClass="btn btn-info" OnClick="btnConfirmExpiry_Click" UseSubmitBehavior="false" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
