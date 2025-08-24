function OpenDiv(obj, divId) {
    $(obj).tab('show');
    $(".nav-item").removeClass("active");
    $(obj).closest("li").addClass('active');
    $(".select-item").css("display", "none");
    $("#" + divId).css("display", "block");
}

function SelectDataItem(obj) {
    var selectItem = $(obj).attr("SelectItem");
    if (selectItem == "1") {
        $(obj).removeClass("info");
        $(obj).attr("SelectItem", "0");
    } else {
        $(obj).addClass('info');
        $(obj).attr("SelectItem", "1");
    }
}