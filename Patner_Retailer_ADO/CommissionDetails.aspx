<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CommissionDetails.aspx.cs" Inherits="Patner_Retailer_ADO.CommissionDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        :root {
            --primary-green: #27ae60;
            --secondary-green: #2ecc71;
            --orange: #f39c12;
            --blue: #2980b9;
            --purple: #8e44ad;
            --light-gray: #f9f9f9;
            --text-dark: #333;
            --text-light: #666;
        }

        body, p, h2, h3, h4 {
            margin: 0;
            padding: 0;
        }

            /* ---------- Section Headings ---------- */
            h2.section-title {
                text-align: center;
                margin: 50px 0 10px;
                font-size: 30px;
                font-weight: 700;
                color: var(--text-dark);
                display: flex;
            }

            p.section-subtitle {
                color: var(--text-light);
                margin-bottom: 40px;
                font-size: 16px;
            }

        /* ---------- Base Margin Steps ---------- */
        .base-margin-container {
            display: flex;
            justify-content: center;
            align-items: flex-end;
            gap: 40px;
            flex-wrap: nowrap;
            margin-bottom: 60px;
        }

        .step-box {
            position: relative;
            background: linear-gradient(135deg, #eafaf1, #d7f5e3);
            border: 1px solid #bfe8d0;
            border-radius: 5px;
            padding: 20px;
            width: 160px;
            height: 120px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.12);
            text-align: center;
            transition: transform 0.3s;
            box-shadow: -18px -11px 0px 0px #ed7d31;
        }

            .step-box:hover {
                transform: translateY(-6px);
            }

            .step-box:nth-child(odd) {
                background: linear-gradient(135deg, #d0f0db, #bfe8ce);
            }

        .step-header {
            position: absolute;
            top: -22px;
            left: 50%;
            transform: translateX(-50%) skewX(-15deg);
            background: var(--orange);
            padding: 6px 16px;
            border-radius: 6px;
            color: #fff;
            font-weight: bold;
            font-size: 15px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.2);
        }

            .step-header span {
                transform: skewX(15deg);
                display: inline-block;
            }

        .step-box p {
            margin-top: 42px;
            font-size: 15px;
            font-weight: 600;
            color: var(--text-dark);
        }
        /* stair effect (progressively higher) */
        .base-margin-container .step-box {
            margin-bottom: calc(var(--i) * 25px);
        }

            .base-margin-container .step-box:nth-child(1) {
                --i: 0;
            }

            .base-margin-container .step-box:nth-child(2) {
                --i: 1;
            }

            .base-margin-container .step-box:nth-child(3) {
                --i: 2;
            }

            .base-margin-container .step-box:nth-child(4) {
                --i: 3;
            }

            .base-margin-container .step-box:nth-child(5) {
                --i: 4;
            }

            .base-margin-container .step-box:nth-child(6) {
                --i: 5;
            }

            .base-margin-container .step-box:nth-child(7) {
                --i: 6;
            }

        /* ---------- Info + Banner ---------- */
        .info-box {
            margin-right: 40px;
            padding: 20px 25px;
            border: 2px solid #ddd;
            border-radius: 10px;
            background: var(--light-gray);
            text-align: center;
            max-width: 650px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.08);
        }

            .info-box p {
                margin: 5px 0;
                font-weight: 600;
                font-size: 17px;
            }

            .info-box span {
                font-size: 22px;
                color: var(--blue);
            }

        .note-text {
            font-size: 14px !important;
            color: #000;
            font-weight: 400 !important;
            margin-top: 6px;
        }

        .banner {
            margin: 0;
            display: inline-block;
            padding: 14px 28px;
            background: linear-gradient(135deg, var(--purple), #a56cf7);
            color: #fff;
            border-radius: 12px;
            transform: skewX(-15deg);
            font-weight: bold;
            font-size: 16px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.2);
        }

            .banner span {
                display: inline-block;
                transform: skewX(15deg);
            }

        /* ---------- Promo Box ---------- */
        .promo-box {
            width: 500px;
            max-width: 90%;
            margin: 40px auto;
            border-radius: 15px;
            overflow: hidden;
            box-shadow: 0 8px 20px rgba(0,0,0,0.2);
        }

        .promo-header {
            background: linear-gradient(135deg, var(--blue), #4a69bd);
            color: #fff;
            text-align: center;
            padding: 20px;
        }

            .promo-header h3 {
                margin: 0;
                font-size: 26px;
                font-weight: bold;
                color: #ffeb3b;
            }

            .promo-header p {
                margin: 8px 0 0;
                font-size: 20px;
                font-weight: bold;
            }

        .promo-body {
            background: #eef2fa;
            padding: 20px;
            text-align: center;
        }

            .promo-body h4 {
                margin: 0 0 15px;
                font-size: 18px;
                font-weight: bold;
            }

        .promo-criteria {
            text-align: left;
            display: inline-block;
        }

            .promo-criteria p {
                margin: 8px 0;
                font-size: 16px;
                font-weight: 600;
            }

                .promo-criteria p::before {
                    content: "✦ ";
                    color: #f39c12;
                    font-size: 18px;
                    margin-right: 4px;
                }

        .promo-note {
            font-size: 12px;
            color: var(--text-light);
            margin-top: 10px;
            text-align: right;
        }

        /* ---------- Sales Executive Chart ---------- */
        .se-stepped-chart {
            display: flex;
            justify-content: center;
            align-items: flex-end;
            gap: 40px;
            margin: 50px auto;
        }

        .se-step-box {
            position: relative;
            padding: 15px;
            width: 140px;
            height: 120px;
            border-radius: 8px;
            text-align: center;
            font-weight: bold;
            color: #fff;
            box-shadow: 0 4px 10px rgba(0,0,0,0.15);
        }

            .se-step-box p {
                margin: 42px 0 0;
                font-size: 14px;
                font-weight: 500;
            }

            .se-step-box.blue {
                background: #4a69bd;
                margin-bottom: 40px;
            }

            .se-step-box.green {
                background: #27ae60;
                margin-bottom: 80px;
            }

            .se-step-box.dark-green {
                background: #145a32;
                margin-bottom: 120px;
            }

        .se-step-label {
            position: absolute;
            top: 10px;
            left: 50%;
            transform: translateX(-50%);
            font-size: 16px;
        }

        /* ---------- EW Plan Circles ---------- */
        .ew-circles {
            display: flex;
            justify-content: center;
            align-items: center;
            margin: 60px auto;
            height: 380px;
            position: relative;
            /* top: 31px; */
        }

        .ew-circle1 {
            width: 180px;
            height: 180px;
            border-radius: 50%;
            /* margin-right: -40px; */
            background: rgb(165 195 221 / 50%);
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            color: #ffff00;
            font-weight: 600;
            font-size: 24px;
            text-align: center;
            box-shadow: 0 0px 3px rgb(91 145 191);
            transition: transform 0.3s;
            position: absolute;
            top: 45%;
            left: 25%;
        }

        .ew-circle2 {
            width: 180px;
            height: 180px;
            border-radius: 50%;
            /* margin-right: -40px; */
            background: rgba(143 183 218 / 80%);
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            color: #ffff00;
            font-weight: 600;
            font-size: 24px;
            text-align: center;
            /* box-shadow: 0 4px 12px rgba(0,0,0,0.2); */
            transition: transform 0.3s;
            position: absolute;
            top: 23%;
            left: 37%;
        }

        .ew-circle3 {
            width: 180px;
            height: 180px;
            border-radius: 50%;
            /* margin-right: -40px; */
            background: rgb(58 152 231 / 60%);
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            color: #ffff00;
            font-weight: 600;
            font-size: 24px;
            text-align: center;
            /* box-shadow: 0 4px 12px rgba(0,0,0,0.2); */
            transition: transform 0.3s;
            position: absolute;
            top: 0%;
            left: 49%;
        }

        .ew-circle:hover {
            transform: scale(1.1);
        }

        .ew-text {
            font-size: 14px;
            font-weight: 400;
            margin-top: 6px;
            color: #000;
            line-height: 20px;
            text-align: center;
        }

        /* ---------- Incentive Chart ---------- */
        .incentive-chart {
            display: flex;
            justify-content: center;
            align-items: flex-end;
            gap: 15px;
            margin-bottom: 30px;
            width: 100%;
            margin-top: 60px;
        }

        .incentive-box {
            position: relative;
            padding: 20px 25px;
            border-radius: 8px;
            text-align: left;
            min-width: 250px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            height: 180px;
        }

            .incentive-box.blueBox {
                transform: translateY(0);
                border-left: 15px solid #3a7bd5;
                border-top: 15px solid #3a7bd5;
            }

            .incentive-box.greenBox {
                transform: translateY(-20px);
                border-left: 15px solid #3bd9b3;
                border-top: 15px solid #3bd9b3;
            }

            .incentive-box.darkgreenBox {
                transform: translateY(-40px);
                border-left: 15px solid #336600;
                border-top: 15px solid #336600;
            }

            .incentive-box::before {
                content: '';
                position: absolute;
                top: -15px;
                right: 15px;
                width: 0;
                height: 0;
                border-left: 15px solid transparent;
                border-right: 15px solid transparent;
                border-bottom: 20px solid rgba(255, 255, 255, 0.3); /* Triangle color */
            }

            .incentive-box.blue::before {
                border-bottom-color: #4a90e2;
            }

            .incentive-box.green::before {
                border-bottom-color: #50e3c2;
            }

            .incentive-box.dark-green::before {
                border-bottom-color: #417505;
            }

        .incentive-amount {
            font-size: 2.2em;
            font-weight: bold;
            margin-bottom: 10px;
            letter-spacing: -0.5px;
        }

        .incentive-text {
            font-size: 0.9em;
            line-height: 1.4;
        }

        .total-margin-box {
            background-color: #ffffff;
            border: 2px solid #ddd;
            padding: 15px 25px;
            border-radius: 5px;
            text-align: left;
            margin-top: 30px;
            margin-bottom: 20px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
            width: 100%;
            max-width: 550px;
        }

            .total-margin-box p {
                margin: 0;
                font-size: 0.9em;
                line-height: 1.6;
                color: #333;
            }

            .total-margin-box .main-text {
                font-size: 1.1em;
                font-weight: bold;
                color: #000;
                margin-bottom: 5px;
            }

        .notes-section {
            width: 100%;
            max-width: 900px;
            text-align: left;
            margin-top: 20px;
            font-size: 0.75em;
            color: #555;
            line-height: 1.5;
        }

            .notes-section ul {
                list-style-type: disc;
                padding-left: 20px;
                margin: 0;
            }

            .notes-section li {
                margin-bottom: 5px;
            }

        .happy-selling-graphic {
            position: absolute;
            right: 50px;
            bottom: 50px;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            width: 120px;
            height: 120px;
        }

        .purple-stack {
            position: absolute;
            bottom: 0;
        }

        .purple-stack-item {
            background-color: #9b59b6; /* Purple color */
            border-radius: 5px;
            width: 80px;
            height: 25px;
            position: absolute;
        }

            .purple-stack-item:nth-child(1) {
                transform: rotate(-10deg);
                bottom: 0;
                left: 10px;
                opacity: 0.7;
            }

            .purple-stack-item:nth-child(2) {
                transform: rotate(0deg);
                bottom: 15px;
                left: 0;
                background-color: #a77dc6;
                opacity: 0.85;
            }

            .purple-stack-item:nth-child(3) {
                transform: rotate(5deg);
                bottom: 30px;
                left: -5px;
                background-color: #b18fd0;
                opacity: 1;
            }

        .happy-selling-text {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            color: white;
            font-weight: bold;
            font-size: 0.9em;
            text-align: center;
            z-index: 10;
        }

        .guaranteed-trust {
            position: absolute;
            bottom: -30px;
            right: -10px;
            width: 70px;
            height: auto;
            transform: rotate(10deg);
            opacity: 0.8;
        }

        /* ---------- Responsive ---------- */
        @media (max-width: 768px) {
            .base-margin-container, .se-stepped-chart, .incentive-container {
                flex-direction: column;
                align-items: center;
                margin-bottom: 0px;
            }

            .step-box, .se-step-box {
                margin-bottom: 20px !important;
            }

            .ew-circles {
                flex-direction: column;
                margin: 20px auto;
            }

            .ew-circle {
                margin: 10px 0;
            }
        }

        .section-title-Para {
            font-size: 20px !important;
            font-weight: 400 !important;
        }


        .ncb-box {
            position: relative;
            display: flex;
            flex-direction: column;
            max-width: 300px;
            height: 250px;
            margin: 60px auto 40px auto;
            border: 1px solid #c8d3e6;
            background-color: #eaf1f9;
            box-shadow: 0 4px 15px rgba(0,0,0,0.1);
        }

        .ncb-percent {
            position: absolute;
            top: -40px;
            left: -70px;
            width: 100px;
            height: 100px;
            background: #3498db;
            border-radius: 50%;
            border: 2px solid #2980b9;
            display: flex;
            justify-content: center;
            align-items: center;
            color: #ffeb3b;
            font-size: 36px;
            font-weight: bold;
            box-shadow: 0 4px 10px rgba(0,0,0,0.2);
        }

        .ncb-section {
            flex-grow: 1;
            display: flex;
            justify-content: center;
            align-items: center;
            text-align: center;
            font-size: 28px;
            font-weight: 500;
            color: #000;
            padding: 10px;
            line-height: 32px;
        }

        .ncb-top {
            border-bottom: 1px solid #c8d3e6;
        }

        .ncb-note {
            font-size: 10px;
            color: var(--text-light);
            text-align: center;
            margin-top: 15px;
        }

        .total-margin-box.ncb-margin {
            max-width: 400px;
            /*margin: 40px auto 20px;*/
            text-align: center;
        }

        .stackscard {
            position: relative;
            width: 300px; /* Adjust as needed */
            perspective: 1000px; /* For 3D effect */
        }

            .stackscard .card {
                position: absolute;
                width: 260px; /* Slightly smaller for the back cards */
                height: 80px;
                background-color: #8A2BE2; /* Purple color */
                border-radius: 10px; /* Rounded corners */
                box-shadow: 2px 2px 5px rgba(0,0,0,0.3);
                display: flex;
                justify-content: center;
                align-items: center;
                color: white;
                font-family: sans-serif;
                font-size: 1.2em;
                padding: 10px;
                box-sizing: border-box;
            }

        .card-front {
            z-index: 3;
            transform: rotateY(-10deg) rotateX(5deg) translateX(-5px); /* Adjust for perspective */
            /* For the wavy bottom, you'd need SVG mask or more complex CSS shapes */
        }

        .card-back-1 {
            z-index: 2;
            transform: translateX(15px) translateY(10px) rotateY(-12deg) rotateX(6deg);
            opacity: 0.8; /* Slightly less opaque */
            width: 270px; /* Make back cards slightly smaller */
            height: 115px;
        }

        .card-back-2 {
            z-index: 1;
            transform: translateX(30px) translateY(20px) rotateY(-14deg) rotateX(7deg);
            opacity: 0.6; /* Even less opaque */
            width: 260px;
            height: 110px;
        }

        .card-front span {
            /* Style for your text */
            /*white-space: nowrap;*/
            text-align: center
        }

        .incentive-notes {
            margin-top: 30px;
        }

        @media (max-width:767px) {
            .incentive-chart {
                display: block;
            }

            h2.section-title {
                text-align: center;
                margin: 0px 0 10px;
                font-size: 18px;
                display: block;
            }

            .section-title-Para {
                font-size: 16px !important;
                font-weight: 400 !important;
            }

            p.section-subtitle {
                font-size: 14px;
            }

            .step-box {
                width: 100%;
            }

            .info-box {
                margin-right: 0px;
                margin-bottom: 20px;
            }

                .info-box p {
                    margin: 0;
                    font-weight: 600;
                    font-size: 16px;
                }

            .stackscard .card {
                width: 230px;
            }

            .promo-header h3 {
                font-size: 24px;
            }

            .promo-header p {
                font-size: 15px;
                font-weight: 600;
            }

            .promo-body h4 {
                margin: 0 0 15px;
                font-size: 16px;
                font-weight: 600;
                line-height: 22px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid px-0">
        <div class="card">
            <div class="card-body">

                <!-- Base Margin -->
                <h2 class="section-title">Slab Based Margin &nbsp;<p class="section-title-Para">- in % of Net Sales Value</p>
                </h2>
                <p class="section-subtitle">(Grows with respect to the sales volumes in a Calendar month)</p>
                <div class="base-margin-container">
                    <div class="step-box">
                        <div class="step-header"><span>20%</span></div>
                        <p>Up to 20 Plans</p>
                    </div>
                    <div class="step-box">
                        <div class="step-header"><span>21%</span></div>
                        <p>Up to 25 Plans</p>
                    </div>
                    <div class="step-box">
                        <div class="step-header"><span>22%</span></div>
                        <p>Up to 30 Plans</p>
                    </div>
                    <div class="step-box">
                        <div class="step-header"><span>23%</span></div>
                        <p>Up to 37 Plans</p>
                    </div>
                    <div class="step-box">
                        <div class="step-header"><span>24%</span></div>
                        <p>Up to 45 Plans</p>
                    </div>
                    <div class="step-box">
                        <div class="step-header"><span>25%</span></div>
                        <p>Up to 55 Plans</p>
                    </div>
                    <div class="step-box">
                        <div class="step-header"><span>27%</span></div>
                        <p>Above 56 Plans</p>
                    </div>
                </div>
                <div class="d-lg-flex justify-content-end gap-3">
                    <div class="info-box">
                        <p>This is your First Part of Earnings</p>
                    </div>
                    <%--<div class="banner"><span>Earnings Continue...</span></div>--%>
                    <div class="stackscard">
                        <div class="card card-back-2"></div>
                        <div class="card card-back-1"></div>
                        <div class="card card-front">
                            <span>Earnings Continue...</span>
                        </div>
                    </div>
                </div>
                <!-- Promo -->
                <div class="promo-box">
                    <div class="promo-header">
                        <h3>Sell More!</h3>
                        <p>Earn More + More!</p>
                    </div>
                    <div class="promo-body">
                        <h4>"3%" additional margin when you achieve</h4>
                        <div class="promo-criteria">
                            <p>> 100 Service Plans*</p>
                            <span style="display: flex; justify-content: center; font-weight: 600;">or</span>
                            <p>> Rs.2 Lakhs in Sales Value*</p>
                        </div>
                        <p class="promo-note">(*In a Calendar month)</p>
                    </div>
                </div>
                <%--<div class="info-box">
                <p>... Margin so far: <span>30%*</span></p>
                <p class="note-text">(Base Margin (27%) + Additional Margin (3%))</p>
            </div>
            <div class="banner"><span>Now Additional Margin to Your Sales Executive...</span></div>--%>

                <div class="d-lg-flex justify-content-end gap-3">
                    <div class="info-box">
                        <p>... Margin so far: <span>30%*</span></p>
                        <p class="note-text">(Base Margin (27%) + Additional Margin (3%))</p>
                    </div>
                    <%--<div class="banner"><span>Earnings Continue...</span></div>--%>
                    <div class="stackscard">
                        <div class="card card-back-2"></div>
                        <div class="card card-back-1"></div>
                        <div class="card card-front">
                            <span>Now Additional Margin to Your Sales Executive...</span>
                        </div>
                    </div>
                </div>

                <div class="ncb-box">
                    <div class="ncb-percent">2%</div>
                    <div class="ncb-section ncb-top">No Claim Bonus*</div>
                    <div class="ncb-section">Direct Benefits to the Service Centre</div>
                </div>
                <p class="ncb-note">*NCB: No Claim Bonus, will be due post plan expiry, if no claim is made by customer</p>

                <div class="d-lg-flex justify-content-end mt-4 mb-4">
                    <div class="total-margin-box ncb-margin mt-0 mr-5">
                        <p class="main-text">Total Margin so far: <span>32%*</span></p>
                        <p>(Base Margin (27%) + Additional Margin (3%) + NCB (2%))</p>
                    </div>
                    <%--<div class="banner"><span>Earnings Continue...</span></div>--%>
                    <div class="stackscard">
                        <div class="card card-back-2"></div>
                        <div class="card card-back-1"></div>
                        <div class="card card-front">
                            <span>Earnings not over yet...</span>
                        </div>
                    </div>
                </div>


                <%--<div class="total-margin-box ncb-margin">
                <p class="main-text">Total Margin so far: <span>32%*</span></p>
                <p>(Base Margin (27%) + Additional Margin (3%) + NCB (2%))</p>
            </div>
            <div class="banner"><span>Earnings not over yet...</span></div>--%>
                <!-- Sales Executive -->

                <div class="incentive-chart">
                    <div class="incentive-box blueBox">
                        <div class="incentive-amount">@4%</div>
                        <div class="incentive-text">
                            Up to 10 Plans<br />
                        </div>
                    </div>

                    <div class="incentive-box greenBox">
                        <div class="incentive-amount">@4.5%</div>
                        <div class="incentive-text">
                            11-15 Plans<br />
                        </div>
                    </div>

                    <div class="incentive-box darkgreenBox">
                        <div class="incentive-amount">5%</div>
                        <div class="incentive-text">
                            15+ Plans<br />
                        </div>
                    </div>
                </div>

                <div class="d-lg-flex justify-content-end">
                    <div class="info-box">
                        <p>Total Margin so far: <span>37%*</span></p>
                        <p class="note-text">(Base Margin (27%) + Additional Margin (3%) + SE Base Margin (5%) + NCB (2%))</p>
                    </div>
                    <%--<div class="banner"><span>Earnings Continue...</span></div>--%>
                    <div class="stackscard">
                        <div class="card card-back-2"></div>
                        <div class="card card-back-1"></div>
                        <div class="card card-front">
                            <span>Earnings Continue...</span>
                        </div>
                    </div>
                </div>


                <%--<div class="info-box">
                <p>Total Margin so far: <span>37%*</span></p>
                <p class="note-text">(Base Margin (27%) + Additional Margin (3%) + SE Base Margin (5%) + NCB (2%))</p>
            </div>
            <div class="banner"><span>Earnings Continue...</span></div>--%>

                <!-- EW Circles -->

                <div class="ew-circles">
                    <div class="ew-circle1">
                        <span>3%</span>
                        <div class="ew-text">on sales of
                            <br />
                            2Y EW Plan</div>
                    </div>
                    <div class="ew-circle2">
                        <span>4%</span>
                        <div class="ew-text">on sales of
                            <br />
                            3Y EW Plan</div>
                    </div>
                    <div class="ew-circle3">
                        <span>5%</span>
                        <div class="ew-text">on sales of
                            <br />
                            4Y EW Plan</div>
                    </div>
                </div>

                <div class="d-lg-flex justify-content-end gap-3">
                    <div class="info-box">
                        <p>Total Margins so far: <span>42%*</span></p>
                        <p class="note-text">(Base Margin (27%) + Additional Margin (3%) + SE Margin (5%) + NCB (2%) + EW Margin (5%))</p>
                    </div>
                    <%--<div class="banner"><span>Earnings Continue...</span></div>--%>
                    <div class="stackscard">
                        <div class="card card-back-2"></div>
                        <div class="card card-back-1"></div>
                        <div class="card card-front">
                            <span>Earnings not stopped yet...</span>
                        </div>
                    </div>
                </div>

                <%--<div class="info-box">
                <p>Total Margins so far: <span>42%*</span></p>
                <p class="note-text">(Base Margin (27%) + Additional Margin (3%) + SE Margin (5%) + NCB (2%) + EW Margin (5%))</p>
            </div>
            <div class="banner"><span>Earnings not stopped yet...</span></div>--%>


                <div class="incentive-chart">
                    <div class="incentive-box blueBox">
                        <div class="incentive-amount">Rs.5,999/-</div>
                        <div class="incentive-text">
                            Worth Gift or<br />
                            En-cash for Rs.4,999/-<br />
                            on Sales Value of Rs.1.5 Lakhs+ in a Calendar Month*
   
                        </div>
                    </div>

                    <div class="incentive-box greenBox">
                        <div class="incentive-amount">Rs.10,999/-</div>
                        <div class="incentive-text">
                            Worth Gift or<br />
                            En-cash for Rs.9,999/-<br />
                            on Sales Value of Rs.2.00+ Lakhs in a Calendar Month*
   
                        </div>
                    </div>

                    <div class="incentive-box darkgreenBox">
                        <div class="incentive-amount">Rs.15,999/-</div>
                        <div class="incentive-text">
                            Worth Gift or<br />
                            En-cash for Rs.14,999/-<br />
                            on Sales Value of Rs.2.50+ Lakhs in a Calendar Month*
   
                        </div>
                    </div>
                </div>

                <div class="d-lg-flex justify-content-end gap-3">
                    <div class="info-box">
                        <p>Total Margin now: <span>42%*</span> + <span>~Rs.16k as Incentive</span></p>
                        <p class="note-text">(Base Margin (27%) + Additional Margin (3%)<br>
                            + Sales Executive Margin (5%) + NCB (2%) + Plan based Margin (5%))</p>
                    </div>
                    <%--<div class="banner"><span>Earnings Continue...</span></div>--%>
                    <div class="stackscard">
                        <div class="card card-back-2"></div>
                        <div class="card card-back-1"></div>
                        <div class="card card-front">
                            <span>Happy Selling !</span>
                        </div>
                    </div>
                </div>

                <%--<div class="info-box">
                <p>Total Margin now: <span>42%*</span> + <span>~Rs.16k as Incentive</span></p>
                <p class="note-text">(Base Margin (27%) + Additional Margin (3%)<br>+ Sales Executive Margin (5%) + NCB (2%) + Plan based Margin (5%))</p>
            </div>
             <div class="banner"><span>Happy Selling !</span></div>--%>



                <div class="incentive-notes">
                    <ul>
                        <li>Special Incentive will be announced by Infinity as per value target achieved by individual Promoter/Sales Executive in a calendar Month.</li>
                        <li>Only approved plans will be considered as achievement; excluding all types of cancellation.</li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
