<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="admin_login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- Required meta tags -->
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap CSS -->
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/styles.css" rel="stylesheet" />

    <title>Aiims Delhi</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">

            <%--login form--%>
            <div class="row mx-auto align-items-center vh-100">
                <div class="col-md-6">
                    <img src="../images/logo.png" alt="Logo of AIIMS Delhi" class="img-fluid d-block pt-5 pb-4 float-end" />
                    <h3 class="float-end text-end pb-5">अखिल भारतीय आयुर्विज्ञान संस्थान, नई दिल्ली<br />
                        All India Institute Of Medical Sciences, New Delhi
                    </h3>
                </div>
                <div class="col-md-6">
                    <div class="card bg-dark w-75">
                        <div class="card-header text-white">
                            <div class="pt-2">
                                <h4>Login</h4>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="form-floating mb-3">
                                <asp:TextBox runat="server" ID="txtUserName" placeholder="User Name" CssClass="form-control"></asp:TextBox>
                                <label for="txtUserName">UserName</label>
                            </div>
                            <div class="form-floating">
                                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" placeholder="Password" CssClass="form-control"></asp:TextBox>
                                <label for="txtPassword">Password</label>
                            </div>
                        </div>
                        <div class="card-footer">
                            <div class="pt-2 pb-2">
                                <asp:Button Text="Login" CssClass="btn btn-light" ID="btnLogin" OnClick="btnLogin_Click" runat="server" UseSubmitBehavior="false" OnClientClick="this.disabled='true'; this.value='Please Wait..';" />
                                <a href="../index.html" class="btn btn-light" role="button">Home</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <%--loader--%>
        <div class='loader-background d-none'>
            <div class='loader'></div>
        </div>
    </form>

    <!-- Bootstrap JS -->
    <script src="../js/bootstrap.bundle.min.js"></script>
</body>
</html>
