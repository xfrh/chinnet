<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="<$$ClassName$$>.aspx.cs" Inherits="<$$NameSpace$$>.<$$ClassName$$>" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><$$TableTitle$$></title>
    <link rel="stylesheet" type="text/css" href="/Themes/Scripts/jqEasyUI/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Themes/Scripts/jqEasyUI/themes/icon.css" />
    <link href="/Themes/Styles/general.css" rel="stylesheet" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jqEasyUI/jquery.easyui.min.js"></script>
    <script src="/Themes/Scripts/jqEasyUI/easyui-lang-zh_CN.js"></script>
    <script src="/Themes/Scripts/jqEasyUI/datagrid-filter.js"></script>

    <script src="/Scripts/comm.js"></script>
</head>
<body>
    <div style="margin: 20px 0;"></div>
    <div class="easyui-panel" title="<$$TableTitle$$>" style="padding: 10px 10px;">

	<!-- 头部数据查找 -->
        <table cellspacing="1" cellpadding="8" style="width: 100%; background-color: #95B8E7;">
            <tr bgcolor="#F4FAFF">
                <td style="width: 150px"><b>名称:</b>
                </td>
                <td>
                    <input id="txtName" class="easyui-textbox" />
                </td>
                <td style="width: 150px"><b>状态:</b>
                </td>
                <td style="width: 200px">
                    <select id="ddlstate" class="easyui-combobox" editable="false">
                        <option value="0">1</option>
                        <option value="3">2</option>
                    </select>
                </td>
                <td>
                    <a id="btnSearch" href="javascript:;" class="easyui-linkbutton" iconcls="icon-search">查询</a>
                </td>
            </tr>
        </table>
        <br />

		<!-- 数据列表 -->
       <$$AspxHtml$$>

        <div id="toolbar">
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="NewData()">添加<$$TableTitle$$></a>
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="EditData()">编辑<$$TableTitle$$></a>
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="DeleteData()">删除<$$TableTitle$$></a>
        </div>
    </div>

   <script src="<$$JSPath$$>?v=1"></script>

</body>
</html>
