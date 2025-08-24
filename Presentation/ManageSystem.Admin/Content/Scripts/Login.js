
jQuery.validator.addMethod("stringCheck", function (value, element) {
    return this.optional(element) || /^[\u0391-\uFFE5\w]+$/.test(value);
}, "只能包括英文字母、数字和下划线");


$().ready(function () {
	    $("#form1").validate({
				rules: {
				    txtLoginId: { required: true, minlength: 3, stringCheck: true },
				    txtPassword: { required: true, minlength: 6 },
				},

				messages: {
				    txtLoginId: { required: "请输入用户名", minlength: "格式不正确", stringCheck: "只能包括英文字母、数字和下划线" },
				    txtPassword: { required: "请输入密码", minlength: "格式不正确" }
				}
		});
	})






	