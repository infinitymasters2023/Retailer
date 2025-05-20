<%@ Page Title="Retailer Profile" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Patner_Retailer_ADO.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Custom CSS for the profile page */
        .profile-panel .row .col-md-3 {
            margin-bottom: 0.75rem; /* Add some vertical spacing between label-value pairs */
        }

            .profile-panel .row .col-md-3 label {
                font-weight: bold; /* Make labels stand out */
                display: block; /* Ensure label takes full width of its column */
                margin-bottom: 0.25rem; /* Add a little space between label and value */
                color: #495057; /* Slightly darker text for labels */
            }

            .profile-panel .row .col-md-3 span {
                display: block; /* Display value on the next line */
                color: #212529; /* Standard text color for values */
            }

        .profile-panel h5 {
            margin-top: 1.5rem; /* Add more space above the headings */
            padding-bottom: 0.5rem;
            border-bottom: 1px solid #eee; /* Add a subtle bottom border */
            color: #007bff; /* A primary color for headings */
        }

        /* Adjust margin-top for the first heading (Retailer Profile Details) if needed */
        .profile-panel .card-header + .card-body h5 {
            margin-top: 0;
        }

        .profile-panel .table {
            width: 100%;
            margin-bottom: 1rem;
            color: #212529;
            border-collapse: collapse;
        }

            .profile-panel .table th,
            .profile-panel .table td {
                padding: 0.75rem;
                vertical-align: top;
                border-top: 1px solid #dee2e6;
                text-align: left; /* Align text to the left for better readability */
            }

            .profile-panel .table thead th {
                vertical-align: bottom;
                border-bottom: 2px solid #dee2e6;
                background-color: #343a40; /* Dark background for header */
                color: white;
            }

            .profile-panel .table tbody tr:nth-of-type(odd) {
                background-color: rgba(0, 0, 0, 0.05); /* Add subtle background for odd rows */
            }

        .profile-panel .table-sm th,
        .profile-panel .table-sm td {
            padding: 0.5rem; /* Reduce padding for a more compact table */
        }

        .profile-panel .card {
            border: 1px solid rgba(0, 0, 0, 0.125);
            border-radius: 0.25rem;
            margin-bottom: 1.5rem; /* Add some space below the profile card */
        }

        .profile-panel .card-header {
            background-color: #f8f9fa; /* Light background for header */
            padding: 0.75rem 1.25rem;
            margin-bottom: 0;
            border-bottom: 1px solid rgba(0, 0, 0, 0.125);
        }

        .profile-panel .card-body {
            padding: 1.25rem;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlProfile" runat="server" CssClass="profile-panel">
        <div class="container-fluid dashboard-content">
            <div class="card">
                <h5 class="card-header">Retailer Profile Details</h5>
                <div class="card-body">

                    <div class="row">
                        <div class="col-md-3">
                            <asp:Label ID="lblName" runat="server" Text="Name: " />
                            <asp:Label ID="lblNameValue" runat="server" />
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblMobileNo" runat="server" Text="Mobile No: " />
                            <asp:Label ID="lblMobileNoValue" runat="server" />
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblWhatsappNo" runat="server" Text="WhatsApp No: " />
                            <asp:Label ID="lblWhatsappNoValue" runat="server" />
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblEmail" runat="server" Text="Email: " />
                            <asp:Label ID="lblEmailValue" runat="server" />
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblDOB" runat="server" Text="Date of Birth: " />
                            <asp:Label ID="lblDOBValue" runat="server" />
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblGender" runat="server" Text="Gender: " />
                            <asp:Label ID="lblGenderValue" runat="server" />
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblAddress" runat="server" Text="Address: " />
                            <asp:Label ID="lblAddressValue" runat="server" />
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblCity" runat="server" Text="City: " />
                            <asp:Label ID="lblCityValue" runat="server" />
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblState" runat="server" Text="State: " />
                            <asp:Label ID="lblStateValue" runat="server" />
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblPAN" runat="server" Text="PAN No: " />
                            <asp:Label ID="lblPANValue" runat="server" />
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblAadhar" runat="server" Text="Aadhar No: " />
                            <asp:Label ID="lblAadharValue" runat="server" />
                        </div>

                    </div>
                </div>

            </div>
            <hr />
            <div class="row">
                <!-- ============================================================== -->
                <!-- fixed header  -->
                <!-- ============================================================== -->
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0">Bank Details </h5>

                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:Repeater ID="RepeaterBankDetails" runat="server">
                                    <HeaderTemplate>
                                        <table id="example44" class="table-responsive table data-table table-striped table-bordered nowrap" style="width: 100%">
                                            <thead>
                                                <tr>
                                                    <th>S.No.</th>
                                                    <th>Bank Account Number</th>
                                                    <th>IFSC Code</th>
                                                    <th>Bank Name</th>
                                                    <th>Bank Branch</th>
                                                    <th>Branch Address</th>
                                                    <th>Account Holder Name</th>
                                                    <th>Created Date</th>
                                                    <th>IP Address</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Container.ItemIndex + 1 %></td>
                                            <td><%# Eval("BankAccountNumber") %></td>
                                            <td><%# Eval("IFSCCode") %></td>
                                            <td><%# Eval("BankName") %></td>
                                            <td><%# Eval("BankBranch") %></td>
                                            <td><%# Eval("BankBranchAddress") %></td>
                                            <td><%# Eval("AccountHolderName") %></td>
                                            <td><%# Eval("CreatedDate", "{0:dd-MMM-yyyy}") %></td>
                                            <td><%# Eval("IPAddress") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>


                            </div>
                        </div>
                    </div>
                </div>
                <!-- ============================================================== -->
                <!-- end fixed header  -->
                <!-- ============================================================== -->
            </div>


            <div class="row">
                <!-- ============================================================== -->
                <!-- fixed header  -->
                <!-- ============================================================== -->
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0">Dealer Details</h5>

                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:Repeater ID="RepeaterEmployeeDetails" runat="server">
                                    <HeaderTemplate>
                                        <div class="table-responsive">
                                            <table id="example45" class="table-responsive table data-table table-striped table-bordered nowrap" style="width: 100%">
                                                <thead>
                                                    <tr>
                                                        <th>S.No.</th>
                                                        <th>Name</th>
                                                        <th>GSTIN</th>
                                                        <th>Address Line 1</th>
                                                        <th>City</th>
                                                        <th>State</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Container.ItemIndex + 1 %></td>
                                            <td><%# Eval("SellerName") %></td>
                                            <td><%# Eval("SellerGSTINNo") %></td>
                                            <td><%# Eval("AddressLine1") %></td>
                                            <td><%# Eval("City") %></td>
                                            <td><%# Eval("State") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                        </table>
                                        </div>
                                    </FooterTemplate>
                                </asp:Repeater>

                            </div>
                        </div>
                    </div>
                </div>
                <!-- ============================================================== -->
                <!-- end fixed header  -->
                <!-- ============================================================== -->
            </div>
            <div class="row">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <div class="card">
                        <div class="card-header">
                            <h5>Uploaded Documents</h5>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:Repeater ID="rptDocuments" runat="server">
                                    <HeaderTemplate>
                                        <table class="table-responsive table data-table table-striped table-bordered nowrap">
                                            <thead class="thead-dark">
                                                <tr>
                                                    <th>S.No.</th>
                                                    <th>Document Number</th>
                                                    <th>Document Path</th>
                                                    <th>Remarks</th>
                                                    <th>Status</th>
                                                    <th>Action By</th>
                                                    <th>Action Date</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Container.ItemIndex + 1 %></td>
                                            <td><%# Eval("documentNumber") %></td>
                                            <td><%# Eval("DocumentPath") %></td>
                                            <td><%# Eval("Remarks") %></td>
                                            <td><%# Eval("Status") %></td>
                                            <td><%# Eval("ActionBy") %></td>
                                            <td><%# Eval("ActionDate") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                    </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </asp:Panel>
</asp:Content>
