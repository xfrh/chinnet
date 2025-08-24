$(function() {
	$('.jd-see-more').click(function(){
		if ($(this).parent().hasClass('active')) {
			$(this).parent().removeClass('active');
			$(this).text('更多');
		}else{
			$(this).parent().addClass('active');
			$(this).text('收起');
		}
	})
	$('.cy-search').click(function(){
		$('.cy-search').css('width','160px');
		$('#search_cy').removeClass('hide');
	})
})
