
//新增收货地址
function AddNewAddress() {
    //iframe层-父子操作
    layer.open({
        type: 2,
        area: ['660px', '600px'],
        fix: false, //不固定
        maxmin: true,
        title: "新增收货地址",
        content: '/Center/AddressAdd'
    });
}

//编辑收货地址
function EditAddress(id) {
    //iframe层-父子操作
    layer.open({
        type: 2,
        area: ['660px', '600px'],
        fix: false, //不固定
        maxmin: true,
        title: "编辑收货地址",
        content: '/Center/AddressEdit/'+id
    });
}


//重新加载页面
function ReloadPage() {
    
    location.reload();
}

//删除地址
function DeleteAddress(id) {
    //询问框
    layer.confirm('您确认要删除该收货地址吗？', {
        title: '系统提示',
        btn: ['确认', '取消'] //按钮
    }, function () {

        $.ajax({
            type: "POST",
            url: "/Center/AddressDelete",
            data: {
            Id: id
            },
        dataType: "json",
        success: function (data) {
            if (data == "" || data == null) return;

            if (data.status == "true") {
                SuccessWindow("删除成功！", true, "");

            } else {
                ErrorWindow(data.message, true, "");
            }
        }
    });

});
}
