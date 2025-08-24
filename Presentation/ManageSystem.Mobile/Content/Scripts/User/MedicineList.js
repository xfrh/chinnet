var mySwiper,
    app = new Vue({
        el: '#app',
        data: {
            dataUrl: '/User/MedicineList',
            items: [],
            pageConfig: {
                page: 1,
                pageSize: 15
            },
            nextPageLoading: false,
            noNetxPage: false,
            pageNull: false,
            ajaxLoading: false,
            resFailMessage: null,
            dialogContent: null,
            loadingText: '加载中'
        },
        created: function () {
            this.$nextTick(() => {
                var _this = this,
                    $loadingToast = $(_this.$refs.loadingToast);
                $.ajax({
                    url: _this.dataUrl,
                    type: 'POST',
                    data: {
                        token: localStorage.getItem('token'),
                        page: _this.pageConfig.page,
                        pageSize: _this.pageConfig.pageSize
                    },
                    dataType: 'json',
                    cache: false,
                    timeout: 0,
                    beforeSend: function () {
                        _this.pageNull = false;
                        _this.loadingText = '加载中';
                        $loadingToast.fadeIn(200);
                    },
                    error: function () {
                        _this.resFailMessage = '网络请求失败.';
                        $loadingToast.fadeOut(50);
                        $(_this.$refs.failToast).fadeIn(100);
                        setTimeout(function () {
                            $(_this.$refs.failToast).fadeOut(200);
                            _this.resFailMessage = null;
                        }, 1800);
                    },
                    success: function (res) {
                        console.log(res);
                        if (res.status) {
                            if (res.data.length === 0) {
                                // 首次加载无数据
                                _this.pageNull = true;
                                setTimeout(function () {
                                    var listNullDivHeight = _this.$refs.middle.offsetHeight * 0.8 - document.documentElement.clientWidth * 0.26 * 2 / 10,
                                        swipercontainerHeight = _this.$refs.middle.offsetHeight - document.documentElement.clientWidth * 0.26 * 2 / 10;
                                    _this.$refs.listNullDiv.style.height = listNullDivHeight + 'px';
                                    _this.$refs.swiper_container.style.height = swipercontainerHeight + 'px';
                                });
                            }
                            else {
                                _this.pageConfig.page = res.nextPage;
                                _this.noNetxPage = !res.existNextPage;

                                $.each(res.data, function (index, item) { _this.items.push(item); });
                                _this.initSwiper();
                            }
                            setTimeout(() => {
                                $loadingToast.fadeOut(200);
                                _this.loadingText = null;
                            }, 800);
                        }
                        else {
                            _this.dialogContent = res.message;
                            $loadingToast.fadeOut(50);
                            $(_this.$refs.iosDialog).fadeIn(200);
                        }
                    }
                });
            });
        },
        methods: {
            initSwiper() {
                var _this = this,
                    swipercontainerHeight = _this.$refs.middle.offsetHeight - document.documentElement.clientWidth * 0.26 * 2 / 10;
                _this.$refs.swiper_container.style.height = swipercontainerHeight + 'px';

                setTimeout(() => {
                    mySwiper = new Swiper('.swiper-container', {
                        direction: 'vertical',
                        loop: false,
                        observer: true,
                        observeParents: false,
                        watchSlidesProgress: true,
                        freeMode: true,
                        slidesPerView: 'auto',
                        on: {
                            touchMove() {
                                var _viewHeight = document.getElementsByClassName('swiper-wrapper')[0].offsetHeight,
                                    _contentHeight = document.getElementsByClassName('swiper-slide')[0].offsetHeight;
                                if (mySwiper.translate <= _viewHeight - _contentHeight + 150 && mySwiper.translate < 0) {
                                    if (!_this.nextPageLoading && !_this.noNetxPage && !_this.ajaxLoading) {
                                        _this.nextPageLoading = true;
                                        setTimeout(() => {
                                            mySwiper.update();
                                            setTimeout(() => { _this.moreItems(); }, 800);
                                        });
                                    }
                                }
                            },
                            touchEnd() {
                                var _viewHeight = document.getElementsByClassName('swiper-wrapper')[0].offsetHeight,
                                    _contentHeight = document.getElementsByClassName('swiper-slide')[0].offsetHeight;

                                if (mySwiper.translate <= _viewHeight - _contentHeight + 150 && mySwiper.translate < 0) {
                                    if (!_this.nextPageLoading && !_this.noNetxPage && !_this.ajaxLoading) {
                                        _this.nextPageLoading = true;
                                        setTimeout(() => {
                                            mySwiper.update();
                                            setTimeout(() => { _this.moreItems(); }, 800);
                                        });
                                    }
                                }
                            },
                            transitionEnd() {
                                var _viewHeight = document.getElementsByClassName('swiper-wrapper')[0].offsetHeight,
                                    _contentHeight = document.getElementsByClassName('swiper-slide')[0].offsetHeight;

                                if (mySwiper.translate <= _viewHeight - _contentHeight + 150 && mySwiper.translate < 0) {
                                    if (!_this.nextPageLoading && !_this.noNetxPage && !_this.ajaxLoading) {
                                        _this.nextPageLoading = true;
                                        setTimeout(() => {
                                            mySwiper.update();
                                            setTimeout(() => { _this.moreItems(); }, 800);
                                        });
                                    }
                                }
                            }
                        }
                    });
                });
            },
            moreItems() {
                var _this = this;
                $.ajax({
                    url: _this.dataUrl,
                    type: 'POST',
                    data: {
                        token: localStorage.getItem('token'),
                        page: _this.pageConfig.page,
                        pageSize: _this.pageConfig.pageSize
                    },
                    dataType: 'json',
                    cache: false,
                    timeout: 0,
                    beforeSend: function () {
                        _this.pageNull = false;
                        _this.ajaxLoading = true;
                    },
                    complete: function () {
                        _this.ajaxLoading = false;
                        _this.nextPageLoading = false;
                    },
                    error: function () {
                        _this.resFailMessage = '网络请求失败.';
                        $(_this.$refs.failToast).fadeIn(100);
                        setTimeout(function () {
                            $(_this.$refs.failToast).fadeOut(200);
                            _this.resFailMessage = null;
                        }, 1800);
                    },
                    success: function (res) {
                        console.log(res);
                        if (res.status) {
                            if (res.data.length === 0) {
                                // 再次加载无数据
                                _this.noNetxPage = true;
                                setTimeout(() => {
                                    if (mySwiper) {
                                        mySwiper.update();
                                    }
                                });
                            }
                            else {
                                _this.pageConfig.page = res.nextPage;
                                _this.noNetxPage = !res.existNextPage;
                                $.each(res.data, function (index, item) { _this.items.push(item); });
                                setTimeout(() => {
                                    if (mySwiper) {
                                        mySwiper.update();
                                    }
                                });
                            }
                        }
                        else {
                            _this.dialogContent = res.message;
                            $(_this.$refs.iosDialog).fadeIn(200);
                        }
                    }
                });
            },
            closeDialog() {
                $(this.$refs.iosDialog).fadeOut(200);
                this.dialogContent = null;
            }
        }
    });