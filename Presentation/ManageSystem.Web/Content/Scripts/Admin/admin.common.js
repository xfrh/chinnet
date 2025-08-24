function setLocation(url) {
    window.location.href = url;
}

function OpenWindow(query, w, h, scroll) {
    var l = (screen.width - w) / 2;
    var t = (screen.height - h) / 2;

    winprops = 'resizable=1, height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + 'w';
    if (scroll) winprops += ',scrollbars=1';
    var f = window.open(query, "_blank", winprops);
}

function showThrobber(message) {
    $('.throbber-header').html(message);
    window.setTimeout(function () {
        $(".throbber").show();
    }, 1000);
}

$(document).ready(function () {
    $('.multi-store-override-option').each(function (k, v) {
        checkOverriddenStoreValue(v, $(v).attr('data-for-input-selector'));
    });
});

function checkAllOverriddenStoreValue(item) {
    $('.multi-store-override-option').each(function (k, v) {
        $(v).attr('checked', item.checked);
        checkOverriddenStoreValue(v, $(v).attr('data-for-input-selector'));
    });
}

function checkOverriddenStoreValue(obj, selector) {
    var elementsArray = selector.split(",");
    if (!$(obj).is(':checked')) {
        $(selector).attr('disabled', true);
        //Kendo UI elements are enabled/disabled some other way
        $.each(elementsArray, function (key, value) {
            var kenoduiElement = $(value).data("kendoNumericTextBox");
            if (kenoduiElement !== undefined && kenoduiElement !== null) {
                kenoduiElement.enable(false);
            }
        });
    }
    else {
        $(selector).removeAttr('disabled');
        //Kendo UI elements are enabled/disabled some other way
        $.each(elementsArray, function (key, value) {
            var kenoduiElement = $(value).data("kendoNumericTextBox");
            if (kenoduiElement !== undefined && kenoduiElement !== null) {
                kenoduiElement.enable();
            }
        });
    };
}

function tabstrip_on_tab_select(e) {
    //we use this function to store selected tab index into HML input
    //this way we can persist selected tab between HTTP requests
    $("#selected-tab-index").val($(e.item).index());
}

function display_kendoui_grid_error(e) {
    if (e.errors) {
        if ((typeof e.errors) == 'string') {
            //single error
            //display the message
            alert(e.errors);
        } else {
            //array of errors
            //source: http://docs.kendoui.com/getting-started/using-kendo-with/aspnet-mvc/helpers/grid/faq#how-do-i-display-model-state-errors?
            var message = "The following errors have occurred:";
            //create a message containing all errors.
            $.each(e.errors, function (key, value) {
                if (value.errors) {
                    message += "\n";
                    message += value.errors.join("\n");
                }
            });
            //display the message
            alert(message);
        }
    } else {
        alert('Error happened');
    }
}

// CSRF (XSRF) security
function addAntiForgeryToken(data) {
    //if the object is undefined, create a new one.
    if (!data) {
        data = {};
    }
    //add token
    var tokenInput = $('input[name=__RequestVerificationToken]');
    if (tokenInput.length) {
        data.__RequestVerificationToken = tokenInput.val();
    }
    return data;
};


// Ajax activity indicator bound to ajax start/stop document events
$(document).ajaxStart(function () {
    $('#ajaxBusy').show();
}).ajaxStop(function () {
    $('#ajaxBusy').hide();
});




/*
*全选功能
*/
function CheckSelect() {
   alert($("#mastercheckbox").attr("checked"));
   
   if ($("#mastercheckbox").attr("checked") == true || $("#mastercheckbox").attr("checked") == "checked") {
       
        $(".checkboxGroups").attr("checked", "checked");
   } else {

        $(".checkboxGroups").removeAttr("checked");
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
