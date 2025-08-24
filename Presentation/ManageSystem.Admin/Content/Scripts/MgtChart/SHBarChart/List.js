$(validateDataSegment());
$(validateEditDataSegment());

function validateDataSegment() {
    return $('#datasegmentform').validate({
        rules: {
            Name: {
                required: true,
                minlength: 3,
                maxlength: 50,
                remote: {
                    type: 'POST',
                    url: '/MgtSHChart/DataSegment_OnCheck',
                    data: {
                        Name: () => { return $('#DS_Name').val(); },
                        ProjectType: $('#ProjectType').val()
                    }
                }
            },
            Sort: {
                required: true,
                digits: true,
                min: 1
            }
        },
        messages: {
            Name: {
                required: '数据段名称不能为空',
                minlength: '请输入3-50个字符的数据段名称',
                maxlength: '请输入3-50个字符的数据段名称',
                remote: '此数据段名称已经存在'
            },
            Sort: {
                required: '排序编号不能为空',
                digits: '排序编号只能是整数',
                min: jQuery.validator.format("排序编号必须大于或等于{0}")
            }
        },
        errorClass: 'has-error',
        errorElement: 'span',
        wrapper: 'div',
        success: 'valid',
        errorPlacement: function (error, element) {
            error.appendTo(element.parent());
        },
        submitHandler: function (form) {
            alert("submit!");
        }
    });
}

function validateEditDataSegment() {
    return $('#EditDataSegmentForm').validate({
        rules: {
            Name: {
                required: true,
                minlength: 3,
                maxlength: 50,
                remote: {
                    type: 'POST',
                    url: '/MgtSHChart/DataSegment_OnCheck',
                    data: {
                        Id: () => { return $('#EditDataSegment_ID').val(); },
                        Name: () => { return $('#EditDS_Name').val(); },
                        ProjectType: $('#EditDataSegment_ProjectType').val()
                    }
                }
            },
            Sort: {
                required: true,
                digits: true,
                min: 1
            }
        },
        messages: {
            Name: {
                required: '数据段名称不能为空',
                minlength: '请输入3-50个字符的数据段名称',
                maxlength: '请输入3-50个字符的数据段名称',
                remote: '此数据段名称已经存在'
            },
            Sort: {
                required: '排序编号不能为空',
                digits: '排序编号只能是整数',
                min: jQuery.validator.format("排序编号必须大于或等于{0}")
            }
        },
        errorClass: 'has-error',
        errorElement: 'span',
        wrapper: 'div',
        success: 'valid',
        errorPlacement: function (error, element) {
            error.appendTo(element.parent());
        }
    });
}

/* 打开新增数据段modal */
onCreateDataSegment = function () {
    $('#modal-datasegment').modal('show');
};

/* 编辑数据段modal显示事件 */
$('#modal-datasegment').on('shown.bs.modal', function () {
    validateDataSegment().resetForm();
});

/* 编辑数据段modal隐藏事件 */
$('#modal-datasegment').on('hidden.bs.modal', function () {
    var $modal = $('#modal-datasegment');
    $modal.find('input[id="DS_Name"]').val(null);
    $modal.find('input[id="DS_Sort"]').val(null);

});

/* 提交新增数据段 */
onSubmitDataSegment = function () {
    var validator = validateDataSegment();
    if (validator.form()) {
        validateDataSegment().currentForm.submit();
    }
    return false;
};

/* 打开报表数据状态变更modal */
onShowSettingDisplay = function (id, state) {
    if (id) {
        $('#modal-display').find('input[id="SetDisplayId"]').val(id);
        $('#modal-display').find('input[id="SetDisplayValue"]').val(state === 1 ? 'true' : 'false');
        $('#modal-display').find('span[id="statusText"]').text(state === 1 ? '显示' : '不显示');
        $('#modal-display').modal('show');
    }
};

/* 报表数据状态变更modal隐藏事件 */
$('#modal-display').on('hidden.bs.modal', function () {
    $('#modal-display').find('input[id="SetDisplayId"]').val('');
    $('#modal-display').find('input[id="SetDisplayValue"]').empty();
    $('#modal-display').find('span[id="statusText"]').empty();
});

/* 提交报表数据状态变更 */
onSubmitDisplay = function () {
    var l = Ladda.create(document.getElementById('btnSubmitDisplay'));
    if (!l.isLoading()) {
        l.start();
        $('#displayform').submit();
    }
    return false;
};

/* 打开删除报表数据modal */
onShowDeleteBarChart = function (id) {
    if (id) {
        $('#modal-deleteHeatmap').find('input[id="DeleteHeatmapId"]').val(id);
        $('#modal-deleteHeatmap').modal('show');
    }
};

/* 删除报表数据modal隐藏事件 */
$('#modal-deletetable').on('hidden.bs.modal', function () {
    $('#modal-deletetable').find('input[id="DeleteHeatmapId"]').val('');
});

/* 删除报表数据modal隐藏事件 */
$('#modal-deleteHeatmap').on('hidden.bs.modal', function () {
    $('#modal-deleteHeatmap').find('input[id="DeleteHeatmapId"]').val('');
});

/* 提交删除报表数据 */
onSubmitDelete = function () {
    var l = Ladda.create(document.getElementById('btnSubmitDelete'));
    if (!l.isLoading()) {
        l.start();
        $('#deleteBarChartForm').submit();
    }
    return false;
};

/* 打开删除数据段modal */
onShowDeleteDataSegment = function (id) {
    if (id) {
        $('#modal-deleteDataSegment').find('input[id="DeleteDataSegmentId"]').val(id);
        $('#modal-deleteDataSegment').modal('show');
    }
};

/* 删除数据段modal隐藏事件 */
$('#modal-deleteDataSegment').on('hidden.bs.modal', function () {
    $('#modal-deleteDataSegment').find('input[id="DeleteDataSegmentId"]').val('');
});

/* 提交删除数据段按钮 */
onSubmitDataSegmentDelete = function () {
    var l = Ladda.create(document.getElementById('btnSubmitDataSegmentDelete'));
    if (!l.isLoading()) {
        l.start();
        $('#deleteDataSegmentForm').submit();
    }
    return false;
};

/* 打开编辑数据段modal */
onShowEditDataSegment = function (id) {
    if (id) {
        var strJson = $('#DataSegmentData_' + id).val();
        if (strJson) {
            var data = JSON.parse(strJson);
            var $modal = $('#modal-editdatasegment');
            $modal.find('input[id="EditDataSegment_ID"]').val(data.Id);
            $modal.find('input[id="EditDS_Name"]').val(data.Name);
            $modal.find('input[id="EditDS_Sort"]').val(data.Sort);
            $modal.modal('show');
        }
    }
    return false;
};

/* 编辑数据段modal显示事件 */
$('#modal-editdatasegment').on('shown.bs.modal', function () {
    validateEditDataSegment().resetForm();
});

/* 编辑数据段modal隐藏事件 */
$('#modal-editdatasegment').on('hidden.bs.modal', function () {
    var $modal = $('#modal-editdatasegment');
    $modal.find('input[id="EditDataSegment_ID"]').val(null);
    $modal.find('input[id="EditDS_Name"]').val(null);
    $modal.find('input[id="EditDS_Sort"]').val(null);
});

/* 提交编辑数据段 */
onSubmitEditDataSegment = function () {
    var l = Ladda.create(document.getElementById('btnSubmitEditDataSegment'));
    var validator = validateEditDataSegment();
    if (validator.form() && !l.isLoading()) {
        l.start();
        validator.currentForm.submit();
    }
    return false;
};

/* 显示更多 */
showReadMore = function (el) {
    if (el) {
        $el = $(el);
        $el.find('div.hide-article-box').hide();
        $el.height('');
    }
    return false;
};

/* 设置显示/隐藏 */
onShowDSSettingDisplay = function (id, status) {
    if (id) {
        var $modal = $('#modal-settingDisplay');
        $modal.find('input[id="SetDisplayId"]').val(id);
        if (status.toUpperCase() === 'SHOW') {
            $modal.find('h4[id="dsDisplayLabel"]').text('设置数据段显示');
            $modal.find('input[id="SetDisplayValue"]').val('True');
            $modal.find('span[id="statusText"]').text('显示');
        }
        else {
            $modal.find('h4[id="dsDisplayLabel"]').text('设置数据段隐藏');
            $modal.find('input[id="SetDisplayValue"]').val(0);
            $modal.find('span[id="statusText"]').text('隐藏');
        }
        $modal.modal('show');
    }
};

onSubmitDSDisplay = function () {
    var l = Ladda.create(document.getElementById('btnSubmitDSDisplay'));
    if (!l.isLoading()) {
        l.start();
        $('#dsDisplayForm').submit();
    }
    return false;
};

/* 设置默认显示 */
$('input.checkbox-slider[type="checkbox"]').on('click', function () {
    var $this = $(this),
        $modal = $('#modal_setting_default');
    $modal.find('#setting_default_id').val($this.val());
    if ($this.prop('checked')) {
        $modal.modal('show');
    }
    else {
        $modal.find('#setting_default_value').val('false');
        $('#settingDefaultForm').submit();
        $this.attr('disabled', 'disabled');
        setTimeout(function () {
            $this.removeAttr('disabled');
        }, 5000);
    }
});

/* 设置默认显示 */
onSettingDefault = function () {
    var l = Ladda.create(document.getElementById('btnSubmitSettingDefault')),
        $modal = $('#modal_setting_default');
    if (!l.isLoading()) {
        l.start();
        $modal.find('#setting_default_value').val('true');
        $('#settingDefaultForm').submit();
        $modal.find('button[data-dismiss="modal"]').attr('disabled', 'disabled');
        setTimeout(function () {
            $modal.find('button[data-dismiss="modal"]').removeAttr('disabled');
        }, 5000);
    }
}

/* 设置默认显示 */
onCancelDefault = function () {
    var $modal = $('#modal_setting_default');
    var _id = $modal.find('#setting_default_id').val();
    if (_id) {
        $('#setdefault_' + _id).prop('checked', false);
        $modal.find('#setting_default_id').val('');
    }
}