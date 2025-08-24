
//全选功能
function SelectAll() {
    var status = $(".chkAll").prop("checked");
    $(".chkItem").prop("checked", status);

    SetCartData();
}

//设置购物车数据
function SetCartData() {
    var itemList = $(".cart-item");

    var count = 0;
    var amount = 0;
    for (var i = 0; i < itemList.length; i++) {
        if ($(itemList[i]).find(".chkItem").prop("checked")) {
            count++;

            //计算总价
            var count = parseInt($(itemList[i]).find(".txt-count").val());
            var price = parseFloat($(itemList[i]).find(".txt-count").attr("Price"));
            var amountTemp = (price * count).toFixed(2);

            $(itemList[i]).find(".item-amount").html(amountTemp);

            amount += parseFloat(amountTemp);
        }
    }

    //全选效果
    if (count == itemList.length) {
        $(".chkAll").prop("checked", true);
    } else {
        $(".chkAll").prop("checked", false);
    }

    //购买数量
    count = count <= 0 ? 0 : count;
    $(".product-count").html(count);

    //购买总价
    $(".cart-amount").html(amount + " 元");

}

//添加购物车
function DeleteCart(id) {

    if (id <= 0) {
        MessageWindow("数据错误，请刷新重试！");
        return;
    }

    $.ajax({
        url: "/Mall/DeleteCart",
        type: "Post",
        dataType: "json",
        data: {
            id: id
        },
        success: function (json) {

            if (json == null || json == "") {
                ErrorWindow("数据错误，请刷新重试！", true, "");
                return;
            }

            if (json.status == "true") {
                $(".item-" + id).remove();
                MessageWindow("删除购物车成功！");

                SetCartData();
                return;
            } else {
                ErrorWindow(json.message, false, "");
            }

        }
    });
}

//检查输入的购买数量是否合法
function CheckCount(obj) {
    var count = $(obj).val();
    if (!IsPositiveInteger(count) || parseInt(count) > 100000 || parseInt(count) <= 0) {
        $(obj).val(1);
        count = 1;
    }

    //改变合计
    var price = $(obj).attr("Price");
    price = price <= 0 ? 0 : price;
    var amountObj = $(obj).closest("tr").find(".item-amount");
    var amount = (parseFloat(price) * parseInt(count)).toFixed(2);
    $(amountObj).html(amount);

    //设置购物车数据
    SetCartData();
}

//去结算
function CartSettlement() {
    //验证数据
    var dataArray = new Array();
    var itemList = $(".cart-item");
    if (itemList == null || itemList.length <= 0) {
        ErrorWindow("请勾选需要结算的明细！", false, "");
        return;
    }

    for (var i = 0; i < itemList.length; i++) {
        if ($(itemList[i]).find(".chkItem").prop("checked")) {

            //计算总价
            var id = $(itemList[i]).attr("DataId");
            var count = parseInt($(itemList[i]).find(".txt-count").val());

            if (id == null || id == "" || count == null || count <= 0)
                continue;

            dataArray.push({ Id: id, Count: count });
        }
    }

    //提交请求
    if (dataArray == null || dataArray.length <= 0) {
        ErrorWindow("请勾选需要结算的明细！", false, "");
        return;
    }

    //跳转到支付订单结算页面
    $.ajax({
        url: "/Mall/CartSettlement",
        type: "Post",
        dataType: "json",
        data: {
            data: dataArray
        },
        success: function (json) {

            if (json == null || json == "") {
                ErrorWindow("数据错误，请刷新重试！", true, "");
                return;
            }

            if (json.status == "true" && json.message != "") {
                location.href = "/Mall/Settlement?key=" + json.message;
            } else {
                ErrorWindow(json.message, false, "");
            }

        }
    });
}

