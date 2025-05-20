<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvalidLink.aspx.cs" Inherits="Patner_Retailer_ADO.InvalidLink" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Link Not Valid</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background-color: #f8f9fc;
        }
        .expired-wrapper {
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        .expired-card {
            max-width: 1000px;
            background: #ffffff;
            border-radius: 15px;
            box-shadow: 0 0 15px rgba(0,0,0,0.1);
            padding: 40px;
        }
        .expired-image {
            max-width: 100%;
        }
        .btn-home {
            background-color: #8364e8;
            color: white;
        }
        .btn-home:hover {
            background-color: #6c4edb;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
       <div class="expired-wrapper">
            <div class="expired-card row g-4 align-items-center">
                <!-- Text Section -->
                <div class="col-md-6 text-center text-md-start">
                    <h2 class="fw-bold">Link is not valid any more</h2>
                    <p class="text-muted mt-2">Oops! This invite URL is not valid anymore.</p>                    
                </div>

                <!-- Image Section -->
                <div class="col-md-6 text-center">
                    <img src="assets/images/LinkExpired.jpg" alt="Link Expired" class="expired-image" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
