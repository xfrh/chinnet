Vue.use(VeeValidate, { locale: 'zh_CN' });
var app = new Vue({
    el: '#app',
    data: {
        mySwiper: null,
        page_loading: null,
        Q3: {
            title: '请从标本来源中分别选出对应的CR检出率：',
            options: [
                { id: 'checkbox__3_1', name: '_3_biaoben', text: '痰', value: '痰', rate: null, rate_name: '_3_biaoben_1_rate', divid: '3_biaoben_1_rate' },
                { id: 'checkbox__3_2', name: '_3_biaoben', text: '血液', value: '血液', rate: null, rate_name: '_3_biaoben_2_rate', divid: '3_biaoben_2_rate' },
                { id: 'checkbox__3_3', name: '_3_biaoben', text: '粪便', value: '粪便', rate: null, rate_name: '_3_biaoben_3_rate', divid: '3_biaoben_3_rate' },
                { id: 'checkbox__3_4', name: '_3_biaoben', text: '肺泡灌洗液', value: '肺泡灌洗液', rate: null, rate_name: '_3_biaoben_4_rate', divid: '3_biaoben_4_rate' },
                { id: 'checkbox__3_5', name: '_3_biaoben', text: '脓液/伤口', value: '脓液/伤口', rate: null, rate_name: '_3_biaoben_5_rate', divid: '3_biaoben_5_rate' },
                { id: 'checkbox__3_6', name: '_3_biaoben', text: '尿道', value: '尿道', rate: null, rate_name: '_3_biaoben_6_rate', divid: '3_biaoben_6_rate' },
                { id: 'checkbox__3_7', name: '_3_biaoben', text: '中心静脉导管', value: '中心静脉导管', rate: null, rate_name: '_3_biaoben_7_rate', divid: '3_biaoben_7_rate' },
                { id: 'checkbox__3_8', name: '_3_biaoben', text: '穿刺液', value: '穿刺液', rate: null, rate_name: '_3_biaoben_8_rate', divid: '3_biaoben_8_rate' },
                { id: 'checkbox__3_9', name: '_3_biaoben', text: '其他', value: '其他', rate: null, rate_name: '_3_biaoben_9_rate', divid: '3_biaoben_9_rate' }
            ],
            answer: [],
            other: null,
            picker: [
                { label: '0-5%', value: '0-5%' },
                { label: '6%-10%', value: '6%-10%' },
                { label: '11%-15%', value: '11%-15%' },
                { label: '16%-20%', value: '16%-20%' },
                { label: '21%-30%', value: '21%-30%' },
                { label: '31%-40%', value: '31%-40%' },
                { label: '41%-50%', value: '41%-50%' },
                { label: '51%以上', value: '51%以上' }
            ]
        },
        Q4: {
            title: '请从科室来源中分别选出对应的CR检出率：',
            options: [
                { id: 'checkbox__4_1', name: '_4_keshi', text: '血液科', value: '血液科', rate: null, rate_name: '_4_biaoben_1_rate', divid: '4_biaoben_1_rate' },
                { id: 'checkbox__4_2', name: '_4_keshi', text: 'ICU', value: 'ICU', rate: null, rate_name: '_4_biaoben_2_rate', divid: '4_biaoben_2_rate' },
                { id: 'checkbox__4_3', name: '_4_keshi', text: '呼吸科', value: '呼吸科', rate: null, rate_name: '_4_biaoben_3_rate', divid: '4_biaoben_3_rate' },
                { id: 'checkbox__4_4', name: '_4_keshi', text: '感染科', value: '感染科', rate: null, rate_name: '_4_biaoben_4_rate', divid: '4_biaoben_4_rate' },
                { id: 'checkbox__4_5', name: '_4_keshi', text: '移植科', value: '移植科', rate: null, rate_name: '_4_biaoben_5_rate', divid: '4_biaoben_5_rate' },
                { id: 'checkbox__4_6', name: '_4_keshi', text: '其他', value: '其他', rate: null, rate_name: '_4_biaoben_6_rate', divid: '4_biaoben_6_rate' }
            ],
            answer: [],
            other: null,
            picker: [
                { label: '0-5%', value: '0-5%' },
                { label: '6%-10%', value: '6%-10%' },
                { label: '11%-15%', value: '11%-15%' },
                { label: '16%-20%', value: '16%-20%' },
                { label: '21%-30%', value: '21%-30%' },
                { label: '31%-40%', value: '31%-40%' },
                { label: '41%-50%', value: '41%-50%' },
                { label: '51%以上', value: '51%以上' }
            ]
        },
        Q25: {
            title: '目前实验室检测碳青霉烯酶的方法',
            type: 'checkbox',
            name: 'Q25',
            options: [
                { id: 'checkbox__25_1', name: 'Q25_checkbox', text: '(1) 仅以药敏试验结果判断CRE', value: '(1) 仅以药敏试验结果判断CRE' },
                { id: 'checkbox__25_2', name: 'Q25_checkbox', text: '(2) 改良Hodge试验', value: '(2) 改良Hodge试验' },
                { id: 'checkbox__25_3', name: 'Q25_checkbox', text: '(3) Carba NP', value: '(3) Carba NP' },
                { id: 'checkbox__25_4', name: 'Q25_checkbox', text: '(4) mCIM和eCIM', value: '(4) mCIM和eCIM' },
                { id: 'checkbox__25_5', name: 'Q25_checkbox', text: '(5) EDTA和APB抑制试验', value: '(5) EDTA和APB抑制试验' },
                { id: 'checkbox__25_6', name: 'Q25_checkbox', text: '(6) 金标免疫快速检测技术', value: '(6) 金标免疫快速检测技术' },
                { id: 'checkbox__25_7', name: 'Q25_checkbox', text: '(7) 常规PCR技术', value: '(7) 常规PCR技术' },
                { id: 'checkbox__25_8', name: 'Q25_checkbox', text: '(8) GeneXpert', value: '(8) GeneXpert' },
                { id: 'checkbox__25_9', name: 'Q25_checkbox', text: '(9) 国产碳青霉烯酶基因检测试剂盒', value: '(9) 国产碳青霉烯酶基因检测试剂盒' }
            ],
            answer: []
        },
        Q26: {
            title: '药敏试验是否常规报告CRE所产碳青霉烯酶型别？',
            type: 'radio',
            name: 'Q26',
            options: [
                { id: 'radio_26_1', name: 'Q26_radio', text: '否', value: '否' },
                { id: 'radio_26_2', name: 'Q26_radio', text: '是', value: '是' }
            ],
            answer: null,
            other: null
        },
        Q11: {
            title: '您单位分离的 CR-ECO 主要产哪种碳青霉烯酶及菌株数',
            options: [
                { id: 'checkbox__11_1', name: '_11_biaoben', text: 'KPC', value: 'KPC', rate: null, rate_name: '_11_biaoben_1_rate', divid: '11_biaoben_1_rate'},
                { id: 'checkbox__11_2', name: '_11_biaoben', text: 'NDM', value: 'NDM', rate: null, rate_name: '_11_biaoben_2_rate', divid: '11_biaoben_2_rate' },
                { id: 'checkbox__11_3', name: '_11_biaoben', text: 'OXA-48', value: 'OXA-48', rate: null, rate_name: '_11_biaoben_3_rate', divid: '11_biaoben_3_rate' },
                { id: 'checkbox__11_4', name: '_11_biaoben', text: 'IPM', value: 'IPM', rate: null, rate_name: '_11_biaoben_4_rate', divid: '11_biaoben_4_rate' },
                { id: 'checkbox__11_5', name: '_11_biaoben', text: 'VIM', value: 'VIM', rate: null, rate_name: '_11_biaoben_5_rate', divid: '11_biaoben_5_rate'},
                { id: 'checkbox__11_6', name: '_11_biaoben', text: '其他', value: '其他', rate: null, rate_name: '_11_biaoben_6_rate', divid: '11_biaoben_6_rate' },
            ],
            answer: [],
        },
        Q12: {
            title: '请输入检测出的各类抗生素耐药株数',
            options: [
                { id: 'checkbox__12_1', name: '_12_biaoben', text: '(1) 多粘菌素', value: '(1) 多粘菌素', rate: null, rate_name: '_12_biaoben_1_rate', divid:'12_biaoben_1_rate' },
                { id: 'checkbox__12_2', name: '_12_biaoben', text: '(2) 替加环素', value: '(2) 替加环素', rate: null, rate_name: '_12_biaoben_2_rate', divid: '12_biaoben_2_rate' },
                { id: 'checkbox__12_3', name: '_12_biaoben', text: '(3) 头孢他啶-阿维巴坦', value: '(3) 头孢他啶-阿维巴坦', rate: null, rate_name: '_12_biaoben_3_rate', divid: '12_biaoben_3_rate' },
                { id: 'checkbox__12_4', name: '_12_biaoben', text: '(4) 磷霉素', value: '(4) 磷霉素', rate: null, rate_name: '_12_biaoben_4_rate', divid: '12_biaoben_4_rate' },
                { id: 'checkbox__12_5', name: '_12_biaoben', text: '(5) 氯霉素', value: '(5) 氯霉素', rate: null, rate_name: '_12_biaoben_5_rate', divid: '12_biaoben_5_rate' },
                { id: 'checkbox__12_6', name: '_12_biaoben', text: '(6) 联合药敏试验', value: '(6) 联合药敏试验', rate: null, rate_name: '_12_biaoben_6_rate', divid: '12_biaoben_6_rate' },
            ],
            answer: [],
        },
    },
    created: function () {
        this.$nextTick(() => {
        });
    },
    methods: {
        GoTop() {
            mySwiper.slideTo(0, 100, false);
            return false;
        },
        toPage(url) {
            if (url) {
                location.href = url;
            }
        },
        onQ3Picker: function (e, obj) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q3.picker[0].value;
                if (obj.rate && obj.rate.length > 0) {
                    defaultValue = obj.rate;
                }

                weui.picker(that.Q3.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        obj.rate = result[0].value;
                    },
                    id: window.uuid(8, 18)
                });
            });
        },
        onQ4Picker: function (e, obj) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q4.picker[0].value;
                if (obj.rate && obj.rate.length > 0) {
                    defaultValue = obj.rate;
                }

                weui.picker(that.Q4.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        obj.rate = result[0].value;
                    },
                    id: window.uuid(8, 18)
                });
            });
        },       
    }
});

GetDistrictList(0, 1)
function GetDistrictList(parentId, location) {
    $.ajax({
        url: "/CreUploadData/GetArea",
        type: "Post",
        dataType: "json",
        data: { parentId: parentId },
        success: function (json) {

            if (json == null || json == "") {
                return;
            }
            //<option value=\"0\" >-- 请选择 --</option>
            var content = "<option value=\"0\" >--请选择--</option>";
            //var content = "";
            for (var i = 0; i < json.length; i++) {
                content += " <option value=\"" + json[i].Id + "\" >" + json[i].Name + "</option>";
            }

            switch (location) {
                case 1:
                    //绑定省份
                    $("#province").html(content);
                    break;
                case 2:
                    //绑定市
                    $("#City").html(content);
                    break;
                case 3:
                    //绑定区县
                    $("#Area").html(content);
                    break;

                default:


            }


        },

        error: function (x, e) { },
        complete: function (x) { }
    });
}
//选择省份事件，绑定市数据
function ProvinceClick() {
    var provinceId = $("#province").val();
    if (provinceId == null || provinceId == "" || provinceId == "0") {
        $("#Area").html("<option value=\"0\" >-- 请选择 --</option>");
        $("#City").html("<option value=\"0\" >-- 请选择 --</option>");
        return;
    }

    GetDistrictList(provinceId, 2);
}
function CityClick() {
    var cityId = $("#City").val();
    if (cityId == null || cityId == "" || cityId == 0) {
        return;
    }
    GetDistrictList(cityId, 3);
}

function SetDistrictSelect(provinceId, cityId, areaId) {

    if (provinceId == null || provinceId == "") return;

    //绑定省
    SetProvinceSelect(provinceId, cityId, areaId);
}
//绑定省
function SetProvinceSelect(provinceId, cityId, areaId) {
    $.ajax({
        url: "/CreUploadData/GetArea",
        type: "Post",
        dataType: "json",
        data: { parentId: 0 },
        success: function (json) {
            var html = GetSetDistrictSelectHtml(json);
            $("#province").html(html);
            $("#province").val(provinceId);
            //绑定市
            SetCitySelect(provinceId, cityId, areaId);
        }
    });
}

//绑定市
function SetCitySelect(provinceId, cityId, areaId) {
    $.ajax({
        url: "/CreUploadData/GetArea",
        type: "Post",
        dataType: "json",
        data: { parentId: provinceId },
        success: function (json) {

            var html = GetSetDistrictSelectHtml(json);
            $("#City").html(html);
            $("#City").val(cityId);

            //绑定区
            SetAreaSelect(provinceId, cityId, areaId);
        }
    });
}

//绑定区
function SetAreaSelect(provinceId, cityId, areaId) {
    $.ajax({
        url: "/CreUploadData/GetArea",
        type: "Post",
        dataType: "json",
        data: { parentId: cityId },
        success: function (json) {
            var html = GetSetDistrictSelectHtml(json);
            $("#Area").html(html);
            $("#Area").val(areaId);
        }
    });
}

//获取html
function GetSetDistrictSelectHtml(json) {
    var content = " <option value=\"0\" >-- 请选择 --</option>";
    for (var i = 0; i < json.length; i++) {
        content += " <option value=\"" + json[i].Id + "\" >" + json[i].Name + "</option>";
    }
    return content;
}

var date = new Date();
//var tYear = date.getFullYear();
//$("#year").val(tYear);
month = date.getMonth() + 1;
//var content = "";
//if (tYear == 2019) {
//    $("#Ession").html(" <option value=" + 4 + ">第四季度（10月-12月）</option>")
//}
//else if (tYear > 2019 && month >= 1 && month <= 3) {
//    content += "<option value=" + 1 + ">第一季度（1月-3月）</option>";
//    $("#Ession").html(content);
//}
//else if (tYear > 2019 && month >= 4 && month <= 6) {
//    content += "<option value=" + 1 + ">第一季度（1月-3月）</option>";
//    content += "<option value=" + 2 + ">第二季度（4月-6月）</option>";
//    $("#Ession").html(content);

//}
//else if (tYear > 2019 && month >= 7 && month <= 9) {
//    content += "<option value=" + 1 + ">第一季度（1月-3月）</option>";
//    content += "<option value=" + 2 + ">第二季度（4月-6月）</option>";
//    content += "<option value=" + 3 + ">第三季度（7月-9月）</option>";
//    $("#Ession").html(content);
//}
//else if (tYear > 2019 && month >= 10 && month <= 12) {
//    content += "<option value=" + 1 + ">第一季度（1月-3月）</option>";
//    content += "<option value=" + 2 + ">第二季度（4月-6月）</option>";
//    content += "<option value=" + 3 + ">第三季度（7月-9月）</option>";
//    content += "<option value=" + 4 + ">第四季度（10月-12月）</option>";
//    $("#Ession").html(content);
//}
$("#year").change(function () {
    var content = "";
    var tYear = $("#year").val();
    if (tYear == 2019) {
        $("#Ession").html(" <option value=" + 4 + ">第四季度（10月-12月）</option>")
    }
    else if (tYear > 2019 && month >= 1 && month <= 3) {
        content += "<option value=" + 1 + ">第一季度（1月-3月）</option>";
        $("#Ession").html(content);
    }
    else if (tYear > 2019 && month >= 4 && month <= 6) {
        content += "<option value=" + 1 + ">第一季度（1月-3月）</option>";
        content += "<option value=" + 2 + ">第二季度（4月-6月）</option>";
        $("#Ession").html(content);

    }
    else if (tYear > 2019 && month >= 7 && month <= 9) {
        content += "<option value=" + 1 + ">第一季度（1月-3月）</option>";
        content += "<option value=" + 2 + ">第二季度（4月-6月）</option>";
        content += "<option value=" + 3 + ">第三季度（7月-9月）</option>";
        $("#Ession").html(content);
    }
    else if (tYear > 2019 && month >= 10 && month <= 12) {
        content += "<option value=" + 1 + ">第一季度（1月-3月）</option>";
        content += "<option value=" + 2 + ">第二季度（4月-6月）</option>";
        content += "<option value=" + 3 + ">第三季度（7月-9月）</option>";
        content += "<option value=" + 4 + ">第四季度（10月-12月）</option>";
        $("#Ession").html(content);
    }
    var tYear = $("#year").val();
    dataswitch(tYear)
})
$("#Ession").change(function () {
    //season = $("#Ession").val();
    var tYear = $("#year").val();
    dataswitch(tYear);

})
//跳转提示
function clicks(id) {
    if (confirm("即将离开当前页，请确认数据已【提交】！")) {
        location.href = "/cre/CreUploadData/Dataupload?creid=" + id;
    }
}