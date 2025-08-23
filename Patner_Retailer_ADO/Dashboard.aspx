<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Patner_Retailer_ADO.Dashboard" %>

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
    <div runat="server" id="hdrMessage">
        <div id="AccountPendingMessage" runat="server" class="alert alert-danger" role="alert">
            <asp:LinkButton CssClass="hideMessagecls" runat="server" ID="hideMessage" OnClick="hideMessageClick">
             <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="20">
                 <path d="M320 112C434.9 112 528 205.1 528 320C528 434.9 434.9 528 320 528C205.1 528 112 434.9 112 320C112 205.1 205.1 112 320 112zM320 576C461.4 576 576 461.4 576 320C576 178.6 461.4 64 320 64C178.6 64 64 178.6 64 320C64 461.4 178.6 576 320 576zM231 231C221.6 240.4 221.6 255.6 231 264.9L286 319.9L231 374.9C221.6 384.3 221.6 399.5 231 408.8C240.4 418.1 255.6 418.2 264.9 408.8L319.9 353.8L374.9 408.8C384.3 418.2 399.5 418.2 408.8 408.8C418.1 399.4 418.2 384.2 408.8 374.9L353.8 319.9L408.8 264.9C418.2 255.5 418.2 240.3 408.8 231C399.4 221.7 384.2 221.6 374.9 231L319.9 286L264.9 231C255.5 221.6 240.3 221.6 231 231z"/>
             </svg>
            </asp:LinkButton>
            <div class="mb-1">Dear Partner,</div>
            <ul class="mb-0 ms-0 ps-3">
                <li>Your basic details have been received and are under review. We will revert shortly.</li>
                <li>You may review the submitted details here as under and submit changes if any.</li>
                <li>We encourage you to visit your Profile by clicking the top right section in the header for the same.</li>
            </ul>
        </div>
        <div id="AccountMoreDocumentRequired" runat="server" class="alert alert-danger" role="alert">
            <asp:LinkButton CssClass="hideMessagecls" runat="server" ID="LinkButton1" OnClick="hideMessageClick">
             <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="20">
                 <path d="M320 112C434.9 112 528 205.1 528 320C528 434.9 434.9 528 320 528C205.1 528 112 434.9 112 320C112 205.1 205.1 112 320 112zM320 576C461.4 576 576 461.4 576 320C576 178.6 461.4 64 320 64C178.6 64 64 178.6 64 320C64 461.4 178.6 576 320 576zM231 231C221.6 240.4 221.6 255.6 231 264.9L286 319.9L231 374.9C221.6 384.3 221.6 399.5 231 408.8C240.4 418.1 255.6 418.2 264.9 408.8L319.9 353.8L374.9 408.8C384.3 418.2 399.5 418.2 408.8 408.8C418.1 399.4 418.2 384.2 408.8 374.9L353.8 319.9L408.8 264.9C418.2 255.5 418.2 240.3 408.8 231C399.4 221.7 384.2 221.6 374.9 231L319.9 286L264.9 231C255.5 221.6 240.3 221.6 231 231z"/>
             </svg>
            </asp:LinkButton>
            <div class="mb-1">Dear Partner,</div>
            <ul class="mb-0 ms-0 ps-3">
                <li>We have received your basic details. However, additional documents are required to proceed with the verification.</li>
                <li>Please review your submitted details below and upload the necessary documents or make changes if needed.</li>
                <li>We encourage you to visit your Profile by clicking the top right section in the header for the same.</li>
            </ul>
        </div>
        <div id="AccountRejectMessage" runat="server" class="alert alert-danger" role="alert">
            <asp:LinkButton CssClass="hideMessagecls" runat="server" ID="LinkButton2" OnClick="hideMessageClick">
             <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="20">
                 <path d="M320 112C434.9 112 528 205.1 528 320C528 434.9 434.9 528 320 528C205.1 528 112 434.9 112 320C112 205.1 205.1 112 320 112zM320 576C461.4 576 576 461.4 576 320C576 178.6 461.4 64 320 64C178.6 64 64 178.6 64 320C64 461.4 178.6 576 320 576zM231 231C221.6 240.4 221.6 255.6 231 264.9L286 319.9L231 374.9C221.6 384.3 221.6 399.5 231 408.8C240.4 418.1 255.6 418.2 264.9 408.8L319.9 353.8L374.9 408.8C384.3 418.2 399.5 418.2 408.8 408.8C418.1 399.4 418.2 384.2 408.8 374.9L353.8 319.9L408.8 264.9C418.2 255.5 418.2 240.3 408.8 231C399.4 221.7 384.2 221.6 374.9 231L319.9 286L264.9 231C255.5 221.6 240.3 221.6 231 231z"/>
             </svg>
            </asp:LinkButton>
            <div class="mb-1">Dear Partner,</div>
            <ul class="mb-0 ms-0 ps-3">
                <li>Unfortunately, your application has been rejected due to incomplete or invalid information.</li>
                <li>Please review your submitted details below and make the necessary corrections or upload the required documents to reapply.</li>
                <li>We encourage you to visit your Profile by clicking the top right section in the header for the same.</li>
            </ul>
        </div>
        <div id="PendingWithdrawApplicationMessage" runat="server" class="alert alert-danger" role="alert">
            <asp:LinkButton CssClass="hideMessagecls" runat="server" ID="LinkButton3" OnClick="hideMessageClick">
             <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="20">
                 <path d="M320 112C434.9 112 528 205.1 528 320C528 434.9 434.9 528 320 528C205.1 528 112 434.9 112 320C112 205.1 205.1 112 320 112zM320 576C461.4 576 576 461.4 576 320C576 178.6 461.4 64 320 64C178.6 64 64 178.6 64 320C64 461.4 178.6 576 320 576zM231 231C221.6 240.4 221.6 255.6 231 264.9L286 319.9L231 374.9C221.6 384.3 221.6 399.5 231 408.8C240.4 418.1 255.6 418.2 264.9 408.8L319.9 353.8L374.9 408.8C384.3 418.2 399.5 418.2 408.8 408.8C418.1 399.4 418.2 384.2 408.8 374.9L353.8 319.9L408.8 264.9C418.2 255.5 418.2 240.3 408.8 231C399.4 221.7 384.2 221.6 374.9 231L319.9 286L264.9 231C255.5 221.6 240.3 221.6 231 231z"/>
             </svg>
            </asp:LinkButton>
            <div class="mb-1">Dear Partner,</div>
            <ul class="mb-0 ms-0 ps-3">
                <li>Your request to withdraw your application has been received and is currently under review.</li>
                <li>Please wait while our team verifies the request. You will be notified once the process is completed.</li>
                <li>We encourage you to visit your Profile by clicking the top right section in the header for the same.</li>
            </ul>
        </div>
        <div id="TerminateApplication" runat="server" class="alert alert-danger" role="alert">
            <asp:LinkButton CssClass="hideMessagecls" runat="server" ID="LinkButton4" OnClick="hideMessageClick">
             <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="20">
                 <path d="M320 112C434.9 112 528 205.1 528 320C528 434.9 434.9 528 320 528C205.1 528 112 434.9 112 320C112 205.1 205.1 112 320 112zM320 576C461.4 576 576 461.4 576 320C576 178.6 461.4 64 320 64C178.6 64 64 178.6 64 320C64 461.4 178.6 576 320 576zM231 231C221.6 240.4 221.6 255.6 231 264.9L286 319.9L231 374.9C221.6 384.3 221.6 399.5 231 408.8C240.4 418.1 255.6 418.2 264.9 408.8L319.9 353.8L374.9 408.8C384.3 418.2 399.5 418.2 408.8 408.8C418.1 399.4 418.2 384.2 408.8 374.9L353.8 319.9L408.8 264.9C418.2 255.5 418.2 240.3 408.8 231C399.4 221.7 384.2 221.6 374.9 231L319.9 286L264.9 231C255.5 221.6 240.3 221.6 231 231z"/>
             </svg>
            </asp:LinkButton>
            <div class="mb-1">Dear Partner,</div>
            <ul class="mb-0 ms-0 ps-3">
                <li>Your application has been terminated due to non-compliance or failure to meet the required criteria.</li>
                <li>If you believe this was a mistake or would like to reapply, please contact support or follow the reapplication process.</li>
                <li>We encourage you to visit your Profile by clicking the top right section in the header for the same.</li>
            </ul>
        </div>
        <div id="ApprovedWithdrawApplication" runat="server" class="alert alert-success" role="alert">
            <asp:LinkButton CssClass="hideMessagecls" runat="server" ID="LinkButton5" OnClick="hideMessageClick">
             <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="20">
                 <path d="M320 112C434.9 112 528 205.1 528 320C528 434.9 434.9 528 320 528C205.1 528 112 434.9 112 320C112 205.1 205.1 112 320 112zM320 576C461.4 576 576 461.4 576 320C576 178.6 461.4 64 320 64C178.6 64 64 178.6 64 320C64 461.4 178.6 576 320 576zM231 231C221.6 240.4 221.6 255.6 231 264.9L286 319.9L231 374.9C221.6 384.3 221.6 399.5 231 408.8C240.4 418.1 255.6 418.2 264.9 408.8L319.9 353.8L374.9 408.8C384.3 418.2 399.5 418.2 408.8 408.8C418.1 399.4 418.2 384.2 408.8 374.9L353.8 319.9L408.8 264.9C418.2 255.5 418.2 240.3 408.8 231C399.4 221.7 384.2 221.6 374.9 231L319.9 286L264.9 231C255.5 221.6 240.3 221.6 231 231z"/>
             </svg>
            </asp:LinkButton>
            <div class="mb-1">Dear Partner,</div>
            <ul class="mb-0 ms-0 ps-3">
                <li>Your request to withdraw your application has been approved successfully.</li>
                <li>If you wish to reapply in the future, you may initiate a new application at any time.</li>
                <li>We encourage you to visit your Profile by clicking the top right section in the header for the same.</li>
            </ul>
        </div>


        <div id="AccountApprovedMessage" runat="server" class="alert alert-success" role="alert">
            <asp:LinkButton CssClass="hideMessagecls" runat="server" ID="LinkButton6" OnClick="hideMessageClick">
             <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" height="20">
                 <path d="M320 112C434.9 112 528 205.1 528 320C528 434.9 434.9 528 320 528C205.1 528 112 434.9 112 320C112 205.1 205.1 112 320 112zM320 576C461.4 576 576 461.4 576 320C576 178.6 461.4 64 320 64C178.6 64 64 178.6 64 320C64 461.4 178.6 576 320 576zM231 231C221.6 240.4 221.6 255.6 231 264.9L286 319.9L231 374.9C221.6 384.3 221.6 399.5 231 408.8C240.4 418.1 255.6 418.2 264.9 408.8L319.9 353.8L374.9 408.8C384.3 418.2 399.5 418.2 408.8 408.8C418.1 399.4 418.2 384.2 408.8 374.9L353.8 319.9L408.8 264.9C418.2 255.5 418.2 240.3 408.8 231C399.4 221.7 384.2 221.6 374.9 231L319.9 286L264.9 231C255.5 221.6 240.3 221.6 231 231z"/>
             </svg>
            </asp:LinkButton>
            <div class="mb-1">Dear Partner,</div>
            <ul class="mb-0 ms-0 ps-3">
                <li>Your profile has been approved successfully.</li>
                <li>You may now access all features and continue managing your services.</li>
            </ul>
        </div>
    </div>
    <div class="card">

        <div class="dashboard-influence">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <div class="page-header">
                            <h3 class="my-3">InfyShield Sales Dashboard</h3>
                            <p class="pageheader-text">Track key metrics and generate reports for Infyshield sales performance.</p>
                            <%--<div class="page-breadcrumb">
                            <nav aria-label="breadcrumb">
                                <ol class="breadcrumb">
                                    <li class="breadcrumb-item"><a href="#" class="breadcrumb-link">Dashboard</a></li>
                                    <li class="breadcrumb-item active" aria-current="page">Infyshield Sales</li>
                                </ol>
                            </nav>
                        </div>--%>
                        </div>
                    </div>
                </div>
                <div class="row dashboard" id="mainpanal" runat="server">
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="TotalEarning_Click" CssClass="text-decoration-none text-dark">
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
                                        <h5 class="text-muted">Your Earnings so far (In Rs.)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblTotalSales" runat="server" Text="0"></asp:Label>
                                        </h2>

                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="TotalEarning_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12 pl-lg-0">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="ServicePlanSold_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: rgb(108, 69, 191);">
                                        <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 512 512" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M396.8 352h22.4c6.4 0 12.8-6.4 12.8-12.8V108.8c0-6.4-6.4-12.8-12.8-12.8h-22.4c-6.4 0-12.8 6.4-12.8 12.8v230.4c0 6.4 6.4 12.8 12.8 12.8zm-192 0h22.4c6.4 0 12.8-6.4 12.8-12.8V140.8c0-6.4-6.4-12.8-12.8-12.8h-22.4c-6.4 0-12.8 6.4-12.8 12.8v198.4c0 6.4 6.4 12.8 12.8 12.8zm96 0h22.4c6.4 0 12.8-6.4 12.8-12.8V204.8c0-6.4-6.4-12.8-12.8-12.8h-22.4c-6.4 0-12.8 6.4-12.8 12.8v134.4c0 6.4 6.4 12.8 12.8 12.8zM496 400H48V80c0-8.84-7.16-16-16-16H16C7.16 64 0 71.16 0 80v336c0 17.67 14.33 32 32 32h464c8.84 0 16-7.16 16-16v-16c0-8.84-7.16-16-16-16zm-387.2-48h22.4c6.4 0 12.8-6.4 12.8-12.8v-70.4c0-6.4-6.4-12.8-12.8-12.8h-22.4c-6.4 0-12.8 6.4-12.8 12.8v70.4c0 6.4 6.4 12.8 12.8 12.8z"></path>
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">Service Plans Sold (Nos./Rs.)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblServicePlansSoldNo" runat="server" Text="0"></asp:Label>/
                                        <asp:Label ID="lblServicePlansSoldValue" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="ServicePlanSold_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12 pl-lg-0">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="WalletDueForRedemption_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: rgba(217, 186, 28, 0.84);">
                                        <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24" height="35px" width="200px" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M16 12h2v4h-2z"></path>
                                            <path d="M20 7V5c0-1.103-.897-2-2-2H5C3.346 3 2 4.346 2 6v12c0 2.201 1.794 3 3 3h15c1.103 0 2-.897 2-2V9c0-1.103-.897-2-2-2zM5 5h13v2H5a1.001 1.001 0 0 1 0-2zm15 14H5.012C4.55 18.988 4 18.805 4 18V8.815c.314.113.647.185 1 .185h15v10z"></path>
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">Wallet - Due for Redemption (Nos.)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblWalletDueForRedemption" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="WalletDueForRedemption_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12 pl-lg-0">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="InProcess_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: rgba(217, 186, 28, 0.84);">
                                        <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24" height="35px" width="200px" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M16 12h2v4h-2z"></path>
                                            <path d="M20 7V5c0-1.103-.897-2-2-2H5C3.346 3 2 4.346 2 6v12c0 2.201 1.794 3 3 3h15c1.103 0 2-.897 2-2V9c0-1.103-.897-2-2-2zM5 5h13v2H5a1.001 1.001 0 0 1 0-2zm15 14H5.012C4.55 18.988 4 18.805 4 18V8.815c.314.113.647.185 1 .185h15v10z"></path>
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">In Process - Not in Wallet Yet (Nos.)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblInProcess" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="InProcess_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="PlanUnderApproval_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: rgba(217, 186, 28, 0.84)">
                                        <svg stroke="currentColor" fill="none" stroke-width="2" viewBox="0 0 24 24" stroke-linecap="round" stroke-linejoin="round" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"></path>
                                            <polyline points="22 4 12 14.01 9 11.01"></polyline>
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">Plans - Registration Under Approval (Nos.)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblPlansUnderRegistrationApproval" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="PlanUnderApproval_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12  pl-lg-0">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="ApprovedPlan_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: rgb(39, 201, 134);">
                                        <svg stroke="currentColor" fill="none" stroke-width="2" viewBox="0 0 24 24" stroke-linecap="round" stroke-linejoin="round" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M3.85 8.62a4 4 0 0 1 4.78-4.77 4 4 0 0 1 6.74 0 4 4 0 0 1 4.78 4.78 4 4 0 0 1 0 6.74 4 4 0 0 1-4.77 4.78 4 4 0 0 1-6.75 0 4 4 0 0 1-4.78-4.77 4 4 0 0 1 0-6.76Z"></path>
                                            <path d="m9 12 2 2 4-4"></path>
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">Plans - Registration Approved (Nos.)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblPlansSoldApproved" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="ApprovedPlan_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>

                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12 pl-lg-0">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="RejectPlan_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: rgb(201, 43, 39);">
                                        <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 512 512" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M256 90c44.3 0 86 17.3 117.4 48.6C404.7 170 422 211.7 422 256s-17.3 86-48.6 117.4C342 404.7 300.3 422 256 422s-86-17.3-117.4-48.6C107.3 342 90 300.3 90 256s17.3-86 48.6-117.4C170 107.3 211.7 90 256 90m0-42C141.1 48 48 141.1 48 256s93.1 208 208 208 208-93.1 208-208S370.9 48 256 48z"></path>
                                            <path d="M360 330.9L330.9 360 256 285.1 181.1 360 152 330.9l74.9-74.9-74.9-74.9 29.1-29.1 74.9 74.9 74.9-74.9 29.1 29.1-74.9 74.9z"></path>
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">Plans - Registration Rejected (Nos.)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblPlansRejected" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="RejectPlan_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12 pl-lg-0">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="RefundUnderApprovel_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: #5c6bc0">
                                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" fill="White" height="30px">
                                            <path d="M142.9 142.9c-17.5 17.5-30.1 38-37.8 59.8c-5.9 16.7-24.2 25.4-40.8 19.5s-25.4-24.2-19.5-40.8C55.6 150.7 73.2 122 97.6 97.6c87.2-87.2 228.3-87.5 315.8-1L455 55c6.9-6.9 17.2-8.9 26.2-5.2s14.8 12.5 14.8 22.2l0 128c0 13.3-10.7 24-24 24l-8.4 0c0 0 0 0 0 0L344 224c-9.7 0-18.5-5.8-22.2-14.8s-1.7-19.3 5.2-26.2l41.1-41.1c-62.6-61.5-163.1-61.2-225.3 1zM16 312c0-13.3 10.7-24 24-24l7.6 0 .7 0L168 288c9.7 0 18.5 5.8 22.2 14.8s1.7 19.3-5.2 26.2l-41.1 41.1c62.6 61.5 163.1 61.2 225.3-1c17.5-17.5 30.1-38 37.8-59.8c5.9-16.7 24.2-25.4 40.8-19.5s25.4 24.2 19.5 40.8c-10.8 30.6-28.4 59.3-52.9 83.8c-87.2 87.2-228.3 87.5-315.8 1L57 457c-6.9 6.9-17.2 8.9-26.2 5.2S16 449.7 16 440l0-119.6 0-.7 0-7.6z" />
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">Refund Under Approval (Nos.)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblRefundUnderApproval" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="RefundUnderApprovel_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="PendingForPayment_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: rgba(217, 186, 28, 0.84);">
                                        <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                                            <path fill="none" d="M0 0h24v24H0z"></path>
                                            <path d="M20 4H4c-1.11 0-1.99.89-1.99 2L2 18c0 1.11.89 2 2 2h16c1.11 0 2-.89 2-2V6c0-1.11-.89-2-2-2zm0 14H4v-6h16v6zm0-10H4V6h16v2z"></path>
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">Pending for Payment from You (Nos.)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblPendingForPayment" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="PendingForPayment_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12 pl-lg-0">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="UnsoldPlanInCart_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: rgb(221, 106, 29);">
                                        <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                                            <path fill="none" d="M0 0h24v24H0V0z"></path>
                                            <path d="M15.55 13c.75 0 1.41-.41 1.75-1.03l3.58-6.49A.996.996 0 0 0 20.01 4H5.21l-.94-2H1v2h2l3.6 7.59-1.35 2.44C4.52 15.37 5.48 17 7 17h12v-2H7l1.1-2h7.45zM6.16 6h12.15l-2.76 5H8.53L6.16 6zM7 18c-1.1 0-1.99.9-1.99 2S5.9 22 7 22s2-.9 2-2-.9-2-2-2zm10 0c-1.1 0-1.99.9-1.99 2s.89 2 1.99 2 2-.9 2-2-.9-2-2-2z"></path>
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">Unsold Plans in Cart (Nos.)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblUnsoldPlanInCart" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="UnsoldPlanInCart_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12 pl-lg-0">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="MissedSales_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: #5c6bc0;">
                                        <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                                            <path fill="none" d="M0 0h24v24H0V0z"></path>
                                            <path d="M15.55 13c.75 0 1.41-.41 1.75-1.03l3.58-6.49A.996.996 0 0 0 20.01 4H5.21l-.94-2H1v2h2l3.6 7.59-1.35 2.44C4.52 15.37 5.48 17 7 17h12v-2H7l1.1-2h7.45zM6.16 6h12.15l-2.76 5H8.53L6.16 6zM7 18c-1.1 0-1.99.9-1.99 2S5.9 22 7 22s2-.9 2-2-.9-2-2-2zm10 0c-1.1 0-1.99.9-1.99 2s.89 2 1.99 2 2-.9 2-2-.9-2-2-2z"></path>
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">Missed Sales - Not in Cart (Nos.)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblMissedSales" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="MissedSales_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                    <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12 pl-lg-0">
                        <div class="card mb-3 mx-0 mt-0 p-3">
                            <asp:LinkButton runat="server" OnClick="Refund_Click" CssClass="text-decoration-none text-dark">
                                <div class="card-body align-items-center gap-3 p-0">
                                    <div class="float-right icon-circle-medium  icon-box-lg mt-1" style="background: #5c6bc0;">
                                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" fill="White" height="30px">
                                            <path d="M142.9 142.9c-17.5 17.5-30.1 38-37.8 59.8c-5.9 16.7-24.2 25.4-40.8 19.5s-25.4-24.2-19.5-40.8C55.6 150.7 73.2 122 97.6 97.6c87.2-87.2 228.3-87.5 315.8-1L455 55c6.9-6.9 17.2-8.9 26.2-5.2s14.8 12.5 14.8 22.2l0 128c0 13.3-10.7 24-24 24l-8.4 0c0 0 0 0 0 0L344 224c-9.7 0-18.5-5.8-22.2-14.8s-1.7-19.3 5.2-26.2l41.1-41.1c-62.6-61.5-163.1-61.2-225.3 1zM16 312c0-13.3 10.7-24 24-24l7.6 0 .7 0L168 288c9.7 0 18.5 5.8 22.2 14.8s1.7 19.3-5.2 26.2l-41.1 41.1c62.6 61.5 163.1 61.2 225.3-1c17.5-17.5 30.1-38 37.8-59.8c5.9-16.7 24.2-25.4 40.8-19.5s25.4 24.2 19.5 40.8c-10.8 30.6-28.4 59.3-52.9 83.8c-87.2 87.2-228.3 87.5-315.8 1L57 457c-6.9 6.9-17.2 8.9-26.2 5.2S16 449.7 16 440l0-119.6 0-.7 0-7.6z" />
                                        </svg>
                                    </div>
                                    <div class="d-inline-block">
                                        <h5 class="text-muted">Refund (Nos.)</h5>
                                        <h2 class="mb-0">
                                            <asp:Label ID="lblRefund" runat="server" Text="0"></asp:Label>
                                        </h2>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <button class="btn btn-view-details btn-block" runat="server" onserverclick="Refund_Click">View Details <i class="fa fa-arrow-right mr-0">&nbsp;</i></button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
