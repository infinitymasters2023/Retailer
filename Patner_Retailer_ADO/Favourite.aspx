<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Favourite.aspx.cs" Inherits="Patner_Retailer_ADO.Favourite" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        
    <div class="card">
    <div class="card-body">
        <h4 class="filter-txt">Favourite</h4>
        <div class="row justify-content-start mb-3">
            <div class="col-12 col-lg-10">
                <div class="row">
                    <div class="col-12 col-lg-2 pr-0 mb-2 mb-lg-0">
                        <asp:TextBox ID="txtBrnad" runat="server" CssClass="form-control" placeholder="Brand Name"></asp:TextBox>
                    </div>
                    <div class="col-12 col-lg-2 pr-0 mb-2 mb-lg-0">
                        <asp:TextBox ID="txtModel" runat="server" CssClass="form-control" placeholder="Model Name"></asp:TextBox>
                    </div>
                    <div class="col-12 col-lg-2 pr-0 mb-2 mb-lg-0">
                        <asp:TextBox ID="txtPlanName" runat="server" CssClass="form-control" placeholder="Plan Name"></asp:TextBox>
                    </div>
                    <div class="col-12 col-lg-2 pr-0 mb-3 mb-lg-0">
                        <asp:TextBox ID="txtProductname" runat="server" CssClass="form-control" placeholder="Product Name"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-lg-2 pr-0">
                <div class="d-flex justify-content-start">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-success" OnClick="SubmitReport" />
                    <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btn btn-info ml-2" OnClick="btnFavourite_Click" />
                    <asp:Button ID="btnExportExcel" runat="server" Text="Export to Excel" CssClass="btn btn-info ml-2" OnClick="btnExportExcel_Click" />
                </div>
            </div>
        </div>
            <asp:GridView ID="GvFavourite" runat="server" AutoGenerateColumns="false" CssClass="table-responsive table data-table table-striped table-bordered nowrap"
                UseAccessibleHeader="true" HeaderStyle-CssClass="thead">
                <Columns>
                    <asp:TemplateField HeaderText="S.No.">
                        <ItemTemplate>
                            <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ProductSubCategory" HeaderText="Product Sub Category" />
                    <asp:BoundField DataField="Product" HeaderText="Product Type" />
                    <asp:BoundField DataField="Brand" HeaderText="Make" />
                    <asp:BoundField DataField="Model" HeaderText="Model" />
                    <asp:BoundField DataField="PlanName" HeaderText="Plan Name" />
                    <asp:BoundField DataField="PlanPrice" HeaderText="Plan Price" />
                </Columns>
            </asp:GridView>
    </div>
</div>


</asp:Content>
