<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Incentive_Dashboard.aspx.cs" Inherits="Patner_Retailer_ADO.Incentive_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Custom styles for the Infyshield dashboard */
        .dashboard-content {
            padding: 20px;
        }

        .page-header {
            margin-bottom: 20px;
        }

            .page-header h3 {
                margin-bottom: 5px;
                color: #2c3e50; /* Darker heading color */
                font-weight: 400;
            }

            .page-header p {
                color: #667885; /* Slightly darker description */
            }

        .breadcrumb {
            background-color: #e9ecef; /* Lighter breadcrumb background */
            border-radius: 5px;
            padding: 8px 15px;
            margin-bottom: 20px;
        }

        .breadcrumb-item a {
            color: #0078d7; /* Bootstrap primary color for links */
            text-decoration: none;
        }

            .breadcrumb-item a:hover {
                text-decoration: underline;
            }

        .breadcrumb-item.active {
            color: #6c757d; /* Gray color for the active breadcrumb item */
        }

        .card {
            border: 1px solid #dee2e6; /* Lighter card border */
            border-radius: 5px;
            margin-bottom: 20px;
            /*box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);*/
            transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out; /* Smooth transition */
        }

        /*.card:hover {
            transform: translateY(-5px);
            box-shadow: 0 6px 8px rgba(0, 0, 0, 0.15); 
        }*/


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
            margin-bottom: 5px;
            font-weight: 400;
            line-height: 18px;
        }

        .card-body h2 {
            margin-top: 0;
            margin-bottom: 0;
            font-size: 16px;
            color: #252d35;
            font-weight: 600;
        }

        .icon-circle-medium {
            width: 60px;
            height: 60px;
            border-radius: 100px;
            display: flex;
            align-items: center;
            justify-content: center;
            margin-top: 0 !important;
            padding: 15px;
        }

            .icon-circle-medium svg {
                color: #fff;
            }

        .icon-box-lg {
            font-size: 32px; /* Larger icon size */
        }

        .bg-info-light {
            background-color: #e0f7fa; /* Light background for info */
        }

        .text-info {
            color: #03a9f4; /* Info color */
        }

        .bg-primary-light {
            background-color: #e3f2fd; /* Light background for primary */
        }

        .text-primary {
            color: #338af3; /* Primary color */
        }

        .bg-secondary-light {
            background-color: #f0f4c3; /* Light background for secondary */
        }

        .text-secondary {
            color: #c0ca33; /* Secondary color */
        }

        .bg-brand-light {
            background-color: #fbe9e7; /* Light background for brand */
        }

        .text-brand {
            color: #ff5722; /* Brand color */
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

        .dashboard .card {
            border: 1px solid #e7e6e6 !important;
            border-radius: 10px;
            background: #efeeee;
            margin-bottom: 15px;
            position: relative;
            box-shadow: none;
            min-height: 160px;
        }

        /* Responsive adjustments for smaller screens */
        @media (max-width: 992px) { /* Medium screens and below */
            .col-lg-6 {
                flex: 0 0 50%; /* Each card takes up half the row */
                max-width: 50%;
            }
        }

        @media (max-width: 768px) { /* Small screens and below */
            .col-md-6 {
                flex: 0 0 100%; /* Each card takes up the full row */
                max-width: 100%;
            }

            .card-body {
                flex-direction: row;
                text-align: left;
            }

                .card-body h2 {
                    margin-bottom: 10px;
                }

            .icon-circle-medium {
                margin-top: 10px; /* Add space above the icon */
            }

            .card .btn-view-details {
                font-size: 12px;
            }
        }

        @media (max-width: 576px) {
            .page-header h3 {
                font-size: 1.2rem;
                font-weight: 500;
            }

            .pageheader-text {
                font-size: 0.9rem;
            }
        }

        .hideMessagecls {
            display: flex;
            justify-content: end;
            position: absolute;
            right: 10px;
        }
    </style>

    <script>
        const ctx = document.getElementById('dashboardChart').getContext('2d');
        const dashboardChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: [
                    'Your Earnings',
                    'Service Plans Sold',
                    'Due for Redemption',
                    'Not in Wallet Yet',
                    'Unsold Plans in Cart',
                    'Not in Cart',
                    'Registration Approved',
                    'Regd. Under Approval'
                ],
                datasets: [{
                    label: 'Dashboard Stats',
                    data: [65, 59, 80, 81, 56, 55, 40, 70],
                    backgroundColor: '#5BA0B9'
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    x: {
                        ticks: {
                            autoSkip: false,
                            maxRotation: 45,
                            minRotation: 45
                        }
                    },
                    y: {
                        beginAtZero: true,
                        max: 100
                    }
                },
                plugins: {
                    legend: {
                        display: true,
                        position: 'top'
                    }
                }
            }
        });
    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card">
        <div class="dashboard-influence">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <div class="page-header">
                            <h3 class="my-3">InfyShield Incentive Dashboard</h3>
                            <p class="pageheader-text">Track key metrics and generate reports for Infyshield sales performance.</p>
                        </div>
                    </div>
                </div>
                <div class="row dashboard" id="mainpanal" runat="server">
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="BaseMargin_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: rgba(60, 128, 151, 0.78);">
                                        <svg stroke="currentColor" fill="none" stroke-width="2" viewBox="0 0 24 24" stroke-linecap="round" stroke-linejoin="round" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M6 3h12"></path><path d="M6 8h12"></path>
                                            <path d="m6 13 8.5 8"></path>
                                            <path d="M6 13h3"></path>
                                            <path d="M9 13c6.667 0 6.667-10 0-10"></path>
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">Slab Based Margin(Total Sales/ Add More / Additional Commission)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblBaseMarginSelles" runat="server" Text="0"></asp:Label>/
                                            <asp:Label ID="lblBaseMarginAddMore" runat="server" Text="0"></asp:Label>/
                                            <asp:Label ID="lblBaseMarginCommision" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="BaseMargin_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12 pl-lg-0">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="NoClaimBonus_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: rgb(108, 69, 191);">
                                        <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 512 512" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M396.8 352h22.4c6.4 0 12.8-6.4 12.8-12.8V108.8c0-6.4-6.4-12.8-12.8-12.8h-22.4c-6.4 0-12.8 6.4-12.8 12.8v230.4c0 6.4 6.4 12.8 12.8 12.8zm-192 0h22.4c6.4 0 12.8-6.4 12.8-12.8V140.8c0-6.4-6.4-12.8-12.8-12.8h-22.4c-6.4 0-12.8 6.4-12.8 12.8v198.4c0 6.4 6.4 12.8 12.8 12.8zm96 0h22.4c6.4 0 12.8-6.4 12.8-12.8V204.8c0-6.4-6.4-12.8-12.8-12.8h-22.4c-6.4 0-12.8 6.4-12.8 12.8v134.4c0 6.4 6.4 12.8 12.8 12.8zM496 400H48V80c0-8.84-7.16-16-16-16H16C7.16 64 0 71.16 0 80v336c0 17.67 14.33 32 32 32h464c8.84 0 16-7.16 16-16v-16c0-8.84-7.16-16-16-16zm-387.2-48h22.4c6.4 0 12.8-6.4 12.8-12.8v-70.4c0-6.4-6.4-12.8-12.8-12.8h-22.4c-6.4 0-12.8 6.4-12.8 12.8v70.4c0 6.4 6.4 12.8 12.8 12.8z"></path>
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">No Claim Bonus(Bonus Value/ Nos./ Rs.)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblNCBValue" runat="server" Text="0"></asp:Label>/
                                            <asp:Label ID="lblNCBNo" runat="server" Text="0"></asp:Label>/
                                            <asp:Label ID="lblNCBRs" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="NoClaimBonus_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12 pl-lg-0">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="Incentive_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: rgba(217, 186, 28, 0.84);">
                                        <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24" height="35px" width="200px" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M16 12h2v4h-2z"></path>
                                            <path d="M20 7V5c0-1.103-.897-2-2-2H5C3.346 3 2 4.346 2 6v12c0 2.201 1.794 3 3 3h15c1.103 0 2-.897 2-2V9c0-1.103-.897-2-2-2zM5 5h13v2H5a1.001 1.001 0 0 1 0-2zm15 14H5.012C4.55 18.988 4 18.805 4 18V8.815c.314.113.647.185 1 .185h15v10z"></path>
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">Incentive (Sales Rs/ Incentive Rs.)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblIncentiveSellesRs" runat="server" Text="0"></asp:Label>/
                                            <asp:Label ID="IncentiveRs" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="Incentive_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12 pl-lg-0">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="AdditionalMargin_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: rgba(60, 128, 151, 0.78);">
                                        <svg stroke="currentColor" fill="none" stroke-width="2" viewBox="0 0 24 24" stroke-linecap="round" stroke-linejoin="round" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M6 3h12"></path><path d="M6 8h12"></path>
                                            <path d="m6 13 8.5 8"></path>
                                            <path d="M6 13h3"></path>
                                            <path d="M9 13c6.667 0 6.667-10 0-10"></path>
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">Additional Margin(Total Sales Rs/Total Sales Nos./ Add More Rs. / Add More Rs / Addition Commission)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblAdditionalMarginTotalSalesRs" runat="server" Text="0"></asp:Label>/
                                    <asp:Label ID="lblAdditionalMarginTotalSalesNo" runat="server" Text="0"></asp:Label>/
                                    <asp:Label ID="lblAdditionalMarginAddMoreRs" runat="server" Text="0"></asp:Label>/
                                    <asp:Label ID="lblAdditionalMarginAddMoreNo" runat="server" Text="0"></asp:Label>/
                                    <asp:Label ID="lblAdditionalMarginCommision" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="AdditionalMargin_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                    </div>
                <div class="row dashboard" id="divSalesPersonPanel" runat="server">
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12">
                        <div class="card mb-3 mx-0 mt-0 p-3">

                            <asp:LinkButton runat="server" OnClick="EWMargin_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: rgba(217, 186, 28, 0.84);">
                                        <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24" height="35px" width="200px" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M16 12h2v4h-2z"></path>
                                            <path d="M20 7V5c0-1.103-.897-2-2-2H5C3.346 3 2 4.346 2 6v12c0 2.201 1.794 3 3 3h15c1.103 0 2-.897 2-2V9c0-1.103-.897-2-2-2zM5 5h13v2H5a1.001 1.001 0 0 1 0-2zm15 14H5.012C4.55 18.988 4 18.805 4 18V8.815c.314.113.647.185 1 .185h15v10z"></path>
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">EW Margin (Nos./ Rs.)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblEWMarginNo" runat="server" Text="0"></asp:Label>/
                                    <asp:Label ID="lblEWMarginRs" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="EWMargin_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12 pl-lg-0">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="SalesExecutiveEarning_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: rgb(108, 69, 191);">
                                        <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 512 512" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M396.8 352h22.4c6.4 0 12.8-6.4 12.8-12.8V108.8c0-6.4-6.4-12.8-12.8-12.8h-22.4c-6.4 0-12.8 6.4-12.8 12.8v230.4c0 6.4 6.4 12.8 12.8 12.8zm-192 0h22.4c6.4 0 12.8-6.4 12.8-12.8V140.8c0-6.4-6.4-12.8-12.8-12.8h-22.4c-6.4 0-12.8 6.4-12.8 12.8v198.4c0 6.4 6.4 12.8 12.8 12.8zm96 0h22.4c6.4 0 12.8-6.4 12.8-12.8V204.8c0-6.4-6.4-12.8-12.8-12.8h-22.4c-6.4 0-12.8 6.4-12.8 12.8v134.4c0 6.4 6.4 12.8 12.8 12.8zM496 400H48V80c0-8.84-7.16-16-16-16H16C7.16 64 0 71.16 0 80v336c0 17.67 14.33 32 32 32h464c8.84 0 16-7.16 16-16v-16c0-8.84-7.16-16-16-16zm-387.2-48h22.4c6.4 0 12.8-6.4 12.8-12.8v-70.4c0-6.4-6.4-12.8-12.8-12.8h-22.4c-6.4 0-12.8 6.4-12.8 12.8v70.4c0 6.4 6.4 12.8 12.8 12.8z"></path>
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">Sales Executive Base Margin(Total Sales/ Add More / Additional Commission)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblSalesExecutiveMarginTotalSales" runat="server" Text="0"></asp:Label>/
                                            <asp:Label ID="lblSalesExecutiveMarginTotalNo" runat="server" Text="0"></asp:Label>/
                                            <asp:Label ID="lblSalesExecutiveMarginAdditionalCommision" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="SalesExecutiveEarning_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                </div>
            </div>
    </div>
    </div>
</asp:Content>

