var myChart = undefined;
var titlename = "";
var dataName = ""
$(function () {
    //设置帮助的顺序  
    var url = window.location.href;
    var index = url.lastIndexOf("\/");
    var str = url.substring(index + 1, url.length);
    if (isNaN(str) == false) {
        renderBarChart(str);
    }
    else {
        $(".page-footer-main").attr("data-step", "12");
        if ($('a[onclick^="renderBarChart"][data-default="1"]').length > 0) {
            $('a[onclick^="renderBarChart"][data-default="1"]').click();
        }
        else {
            $('a[onclick^="renderBarChart"]:eq(0)').click();
        }
    }    
});

renderBarChart = function (id) {
    if (id) {
        $('#sjd-container li.active').removeClass('active');
        $('#li' + id).addClass('active');

        $("#dataError").css("display", "none");
        $(".main-content2").css("display", "block");

        if (!myChart) {
            myChart = echarts.init(document.getElementById('main'));
        }

        $.ajax({
            url: '/Data/AntibioticDrugFastData',
            method: 'POST',
            data: { id: id },
            dataType: 'JSON',
            beforeSend: function () {
                $('#PageContentTitle').html('');
                $('#PageContentSubTitle').html('');
                myChart.clear();
                myChart.resize();
                myChart.showLoading();
                myChart.resize();                    
            },
            complete: function () {
                setTimeout(function () {
                    myChart.hideLoading();
                }, 600);                  
            },
            success: function (res) {
                var option = {
                    color: res.color,
                    title: {
                        text: res.title,
                        subtext: res.subtitle,
                        left: 'center',
                        show: true,
                        textStyle: {
                            fontSize: 18,
                            fontWeight: 400
                        },
                         subtextStyle: {
                             fontWeight:400,
                             fontSize: 16,
                             color:'#000'
                        }
                    },
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: { // 坐标轴指示器，坐标轴触发有效
                            type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
                        }
                    },
                    grid: {
                        top: '18%',
                        left: '3%',
                        right: '4%',
                        bottom: '15%',
                        containLabel: true
                    },
                    xAxis: [
                        {
                            type: 'category',
                            axisLabel: {
                                interval: 0,
                                rotate: res.rotate,
                                margin: 10,
                                textStyle: {
                                    fontSize: "14px"
                                }
                            },
                            data: res.xAxis,  //'替考拉宁', '万古霉素', '利奈唑胺', '利福平', '庆大霉素', '左氧氟沙星', '环丙沙星', '克林霉素', '复方磺胺甲恶唑', '红霉素', '青霉素', '苯唑西林'
                            axisTick: {
                                alignWithLabel: true
                            }
                        }
                    ],
                    yAxis: [
                        {
                            type: 'value',
                            axisLabel: {
                                show: true,
                                interval: 'auto',
                                formatter: '{value} %'
                            },
                            show: true
                        }
                    ],                   
                    toolbox: {
                        show: true,
                        orient: 'vertical',
                        left: 'right',
                        top: 'center',
                        feature: {
                            dataView: {
                                title: "数据",
                                readOnly: false
                            },
                            restore: {},
                            saveAsImage: {
                                title: "下载",
                                type: "jpeg"
                            },
                            //myTool1: {
                            //    show: true,
                            //    title: '分享',
                            //    icon: 'image:///Content/Images/weixinfenxiang.jpg',
                            //    onclick: function () {                                    
                            //        var imgURL = myChart.getDataURL({                                       
                            //            type: "jpg",
                            //            excludeComponents: ['toolbox'],
                            //            backgroundColor: '#fff'
                            //        });//获取base64编码
                            //        console.log(imgURL);
                            //        var dataName=""
                            //        for (var i = 0; i < res.series[0].data.length; i++) {
                            //            dataName += res.series[0].data[i].value + ",";
                            //        }
                            //        $.ajax({
                            //            url: '/Data/Picturesharing',
                            //            method: 'POST',
                            //            data: { dataURL: imgURL, imgName: dataName },                                      
                            //            success: function (json) {
                            //                $('#erWeiMa').empty();
                            //                $('#erWeiMa').qrcode('http://www.chinets.com' + json);                                            
                            //                $('#erWeiMa_outgoing').toggle();
                            //            },
                            //            error: function (response) { alert("error"); }
                            //        });                      
                            //    }
                            //},

                        }
                    },
                    series: res.series,
                    textStyle:
                    {
                        fontSize: 12
                    }
                };                
                
                $('#PageContentTitle').text(res.title);
                $('#PageContentSubTitle').text(res.subtitle);
                //使用制定的配置项和数据显示图表
                myChart.setOption(option);                 
                myChart.resize();              
                setTimeout(function () {
                    myChart.hideLoading();
                }, 350);
                titlename = res.title;             
                for (var i = 0; i < res.series[0].data.length; i++) {
                    dataName += res.series[0].data[i].value + ",";
                }
                QRcodesharing(dataName);
            }
        });
                
    }  
};
function QRcodesharing(data) {  
    myChart.hideLoading();
    var imgURL = myChart.getDataURL({
        type: "jpg",
        excludeComponents: ['toolbox'],
        backgroundColor: '#fff'
    });//获取base64编码  
    $.ajax({
       url: '/Data/Picturesharing',
       method: 'POST',
        data: { dataURL: imgURL, imgName: data, Name: titlename },
       success: function (json) {
       $('#erWeiMa').empty();
       $('#erWeiMa').qrcode('http://www.chinets.com' + json); 
       //$('#erWeiMa').qrcode('http://www.chinets.com');
       },
       error: function (response) { alert("error"); }
    });                      
}

//取消选择
function ClearSelect() {

    $(".date-item-va").removeClass("active");
    var itemList = $(".date-item-va");

    for (var i = 0; i < itemList.length; i++) {
        $(itemList[i]).attr("data-status", "0");
    }

    //隐藏
    $("#dataError").css("display", "block");
    $(".main-content2").css("display", "none");
    $("#PageContentTitle").html("");
    $("#PageContentSubTitle").html("");
}


//设置单个项目的显示和隐藏（公共函数）
function ShowStatus(className) {
    var $el = $("#" + className),
        status = $el.attr("data-status");

    if (status === "1" || status === 1) {
        //隐藏
        $el.slideUp(300, function () {
            $el.attr("data-status", "0");
            $(".btn-image-" + className).attr("src", "/Content/Images/jiantou-shang.png");
        });
    } else {
        //显示
        $el.slideDown(300, function () {
            $el.attr("data-status", "1");
            $(".btn-image-" + className).attr("src", "/Content/Images/jiantou-xia.png");
        });
    }
}

renderBarChart('5685375413080704279');


