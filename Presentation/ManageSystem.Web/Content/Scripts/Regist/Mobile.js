//注册
function Regist() {

    var phone = $("#Phone").val();
    if (!IsPhoneNumber(phone)) {
        MessageWindow("请输入手机号码");
        return;
    }

    var password = $("#Password").val();
    if (password == "") {
        MessageWindow("请输入密码");
        return;
    }
    if ($.trim(password).length < 6) {
        MessageWindow("密码长度必须大于等于6位");
        return;
    }

    //var password2 = $("#Password2").val();
    //if (password2 == "") {
    //    MessageWindow("请输入确认密码");
    //    return;
    //}
    if ($.trim(password2).length < 6) {
        MessageWindow("确认密码长度必须大于等于6位");
        return;
    }

    //if (password != password2) {
    //    MessageWindow("2次密码不一致");
    //    return;
    //}

    var areaId = $("#AreaId").val();
    if (areaId == null || areaId == "" || areaId == "0") {
        MessageWindow("请选择所属省份");
        return;
    }

    var hospital = $("#OtherHospital").val();
    if (hospital == "") {
        MessageWindow("请输入所属医院名称");
        return;
    }

    $(".btn-login").attr("disabled", "disabled");

    $.ajax({
        type: "POST",
        url: "/Regist/Regist",
        data: {
            Phone: phone,
            AreaId: areaId,
            Password: password,
            Password2: password2,
            HospitalId: 0,
            Code: $("#Code").val(),
            OtherHospital: hospital,
            Source:2
        },
        dataType: "json",
        success: function (result) {

            $(".btn-login").removeAttr("disabled");

            if (result == null || result == "") {
                ErrorWindow("注册失败，请重试！");
                return;
            }

            if (result.status == "true") {
                SuccessWindow("注册成功", true);
            } else {
                ErrorWindow(result.message);
            }
        }
    });
}


//选择医院事件，如果选择了“其他的医院”，则打开输入医院的名称
function SelectHospital() {
    var text = $("#HospitalId").find("option:selected").text();
    if (text != null && text == "其他医院") {
        $("#OtherHospital").css("display", "block");
    } else {
        $("#OtherHospital").css("display", "none");
    }
}