var myChart = undefined;

$(document).ready(function () {
    //设置帮助的顺序
    $(".page-footer-main").attr("data-step", "6");

    if ($('a[onclick^="renderTrendChart"][data-default="1"]').length > 0) {
        $('a[onclick^="renderTrendChart"][data-default="1"]').click();
    }
    else {
        $('a[onclick^="renderTrendChart"]:eq(0)').click();
    }
});


renderTrendChart = function (id) {
    if (id) {
        $('#sjd-container li.active').removeClass('active');
        $('#li' + id).addClass('active');

        $("#dataError").css("display", "none");
        $(".main-content2").css("display", "block");

        if (!myChart) {
            myChart = echarts.init(document.getElementById('main'));
        }

        $.ajax({
            url: '/Data/GermYearData',
            method: 'POST',
            data: { id: id },
            dataType: 'JSON',
            timeout: 0,
            cache: false,
            beforeSend: function () {
                $('#PageContentTitle').html('');
                $('#PageContentSubTitle').html('');
                myChart.clear();
                myChart.resize();
                myChart.showLoading();
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
                        subtext: null, //res.subtitle,
                        left: 'center',
                        show: true,
                        textStyle: {
                            fontWeight: 'normal'
                        }
                    },
                    legend: {
                        data: res.legend,
                        top: '30px'
                    },
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: { // 坐标轴指示器，坐标轴触发有效
                            type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
                        }
                    },
                    grid: {
                        top: '18%',
                        left: '0',
                        right: '3%',
                        bottom: '8%',
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
                            data: res.xAxis,
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
                            }
                        }
                    },
                    series: res.series,
                    textStyle:
                    {
                        fontSize: 12
                    }
                };

                if (res.title === '历年成员单位及监测总株数') {
                    option.yAxis = [
                        {
                            name: '菌株数',
                            type: 'value',
                            position: 'right',
                            axisLabel: {
                                show: true,
                                interval: 'auto'
                            },
                            axisLine: {
                                lineStyle: {
                                    color: '#015BAA'
                                }
                            },
                            min: 0,
                            max: 300000,
                            splitNumber: 6,
                            show: true
                        },
                        {
                            name: '医院数',
                            type: 'value',
                            axisLabel: {
                                show: true,
                                interval: 'auto',
                                formatter: '{value} %'
                            },
                            axisLine: {
                                lineStyle: {
                                    color: '#C1232B'
                                }
                            },
                            min: 0,
                            max: 50,
                            splitNumber: 10,
                            show: true
                        }
                    ];
                    for (var i = 0, len = option.series.length; i < len; i++) {
                        if (option.series[i].name === '医院数') {
                            option.series[i].yAxisIndex = 1;
                            option.series[i].barWidth = 30;
                        }
                    }
                }

                console.log(option);
                //使用制定的配置项和数据显示图表
                myChart.setOption(option);
                myChart.resize();
                setTimeout(function () {
                    myChart.hideLoading();
                }, 350);
            }
        });
    }
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