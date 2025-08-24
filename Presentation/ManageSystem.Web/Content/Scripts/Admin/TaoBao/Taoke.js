	$().ready(function(){ 
		$("#form1").validate({ 
				rules: {
				    Title: { required: true },
				    Url: { required: true },
				    ImagePath: { required: true },
				    Price: { required: true, number: true },
				    MarketPrice: { required: true, number: true },
				    Sort: { required: true, digits: true }
				},

				messages: {
				    Title: { required: "请输入商品标题" },
				    Url: { required: "请输入商品地址" },
				    ImagePath: { required: "请输入图片地址" },
				    Price: { required: "请输入商品价格", number: "请输入数字" },
				    MarketPrice: { required: "请输入市场价", number: "请输入数字" },
				    Sort: { required: "请输入排序编号",digits: "请输入整数"  }
				}

		});
	})
