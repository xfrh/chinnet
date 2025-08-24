
//保存
function EditProject() {

    var id = $("#Id").val();
    if (id == null || id == "") {
        ErrorWindow("数据不存在，请重新打开");
        return;
    }

    if ($("#HospitalName").val() == "") {
        ErrorWindow("请选择输入医院名称");
        return;
    }
    if ($("#HospitalGrade").val() == "") {
        ErrorWindow("请选择输入医院等级");
        return;
    }

    if ($("#ddlProvince").val() == "" || $("#ddlProvince").val() == "0") {
        ErrorWindow("请选择省市区");
        return;
    }

    if ($("#ddlCity").val() == "" || $("#ddlCity").val() == "0") {
        ErrorWindow("请选择省市区");
        return;
    }

    if ($("#ddlArea").val() == "" || $("#ddlArea").val() == "0") {
        ErrorWindow("请选择省市区");
        return;
    }

    if ($("#Address").val() == "") {
        ErrorWindow("请选择输入详细地址");
        return;
    }

    //if ($("#CREDetectionRate").val() == "") {
    //    ErrorWindow("请选择输入上年CRE平均检出率");
    //    return;
    //}
    //if ($("#CREDrugRate").val() == "") {
    //    ErrorWindow("请选择输入上年CRAB平均检出率");
    //    return;
    //}
    if (!$("#MHSource").val() || $("#MHSource").val() === "") {
        ErrorWindow("请输入MH平板来源厂家");
        return;
    }
    if ($("#Name").val() == "") {
        ErrorWindow("请选择输入负责人姓名");
        return;
    }

    if ($("#Phone").val() == "") {
        ErrorWindow("请选择输入联系电话");
        return;
    }

    var email = $("#Email").val();
    if (email.indexOf("@") == -1) {
        ErrorWindow("邮箱格式不正确");
        return;
    }

    var data = {
        Id: id,
        HospitalName: $("#HospitalName").val(),
        HospitalGrade: $("#HospitalGrade").val(),
        ProvinceId: $("#ddlProvince").val(),
        CityId: $("#ddlCity").val(),
        DistrictsId: $("#ddlArea").val(),
        Province: $('#ddlProvince option:selected').text(),
        City:$('#ddlCity option:selected').text(),
        Area: $('#ddlArea option:selected').text(),
        Address: $("#Address").val(),
        MHSource: $("#MHSource").val(),
        Name: $("#Name").val(),
        Phone: $("#Phone").val(),
        Email: $("#Email").val(),
        HospitalId: $("#HospitalId").val()
    };

    $.ajax({
        async: false,
        type: "POST",
        url: "/CR/CRUpload/Edit",
        data: data,
        dataType: "json",
        success: function (json) {

            if (json == null || json == "") {
                ErrorWindow("添加失败，请重试！", true);
                return;
            }

            if (json.status == "true") {
                SuccessWindow("修改成功！", false, "/CR/CRUpload/Upload");
            } else {
                ErrorWindow(json.message, false);
            }
        }

    });
}




