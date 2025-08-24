	$().ready(function(){ 
		$("#form1").validate({ 
				rules: {
				   txtOldPassword: { required: true, minlength: 6 },
				   txtNewPassword: { required: true, minlength: 6 },
				   txtConfirmPassword: { required: true, minlength: 6, equalTo: ".txtNewPassword" }
				},

				messages: {
				  txtOldPassword: { required: "请输入原始密码", minlength: "密码长度必须大于等于6位" },
				  txtNewPassword: { required: "请输入新密码",minlength: "密码长度必须大于等于6位"},
				  txtConfirmPassword: { required: "请输入确认新密码", minlength: "密码长度必须大于等于6位",equalTo:"两次输入密码不一致不一致" }
				}

		});
	})
