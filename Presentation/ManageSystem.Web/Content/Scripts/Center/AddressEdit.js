

//选择省市区以后的回调函数
function SelectAreaCallBack(areaId) {
    $("#DistrictsId").val("" + areaId + "");
    $("#ProvinceId").val($("#ddlProvince").val());
    $("#CityId").val($("#ddlCity").val());
}


function Save() {

    if ($("#form").valid()) {  //触发验证

        $.ajax({
            type: "POST",
            url: "/Center/AddressEdit",
            data: {
            Id:$("#Id").val(),
            Name: $("#Name").val(),
            Phone: $("#Phone").val(),
            Tel: $("#Tel").val(),
            IsMain: $("#IsMain").prop("checked"),
            ProvinceId: $("#ProvinceId").val(),
            CityId: $("#CityId").val(),
            DistrictsId: $("#DistrictsId").val(),
            Address: $("#Address").val(),
            Email: $("#Email").val()
            },
        dataType: "text",
        success: function (data) {
            if (data == "" || data == null) return;

            if (data != "true") {
                MessageWindow(data);
            } else {
                parent.ReloadPage();
                parent.layer.close(index);
            }
        }
    });
}

}
