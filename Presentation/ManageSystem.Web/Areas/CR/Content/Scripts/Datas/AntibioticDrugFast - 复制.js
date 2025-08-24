
$(document).ready(function () {
    $("#DefaultItem").click();
})

//选择细菌
function SelectGerm(obj) {

    $(".date-item-va").removeClass("active");
    $(obj).addClass("active");

    //加载报表数据
    LoadData( $(obj).attr("data-name"),"");
}

//选择抗生素
function SelectAntibiotic(obj) {

    $(".date-item-va").removeClass("active");
    $(obj).addClass("active");

    //加载报表数据
    LoadData("", $(obj).attr("data-name"));
}

//加载报表数据
function LoadData(germValue, antibioticValue) {

    var pageContentTitle = "";
    var xRotate = 0;

    if (germValue != "") {

        var germCount = GetGermCount(germValue);
        if (germCount != "")
            germCount += "株";

        //选择了细菌分类
        pageContentTitle = germCount+ germValue + "对抗菌药物的耐药率";

        if (germValue == "克雷伯菌属" || germValue == "柠檬酸杆菌属" || germValue == "大肠埃希菌")
            xRotate = 45;

    } else if (antibioticValue != "") {
        //选择了抗菌药物
        pageContentTitle = "不同细菌对" + antibioticValue + "的耐药率";

    } else {
        //什么都没有选择
        $("#dataError").css("display", "none");
        $(".main-content2").css("display", "block");
    }

    $("#PageContentTitle").html(pageContentTitle);

    $.ajax({
        type: "POST",
        url: "/Data/GetAntibioticDrugFastData",
        data: {
            GermValue: germValue,
            AntibioticValue: antibioticValue
        },
        dataType: "json",
        success: function (result) {

            if (result == null || result == "") {
                MessageWindow("获取数据失败");
                return;
            }

            if (result.Status) {

                var text = result.Text.split(",");
                var value = result.Value.split(",");

                LoadEcharts(text, value, xRotate, 30, pageContentTitle);

            } else {
                var text = [];    //类别数组（实际用来盛放X轴坐标值）
                var value = [];    //销量数组（实际用来盛放Y坐标值）
                LoadEcharts(text, value);

                MessageWindow(result.Message);
            }
        }
    });

}

//根据细菌名称获取对应的细菌数值
function GetGermCount(germValue) {
    var germCount = "";
    if (germValue == "大肠埃希菌")
        germCount = "36735";
    else if (germValue == "克雷伯菌属")
        germCount = "27977";
    else if (germValue == "变形杆菌属")
        germCount = "3646";
    else if (germValue == "肠杆菌属")
        germCount = "7491";
    else if (germValue == "沙雷菌属")
        germCount = "2330";
    else if (germValue == "柠檬酸杆菌属")
        germCount = "1689";
    else if (germValue == "摩根菌属")
        germCount = "943";
    else if (germValue == "铜绿假单胞菌")
        germCount = "16562";
    else if (germValue == "不动杆菌属")
        germCount = "19246";
    else if (germValue == "MSSA")
        germCount = "11120";
    else if (germValue == "MRSA")
        germCount = "6084";
    else if (germValue == "MSCNS")
        germCount = "1586";
    else if (germValue == "MRCNS")
        germCount = "6717";
    else if (germValue == "粪肠球菌")
        germCount = "6693";
    else if (germValue == "屎肠球菌")
        germCount = "8173";
    else if (germValue == "肠杆菌属")
        germCount = "82754";
    else if (germValue == "嗜麦芽窄食单胞菌")
        germCount = "5471";
    else if (germValue == "伯克霍尔德菌属")
        germCount = "1804";
    else if (germValue == "流感嗜血杆菌")
        germCount = "5070";
    else if (germValue == "流感嗜血杆菌(儿童)")
        germCount = "3161";
    else if (germValue == "流感嗜血杆菌(成人)")
        germCount = "1898";
    else if (germValue == "PSSP(儿童)")
        germCount = "2787";
    else if (germValue == "PISP(儿童)")
        germCount = "355";
    else if (germValue == "PRSP(儿童)")
        germCount = "70";
    else if (germValue == "PSSP(成人)")
        germCount = "1288";
    else if (germValue == "PISP(成人)")
        germCount = "46";
    else if (germValue == "PRSP(成人)")
        germCount = "26";

    return germCount;
}

//加载百度柱状图插件   text：显示的文本内容    value：对应的值     xRotate：x轴文字的显示方向      barWidth：柱子的宽度
function LoadEcharts(text, value, xRotate, barWidth,pageName) {

    $("#dataError").css("display", "none");
    $(".main-content2").css("display", "block");

    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById('main'));

    option = {
        color: ['#015BAA'],
        title: {
            text: '' + pageName+'',
            subtext: '',
            left: 'center',
            show: false
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: { // 坐标轴指示器，坐标轴触发有效
                type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '15%',
            containLabel: true
        },
        xAxis: [
            {
                type: 'category',
                axisLabel: {
                    interval: 0,
                    rotate: xRotate,
                    margin: 10,
                    textStyle: {
                        fontSize: "14px"
                    }
                },
                data: text,  //'替考拉宁', '万古霉素', '利奈唑胺', '利福平', '庆大霉素', '左氧氟沙星', '环丙沙星', '克林霉素', '复方磺胺甲恶唑', '红霉素', '青霉素', '苯唑西林'
                axisTick: {
                    alignWithLabel: true
                }
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
        toolbox: {
            show: true,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            feature: {
                dataView: {
                    title: "数据",
                    readOnly: false
                },
                restore: {},
                saveAsImage: {
                    title: "下载",
                    type: "jpeg"
                }
            }
        },
        series: [
            {
                name: '数值',
                type: 'bar',
                barWidth: barWidth,//柱图宽度
                data: value,//0, 0, 0, 12.7, 31.2, 56.7, 55.7, 42.9, 54.7, 85.7, 100, 100
                //以下为是否显示，显示位置和显示格式的设置了
                itemStyle: {
                    normal: {

                        //以下为是否显示，显示位置和显示格式的设置了
                        label: {
                            show: true,
                            position: 'top',
                            // formatter: '{c}'
                            formatter: '{c}'
                        }
                    }
                },
            }
        ],
        textStyle:
        {
            fontSize: 10
        }
    };
    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
}

//选择主要标本菌种分布
function SelectMainGerm(obj) {

    $(".date-item-va").removeClass("active");
    $(obj).addClass("active");

    var name = $(obj).attr("data-name");
    var code = $(obj).attr("data-code");
    var text = [];
    var value = [];
    var title = "";

    switch (code) {

        case "100":
            title = "190610株临床分离菌株主要菌种分布";
            text = ["大肠埃希菌", "克雷伯菌属", "不动杆菌属", "金葡菌", "铜绿假单胞菌", "肠球菌属", "凝固酶阴性葡萄球菌", "肠杆菌属", "B溶血性链球菌", "嗜麦芽窄食单胞菌", "流感嗜血杆菌", "肺炎链球菌", "变形杆菌属", "沙雷菌属", "草绿色链球菌", "伯克霍尔德菌属", "柠檬酸杆菌属", "卡他莫拉菌", "沙门菌属", "摩根菌属"];
            value = [19.27, 14.68, 10.1, 9.03, 8.69, 8.42, 4.39, 3.93, 3.57, 2.87, 2.66, 2.41, 1.91, 1.22, 1.04, 1.01, 0.89, 0.81, 0.65, 0.49];
            break;
        case "101":
            title = "76333株呼吸道标本分离菌主要菌种分布";
            text = ["肺炎克雷伯菌", "鲍曼不动杆菌", "铜绿假单胞菌", "金黄色葡萄球菌", "流感嗜血杆菌", "嗜麦芽窄食单胞菌", "肺炎链球菌", "大肠埃希菌", "化脓链球菌", "阴沟肠杆菌", "卡他莫拉菌", "黏质沙雷菌", "洋葱伯克霍尔德菌", "产气肠杆菌", "产酸克雷伯菌", "奇异变形杆菌", "琼氏不动杆菌"]
            value = [17.6, 17.3, 14.5, 9.1, 6.1, 5.8, 5.1, 5, 3.6, 3, 1.9, 1.6,  1.6, 1.2, 1, 0.8, 0.3];
            break;
        case "102":
            title = "36635株尿道标本分离菌主要菌种分布";
            text = ["大肠埃希菌", "屎肠球菌", "肺炎克雷伯菌", "粪肠球菌", "铜绿假单胞菌", "奇异变形杆菌", "无乳链球菌", "阴沟肠杆菌", "鲍曼不动杆菌", "金黄色葡萄球菌",  "弗劳地柠檬酸杆菌", "摩根摩根菌", "产酸克雷伯菌", "产气肠杆菌", "嗜麦芽窄食单胞菌", "恶臭假单胞菌", "洋葱伯克霍尔德菌"]
            value = [46.4, 10.6, 9.8, 8.6, 3.7, 3.5, 2.7, 2.2, 2.1, 1.4, 1, 0.9, 0.9, 0.8, 0.7, 0.6, 0.4];
            break;
        case "103":
            title = "29000株血液标本分离菌主要菌种分布";
            text = ["大肠埃希菌", "肺炎克雷伯菌", "表皮葡萄球菌", "金黄色葡萄球菌", "人葡萄球菌", "屎肠球菌", "鲍曼不动杆菌", "铜绿假单胞菌", "粪肠球菌",  "阴沟肠杆菌", "溶血葡萄球菌", "肺炎链球菌", "洋葱伯克霍尔德菌", "黏质沙雷菌", "嗜麦芽窄食单胞菌", "缓症链球菌", "咽峡炎链球菌", "奇异变形杆菌"]
            value = [22.2, 15.3, 10.2, 7.9, 7, 4.4, 3.9, 3.3, 2.7, 2.4, 2.4, 1.3, 1.1, 1, 0.8, 0.7, 0.7, 0.6];
            break;
        case "104":
            title = "22320株伤口脓液标本分离菌主要菌种分布";
            text = ["金黄色葡萄球菌", "大肠埃希菌", "肺炎克雷伯菌", "铜绿假单胞菌", "无乳链球菌", "鲍曼不动杆菌", "阴沟肠杆菌", "粪肠球菌", "屎肠球菌", "奇异变形杆菌","黏质沙雷菌", "化脓链球菌", "产酸克雷伯菌", "摩根摩根菌", "嗜麦芽窄食单胞菌", "产酸克雷伯菌", "弗劳地柠檬酸杆菌", "肺炎链球菌", "产气肠杆菌", "肺炎链球菌", "鸟肠球菌"]
            value = [25.3, 20.6, 9.1, 8.7, 4.6, 4.3, 3.9, 3.7, 2.8, 2.4, 1.2, 1.1, 1, 1, 0.9, 0.8, 0.8, 0.7, 0.7, 0.7, 0.6];
            break;
        case "105":
            title = "8331株其他无菌体液标本分离菌主要菌种分布";
            text = ["大肠埃希菌", "肺炎克雷伯菌", "屎肠球菌", "粪肠球菌", "铜绿假单胞菌", "鲍曼不动杆菌", "表皮葡萄球菌", "阴沟肠杆菌", "金黄色葡萄球菌", "溶血葡萄球菌","嗜麦芽窄食单胞菌", "奇异变形杆菌", "咽峡炎链球菌", "人葡萄球菌", "产气肠杆菌", "铅黄肠球菌", "产酸克雷伯菌", "弗劳地柠檬酸杆菌", "缓症链球菌", "摩根摩根菌", "鸟肠球菌", "草绿色链球菌", "星座链球菌"]
            value = [21, 10.9, 10.6, 7.9, 5.3, 5, 4.4, 3.5, 3.4, 2.1, 2, 1.2, 1.2, 1.1, 1.1, 1.1, 1, 1, 0.9, 0.9, 0.9, 0.8, 0.8];
            break;
        case "106":
            title = "2708株脑脊液标本分离菌主要菌种分布";
            text = ["表皮葡萄球菌", "鲍曼不动杆菌", "人葡萄球菌", "肺炎克雷伯菌", "溶血葡萄球菌", "屎肠球菌", "金黄色葡萄球菌", "肺炎链球菌", "大肠埃希菌", "沃氏葡萄球菌","铜绿假单胞菌", "粪肠球菌", "阴沟肠杆菌", "嗜麦芽窄食单胞菌", "洛菲不动杆菌", "科氏葡萄球菌", "黏质沙雷菌", "无乳链球菌", "弗劳地柠檬酸杆菌", "琼氏不动杆菌", "克氏葡萄球菌"]
            value = [21.3, 16.1, 8.6, 8.1, 5.5, 4.4, 3.5, 3.1, 3.1, 2.8, 2.3, 1.4, 1.3, 1.1, 1.1, 0.9, 0.8, 0.7, 0.6, 0.6, 0.5];
            break;
        default:
    }

    $("#PageContentTitle").html(title);
    LoadEcharts(text, value, 45, 30);
}

//选择不同医院耐药菌分布
function SelectHospitalGerm(obj) {

    $(".date-item-va").removeClass("active");
    $(obj).addClass("active");

    var name = $(obj).attr("data-name");
    var code = $(obj).attr("data-code");
    var text = [];
    var value = [];
    var title = "";

    switch (code) {

        case "200":
            title = "各医院金葡菌MR菌株检出率";
            text = ["sxe", "hyd", "nmg", "ZGH", "SYH", "TJJ", "XJH", "SZS", "JJH", "JLH", "PUH", "nxz", "SCS", "XYH", "bch", "KMH", "HXH", "SDH", "PDH", "SYF", "PED", "JXE", "bjh", "SCH", "GLH", "RJH", "GSY", "ayd", "LSH", "GZH", "HNH", "HSH", "TJH", "SXS"];
            value = [10.3, 16.2, 19, 19.1, 21.8, 22.1, 22.6, 23.6, 24.9, 25.1, 25.7, 26.3, 27.1, 27.3, 27.7, 28, 28.2, 29.2, 35, 36.5, 36.9, 41.3, 47, 47.1, 47.9, 48.8, 49.1, 51, 51.7, 52, 53.3, 54, 59.9, 62.1];
            break;
        case "201":
            title = "各医院凝固酶阴性葡萄球菌MR菌株检出率";
            text = ["PDH", "JXE", "XJH", "LSH", "SZS", "PUH", "hyd", "SYF", "sxe", "GLH", "SCS", "HSH", "XYH", "JLH", "SDH", "GSY", "HXH", "SXS", "nmg", "ayd", "PED", "bch", "KMH", "GZH", "SCH", "TJH", "nxz", "SYH", "TJJ", "bjh", "RJH", "HNH", "ZGH", "JJH"];
            value = [49.3, 66.7, 68.5, 69.6, 72.1, 73.6, 74, 75, 75.5, 77.7, 78.4, 78.8, 79.1, 80.1, 80.4, 80.6, 81.3, 82, 82.2, 82.5, 82.6, 83, 83.3, 83.5, 83.9, 84, 84.6, 84.8, 85.2, 85.8, 86.8, 88.7, 100, 100];
            break;
        case "202":
            title = "各医院分离铜绿假单胞菌对亚胺培南的耐药率（68-1025株）";
            text = ["ZGH", "JXE", "NXZ", "PDH", "PUH", "XJH", "JLH", "HYD", "GSY", "NMG", "AYD", "GZH", "HXH", "SZS", "LSH", "GLH", "SCH", "SCS", "BCH", "SXE", "TJJ", "XYH", "JJH", "PED", "SDH", "TJH", "SYH", "HNH", "RJH", "SXS", "BJH", "HSH", "SYF", "KMH"];
            value = [1.7, 11.8, 11.8, 13, 14.8, 15.7, 16.3, 19.8, 20.4, 20.4, 20.7, 22.1, 22.4, 22.9, 24.1, 24.3, 24.7, 24.9, 25.2, 25.2, 28, 29.8, 30.1, 31, 31.2, 31.6, 33.3, 35.8, 35.8, 40.9, 41.2, 41.2, 43.9, 45.2];
            break;
        case "203":
            title = "各医院分离鲍曼不动杆菌对亚胺培南的耐药率（19-1299株）";
            text = ["LSH", "JXE", "ZGH", "SXE", "PDH", "JLH", "BCH", "TJJ", "PED", "NMG", "BJH", "XJH", "RJH", "GZH", "JJH", "SDH", "GLH", "PUH", "AYD", "SCH", "HXH", "SCS", "SZS", "GSY", "NXZ", "HSH", "SYF", "HYD", "KMH", "SYH", "XYH", "SXS", "HNH", "TJH"];
            value = [3.8, 21.1, 26.5, 34.1, 37.9, 42.7, 43, 46.9, 50.4, 50.7, 51.5, 54, 55.4, 57, 62.7, 64, 64.4, 71.7, 72.4, 72.6, 72.7, 73.2, 74.2, 74.3, 74.9, 76.7, 78.9, 80.6, 81.2, 83.6, 86.4, 87, 91.3, 91.4];
            break;
        case "204":
            title = "各医院分离肺炎克雷伯菌对亚胺培南的耐药率（66-1838株）";
            text = ["JJH","ZGH","LSH","GSY","JLH","SXE","XJH","NXZ","NMG","PDH","GZH","SDH","HYD","TJJ","PUH","GLH","SZS","HXH","SCS","SYH","AYD","KMH","RJH","XYH","PED","BJH","SXS","SCH","BCH","SYF","TJH","JXE","HSH","HNH"];
            value = [0, 0.6, 1.7, 2.1, 2.1, 2.5, 3.2, 3.5, 5, 5.5, 5.6, 6.4, 7.3, 7.4, 9.3, 10.4, 15.1, 15.2, 15.8, 19.7, 26.3, 26.3, 27.6, 29.1, 32.1, 33.4, 36.4, 38.7, 40, 42.5, 42.9, 45.5, 52.7, 53.1];
            break;
        default:
    }

    $("#PageContentTitle").html(title);
    LoadEcharts(text, value, 45, 20);
}

//设置单个项目的显示和隐藏（公共函数）
function ShowStatus(className) {
    var status = $("#" + className).attr("data-status");
    if (status == "1") {
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

//取消选择
function ClearSelect() {

    $(".date-item-va").removeClass("active");
    var itemList = $(".date-item-va");

    for (var i = 0; i < itemList.length; i++) {
        $(itemList[i]).attr("data-status", "0");
    }

    //隐藏
    $("#dataError").css("display", "block");
    $(".main-content2").css("display", "none");
    $("#PageContentTitle").html("");
}
