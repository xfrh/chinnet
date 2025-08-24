<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AdminPageMaster.Master" AutoEventWireup="true" CodeBehind="<$$ClassName$$>.aspx.cs" Inherits="<$$NameSpace$$>.<$$ClassName$$>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <UC:AdminBreadcrumbs ID="AdminBreadcrumbs" runat="server"  MenuName="一级菜单"  SecondName="二级菜单" LocationName="查看<$$TableTitle$$>" > </UC:AdminBreadcrumbs>
    <div class="page-body  role-page">
        <div class="row add-page">
            <div class="col-lg-12 col-sm-12 col-xs-12">

                <div class="widget ">
                    <div class="widget-header add-page-titlebox ">
                        <span class="widget-caption add-page-title"><i class="glyphicon glyphicon-plus "></i>查看<$$TableTitle$$></span>
                    </div>
                    <div class="widget-body add-content " id="BaseDiv" runat="server">
                       
							<$$AspxHtml$$>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
