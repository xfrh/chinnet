	$().ready(function(){ 
		$("#form1").validate({ 
				rules: {
						Name: {required: true},
						Sort: {required: true,digits:true}
				},

				messages: {
						Name: {required: "请输入角色名称"},
						Sort: {required: "请输入排序编号",digits: "请输入整数"}
				}

		});
	})


	function Save() {
	    GetFunctionValue();
	    $("#form1").validate({
	        submitHandler: function (form) {
	            form.submit();
	        }
	    });

	    return false;
	}

	function GetFunctionValue() {
	    var mentArray = $(".menu_div input[type='checkbox']");
	    var result = "";

	    for (var i = 0; i < mentArray.length; i++) {
	        var item = $(mentArray[i]);
	        var itemValue = item.val() + "|" + (mentArray[i].checked ? "t" : "f");

	        var functionArray = $(".menu_ul_" + item.val() + " input[type='checkbox']");
	        var functionValue = "";
	        for (var z = 0; z < functionArray.length; z++) {
	            var fun = $(functionArray[z]);
	            if (functionArray[z].checked) functionValue += fun.val() + ",";
	        }

	        if (functionValue.length != "") functionValue = functionValue.substring(0, functionValue.length - 1);
	        result += itemValue + "|" + functionValue + "&";
	    }

	    if (result != "") result = result.substring(0, result.length - 1);

	    $("#FunctionValue").val(result);
	}