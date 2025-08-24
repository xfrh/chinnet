
      //注册
      function Regist() {

          var loginId = $("#LoginId").val();
          if (loginId == null || loginId == "") {
              ErrorWindow("请输入登录帐号");
              return;
          }

          var loginIdRe = /^[a-zA-Z][a-zA-Z0-9_]{3,15}$/;
          if (!loginIdRe.test(loginId)) {
              ErrorWindow("登录帐号格式不正确");
              return;
          }

          var name = $("#Name").val();
          if (name == "" || $.trim(name).length < 2) {
              ErrorWindow("请输入姓名");
              return;
          }

          var phone = $("#Phone").val();
          if (!IsPhoneNumber(phone)) {
              ErrorWindow("请输入手机号码");
              return;
          }

          var phoneCode = $("#PhoneCode").val();
          if (!IsNumber(phoneCode) || phoneCode.length != 6) {
              ErrorWindow("请输入短信验证或格式不正确");
              return;
          }

          var password = $("#Password").val();
          if (password == "") {
              ErrorWindow("请输入密码");
              return;
          }
          if ($.trim(password).length < 6) {
              ErrorWindow("密码长度必须大于等于6位");
              return;
          }

          var password2 = $("#Password2").val();
          if (password2 == "") {
              ErrorWindow("请输入确认密码");
              return;
          }
          if ($.trim(password2).length < 6) {
              ErrorWindow("确认密码长度必须大于等于6位");
              return;
          }

          if (password != password2)
          {
              ErrorWindow("2次密码不一致");
              return;
          }

          $(".btn-login").attr("disabled", "disabled");

          $.ajax({
              type: "POST",
              url: "/Home/Regist",
              data: {
              LoginId: loginId,
              Name: name,
              Phone: phone,
              PhoneCode: phoneCode,
              Password:password,
              Password2:password2,
              Code:$("#Code").val()
              },
          dataType: "json",
          success: function (result) {

              if (result == null || result == "") {
                  ErrorWindow("注册失败，请重试！");
                  $(".btn-login").removeAttr("disabled");
                  return;
              }

              if (result.status == "true") {
                  location.href = "/Home/Index";
              } else {
                  ErrorWindow(result.message);
                  $(".btn-login").removeAttr("disabled");
              }
          }
      });
}



//发送短信验证码
function SendValidateCode() {
    var phone = $("#Phone").val();
    if (!IsPhoneNumber(phone)) {
        ErrorWindow("请输入手机号码");
        $("#phone").focus();
        return;
    }

    $.ajax({
        type: "POST",
        url: "/Home/SendRegistValidateCode",
        data: "phone=" + phone,
    dataType: "json",
    success: function (result) {

        if (result == null || result == "") {
            alert("发送验证码失败，请重试");
            return;
        }

        if (result.status == "true") {
            //成功
            $(".yzm").attr("disabled", "disabled");
            setInterval(SetValidateTime, 1000);

            return;
        } else {
            ErrorWindow(result.message);
        }
    }
});
}

//设置更新时间
var timeIndex = 180; //180秒
function SetValidateTime() {

    if (timeIndex <= 0) {
        $(".yzm").removeAttr("disabled");
        $(".yzm").val("获取验证码");
        return;
    }

    $(".yzm").val(timeIndex + " 秒后重发");

    timeIndex--;
}

