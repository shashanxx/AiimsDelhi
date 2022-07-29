<%@ Page Title="" Language="C#" MasterPageFile="~/admin/adminMaster.master" AutoEventWireup="true" CodeFile="updatesubjectdata.aspx.cs" Inherits="admin_updatesubjectdata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_body_AdminMaster" runat="Server">
    <div class="col-md-12 pb-3">
        <h3>Update Subject Code & Subject Code PH</h3>
    </div>

    <%--download format--%>
    <div class="col-md-12 pt-3 pb-3">
        <div class="row">
            <div class="col-md-12">
                <asp:Button ID="btnDownloadUpdatedExcelFormat" runat="server" CssClass="btn btn-dark" Text="Download Updated Data Excel Format" OnClick="btnDownloadUpdatedExcelFormat_Click" />
            </div>
        </div>
    </div>
    <div runat="server" visible="false">
        <asp:GridView runat="server" ID="gvFullFormatExcelData"></asp:GridView>
    </div>
    <div class="col-md-12">
        <hr />
    </div>

    <%--upload data--%>
    <div class="col-md-12 pt-3 pb-3">
        <div class="row">
            <div class="col-md-12">
                <asp:Label runat="server" ID="lblfuUpdatedExcelData" AssociatedControlID="fuUpdatedExcelData">Click here to Upload Updated Excel Data: </asp:Label>
            </div>
            <div class="col-md-6">
                <asp:FileUpload ID="fuUpdatedExcelData" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-6">
                <asp:Button ID="btnUpdatedUpload" runat="server" CssClass="btn btn-dark" Text="Upload Updated Data" OnClick="btnUpdatedUpload_Click" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='Please Wait..';" />
            </div>
        </div>
    </div>
    <div class="col-md-12">
        <hr />
    </div>
</asp:Content>

