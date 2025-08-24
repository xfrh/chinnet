$(function () {
    FindData();

    $("#btnSearch").click(function () {
        FindData();
    });
});

//加载数据
function FindData() {
    //绑定数据
    $('#dg').datagrid({
        url: '<$$SearchURLPath$$>',
        queryParams: {

            //查询条件
            //  name: $("#txtname").val(), //文本框获取值格式
            //state: $("#ddlState").combobox("getValues") , //下拉列表获取选中值格式

        }
    });

    //设置分页控件，请勿随意修改
    var p = $('#dg').datagrid('getPager');
    $(p).pagination({
        pageSize: 30,//每页显示的记录条数，默认为10           
        pageList: [30, 50],//可以设置每页记录条数的列表           
        beforePageText: '第',//页数文本框前显示的汉字           
        afterPageText: '页    共 {pages} 页',
        displayMsg: '当前显示 {from} - {to} 条记录   共 {total} 条记录'
    });

}

//添加事件
function NewData() {
    var url = "<$$AddPagePath$$>";
    top.OpenWindow(url, '添加<$$TableTitle$$>', 695, 350, false);
}

//修改事件
function EditData() {
    var rows = $('#dg').treegrid('getSelections');
    if (rows.length < 1) {
        OpenAlert("必须选中1行");
        return;
    }
    if (rows.length > 1) {
        OpenAlert("只能选择1行");
        return;
    }

    var url = "<$$EditPagePath$$>?id=" + rows[0].Id;
    top.OpenWindow(url, '修改<$$TableTitle$$>', 695, 350, false);
}

//删除事件
function DeleteData() {
    var ids = [];
    var rows = $('#dg').treegrid('getSelections');

    //如果未选择任何行，给出提示
    if (rows.length <= 0) {
        OpenAlert("请选择要删除的行！");
        return;
    }

    $.messager.confirm('删除<$$TableTitle$$>', '确定要删除这些<$$TableTitle$$>吗?', function (r) {
        for (var i = 0; i < rows.length; i++) {
            ids.push(rows[i].Id);
        }

        if (r) {
            $.ajax({
                url: "<$$DeletePagePath$$>",
                type: "Post",
                dataType: "text",
                data: { ids: ids.join(',') },
                success: function (json) {
                    OpenAlert(json);
                    FindData();
                },
                error: function (x, e) {  },
                complete: function (x) {  }
            });
        }
    });

}




