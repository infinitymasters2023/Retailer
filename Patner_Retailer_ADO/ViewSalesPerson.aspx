<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewSalesPerson.aspx.cs" Inherits="Patner_Retailer_ADO.ViewSalesPerson" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card px-3 py-3">
        <div class="btn-group">
            <a class="btn btn-primary" href="CreateSalesPerson.aspx"><i class="fa fa-plus"></i>&nbsp  Add Sales Person</a>
        </div>
    </div>
    <div class="card">
        <div class="card-body">
            <div>

                <asp:GridView ID="GvSalePerson" runat="server" AutoGenerateColumns="false" OnRowCommand="GvSalePerson_RowCommand"  EmptyDataText="No records available. Please refine your search.">
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
                                    OnClientClick="return confirm('Are you sure you want to toggle the status of this profile?');"
                                    ToolTip="Toggle Status">
            <i class='fa <%# Eval("ProfileStatus").ToString() == "Active" ? "fa-toggle-on text-success" : "fa-toggle-off text-danger" %>' style="font-size: 1.7rem;"></i>
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
                        <asp:BoundField DataField="Country" HeaderText="Country" />
                        <asp:BoundField DataField="PANNo" HeaderText="PAN No" />
                        <asp:BoundField DataField="AdhaarNo" HeaderText="Aadhaar No" />
                        <asp:BoundField DataField="ProfileStatus" HeaderText="Profile Status" />
                        <asp:BoundField DataField="ProfileCreatedDate" HeaderText="Created Date" DataFormatString="{0:dd-MM-yyyy}" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <a href='CreateSalesPerson.aspx?Mid=<%# Eval("MId") %>' class="text-primary mr-2" title="Edit">
                                    <i class="fa fa-edit"></i>
                                </a>
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
