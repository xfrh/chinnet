
$(function () {
    var dataItemType = document.getElementById('DataItemType').value;
    dataItemType = dataItemType === 'MultipleData' || dataItemType === '2' || dataItemType === 2 ? 2 : 1;
    if (dataItemType === 2) {
        initMultipleDataChart();
    }
    else {
        initSingleDataChart();
    }
});

function initSingleDataChart() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById('chartpreview'));

    myChart.showLoading();
    var xAxisData = JSON.parse(document.getElementById('HiddenxAxisData').value);
    var xRotate = 0;
    if (xAxisData.length > 8 && xAxisData.length <= 12) {
        xRotate = 35;
    }
    else if (xAxisData.length > 12 && xAxisData.length <= 18) {
        xRotate = 45;
    }
    else if (xAxisData.length > 18 && xAxisData.length <= 25) {
        xRotate = 60;
    }
    else if (xAxisData.length > 25) {
        xRotate = 90;
    }
    option = {
        color: [document.getElementById('BarColor').value],
        title: {
            text: document.getElementById('Title').value,
            subtext: document.getElementById('SubTitle').value,
            left: 'center',
            show: true
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: { // 坐标轴指示器，坐标轴触发有效
                type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        grid: {
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
                data: xAxisData,  //'替考拉宁', '万古霉素', '利奈唑胺', '利福平', '庆大霉素', '左氧氟沙星', '环丙沙星', '克林霉素', '复方磺胺甲恶唑', '红霉素', '青霉素', '苯唑西林'
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
                barWidth: 30,//柱图宽度
                data: JSON.parse(document.getElementById('HiddenseriesData').value),//0, 0, 0, 12.7, 31.2, 56.7, 55.7, 42.9, 54.7, 85.7, 100, 100
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
            fontSize: 12
        }
    };
    console.log(option);
    myChart.resize();
    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
    setTimeout(function () {
        myChart.hideLoading();
    }, 300);
}

function initMultipleDataChart() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById('chartpreview'));

    myChart.showLoading();
    var xAxisData = JSON.parse(document.getElementById('HiddenxAxisData').value);
    var xRotate = 0;
    if (xAxisData.length > 8 && xAxisData.length <= 12) {
        xRotate = 35;
    }
    else if (xAxisData.length > 12 && xAxisData.length <= 18) {
        xRotate = 45;
    }
    else if (xAxisData.length > 18 && xAxisData.length <= 25) {
        xRotate = 60;
    }
    else if (xAxisData.length > 25) {
        xRotate = 90;
    }
    option = {
        color: JSON.parse(document.getElementById('HiddenColorList').value),
        title: {
            text: document.getElementById('Title').value,
            subtext: document.getElementById('SubTitle').value,
            left: 'center',
            show: true
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
                    rotate: xRotate,
                    margin: 10,
                    textStyle: {
                        fontSize: "14px"
                    }
                },
                data: xAxisData,  //'替考拉宁', '万古霉素', '利奈唑胺', '利福平', '庆大霉素', '左氧氟沙星', '环丙沙星', '克林霉素', '复方磺胺甲恶唑', '红霉素', '青霉素', '苯唑西林'
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
        series: JSON.parse(document.getElementById('HiddenseriesData').value),
        textStyle:
        {
            fontSize: 12
        }
    };
    console.log(option);
    myChart.resize();
    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
    setTimeout(function () {
        myChart.hideLoading();
    }, 300);

}