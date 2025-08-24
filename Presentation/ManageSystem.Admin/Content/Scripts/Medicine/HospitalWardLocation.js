
$(function () {

    $('#hospital_id').chosen({
        no_results_text: '没有找到与之匹配的医院',
        search_contains: true,
        allow_single_deselect: true,
        inherit_select_classes: true,
        placeholder_text_single: '请输入或选择医院'
    });

    $('#hospital_id').css({
        'display': 'block',
        'width': '1px',
        'height': '1px',
        'position': 'absolute',
        'left': '-1000rem'
    });

    $("#hospital_id").chosen().change(function () {
        //$(this).valid();
    });

    var options = {
        serviceUrl: '/Medicine/SearchHospitalByWardLocation',
        type: 'POST',
        paramName: 'name',
        deferRequestBy: 300,
        onSelect: function (suggestion) {
            if (suggestion.id) {
                $('#hospital_id').val(suggestion.id);
            }
            else {
                $('#hospital_id').val('');
            }
        }
    };
    $('#hospital').autocomplete(options);

    onSubmit = function () {
        var l = Ladda.create(document.getElementById('btnSubmit')),
            hospital_id = $('#hospital_id').val() || 0,
            files = $('#excelFile')[0].files,
            formData = new FormData();

        if (!hospital_id || hospital_id === 0 || hospital_id === '0') {
            toastr.error('请输入或选择医院名称', '所属医院', {
                positionClass: 'toast-top-center'
            });
            return false;
        }

        if (!files || files.length <= 0) {
            alert('请选择医院科室配置文件');
            return false;
        }
        if (!l.isLoading()) {
            formData.append('id', hospital_id);
            formData.append('file', files[0]);
            $.ajax({
                url: '/Medicine/OnSubmit_HospitalWardLocation',
                method: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                beforeSend: function () { l.start(); },
                complete: function () { setTimeout(function () { l.stop(); }, 600); },
                success: function (res) {
                    alert(res.message);
                    if (res.status) {
                        location.reload();
                    }
                }
            });
        }
        return false;
    };
});