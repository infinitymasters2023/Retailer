<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Passbook.aspx.cs" Inherits="Patner_Retailer_ADO.Passbook" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .dashboard-content {
            padding: 20px;
        }

        .page-header {
            margin-bottom: 20px;
        }

            .page-header h3 {
                margin-bottom: 5px;
                color: #2c3e50;
                font-weight: 500;
            }

            .page-header p {
                color: #667885;
            }

        .breadcrumb {
            background-color: #e9ecef;
            border-radius: 5px;
            padding: 8px 15px;
            margin-bottom: 20px;
        }

        .breadcrumb-item a {
            color: #0078d7;
            text-decoration: none;
        }

            .breadcrumb-item a:hover {
                text-decoration: underline;
            }

        .breadcrumb-item.active {
            color: #6c757d;
        }

        .card {
            border: 1px solid #dee2e6;
            border-radius: 5px;
            margin-bottom: 20px;
            transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
        }

        .card-body {
            padding: 20px;
            display: flex;
            align-items: center;
            justify-content: start;
            gap: 15px;
        }

        .bg-light-blue {
            background-color: #c8e6f1 !important;
            border: 1px solid #c8e6f1 !important;
        }

        .bg-light-purple {
            background-color: #ddd2f4 !important;
            border: 1px solid #ddd2f4 !important;
        }

        .bg-light-yallow {
            background-color: #f6e898 !important;
            border: 1px solid #f6e898 !important;
        }

        .bg-light-sage {
            background-color: #cdefdd !important;
            border: 1px solid #cdefdd !important;
        }

        .bg-light-red {
            background-color: #ffd3cd !important;
            border: 1px solid #ffd3cd !important;
        }

        .bg-light-sky {
            background-color: #a7f1ed !important;
            border: 1px solid #a7f1ed !important;
        }

        .card-body .text-muted {
            font-size: 14px;
            color: #282F53 !important;
            margin-bottom: 0;
            font-weight: 400;
        }

        .card-body h2 {
            margin-top: 0;
            margin-bottom: 0;
            font-size: 24px;
            color: #252d35;
        }

        .icon-circle-medium {
            width: 60px;
            height: 60px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .icon-box-lg {
            font-size: 24px;
        }

        .bg-info-light {
            background-color: #e0f7fa;
        }

        .text-info {
            color: #03a9f4;
        }

        .bg-primary-light {
            background-color: #e3f2fd;
        }

        .text-primary {
            color: #338af3;
        }

        .bg-secondary-light {
            background-color: #f0f4c3;
        }

        .text-secondary {
            color: #c0ca33;
        }

        .bg-brand-light {
            background-color: #fbe9e7;
        }

        .text-brand {
            color: #ff5722;
        }

        .card .btn-view-details {
            background: #fff;
            margin-top: 15px;
            font-size: 14px;
            text-align: left;
            padding: 8px 15px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            border: 2px solid #fff;
            opacity: .8;
            width: 100%;
            border-radius: 5px;
        }

        @media (max-width: 992px) {
            .col-lg-6 {
                flex: 0 0 50%;
                max-width: 50%;
            }
        }

        @media (max-width: 768px) {
            .col-md-6 {
                flex: 0 0 100%;
                max-width: 100%;
            }

            .card-body {
                flex-direction: column;
                text-align: center;
            }

                .card-body h2 {
                    margin-bottom: 10px;
                }

            .icon-circle-medium {
                margin-top: 10px;
            }
        }

        @media (max-width: 576px) {
            .page-header h3 {
                font-size: 1.5rem;
            }

            .pageheader-text {
                font-size: 0.9rem;
            }
        }
    </style>

    <script>
        setTimeout(function () {
            const rows = document.querySelectorAll('#ContentPlaceHolder1_GvTransactionDetails_wrapper .row');
            if (rows.length > 1) {
                rows[1].classList.add('table-responsive');
            }
        }, 500);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card">
        <div class="dashboard-influence">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <div class="page-header">
                            <h3 class="my-3">My Passbook</h3>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12 col-lg-8">
                        <div class="row">
                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6">
                                <div class="card bg-success-light mb-3 mx-0 mt-0 p-3">
                                    <asp:LinkButton runat="server" OnClick="TotalEarning_Click" CssClass="text-decoration-none text-dark">
                                        <div class="card-body p-0">
                                            <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: floralwhite;">
                                                <svg width="64" height="64" viewBox="0 0 64 64" xmlns="http://www.w3.org/2000/svg" fill="none">
                                                    <rect x="12" y="40" width="40" height="12" rx="2" fill="#4CAF50" />
                                                    <rect x="16" y="36" width="36" height="12" rx="2" fill="#66BB6A" />
                                                    <rect x="20" y="32" width="32" height="12" rx="2" fill="#81C784" />
                                                    <circle cx="44" cy="20" r="10" fill="#FFD54F" stroke="#FBC02D" stroke-width="2" />
                                                    <text x="44" y="25" text-anchor="middle" font-size="14" font-family="Arial" fill="#555" font-weight="bold">Rs. </text>
                                                    <path d="M24 32 L32 20 L40 32" stroke="#388E3C" stroke-width="4" fill="none" stroke-linecap="round" stroke-linejoin="round" />
                                                    <line x1="32" y1="20" x2="32" y2="40" stroke="#388E3C" stroke-width="4" stroke-linecap="round" />
                                                </svg>
                                            </div>
                                            <div class="d-inline-block">
                                                <h5 class="text-muted">Total Earning</h5>
                                                <h2 class="mb-0">
                                                    <asp:Label ID="lblRevenue" runat="server" Text="0"></asp:Label>
                                                </h2>
                                            </div>
                                        </div>
                                    </asp:LinkButton>
                                    <button class="btn btn-view-details btn-block" runat="server" onserverclick="TotalEarning_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                                </div>
                            </div>
                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 pl-1">
                                <div class="card mb-3 mx-0 mt-0 p-3" style="background: #8bd3e6;">
                                    <asp:LinkButton runat="server" OnClick="SettledAmount_Click" CssClass="text-decoration-none text-dark">
                                        <div class="card-body p-0">
                                            <div class="float-right icon-circle-medium icon-box-lg bg-info-light mt-1">
                                                <i class="fas fa-hand-holding-usd text-primary"></i>
                                            </div>
                                            <div class="d-inline-block">
                                                <h5 class="text-muted">Settled Amount</h5>
                                                <h2 class="mb-0">
                                                    <asp:Label ID="lblSettledAmount" runat="server" Text="0"></asp:Label>
                                                </h2>
                                            </div>
                                        </div>
                                    </asp:LinkButton>
                                    <button class="btn btn-view-details btn-block" runat="server" onserverclick="SettledAmount_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                                </div>
                            </div>
                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6">
                                <div class="card bg-light-blue mb-3 mx-0 mt-0 p-3">
                                    <asp:LinkButton runat="server" OnClick="UnsettledAmount_Click" CssClass="text-decoration-none text-dark">
                                        <div class="card-body p-0">
                                            <div class="float-right icon-circle-medium  icon-box-lg  bg-info-light mt-1">
                                                <svg width="64" height="64" viewBox="0 0 64 64" xmlns="http://www.w3.org/2000/svg" fill="none">
                                                    <rect x="10" y="24" width="44" height="28" rx="4" fill="#4CAF50" stroke="#388E3C" stroke-width="2" />
                                                    <rect x="10" y="20" width="44" height="10" rx="2" fill="#66BB6A" stroke="#388E3C" stroke-width="2" />
                                                    <circle cx="48" cy="38" r="5" fill="#FFD54F" stroke="#FBC02D" stroke-width="2" />
                                                    <text x="48" y="42" text-anchor="middle" font-size="10" font-family="Arial" fill="#555" font-weight="bold">Rs. </text>
                                                    <path d="M32 10v16" stroke="#F57C00" stroke-width="4" stroke-linecap="round" />
                                                    <path d="M26 20l6 6 6-6" fill="none" stroke="#F57C00" stroke-width="4" stroke-linecap="round" stroke-linejoin="round" />
                                                </svg>
                                            </div>
                                            <div class="d-inline-block">
                                                <h5 class="text-muted">Unsettled Amount</h5>
                                                <h2 class="mb-0">
                                                    <asp:Label ID="lblUnsettledAmount" runat="server" Text="0"></asp:Label>
                                                </h2>
                                            </div>
                                        </div>
                                    </asp:LinkButton>
                                    <button class="btn btn-view-details btn-block" runat="server" onserverclick="UnsettledAmount_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                                </div>
                            </div>
                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 pl-1" runat="server" id="SettledGSTAmountPanel">
                                <div class="card mb-3 mx-0 mt-0 p-3" style="background: #8bd3e6;">
                                    <asp:LinkButton runat="server" OnClick="SettledGSTAmount_Click" CssClass="text-decoration-none text-dark">
                                        <div class="card-body p-0">
                                            <div class="float-right icon-circle-medium icon-box-lg bg-info-light mt-1">
                                                <i class="fas fa-hand-holding-usd text-primary"></i>
                                            </div>
                                            <div class="d-inline-block">
                                                <h5 class="text-muted">Settled GST Amount</h5>
                                                <h2 class="mb-0">
                                                    <asp:Label ID="lblSettledGSTAmount" runat="server" Text="0"></asp:Label>
                                                </h2>
                                            </div>
                                        </div>
                                    </asp:LinkButton>
                                    <button class="btn btn-view-details btn-block" runat="server" onserverclick="SettledGSTAmount_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                                </div>
                            </div>
                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6" runat="server" id="UnsettledGSTAmountPanel">
                                <div class="card bg-light-blue mb-3 mx-0 mt-0 p-3">
                                    <asp:LinkButton runat="server" OnClick="UnsettledGSTAmount_Click" CssClass="text-decoration-none text-dark">
                                        <div class="card-body p-0">
                                            <div class="float-right icon-circle-medium  icon-box-lg  bg-info-light mt-1">
                                                <svg width="64" height="64" viewBox="0 0 64 64" xmlns="http://www.w3.org/2000/svg" fill="none">
                                                    <rect x="10" y="24" width="44" height="28" rx="4" fill="#4CAF50" stroke="#388E3C" stroke-width="2" />
                                                    <rect x="10" y="20" width="44" height="10" rx="2" fill="#66BB6A" stroke="#388E3C" stroke-width="2" />
                                                    <circle cx="48" cy="38" r="5" fill="#FFD54F" stroke="#FBC02D" stroke-width="2" />
                                                    <text x="48" y="42" text-anchor="middle" font-size="10" font-family="Arial" fill="#555" font-weight="bold">Rs. </text>
                                                    <path d="M32 10v16" stroke="#F57C00" stroke-width="4" stroke-linecap="round" />
                                                    <path d="M26 20l6 6 6-6" fill="none" stroke="#F57C00" stroke-width="4" stroke-linecap="round" stroke-linejoin="round" />
                                                </svg>
                                            </div>
                                            <div class="d-inline-block">
                                                <h5 class="text-muted">Unsettled GST Amount</h5>
                                                <h2 class="mb-0">
                                                    <asp:Label ID="lblUnsettledGSTAmount" runat="server" Text="0"></asp:Label>
                                                </h2>
                                            </div>
                                        </div>
                                    </asp:LinkButton>
                                    <button class="btn btn-view-details btn-block" runat="server" onserverclick="UnsettledGSTAmount_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                                </div>
                            </div>
                            
                   <%--         <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 pl-1">
                                <div class="card mb-3 mx-0 mt-0 p-3" style="background: #8bd3e6;">
                                    <asp:LinkButton runat="server" OnClick="ApprovedAmount_Click" CssClass="text-decoration-none text-dark">
                                        <div class="card-body p-0">
                                            <div class="float-right icon-circle-medium icon-box-lg bg-info-light mt-1">
                                                <i class="fas fa-hand-holding-usd text-primary"></i>
                                            </div>
                                            <div class="d-inline-block">
                                                <h5 class="text-muted">Approved Amount</h5>
                                                <h2 class="mb-0">
                                                    <asp:Label ID="lblApprovedAmount" runat="server" Text="0"></asp:Label>
                                                </h2>
                                            </div>
                                        </div>
                                    </asp:LinkButton>
                                    <button class="btn btn-view-details btn-block" runat="server" onserverclick="ApprovedAmount_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                                </div>
                            </div>
                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6">
                                <div class="card bg-light-blue mb-3 mx-0 mt-0 p-3">
                                    <asp:LinkButton runat="server" OnClick="TotalWithdrawal_Click" CssClass="text-decoration-none text-dark">
                                        <div class="card-body p-0">
                                            <div class="float-right icon-circle-medium  icon-box-lg  bg-info-light mt-1">
                                                <svg width="64" height="64" viewBox="0 0 64 64" xmlns="http://www.w3.org/2000/svg" fill="none">
                                                    <rect x="10" y="24" width="44" height="28" rx="4" fill="#4CAF50" stroke="#388E3C" stroke-width="2" />
                                                    <rect x="10" y="20" width="44" height="10" rx="2" fill="#66BB6A" stroke="#388E3C" stroke-width="2" />
                                                    <circle cx="48" cy="38" r="5" fill="#FFD54F" stroke="#FBC02D" stroke-width="2" />
                                                    <text x="48" y="42" text-anchor="middle" font-size="10" font-family="Arial" fill="#555" font-weight="bold">Rs. </text>
                                                    <path d="M32 10v16" stroke="#F57C00" stroke-width="4" stroke-linecap="round" />
                                                    <path d="M26 20l6 6 6-6" fill="none" stroke="#F57C00" stroke-width="4" stroke-linecap="round" stroke-linejoin="round" />
                                                </svg>
                                            </div>
                                            <div class="d-inline-block">
                                                <h5 class="text-muted">Total Withdrawal</h5>
                                                <h2 class="mb-0">
                                                    <asp:Label ID="lblWithdrawal" runat="server" Text="0"></asp:Label>
                                                </h2>
                                            </div>
                                        </div>
                                    </asp:LinkButton>
                                    <button class="btn btn-view-details btn-block" runat="server" onserverclick="TotalWithdrawal_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                                </div>
                            </div>
                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 pl-1">
                                <div class="card bg-light-yallow mb-3 mx-0 mt-0 p-3">
                                    <asp:LinkButton runat="server" OnClick="UnderProcess_Click" CssClass="text-decoration-none text-dark">
                                        <div class="card-body p-0">
                                            <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: antiquewhite;">
                                                <svg class="hourglass-icon" width="64" height="64" viewBox="0 0 64 64" fill="none" xmlns="http://www.w3.org/2000/svg">
                                                    <path d="M44 52H20C18.8954 52 18 51.1046 18 50V38C18 36.5765 18.8184 35.3193 20.0001 34.5097L30.0001 28.5097C30.9529 27.893 30.9529 26.107 30.0001 25.4903L20.0001 19.4903C18.8184 18.6807 18 17.4235 18 16V14C18 12.8954 18.8954 12 20 12H44C45.1046 12 46 12.8954 46 14V16C46 17.4235 45.1816 18.6807 44 19.4903L34 25.4903C33.0471 26.107 33.0471 27.893 34 28.5097L44 34.5097C45.1816 35.3193 46 36.5765 46 38V50C46 51.1046 45.1046 52 44 52Z" stroke="#546E7A" stroke-width="4" />
                                                    <path class="top-sand-fill" d="M22 16V22C25.4243 24.1378 28.618 25.8622 32 27C35.382 25.8622 38.5757 24.1378 42 22V16H22Z" fill="#FFD54F" />
                                                    <path class="bottom-sand-fill" d="M22 48V42C25.4243 39.8622 28.618 38.1378 32 37C35.382 38.1378 38.5757 39.8622 42 42V48H22Z" fill="#FFD54F" />
                                                    <line class="sand-stream" x1="32" y1="27" x2="32" y2="37" stroke="#FFD54F" stroke-width="2" />
                                                </svg>
                                            </div>
                                            <div class="d-inline-block">
                                                <h5 class="text-muted">Under process</h5>
                                                <h2 class="mb-0">
                                                    <asp:Label ID="lblUnderProcess" runat="server" Text="0"></asp:Label>
                                                </h2>
                                            </div>
                                        </div>
                                    </asp:LinkButton>
                                    <button class="btn btn-view-details btn-block" runat="server" onserverclick="UnderProcess_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                                </div>
                            </div>--%>
                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 pl-1">
                                <div class="card mb-3 mx-0 mt-0 p-3" style="background: #f7a5a5;">
                                    <asp:LinkButton runat="server" OnClick="PaymentFailed_Click" CssClass="text-decoration-none text-dark">
                                        <div class="card-body p-0">
                                            <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: aliceblue;">
                                                <svg width="64" height="64" viewBox="0 0 64 64" fill="none" xmlns="http://www.w3.org/2000/svg">
                                                    <path d="M56 22H8C6.89543 22 6 22.8954 6 24V48C6 49.1046 6.89543 50 8 50H56C57.1046 50 58 49.1046 58 48V24C58 22.8954 57.1046 22 56 22Z" fill="#E0E0E0" />
                                                    <path d="M6 30H58V26C58 24.8954 57.1046 24 56 24H8C6.89543 24 6 24.8954 6 26V30Z" fill="#BDBDBD" />
                                                    <path d="M42 42H52" stroke="#424242" stroke-width="2" stroke-linecap="round" />
                                                    <path d="M30 42H36" stroke="#424242" stroke-width="2" stroke-linecap="round" />
                                                    <path d="M48 30L40 38" stroke="#D32F2F" stroke-width="4" stroke-linecap="round" stroke-linejoin="round" />
                                                    <path d="M40 30L48 38" stroke="#D32F2F" stroke-width="4" stroke-linecap="round" stroke-linejoin="round" />
                                                </svg>
                                            </div>
                                            <div class="d-inline-block">
                                                <h5 class="text-muted">Payment Failed</h5>
                                                <h2 class="mb-0">
                                                    <asp:Label ID="lblPaymentFailed" runat="server" Text="0"></asp:Label>
                                                </h2>
                                            </div>
                                        </div>
                                    </asp:LinkButton>
                                    <button class="btn btn-view-details btn-block" runat="server" onserverclick="PaymentFailed_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                                </div>
                            </div>
                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6">
                                <div class="card mb-3 mx-0 mt-0 p-3" style="background: #c77c78;">
                                    <asp:LinkButton runat="server" OnClick="Penalty_Click" CssClass="text-decoration-none text-dark">
                                        <div class="card-body p-0">
                                            <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: aliceblue;">
                                                <svg width="64" height="64" viewBox="0 0 64 64" fill="none" xmlns="http://www.w3.org/2000/svg">
                                                    <rect x="12" y="8" width="40" height="48" rx="4" fill="#ECEFF1" stroke="#37474F" stroke-width="4" />
                                                    <path d="M26 8V6C26 4.89543 26.8954 4 28 4H36C37.1046 4 38 4.89543 38 6V8" stroke="#37474F" stroke-width="4" />
                                                    <rect x="22" y="8" width="20" height="8" fill="#D2B48C" stroke="#37474F" stroke-width="4" />
                                                    <circle cx="32" cy="11" r="1.5" fill="#37474F" />
                                                    <path d="M20 24H44" stroke="#37474F" stroke-width="3" stroke-linecap="round" />
                                                    <path d="M20 30H44" stroke="#37474F" stroke-width="3" stroke-linecap="round" />
                                                    <path d="M20 50H44" stroke="#37474F" stroke-width="3" stroke-linecap="round" />
                                                    <circle cx="32" cy="40" r="8" fill="#FFCA28" stroke="#37474F" stroke-width="3" />
                                                    <path d="M31.1465 42.1094C31.5117 42.0234 32.5117 41.7266 33.0977 41.4844C34.4258 40.9141 34.7812 40.0156 34.7812 39.1172C34.7812 38.0312 34.0781 37.3281 32.75 37.3281C31.4219 37.3281 30.6328 37.8984 30.6328 37.8984L30.1367 36.75C30.1367 36.75 30.8242 36.2266 32.125 36.2266C33.7227 36.2266 35.1523 37.0703 35.1523 38.8359C35.1523 40.0977 34.2812 40.8594 32.9531 41.3438C32.1641 41.6406 31.0781 41.9648 30.5195 42.1641C30.207 42.2734 29.5742 42.543 29.5742 43.3047C29.5742 43.9922 30.1992 44.4219 31.0781 44.4219C32.2539 44.4219 33.043 43.8516 33.043 43.8516L33.5391 44.9844C33.5391 44.9844 32.75 45.5625 31.0781 45.5625C29.2578 45.5625 28.082 44.7188 28.082 43.1484C28.082 41.4297 29.9648 40.6406 31.1465 42.1094Z" fill="#37474F" transform="scale(0.8) translate(8, 10)" />
                                                    <g transform="rotate(50, 24, 30)">
                                                        <rect x="18" y="28" width="6" height="18" rx="1" fill="#A1887F" stroke="#37474F" stroke-width="3" />
                                                        <path d="M14 22H28L30 28H12L14 22Z" fill="#D2B48C" stroke="#37474F" stroke-width="3" />
                                                    </g>
                                                </svg>
                                            </div>
                                            <div class="d-inline-block">
                                                <h5 class="text-muted">Penalty</h5>
                                                <h2 class="mb-0">
                                                    <asp:Label ID="lblPanelty" runat="server" Text="0"></asp:Label>
                                                </h2>
                                            </div>
                                        </div>
                                    </asp:LinkButton>
                                    <button class="btn btn-view-details btn-block" runat="server" onserverclick="Penalty_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                                </div>
                            </div>
                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-6 pl-1">
                                <div class="card bg-light-red mb-3 mx-0 mt-0 p-3">
                                    <asp:LinkButton runat="server" OnClick="CancellationCharges_Click" CssClass="text-decoration-none text-dark">
                                        <div class="card-body p-0">
                                            <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: aliceblue;">
                                                <svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 122.88 120.11" style="height: 30px;" xml:space="preserve">
                                                    <style type="text/css">
                                                    <![CDATA[
                                                        .st0 {
                                                            fill: #393939;
                                                        }

                                                        .st1 {
                                                            fill-rule: evenodd;
                                                            clip-rule: evenodd;
                                                            fill: #FF4141;
                                                        }
                                                    ]]>
                                                </style>
                                                    <g>
                                                        <path class="st1" d="M91.09,56.53c8.77,0,16.73,3.56,22.48,9.31c5.75,5.76,9.31,13.71,9.31,22.48c0,8.78-3.56,16.73-9.31,22.48 c-5.76,5.75-13.71,9.31-22.48,9.31c-8.78,0-16.73-3.56-22.48-9.31c-5.75-5.76-9.31-13.71-9.31-22.48c0-8.77,3.56-16.73,9.31-22.48 C74.36,60.09,82.32,56.53,91.09,56.53L91.09,56.53z M97.31,76.78c1.42-1.44,3.74-1.45,5.17-0.01c1.43,1.44,1.44,3.77,0.01,5.22 l-6.24,6.33l6.25,6.34c1.41,1.43,1.39,3.75-0.04,5.19c-1.43,1.43-3.74,1.43-5.15-0.01l-6.21-6.29l-6.22,6.31 c-1.42,1.45-3.74,1.45-5.17,0.01c-1.43-1.44-1.44-3.77-0.01-5.22l6.24-6.33l-6.25-6.34c-1.41-1.43-1.39-3.75,0.04-5.19 c1.43-1.43,3.74-1.43,5.15,0.01l6.21,6.29L97.31,76.78L97.31,76.78L97.31,76.78z" />
                                                        <path class="st0" d="M69.43,4.04c0-2.22,2.19-4.04,4.92-4.04s4.92,1.8,4.92,4.04V21.7c0,2.22-2.19,4.04-4.92,4.04 s-4.92-1.8-4.92-4.04V4.04L69.43,4.04L69.43,4.04z M13.41,57.17c-0.28,0-0.53-1.23-0.53-2.73c0-1.51,0.22-2.72,0.53-2.72h13.44 c0.28,0,0.53,1.23,0.53,2.72c0,1.5-0.22,2.73-0.53,2.73H13.41L13.41,57.17L13.41,57.17z M34.82,57.17c-0.28,0-0.53-1.23-0.53-2.73 c0-1.51,0.22-2.72,0.53-2.72h13.44c0.28,0,0.53,1.23,0.53,2.72c0,1.5-0.22,2.73-0.53,2.73H34.82L34.82,57.17L34.82,57.17z M56.24,57.17c-0.28,0-0.53-1.23-0.53-2.73c0-1.51,0.22-2.72,0.53-2.72h13.44c0.28,0,0.53,1.22,0.53,2.71 c-1.35,0.84-2.64,1.75-3.88,2.74H56.24L56.24,57.17L56.24,57.17z M13.44,72.8c-0.28,0-0.53-1.23-0.53-2.73 c0-1.51,0.22-2.73,0.53-2.73h13.44c0.28,0,0.53,1.22,0.53,2.73c0,1.5-0.22,2.73-0.53,2.73H13.44L13.44,72.8L13.44,72.8z M34.85,72.8c-0.28,0-0.53-1.23-0.53-2.73c0-1.51,0.22-2.73,0.53-2.73h13.44c0.28,0,0.53,1.22,0.53,2.73c0,1.5-0.22,2.73-0.53,2.73 H34.85L34.85,72.8L34.85,72.8z M13.47,88.43c-0.28,0-0.53-1.22-0.53-2.73c0-1.5,0.22-2.73,0.53-2.73H26.9 c0.28,0,0.53,1.23,0.53,2.73c0,1.51-0.22,2.73-0.53,2.73H13.47L13.47,88.43L13.47,88.43z M34.88,88.43c-0.28,0-0.53-1.22-0.53-2.73 c0-1.5,0.22-2.73,0.53-2.73h13.44c0.28,0,0.53,1.23,0.53,2.73c0,1.51-0.22,2.73-0.53,2.73H34.88L34.88,88.43L34.88,88.43z M25.21,4.04C25.21,1.81,27.4,0,30.13,0s4.92,1.8,4.92,4.04V21.7c0,2.22-2.2,4.04-4.92,4.04c-2.73,0-4.92-1.8-4.92-4.04V4.04 L25.21,4.04L25.21,4.04z M5.42,38.61h93.77V18.28c0-0.7-0.28-1.31-0.73-1.75s-1.09-0.73-1.75-0.73h-8.99 c-1.5,0-2.73-1.23-2.73-2.73c0-1.5,1.23-2.73,2.73-2.73h8.99c2.2,0,4.18,0.89,5.62,2.33c1.45,1.44,2.33,3.42,2.33,5.62v32.28 c-1.79-0.62-3.64-1.12-5.53-1.49v-5.04h0.06H5.42V96.7c0,0.7,0.28,1.3,0.73,1.75s1.09,0.73,1.75,0.73h44.56 c0.51,1.89,1.15,3.74,1.91,5.51H7.95c-2.19,0-4.18-0.89-5.62-2.33C0.89,100.92,0,98.95,0,96.75V18.3c0-2.19,0.89-4.18,2.33-5.62 c1.44-1.45,3.42-2.33,5.62-2.33h9.6c1.51,0,2.73,1.23,2.73,2.73c0,1.5-1.23,2.73-2.73,2.73h-9.6c-0.7,0-1.31,0.28-1.75,0.73 c-0.45,0.45-0.73,1.09-0.73,1.75v20.33H5.42V38.61L5.42,38.61z M42.93,15.8c-1.5,0-2.73-1.23-2.73-2.73c0-1.5,1.23-2.73,2.73-2.73 h18.3c1.5,0,2.73,1.23,2.73,2.73c0,1.51-1.23,2.73-2.73,2.73H42.93L42.93,15.8L42.93,15.8z" />
                                                    </g>
                                                </svg>
                                            </div>
                                            <div class="d-inline-block">
                                                <h5 class="text-muted">Cancellation Charges</h5>
                                                <h2 class="mb-0">
                                                    <asp:Label ID="lblCancellationCharges" runat="server" Text="0"></asp:Label>
                                                </h2>
                                            </div>
                                        </div>
                                    </asp:LinkButton>
                                    <button class="btn btn-view-details btn-block" runat="server" onserverclick="CancellationCharges_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-lg-4">
                        <h3>Account Details</h3>
                        <div class="table-responsive">
                            <table class="table table-bordered">
                                <tbody>
                                    <tr>
                                        <th scope="row">Account Holder Name</th>
                                        <td>
                                            <asp:Label ID="lblAccountHolderName" runat="server" Text="-"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <th scope="row">Account Number</th>
                                        <td>
                                            <asp:Label ID="lblAccountNumber" runat="server" Text="-"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <th scope="row">IFSC Code</th>
                                        <td>
                                            <asp:Label ID="lblIFSC" runat="server" Text="-"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <th scope="row">Bank Name</th>
                                        <td>
                                            <asp:Label ID="lblBankName" runat="server" Text="-"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <th scope="row">Branch</th>
                                        <td>
                                            <asp:Label ID="lblBranch" runat="server" Text="-"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <th scope="row">Bank Address</th>
                                        <td>
                                            <asp:Label ID="lblBankAddress" runat="server" Text="-"></asp:Label></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-9 d-flex">
                    </div>
                    <div class="col-md-3">
                    </div>
                </div>
                <div class="row" runat="server" visible="false">
                    <div class="col-md-12">
                        <asp:GridView ID="GvTransactionDetails" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead"
                            EmptyDataText="No records available. Please refine your search.">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Sales Executive Name" />
                                <asp:BoundField DataField="emailID" HeaderText="Sales Executive Email Id" />
                                <asp:BoundField DataField="MobileNo" HeaderText="Sales Executive Mobile No" />
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
                                <asp:BoundField DataField="CommissionType" HeaderText="Commission Type" />
                                <asp:BoundField DataField="CommissionPercentage" HeaderText="Commission Percentage" />
                                <asp:BoundField DataField="CommissionValue" HeaderText="Commission Value" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
