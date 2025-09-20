<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhonePeTest.aspx.cs" Inherits="Patner_Retailer_ADO.PhonePay.PhonePeTest" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PhonePe Payment</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Enter Amount: 
            <asp:TextBox ID="txtAmount" runat="server" />
            <asp:Button ID="btnSubmit" runat="server" Text="Submit Payment" OnClick="btnSubmit_Click" />
            <br />
            <asp:Label ID="lblResult" runat="server" ForeColor="Green" />
        </div>
    </form>
</body>
</html>
