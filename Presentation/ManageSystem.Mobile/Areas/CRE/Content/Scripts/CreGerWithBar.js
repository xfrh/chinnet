var mySwiper;

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
        _that.$nextTick(() => { _that.initSwiper(); });
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
var grem_id = [];
$(function () {
    $("#pageContentTitle").html("痰标本分离CRE菌株分布");
    $("input[name='Germ']").each(function (j, item) {
        grem_id.push(item.value);
    });
    var Escherichiacoli = [];
    $.ajax({
        url: "/CreData/Escherichiacoli",
        type: "post",
        async: false,
        dataType: "json",
        data: { field: null, Germ_id: grem_id[0] },
        success: function (data) {
            if (data == "") {
                Escherichiacoli = ["0", "0", "0", "0", "0", "0", "0", "0"];
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    Escherichiacoli.push(data[i]);
                }
            }
        }

    });
    var klebsiella = [];
    $.ajax({
        url: "/CreData/klebsiella",
        type: "post",
        async: false,
        dataType: "json",
        data: { field: null, Germ_id: grem_id[1] },
        success: function (data) {
            if (data == "") {
                klebsiella = ["0", "0", "0", "0", "0", "0", "0", "0"];
            } else {
                for (var i = 0; i < data.length; i++) {
                    klebsiella.push(data[i]);
                }
            }
        }

    });
    var Entero = [];
    $.ajax({
        url: "/CreData/Entero",
        type: "post",
        async: false,
        dataType: "json",
        data: { field: null, Germ_id: grem_id[2] },
        success: function (data) {
            if (data == "") {
                Entero = ["0", "0", "0", "0", "0", "0", "0", "0"];
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    Entero.push(data[i]);
                }
            }
        }

    });
    var serrati = [];
    $.ajax({
        url: "/CreData/serratia",
        type: "post",
        async: false,
        dataType: "json",
        data: { field: null, Germ_id: grem_id[3] },
        success: function (data) {
            if (data == "") {
                serrati = ["0", "0", "0", "0", "0", "0", "0", "0"];
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    serrati.push(data[i]);
                }
            }
        }

    });
    var option = {
        tooltip: {
            trigger: 'axis',
            axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
            },
            formatter: function (params) { //在此处直接用 formatter 属性
                //console.log(params)  // 打印数据
                var htmlStr = '';
                for (var i = params.length - 1; i >= 0; i--) {
                    var param = params[i];
                    var xName = param.name;//x轴的名称
                    var seriesName = param.seriesName;//图例名称
                    var value = param.value;//y轴值
                    var color = param.color;//图例颜色

                    if (i === 3) {
                        htmlStr += xName + '<br/>';//x轴的名称
                    }
                    htmlStr += '<div>';
                    //为了保证和原来的效果一样，这里自己实现了一个点的效果
                    htmlStr += '<span style="margin-right:5px;display:inline-block;width:10px;height:10px;border-radius:5px;background-color:' + color + ';"></span>';

                    //圆点后面显示的文本
                    htmlStr += seriesName + '：' + value;

                    htmlStr += '</div>';
                }
                return htmlStr;


            }
        },
        //toolbox: {
        //    show: true,
        //    orient: 'vertical',
        //    left: 'right',
        //    top: 'center',
        //     feature: {
        //            dataView: {readOnly: false},
        //            restore: {},
        //            saveAsImage: {}
        //     }
        //},
        //legend: {
        //    data: ['大肠埃希菌', '肺炎克雷伯菌', '阴沟肠杆菌', '黏质沙雷菌']
        //},
        grid: {
            top: '5%',
            left: '2%',
            right: '4%',
            bottom: '20%',
            containLabel: true
        },

        xAxis: {
            type: 'category',
            axisLabel: {
                interval: 0,
                margin: 10,
                textStyle: {
                    fontSize: "14px"
                }
            },
            data: ['0-5%', '6%-10%', '11%-15%', '16%-20%', '21%-30%', '31%-40%', '41%-50%', '51%以上'],
            axisTick: {
                alignWithLabel: true
            },
            axisLabel: {
                interval: 0,
                rotate: 40
            }  
        },
        yAxis: {
            type: 'value',
            axisLabel: {
                show: true,
                interval: 'auto',
                formatter: '{value} %'
            },
            show: true
        },

        series: [
            {
                name: '大肠埃希菌CR-ECO',
                type: 'bar',
                stack: '总量',
                barWidth: 25,
                label: {
                    show: true,
                    position: 'insideRight'
                },
                itemStyle: {
                    normal: { color: "#c23531" },
                },
                data: Escherichiacoli
            },
            {
                name: '肺炎克雷伯菌CR-KPN',
                type: 'bar',
                stack: '总量',
                barWidth: 25,
                label: {
                    show: true,
                    position: 'insideRight'
                },
                itemStyle: {
                    normal: { color: "#91c7ae" },
                },
                data: klebsiella
            },
            {
                name: '阴沟肠杆菌CR-ECL',
                type: 'bar',
                stack: '总量',
                barWidth: 25,
                itemStyle: {
                    normal: { color: "#61a0a8" },
                },
                label: {
                    show: true,
                    position: 'insideRight'
                },
                data: Entero
            },
            {
                name: '粘质沙雷菌CR-SMA',
                type: 'bar',
                stack: '总量',
                barWidth: 25,
                label: {
                    show: true,
                    position: 'insideRight'
                },
                itemStyle: {
                    normal: { color: "#d48265" },
                },
                data: serrati
            }                   
        ],
        textStyle:
        {
            fontSize: 10
        },
        //dataZoom: [
        //    {
        //        type: 'slider',
        //        show: true,
        //        xAxisIndex: [0],
        //        start: 0,
        //        bottom: "0",
        //        left: "6%",
        //        right: "4%",
        //        textStyle: false,
        //        handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
        //        handleSize: '80%',
        //        handleStyle: {
        //            color: '#fff',
        //            shadowBlur: 3,
        //            shadowColor: 'rgba(0, 0, 0, 0.6)',
        //            shadowOffsetX: 2,
        //            shadowOffsetY: 2
        //        }
        //    }
        //],
    }
    var myChart = echarts.init(document.getElementById('main'));

    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
})
$("[data-code]").on("click", function () {
    var Identi = $(this).attr("data-year")
    var field = "";
    if (Identi == "s") {
        field = "s" + $(this).attr("data-code");
    }
    else if (Identi == "d") {
        field = "d" + $(this).attr("data-code");
    }
    var title = $(this).attr("data-code") + "标本分离CRE菌株分布";
    var titles = $(this).attr("data-code") + "CRE菌株分布";
    var cateogry_id = $(this).attr("cateogry_id");
    $("[data-year]").removeClass("active");
    $(this).addClass("active");
    if (cateogry_id == "1") {
        $("#pageContentTitle").html(title);
    } else {
        $("#pageContentTitle").html(titles);
    }
    var Escherichiacoli = [];
    $.ajax({
        url: "/CreData/Escherichiacoli",
        type: "post",
        async: false,
        dataType: "json",
        data: { field: field, Germ_id: grem_id[0] },
        success: function (data) {
            if (data == "") {
                Escherichiacoli = ["0", "0", "0", "0", "0", "0", "0", "0"];
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    Escherichiacoli.push(data[i]);

                }
            }
        }

    });
    var klebsiella = [];
    $.ajax({
        url: "/CreData/klebsiella",
        type: "post",
        async: false,
        dataType: "json",
        data: { field: field, Germ_id: grem_id[1] },
        success: function (data) {
            if (data == "") {
                klebsiella = ["0", "0", "0", "0", "0", "0", "0", "0"];
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    klebsiella.push(data[i]);

                }
            }
        }

    });
    var Entero = [];
    $.ajax({
        url: "/CreData/Entero",
        type: "post",
        async: false,
        dataType: "json",
        data: { field: field, Germ_id: grem_id[2] },
        success: function (data) {
            if (data == "") {
                Entero = ["0", "0", "0", "0", "0", "0", "0", "0"];
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    Entero.push(data[i]);

                }
            }
        }

    });
    var serrati = [];
    $.ajax({
        url: "/CreData/serratia",
        type: "post",
        async: false,
        dataType: "json",
        data: { field: field, Germ_id: grem_id[3] },
        success: function (data) {
            if (data == "") {
                serrati = ["0", "0", "0", "0", "0", "0", "0", "0"];
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    serrati.push(data[i]);

                }
            }
        }

    });
    var option = {
        tooltip: {
            trigger: 'axis',
            axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
            },
            formatter: function (params) { //在此处直接用 formatter 属性
                //console.log(params)  // 打印数据
                var htmlStr = '';
                for (var i = params.length - 1; i >= 0; i--) {
                    var param = params[i];
                    var xName = param.name;//x轴的名称
                    var seriesName = param.seriesName;//图例名称
                    var value = param.value;//y轴值
                    var color = param.color;//图例颜色

                    if (i === 3) {
                        htmlStr += xName + '<br/>';//x轴的名称
                    }
                    htmlStr += '<div>';
                    //为了保证和原来的效果一样，这里自己实现了一个点的效果
                    htmlStr += '<span style="margin-right:5px;display:inline-block;width:10px;height:10px;border-radius:5px;background-color:' + color + ';"></span>';

                    //圆点后面显示的文本
                    htmlStr += seriesName + '：' + value;

                    htmlStr += '</div>';
                }
                return htmlStr;


            }
        },
        //legend: {
        //    data: ['大肠埃希菌', '肺炎克雷伯菌', '阴沟肠杆菌', '黏质沙雷菌']
        //},
        grid: {
            top: '5%',
            left: '2%',
            right: '4%',
            bottom: '20%',
            containLabel: true
        },

        xAxis: {
            type: 'category',
            axisLabel: {
                interval: 0,
                margin: 10,
                textStyle: {
                    fontSize: "14px"
                }
            },
            data: ['0-5%', '6%-10%', '11%-15%', '16%-20%', '21%-30%', '31%-40%', '41%-50%', '51%以上'],
            axisTick: {
                alignWithLabel: true
            },
            axisLabel: {
                interval: 0,
                rotate: 40
            }  
        },
        yAxis: {
            type: 'value',
            axisLabel: {
                show: true,
                interval: 'auto',
                formatter: '{value} %'
            },
            show: true
        },

        series: [
            {
                name: '大肠埃希菌CR-ECO',
                type: 'bar',
                stack: '总量',
                barWidth: 25,
                label: {
                    show: true,
                    position: 'insideRight'
                },
                itemStyle: {
                    normal: { color: "#c23531" },
                },
                data: Escherichiacoli
            },
            {
                name: '肺炎克雷伯菌CR-KPN',
                type: 'bar',
                stack: '总量',
                barWidth: 25,
                label: {
                    show: true,
                    position: 'insideRight'
                },
                itemStyle: {
                    normal: { color: "#91c7ae" },
                },
                data: klebsiella
            },
            {
                name: '阴沟肠杆菌CR-ECL',
                type: 'bar',
                stack: '总量',
                barWidth: 25,
                itemStyle: {
                    normal: { color: "#61a0a8" },
                },
                label: {
                    show: true,
                    position: 'insideRight'
                },
                data: Entero
            },
            {
                name: '粘质沙雷菌CR-SMA',
                type: 'bar',
                stack: '总量',
                barWidth: 25,
                label: {
                    show: true,
                    position: 'insideRight'
                },
                itemStyle: {
                    normal: { color: "#d48265" },
                },
                data: serrati
            }                   
        ],
        //dataZoom: [
        //    {
        //        type: 'slider',
        //        show: true,
        //        xAxisIndex: [0],
        //        start: 0,
        //        bottom: "0",
        //        left: "6%",
        //        right: "4%",
        //        textStyle: false,
        //        handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
        //        handleSize: '80%',
        //        handleStyle: {
        //            color: '#fff',
        //            shadowBlur: 3,
        //            shadowColor: 'rgba(0, 0, 0, 0.6)',
        //            shadowOffsetX: 2,
        //            shadowOffsetY: 2
        //        }
        //    }
        //]
    }
    var myChart = echarts.init(document.getElementById('main'));

    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
    window.scrollTo(0, 0);
})
$("[data]").on("click", function () {
    var field = $(this).attr("data");
    $("[data-year]").removeClass("active");
    $(this).addClass("active");
    $("#pageContentTitle").html("CRE菌株对" + field + "的耐药率（%）");
    var Strainrate = [];
    $.ajax({
        url: "/CreData/Strainrate",
        type: "post",
        async: false,
        dataType: "json",
        data: { field: field },
        success: function (data) {
            if (data == "") {
                Strainrate = ["0", "0", "0", "0"];
            } else {
                for (var i = 0; i < data.length; i++) {
                    Strainrate.push(data[i]);
                }
            }
        }

    });
    var option = {
        tooltip: {
            // trigger: 'axis',
            //axisPointer: {            // 坐标轴指示器，坐标轴触发有效
            //    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
            //}
        },
        //toolbox: {
        //    show: true,
        //    orient: 'vertical',
        //    left: 'right',
        //    top: 'center',
        //    feature: {
        //        dataView:
        //        {
        //            readOnly: false,
        //            optionToContent: function (opt) {
        //                var axisData = opt.xAxis[0].data;
        //                var series = opt.series;
        //                var table = '<table class="table table-bordered" style="text-align:center"><tbody><tr>'
        //                    + '<th style="text-align:center">细菌名称</th>'
        //                    + '<th style="text-align:center">' + series[0].name + '</th>'
        //                    // + '<td>' + series[1].name + '</td>'
        //                    + '</tr>';
        //                for (var i = 0, l = axisData.length; i < l; i++) {
        //                    table += '<tr>'
        //                        + '<td>' + axisData[i] + '</td>'
        //                        + '<td>' + series[0].data[i] + '</td>'
        //                        //  + '<td>' + series[1].data[i] + '</td>'
        //                        + '</tr>';
        //                }
        //                table += '</tbody></table>';
        //                return table;
        //            }
        //        },
        //        restore: {},
        //        saveAsImage: {}
        //    }
        //},
        grid: {
            top: '5%',
            left: '2%',
            right: '4%',
            bottom: '20%',
            containLabel: true
        },
        xAxis: {
            type: 'category',
            axisLabel: {
                interval: 0,
                margin: 10,
                textStyle: {
                    fontSize: "14px"
                }
            },
            data: ['大肠埃希菌', ' 肺炎克雷伯菌', ' 阴沟肠杆菌', '粘质沙雷菌'],
            axisTick: {
                alignWithLabel: true
            },
            axisLabel: {
                interval: 0,
                rotate: 40
            }  
        },
        yAxis: {
            type: 'value',
            axisLabel: {
                show: true,
                interval: 'auto',
                formatter: '{value} %',
            },
            show: true
        },
        series: [
            {
                name: "数据",
                barWidth: 40,
                data: Strainrate,
                itemStyle: {
                    normal: {
                        label: {
                            show: true, //开启显示
                            position: 'top', //在上方显示
                            textStyle: { //数值样式
                                color: 'black',
                                fontSize: 12
                            }
                        }
                    }
                },
                type: 'bar'
            }
        ],
        //dataZoom: [
        //    {
        //        type: 'slider',
        //        show: true,
        //        xAxisIndex: [0],
        //        start: 0,
        //        bottom: "0",
        //        left: "6%",
        //        right: "4%",
        //        textStyle: false,
        //        handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
        //        handleSize: '80%',
        //        handleStyle: {
        //            color: '#fff',
        //            shadowBlur: 3,
        //            shadowColor: 'rgba(0, 0, 0, 0.6)',
        //            shadowOffsetX: 2,
        //            shadowOffsetY: 2
        //        }
        //    }
        //]
    };
    var myChart = echarts.init(document.getElementById('main'));

    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
    window.scrollTo(0, 0);
});
$("[data-name]").on("click", function () {
    var field = $(this).attr("data-name");
    var Germ_id = $(this).attr("cateogry_id");
    $("[data-year]").removeClass("active");
    $(this).addClass("active");
    $("#pageContentTitle").html(field + "对常用抗生素耐药率（%）");
    var Drugrate = [];
    $.ajax({
        url: "/CreData/Drugrate",
        type: "post",
        async: false,
        dataType: "json",
        data: { field: field, Germ_id: Germ_id },
        success: function (data) {
            if (data == "") {
                Drugrate = ["0", "0", "0", "0", "0", "0"];
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    Drugrate.push(data[i]);
                }
            }
        }

    });
    var option = {
        tooltip: {
            //trigger: 'axis',
            //axisPointer: {            // 坐标轴指示器，坐标轴触发有效
            //    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
            //},                  
        },
        //toolbox: {
        //    show: true,
        //    orient: 'vertical',
        //    left: 'right',
        //    top: 'center',
        //    feature: {
        //        dataView:
        //        {
        //            title: "数据",
        //            readOnly: false,
        //            optionToContent: function (opt) {
        //                var axisData = opt.xAxis[0].data;
        //                var series = opt.series;
        //                var table = '<table class="table table-bordered" style="text-align:center"><tbody><tr>'
        //                    + '<th style="text-align:center">细菌名称</th>'
        //                    + '<th style="text-align:center">' + series[0].name + '</th>'
        //                    + '</tr>';
        //                for (var i = 0, l = axisData.length; i < l; i++) {
        //                    table += '<tr>'
        //                        + '<td>' + axisData[i] + '</td>'
        //                        + '<td>' + series[0].data[i] + '</td>'
        //                        + '</tr>';
        //                }
        //                table += '</tbody></table>';
        //                return table;
        //            }
        //        },
        //        restore: {},
        //        saveAsImage:
        //        {
        //            title: "下载",
        //            type: "jpeg"
        //        }
        //    }
        //},
        grid: {
            top: '5%',
            left: '2%',
            right: '4%',
            bottom: '20%',
            containLabel: true
        },
        xAxis: {
            type: 'category',
            axisLabel: {
                interval: 0,
                margin: 10,
                textStyle: {
                    fontSize: "14px"
                }
            },
            data: ['多粘菌素', '替加环素', '阿维巴坦  ', ' 磷霉素', '氯霉素', '联合药敏试验'],
            axisTick: {
                alignWithLabel: true
            },
            axisLabel: {
                interval: 0,
                rotate: 40
            }  
        },
        yAxis: {
            type: 'value',
            axisLabel: {
                show: true,
                interval: 'auto',
                formatter: '{value} %',
            },
            show: true
        },
        series: [{
            name: "数据",
            barWidth: 35,
            data: Drugrate,
            type: 'bar',
            itemStyle: {
                normal: {
                    label: {
                        show: true, //开启显示
                        position: 'top', //在上方显示
                        textStyle: { //数值样式
                            color: 'black',
                            fontSize: 12
                        }
                    }
                }
            },
        }],
        //dataZoom: [
        //    {
        //        type: 'slider',
        //        show: true,
        //        xAxisIndex: [0],
        //        start: 0,
        //        bottom: "0",
        //        left: "6%",
        //        right: "4%",
        //        textStyle: false,
        //        handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
        //        handleSize: '80%',
        //        handleStyle: {
        //            color: '#fff',
        //            shadowBlur: 3,
        //            shadowColor: 'rgba(0, 0, 0, 0.6)',
        //            shadowOffsetX: 2,
        //            shadowOffsetY: 2
        //        }
        //    }
        //]
    };
    var myChart = echarts.init(document.getElementById('main'));

    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
    window.scrollTo(0, 0);
})
