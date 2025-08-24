/// <reference path="province/anhui.js" />
var myChart = echarts.init(document.getElementById('mains'));

var oBack = document.getElementById("back");

var provinces = ['shanghai', 'hebei', 'shanxi', 'neimenggu', 'liaoning', 'jilin', 'heilongjiang', 'jiangsu', 'zhejiang', 'anhui', 'fujian', 'jiangxi', 'shandong', 'henan', 'hubei', 'hunan', 'guangdong', 'guangxi', 'hainan', 'sichuan', 'guizhou', 'yunnan', 'xizang', 'shanxi1', 'gansu', 'qinghai', 'ningxia', 'xinjiang', 'beijing', 'tianjin', 'chongqing', 'xianggang', 'aomen'];

var provincesText = ['上海', '河北', '山西', '内蒙古', '辽宁', '吉林', '黑龙江', '江苏', '浙江', '安徽', '福建', '江西', '山东', '河南', '湖北', '湖南', '广东', '广西', '海南', '四川', '贵州', '云南', '西藏', '陕西', '甘肃', '青海', '宁夏', '新疆', '北京', '天津', '重庆', '香港', '澳门'];

var toolTipData = [];
var seriesData = [];
var provinceData = [];
var seriesDataPro = [];
var attr = [];
var col = [];
var _arr_values = [];
var _max = "";
var title = "";
$(function () {
    $.ajax({
        url: "/CR/CRData/GetCRslist",
        type: "post",
        async: false,
        dataType: "json",
        data: { year: "" },
        success: function (data) {
            //data = JSON.parse(data)
            for (var i = 0; i < data.length; i++) {
                // var Region = data[i].Region
                _arr_values.push(data[i].cre);
                toolTipData.push(
                    {
                        "provinceName": data[i].Province,
                        "cre": data[i].cre,
                        "crab": data[i].crab,
                    }
                )
                _max = Math.max.apply(Math, _arr_values);

            }
        }

    });

    for (var i = 0; i < toolTipData.length; i++) {
        seriesData[i] = {};
        seriesData[i].name = toolTipData[i].provinceName;
        seriesData[i].value = toolTipData[i].cre;
        seriesData[i].provinceKey = toolTipData[i].crab;
    }

    $.ajax({
        url: "/CR/CRData/GetCitylist",
        type: "post",
        async: false,
        dataType: "json",
        data: { year: "" },
        success: function (data) {

            for (var i = 0; i < data.length; i++) {

                provinceData.push({
                    "cityName": data[i].City,
                    "cre": data[i].cre,
                    "crab": data[i].crab,
                });
            };
        }

    });

    for (var i = 0; i < provinceData.length; i++) {
        seriesDataPro[i] = {};
        seriesDataPro[i].name = provinceData[i].cityName;
        seriesDataPro[i].value = provinceData[i].cre;
    }
    initEcharts("china", "中国");
})
oBack.onclick = function () {
    $('#back').addClass('hidden');
    initEcharts("china", "中国");
};

var year = $("#year").val();
title = "各省份CRE，CRAB检出率" + "(" + year + ")";
$("[data-year]").on("click", function () {  
    _arr_values = [];
    $('#dataError').css('display', 'none');
    $('.main-content2').css('display', 'block');
    $('#back').addClass('hidden');
    var year1 = $(this).attr("data-year");
    $("[data-year]").removeClass("active");
    $(this).addClass("active");
     title = "各省份CRE，CRAB检出率" + "(" + year1 + ")";
    toolTipData = [];
    seriesData = [];
    provinceData = [];
    seriesDataPro = [];

    $.ajax({
        url: "/CRData/GetCRslist",
        type: "post",
        async: false,
        dataType: "json",
        data: { year: year1 },
        success: function (data) {
            // data = JSON.parse(data)
            for (var i = 0; i < data.length; i++) {
                _arr_values.push(data[i].cre);
                toolTipData.push(
                    {
                        "provinceName": data[i].Province,
                        "cre": data[i].cre,
                        "crab": data[i].crab,
                    },
                )
                _max = Math.max.apply(Math, _arr_values);

            };
        }

    });

    for (var i = 0; i < toolTipData.length; i++) {
        seriesData[i] = {};
        seriesData[i].name = toolTipData[i].provinceName;
        seriesData[i].value = toolTipData[i].cre;
        seriesData[i].provinceKey = toolTipData[i].crab;
    }
    $.ajax({
        url: "/CR/CRData/GetCitylist",
        type: "post",
        async: false,
        dataType: "json",
        data: { year: year1 },
        success: function (data) {

            for (var i = 0; i < data.length; i++) {

                provinceData.push({
                    "cityName": data[i].City,
                    "cre": data[i].cre,
                    "crab": data[i].crab,
                });
            };
        }

    });

    for (var i = 0; i < provinceData.length; i++) {
        seriesDataPro[i] = {};
        seriesDataPro[i].name = provinceData[i].cityName;
        seriesDataPro[i].value = provinceData[i].cre;
    }
    initEcharts("china", "中国");
})
function isMobile() {
    try {
        document.createEvent("TouchEvent");
        return true;
    } catch (e) {
        return false;
    }
}
function initEcharts(pName, Chinese_) {
    var tmpSeriesData = pName === "china" ? seriesData : seriesDataPro;
    var tmp = pName === "china" ? toolTipData : provinceData;
    tmpSeriesData.push({
        name: '南海诸岛',
        value: '0',
        itemStyle: {
            normal: {
                opacity: 0,
                label: {
                    show: false,
                    normal: {
                        show: false,
                    },
                    emphasis: {//对应的鼠标悬浮效果
                        show: false,
                    }
                }
            }
        }
    });
    if (isMobile()) {
        var option = {
            title: {
                show: true,
                text: title,
                x: 'center',
                textStyle: {
                    fontSize: 12
                }
            },
            tooltip: {
                trigger: 'item',
                formatter: function (params) {
                    if (isNaN(params.value)) {
                        return params.name + "\n" + "无";
                    }
                    else if (pName === "china") {
                        var toolTiphtml = ''
                        for (var i = 0; i < tmp.length; i++) {
                            if (params.name == tmp[i].provinceName) {
                                toolTiphtml += tmp[i].provinceName + '<br>CRE：' + tmp[i].cre + '%<br>CRAB：' + tmp[i].crab + '%';
                            }
                        }
                        return toolTiphtml;
                    } else {
                        var toolTiphtml = ''
                        for (var i = 0; i < tmp.length; i++) {
                            if (params.name == tmp[i].cityName) {
                                toolTiphtml += tmp[i].cityName + '<br>CRE：' + tmp[i].cre + '%<br>CRAB：' + tmp[i].crab + '%';
                            }
                        }
                        return toolTiphtml;
                    }
                },
            },

            visualMap: {
                // min: 0,       		// 值域最小值，必须参数
                // max: 30,			// 值域最大值，必须参数
                show: true,
                x: 'left',
                y: 'bottom',
                //calculable: false,
                //splitList: attr,
                pieces: [
                    { min: Math.ceil(_max * 3 / 4) },
                    { min: Math.ceil(_max * 2 / 4), max: Math.ceil(_max * 3 / 4) },
                    { min: Math.ceil(_max / 4), max: Math.ceil(_max * 2 / 4) },
                    { min: 0, max: Math.ceil(_max / 4) }
                ],
                inverse: true,
                color: ['#c61620', '#e38788', '#e4bcc4', '#fbdcd7'],
                textStyle: {
                    fontSize: 8,
                    lineHeight:56
                },
                itemWidth:25,
                itemHeight: 10,
                itemGap: 5,
                itemSymbol: 'roundRect',
                textGap: 5,
                formatter: function (value, value2) {
                    if (value === Math.ceil(_max * 3 / 4)) {
                        return "CRE检出率： " + '>' + value + '%';
                    }
                    return "CRE检出率： " + value + ' - ' + value2 + '%';
                },
                top: '60%',
                left: '2%',
                bottom: '10%',
                // dimension:1,   //指定用数据的『哪个维度』，映射到视觉元素上。『数据』即 series.data。 可以把 series.data 理解成一个二维数组,其中每个列是一个维度,默认取 data 中最后一个维度
                // seriesIndex:1, //指定取哪个系列的数据，即哪个系列的 series.data,默认取所有系列
            },
            //toolbox: {
            //    show: true,
            //    orient: 'vertical',
            //    left: 'right',
            //    top: 'center',
            //    feature: {
            //        mark: { show: true },
            //        dataView: { show: true, readOnly: false },
            //        restore: { show: true },
            //        saveAsImage: { show: true ,type:"jpg"}
            //    }
            //},
            geo: {
                show: false,
                map: pName,
                roam: false,
                label: {
                    normal: {
                        show: false
                    },
                    emphasis: {
                        show: false,
                    }
                },
                //itemStyle: {
                //    normal: {
                //        areaColor: '#3c8dbc', // 没有值得时候颜色
                //        borderColor: '#097bba',
                //    },
                //    emphasis: {
                //        areaColor: '#fbd456', // 鼠标滑过选中的颜色
                //    }
                //}
            },
            series: [
                {
                    name: Chinese_ || pName,
                    type: 'map',
                    mapType: pName,
                    roam: true,//是否开启鼠标缩放和平移漫游
                    data: tmpSeriesData,
                    top: "10%",//组件距离容器的距离
                    zoom: 1.1,
                    selectedMode: 'single',
                    //showLegendSymbol: false,
                    label: {
                        normal: {
                            show: true,//显示省份标签
                            //formatter: function (params) {
                            //    return params.name + "\n KPC: \n " + params.value + "% NDM: \n" + params.provinceKey + "%";    //地图上展示文字 + 数值
                            //},
                            textStyle: {
                                color: "#545454",
                                fontSize: 8
                            }//省份标签字体颜色
                        },
                        emphasis: {//对应的鼠标悬浮效果
                            show: true,
                            textStyle: {
                                color: "#545454",
                                fontSize: 8
                            }
                        }
                    },
                    itemStyle: {
                        normal: {
                            borderWidth: 2,//区域边框宽度
                            borderColor: '#FFFFFF',//区域边框颜色
                            areaColor: '#CFCFCF',//区域颜色
                            shadowOffsetX: 0,
                            shadowOffsetY: 0,

                        },

                        emphasis: {
                            areaColor: '',
                            shadowOffsetX: 0,
                            shadowOffsetY: 0,
                            shadowBlur: 20,
                            borderWidth: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        },
                    },                  
                },
            ],
             textStyle: {
                fontSize: 10
            }

        }
        var myChart = echarts.init(document.getElementById('main'));

        myChart.setOption(option);
        $("#mains").addClass('hidden');
        myChart.off("click");

        var n = 0;
        if (pName === "china") { // 全国时，添加click 进入省级
            myChart.on('click', function (param) {
                n++;
                setTimeout(function () {
                    n = 0;
                }, 500);
                if (n > 1) {
                    n = 0;
                    if (param.name === "") {
                        initEcharts("china", "中国");
                        return;
                    }
                    //console.log(param.name);
                    $('#back').removeClass('hidden');
                    // 遍历取到provincesText 中的下标  去拿到对应的省js
                    for (var i = 0; i < provincesText.length; i++) {
                        if (param.name === provincesText[i]) {
                            //显示对应省份的方法
                            showProvince(provinces[i], provincesText[i]);
                            break;
                        }
                    }
                    if (param.componentType === 'series') {
                        var provinceName = param.name;
                        $('#box').css('display', 'block');
                        $("#box-title").html(
                        );

                    }
                }
            });           
        }
    } else {
        var option = {
            title: {
                text: title,
                left: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: function (params) {
                    if (isNaN(params.value)) {
                        return params.name + "\n" + "无";
                    }
                    else if (pName === "china") {
                        var toolTiphtml = ''
                        for (var i = 0; i < tmp.length; i++) {
                            if (params.name == tmp[i].provinceName) {
                                toolTiphtml += tmp[i].provinceName + '<br>CRE：' + tmp[i].cre + '%<br>CRAB：' + tmp[i].crab + '%';
                            }
                        }
                        return toolTiphtml;
                    } else {
                        var toolTiphtml = ''
                        for (var i = 0; i < tmp.length; i++) {
                            if (params.name == tmp[i].cityName) {
                                toolTiphtml += tmp[i].cityName + '<br>CRE：' + tmp[i].cre + '%<br>CRAB：' + tmp[i].crab + '%';
                            }
                        }
                        return toolTiphtml;
                    }
                },
            },

            visualMap: {
                // min: 0,       		// 值域最小值，必须参数
                // max: 30,			// 值域最大值，必须参数
                show: true,
                x: 'left',
                y: 'bottom',
                //calculable: false,
                //splitList: attr,
                pieces: [
                    { min: Math.ceil(_max * 3 / 4) },
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
                    if (value === Math.ceil(_max * 3 / 4)) {
                        return "CRE检出率： " + '>' + value + '%';
                    }
                    return "CRE检出率： " + value + ' - ' + value2 + '%';
                },
                top: '80%',
                left: '10%',
                bottom: '10%',
                // dimension:1,   //指定用数据的『哪个维度』，映射到视觉元素上。『数据』即 series.data。 可以把 series.data 理解成一个二维数组,其中每个列是一个维度,默认取 data 中最后一个维度
                // seriesIndex:1, //指定取哪个系列的数据，即哪个系列的 series.data,默认取所有系列
            },
            //toolbox: {
            //    show: true,
            //    orient: 'vertical',
            //    left: 'right',
            //    top: 'center',
            //    feature: {
            //        mark: { show: true },
            //        dataView: { show: true, readOnly: false },
            //        restore: { show: true },
            //        saveAsImage: { show: true ,type:"jpg"}
            //    }
            //},
            geo: {
                show: false,
                map: pName,
                roam: false,
                label: {
                    normal: {
                        show: false
                    },
                    emphasis: {
                        show: false,
                    }
                },
                //itemStyle: {
                //    normal: {
                //        areaColor: '#3c8dbc', // 没有值得时候颜色
                //        borderColor: '#097bba',
                //    },
                //    emphasis: {
                //        areaColor: '#fbd456', // 鼠标滑过选中的颜色
                //    }
                //}
            },
            series: [
                {
                    name: Chinese_ || pName,
                    type: 'map',
                    mapType: pName,
                    roam: false,//是否开启鼠标缩放和平移漫游
                    data: tmpSeriesData,
                    top: "5%",//组件距离容器的距离
                    zoom: 1.1,
                    selectedMode: 'single',
                    //showLegendSymbol: false,
                    label: {
                        normal: {
                            show: true,//显示省份标签
                            //formatter: function (params) {
                            //    return params.name + "\n KPC: \n " + params.value + "% NDM: \n" + params.provinceKey + "%";    //地图上展示文字 + 数值
                            //},
                            textStyle: {
                                color: "#545454",
                                fontSize: 12

                            }//省份标签字体颜色
                        },
                        emphasis: {//对应的鼠标悬浮效果
                            show: true,
                            textStyle: {
                                color: "#545454",
                                fontSize: 12
                            }
                        }
                    },
                    itemStyle: {
                        normal: {
                            borderWidth: 2,//区域边框宽度
                            borderColor: '#FFFFFF',//区域边框颜色
                            areaColor: '#CFCFCF',//区域颜色
                            shadowOffsetX: 0,
                            shadowOffsetY: 0,

                        },

                        emphasis: {
                            areaColor: '',
                            shadowOffsetX: 0,
                            shadowOffsetY: 0,
                            shadowBlur: 20,
                            borderWidth: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        }
                    },
                },
            ],
            textStyle: {
                fontSize: 10
            }

        }
        var myChart = echarts.init(document.getElementById('mains'));

        myChart.setOption(option);
        $("#main").addClass('hidden');
        myChart.off("click");

        if (pName === "china") { // 全国时，添加click 进入省级
            myChart.on('click', function (param) {
                if (param.name === "") {
                    // xtip.alert('通知');
                    MessageWindow("此地区暂无数据！");
                    initEcharts("china", "中国");
                    return;
                }
                //console.log(param.name);
                $('#back').removeClass('hidden');
                // 遍历取到provincesText 中的下标  去拿到对应的省js
                for (var i = 0; i < provincesText.length; i++) {
                    if (param.name === provincesText[i]) {
                        //显示对应省份的方法
                        showProvince(provinces[i], provincesText[i]);
                        break;
                    }
                }
                if (param.componentType === 'series') {
                    var provinceName = param.name;
                    $('#box').css('display', 'block');
                    $("#box-title").html(
                    );

                }
            });
        } else { // 省份，添加双击 回退到全国
            myChart.on("dblclick", function () {
                $('#back').addClass('hidden');
                initEcharts("china", "中国");
            });
        }
    }   
}

// 展示对应的省
function showProvince(pName, Chinese_) {
    //这写省份的js都是通过在线构建工具生成的，保存在本地，需要时加载使用即可，最好不要一开始全部直接引入。
    loadBdScript('$' + pName + 'JS', '/Areas/CR/Content/js/province/' + pName + '.js', function () {
        initEcharts(Chinese_);
    });
}

// 加载对应的JS
function loadBdScript(scriptId, url, callback) {
    var script = document.createElement("script");
    script.type = "text/javascript";
    if (script.readyState) {  //IE
        script.onreadystatechange = function () {
            if (script.readyState === "loaded" || script.readyState === "complete") {
                script.onreadystatechange = null;
                callback();
            }
        };
    } else {  // Others
        script.onload = function () {
            callback();
        };
    }
    script.src = url;
    script.id = scriptId;
    document.getElementsByTagName("head")[0].appendChild(script);
};


/**
 * 取消选择 
 */
function SelectNull() {
    $('[data-year]').removeClass('active');
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

//$(function () {
//    var paramsString = location.search;    //从问号 (?) 开始的 URL（查询部分）
//    var str = paramsString.toString().split("?");
//    var datas = str[1];
//    if (datas != undefined) {
//        var decodeData = decodeURIComponent(atob(datas));    //解密
//        var num = decodeData.toString().split("=");
//        var aid = num[1];
//    }
//    $("#sp1").text(aid);
//})

function SelectType(obj) {

    var status = $(obj).attr("data-status");
    var name = $(obj).attr("data-name");
    var url = $(obj).attr("data-url");

    $(".germ-li").removeClass("active");
    $(obj).addClass("active");
    $("#TypeValue").val(name);
    $("#pageImage").attr("src", url);

}
