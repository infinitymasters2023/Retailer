<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Patner_Retailer_ADO.Login" %>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Login</title>
    <link rel="stylesheet" href="../assets/vendor/bootstrap/css/bootstrap.min.css">
    <link href="../assets/vendor/fonts/circular-std/style.css" rel="stylesheet">
    <link rel="stylesheet" href="../assets/libs/css/style.css">
    <link rel="stylesheet" href="../assets/vendor/fonts/fontawesome/css/fontawesome-all.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/css/intlTelInput.css">
    <style>
        body {
            background: url(../assets/images/BackCover.png) no-repeat;
            background-size: cover;
            display: flex;
            align-items: center;
            justify-content: center;
            min-height: 100vh; /* Ensure the background covers the entire viewport */
        }

        .login-container {
            min-width: 350px; /* Increased width for better spacing */
            border-radius: 10px;
            background-color: #fff;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            padding: 2rem;
            margin: 20px; /* Add some margin around the container */
        }

        .login-header {
            text-align: center;
            margin-bottom: 1.5rem;
        }

        .logo-img {
            max-width: 150px;
            height: auto;
        }

        .form-group {
            margin-bottom: 1.5rem;
        }

        .otp-input-group {
            position: relative;
        }

        .otp-input {
            padding-right: 3rem; /* Space for the lock icon */
        }

        .otp-icon {
            position: absolute;
            right: 10px;
            top: 50%;
            transform: translateY(-50%);
            z-index: 10;
            color: #777;
        }

        .resend-otp {
            cursor: pointer;
            color: #5abec4;
        }

        .captcha-group {
            display: flex;
            gap: 0.5rem;
            align-items: center;
        }

        .captcha-input {
            flex-grow: 1;
        }

        .gradient-btn {
            width: 100%;
            border: none;
            border-radius: 5px;
            padding: 0.75rem 1.5rem; /* Slightly increased padding */
            font-weight: 500;
            font-size: 1rem; /* Adjusted font size */
            color: #fff;
            background: linear-gradient(90deg, #7d6fad, #5abec4);
            cursor: pointer;
            transition: background 0.3s ease;
        }

            .gradient-btn:hover {
                background: linear-gradient(90deg, #5abec4, #7d6fad);
            }

        .card-footer {
            text-align: center;
            padding-top: 1rem;
            border-top: 1px solid #eee;
        }

        .footer-link {
            color: #5abec4;
            text-decoration: none;
        }

            .footer-link:hover {
                text-decoration: underline;
            }

        .error-message {
            color: red;
            margin-top: 0.5rem;
            display: block;
        }

        /* Styles for OTP display (if you choose to display it) */
        .otp-display-group {
            display: flex;
            gap: 0.5rem;
            margin-bottom: 1.5rem;
        }

        .otp-digit {
            flex: 1;
            text-align: center;
            padding: 0.5rem;
            border: 1px dashed #ccc;
            border-radius: 5px;
            font-weight: bold;
            letter-spacing: 0.5rem;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="login-header">
                <a href="Default.aspx">
                    <img class="logo-img" src="assets/images/logo.png" alt="logo">
                </a>
                <h2 class="mt-3">Login</h2>
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />

            <div class="form-group" style="display: grid">
                <label for="mobile_code" class="form-label">Phone Number</label>
                <input type="tel" id="mobile_code" class="form-control" runat="server" placeholder="Enter Phone Number" name="mobile_code">
                <asp:HiddenField ID="hdnCountryCode" runat="server" />
                <asp:HiddenField ID="hdnPhoneNumber" runat="server" />
            </div>
            <div id="divotppanel" runat="server" visible="false">
                <div class="form-group otp-input-group">
                    <label for="txtOTP" class="form-label">OTP</label>
                    <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control otp-input" placeholder="Enter OTP" MaxLength="6"></asp:TextBox>
                    <span class="otp-icon">
                        <svg stroke="currentColor" fill="none" stroke-width="2" viewBox="0 0 24 24" stroke-linecap="round" stroke-linejoin="round" height="24" width="24" xmlns="http://www.w3.org/2000/svg">
                            <path d="M5 13a2 2 0 0 1 2 -2h10a2 2 0 0 1 2 2v6a2 2 0 0 1 -2 2h-10a2 2 0 0 1 -2 -2z"></path>
                            <path d="M8 11v-4a4 4 0 1 1 8 0v4"></path>
                            <path d="M15 16h.01"></path>
                            <path d="M12.01 16h.01"></path>
                            <path d="M9.02 16h.01"></path>
                        </svg>
                    </span>
                    <div class="d-flex justify-content-between mt-2">
                        <span></span>
                        <asp:LinkButton ID="lnkResendOTP" runat="server" CssClass="resend-otp" OnClick="lnkResendOTP_Click">Resend OTP</asp:LinkButton>
                    </div>
                </div>

                <div class="form-group captcha-group">
                    <label for="txtCaptcha" class="form-label">Captcha</label>
                    <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control captcha-input" placeholder="Enter Captcha"></asp:TextBox>
                    <div class="rounded bg-light p-2" style="border: 1px solid #ccc;">
                        <asp:Label ID="lblCaptcha" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                    </div>
                    <asp:LinkButton ID="lnkRefreshCaptcha" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="lnkRefreshCaptcha_Click">
                       <i class="fas fa-sync-alt"></i>
                    </asp:LinkButton>
                </div>
            </div>
            <asp:Button ID="btnLogin" runat="server" Text="Get OTP" CssClass="btn btn-primary btn-lg btn-block gradient-btn" OnClick="btnLogin_Click" />

            <div class="card-footer">
                Don't have an account yet?
                <a href="CreateAnAccount.aspx" class="footer-link">Register</a>
            </div>
        </div>
    </form>

    <script src="../assets/vendor/jquery/jquery-3.3.1.min.js"></script>
    <script src="../assets/vendor/bootstrap/js/bootstrap.bundle.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/js/intlTelInput.min.js"></script>
    <script>
        const mobileCodeInput = document.querySelector("#mobile_code");
        const countryCodeHidden = document.querySelector("#hdnCountryCode");
        const phoneNumberHidden = document.querySelector("#hdnPhoneNumber");

        const iti = window.intlTelInput(mobileCodeInput, {
            initialCountry: "in",
            separateDialCode: true,
            utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/js/utils.js",
        });

        mobileCodeInput.addEventListener('countrychange', function () {
            countryCodeHidden.value = iti.getSelectedCountryData().dialCode;
            phoneNumberHidden.value = iti.getNumber();
        });

        mobileCodeInput.addEventListener('blur', function () {
            phoneNumberHidden.value = iti.getNumber();
        });
    </script>
</body>

</html>
