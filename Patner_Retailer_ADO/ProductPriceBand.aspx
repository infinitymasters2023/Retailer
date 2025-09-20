<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ProductPriceBand.aspx.cs" Inherits="Patner_Retailer_ADO.ProductPriceBand" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table thead th, .table th {
            background: #4397a7 !important;
            color:white;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card">
        <div class="card-header">
            <div class="d-flex justify-content-between">
                <h5 class="mb-0">Product Price Band</h5>
            </div>
        </div>
        <div class="card-body">
            <div class="mt-3">
                <div class="table-responsive">
                    <asp:GridView ID="gvPlans" runat="server" AutoGenerateColumns="False"
                        OnRowCreated="gvPlans_RowCreated" OnRowDataBound="gvPlans_RowDataBound"
                        CssClass="table table-bordered" ShowHeader="false">
                        <columns>
                            <asp:BoundField DataField="ProductType" HeaderText="Product Type" />
                            <asp:BoundField DataField="PriceBand" HeaderText="Price Band" />
                            <asp:BoundField DataField="MRP1Y" HeaderText="MRP" />
                            <asp:BoundField DataField="Disc1Y" HeaderText="Discounted Price" />
                            <asp:BoundField DataField="MRP2Y" HeaderText="MRP" />
                            <asp:BoundField DataField="Disc2Y" HeaderText="Discounted Price" />
                            <asp:BoundField DataField="MRP3Y" HeaderText="MRP" />
                            <asp:BoundField DataField="Disc3Y" HeaderText="Discounted Price" />
                            <asp:BoundField DataField="MRP4Y" HeaderText="MRP" />
                            <asp:BoundField DataField="Disc4Y" HeaderText="Discounted Price" />
                        </columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
