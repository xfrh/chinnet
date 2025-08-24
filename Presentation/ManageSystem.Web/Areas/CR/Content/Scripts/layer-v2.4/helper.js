
/**
 
 layer 插件的助手类，通用函数，请勿随意修改

*/


//弹出操作成功窗口，detail：提示内容，reload 是否重新加载当前页面， url 跳转页面地址
function SuccessWindow(detail, reload, url) {

    layer.alert("" + detail + "", {
        icon: 1,
        skin: 'layui-layer-molv',
        closeBtn: 0
    },
    function () {
        if (reload) {
            location.reload();
        } else if (url != null && url != undefined && url != "") {
            location.href = url;
        } else {
            layer.closeAll('dialog');
        }
    });
}

//弹出操作失败窗口，detail：提示内容，reload 是否重新加载当前页面， url 跳转页面地址
function ErrorWindow(detail, reload, url) {
 
    layer.alert("" + detail + "", {
        icon: 2,
        skin: 'layui-layer-lan',
        closeBtn: 0
    },
    function () {
        if (reload) {
            location.reload();
        } else if (url != null && url != undefined && url != "") {
            location.href = url;
        } else {
            layer.closeAll('dialog');
        }
    });
}

//弹出提示信息
function MessageWindow(detail) {
    layer.msg('' + detail + '');
}


//弹出操作成功窗口，detail：提示内容，reload 是否重新加载当前页面， url 跳转页面地址
function SuccessWindows(detail, reload, url) {

    layer.alert("" + detail + "", {
       
        skin: 'layui-layer-molv',
        closeBtn: 0
    },
        function () {
            if (reload) {
                location.reload();
            } else if (url != null && url != undefined && url != "") {
                location.href = url;
            } else {
                layer.closeAll('dialog');
            }
        });
}

 