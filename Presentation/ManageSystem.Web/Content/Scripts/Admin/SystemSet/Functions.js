	$().ready(function(){ 
		$("#form1").validate({ 
				rules: {
						Name: {required: true},
						Sort: {required: true,digits:true}
				},

				messages: {
						Name: {required: "请输入功能名称"},
						Sort: {required: "请输入排序编号",digits: "请输入整数"}
				}

		});
	})
