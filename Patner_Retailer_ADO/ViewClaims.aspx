<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewClaims.aspx.cs" Inherits="Patner_Retailer_ADO.ViewClaims" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .table-cell {
            width: 200px;
            padding: 5px;
        }

        .label {
            display: block;
            width: 100%;
        }

        .mydatagrid tr th {
            font-size: 13px;
            padding: 5px;
            font-weight: 400;
            white-space: nowrap;
        }

        .mydatagrid tr td {
            padding: 5px;
            font-weight: 400;
        }
        .notes-sty ol {
            margin: 0;
            padding: 0 0 0 26px;
        }
        .notes-sty ol li {
            line-height: 24px;
        }
    
        body {
            margin: 0;
            padding: 0;
            font-family: Arial;
        }

        .modal1 {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .center {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 90px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }

        .center img {
            height: 75px;
            width: 75px;
        }

        .radio-inline {
            display: inline-block;
            padding-left: 40px;
            margin-bottom: 0px;
            font-weight: 400;
            vertical-align: middle;
            cursor: pointer;
        }

        .approve {
            background-color: #0080004f;
        }

        .reject {
            background-color: #ff000066;
        }

        .reject1 {
            background-color: White;
        }

        .hold {
            background-color: #ffff0096;
        }

        .hold1 {
            background-color: White;
        }

        .header {
            background-color: #29b5d2cc;
            font-family: Arial;
            border: none 0px transparent;
            height: 15px;
            text-align: center;
            font-size: 13px;
        }

        .rows:hover {
            font-family: Arial;
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<link href="Css/jquery.multiselect.css" rel="stylesheet" type="text/css" />
    <script src="Css/jquery.multiselect.js" type="text/javascript"></script>
    <link href="CSS/bootstrap-timepicker.min.css" rel="stylesheet" type="text/css" />
    <script src="CSS/bootstrap-timepicker.min.js" type="text/javascript"></script>--%>
    <style>
        .radio-inline {
            display: inline-block;
            padding-left: 25px;
            margin-bottom: 0;
            font-weight: normal;
            vertical-align: middle;
            cursor: pointer;
            font-weight: 800;
            font-size: 14px;
        }

        .hc {
            text-align: center;
        }
    </style>
    <style type="text/css">
        .red {
            color: red;
        }

        .ques {
            border: 0px solid white;
            margin: 5px;
            background-color: #337ab747;
            padding: 15px;
        }

        .btn-info:hover {
            color: #fff;
            background-color: #650d6d;
            border-color: #650d6d;
        }

        .btn-info {
            color: #fff;
            background-color: #650d6d;
            border-color: #650d6d;
        }

        .header {
            font-family: Arial;
            border: none 0px transparent;
            height: 15px;
            text-align: center;
            font-size: 10px;
        }

        .rows {
            background-color: white;
            font-family: Arial;
            font-size: 12px;
            color: #333333;
            min-height: 10px;
            text-align: left;
            border: 1px solid;
            border-color: #e7dbe8;
        }

            .rows:hover {
                background-color: #e7dbe8;
                font-family: Arial;
                color: Black !important;
                text-align: left;
            }

        .table > thead > tr > th, .table > tbody > tr > th, .table > tfoot > tr > th, .table > thead > tr > td, .table > tbody > tr > td, .table > tfoot > tr > td {
            padding: 3px;
            line-height: 1.42857143;
            vertical-align: top;
            border-top: 1px solid #ddd;
        }

        .radio-inline {
            display: inline-block;
            padding-left: 32px;
            margin-bottom: 0px;
            font-weight: 400;
            vertical-align: middle;
            cursor: pointer;
        }

        input[type="radio"], input[type="checkbox"] {
            margin: 6px 8px -1px;
            margin-top: 1px \9;
            line-height: normal;
        }

        .radio-inline {
            display: inline-block;
            padding-left: 25px;
            margin-bottom: 0;
            font-weight: normal;
            vertical-align: middle;
            cursor: pointer;
            font-weight: 800;
            font-size: 14px;
        }

        .hc {
            text-align: center;
        }

        .red {
            color: red;
        }

        .btn-info:hover {
            color: #fff;
            background-color: #650d6d;
            border-color: #650d6d;
        }

        .btn-info {
            color: #fff;
            background-color: #650d6d;
            border-color: #650d6d;
        }

        .header {
            font-family: Arial;
            border: none 0px transparent;
            height: 15px;
            text-align: center;
            font-size: 10px;
        }

        .rows {
            background-color: white;
            font-family: Arial;
            font-size: 12px;
            color: #333333;
            min-height: 10px;
            text-align: left;
            border: 1px solid;
            border-color: #e7dbe8;
        }

            .rows:hover {
                background-color: #ffffff;
                font-family: Arial;
                text-align: left;
            }

        .table > thead > tr > th, .table > tbody > tr > th, .table > tfoot > tr > th, .table > thead > tr > td, .table > tbody > tr > td, .table > tfoot > tr > td {
            padding: 3px;
            line-height: 1.42857143;
            vertical-align: top;
            border-top: 1px solid #ddd;
        }
        .panel-body{
            color: #000;
        }
    </style>
    <%--<script type="text/javascript" src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script> 

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>--%>
    <asp:UpdatePanel ID="up1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnfuupload" />
        </Triggers>
        <ContentTemplate>

            <div class="card p-3 mt-4">
                <div class="panel panel-info pt-0">
                    <div class="panel-body">
                        <div class="form-group mb-0">
                            <div class="">
                                <div id="divcert" runat="server">
                                    <div class="row mb-3">
                                        <div class="col-lg-3">
                                            <asp:Label ID="Label5" runat="server" Text="Ticket No.: "></asp:Label>
                                            <asp:Label ID="Label2" runat="server" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-lg-3">
                                            CoI No.:
                                        <asp:Label ID="lblcertificate" Font-Bold="true" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-lg-3">
                                            Loan No.:
                                        <asp:Label ID="lblloan" Font-Bold="true" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-lg-3">
                                            Claim No.:
                                        <asp:Label ID="lblclamno" Font-Bold="true" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="card">
                                        <table align='left' cellpadding='5' cellspacing='0' class="table table-striped table-bordered">
                                            <tr>
                                                <td>Branch Code / Name :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBranchcode" Font-Bold="true" Style="color: green; font-weight: 600;" runat="server"></asp:Label>
                                                </td>
                                                <td>Branch Emp Code / Name :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBranchName" Font-Bold="true" Style="color: green; font-weight: 600;" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Customer :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblname2" runat="server"></asp:Label>
                                                </td>
                                                <td>Mobile No. :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblmobile" Text="" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Email ID :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblemail" runat="server"></asp:Label>
                                                </td>
                                                <td>Address :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbladdress" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>City:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblcity" runat="server"></asp:Label>
                                                </td>
                                                <td>State:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblsate" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Pincode :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblpin" runat="server"></asp:Label>
                                                </td>
                                                <td>Make &amp; Model :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblbrand" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>IMEI No :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblIMEINo" runat="server"></asp:Label>
                                                </td>
                                                <td>Sum Assured (Rs.) :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSumAssured" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Plan :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblplan" runat="server" Style="color: green; font-weight: 600;"></asp:Label>
                                                </td>
                                                <td>Plan Period :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblplanperiod" runat="server" Style="color: green; font-weight: 600;"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="form-group mb-4">
                                    <div class="">
                                        <table class="mydatagrid" cellspacing="0" rules="all" border="1" id="ctl00_ContentPlaceHolder1_GridView2"
                                            style="width: 100%; border-collapse: collapse;">
                                            <tbody>
                                                <tr class="header" style="color: White;">
                                                    <th scope="col">Sr. No</th>
                                                    <th scope="col">Summary</th>
                                                    <th scope="col">Status</th>
                                                </tr>
                                                <tr class="rows">
                                                    <td>1</td>
                                                    <td><span id="Span1">Estimate</span></td>
                                                    <td>
                                                        <asp:Label ID="lbleststatus" runat="server" Style="font-weight: 600;"></asp:Label></td>
                                                </tr>
                                                <tr class="rows">
                                                    <td>2</td>
                                                    <td><span id="Span2">ASC</span></td>
                                                    <td>
                                                        <asp:Label ID="lblascststus" runat="server" Style="font-weight: 600;"></asp:Label></td>
                                                </tr>
                                                <tr class="rows">
                                                    <td>3</td>
                                                    <td><span id="Span3">Documents </span></td>
                                                    <td>
                                                        <asp:Label ID="lbldocstatus" runat="server" Style="font-weight: 600;"></asp:Label></td>
                                                </tr>
                                                <tr class="rows">
                                                    <td>4</td>
                                                    <td><span id="Span6">Claim Settlement </span></td>
                                                    <td>
                                                        <asp:Label ID="lblClaim_settlement" runat="server" Style="font-weight: 600; color: Green"></asp:Label>
                                                        <br />
                                                        <asp:Label ID="Label1" runat="server" Style="font-weight: 200; color: Red"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="rows">
                                                    <td>5</td>
                                                    <td><span id="Span4">Status </span></td>
                                                    <td>
                                                        <asp:Label ID="lblcallstatus" runat="server" Style="font-weight: 600; color: Green"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="divrem2" runat="server" visible="false" class="rows">
                                                    <td>6</td>
                                                    <td><span id="Span7">Remarks </span></td>
                                                    <td>
                                                        <asp:Label ID="Label9" runat="server" Style="font-weight: 600; color: Green"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="divrem1" runat="server" visible="false" class="rows">
                                                    <td>7</td>
                                                    <td><span id="Span5">Remarks </span></td>
                                                    <td>
                                                        <asp:Label ID="lblremarks" runat="server" Style="font-weight: 600; color: Green"></asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="">
                                        <table align='left' cellpadding='5' cellspacing='0' class="table table-bordered table-striped mb-3">
                                            <tr>
                                                <td>Problem Reported / Voice of Customer (VoC):</td>
                                                <td style="width: 600px;">
                                                    <asp:Label ID="lblproblemreported" runat="server"></asp:Label></td>
                                                <td>Type of Damage :</td>
                                                <td>
                                                    <asp:Label ID="lbltypeofdamage" Text="" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>Is your device switching on :</td>
                                                <td>
                                                    <asp:Label ID="lbldeviceswitchon" runat="server"></asp:Label></td>
                                                <td>Parts is Damaged :</td>
                                                <td>
                                                    <asp:Label ID="lblpartdamaged" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>Place of Damage:</td>
                                                <td>
                                                    <asp:Label ID="lblplaceofdamage" runat="server"></asp:Label></td>
                                                <td>Touch Screen Working:</td>
                                                <td>
                                                    <asp:Label ID="lbltouchscreenworking" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>Damage Date / Time :</td>
                                                <td>
                                                    <asp:Label ID="lbldamagedatae" runat="server"></asp:Label></td>
                                                <td></td>
                                                <td>
                                                    <asp:Label ID="lblclaimreporteddate" runat="server"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div id="divproblemupdate" runat="server" visible="false">
                                    <div class="form-group">
                                        <div>
                                            <asp:Label ID="lblsc" runat="server" Style="color: Red" Text="Please Submit Below Information"></asp:Label>
                                        </div>
                                        <table align='left' cellpadding='5' cellspacing='0' class="table table-bordered table-striped">
                                            <tr>
                                                <td>Problem Reported :</td>
                                                <td style="width: 300px;">
                                                    <asp:TextBox ID="txtpreported" Style="width: 303px; height: 43px; min-width: 303px; max-width: 313px; max-height: 50px;"
                                                        TextMode="MultiLine" runat="server"></asp:TextBox>
                                                </td>
                                                <td>Type of Damage :</td>
                                                <td>
                                                    <asp:RadioButtonList CssClass="inline-rb" ID="rbltypeofdamage" RepeatDirection="Horizontal"
                                                        Style="font-size: 10px; width: 230px" runat="server">
                                                        <asp:ListItem Text="Physical" Value="Physical"></asp:ListItem>
                                                        <asp:ListItem Text="Liquid" Value="Liquid"></asp:ListItem>
                                                        <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Is your device switching on :</td>
                                                <td>
                                                    <asp:RadioButtonList CssClass="inline-rb" ID="rblphoneswitchingonornot" RepeatDirection="Horizontal"
                                                        Style="font-size: 10px; width: 170px" runat="server">
                                                        <asp:ListItem Text="Yes" Value="Yes" Enabled="true"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>Parts is Damaged :</td>
                                                <td>
                                                    <asp:CheckBoxList CssClass="inline-rb" ID="rblpartdamage" RepeatColumns="3" Style="font-size: 10px; width: 290px"
                                                        RepeatDirection="Horizontal" runat="server">
                                                        <asp:ListItem Text="Screen" Value="Screen"></asp:ListItem>
                                                        <asp:ListItem Text="Camera" Value="Camera"></asp:ListItem>
                                                        <asp:ListItem Text="Touch Screen" Value="Touch Screen"></asp:ListItem>
                                                        <asp:ListItem Text="Button" Value="Button"></asp:ListItem>
                                                        <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Place of Damage:</td>
                                                <td>
                                                    <asp:TextBox ID="txtplacedamage" runat="server"></asp:TextBox></td>
                                                <td>Touch Screen Working:</td>
                                                <td>
                                                    <asp:RadioButtonList ID="rbltouchworking" CssClass="inline-rb" RepeatColumns="3"
                                                        Style="font-size: 10px; width: 170px; margin-top: 8px;" RepeatDirection="Horizontal"
                                                        runat="server">
                                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Damage Date :</td>
                                                <td>
                                                    <asp:TextBox ID="txtddate" runat="server" AutoCompleteType="Disabled" AutoComplete="off"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="MM/dd/yyyy" TargetControlID="txtddate"></cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="txtddate"
                                                        ValidationGroup="log" ErrorMessage="" runat="Server"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>Damage Time :</td>
                                                <td>
                                                    <asp:TextBox ID="txtdtime" data-provide="timepicker" placeholder="Damage Time" runat="server"
                                                        autocomplete="off" CssClass="timepicker" MaxLength="12">
                                                    </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ControlToValidate="txtdtime"
                                                        ValidationGroup="log" ErrorMessage="" runat="Server"> </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <td>
                                                <td colspan="5">
                                                    <asp:Button ID="btnsave" runat="server" OnClick="btnsave_click" Text="Save" class="btn btn-success" />
                                                </td>
                                            </td>
                                        </table>
                                    </div>
                                </div>

                                <div class="mt-0">
                                    <h3 class="mb-0" style="text-align:center;">Information Submission - Please Click on the Relevant Weblink (POPUP should not be blocked)</h3>
                                </div>

                                <div id="divlinkgn" runat="server">

                                    <div class="">
                                        
                                        <table align='left' cellpadding='5' cellspacing='0' class="table table-bordered table-striped mt-3 mb-3">
                                            <tr>
                                                <td>Claim Form</td>
                                                <td>
                                                    <a runat="server" id="A1" style="margin-left: 3px;" target="_blank"></a>
                                                    <asp:Label ID="Label4" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Bank Details for Reimbursement / Claim Settlement Amount</td>
                                                <td>
                                                    <a runat="server" id="A2" style="margin-left: 3px;" target="_blank"></a>
                                                    <asp:Label ID="Label6" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Make Excess Charge Payment</td>
                                                <td>
                                                    <a runat="server" id="A4" style="margin-left: 3px;" target="_blank"></a>
                                                    <asp:Label ID="Label7" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Consent Form for Device Pickup Request</td>
                                                <td>
                                                    <a runat="server" id="A3" style="margin-left: 3px;" target="_blank"></a>
                                                    <asp:Label ID="Label8" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                </div>
                                <asp:Panel ID="Panelc" runat="server">
                                    <div class="form-group">
                                        <div class="">
                                            <div class="notes-sty">
                                                <h4 class="mb-0">Please note:</h4>
                                                <ol>
                                                    <li>
                                                        Select CORRECT DOCUMENT TITLE to Submit Supporting DOCUMENT / IMAGE FILES
                                                    </li>
                                                     <li>
                                                         Do not submit MULTIPLE IMAGES or MULTIPLE DOCUMENTS in a single file.
                                                     </li>
                                                    <li>
                                                         Each Document / Image file should be attached separately to avoid rejection.
                                                     </li>
                                                    <li>
                                                         Submit only VERY GOOD QUALITY DOCUMENTS / IMAGES which are readable and relevant to the claim.
                                                     </li>
                                                </ol>
                                           
                                            </div>
                                            <hr />
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <label>
                                                        <asp:DropDownList ID="ddldocumentattached2" runat="server" CssClass="form-control ">
                                                        </asp:DropDownList>
                                                    </label>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:FileUpload ID="fupupload2" runat="server" CssClass="form-control input-sm" />
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Button ID="btnfuupload" OnClick="UploadImage1" runat="server" CssClass="btn btn-success"
                                                        Text="Submit" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group mb-0">
                                        <div class="">
                                            <asp:GridView ID="GridView2" Visible="true" OnRowCommand="gvFiles_RowCommand" Width="100%"
                                                OnRowDataBound="ChangeColour" AutoGenerateColumns="false" runat="server" RowStyle-CssClass="rows"
                                                HeaderStyle-CssClass="header" CssClass="mydatagrid">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr. No.">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex+ 1 %>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false" HeaderText="View">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkview" Enabled='<%#Bind("ststus") %>' ForeColor=" green" ToolTip="View Uploaded Documents"
                                                                Width="40px" CommandName="ViewDOC" Text="View DOC" CommandArgument='<%#Eval("DocumentPath") %>'
                                                                runat="server"> <i class="fa fa-share" aria-hidden="true" style="font-size: 28px;"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="View" Visible="true">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkopen" Visible='<%#Bind("ststus") %>' ForeColor="green" ToolTip="Open Uploaded Documents in tab"
                                                                Width="40px" CommandName="ViewDOCopen" CommandArgument='<%# Eval("DocumentPath") %>'
                                                                runat="server">
                                                                <i id="seys" runat="server" class="fa fa-eye" aria-hidden="true" style="font-size: 16px;"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false" HeaderText="Ticket No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblticketno" runat="server" Text='<%#Bind("TicketNo") %>'></asp:Label>
                                                            <asp:Label ID="lblid" runat="server" Text='<%#Bind("mid") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Document Title">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldocumentname" runat="server" Width="120px" Text='<%#Bind("DocumentName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="320px" Wrap="false" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false" HeaderText="Document Attached">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDocumentPath" runat="server" Text='<%#Bind("DocumentPath") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Uploaded Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcreateddate" Width="125px" runat="server" Text='<%#Bind("CreateDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Uploaded By" Visible="true">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbluoloadedby" Width="75px" runat="server" Text='<%#Bind("CreatedBy") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldocremarks" runat="server" Text='<%#Bind("docremarks") %>' Width="250px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="250px" Wrap="true" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" Visible="true">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblverify" Width="150px" runat="server" Text='<%#Bind("DocStatus") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false" HeaderText="Request To Ignore">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txthold" TextMode="MultiLine" MaxLength="200" class="form-control input-sm"
                                                                Text="" runat="server"></asp:TextBox>
                                                            <div style="margin-left: 78px;">
                                                                <asp:LinkButton ID="lnkdelete" ToolTip="Ignore Uploaded Documents" Width="40px" Text="View DOC"
                                                                    OnClick="Deletefile" OnClientClick="return confirm('Are you sure you want to Request To Ignore this file?');"
                                                                    runat="server"> <i class="fa fa-eraser" aria-hidden="true" style="font-size: 21px;color: red;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle ForeColor="White" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="col-md-12">
                                    <asp:Label ID="lbldoccontrolmsg" Style="color: Red; font-size: 20px; background: yellow;"
                                        runat="server"></asp:Label>
                                </div>
                                <asp:Button Text="Select All" ID="Button1" CssClass="btn-info btn-sm" Visible="false"
                                    OnClick="checkall" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>



        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <%-- <cc1:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />--%>
    <script type="text/javascript" language="javascript">
        Sys.UI.Point = function Sys$UI$Point(x, y) {

            x = Math.round(x);
            y = Math.round(y);

            var e = Function._validateParams(arguments, [
                { name: "x", type: Number, integer: true },
                { name: "y", type: Number, integer: true }
            ]);
            if (e) throw e;
            this.x = x;
            this.y = y;
        }
    </script>
</asp:Content>
