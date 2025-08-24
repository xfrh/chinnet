
$().ready(function () {
		$("#form1").validate({ 
				rules: {
				    txtPassword: { required: true,minlength:6 }
				},

				messages: {
				    txtPassword: { required: "",minlength:"" }
				}

		});
	
	})

