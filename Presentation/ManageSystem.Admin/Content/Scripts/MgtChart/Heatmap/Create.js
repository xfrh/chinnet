var geoCoordMap = {
    '北京市': [116.405285, 39.904989],
    '天津市': [117.190182, 39.125596],
    '河北省': [114.502461, 38.045474],
    '山西省': [112.549248, 37.857014],
    '内蒙古自治区': [111.670801, 40.818311],
    '辽宁省': [123.429096, 41.796767],
    '吉林省': [125.3245, 43.886841],
    '黑龙江省': [126.642464, 45.756967],
    '上海市': [121.472644, 31.231706],
    '江苏省': [118.767413, 32.041544],
    '浙江省': [120.153576, 30.287459],
    '安徽省': [117.283042, 31.86119],
    '福建省': [119.306239, 26.075302],
    '江西省': [115.892151, 28.676493],
    '山东省': [117.000923, 36.675807],
    '河南省': [113.665412, 34.757975],
    '湖北省': [114.298572, 30.584355],
    '湖南省': [112.982279, 28.19409],
    '广东省': [113.280637, 23.125178],
    '广西壮族自治区': [108.320004, 22.82402],
    '海南省': [110.33119, 20.031971],
    '重庆市': [106.504962, 29.533155],
    '四川省': [104.065735, 30.659462],
    '贵州省': [106.713478, 26.578343],
    '云南省': [102.712251, 25.040609],
    '西藏自治区': [91.132212, 29.660361],
    '陕西省': [108.948024, 34.263161],
    '甘肃省': [103.823557, 36.058039],
    '青海省': [101.778916, 36.623178],
    '宁夏回族自治区': [106.278179, 38.46637],
    '新疆维吾尔自治区': [87.617733, 43.792818],
    '台湾省': [121.509062, 25.044332],
    '香港特别行政区': [114.173355, 22.320048],
    '澳门特别行政区': [113.54909, 22.198951]
};

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
    onAddItemRow();
});

/* 添加数据项 */
onAddItemRow = function () {
    if ($('#item_tbody tr').length > 34) {
        toastr.warning('不能再添加了数据了');
        return false;
    }
    var input_id = Utils.Common.getUuid(),
        input_name = _.uniqueId('ItemModel[].Value_');

    $('#item_tbody').append(template('tpl_item_table_tr', {
        row_id: Utils.Common.getUuid(),
        dropProvinces: provinceMap,
        province: '',
        select_name: 'ItemModel[].Name',
        input_id: input_id,
        input_name: input_name,
        input_value: ''
    }));
    setTimeout(function () {
        $('input[id="' + input_id + '"]').rules('add', {
            required: true,
            number: true,
            min: 0,
            messages: {
                required: "请录入数值",
                number: "请录入正确的数值",
                min: "值不能小于0"
            }
        });
    }, 20);
};

/*插入数据项*/
onAddItemRow1 = function (id) {
    var l = Ladda.create(document.getElementById('btnAddItemRow'));
    if (l.isLoading()) { return false; }
    l.start();
    var $tbody = $('#single_datatable > tbody#item_tbody');
    var arrSort = [],
        arrData = [];

    $tbody.find('input[name^="ItemModel"][name$=".Sort"]').each(function (i, item) {
        if (!s.isBlank(item.value)) {
            var _sort = parseInt(item.value) || 0;
            if (_sort && _sort > 0) {
                arrSort.push(_sort);
            }
        }
    });

    arrData.push({
        uuid: Utils.Common.getUuid(),
        name: null,
        value: null,
        display: true,
        //sort: arrSort.length > 0 ? Math.max(...arrSort) + 1 : null
    });
    setTimeout(function () {
        $(id).after(template('template_singletable_tbody', { data: arrData })).updateSingleDataTableName();
        var length = $tbody.find('input[name^="ItemModel"][name$=".Sort"]').length;
        for (var i = 0; i < length; i++) {
            $('input[name="ItemModel[' + i + '].Sort"]').val(i + 1);
        }
        setTimeout(function () {
            l.stop();
        }, 300);
    }, 300);

}

/* 删除数据项 */
onDeleteItem = function (id) {
    $(id).remove();
};

/* 预览 */
onPreview = function () {
    $('#modal-preview').modal('show');
};

$('#modal-preview').on('shown.bs.modal', function () {
    var chinaMap = echarts.init(document.getElementById('china-map'));
    var data = [],
        names = [];

    $('#item_tbody tr[id]').each(function (i, item) {
        var name = item.querySelector('select[name="ItemModel[].Name"]').value;
        var value = item.querySelector('input[name^="ItemModel[].Value_"]').value;

        if (names.indexOf(name) === -1 && !s.isBlank(value)) {
            names.push(name);
            data.push({ name: name, value: value });
        }
    });

    data.push({
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

    var _max = Math.max.apply(Math, data.map(item => { return item.value; }));

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
            data: data  //数据
        }]
    };

    //使用制定的配置项和数据显示图表
    chinaMap.setOption(optionMap);
    chinaMap.resize();
});

onExcel = function () {
    $('#modal-excel').modal('show');
};

onSubmitExcel = function () {
    var l = Ladda.create(document.getElementById('btnSubmit')),
        files = $('#excelFile')[0].files,
        formData = new FormData();

    if (!files || files.length <= 0) {
        alert('请选择热图数据上传文件');
        return false;
    }
    if (!l.isLoading()) {
        formData.append('file', files[0]);
        $.ajax({
            url: '/MgtChart/HeatmapExcel_OnSubmit',
            method: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            beforeSend: function () { l.start(); },
            complete: function () { setTimeout(function () { l.stop(); }, 600); },
            success: function (res) {
                if (res.status) {
                    var existData = [];
                    $('#item_tbody tr[id]').each(function (i, item) {
                        var selectedIndex = item.querySelector('select[name^="ItemModel"]').options.selectedIndex;
                        var text = item.querySelector('select[name^="ItemModel"]').options[selectedIndex].text;
                        existData.push({ province: text, input_el: item.querySelector('input[name^="ItemModel"]') });
                    });
                    for (i = 0, len = res.data.length; i < len; i++) {
                        var exist_item = existData.find(item => item.province === res.data[i].name);
                        if (exist_item) {
                            exist_item.input_el.value = res.data[i].value;
                        }
                        else {
                            var input_id = Utils.Common.getUuid(),
                                input_name = _.uniqueId('ItemModel[].Value_');
                            $('#item_tbody').append(template('tpl_item_table_tr', {
                                row_id: Utils.Common.getUuid(),
                                dropProvinces: provinceMap,
                                province: res.data[i].name,
                                select_name: 'ItemModel[].Name',
                                input_id: input_id,
                                input_name: input_name,
                                input_value: res.data[i].value
                            }));
                            setTimeout(function () {
                                $('input[id="' + input_id + '"]').rules('add', {
                                    required: true,
                                    number: true,
                                    min: 0,
                                    messages: {
                                        required: "请录入数值",
                                        number: "请录入正确的数值",
                                        min: "值不能小于0"
                                    }
                                });
                            }, 20);
                        }
                    }

                    $('#modal-excel').modal('hide');
                }
                else {
                    alert(res.message);
                }
            }
        });
    }
    return false;
};

$('#modal-excel').on('hidden.bs.modal', function () {
    if (!!window.ActiveXObject || "ActiveXObject" in window) {
        $('#excelFile').replaceWith($('#excelFile').clone(true));
    }
    else {
        $('#excelFile').val('');
    }
});

onSubmit = function () {
    $('#item_tbody tr[id]').each(function (i, item) {
        if (item.querySelector('select[name^="ItemModel"]').name === 'ItemModel[].Name') {
            var input_id = item.querySelector('input[name^="ItemModel[].Value"]').id;
            item.querySelector('select[name="ItemModel[].Name"]').name = 'ItemModel[' + i + '].Name';
            item.querySelector('input[name^="ItemModel[].Value_"]').name = 'ItemModel[' + i + '].Value';
            $('input[id="' + input_id + '"]').rules('remove');
            $('input[id="' + input_id + '"]').rules('add', {
                required: true,
                number: true,
                min: 0,
                messages: {
                    required: "请录入数值",
                    number: "请录入正确的数值",
                    min: "值不能小于0"
                }
            });
        }
    });

    return true;
};