<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ProductInformation.aspx.cs" Inherits="Patner_Retailer_ADO.ProductInformation" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .headerText {
            font-size: 16px;
            font-weight: 600 !important;
            color: black;
        }

        .product-list-image {
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
            align-items: center;
            margin-bottom: 15px;
            border-bottom: 1px solid #d9d9d9;
            padding-bottom: 25px;
        }

        .custom-card-box {
            min-height: 180px;
            border-radius: 10px;
            box-shadow: 0 0 2px #e0e0e0;
            /*box-shadow: 0 0 5px #000;*/
            text-align: center;
            width: 200px;
            cursor: pointer;
            /*border:1px solid #ccc;*/
        }

        .custom-select-with-caret img {
            margin-bottom: 10px;
        }
        .custom-card-box label{
            line-height: 20px;
        }
        
        @media (max-width:480px){
            .custom-card-box{
                width:100%;
            }
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card">
        <h5 class="card-header" style="display: flex; justify-content: space-between;">Sell InfyShield Plans</h5>
        <div class="card-body">
            <asp:Repeater ID="rptCategory" runat="server">
                <ItemTemplate>
                    <div class="row">
                        <h2 class="headerText pl-3 mb-3 pt-0" style="font-weight: 400"><%# Eval("Category") %></h2>
                    </div>
                    <div class="product-list-image">
                        <asp:Repeater ID="rptProducts" runat="server" DataSource='<%# Eval("Products") %>'>
                                <itemtemplate>
                            <asp:LinkButton ID="lnkProduct" runat="server" CssClass="custom-card-box" CommandName="SelectProduct" CommandArgument='<%# Eval("ProductTypeID") %>' OnCommand="AddProduct_Click">
                                    <div class="custom-card-box" runat="server">
                                        <div class="custom-select-with-caret">
                                            <img class="mt-1" alt="Product-Image" src='<%# string.IsNullOrEmpty(Eval("product_imageUrl")?.ToString()) ? "/assets/images/Infyshield-logo.png" : Eval("product_imageUrl").ToString() %>' style="height: 125px;max-width:200px;border-radius:5px;" />
                                        </div>
                                        <label><%# Eval("ProductType") %></label>
                                    </div>
                            </asp:LinkButton>
                                    <asp:HiddenField ID="hdnProductTypeName" runat="server" Value='<%# Eval("ProductType") %>' />
                                </itemtemplate>
                        </asp:Repeater>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>

