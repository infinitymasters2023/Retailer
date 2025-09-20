<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewSalesPerson.aspx.cs" Inherits="Patner_Retailer_ADO.ViewSalesPerson" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        setTimeout(function () {
            const rows = document.querySelectorAll('#ContentPlaceHolder1_GvSalePerson_wrapper .row');
            if (rows.length > 1) {
                rows[1].classList.add('table-responsive');
            }
        }, 500);
    </script>
    <style>
        #<%= GvSalePerson.ClientID %> td {
             text-wrap-mode: nowrap;
            word-wrap: break-word;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card px-4 pt-3">
        <div class="btn-group">
            <a class="btn btn-primary" href="CreateSalesPerson.aspx"><i class="fa fa-plus"></i>&nbsp  Add Sales Executive</a>
        </div>
    </div>
    <div class="card">
        <div class="card-body">
            <div>

                <asp:GridView ID="GvSalePerson" runat="server" AutoGenerateColumns="false" OnRowCommand="GvSalePerson_RowCommand" EmptyDataText="No records available. Please refine your search.">
                    <Columns>
                        <asp:TemplateField HeaderText="Profile ID">
                            <ItemTemplate>
                                <%# "Infy-2025-" + Eval("Mid") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Change Status">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkChangeStatus" runat="server"
                                    CommandName="ChangeStatusRow"
                                    CommandArgument='<%# Eval("MId") + "|" + Eval("ProfileStatus") %>'
                                    CssClass="p-0 border-0 bg-transparent"
                                    OnClientClick='<%# Eval("AdminStatus").ToString() == "Approved" ? "return confirm(\"Are you sure you want to toggle the status of this profile?\");" : "return false;" %>'
                                    ToolTip='<%# Eval("AdminStatus").ToString() == "Approved" ? "Toggle Status" : "Admin approval is pending" %>'
                                    Enabled='<%# Eval("AdminStatus").ToString() == "Approved" %>'
                                    CausesValidation="false"
                                    UseSubmitBehavior="false">
                                    <i class='fa <%# Eval("ProfileStatus").ToString() == "Approved" ? "fa-toggle-on text-success" : "fa-toggle-off text-danger" %>' style="font-size: 1.7rem;"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" />
                        <asp:BoundField DataField="WhatsappMobileNo" HeaderText="WhatsApp No" />
                        <asp:BoundField DataField="EmailID" HeaderText="Email ID" />
                        <asp:BoundField DataField="DateOfBirth" HeaderText="DOB" DataFormatString="{0:dd-MM-yyyy}" />
                        <asp:BoundField DataField="Gender" HeaderText="Gender" />
                        <asp:BoundField DataField="Address" HeaderText="Address" />
                        <asp:BoundField DataField="Pincode" HeaderText="Pincode" />
                        <asp:BoundField DataField="City" HeaderText="City" />
                        <asp:BoundField DataField="State" HeaderText="State" />
                        <asp:BoundField DataField="ProfileStatus" HeaderText="Profile Status" />
                        <asp:BoundField DataField="AdminStatus" HeaderText="Admin Status" />
                        <asp:BoundField DataField="ProfileCreatedDate" HeaderText="Created Date" DataFormatString="{0:dd-MM-yyyy}" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <%# Eval("AdminStatus").ToString() != "Approved"
                                        ? $"<a href='CreateSalesPerson.aspx?Mid={Eval("MId")}' class='text-primary mr-2' title='Edit'><i class='fa fa-edit'></i></a>"
                                        : "<span class='text-muted mr-2' title='Editing not allowed'><i class='fa fa-edit' style='opacity:0.4; cursor: not-allowed;'></i></span>"
                                        %>
                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteRow" CommandArgument='<%# Eval("MId") %>'
                                    CssClass="text-danger" OnClientClick="return confirm('Are you sure you want to delete this profile?');">
                               <i class="fa fa-trash"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
