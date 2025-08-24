var myChart = undefined;
$(document).ready(function () {

    var url = window.location.href;
    var index = url.lastIndexOf("\/");
    var str = url.substring(index + 1, url.length);
    if (isNaN(str) == false) {
        renderTrendChart(str);
    }
    else {

        //设置帮助的顺序
        $(".page-footer-main").attr("data-step", "6");

        if ($('a[onclick^="renderTrendChart"][data-default="1"]').length > 0) {
            $('a[onclick^="renderTrendChart"][data-default="1"]').click();
        }
        else {
            $('a[onclick^="renderTrendChart"]:eq(0)').click();
        }
    }
});


renderTrendChart = function (id) {
    if (id) {
        $('#sjd-container li.active').removeClass('active');
        $('#li' + id).addClass('active');
        $('#dataError, #mainChart, #data-grid-chart').show();
        $('#fungus-container, #data-grid, #data-grid-intro').hide();
        $("#dataError").css("display", "none");
        $(".main-content2").css("display", "block");

        if (!myChart) {
            myChart = echarts.init(document.getElementById('mainChart'));
        }

        $.ajax({
            url: '/Shanghai/Data/AntibioticDrugFastData',
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
                        text: res.title + res.subtitle,
                        subtext: null, //res.subtitle,
                        left: 'center',
                        show: true,
                        textStyle: {
                            fontWeight: 'normal'
                        }
                    },
                    legend: {
                        data: res.legend,
                        top: '35px'
                    },
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: { // 坐标轴指示器，坐标轴触发有效
                            type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
                        }
                    },
                    grid: {
                        top: '80px',
                        left: '0',
                        right: '0',
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
                            data: res.xAxis,
                            axisTick: {
                                show: false,
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
    $('#mainChart, #data-grid-chart').hide();
    $('#fungus-container, #data-grid, #data-grid-intro').hide();
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



function clicktable(id) {
    $('#sjd-container li.active').removeClass('active');
    $('#li' + id).addClass('active');
    $('#dataError, #mainChart, #data-grid-chart').hide();
    $('#fungus-container, #data-grid, #data-grid-intro').show();
    var dataDisplayGrid = $("#data-grid").data("kendoGrid");
    if (dataDisplayGrid) {
        // 存在则销毁
        $('#data-grid').kendoGrid('destroy').empty();
    }   
    $.ajax({
        url: '/Shanghai/Data/Get_SHBarChartsTable',
        method: 'POST',
        data: { id: id },
        dataType: 'JSON',
        timeout: 0,
        cache: false,
        success: function (res) {
            $("#data-grid").kendoGrid({
                dataSource: JSON.parse(res[0].Antibiotics),
                scrollable: false,
                groupable: false,
                toolbar: res[0].Title,
                columns: [
                    { field: "Name", title: "抗真菌药物", width: 350, headerAttributes: { style: 'font-weight: bold;' } },
                    { field: "MIC_Range", title: "MIC范围", width: 150, headerAttributes: { style: 'text-align: center; font-weight: bold;' }, attributes: { style: 'text-align: center;' } },
                    { field: "MIC50", title: "MIC<sub>50</sub>", width: 150, headerAttributes: { style: 'text-align: center; font-weight: bold;' }, attributes: { style: 'text-align: center;' } },
                    { field: "MIC90", title: "MIC<sub>90</sub>", width: 150, headerAttributes: { style: 'text-align: center; font-weight: bold;' }, attributes: { style: 'text-align: center;' } },
                    { field: "S", title: "敏感", width: 150, headerAttributes: { style: 'text-align: center; font-weight: bold;' }, attributes: { style: 'text-align: center;' } },
                    { field: "SDD", title: "剂量依赖敏感", width: 150, headerAttributes: { style: 'text-align: center; font-weight: bold;' }, attributes: { style: 'text-align: center;' } },
                    { field: "I", title: "中介", width: 150, headerAttributes: { style: 'text-align: center; font-weight: bold;' }, attributes: { style: 'text-align: center;' } },
                    { field: "R", title: "耐药", width: 150, headerAttributes: { style: 'text-align: center; font-weight: bold;' }, attributes: { style: 'text-align: center;' } }
                ]
            });
        }
    })

}

