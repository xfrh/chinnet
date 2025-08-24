$().ready(function () {
    $('#CheckAll').click(function () {
        var elements =$(".table-select input");
        for (var i = 0; i < elements.length; i++) {
            var e = elements[i];
            if (e.type == 'checkbox' && e != this) {
                e.checked = this.checked;
            }
        }
    });
})



function CheckDel() {
    var elements = document.getElementsByTagName("input");
    var checkAll = document.getElementById("checkAll");
    for (var i = 0; i < elements.length; i++) {
        var e = elements[i];
        if (e.type == 'checkbox' && e != checkAll && e.checked == true) {
            if (confirm("确定要删除？删除后数据将无法恢复！")) {
                return true;
            }
            else {
                return false;
            }
        }
    }

    alert('请选择要删除的项！');
    return false;
}


function CheckReturn() {
    var elements = document.getElementsByTagName("input");
    var checkAll = document.getElementById("checkAll");
    for (var i = 0; i < elements.length; i++) {
        var e = elements[i];
        if (e.type == 'checkbox' && e != checkAll && e.checked == true) {
            if (confirm("确定将所选文章移动到回收站吗？")) {
                return true;
            }
            else {
                return false;
            }
        }
    }

    alert('请选择要移动的项！');
    return false;
}


//列表页面删除数据通用方法，请勿随意修改
function DeleteSelect(actionUrl) {

    try {
        if (actionUrl == "") {
            alert("删除请求地址错误，请检查参数！");
            return;
        }

        var elements = $("#ManageTableDiv input[type='checkbox']");
        var checkAll = $("#checkAll");
        var deleteItemIds = "";

        for (var i = 0; i < elements.length; i++) {
            var e = elements[i];
            if (e.type == 'checkbox' && e != checkAll && e.checked == true) {
                var itemId = $(e).attr("value");
                if (itemId == "") continue;
                deleteItemIds += itemId + ",";
            }
        }

        if (deleteItemIds.trim() == "") {
            alert('请选择要删除的项！');
            return;
        }

        deleteItemIds = deleteItemIds.substring(0, deleteItemIds.length - 1);

        if (confirm("确定要删除？删除后数据将无法恢复！")) {
            $.ajax({
                async: false,
                type: "POST",
                url: "" + actionUrl + "",
                data: "itemIds=" + deleteItemIds,
                dataType: "text",
                success: function (result) {
                    if (result == "true") {
                        alert('删除成功！');
                        location.reload();
                    } else {
                        alert('删除失败，请重试！');
                    }
                }
            });
        }
    } catch (e) {
        alert("删除发送错误，请重试！");
        location.reload();
    }
  
}
