

////刷新当前页面，主要用户弹出框操作完成以后，重新刷新父页面
//  function RefreshPage() {
//      location.reload();
//  }

/*
*全选功能
*/
function CheckSelect() {

    var elements = $(".table-list input[type='checkbox']");
    var checkAll = document.getElementById("CheckAll");

    for (var i = 0; i < elements.length; i++) {
        var e = elements[i];
        if (e.type == 'checkbox' && e != checkAll) {
            e.checked = checkAll.checked;
        }
    }
}

/*
*获取CheckBox所选中的值
*/
function GetCheckBoxSelectVale() {
    var elements = document.getElementsByTagName("input");
    var checkAll = document.getElementById("checkAll");
    var ids = new Array();

    for (var i = 0; i < elements.length; i++) {
        if (elements[i].type == 'checkbox' && elements[i].checked == true && elements[i] != checkAll) {
            ids.push(elements[i].value);
        }
    }
    return ids;
}



/*
*获取多个选中值中的第一个被选中的值
*/
function GetFirstSelectVlaue() {
    var elements = document.getElementsByTagName("input");
    var checkAll = document.getElementById("checkAll");
    for (var i = 0; i < elements.length; i++) {
        var e = elements[i];
        if (e.type == 'checkbox' && e != checkAll && e.checked == true) {
            return $(e).attr("value");
        }
    }
    return "";

}



/*
*导出功能，如果没有选择项，提示导出全部
*/
function IsExport() {
    var checks = document.getElementsByTagName("input");
    var ids = new Array();
    for (var i = 0; i < checks.length; i++) {
        if (checks[i].type == "checkbox" && checks[i].id != "checkAll" && checks[i].checked) {
            ids.push(checks[i].value);
        }
    }

    if (ids.length == 0) {
        return confirm("您没有选择项，是否导出全部？");
    }
}




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

function BatchCheck() {
    var elements = document.getElementsByTagName("input");
    var checkAll = document.getElementById("checkAll");
    for (var i = 0; i < elements.length; i++) {
        var e = elements[i];
        if (e.type == 'checkbox' && e != checkAll && e.checked == true) {
            if (confirm("确定要通过审核吗？")) {
                return true;
            }
            else {
                return false;
            }
        }
    }

    alert('请选择要审核的项！');
    return false;

}


function CheckSelectCheck() {
    var elements = document.getElementsByTagName("input");
    var checkAll = document.getElementById("checkAll");
    if (checkAll == null || checkAll == undefined) {
        checkAll = $(".checkAll");
    }
    for (var i = 0; i < elements.length; i++) {
        var e = elements[i];
        if (e.type == 'checkbox' && e != checkAll && e.checked == true) {
            if (confirm("不可恢复操作，确认操作吗？")) {
                return true;
            }
            else {
                return false;
            }
        }
    }

    alert('请选择要操作的项');
    return false;

}



//去除空格
function trims(ss) {
    return ss.replace(/(^\s*)|(\s*$)/g, "");
}



//取字符串长度 GB
function getStrBytes(str) {
    for (var i = 0, x = 0; i < str.toString().length; i++) {
        x += (str.charCodeAt(i) > 128) ? 2 : 1;
    }
    return x;
}

//取字符串长度 UTF8
function getStrBytesByUtf8(str) {
    for (var i = 0, x = 0; i < str.toString().length; i++) {
        x += (str.charCodeAt(i) > 128) ? 3 : 1;
    }
    return x;
}

//检查数字
function checkNumber(name, type) {
    try {
        var patrn = '';//正则串
        var errorStr = '';//错误提示
        switch (type) {
            case "1"://实数
                patrn = /^(\-?)[0-9]+(\.[0-9]*)?$/gi;
                break;
            case "2"://正实数
                patrn = /^([1-9]+)|([[0-9]\.[0-9]*[1-9][0-9]*)$/gi;
                break;
            case "3"://非负实数 
                patrn = /^[0-9]+(\.[0-9]*)?$/gi;
                break;
            case "4"://整数 
                patrn = /^(\-?)([0-9])+([0-9]*)?$/gi;
                break;
            case "5"://正整数 
                patrn = /^([1-9])+([0-9]*)?$/gi;
                break;
            case "6"://非负整数 
                patrn = /^([0-9])+([0-9]*)?$/gi;
                break;
            case "7"://两位小数内的正数
                patrn = /^[0-9]+(.[0-9]{1,2})?$/gi;
                break;
            case "8"://小于1的正数
                patrn = /^(0\.[0-9]+)|0$/gi;
                break;
        }
        if (errorStr != "" && name.search(patrn) == -1) {
            return false;
        }
    }
    catch (e) {
        return false;
    }

    return true;

}



//格式化浮点数
function genFloatFormat(realNum, preciseBit) {
    var sourceNum = realNum.toString();
    var returnNum;//返回值
    var repeat = 0;//重复次数
    if (sourceNum.search(/^(\-?)[0-9]+(\.[0-9]*)?$/gi) == -1) return 0;
    sourceNum = parseFloat(sourceNum);
    var pos = parseInt(preciseBit);
    if (false == isNaN(pos) && pos > 0) {

        var factor = Math.pow(10, pos);//倍数
        returnNum = Math.round(sourceNum * factor) / factor;
        var startIndex = returnNum.toString().indexOf(".");//小数点位置
        //重复次数
        var repeat = (-1 == startIndex) ? preciseBit
                                        : preciseBit - returnNum.toString().substr(startIndex + 1).length;
        returnNum = returnNum.toString();
    }
    else
        returnNum = sourceNum.toString();
    if (repeat > 0) {
        for (var i = 0; i < repeat; i++) {
            returnNum += (i == 0 && repeat == preciseBit) ? ".0" : "0";
        }
    }
    return returnNum;
}

function getFloatFormat(realNum, preciseBit) {
    return genFloatFormat(realNum, preciseBit);
}



//预览邮件功能
function SearchEmailView(modelId) {
    var content = editor1.html();
    $.ajax({
        async: false,
        type: "POST",
        url: "../SearchEmailViewHandel.ashx",
        data: "data=" + content + "&id=" + modelId,
        dataType: "json",
        success: function (result) {
            if (result != null && result != "") {
                window.open('/Company/EmailView.aspx', 'newwindow', 'height=600,width=400,toolbar=yes,menubar=yes,scrollbars=yes, resizable=yes,location=yes, status=yes')
            }
        }
    });
}


//设置Email radio点击，自己选中，其它取消选中
function EmailAreaRdio_Click(obj, setValue) {
    var inputs = $("#emailArea input");
    if (inputs.length <= 0) return;
    obj.checked = true;
    for (var i = 0; i < inputs.length; i++) {
        var e = inputs[i];
        if (e.type == 'radio' && e != obj) {
            e.checked = false;
        }
    }
}


//商品选择弹出选择商品页面


function changeGoods() {
    mask.show('maskDiv', '../GetGoods.aspx');
    mask.orientation('maskDiv', 817, 617);
}


//选择完成商品以后，返回商品编号，ajax 来获取商品信息，
//不同的页面通过实现AddGoodsItem 函数来动态添加商品
function SetGoodsSelectItem(valus) {
    if (valus.length <= 0) return;

    $.ajax({
        async: false,
        type: "Get",
        url: "/Handel/GetGoods.ashx?1=1" + valus,
        dataType: "json",
        success: function (result) {
            if (result != null && result != "") {
                if (result == "-1") {
                    alert("刷新商品失败，请重试！");
                    return;
                }

                for (var z = 0; z < result.length; z++) {
                    AddGoodsItem(result[z].id, result[z].PImage, result[z].Pname, result[z].PPrice);
                }
            }
        }
    });

}

//将页面添加到收藏
function AddCollectWeb() {
    try {
        window.external.addFavorite('http://www.baidu.com', '爱积通商城');
    } catch (e) {
        try {
            window.sidebar.addPanel('爱积通商城', 'http://www.baidu.com', "");
        } catch (e) {
            alert("加入收藏失败，请使用Ctrl+D进行添加");
            return;
        }
    }
    alert("添加收藏成功！");
}



