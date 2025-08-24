// list 页面的公用js，请勿随意修改

var selectedIds = [];

$(document).ready(function () {

    //查询事件
    $('#btn-search').click(function () {
        var grid = $('#data-grid').data('kendoGrid');
        grid.dataSource.page(1);
        $('.checkboxGroups').attr('checked', false).change();
        selectedIds = [];
        return false;
    });

    //全选功能
    $(document).on('click', '#mastercheckbox',function () {
        $('.checkboxGroups').prop('checked', $(this).is(':checked')).change();
    });
    //$('#mastercheckbox').click(function () {
    //    $('.checkboxGroups').prop('checked', $(this).is(':checked')).change();
    //});

    //选中 checkboxes.
    $('#data-grid').on('change', 'input[type=checkbox][id!=mastercheckbox]', function (e) {
        var $check = $(this);
        if ($check.is(":checked") == true) {
            var checked = jQuery.inArray($check.val(), selectedIds);
            if (checked == -1) {
                //add id to selectedIds.
                selectedIds.push($check.val());
            }
        }
        else {
            var checked = jQuery.inArray($check.val(), selectedIds);
            if (checked > -1) {
                //remove id from selectedIds.
                selectedIds = $.grep(selectedIds, function (item, index) {
                    return item != $check.val();
                });
            }
        }
        updateMasterCheckbox();
    });


    //删除的事件
    $('#btn-delete').click(function (e) {

        if (selectedIds == null || selectedIds.length <= 0) {
            alert("请勾选需要删除的数据！")
            return;
        }

        if (confirm("不可恢复操作，确认删除吗？")) {
            e.preventDefault();
            var ids = selectedIds.join(",");
            $('#delete-selected-form #selectedIds').val(ids);
            $('#delete-selected-form').submit();
            return false;
        }
    });


}
);

//加载数据的时候检查选中
function onDataBound(e) {

    $('#data-grid input[type=checkbox][id!=mastercheckbox]').each(function () {
        var currentId = $(this).val();
        var checked = jQuery.inArray(currentId, selectedIds);
        //set checked based on if current checkbox's value is in selectedIds.
        $(this).attr('checked', checked > -1);
    });

    updateMasterCheckbox();
}

//设置全选的checkboxes
function updateMasterCheckbox() {
    var numChkBoxes = $('#data-grid input[type=checkbox][id!=mastercheckbox]').length;
    var numChkBoxesChecked = $('#data-grid input[type=checkbox][id!=mastercheckbox]:checked').length;
    $('#mastercheckbox').attr('checked', numChkBoxes == numChkBoxesChecked && numChkBoxes > 0);
}
