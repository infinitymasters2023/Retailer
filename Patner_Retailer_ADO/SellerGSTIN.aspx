<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SellerGSTIN.aspx.cs" Inherits="Patner_Retailer_ADO.SellerGSTIN" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Seller GSTIN</title>
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
            flex-direction: column;
            min-height: 100vh;
            padding: 20px;
        }

        .instruction-container {
            max-width: 600px;
            text-align: center;
            background: rgba(255,255,255,0.95);
            padding: 20px;
            border-radius: 10px;
            margin-bottom: 20px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }

        .GSTIN-container {
            min-width: 350px;
            border-radius: 10px;
            background-color: #fff;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            padding: 2rem;
            margin: 20px;
        }

        .GSTIN-header {
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

        .gradient-btn {
            width: 100%;
            border: none;
            border-radius: 5px;
            padding: 0.75rem 1.5rem;
            font-weight: 500;
            font-size: 1rem;
            color: #fff;
            background: linear-gradient(90deg, #7d6fad, #5abec4);
            cursor: pointer;
            transition: background 0.3s ease;
        }

        .gradient-btn:hover {
            background: linear-gradient(90deg, #5abec4, #7d6fad);
        }
    </style>
</head>
<body>
    <!-- Instruction Block -->
    <div class="instruction-container">
        <h3><strong>Seller GSTIN Registration</strong></h3>
        <p style="font-size: 15px; color: #333;">
            Please enter your <strong>GSTIN</strong> if you are a <strong>Dealer or Retailer</strong>.<br />
            If you are an <strong>Influencer, Salesperson, or Individual</strong>, please visit our partner site: <br />
            <a href="https://partners.infyshield.com/" target="_blank">https://partners.infyshield.com/</a> to proceed.
        </p>
    </div>

    <!-- GSTIN Entry Card -->
    <form id="form1" runat="server">
        <div class="GSTIN-container">
            <div class="GSTIN-header">
                <a href="Default.aspx">
                    <img class="logo-img" src="assets/images/logo.png" alt="logo">
                </a>
                <h2 class="mt-3">GSTIN</h2>
            </div>
            <div class="form-group" style="display: grid">
                <label for="lblGSTIN" class="form-label">GSTIN</label>
                <asp:TextBox ID="txtGSTIN" runat="server" MaxLength="15" CssClass="form-control" oninput="validateGSTIN()" autocomplete="off"></asp:TextBox>
                <asp:Label ID="gstinError" Style="color: red;" runat="server"></asp:Label>
            </div>
            <asp:Button ID="btnGSTIN" runat="server" Text="Submit" CssClass="btn btn-primary btn-lg btn-block gradient-btn" OnClick="btnGSTIN_Click" />
        </div>
    </form>

    <!-- GSTIN Validation Script -->
    <script>
        function validateGSTIN() {
            const gstinInput = document.getElementById('<%= txtGSTIN.ClientID %>');
            gstinInput.value = gstinInput.value.toUpperCase();
            const gstin = gstinInput.value.trim().toUpperCase();
            const gstinRegex = /^\d{2}[A-Z]{5}\d{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$/;
            const errorDiv = document.getElementById('gstinError');
            if (gstin && !gstinRegex.test(gstin)) {
                errorDiv.textContent = "Invalid GSTIN format.";
                return false;
            } else {
                errorDiv.textContent = "";
                return true;
            }
        }
    </script>
</body>
</html>
