<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Feedback.aspx.cs" Inherits="Patner_Retailer_ADO.Feedback" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .bg-blue {
            background: #63a9be !important;
            border: 1px solid #63a9be !important;
        }

        .submit_btn2 {
            background: #000 !important;
        }

        .submit_btn, .submit_btn2 {
            color: #fff !important;
            font-size: 13px;
            letter-spacing: .5px;
        }

        .submit_btn {
            background: #7b17c5 !important;
        }
        .generate-url-button{
            gap:5px;
        }

    </style>
    <script>
        setTimeout(function () {
            const rows = document.querySelectorAll('#ContentPlaceHolder1_GVFeedback_wrapper .row');
            if (rows.length > 1) {
                rows[1].classList.add('table-responsive');
            }
        }, 500);
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="card">
        <div class="card-body">
            <h4 class="filter-txt">Feedback</h4>
            <div class="row justify-content-start mb-3">
                <button id="btnAddFeedback" runat="server" disabled="disabled" class="btn btn-info generate-url-button d-flex bg-blue" onserverclick="btnAddFeedback_ServerClick">
                    <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24" height="18" width="18" xmlns="http://www.w3.org/2000/svg">
                        <path fill="none" d="M0 0h24v24H0V0z"></path>
                        <path d="M20 2H4c-1.1 0-1.99.9-1.99 2L2 22l4-4h14c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2zm0 14H5.17l-.59.59-.58.58V4h16v12zm-9-4h2v2h-2zm0-6h2v4h-2z"></path>
                    </svg>Write Feedback
                </button>
            </div>
            <div id="feedbackSection" runat="server" visible="false">
                <div class="form-group">
                    <textarea class="form-control" rows="4" placeholder="Write your feedback here..." runat="server" id="txtFeedback"></textarea>
                </div>
                <div class="mt-2 d-flex justify-content-center">
                    <button type="submit" id="btnCancelFeedback" class="submit_btn2 btn mr-3" onserverclick="btnCancelFeedback_Click" runat="server"><i class="far fa-times-circle mr-1"></i>Cancel</button>
                    <button type="submit" id="btnSubmitFeedback" class="submit_btn btn" runat="server" onserverclick="btnSubmitFeedback_Click"><i class="fas fa-paper-plane mr-1"></i>Submit</button>
                </div>
            </div>
            <div class="mt-3">
                <asp:GridView ID="GVFeedback" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true" HeaderStyle-CssClass="thead" EmptyDataText="No records available. Please refine your search.">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No.">
                            <ItemTemplate>
                                <asp:Label ID="lblSerial" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Feedback" HeaderText="Feedback" />
                        <asp:BoundField DataField="SubmitDate" HeaderText="Submit Date" />
                        <asp:BoundField DataField="Action" HeaderText="Action" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

