
var dataArray = new Array();
dataArray.push({ Name: '安徽', Value: '安徽医科大学第一附属医院' });
dataArray.push({ Name: '北京', Value: '北京儿童医院<br/>北京协和医院<br/>北京医院' });
dataArray.push({ Name: '上海', Value: '复旦大学附属儿科医院<br/>复旦大学附属华山医院<br/>上海交通大学医学院附属瑞金医院<br/>上海浦东新区人民医院<br/>上海市儿童医院' });
dataArray.push({ Name: '福建', Value: '福建晋江市医院<br/>福建省泉州市第一医院<br/>厦门大学附属第一医院' });
dataArray.push({ Name: '甘肃', Value: '甘肃省人民医院' });
dataArray.push({ Name: '广东', Value: '广州医学院第一附属医院<br />深圳市人民医院' });
dataArray.push({ Name: '黑龙江', Value: '哈尔滨医科大学第一附属医院' });
dataArray.push({ Name: '河南', Value: '河南省人民医院' });
dataArray.push({ Name: '湖北', Value: '华中科技大学同济医学院附属同济医院<br/>湖北秭归县人民医院' });
dataArray.push({ Name: '吉林', Value: '吉林大学中日联谊医院' });
dataArray.push({ Name: '江西', Value: '江西省儿童医院' });
dataArray.push({ Name: '云南', Value: '昆明医科大学第一附属医院' });
dataArray.push({ Name: '四川', Value: '四川省人民医院<br/>四川大学华西医院' });
dataArray.push({ Name: '江苏', Value: '南京鼓楼医院' });
dataArray.push({ Name: '内蒙古', Value: '内蒙古医科大学第一附属医院' });
dataArray.push({ Name: '浙江', Value: '宁波龙赛医院<br />浙江大学医学院附属邵逸夫医院' });
dataArray.push({ Name: '宁夏', Value: '宁夏医科大学总医院' });
dataArray.push({ Name: '山东', Value: '山东省立医院' });
dataArray.push({ Name: '陕西', Value: '陕西省人民医院' });
dataArray.push({ Name: '山西', Value: '山西省儿童医院' });
dataArray.push({ Name: '天津', Value: '天津医科大学总医院' });
dataArray.push({ Name: '新疆', Value: '新疆医科大学第一附属医院' });
dataArray.push({ Name: '辽宁', Value: '中国医科大学第一附属医院' });
dataArray.push({ Name: '湖南', Value: '中南大学湘雅医院' });
dataArray.push({ Name: '西藏', Value: '西藏军区总医院' });
dataArray.push({ Name: '河北', Value: '河北医科大学第二医院' });
dataArray.push({ Name: '贵州', Value: '贵州省人民医院' });


$(document).ready(function () {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById('main'));

    function randomData() {
        return Math.round(Math.random() * 1000);
    }

    option = {
        backgroundColor: '#FFFFFF',
        title: {
            text: '细菌统计',
            subtext: '纯属虚构',
            left: 'center',
            show: false
        },
        tooltip: {
            trigger: 'item',
            formatter: function (params, ticket, callback) {                
                for (var i = 0; i < dataArray.length; i++) {
                    if (dataArray[i].Name == params.name)
                        return dataArray[i].Value + "";
                }
            }
        },
        //legend: {
        //    orient: 'vertical',
        //    left: 'left',
        //    data: ['细菌数量']
        //},
        visualMap: {
            min: 0,
            max: 5,
            left: 'left',
            top: 'bottom',
            //text: ['高', '低'],           // 文本，默认为数值文本
            inverse: true,
            color: ['#c61620', '#e38788', '#e4bcc4', '#fbdcd7'],
            show: false,
            itemSymbol: 'roundRect'
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
                    type:"jpeg"
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
                        show: true, //省份名称
                        textStyle: {
                            fontSize: 14
                        },
                        formatter: function (params) {
                            //if (params.value) {
                            //    return params.name + '\n' + params.value + '%';
                            //}
                        }
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
                        areaColor: '#ddd',
                        borderColor: '#fefefe',
                        borderWidth: 2
                    },
                    emphasis: {
                        show: true
                    }
                },
                data: [
                    { name: '北京', value: 3 },
                    { name: '天津', value: 1 },
                    { name: '上海', value: 5 },
                    //{ name: '重庆', value: 0 },
                    { name: '河北', value: 1 },
                    { name: '河南', value: 1 },
                    { name: '云南', value: 1 },
                    { name: '辽宁', value: 1 },
                    { name: '黑龙江', value: 1 },
                    { name: '湖南', value: 1 },
                    { name: '安徽', value: 1 },
                    { name: '山东', value: 1 },
                    { name: '新疆', value: 1 },
                    { name: '江苏', value: 1 },
                    { name: '浙江', value: 2 },
                    { name: '江西', value: 1 },
                    { name: '湖北', value: 2 },
                    //{ name: '广西', value: 0 },
                    { name: '甘肃', value: 1 },
                    { name: '山西', value: 1 },
                    { name: '内蒙古', value: 1 },
                    { name: '陕西', value: 1 },
                    { name: '吉林', value: 1 },
                    { name: '福建', value: 3 },
                    { name: '贵州', value: 1 },
                    { name: '广东', value: 2 },
                    //{ name: '青海', value: 0 },
                    { name: '西藏', value: 1 },
                    { name: '四川', value: 2 },
                    { name: '宁夏', value: 1 },
                    //{ name: '海南', value: 0 },
                    //{ name: '台湾', value: 0 },
                    //{ name: '香港', value: 0 },
                    //{ name: '澳门', value: 0 }
                ]
            }
        ]
    };
    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
});


