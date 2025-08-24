var text = [];
var cre = [];
var creab = [];
$(document).ready(function () {
    $.ajax({
        url: "/CR/CRData/GetCRslist",
        type: "post",
        async: false,
        dataType: "json",
        success: function (data) {
            //data = JSON.parse(data)
            for (var i = 0; i < data.length; i++) {
                text.push(data[i].Province);
                cre.push(data[i].cre);
                creab.push(data[i].crab);  
                LoadCharts(text, cre, creab,50);
            }
           
        }

    });

  
});

// 基于准备好的dom，初始化echarts实例
var myChart = echarts.init(document.getElementById('main'));


function isMobile() {
    try {
        document.createEvent("TouchEvent");
        return true;
    } catch (e) {
        return false;
    }
}

if (isMobile()) {

}
///**
// * 选择数据项
// * @param {int} identity 数据项标识
// */
//function SelectItem(identity) {
//    var $target = $('#data-' + identity);
//    $('.date-item-va').removeClass('active');
//    $target.addClass('active');

//    $.getJSON('/Areas/CR/Content/js/Project1_p1.json', function(res) {
//        res.forEach(function(item, i) {
//            if (item.identity === identity) {
//                var text = [],
//                    cre = [],
//                    crea = [],
//                    xRotate = 30,
//                    title = item.title + '对抗菌药物的耐药率（%）',
//                    barWidth = 30;

//                item.data.forEach(function(v, j) {
//                    text.push(v.name);
//                    data.push(v.value);
//                });
//                LoadCharts(text, data, xRotate, title, barWidth);
//                return;
//            }
//        });
//    });
//}
$(window).resize(function() {
    myChart.resize();
});

//选择抗菌药物，重新加载报表
//function SelectItem2(identity) {

//    var $target = $('#data-' + identity);
//    $('.date-item-va').removeClass('active');
//    $target.addClass('active');

//    $.getJSON('/Areas/CR/Content/js/Project1_p2.json', function(res) {
//        res.forEach(function(item, i) {
//            if (item.identity === identity) {
//                var text = [],
//                    data = [],
//                    xRotate =30,
//                    title = item.title,
//                    barWidth =30;

//                item.data.forEach(function(v, j) {
//                    text.push(v.name);
//                    data.push(v.value);
//                });
//                LoadCharts(text, data, xRotate, title, barWidth);
//                return;
//            }
//        });
//    });
//}



/**
 * 加载报表数据
 * @param {any} xAxisData x轴显示数据
 * @param {any} seriesData1 图表数据
 *  * @param {any} seriesData2 图表数据
 * @param {any} xRotate x轴数据倾斜角度
 * @param {any} title 图表标题
 * @param {any} barWidth 柱状宽度
 */
function LoadCharts(xAxisData, seriesData1, seriesData2, xRotate, title,) {

    $("#dataError").css("display", "none");
    $(".main-content2").css("display", "block");

    myChart.clear();
    myChart.resize();
    myChart.showLoading();
    if (isMobile()) {
        console.log("1111")
        var option = {
            color: ['#015BAA', '#C1232B', '#FE8463', '#ECBF00', '#CF7CA6'],
            title: {
                text: title,
                subtext: '',
                left: 'center',
                show: true,
                textStyle: {
                    fontWeight: 'normal',
                    fontSize: 15,
                    width: '100%',
                },
                textVerticalAlign: 'auto',
                padding: 5
            },
            tooltip: {
                trigger: 'axis',
                axisPointer: { // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
                }
            },
            grid: {
                top: '40px',
                left: '3%',
                right: '4%',
                bottom: '15%',
                containLabel: true
            },
            xAxis: [{
                type: 'category',
                axisLabel: {
                    interval: 0,
                    margin: 10,
                    rotate: xRotate,
                    
                    textStyle: {
                        fontSize: "14px"
                    }
                },
                data: xAxisData,
                axisTick: {
                    alignWithLabel: true
                }
            }],
            yAxis: [{
                type: 'value',
                axisLabel: {
                    show: true,
                    interval: 'auto',
                    formatter: '{value} %'
                },
                show: true
            }],
            // toolbox: {
            //     show: true,
            //     orient: 'vertical',
            //     left: 'right',
            //     top: 'center',
            //     feature: {
            //         dataView: {
            //             title: "数据",
            //             readOnly: false
            //         },
            //         restore: {},
            //         saveAsImage: {
            //             title: "下载",
            //             type: "jpeg"
            //         }
            //     }
            // },
            dataZoom: [{
                type: 'slider',
                show: true,
                xAxisIndex: [0],
                left: '9%',
                bottom: -5,
                start: 0,
                end: 80 //初始化滚动条
            }],
            series: [{
                name: 'CRE',
                type: 'bar',
                barWidth: 8, //柱图宽度
                data: seriesData1,
                //以下为是否显示，显示位置和显示格式的设置了
                itemStyle: {
                    normal: {

                        //以下为是否显示，显示位置和显示格式的设置了
                        label: {
                            show: true,
                            position: 'top',
                            // formatter: '{c}'
                            formatter: '{c}'
                        }
                    }
                }
            }, {
                    name: 'CRAB',
                    type: 'bar',
                    barWidth: 8, //柱图宽度
                    data: seriesData2,
                    //以下为是否显示，显示位置和显示格式的设置了
                    itemStyle: {
                        normal: {

                            //以下为是否显示，显示位置和显示格式的设置了
                            label: {
                                show: true,
                                position: 'top',
                                // formatter: '{c}'
                                formatter: '{c}'
                            }
                        }
                    }
                }
            ],
            textStyle: {
                fontSize: 10
            }
        };
    } else {

        var option = {
            color: ['#015BAA', '#C1232B', '#FE8463', '#ECBF00', '#CF7CA6'],
            title: {
                text: title,
                subtext: '',
                left: 'center',
                show: true,
                textStyle: {
                    fontWeight: 'normal'
                }
            },
            tooltip: {
                trigger: 'axis',
                axisPointer: { // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
                }
            },
            grid: {
                top: '40px',
                left: '3%',
                right: '4%',
                bottom: '15%',
                containLabel: true
            },
            xAxis: [{
                type: 'category',
                axisLabel: {
                    interval: 0,
                    rotate: xRotate,
                    margin: 10,
                    textStyle: {
                        fontSize: "14px"
                    }
                },
                data: xAxisData,
                axisTick: {
                    alignWithLabel: true
                }
            }],
            yAxis: [{
                type: 'value',
                axisLabel: {
                    show: true,
                    interval: 'auto',
                    formatter: '{value} %'
                },
                show: true
            }],
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
            dataZoom: [{
                type: 'slider',
                show: true,
                xAxisIndex: [0],
                //left: '9%',
                bottom: -5,
                start:0,
                end: 100 //初始化滚动条
            }],
            series: [{
                name: 'CRE',
                type: 'bar',
                barWidth:20, //柱图宽度
                data: seriesData1,
                //以下为是否显示，显示位置和显示格式的设置了
                itemStyle: {
                    normal: {

                        //以下为是否显示，显示位置和显示格式的设置了
                        label: {
                            show: true,
                            position: 'top',
                            // formatter: '{c}'
                            formatter: '{c}'
                        }
                    }
                }
            },
                {
                    name: 'CRAB',
                    type: 'bar',
                    barWidth: 20, //柱图宽度
                    data: seriesData2,
                    //以下为是否显示，显示位置和显示格式的设置了
                    itemStyle: {
                        normal: {

                            //以下为是否显示，显示位置和显示格式的设置了
                            label: {
                                show: true,
                                position: 'top',
                                // formatter: '{c}'
                                formatter: '{c}'
                            }
                        }
                    }
                }
            ],
            textStyle: {
                fontSize: 10
            }
        };
    }


    myChart.resize();
    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
    setTimeout(function() {
        myChart.hideLoading();
    }, 300);
}

/**
 * 取消选择 
 */
function SelectNull() {
    $('#germ_list li').removeClass('active');
    $('#dataError').css('display', 'block');
    $('.main-content2').css('display', 'none');
    $('#PageContentTitle').html('');
}

/**
 * 显示/隐藏数据项
 */
function ShowItemContent() {
    var $itemContent = $('#GermContent'),
        $img = $(event.target);
    if ($itemContent.data('status') === '1') {
        $('#GermContent').slideDown(200, function() {
            $itemContent.data('status', '0');
            $img.attr('src', '/Content/Images/jiantou-shang.png');
        });
    } else {
        $('#GermContent').slideUp(200, function() {
            $itemContent.data('status', '1');
            $img.attr('src', '/Content/Images/jiantou-xia.png');
        });
    }
}


//设置单个项目的显示和隐藏（公共函数）
function ShowStatus(className) {
    var status = $("#" + className).attr("data-status");
    if (status === "1") {
        //隐藏
        $("#" + className).hide("slow");
        $("#" + className).attr("data-status", "0");
        $(".btn-image-" + className).attr("src", "/Content/Images/jiantou-shang.png");
    } else {
        //显示
        $("#" + className).show("slow");
        $("#" + className).attr("data-status", "1");
        $(".btn-image-" + className).attr("src", "/Content/Images/jiantou-xia.png");
    }
}