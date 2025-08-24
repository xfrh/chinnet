
$(document).ready(function () {

    //检查是否登录的跳转
    var isHaveLogin = $("#isHaveLogin").val();
    if (isHaveLogin !== null && isHaveLogin === "login") {
        OpenLogin();
    }
    else if (isHaveLogin !== null && isHaveLogin === "regist") {
        OpenRegist();
    }

    if ($('a[onclick^="renderChinaMap"][data-default="1"]').length > 0) {
        $('a[onclick^="renderChinaMap"][data-default="1"]').click();
    }
    else {
        $('a[onclick^="renderChinaMap"]:eq(0)').closest('ul').find('li:last > a').click();
    }
});

function OpenRegist() {
    layer.open({
        title: false,
        type: 2,
        area: '800px',
        fixed: false, //不固定
        maxmin: false,
        content: '/Regist?returnUrl=' + $("#returnUrl").val(),
        success: function (layero, index) {
            layer.iframeAuto(index);
        }
    });
}

function OpenLogin() {
    layer.open({
        title: false,
        type: 2,
        area: '800px',
        fixed: false, //不固定
        maxmin: false,
        content: '/Login/OpenIndex?returnUrl=' + $("#returnUrl").val(),
        success: function (layero, index) {
            layer.iframeAuto(index);
        }
    });
}


//登录成功的回调
function OpenLoginCallback(returnUrl) {
    if (returnUrl === undefined || typeof returnUrl === 'undefined') {
        returnUrl = $("#returnUrl").val();
    }
    if (returnUrl !== "") {
        location.href = returnUrl;
        return;
    } else {
        location.href = "/";
    }
}



var chinaMap = undefined;
renderChinaMap = function (id, event) {
    if (id) {
        $('#sjd-container li.active').removeClass('active');
        $('#li' + id).addClass('active');
        $.ajax({
            url: '/Chinet/IndexData',
            method: 'POST',
            data: { id: id },
            dataType: 'JSON',
            success: function (res) {
                chinaMap = echarts.init(document.getElementById('mainChart'));
                var _arr_values = [];
                for (var i = 0, len = res.data.length; i < len; i++) {
                    _arr_values.push(res.data[i].value);
                }
                var _max = Math.max.apply(Math, _arr_values);

                res.data.push({
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

                var optionMap = {
                    backgroundColor: '#FFFFFF',
                    title: {
                        text: res.title,
                        subtext: '',
                        x: 'center'
                    },
                    tooltip: {
                        trigger: 'item',
                        formatter: function (params) {
                            if (params.value || params.value==0) {
                                return params.name + ': ' + params.value + '%';
                            }
                        }
                    },
                    //左侧小导航图标
                    visualMap: {
                        show: true,
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
                            fontSize: 12,
                            lineHeight: 56
                        },
                        itemWidth: 45,
                        itemHeight: 15,
                        itemGap: 5,
                        itemSymbol: 'roundRect',
                        textGap: 5,
                        formatter: function (value, value2) {
                            if (value2 === Math.ceil(_max + 1)) {
                                return '>' + value + '%';
                            }
                            return value + '% - ' + value2 + '%';
                        },
                        top: '',
                        left: '10%',
                        bottom: '10%'
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
                        data: res.data  //数据
                    }]
                };

                //使用制定的配置项和数据显示图表
                chinaMap.setOption(optionMap);
                chinaMap.resize();

                chinaMap.on('mouseover', function (params) {
                    if (isNaN(params.value)) {
                        chinaMap.dispatchAction({
                            type: 'downplay'
                        });
                    }
                });
            }
        });
    }
    return false;
};

