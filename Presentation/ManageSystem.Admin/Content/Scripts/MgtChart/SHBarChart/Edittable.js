
/* 添加数据项 */
onAddItemRow = function () {
    if ($('#item_tbody tr').length > 34) {
        toastr.warning('不能再添加了数据了');
        return false;
    }
    var input_id = Utils.Common.getUuid(),
        input_name = _.uniqueId('ItemModel[].Value_');

    $('#item_tbody').append(template('tpl_item_table_tr', {
        row_id: Utils.Common.getUuid(),
        province: '',
        input_id: input_id,
        input_name: input_name,
        input_value: ''
    }));
    setTimeout(function () {
        $('input[id="' + input_id + '"]').rules('add', {
            required: true,
            //number: true,
            //min: 0,
            messages: {
                required: "请录入数值",
                number: "请录入正确的数值",
                //min: "值不能小于0"
            }
        });
    }, 20);
};

/*插入数据项*/
onAddItemRow1 = function (id) {
    var l = Ladda.create(document.getElementById('btnAddItemRow'));
    if (l.isLoading()) { return false; }
    l.start();
    var $tbody = $('#single_datatable > tbody#item_tbody');
    var arrSort = [],
        arrData = [];

    $tbody.find('input[name^="ItemModel"][name$=".Sort"]').each(function (i, item) {
        if (!s.isBlank(item.value)) {
            var _sort = parseInt(item.value) || 0;
            if (_sort && _sort > 0) {
                arrSort.push(_sort);
            }
        }
    });

    arrData.push({
        uuid: Utils.Common.getUuid(),
        name: null,
        value: null,
        display: true,
        //sort: arrSort.length > 0 ? Math.max(...arrSort) + 1 : null
    });
    setTimeout(function () {
        $(id).after(template('template_singletable_tbody', { data: arrData })).updateSingleDataTableName();
        var length = $tbody.find('input[name^="ItemModel"][name$=".Sort"]').length;
        for (var i = 0; i < length; i++) {
            $('input[name="ItemModel[' + i + '].Sort"]').val(i + 1);
        }
        setTimeout(function () {
            l.stop();
        }, 300);
    }, 300);

}

/* 删除数据项 */
onDeleteItem = function (id) {
    $(id).remove();
};

$('#modal-excel').on('hidden.bs.modal', function () {
    if (!!window.ActiveXObject || "ActiveXObject" in window) {
        $('#excelFile').replaceWith($('#excelFile').clone(true));
    }
    else {
        $('#excelFile').val('');
    }
});

onSubmit = function () {
    $('#item_tbody tr[id]').each(function (i, item) {
        if (item.querySelector('select[name^="ItemModel"]').name === 'ItemModel[].Name') {
            var input_id = item.querySelector('input[name^="ItemModel[].Value"]').id;
            item.querySelector('select[name="ItemModel[].Name"]').name = 'ItemModel[' + i + '].Name';
            item.querySelector('input[name^="ItemModel[].Value_"]').name = 'ItemModel[' + i + '].Value';
            $('input[id="' + input_id + '"]').rules('remove');
            $('input[id="' + input_id + '"]').rules('add', {
                required: true,
                number: true,
                min: 0,
                messages: {
                    required: "请录入数值",
                    number: "请录入正确的数值",
                    min: "值不能小于0"
                }
            });
        }
    });

    return true;
};