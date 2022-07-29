<%@ Page Title="" Language="C#" MasterPageFile="~/admin/adminMaster.master" AutoEventWireup="true" CodeFile="examdatasummary.aspx.cs" Inherits="admin_examdatasummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_body_AdminMaster" runat="Server">
    <div class="col-md-12 pb-3">
        <h3>Exam Data Summary</h3>
    </div>

    <%--download full exam data--%>
    <div class="col-md-12 pt-3">
        <asp:Button ID="btnDownloadExcel" runat="server" CssClass="btn btn-dark" Text="Download Full Exam Data" OnClick="btnDownloadExcel_Click" />
    </div>
    <div class="col-md-12 pt-3 pb-3">
        <asp:Label runat="server" ID="lblExamDataDetails"></asp:Label>
    </div>
    <div runat="server" visible="false">
        <asp:GridView runat="server" ID="gvFullExcelData"></asp:GridView>
    </div>

    <div class="col-md-12">
        <hr />
    </div>
</asp:Content>
