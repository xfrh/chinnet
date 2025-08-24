var app = new Vue({
    el: '#app',
    data: {
        searchText: null,
        clearSearchText: true,
        pageContentTitle: null,
        pageContentSubTitle: null,
        weui_toast_content: null,
        lookups: []
    },
    created: function () {
        var _that = this;
        _that.$nextTick(() => { _that.initSwiper()});
    },
    methods: {

        /**
         * 页面回到顶部
         * @return {boolean} 默认false
         */
GoTop() {
    mySwiper.slideTo(0, 100, false);
    return false;
},

/**
         * 跳转页面
         * @param {string} url 页面地址
         */
toPage(url) {
    if (url) {
        location.href = url;
    }
},
/**
 * 初始化Swiper
 */
initSwiper() {
    var _this = this;
    _this.$refs.swiper_container.style.height = _this.$refs.middle.offsetHeight + 'px';
    setTimeout(() => {
        mySwiper = new Swiper('.swiper-container', {
            direction: 'vertical',
            loop: false,
            observer: true,
            observeParents: false,
            watchSlidesProgress: true,
            freeMode: true,
            slidesPerView: 'auto'
        });
    });
}
    }
});
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
$(function () {
    $("#back").hide();
    $.ajax({
        url: "/CreData/SHeatMapDataShow",
        type: "post",
        async: false,
        dataType: "json",
        success: function (data) {
            //data = JSON.parse(data)
            for (var i = 0; i < data.length; i++) {
                // var Region = data[i].Region
                toolTipData.push(
                    {
                        "provinceName": data[i].Province,
                        "kpc": data[i].ratekpc,
                        "ndm": data[i].ratendm,
                    }
                )

            }
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
        success: function (data) {

            for (var i = 0; i < data.length; i++) {

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
    initEcharts("china", "中国");
})
oBack.onclick = function () {
    $('#back').hide();
        initEcharts("china", "中国");
    };


// 初始化echarts
function initEcharts(pName, Chinese_) {
    var tmpSeriesData = pName === "china" ? seriesData : seriesDataPro;
    var tmp = pName === "china" ? toolTipData : provinceData;
    //    var tmpSeriesDatandm = null;
    //tmpSeriesDatandm = pName === "china" ? seriesData : seriesDataPro;  

    tmpSeriesData.push({
        name: '南海诸岛',
        value: 0,
        itemStyle: {
            normal: {
                opacity: 0,
                label: {
                    show: false,
                    textStyle: false

                }
            }
        }
    });

    var option = {
        title: {
            // text: Chinese_ || pName,
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
        //color: ['#4169E1'],
        //legend: {
        //    orient: 'vertical',
        //    y: 'bottom',
        //    x: 'right',
        //    data: ['KPC', 'NDM'],
        //    //borderColor: '#FFFFFF',
        //    textStyle: {
        //        color: '#fff'
        //    }
        //},
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
                fontSize: 8,
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
            // dimension: 2,   //指定用数据的『哪个维度』，映射到视觉元素上。『数据』即 series.data。 可以把 series.data 理解成一个二维数组,其中每个列是一个维度,默认取 data 中最后一个维度
            // seriesIndex: 1, //指定取哪个系列的数据，即哪个系列的 series.data,默认取所有系列
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
                        //formatter: function (params) {
                        //    return params.name + "\n KPC: \n " + params.value + "% NDM: \n" + params.provinceKey + "%";    //地图上展示文字 + 数值
                        //},
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
                        //areaStyle: {
                        //    color: '#f3f3f3',//默认的地图板块颜色
                        //},
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

// 展示对应的省
function showProvince(pName, Chinese_) {
    //这写省份的js都是通过在线构建工具生成的，保存在本地，需要时加载使用即可，最好不要一开始全部直接引入。
    loadBdScript('$' + pName + 'JS', '/Areas/CRE/Content/js/map/province/' + pName + '.js', function () {
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