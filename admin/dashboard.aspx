<%@ Page Title="" Language="C#" MasterPageFile="~/admin/adminMaster.master" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="admin_dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_body_AdminMaster" runat="Server">
    <div class="col-md-12 pb-3">
        <h3>Upload Excel Data Step - 1</h3>
    </div>

    <%--download format--%>
    <div class="col-md-12 pt-3 pb-3">
        <div class="row">
            <div class="col-md-12">
                <asp:Button ID="btnDownloadExcelFormat" runat="server" CssClass="btn btn-dark" Text="Download Exam Data Excel Format" OnClick="btnDownloadExcelFormat_Click" />
            </div>
        </div>
    </div>
    <div class="col-md-12">
        <hr />
    </div>

    <%--upload data--%>
    <div class="col-md-12 pt-3 pb-3">
        <div class="row">
            <div class="col-md-12">
                <asp:Label runat="server" ID="lblfuExcelData" AssociatedControlID="fuExcelData">Click here to Upload Excel: </asp:Label>
            </div>
            <div class="col-md-6">
                <asp:FileUpload ID="fuExcelData" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-6">
                <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-dark" Text="Upload Data" OnClick="btnUpload_Click" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='Please Wait..';" />
            </div>
        </div>
    </div>
    <div class="col-md-12">
        <hr />
    </div>

    <%--full exam data details--%>
    <div class="col-md-12 pt-3 pb-3">
        <asp:Label runat="server" ID="lblExamDataDetails"></asp:Label>
    </div>
    <div class="col-md-12">
        <hr />
    </div>
</asp:Content>

