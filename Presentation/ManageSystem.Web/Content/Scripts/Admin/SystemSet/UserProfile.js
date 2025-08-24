	$().ready(function(){ 
	    $("#form1").validate({
				rules: {
				  Name: { required: true },
				    Phone: { required: true },
				    NickName: { required: true },
				    Birthday: { required: true }
				},

				messages: {
				    Name: { required: "请输入真实姓名" },
				    Phone: { required: "请输入联系方式" },
				   NickName: { required: "请输入昵称" },
				  Birthday: { required: "请输入出生日期" }
				}

		 });
	  })

