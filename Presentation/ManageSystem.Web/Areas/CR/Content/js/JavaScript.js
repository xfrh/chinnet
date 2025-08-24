var myChart = echarts.init(document.getElementById('main'));

// 指定图表的配置项和数据
var option = {
    title: {
        text: '模考分数对比',
        subtext: '纯属虚构'
    },
    tooltip: {
        trigger: 'axis'
    },
    legend: {
        data: ['一模', '二模', '三模']
    },
    toolbox: {
        show: true,
        feature: {
            dataView: { show: true, readOnly: false },
            magicType: { show: true, type: ['line', 'bar'] },
            restore: { show: true },
            saveAsImage: { show: true }
        }
    },
    calculable: true,
    xAxis: [
        {
            type: 'category',
            data: ['数学', '语文', '英语', '综合']
        }
    ],
    yAxis: [
        {
            type: 'value'
        }
    ],
    series: [
        {
            name: '一模',
            type: 'bar',
            data: [78, 80, 87, 93],
            color: '#CC0066'
        },
        {
            name: '二模',
            type: 'bar',
            data: [90, 77, 62, 76],
            color: '#009999'
        },
        {
            name: '三模',
            type: 'bar',
            data: [91, 78, 87, 89],
            color: '#FFCC33'
        }
    ]
};
// 使用刚指定的配置项和数据显示图表。
myChart.setOption(option);