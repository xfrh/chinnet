$(document).ready(function () {

    $("#search-city-ul li").click(function () {

        $("#search-city-ul li").removeClass("active");
        $(this).addClass("active")

        $("#SearchArea").val($(this).attr("SearchValue"));

        Search();
    })

    $("#search-hospital-ul li").click(function () {

        $("#search-hospital-ul li").removeClass("active");
        $(this).addClass("active")

        $("#SearchHospital").val($(this).attr("SearchValue"));

        Search();
    })

    $("#search-specimen-ul li").click(function () {

        $("#search-specimen-ul li").removeClass("active");
        $(this).addClass("active")

        $("#SearchSpecimen").val($(this).attr("SearchValue"));

        Search();
    })

    $("#search-bacteria-ul li").click(function () {

        $("#search-bacteria-ul li").removeClass("active");
        $(this).addClass("active")

        $("#SearchBacteria").val($(this).attr("SearchValue"));

        Search();
    })

    $("#search-department-ul li").click(function () {

        $("#search-department-ul li").removeClass("active");
        $(this).addClass("active")

        $("#SearchDepartment").val($(this).attr("SearchValue"));

        Search();
    })

})

function Search() {
    var area = $("#SearchArea").val();
    var hospital = $("#SearchHospital").val();
    var specimen = $("#SearchSpecimen").val();
    var bacteria = $("#SearchBacteria").val();
    var department = $("#SearchDepartment").val();

    location.href = "/Medicine/Index?area=" + area + "&hospital=" + hospital + "&specimen=" + specimen + "&bacteria=" + bacteria + "&department=" + department;
}


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