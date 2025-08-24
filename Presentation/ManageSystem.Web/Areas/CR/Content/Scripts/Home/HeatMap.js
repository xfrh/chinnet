
$(document).ready(function () {
  
})

//选择数据段名称
function SelectType(obj) {

    var status = $(obj).attr("data-status");
    var name = $(obj).attr("data-name");
    var url = $(obj).attr("data-url");

    $(".germ-li").removeClass("active");
    $(obj).addClass("active");
    $("#TypeValue").val(name);
    $("#pageImage").attr("src", url);

}

//取消选择数据段名称
function ClearSelectType() {

    $(".germ-li").removeClass("active");
    $(".germ-li").removeClass("year-select");

    var itemList = $(".germ-li");
    $("#TypeValue").val("");

    for (var i = 0; i < itemList.length; i++) {
        $(itemList[i]).attr("data-status", "0");
    }

    //加载报表数据
    LoadData();
}

//加载报表数据
function LoadData() {

    var typeValue = $("#TypeValue").val();

    if (typeValue == "" && typeValue == "") {

        $("#dataError").css("display", "block");
        $("#main").css("display", "none");
        $("#PageContentTitle").html("");
        return;
    }

    $("#dataError").css("display", "none");
    $("#main").css("display", "block");

    var pageContentTitle = "";
    if (typeValue != "")
        pageContentTitle = typeValue;

    $("#PageContentTitle").html(pageContentTitle);

    SelectItem(typeValue);
}

//获取数据
function SelectItem(type) {

    var legend = [];
    var text = ["2005年", "2006年", "2007年", "2008年", "2009年", "2010年", "2011年", "2012年", "2013年", "2014年", "2015年", "2016年", "2017年"];
    var value = [];

    if (type == "肺炎克雷伯菌对碳青霉烯类耐药变迁") {
        legend = ["亚胺培南", "美罗培南"];
        value = [{ name: '亚胺培南', type: 'bar', stack: '', data: [3, 3.4, 2.4, 4, 4.9, 9.2, 9, 10, 10.3, 11, 15.6, 16.1, 20.9], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: '美罗培南', type: 'bar', stack: '', data: [2.9, 2.6, 2.9, 3.8, 4.8, 9.2, 9, 11.8, 14.1, 14.1, 14.4, 18.8, 24], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } }];
    } else if (type == "铜绿假单细胞菌对碳青霉烯类耐药变迁") {
        legend = ["亚胺培南", "美罗培南"];
        value = [{ name: '亚胺培南', type: 'bar', stack: '', data: [32.5, 35.1, 35.8, 30.5, 30.5, 30.8, 29.1, 29.1, 27.1, 29.1, 27.6, 28.7, 23.6], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: '美罗培南', type: 'bar', stack: '', data: [31.6, 26.4, 28.5, 24.5, 25.2, 25.8, 25, 27.1, 25.1, 26.1, 23.4, 25.3, 20.9], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } }];
    } else if (type == "鲍曼不动杆菌对碳青霉烯类耐药变迁") {
        legend = ["亚胺培南", "美罗培南"];
        value = [{ name: '亚胺培南', type: 'bar', stack: '', data: [31, 30.1, 35.3, 48.1, 50, 57.1, 60.4, 56.8, 62.8, 62.4, 65.7, 69.7, 66.7], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: '美罗培南', type: 'bar', stack: '', data: [39, 40.9, 39.9, 49.3, 52.4, 58.3, 61.4, 61.4, 59.4, 66.7, 72.9, 72.9, 69.3], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } }];
    } else if (type == "CHINET监测历年MRSA和MRCNS检出变迁") {
        legend = ["MRSA", "MRCNS"];
        value = [{ name: 'MRSA', type: 'bar', stack: '', data: [69, 58.4, 58, 55.9, 52.7, 51.7, 50.6, 47.9, 45.2, 44.6, 42.2, 38.4, 35.3], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: 'MRCNS', type: 'bar', stack: '', data: [82, 76.3, 77, 75.9, 71.7, 71.6, 74.6, 77.1, 73.5, 83, 82.6, 77.6, 80.3], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } }];
    } else if (type == "粪肠球菌和屎肠球菌对万古霉素耐药变迁") {
        legend = ["粪肠球菌", "屎肠球菌"];
        value = [{ name: '粪肠球菌', type: 'bar', stack: '', data: [0, 0, 0.49, 0.39, 0.28, 0.55, 0.1, 0.26, 0.03, 0.19, 0.2, 0.4, 0.1], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: '屎肠球菌', type: 'bar', stack: '', data: [0.35, 1.06, 2.06, 3.21, 3.49, 3.52, 2.6, 2.49, 2.12, 3.5, 2.4, 1.9, 1.4], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } }];
    } else if (type == "CHINET监测主要标本所占比例变迁") {
        legend = ["呼吸道分泌物", "尿液", "血液", "伤口脓液"];
        value = [{ name: '呼吸道分泌物', type: 'bar', stack: '', data: [45, 50.1, 50, 49, 49.7, 46.9, 45.8, 44.4, 43.2, 41.6, 42.8, 41.6, 40], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: '尿液', type: 'bar', stack: '', data: [18, 17.9, 19.6, 19.8, 19.9, 19.9, 22.6, 21.4, 20.9, 22.4, 22.1, 19.1, 19.2], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: '血液', type: 'bar', stack: '', data: [9, 9.1, 11, 11.1, 10.8, 11.9, 11.9, 11.7, 13.1, 13.3, 12, 13.3, 15.2], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: '伤口脓液', type: 'bar', stack: '', data: [6, 5.4, 4.9, 6.5, 5.3, 5.2, 10.6, 5.6, 9.8, 4.8, 5.1, 7.3, 11.7], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } }];
    } else if (type == "主要细菌在革兰阴性杆菌中所占比例变迁") {
        legend = ["大肠埃希菌", "铜绿假单胞菌", "肺炎克雷伯菌", "鲍曼不动杆菌"];
        value = [{ name: '大肠埃希菌', type: 'bar', stack: '', data: [25.9, 26.6, 27.6, 26.5, 25.8, 26.9, 28.4, 27.3, 27.2, 28.8, 27.8, 27.2, 27.2], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: '铜绿假单胞菌', type: 'bar', stack: '', data: [17.4, 17.2, 16.9, 16.4, 15.8, 14.8, 14.1, 14, 13.4, 13, 12.4, 12.1, 12.3], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: '肺炎克雷伯菌', type: 'bar', stack: '', data: [14, 13.6, 12.8, 13.6, 14.7, 14.7, 15, 16.5, 17.9, 18.1, 18.2, 17.9, 19.5], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: '鲍曼不动杆菌', type: 'bar', stack: '', data: [13.2, 11.1, 11.5, 12.4, 13.4, 14.4, 13.9, 15, 14.6, 14.2, 14.2, 13.6, 13], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } }
        ];
    } else if (type == "肠杆菌科细菌对头孢噻肟(或头孢曲松)耐药菌株的检出率变迁") {
        legend = ["大肠埃希菌", "肺炎克雷伯菌", "奇异变形杆菌"];
        value = [{ name: '大肠埃希菌', type: 'bar', stack: '', data: [51.9, 54.5, 59.5, 61.2, 59.8, 57.9, 57.2, 61.7, 61.0, 61.1, 59.6, 57.4, 58.1], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: '肺炎克雷伯菌', type: 'bar', stack: '', data: [49.3, 52.2, 50.9, 51.4, 47.5, 45.2, 46.4, 44.8, 43.6, 40.6, 41.9, 42.7, 44.9], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: '奇异变形杆菌', type: 'bar', stack: '', data: [16.0, 25.1, 22.2, 23.8, 25.5, 23.0, 27.8, 30.3, 32.4, 34.7, 33.5, 35.1, 33.9], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } }
        ];
    } else if (type == "CHINET监测历年分离革兰阴性菌和革兰阳性菌所占比例") {
        legend = ["革兰阴性菌", "革兰阳性菌"];
        value = [{ name: '革兰阴性菌', type: 'bar', stack: '', data: [66.9, 68.2, 65.7, 69.5, 71, 71.6, 71.5, 71.9, 73, 72.6, 70.2, 71.6, 70.8], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: '革兰阳性菌', type: 'bar', stack: '', data: [33.1, 31.8, 34.3, 30.5, 29, 28.4, 28.5, 28.1, 27, 27.4, 29.8, 28.4, 29.2], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } }
        ];
    } else if (type == "CHINET监测历年住院和门急诊患者分离菌株数所占比例") {
        legend = ["住院患者分离菌株", "门诊患者分离菌株"];
        value = [{ name: '住院患者分离菌株', type: 'bar', stack: '', data: [89.6, 82.6, 87, 86.8, 87.5, 87.8, 84.6, 87.3, 86.2, 84.9, 83.5, 86.6, 87.3], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: '门诊患者分离菌株', type: 'bar', stack: '', data: [10.4, 17.4, 13, 13.2, 12.5, 12.2, 15.4, 12.7, 13.8, 15.1, 16.5, 13.4, 12.7], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } }
        ];
    } else if (type == "儿童患者非脑膜炎肺炎链球菌的分布") {
        text = ["2015年", "2016年", "2017年"];
        legend = ["PSSP", "PISP", "PRSP"];
        value = [{ name: 'PSSP', type: 'bar', barWidth: 30, stack: '', data: [86.5, 89.6, 86.8], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: 'PISP', type: 'bar', barWidth: 30, stack: '', data: [6.3, 7.4, 11], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: 'PRSP', type: 'bar', barWidth: 30, stack: '', data: [7.2, 3.1, 2.2], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } }
        ];
    } else if (type == "成人患者非脑膜炎肺炎链球菌的分布") {
        text = ["2015年", "2016年", "2017年"];
        legend = ["PSSP", "PISP", "PRSP"];
        value = [{ name: 'PSSP', type: 'bar', barWidth: 30, stack: '', data: [91.8, 95.4, 94.7], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: 'PISP', type: 'bar', barWidth: 30, stack: '', data: [5.6, 3.4, 3.4], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
        { name: 'PRSP', type: 'bar', barWidth: 30, stack: '', data: [2.6, 1.2, 1.9], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } }
        ];
    } else if (type == "肠杆菌科细菌对亚胺培南耐药变迁") {
        legend = ["肺炎克雷伯菌", "大肠埃希菌", "阴沟肠杆菌","弗劳地柠檬酸杆菌"];
        value = [{ name: '肺炎克雷伯菌', type: 'bar', stack: '', data: [3, 3.4, 2.4, 4, 4.9, 9.2, 9, 10, 10.3, 11, 15.6, 16.1, 20.9], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
            { name: '大肠埃希菌', type: 'bar', stack: '', data: [1.1, 1.4, 0.7, 1.2, 1.7, 1.6, 0.9, 0.9, 1, 0.9, 1.4, 1.3, 1.9], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
            { name: '阴沟肠杆菌', type: 'bar', stack: '', data: [8.1, 8.5, 5.2, 5.1, 4.9, 5.4, 2.9, 2.6, 3.6, 3.8, 5.6, 3.7, 7.7], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } },
            { name: '弗劳地柠檬酸杆菌', type: 'bar', stack: '', data: [11.3, 13.4, 8.7, 10.7, 7.3, 10.8, 8, 4.9, 7.1, 10.7, 12.5, 11.6, 11.7], itemStyle: { normal: { label: { show: true, position: 'top', formatter: '{c}' } } } }
        ];
    }

    $("#PageContentTitle").html(type);

    LoadEcharts(legend, text, value, type);
}

//加载柱状图
function LoadEcharts(legend, text, value, pageName) {

    $("#dataError").css("display", "none");
    $("#main").css("display", "block");

    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById('main'));

    option = {
        color: ['#C1232B', '#015BAA', '#FE8463', '#000F1A', '#FAD860', '#F3A43B', '#60C0DD', '#D7504B', '#C6E579', '#F4E001', '#F0805A', '#26C0C0'],
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            }
        },
        title: {
            text: '' + pageName + '',
            subtext: '',
            left: 'center',
            show: false
        },
        legend: {
            data: legend
        }, grid: {
            left: '0',
            right: '3%',
            bottom: '8%',
            containLabel: true
        },
        toolbox: {
            show: true,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            feature: {
                dataView: { title: "数据", readOnly: false },
                restore: {},
                saveAsImage: { title: "下载", type: "jpeg" }
            }
        },
        calculable: true,
        xAxis: [
            {
                type: 'category',
                axisTick: { show: false },
                data: text
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    show: true,
                    interval: 'auto',
                    formatter: '{value} %'
                },
                show: true
            }
        ],
        series: value
    };

    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
}

//加载折点图
//function LoadEcharts(legend, text, value) {

//    $("#dataError").css("display", "none");
//    $("#main").css("display", "block");

//    // 基于准备好的dom，初始化echarts实例
//    var myChart = echarts.init(document.getElementById('main'));

//    option = {
//        title: {
//            text: ''
//        },
//        tooltip: {
//            trigger: 'axis'
//        },
//        legend: {
//            data: legend
//        },
//        grid: {
//            left: '3%',
//            right: '4%',
//            bottom: '3%',
//            containLabel: true
//        },
//        toolbox: {
//            feature: {
//                saveAsImage: {}
//            }
//        },
//        xAxis: {
//            type: 'category',
//            boundaryGap: false,
//            data: text
//        },
//        yAxis: {
//            type: 'value',
//            axisLabel: {
//                show: true,
//                interval: 'auto',
//                formatter: '{value}'
//            },
//            show: true
//        },
//        series: value
//    };

//    // 使用刚指定的配置项和数据显示图表。
//    myChart.setOption(option);
//}



//选择细菌
function ShowGermContent(type) {
    var status = $(".GermContent" + type).attr("data-status");
    if (status == "1") {
        //隐藏
        $(".GermContent" + type).hide("slow");
        $(".GermContent" + type).attr("data-status", "0");
        $(".germ-btn-image" + type).attr("src", "/Content/Images/jiantou-shang.png");
    } else {
        //显示
        $(".GermContent" + type).show("slow");
        $(".GermContent" + type).attr("data-status", "1");
        $(".germ-btn-image" + type).attr("src", "/Content/Images/jiantou-xia.png");
    }
}