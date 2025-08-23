<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ReachUs.aspx.cs" Inherits="Patner_Retailer_ADO.ReachUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .supportimg {
            width: 100%;
        }

        .mailcls {
            color: #007bff
        }

            .mailcls:hover {
                color: #007bff;
                text-decoration: underline;
            }

        .office_address {
            box-shadow: 0 0 5px #e4e1e1;
            padding: 1rem;
            background-color: #f7f7f7;
            border-radius: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">

        <div class="supportWrp col-md-12">
            <div class="card p-3">
                <div class="support_sub row d-flex px-0">
                    <div class="support_left col-md-6">
                        <div class="office_address mb-3">
                            <h3>
                                <svg xmlns="http://www.w3.org/2000/svg" width="1.2rem" height="1.2rem" viewBox="0 0 128 128">
                                    <path fill="#006ca2" d="M21.48 13.03h86.74V128H21.48z"></path>
                                    <path fill="#006ca2" d="M19.3 17.3h91.21v79.87H19.3z"></path>
                                    <path fill="#78a3ad" d="M105.83 2.78H22.16c-5.08 0-9.24 4.15-9.24 9.24V128h35.6v-13.36c0-2.83 2.31-5.15 5.14-5.15h20.66c2.84 0 5.16 2.32 5.16 5.15V128h35.6V12.02c-.01-5.08-4.16-9.24-9.25-9.24m-67.68 85.4h-13v-15.7h13zm0-24.96h-13v-15.7h13zm0-24.95h-13v-15.7h13zm21.79 49.91H46.93v-15.7h13.01zm0-24.96H46.93v-15.7h13.01zm0-24.95H46.93v-15.7h13.01zm21.78 49.91h-13v-15.7h13zm0-24.96h-13v-15.7h13zm0-24.95h-13v-15.7h13zm21.79 49.91H90.5v-15.7h13.01zm0-24.96H90.5v-15.7h13.01zm0-24.95H90.5v-15.7h13.01z"></path>
                                </svg>
                                Office Address
                            </h3>
                            <p>
                                <strong>Infinity Assurance Solutions Pvt Ltd</strong>
                                <br>
                                24, US Complex, Apollo Metro Station, 120, NH-19, adjacent to Jasola, Jasola, New Delhi, Delhi 110076
                            </p>
                        </div>
                        <div class="customer_Service">
                            <h3>
                                <svg xmlns="http://www.w3.org/2000/svg" width="1.2rem" height="1.2rem" viewBox="0 0 48 48">
                                    <path fill="#ffb74d" d="M29 43v-4.6l2.6.5c2.9.6 5.6-1.5 5.8-4.4L38 28l2.9-1.2c1-.4 1.4-1.6.8-2.6L38 18c-.6-7.6-4.9-15-16-15C10.6 3 5 11.4 5 20c0 3.7 1.3 6.9 3.3 9.6c1.8 2.5 2.7 5.5 2.7 8.5v4.8h18z"></path>
                                    <path fill="#ff9800" d="M29 43v-4.6L22 37v6z"></path>
                                    <circle cx="33.5" cy="21.5" r="1.5" fill="#784719"></circle>
                                    <path fill="#ff5722" d="M21.4 3C12.3 3 5 10.3 5 19.4c0 11.1 6 11.4 6 18.6l2.6-.9c2.1-.7 3.9-2.3 4.7-4.4l2.8-6.8L27 23v-6s7-3.8 7-10.3C31 4.2 25.7 3 21.4 3"></path>
                                    <path fill="#546e7a" d="M21 2.1c-.6 0-1 .4-1 1V17c0 .6.4 1 1 1s1-.4 1-1V3.1c0-.6-.4-1-1-1m15.9 29.8c-7.9 0-10.3-4.9-10.4-5.1c-.2-.5-.8-.7-1.3-.5c-.5.2-.7.8-.5 1.3c.1.3 3 6.3 12.2 6.3c.6 0 1-.4 1-1s-.5-1-1-1"></path>
                                    <circle cx="37" cy="33" r="2" fill="#37474f"></circle>
                                    <circle cx="21" cy="23" r="7" fill="#37474f"></circle>
                                    <circle cx="21" cy="23" r="4" fill="#546e7a"></circle>
                                </svg>
                                Customer Inquiry
                            </h3>
                            <p>
                                For any customer inquiry or assistance regarding model, price or service issue. 
                            <a href="mailto:support@infyshield.com" class="mailcls">support@infyshield.com</a>
                            </p>
                        </div>
                    </div>
                    <div class="support_right col-md-6">
                        <img src="assets/images/Retailersupport.png" class="supportimg" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
