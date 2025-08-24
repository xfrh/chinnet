
    $(document).ready(function () {
        GetArea();

        GetHospital();

        GetBacteria();

        LayPageData(0);
    });

//选择区域
function SelectArea(obj)
{
        $("#search-city-ul li").removeClass("active");
        $(obj).addClass("active")

        $("#SearchArea").val($(obj).attr("SearchValue"));
      LayPageData(0);
}

//选择医院
function SelectHospital(obj)
{
        $("#search-hospital-ul li").removeClass("active");
        $(obj).addClass("active")

        $("#SearchHospital").val($(obj).attr("SearchValue"));
       LayPageData(0);
}

//选择药敏实验方法
function SelectBacteria(obj)
{
        $("#search-bacteria-ul li").removeClass("active");
        $(obj).addClass("active")

        $("#SearchBacteria").val($(obj).attr("SearchValue"));
     LayPageData(0);
}

$(document).ready(function () {


    $("#search-specimen-ul li").click(function () {

        $("#search-specimen-ul li").removeClass("active");
        $(this).addClass("active")

        $("#SearchSpecimen").val($(this).attr("SearchValue"));

         LayPageData(0);
    })

    $("#search-department-ul li").click(function () {

        $("#search-department-ul li").removeClass("active");
        $(this).addClass("active")

        $("#SearchDepartment").val($(this).attr("SearchValue"));

        LayPageData(0);
    })

})

//打开更多医院
function MoreHospital() {
    if ($("#MoreHospitalState").val() == "0") {
        //打开
        $("#search-hospital-ul").css("height", "auto");
        $("#MoreHospitalState").val("1");
        $(".sl-e-hospital").addClass("opened");
        $(".sl-e-hospital").html("收起<i></i>");

    } else {
        //关闭
        $("#search-hospital-ul").css("height", "38px");
        $("#MoreHospitalState").val("0");
        $(".sl-e-hospital").removeClass("opened");
        $(".sl-e-hospital").html("更多<i></i>");
    }
}

//打开更多地区
function MoreArea() {
    if ($("#MoreAreaState").val() == "0") {
        //打开
        $("#search-city-ul").css("height", "auto");
        $("#MoreAreaState").val("1");
        $(".sl-e-area").addClass("opened");
        $(".sl-e-area").html("收起<i></i>");

    } else {
        //关闭
        $("#search-city-ul").css("height", "38px");
        $("#MoreAreaState").val("0");
        $(".sl-e-area").removeClass("opened");
        $(".sl-e-area").html("更多<i></i>");
    }
}



    //加载地区数据
    function GetArea() {
        $.ajax({
            async: false,
            type: "POST",
            url: "/Medicine/GetArea",
            data: {},
            dataType: "json",
            success: function (json) {

                var html = "<li searchvalue='' class='active' onclick='SelectArea(this)' ><a href='javascript:'>全国</a></li>";
                if (json != null && json != "") {
                    for (var i = 0; i < json.length; i++) {
                        html += "<li searchvalue='" + json[i].Id + "' class='' onclick='SelectArea(this)'><a href='javascript:'>" + json[i].Name + "</a>  </li>";
                    }
                }
                $("#search-city-ul").html(html);
            }
        });
    }

    //加载医院数据
    function GetHospital() {
        $.ajax({
            async: false,
            type: "POST",
            url: "/Medicine/GetHospital",
            data: {},
            dataType: "json",
            success: function (json) {

                var html = "<li searchvalue='' class='active' onclick='SelectHospital(this)' ><a href='javascript:'>全部</a></li>";
                if (json != null && json != "") {
                    for (var i = 0; i < json.length; i++) {
                        html += "<li searchvalue='" + json[i].Id + "' class='' onclick='SelectHospital(this)' ><a href='javascript:'>" + json[i].Name + "</a>  </li>";
                    }
                }
                $("#search-hospital-ul").html(html);
            }
        });
    }

    //加载药敏实验方法
    function GetBacteria() {
        $.ajax({
            async: false,
            type: "POST",
            url: "/Medicine/GetBacteria",
            data: {},
            dataType: "json",
            success: function (json) {

                var html = "<li searchvalue='' class='active' onclick='SelectBacteria(this)' ><a href='javascript:'>全部</a></li>";
                if (json != null && json != "") {
                    for (var i = 0; i < json.length; i++) {
                        html += "<li searchvalue='" + json[i].Id + "' class='' onclick='SelectBacteria(this)' ><a href='javascript:'>" + json[i].Name + "</a>  </li>";
                    }
                }
                $("#search-bacteria-ul").html(html);
            }
        });
    }

    //加载数据验证的列表数据
    function LayPageData(curr) {

        $.ajax({
            type: 'POST',
            url: "/Medicine/GetMedicineList",
            dataType: 'json',
            data: {
                Page: curr || 1,  //向服务端传的参数，此处只是演示
                PageSize: 10,//每页10行
                Area: $("#SearchArea").val(),
                Hospital: $("#SearchHospital").val(),
                // Specimen : $("#SearchSpecimen").val(),
                //  Department :$("#SearchDepartment").val(),
                Bacteria: $("#SearchBacteria").val()
            },
            success: function (result) {

                //绑定页面的数据
                if (result.rows != null && result.rows.length > 0) {
                    var html = "";
                    var index = 0;
                    for (var i = 0; i < result.rows.length; i++) {
                        var item = result.rows[i];

                        html += "  <tr  >" +
                            "  <td> <a style='color:#00A6F5;' href='/Medicine/Detail/" + item.Id + "' target='_blank'>" + item.SN + "</a> </td> " +
                            "  <td> 	" + item.InsertTime + "</td>         " +
                            "  <td> 	" + item.HospitalName + "</td>     " +
                            "  <td> 	" + item.AreaId + "</td>                 " +
                            "  <td> 	" + item.AreName + "</td>         " +
                            "</tr>";
                    }
                    $("#validateDataTbody").html(html);
                } else {
                    $("#validateDataTbody").html("");
                }

                //显示分页
                laypage({
                    cont: "divIncomeIntegral", //容器。值支持id名、原生dom对象，jquery对象。【如该容器为】：<div id="page1"></div>
                    pages: result.total, //通过后台拿到的总页数
                    curr: curr || 1, //当前页
                    skip: true, //是否开启跳页
                    skin: '#AF0000',
                    groups: 7, //连续显示分页数
                    jump: function (obj, first) { //触发分页后的回调
                        if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                            LayPageData(obj.curr);
                        }
                    }
                });
            },
        });
    }