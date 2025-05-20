<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewSalesPerson.aspx.cs" Inherits="Patner_Retailer_ADO.ViewSalesPerson" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="card px-3 py-3">
            <div class="btn-group">
                <a class="btn btn-primary" href="CreateSalesPerson.aspx"><i class="fa fa-plus"></i> &nbsp  Add Sales Person</a>
            </div>
        </div>

    <div class="card">
    <div class="card-body">
        <div class="table-responsive">

            <asp:GridView ID="GvSalePerson" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered text-nowrap"
                OnRowCommand="GvSalePerson_RowCommand">
                <Columns>
                    <asp:BoundField DataField="MId" HeaderText="Profile ID" />
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

                            <!-- Delete Icon Button -->
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
