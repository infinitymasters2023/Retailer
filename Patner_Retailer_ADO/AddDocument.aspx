<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AddDocument.aspx.cs" Inherits="Patner_Retailer_ADO.AddDocument" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card">
        <h5 class="card-header" id="hdrtext" runat="server">Add Dealer</h5>
        <div class="card-body">
            <div class="row">
                <div class="col-md-6 mb-3">
                    <div class="form-group">
                        <label class="mb-1">Document Name <span style="color:red;">*</span></label>
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlDocumentName">
                        </asp:DropDownList>
                         <asp:RequiredFieldValidator ID="rfvDocumentName" runat="server" ControlToValidate="ddlDocumentName"
                             InitialValue="" ErrorMessage="Document Name is required." CssClass="text-danger" Display="Dynamic" SetFocusOnError="true" />
                    </div>
                </div>
                <div class="col-md-6 mb-3">
                    <div class="form-group">
                        <label class="mb-1">Document Number <span style="color:red;">*</span></label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtDocumentNumber" placeholder="" MaxLength="20"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rfvDocumentNumber" runat="server" ControlToValidate="txtDocumentNumber"
                             ErrorMessage="Document Number is required." CssClass="text-danger" Display="Dynamic" SetFocusOnError="true" />
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="row">
                        <div class="col-md-8">
                            <div class="form-group">
                                <label class="mb-1">File<span style="color:red;">*</span></label>
                                <asp:FileUpload runat="server" ID="fuFrontSide" CssClass="form-control" />                             
                                <asp:Label ID="lblDocument" runat="server" Text="Document is required." ForeColor="Red" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group mt-4" style="text-align:center;">
                                <asp:Button runat="server" ID="btnUploadFront" Text="Upload" CssClass="btn btn-primary" OnClick="btnUploadFront_Click"  />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div class="DocumentData table-responsive">
                        <asp:GridView runat="server" ID="gvDocuments" CssClass="table-responsive table data-table table-striped table-bordered nowrap"
                            AutoGenerateColumns="false" GridLines="None">
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

                <div class="col-md-12">
                    <div class="form-group mb-3 mt-3" style="text-align: center;">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnAddDocument_Click" />
                        <asp:Button ID="btnEdit" runat="server" Text="Submit" CssClass="btn btn-primary" Visible="false" OnClick="btnEditDocument_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
