var provinceMap = {
    '北京市': '北京',
    '天津市': '天津',
    '河北省': '河北',
    '山西省': '山西',
    '内蒙古自治区': '内蒙古',
    '辽宁省': '辽宁',
    '吉林省': '吉林',
    '黑龙江省': '黑龙江',
    '上海市': '上海',
    '江苏省': '江苏',
    '浙江省': '浙江',
    '安徽省': '安徽',
    '福建省': '福建',
    '江西省': '江西',
    '山东省': '山东',
    '河南省': '河南',
    '湖北省': '湖北',
    '湖南省': '湖南',
    '广东省': '广东',
    '广西壮族自治区': '广西',
    '海南省': '海南',
    '重庆市': '重庆',
    '四川省': '四川',
    '贵州省': '贵州',
    '云南省': '云南',
    '西藏自治区': '西藏',
    '陕西省': '陕西',
    '甘肃省': '甘肃',
    '青海省': '青海',
    '宁夏回族自治区': '宁夏',
    '新疆维吾尔自治区': '新疆',
    '台湾省': '台湾',
    '香港特别行政区': '香港',
    '澳门特别行政区': '澳门'
};

$(function () {
    var dataItems = $('#DataItemConfig').val();
    if (dataItems) {
        var jsonItems = JSON.parse(dataItems);
        var rowCount = (jsonItems.length + 2 - 1) / 2;
        for (var i = 0, len = rowCount; i < len; i++) {

            var skipNum = i * 2;
            var newArr = (skipNum + 2 >= jsonItems.length) ? jsonItems.slice(skipNum, jsonItems.length) : jsonItems.slice(skipNum, skipNum + 2);

            if (newArr && newArr.length > 0) {
                if (newArr && newArr.length === 2) {
                    $('#item_tbody').append(template('tpl_item_table_tr', {
                        row_id: Utils.Common.getUuid(),
                        text_col_1: newArr[0].Name,
                        value_col_1: newArr[0].Value,
                        text_col_2: newArr[1].Name,
                        value_col_2: newArr[1].Value
                    }));
                }
                else {
                    $('#item_tbody').append(template('tpl_item_table_tr', {
                        row_id: Utils.Common.getUuid(),
                        text_col_1: newArr[0].Name,
                        value_col_1: newArr[0].Value,
                        text_col_2: null,
                        value_col_2: null
                    }));
                }
            }
        }

        var mapData = [],
            chinaMap = echarts.init(document.getElementById('china-map'));

        for (var i1 = 0, len1 = jsonItems.length; i1 < len1; i1++) {
            mapData.push({ name: provinceMap[jsonItems[i1].Name], value: jsonItems[i1].Value });
        }

        mapData.push({
            name: '南海诸岛',
            value: '0',
            itemStyle: {
                normal: {
                    opacity: 0,
                    label: {
                        show: false
                    }
                }
            }
        });

        var _max = Math.max.apply(Math, mapData.map(item => { return item.value; }));


        var optionMap = {
            backgroundColor: '#FFFFFF',
            title: {
                text: $('select[id="DataSegmentId"]').find('option:selected').text(),
                subtext: '',
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: function (params) {
                    if (params.value) {
                        return params.name + ': ' + params.value + '%';
                    }
                }
            },
            //左侧小导航图标
            visualMap: {
                show: true,
                type: 'piecewise',
                x: 'left',
                y: 'bottom',
                pieces: [
                    { min: Math.ceil(_max * 3 / 4), max: Math.ceil(_max + 1) },
                    { min: Math.ceil(_max * 2 / 4), max: Math.ceil(_max * 3 / 4) },
                    { min: Math.ceil(_max / 4), max: Math.ceil(_max * 2 / 4) },
                    { min: 0, max: Math.ceil(_max / 4) }
                ],
                inverse: true,
                color: ['#c61620', '#e38788', '#e4bcc4', '#fbdcd7'],
                textStyle: {
                    fontSize: 8,
                    lineHeight: 56
                },
                top: '',
                left: '5%',
                bottom: '10%',
                itemWidth: 25,
                itemHeight: 10,
                itemGap: 5,
                itemSymbol: 'roundRect',
                textGap: 5,
                formatter: function (value, value2) {
                    if (value2 === Math.ceil(_max + 1)) {
                        return '>' + value + '%';
                    }
                    return value + '% - ' + value2 + '%';
                }
            },

            //配置属性
            series: [{
                name: '数据',
                type: 'map',
                mapType: 'china',
                roam: false,
                zoom: 1.15,
                label: {
                    normal: {
                        show: true, //省份名称
                        textStyle: {
                            fontSize: 8
                        },
                        formatter: function (params) {
                            if (params.value) {
                                return params.name + '\n' + params.value + '%';
                            }
                        }
                    },
                    emphasis: {
                        show: true,
                        textStyle: {
                            fontSize: 8
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
                data: mapData  //数据
            }]
        };

        //使用制定的配置项和数据显示图表
        chinaMap.setOption(optionMap);
        chinaMap.resize();
    }
});