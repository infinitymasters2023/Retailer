<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RetailerAddURL.aspx.cs" Inherits="Patner_Retailer_ADO.RetailerAddURL" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function showUrlModal(url) {
            document.getElementById('urlToCopy').value = url;
            $('#urlModal').modal('show');
        }

        function copyURL() {
            var copyText = document.getElementById("urlToCopy");
            var msg = document.getElementById("copyMsg");

            copyText.select();
            copyText.setSelectionRange(0, 99999);
            document.execCommand("copy");

            msg.classList.remove("d-none");

            setTimeout(function () {
                msg.classList.add("d-none");
            }, 3000);
        }
    </script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />

    
</asp:Content>
   
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="card">
        <div class="card-body">
            <h4 class="filter-txt">Generate URL</h4>
            <div class="row justify-content-start mb-3">
                <asp:Button ID="btnAddUrl" runat="server" Text="Generate URL" CssClass="btn btn-info generate-url-button" OnClientClick="$('#expiryModal').modal('show'); return false;" />
            </div>
            <asp:GridView ID="GvURL" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead"  EmptyDataText="No records available. Please refine your search.">
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
    <div class="modal fade" id="urlModal" tabindex="-1" role="dialog" aria-labelledby="urlModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">View URL</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <input type="text" id="urlToCopy" class="form-control" readonly />
                    <small id="copyMsg" class="text-success mt-2 d-none">URL copied!</small>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
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
                                   AutoCompleteType="Disabled" AutoComplete="off" ></asp:TextBox>
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
                       <%-- <div class="input-group">
                            <asp:TextBox ID="expiryTime" runat="server" CssClass="form-control"
                                AutoCompleteType="Disabled" AutoComplete="off"></asp:TextBox>
                            <div class="input-group-append">
                                <span class="input-group-text" style="cursor: pointer;"
                                    onclick="document.getElementById('<%= expiryTime.ClientID %>').focus();">
                                    <i class="fa fa-clock"></i>
                                </span>
                            </div>
                        </div>--%>
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
