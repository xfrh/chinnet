
$(function () {
    SetMettingView();
    mySwiper = new Swiper('.swiper-container', {
        direction: 'vertical',
        loop: false,
        observer: true,
        observeParents: false,
        watchSlidesProgress: true,
        freeMode: true,
        slidesPerView: 'auto'
    });
});



function SetMettingView() {
    $.ajax({
        async: false,
        url: '/Information/AddMettingView',
        type: 'POST',
        data: {
            token: localStorage.getItem('token'),
            meetingId: $('#MeetingId').val()
        },
        dataType: 'json',
        cache: false,
        timeout: 0
    });
}