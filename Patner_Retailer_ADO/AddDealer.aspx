<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AddDealer.aspx.cs" Inherits="Patner_Retailer_ADO.AddDealer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function validatePincode(input) {
            input.value = input.value.replace(/\D/g, '');
            if (input.value.length > 6) {
                input.value = input.value.slice(0, 6);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card">
        <h5 class="card-header" id="hdrtext" runat="server">Add Dealer</h5>
        <div class="card-body">
            <div class="row">

                <div class="col-md-4">
                    <div class="form-group mb-3">
                        <label class="mb-1">Seller Name <span style="color: red">*</span> </label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtSellerName" placeholder="" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSellerName" runat="server" ControlToValidate="txtSellerName"
                            ErrorMessage="Seller Name is required." CssClass="text-danger" Display="Dynamic" SetFocusOnError="true" />
                        <label id="lblSellerName" visible="false" runat="server" style="color: red; font-size: 12px">Seller Name is required.</label>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group mb-3">
                        <label class="mb-1">GSTIN <span style="color: red">*</span> </label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtGSTIN" placeholder="" MaxLength="15" Enabled="false"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="rfvGSTIN" runat="server" ControlToValidate="txtGSTIN"
                              ErrorMessage="GSTIN is required." CssClass="text-danger" Display="Dynamic" SetFocusOnError="true" />
                          <asp:RegularExpressionValidator ID="revGSTIN" runat="server" ControlToValidate="txtGSTIN"
                              ErrorMessage="Invalid GSTIN format." CssClass="text-danger" Display="Dynamic" SetFocusOnError="true"
                              ValidationExpression="^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$" />
                        <label id="lblSellerGSTIN" visible="false" runat="server" style="color: red; font-size: 12px">GSTIN is required.</label>
                    </div>
                </div>

                <div class="col-md-1">
                    <div class="form-group">
                        <label class="mb-1">PIN Code <span style="color: red">*</span> </label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtSellerPincode" AutoPostBack="true" OnTextChanged="txtSellerPinCode_TextChanged" placeholder=""
                            MaxLength="6" title="Enter a 6-digit Pincode" oninput="validatePincode(this)">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPIN" runat="server" ControlToValidate="txtSellerPincode"
                            ErrorMessage="PIN Code is required." CssClass="text-danger" Display="Dynamic" SetFocusOnError="true" />
                        <label id="lblSellerPINCode" visible="false" runat="server" style="color: red; font-size: 12px">PIN Code is required.</label>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="mb-2 mt-2 mt-lg-0">City</label>
                                <asp:Label ID="lblSellerCity" CssClass="d-block" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group mb-3">
                                <label class="mb-2 mt-0 mt-lg-0">State</label>
                                <asp:Label ID="lblSellerState" CssClass="d-block" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="mb-1">Address Line 1 <span style="color: red">*</span> </label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtSellerAddressLine1" TextMode="MultiLine" Rows="3" placeholder="" MaxLength="150"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvAddress1" runat="server" ControlToValidate="txtSellerAddressLine1"
                            ErrorMessage="Address Line 1 is required." CssClass="text-danger" Display="Dynamic" SetFocusOnError="true" />
                        <label id="lblSellerAddress" visible="false" runat="server" style="color: red; font-size: 12px">Address is required.</label>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="mb-1">Address Line 2</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtSellerAddressLine2" TextMode="MultiLine" Rows="3" placeholder="" MaxLength="150"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="mb-1">Landmark <span style="color: red">*</span> </label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtSellerLandmark" TextMode="MultiLine" Rows="3" placeholder="" MaxLength="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvLandmark" runat="server" ControlToValidate="txtSellerLandmark"
                            ErrorMessage="Landmark is required." CssClass="text-danger" Display="Dynamic" SetFocusOnError="true" />
                        <label id="lblSellerLandMark" visible="false" runat="server" style="color: red; font-size: 12px">Landmark is required.</label>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="form-group mb-3" style="text-align: center;">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnAddDealer_Click" />
                        <asp:Button ID="btnEdit" runat="server" Text="Submit" CssClass="btn btn-primary" Visible="false" OnClick="btnEditDealer_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
