<%@ Page Title="Infyshield Sales Dashboard" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Patner_Retailer_ADO.Dashboard" %>
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
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Subtle shadow */
            transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out; /* Smooth transition */
        }

        .card:hover {
            transform: translateY(-5px); /* Slight lift on hover */
            box-shadow: 0 6px 8px rgba(0, 0, 0, 0.15); /* Increased shadow on hover */
        }


        .card-body {
            padding: 20px;
            display: flex; /* Use flexbox for alignment */
            align-items: center; /* Vertically center content */
            justify-content: space-between; /* Space out text and icon */
        }

        .card-body .text-muted {
            font-size: 14px;
            color: #86909e; /* Muted text color */
        }

        .card-body h2 {
            margin-top: 0;
            margin-bottom: 0;
            font-size: 24px; /* Larger font size for the number */
            color: #252d35; /* Very dark blue for emphasis */
        }

        .icon-circle-medium {
            width: 60px; /* Increased size of the circle */
            height: 60px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .icon-box-lg {
            font-size: 24px; /* Larger icon size */
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
                flex-direction: column; /* Stack text and icon vertically */
                text-align: center; /* Center the content */
            }
            .card-body h2{
                margin-bottom: 10px;
            }
            .icon-circle-medium {
                margin-top: 10px; /* Add space above the icon */
            }
        }
        @media (max-width: 576px){
            .page-header h3{
                font-size: 1.5rem;
            }
            .pageheader-text{
                font-size: 0.9rem;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="dashboard-influence">
        <div class="container-fluid dashboard-content">
            <div class="row">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <div class="page-header">
                        <h3 class="my-3">Infyshield Sales Dashboard</h3>
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
            <div class="row">
                <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12">
                    <div class="card mb-3 mx-0 mt-0">
                        <div class="card-body">
                            <div class="d-inline-block">
                                <h5 class="text-muted">Total Sales</h5>
                                <h2 class="mb-0">
                                    <asp:Label ID="lblTotalSales" runat="server" Text="0"></asp:Label>
                                </h2>
                            </div>
                            <div class="float-right icon-circle-medium  icon-box-lg  bg-info-light mt-1">
                                <i class="fa fa-shopping-cart fa-fw fa-sm text-info"></i>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-lg-6 col-md-6 col-sm-12 col-12">
                    <div class="card m-0">
                        <div class="card-body">
                            <div class="d-inline-block">
                                <h5 class="text-muted">Revenue</h5>
                                <h2 class="mb-0">
                                    <asp:Label ID="lblRevenue" runat="server" Text="0"></asp:Label>
                                </h2>
                            </div>
                            <div class="float-right icon-circle-medium  icon-box-lg  bg-primary-light mt-1">
                                 <i class="fa fa-money-bill-alt fa-fw fa-sm text-primary"></i>
                            </div>
                        </div>
                    </div>
                </div>
             
            </div>

           

        </div>
    </div>
</asp:Content>
