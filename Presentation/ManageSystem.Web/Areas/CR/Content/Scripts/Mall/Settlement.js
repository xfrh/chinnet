//新增收货地址
function AddNewAddress() {
    //iframe层-父子操作
    layer.open({
        type: 2,
        area: ['750px', '540px'],
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
        area: ['750px', '540px'],
        fix: false, //不固定
        maxmin: true,
        title: "编辑收货地址",
        content: '/Center/AddressEdit/' + id
    });
}

//重新加载页面
function ReloadPage() {
    location.reload();
}

function Save() {

    var key = $("#CartKey").val()
    var productCount = parseFloat($("#ProductAmount").val());
    var orderAmount = parseFloat($("#OrderAmount").val());
    if (key == "" || key.length < 10 || productCount == undefined || productCount <= 0 || orderAmount == undefined || orderAmount <= 0) {
        ErrorWindow("订单数据错误，请重新提交！", false, "");
        return;
    }

    var addressId = $('input[name="MemberAddress"]:checked').val();
    if (addressId == null || addressId == "") {
        ErrorWindow("请选择收货地址！", false, "");
        return;
    }

    var dataArray = new Array();
    var itemList = $(".cart-item");
    if (itemList == null || itemList.length <= 0) {
        ErrorWindow("订单商品明细数据错误！", false, "");
        return;
    }

    for (var i = 0; i < itemList.length; i++) {
        var id = $(itemList[i]).attr("dataId");
        var count = $(itemList[i]).attr("dataCount");

        if (id == null || id == "" || count == null || count <= 0)
            continue;

        dataArray.push({ Id: id, Count: count });
    }

    if (dataArray == null || dataArray.length <= 0) {
        ErrorWindow("订单商品明细数据错误！", false, "");
        return;
    }

    var remark = $("#Remark").val();
    if (remark != null && remark.length > 200) {
        ErrorWindow("订单备注长度不能大于200！", false, "");
        return;
    }

    $(".btn-pay").attr("disabled", "disabled");
    $(".btn-pay").val("订单提交中...");


    $.ajax({
        url: "/Mall/SubmitOrder",
        type: "Post",
        dataType: "json",
        data: {
            Key: key,
            ProductCount: productCount,
            OrderAmount: orderAmount,
            ItemList: dataArray,
            AddressId: addressId,
            Remark: remark
        },
        success: function (json) {

            if (json == null || json == "") {
                ErrorWindow("数据错误，请刷新重试！", true, "");
                return;
            }

            if (json.status == "true") {
                var url = "/Mall/Result?sn=" + json.message;
                location.href = url;

            } else {
                ErrorWindow(json.message, false, "");
                $(".btn-pay").removeAttr("disabled");
                $(".btn-pay").val("确认支付");
            }

        }
    });
}
