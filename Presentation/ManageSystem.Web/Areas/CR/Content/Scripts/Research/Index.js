$(document).ready(function () {
    $("#search-tags-ul li").click(function () {
        $("#search-tags-ul li").removeClass("active");
        $(this).addClass("active")

        $("#SearchType").val($(this).attr("SearchValue"));

        Search();
    })

    $("#search-city-ul li").click(function () {

        $("#search-city-ul li").removeClass("active");
        $(this).addClass("active")

        $("#SearchArea").val($(this).attr("SearchValue"));

        Search();
    })

    $("#search-day-ul li").click(function () {

        $("#search-day-ul li").removeClass("active");
        $(this).addClass("active")

        $("#SearchDay").val($(this).attr("SearchValue"));

        Search();
    })

    $("#search-order-ul li").click(function () {

        $("#search-order-ul li").removeClass("active");
        $(this).addClass("active")

        $("#SearchOrderBy").val($(this).attr("SearchValue"));

        Search();
    })

})

function Search() {
    var type = $("#SearchType").val();
    var area = $("#SearchArea").val();
    var day = $("#SearchDay").val();
    var order = $("#SearchOrderBy").val();

    location.href = "/Research/Index?type=" + type + "&area=" + area + "&day=" + day + "&order=" + order;
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
        $("#search-city-ul").css("height", "40px");
        $("#MoreAreaState").val("0");
        $(".sl-e-area").removeClass("opened");
        $(".sl-e-area").html("更多<i></i>");
    }
}