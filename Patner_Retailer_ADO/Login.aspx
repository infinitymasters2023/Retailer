<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Patner_Retailer_ADO.Login" %>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <title>Retailer</title>
    <link rel="icon" type="image/png" href="../assets/images/Infyshield-logo.png" />
    <link rel="stylesheet" href="../assets/vendor/bootstrap/css/bootstrap.min.css">
    <link href="../assets/vendor/fonts/circular-std/style.css" rel="stylesheet">
    <link rel="stylesheet" href="../assets/libs/css/style.css">
    <link rel="stylesheet" href="../assets/vendor/fonts/fontawesome/css/fontawesome-all.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/css/intlTelInput.css">
    <link href="assets/login.css" rel="stylesheet" />
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
            <asp:Label ID="lblMessage" runat="server" class="mb-2 text-success" />
            <div class="form-group mb-3" style="display: grid">
                <label for="mobile_code" class="form-label">Phone Number</label>
                <input type="tel" id="mobile_code" class="form-control" runat="server" placeholder="Enter Phone Number" name="mobile_code" maxlength="10"
                    oninput="this.value = this.value.replace(/[^0-9]/g, '')"
                    onblur="focusButtonIfTenDigits(this)" />
                <asp:HiddenField ID="hdnCountryCode" runat="server" />
                <asp:HiddenField ID="hdnPhoneNumber" runat="server" />
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="ErrorMessage"></asp:Label>
            </div>
            <div id="divotppanel" runat="server" visible="false">
                <div class="form-group otp-input-group mb-2 mb-lg-0">
                    <label for="txtOTP" class="form-label">Enter OTP</label>
                    <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control otp-input" placeholder="Enter OTP"
                        MaxLength="6" oninput="this.value = this.value.replace(/[^0-9]/g, '').slice(0, 6);"></asp:TextBox>
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
                        <span id="otpTimer" class="text-muted small" style="color: red !important;"></span>
                        <asp:HiddenField ID="hdnOtpExpiry" runat="server" />
                        <asp:LinkButton ID="lnkResendOTP" runat="server" CssClass="resend-otp" OnClick="lnkResendOTP_Click">Resend OTP</asp:LinkButton>
                    </div>
                </div>
                <div class="form-group flex-wrap captcha-group">
                    <label for="txtCaptcha" class="form-label">Captcha</label>
                    <div class="row">
                        <div class="col-6 col-lg-6">
                            <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control captcha-input" placeholder="Enter Captcha" MaxLength="6"></asp:TextBox>
                            <asp:Label ID="lblCaptchaError" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
                        </div>
                        <div class="col-6 col-lg-6 px-0">
                            <div class="d-flex align-items-center">
                                <div class="rounded bg-light captcha-txt">
                                    <asp:Label ID="lblCaptcha" runat="server" Font-Bold="true" Style="font-size: 18px;"></asp:Label>
                                </div>
                                <asp:LinkButton ID="lnkRefreshCaptcha" runat="server" CssClass="text-dark" OnClick="lnkRefreshCaptcha_Click">
                               <i class="fas fa-sync-alt"></i>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:Button ID="btnLogin" runat="server" Text="Get OTP" CssClass="btn btn-primary btn-lg btn-block gradient-btn" OnClick="btnLogin_Click" />
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
            formatOnDisplay: false,
            utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.13/js/utils.js",
        });

        function updateHiddenFields() {
            const dialCode = iti.getSelectedCountryData().dialCode;
            let nationalNumber = iti.getNumber(intlTelInputUtils.numberFormat.NATIONAL).replace(/\D/g, '');
            if (nationalNumber.startsWith('0')) {
                nationalNumber = nationalNumber.substring(1);
            }
            countryCodeHidden.value = dialCode;
            phoneNumberHidden.value = nationalNumber;
        }
        mobileCodeInput.addEventListener('countrychange', updateHiddenFields);
        mobileCodeInput.addEventListener('blur', updateHiddenFields);
        document.addEventListener("DOMContentLoaded", function () {
            const captchaLabel = document.getElementById("lblCaptcha");
            if (captchaLabel) {
                captchaLabel.oncopy = captchaLabel.oncut = captchaLabel.oncontextmenu = function (e) {
                    e.preventDefault();
                    return false;
                };
            }
        });
    </script>
    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            const timerDisplay = document.getElementById("otpTimer");
            const resendBtn = document.getElementById("<%= lnkResendOTP.ClientID %>");
            const expiryRaw = document.getElementById("<%= hdnOtpExpiry.ClientID %>").value;

            if (!expiryRaw) return;

            const expiryTime = new Date(expiryRaw);
            const now = new Date();
            let timeLeft = Math.floor((expiryTime - now) / 1000);
            if (timeLeft <= 0 || isNaN(timeLeft)) {
                timerDisplay.textContent = "OTP expired.";
                resendBtn.disabled = false;
                resendBtn.classList.remove("disabled");
                return;
            }
            resendBtn.disabled = true;
            resendBtn.classList.add("disabled");

            let countdown = setInterval(() => {
                if (timeLeft <= 0) {
                    clearInterval(countdown);
                    timerDisplay.textContent = "OTP expired.";
                    resendBtn.disabled = false;
                    resendBtn.classList.remove("disabled");
                    return;
                }
                const minutes = Math.floor(timeLeft / 60);
                const seconds = timeLeft % 60;
                timerDisplay.textContent = `OTP expires in ${minutes}:${seconds.toString().padStart(2, '0')}`;
                timeLeft--;
            }, 1000);
        });

        function focusButtonIfTenDigits(input) {
            const value = input.value?.trim(); // remove spaces if any
            const errorLabel = document.getElementById("<%= lblErrorMessage.ClientID %>");

            if (!value) return; // if null, undefined, or empty, do nothing

            errorLabel.style.display = 'none';
            errorLabel.innerText = "";

            // Check if length is not 10
            if (value.length !== 10) {
                errorLabel.style.display = 'block';
                errorLabel.innerText = "Phone number must be exactly 10 digits.";
                input.focus();
                return;
            }

            // Check if starts with 0-5
            if (/^[0-5]/.test(value)) {
                errorLabel.style.display = 'block';
                errorLabel.innerText = "Invalid number.";
                input.focus();
                return;
            }

            // Check if all digits are the same (e.g., 9999999999)
            if (/^(\d)\1{9}$/.test(value)) {
                errorLabel.style.display = 'block';
                errorLabel.innerText = "Invalid number.";
                input.focus();
                return;
            }

            // No errors
            errorLabel.style.display = 'none';
        }


    </script>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const form = document.getElementById("form1");
            form.addEventListener("submit", function (e) {
                updateHiddenFields(); // Ensure this runs before submission
            });
        });
    </script>
</body>
</html>
