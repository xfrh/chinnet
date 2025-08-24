<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Config.aspx.cs" Inherits="CodematicWEB.Config" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- 上述3个meta标签*必须*放在最前面，任何其他内容都*必须*跟随其后！ -->

    <title>设置 | 自动生成</title>


    <!-- Bootstrap -->
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="CSS/beyond.min.css" rel="stylesheet" />
    <link href="CSS/font-awesome.min.css" rel="stylesheet" />
    <link href="CSS/Style.css" rel="stylesheet" />
    <script src="JS/skins.min.js"></script>

    <!--[if lt IE 9]>
      <script src="http://cdn.bootcss.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="http://cdn.bootcss.com/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="container" style="margin-top: 30px; margin-bottom: 30px;">
            <!-- 数据库设置 start -->
            <div class="row">
                <div class="col-lg-12 col-sm-12 col-xs-12">
                    <div class="widget">
                        <div class="widget-header bg-blue">
                            <i class="widget-icon fa  fa-th"></i>
                            <span class="widget-caption f-16">数据库设置</span>
                            <div class="widget-buttons">
                                <a href="#" data-toggle="maximize">
                                    <i class="fa fa-expand"></i>
                                </a>
                                <a href="#" data-toggle="collapse">
                                    <i class="fa fa-minus"></i>
                                </a>
                            </div>
                        </div>
                        <div class="widget-body" style="height: auto;">
                            <asp:UpdatePanel ID="DataBaseUpdatePanel" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="form-horizontal form-bordered" role="form">
                                            <div class="form-group">
                                                <label for="inputEmail3" class="col-sm-2 control-label no-padding-right">链接字符串</label>
                                                <div class="col-sm-8">
                                                    <input type="text" class="form-control txtConnection" id="txtConnection" runat="server" placeholder="完整数据库链接字符串，sa 用户或者是Windows验证">
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputPassword3" class="col-sm-2 control-label no-padding-right">数据库版本</label>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="ddlDatabaseVersion" class="form-control" runat="server">
                                                        <asp:ListItem Text="2000">Sql Server 2000</asp:ListItem>
                                                        <asp:ListItem Text="2005">Sql Server 2005 及以上版本</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputPassword3" class="col-sm-2 control-label no-padding-right">数据库</label>
                                                <div class="col-sm-6">
                                                    <asp:DropDownList ID="ddlDatabase" class="form-control" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Button ID="BtnLoadDatabase" runat="server" class="btn btn-palegreen" Text="加载数据库" OnClick="BtnLoadDatabase_Click" />
                                                </div>
                                            </div>
                                            <div runat="server" id="SetDatabaseError"></div>
                                        </div>
                                    </div>
                                    <div class="row p-all50">
                                        <div class="bs-callout bs-callout-warning" id="Div1">
                                            <p>1、链接字符串为完整数据并且需要sa权限或者Windows验证</p>
                                            <p>2、数据库版本为2005及以上版本包括：2008、2012等等</p>
                                            <p>3、设置完成请点击“加载数据库”来获取数据库列表</p>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>

            </div>
            <!-- 数据库设置 end -->

            <!-- 选择模版 start -->
            <div class="row">
                <div class="col-lg-12 col-sm-12 col-xs-12">
                    <div class="widget">
                        <div class="widget-header bg-palegreen">
                            <i class="widget-icon fa fa-file-o"></i>
                            <span class="widget-caption f-16">选择模版</span>
                            <div class="widget-buttons">
                                <a href="#" data-toggle="maximize">
                                    <i class="fa fa-expand"></i>
                                </a>
                                <a href="#" data-toggle="collapse">
                                    <i class="fa fa-minus"></i>
                                </a>
                            </div>
                        </div>
                        <div class="widget-body template-box" style="height: auto;">
                            <div class="row">
                                <asp:Repeater ID="rptTemplateList" runat="server">
                                    <ItemTemplate>
                                        <div class="col-sm-3  ">
                                            <div class="temp-iamge template-item" item-id='<%#Eval("Id") %>' style='<%#Eval("Id").ToString().Equals(this.TemplateId)?"border:2px solid #E46F61;": "" %>'>
                                                <p>
                                                    <img src='<%#Eval("Image") %>' alt="" />
                                                </p>
                                                <p><%#Eval("Name") %></p>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="row p-all50">
                                <div class="bs-callout bs-callout-warning" id="jquery-required">
                                    <p>请注意： 请根据当前项目所使用的后台选择对应模版，不可随意选择</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <!-- 选择模版 end -->

            <!-- 命名空间设置 start -->
            <div class="row">
                <div class="col-lg-12 col-sm-12 col-xs-12 ">
                    <div class="widget">
                        <div class="widget-header bg-blueberry">
                            <i class="widget-icon fa  fa-th-large"></i>
                            <span class="widget-caption f-16">命名空间设置</span>
                            <div class="widget-buttons">
                                <a href="#" data-toggle="maximize">
                                    <i class="fa fa-expand"></i>
                                </a>
                                <a href="#" data-toggle="collapse">
                                    <i class="fa fa-minus"></i>
                                </a>
                            </div>
                        </div>
                        <div class="widget-body" style="height: auto;">
                        </div>
                    </div>
                </div>

            </div>
            <!-- 命名空间设置 end -->

            <!-- 其它设置 start -->
            <div class="row">
                <div class="col-lg-12 col-sm-12 col-xs-12">
                    <div class="widget">
                        <div class="widget-header bg-lightred">
                            <i class="widget-icon fa  fa-cog"></i>
                            <span class="widget-caption f-16">其它设置</span>
                            <div class="widget-buttons">
                                <a href="#" data-toggle="maximize">
                                    <i class="fa fa-expand"></i>
                                </a>
                                <a href="#" data-toggle="collapse">
                                    <i class="fa fa-minus"></i>
                                </a>
                            </div>
                        </div>
                        <div class="widget-body" style="height: auto;">
                            <div class="row">
                                <div class="form-horizontal form-bordered" role="form">
                                    <div class="form-group">
                                        <label for="txtSolutionName" class="col-sm-2 control-label no-padding-right">解决方案名称</label>
                                        <div class="col-sm-8">
                                            <input type="text" class="form-control txtConnection" id="txtSolutionName" runat="server" placeholder="被生成项目的解决方案名称所，例：TestProject">
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="txtSolutionName" class="col-sm-2 control-label no-padding-right">Form表单Id</label>
                                        <div class="col-sm-8">
                                            <input type="text" class="form-control txtConnection" id="txtFromId" value="form1" runat="server" placeholder="Form表单的Id，用于前台JS验证">
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="txtSolutionName" class="col-sm-2 control-label no-padding-right">控件Id前缀</label>
                                        <div class="col-sm-8">
                                            <input type="text" class="form-control txtConnection" id="txtControlId" value="" runat="server" placeholder="控件的id前缀，如果是母版页这里需要设置，例如：ctl00$ContentPlaceHolder1$">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <!-- 其它设置 end -->

            <!-- 保存设置 start -->
            <div class="row">
                <div class="col-lg-12 col-sm-12 col-xs-12">
                    <div class="widget" style="text-align: center;">
                        <asp:Button ID="BtnClearSetting" runat="server" class="btn  btn-darkorange btn-lg" Text="清除设置" OnClick="BtnClearSetting_Click" OnClientClick="return confirm('确认清除所有的设置吗？')" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="BtnSaveSetting" runat="server" class="btn btn-blue btn-lg" Text="保存设置" OnClick="BtnSaveSetting_Click" OnClientClick="return SaveSetting()" />
                    </div>
                    <div class="col-sm-offset-2 col-sm-8" runat="server" id="SaveError" style="display: none;">

                        <div class="alert alert-danger fade in radius-bordered alert-shadowed" id="SaveErrorMessage" runat="server"></div>
                    </div>
                </div>

            </div>
            <!-- 保存设置 end -->

        </div>
        <asp:HiddenField ID="hfTemplateId" runat="server" />

    </form>

    <script src="http://cdn.bootcss.com/jquery/1.11.2/jquery.min.js"></script>
    <script src="JS/bootstrap.js"></script>
    <script src="JS/beyond.min.js"></script>
</body>
</html>

<script type="text/javascript">
    $(document).ready(function () {

        //绑定模版的点击事件
        $(".template-box .template-item").click(function () {
            $(".template-box .template-item").css("border", "2px solid #FBFBFB");
            $(this).css("border", "2px solid #E46F61");
            $("#hfTemplateId").val($(this).attr("item-id"));
        });

    });

    //保存设置
    function SaveSetting() {

        SetSaveError("");

        var txtConnection = $(".txtConnection").val();
        if (txtConnection == "") {
            SetSaveError("数据库链接字符串不能为空！");
            return false;
        }

        var ddlDatabaseVersion = $("#ddlDatabaseVersion").val();

        if (ddlDatabaseVersion == null || ddlDatabaseVersion == "") {
            SetSaveError("请选择数据库版本！");
            return false;
        }

        var ddlDatabase = $("#ddlDatabase").val();
        if (ddlDatabase == null || ddlDatabase == "") {
            SetSaveError("请选择数据库！");
            return false;
        }

        var hfTemplateId = $("#hfTemplateId").val();
        if (hfTemplateId == "") {
            SetSaveError("请选择模版！");
            return false;
        }

        return true;
    }

    function SetSaveError(message) {
        var display = message == "" ? "none" : "block";
        $("#SaveError").css("display", display);
        $("#SaveErrorMessage").html(message);
    }
</script>
