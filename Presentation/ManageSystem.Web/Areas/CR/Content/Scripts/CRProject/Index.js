
//删除数据
function Delete(id) {
    //询问框
    layer.confirm('您确认要删除该数据吗？', {
        title: '系统提示',
        btn: ['确认', '取消'] //按钮
    }, function () {

        $.ajax({
            async: false,
            type: "POST",
            url: "/CRProject/Delete",
            data: {
                id: id
            },
            dataType: "json",
            success: function (json) {

                if (json == null || json == "")
                    return;

                if (json.status == "true") {
                    SuccessWindow("删除成功", true);
                }
                else {
                    ErrorWindow(json.message, true);
                }
            }
        });

    });
}

//上传数据
function Import(id) {
    //iframe层-父子操作
    $("#ProjectId").val(id);
    layer.open({
        type: 2,
        area: ['660px', '520px'],
        fix: false, //不固定
        maxmin: true,
        title: "上传数据",
        content: '/CRProject/Upload/' + id
    });
}

//上传文件成功弹出提示
function UploadSuccess() {
    SuccessWindow("上传成功", true, "");
}