<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SalesReports.aspx.cs" Inherits="Patner_Retailer_ADO.SalesReports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid  dashboard-content">
    <div class="row">
        <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
            <div class="card">
                <div class="card-body">
                    <h4 class="filter-txt">Sales Reports</h4>
                    <div class="row justify-content-start mb-3">
                        <div class="col-12 col-lg-10">
                            <div class="row">
                                <div class="col-12 col-lg-2 pr-0">
                                    <asp:TextBox ID="txtBrnad" runat="server" CssClass="form-control" placeholder="Brand Name"></asp:TextBox>
                                </div>
                                <div class="col-12 col-lg-2 pr-0">
                                    <asp:TextBox ID="txtModel" runat="server" CssClass="form-control" placeholder="Model Name"></asp:TextBox>
                                </div>
                                <div class="col-12 col-lg-2 pr-0">
                                    <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" placeholder="Customer Name"></asp:TextBox>
                                </div>
                                <div class="col-12 col-lg-2 pr-0">
                                    <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control" placeholder="Mobile No"></asp:TextBox>
                                </div>
                                <div class="col-12 col-lg-2 pr-0">
                                    <asp:TextBox ID="txtPlanName" runat="server" CssClass="form-control" placeholder="Plan Name"></asp:TextBox>
                                </div>
                                <div class="col-12 col-lg-2 pr-0">
                                    <asp:TextBox ID="txtProductname" runat="server" CssClass="form-control" placeholder="Product Name"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-12 col-lg-2 pr-0">
                            <div class="d-flex justify-content-between gap-2">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info" style="font-size: 12px;" OnClick="SubmitReport" />
                                <asp:Button ID="btnExportExcel" runat="server" Text="Excel to Excel" CssClass="btn btn-success" OnClick="btnExportExcel_Click" style="font-size: 12px;" />
                            </div>
                        </div>
                    </div>

                    <div class="d-flex justify-content-start mb-3">
                        <asp:Label ID="ReportTotal" runat="server" Style="position: absolute; right: 50px;"></asp:Label>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="GvReport" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered text-nowrap">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Sales Person Name" />
                                <asp:BoundField DataField="emailID" HeaderText="Sales Person Email Id" />
                                <asp:BoundField DataField="MobileNo" HeaderText="Sales Person Mobile No" />
                                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" />
                                <asp:BoundField DataField="EmailIDAddress" HeaderText="Email Id" />
                                <asp:BoundField DataField="WhatsappNo" HeaderText="Whatsapp No" />
                                <asp:BoundField DataField="AddressLine1" HeaderText="Address Line 1" />
                                <asp:BoundField DataField="City" HeaderText="City" />
                                <asp:BoundField DataField="State" HeaderText="State" />
                                <asp:BoundField DataField="Pincode" HeaderText="Pincode" />
                                <asp:BoundField DataField="ProductAddressLandMark" HeaderText="Landmark" />
                                <asp:BoundField DataField="TaxableValue" HeaderText="Taxable Value" />
                                <asp:BoundField DataField="TaxAmout" HeaderText="Tax Amout" />
                                <asp:BoundField DataField="TotalAmountPay" HeaderText="Total Amount Pay" />
                                <asp:BoundField DataField="TranStatus" HeaderText="Tran Status" />
                                <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date" DataFormatString="{0:dd-MM-yyyy}" />
                                <asp:BoundField DataField="OrderId" HeaderText="Order Id" />
                                <asp:BoundField DataField="Productname" HeaderText="Product Name" />
                                <asp:BoundField DataField="Productsubcategoryname" HeaderText="Product Sub Category Name" />
                                <asp:BoundField DataField="Brand" HeaderText="Brand" />
                                <asp:BoundField DataField="ModalName" HeaderText="Modal Name" />
                                <asp:BoundField DataField="serialno" HeaderText="Serial No" />
                                <asp:BoundField DataField="DevicePurchasePrice" HeaderText="Device Purchase Price" />
                                <asp:BoundField DataField="PlanName" HeaderText="Plan Name" />
                                <asp:BoundField DataField="PlanPrice" HeaderText="Plan Price" />
                                <asp:BoundField DataField="DateofImplementation" HeaderText="Date of Implementation" DataFormatString="{0:dd-MM-yyyy}" />
                                <asp:BoundField DataField="ManufacturerWarranty_yymmdd" HeaderText="Manufacturer Warranty(yy/mm/dd)" />
                                <asp:BoundField DataField="ProductPurchaseDate" HeaderText="Product Purchase Date" DataFormatString="{0:dd-MM-yyyy}" />
                                <asp:BoundField DataField="imei" HeaderText="IMEI" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>
