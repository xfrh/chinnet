var oBack = document.getElementById("back");
var toolTipData = [];
var seriesData = [];
var provinceData = [];
var seriesDataPro = [];
var _arr_values = [];
var _max = "";
$("[data-year]").on("click", function () {

    var year = $(this).attr("data-year");
    var cateogry_id = $(this).attr("cateogry_id");
    $("[data-year]").removeClass("active");
    $("input[name='sp']").removeClass("active");
    $(this).addClass("active");
    var code = $(this).attr("data-code");
    var title = "";
    if (code == "CR-ECO") {
        title = "各省市碳青霉烯类耐药大肠埃希菌碳青霉烯酶基因检出率" + "(" + year + ")";
    }
    else if (code == "CR-KPN") {
        title = "各省市碳青霉烯类耐药肺炎克雷伯菌碳青霉烯酶基因检出率" + "(" + year + ")";
    }
    $("#pageContentTitle").html(title);
    toolTipData = [];
    seriesData = [];
    provinceData = [];
    seriesDataPro = [];

    $.ajax({
        url: "/CreData/SHeatMapDataShow",
        type: "post",
        async: false,
        dataType: "json",
        data: { year: year, Germ_id: cateogry_id },
        success: function (data) {
            // data = JSON.parse(data)
            for (var i = 0; i < data.length; i++) {
                // var Region = data[i].Region
                toolTipData.push(
                    {
                        "provinceName": data[i].Province,
                        "kpc": data[i].ratekpc,
                        "ndm": data[i].ratendm,
                    }
                );

            };
        }

    });

    for (var i = 0; i < toolTipData.length; i++) {
        seriesData[i] = {};
        seriesData[i].name = toolTipData[i].provinceName;
        seriesData[i].value = toolTipData[i].kpc;
        seriesData[i].provinceKey = toolTipData[i].ndm;
    }

    $.ajax({
        url: "/CreData/CHeatMapDataShow",
        type: "post",
        async: false,
        dataType: "json",
        data: { year: year, Germ_id: cateogry_id },
        success: function (data) {
            //data = JSON.parse(data)
            for (var i = 0; i < data.length; i++) {
                // var Region = data[i].Region
                provinceData.push({
                    "cityName": data[i].City,
                    "kpc": data[i].ratekpc,
                    "ndm": data[i].ratendm,
                });
            };
        }

    });

    for (var i = 0; i < provinceData.length; i++) {
        seriesDataPro[i] = {};
        seriesDataPro[i].name = provinceData[i].cityName;
        seriesDataPro[i].value = provinceData[i].kpc;
    }
    initEcharts("china", "中国");
})
function initEcharts(pName, Chinese_) {
    var attr = [];
    var col = [];
    $.ajax({
        url: "/CreData/GetEcharts",
        type: "post",
        async: false,
        dataType: "json",
        success: function (data) {
            //data = JSON.parse(data)
            for (var i = data.length - 1; i >= 0; i--) {
                _arr_values.push(data[i][1]);
                if (i == 3) {
                    attr.push({
                        start: parseInt(data[i][0])
                    })
                } else {
                    attr.push({ start: parseInt(data[i][0]), end: parseInt(data[i][1]) })
                }
                col.push(data[i][2])
            }
            _max = Math.max.apply(Math, _arr_values);
        }

    })

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
    var option = {
        title: {
            //text: Chinese_ || pName,
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
                            toolTiphtml += tmp[i].provinceName + '<br>kpc：' + tmp[i].kpc + '%<br>ndm：' + tmp[i].ndm + '%';
                        }
                    }
                    return toolTiphtml;
                } else {
                    var toolTiphtml = ''
                    for (var i = 0; i < tmp.length; i++) {
                        if (params.name == tmp[i].cityName) {
                            toolTiphtml += tmp[i].cityName + '<br>kpc：' + tmp[i].kpc + '%<br>ndm：' + tmp[i].ndm + '%';
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
            color: col,
            textStyle: {
                fontSize: 12,
                lineHeight: 56
            },
            itemWidth: 25,
            itemHeight: 10,
            itemGap: 5,
            itemSymbol: 'roundRect',
            textGap: 5,
            formatter: function (value, value2) {
                if (value === Math.ceil(_max * 3 / 4)) {
                    return "KPC检出率： " + '>' + value + '%';
                }
                return "KPC检出率： " + value + ' - ' + value2 + '%';
            },
            top: '70%',
            left: '5%',
            bottom: '5%',
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
        series: [
            {
                name: Chinese_ || pName,
                type: 'map',
                mapType: pName,
                roam: true,//是否开启鼠标缩放和平移漫游
                data: tmpSeriesData,
                top: "5%",//组件距离容器的距离
                zoom: 1.15,
                //selectedMode: 'single',
                //showLegendSymbol: false,
                label: {
                    normal: {
                        show: true,//显示省份标签                               
                        textStyle:
                        {
                            color: "#545454",
                            fontSize: 8
                        }//省份标签字体颜色
                    },
                    emphasis: {//对应的鼠标悬浮效果
                        show: true,
                        textStyle:
                        {
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
                    }
                },
            },
        ],
        textStyle: {
            fontSize: 10
        }

    };
    var myChart = echarts.init(document.getElementById('main'));
    myChart.setOption(option);

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
                $('#back').show();
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
    //} else { // 省份，添加双击 回退到全国
    //    myChart.on("dblclick", function () {
    //        initEcharts("china", "中国");
    //    });
    }
}