
$(document).ready(function () {

    $.ajax({
        url: '/Data/Guanyuchinet',
        method: 'Get',
        data: {},
        dataType: 'JSON',
        success: function (res) {         
            // 基于准备好的dom，初始化echarts实例
            var myChart = echarts.init(document.getElementById('main'));

            function randomData() {
                return Math.round(Math.random() * 1000);
            }

            option = {
                //backgroundColor: '#FFFFFF',
                title: {
                    text: '细菌统计',
                    subtext: '纯属虚构',
                    left: 'center',
                    show: false
                },
                tooltip: {
                    trigger: 'item',
                    formatter: function (params, ticket, callback) {
                        for (var i = 0; i < res[0].Hospital.length; i++) {
                            if (res[0].Hospital[i].name == params.name)
                                return res[0].Hospital[i].value + "";
                        }
                    }
                },
                // legend: {
                //    orient: 'vertical',
                //    left: 'left',
                // data: ['细菌数量']
                //  },
                visualMap: {
                    min: 0,
                    max: 5,
                    left: 'left',
                    top: 'bottom',
                    //text: ['高', '低'],           // 文本，默认为数值文本
                    calculable: true,
                    color: ['#c61620', '#e38788', '#e4bcc4', '#fbdcd7'],
                    show: false
                },
                toolbox: {
                    show: false,
                    orient: 'vertical',
                    left: 'right',
                    top: 'center',
                    feature: {
                        dataView: { readOnly: false },
                        restore: {},
                        saveAsImage: {
                            type: "jpeg"
                        }
                    }
                },
                series: [
                    {
                        name: '细菌数量',
                        type: 'map',
                        mapType: 'china',
                        roam: false,
                        label: {
                            normal: {
                                show: true,
                                textStyle: {
                                    fontSize: 14
                                },
                            },
                            emphasis: {
                                show: true,
                                textStyle: {
                                    fontSize: 14,
                                    align: 'right'
                                }
                            }
                        },
                        itemStyle: {
                            normal: {
                                borderColor: '#fefefe',
                                //color: '#DBFCFE',
                                areaColor: '#ddd',
                                borderWidth: 2
                            },
                            emphasis: {
                                show: true
                            }
                        },
                        data: res[0].ProvinceName                    
                    }
                ]
            };
            // 使用刚指定的配置项和数据显示图表。
            myChart.setOption(option);

        }
    });

    
});


