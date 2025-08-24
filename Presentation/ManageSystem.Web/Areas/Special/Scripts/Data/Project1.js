$(document).ready(function () {
    // 初始化加载数据项
    $.getJSON('/Areas/Special/Scripts/Data/Project1_p1.json', function (data) {
        var html = template('art_germ_li', { data: data });
        document.getElementById('germ_list').innerHTML = html;
        SelectItem(101);
    });

    // 初始化加载数据项  抗菌药物
    $.getJSON('/Areas/Special/Scripts/Data/Project1_p2.json', function (data) {
        var html = template('art_antibiotic_li', { data: data });
        document.getElementById('antibiotic_list').innerHTML = html;
    });

});

// 基于准备好的dom，初始化echarts实例
var myChart = echarts.init(document.getElementById('main'));

/**
 * 选择数据项
 * @param {int} identity 数据项标识
 */
function SelectItem(identity) {
    var $target = $('#data-' + identity);
    $('.date-item-va').removeClass('active');
    $target.addClass('active');

    $.getJSON('/Areas/Special/Scripts/Data/Project1_p1.json', function (res) {
        res.forEach(function (item, i) {
            if (item.identity === identity) {
                var text = [],
                    data = [],
                    xRotate = 30,
                    title = item.title + '对抗菌药物的耐药率（%）',
                    barWidth = 30;

                item.data.forEach(function (v, j) {
                    text.push(v.name);
                    data.push(v.value);
                });
                LoadCharts(text, data, xRotate, title, barWidth);
                return;
            }
        });
    });
}


//选择抗菌药物，重新加载报表
function SelectItem2(identity) {

    var $target = $('#data-' + identity);
    $('.date-item-va').removeClass('active');
    $target.addClass('active');

    $.getJSON('/Areas/Special/Scripts/Data/Project1_p2.json', function (res) {
        res.forEach(function (item, i) {
            if (item.identity === identity) {
                var text = [],
                    data = [],
                    xRotate = 30,
                    title = item.title,
                    barWidth = 30;

                item.data.forEach(function (v, j) {
                    text.push(v.name);
                    data.push(v.value);
                });
                LoadCharts(text, data, xRotate, title, barWidth);
                return;
            }
        });
    });
}



/**
 * 加载报表数据
 * @param {any} xAxisData x轴显示数据
 * @param {any} seriesData 图表数据
 * @param {any} xRotate x轴数据倾斜角度
 * @param {any} title 图表标题
 * @param {any} barWidth 柱状宽度
 */
function LoadCharts(xAxisData, seriesData, xRotate, title, barWidth) {

    $("#dataError").css("display", "none");
    $(".main-content2").css("display", "block");

    myChart.clear();
    myChart.resize();
    myChart.showLoading();

    var option = {
        color: ['#015BAA'],
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
        xAxis: [
            {
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
        series: [
            {
                name: '数值',
                type: 'bar',
                barWidth: barWidth,//柱图宽度
                data: seriesData,
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
        textStyle:
        {
            fontSize: 10
        }
    };

    myChart.resize();
    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
    setTimeout(function () {
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
        $('#GermContent').slideDown(200, function () {
            $itemContent.data('status', '0');
            $img.attr('src', '/Content/Images/jiantou-shang.png');
        });
    }
    else {
        $('#GermContent').slideUp(200, function () {
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
