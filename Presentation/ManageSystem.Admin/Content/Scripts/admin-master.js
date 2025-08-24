
$().ready(function () {

    //设置修改头像是否显示
    $('#UserHeadImage').mousemove(function () {
        $("#ChangeHeadImage").show();
    });

    $('#UserHeadImage').mouseout(function () {
        $("#ChangeHeadImage").hide();
    });

})
