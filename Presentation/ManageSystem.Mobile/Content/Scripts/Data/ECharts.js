var mySwiper;
app = new Vue({
    el: '#app',
    data: {
        HeatMap: {
            Show: true,
            PageImgSrc: '/Content/File/HeatMap/碳青霉烯类耐药肺炎克雷伯菌/2017.jpg'
        },
        GermBar: {
            Show: false,
            PageContentTitle: null
        },
        GermYear: {
            Show: false,
            PageContentTitle: null
        },

        GermBarEchartObj: null,
        GermYearEchartObj: null,
        HeatMapSwiperObj: null,
        TabSwiperObj: null,
        SwiperObj: null,
        weui_toast_content: null
    },
    created: function () {
        this.$nextTick(() => {
            $('.map-container').height(this.$refs.middle.offsetHeight - 30);
            this.initPhotoSwipeFromDOM('.my-gallery');
            this.initHeatMapSwiper(this.$refs.HeatMapSwiperContainer);
        });
    },
    mounted: function () {
    },
    methods: {
        /* 热图 */
        initHeatMapSwiper(swiperContainer) {
            _this = this;
            swiperContainer.style.height = _this.$refs.middle.offsetHeight + 'px';
            setTimeout(() => {
                _this.HeatMapSwiperObj = new Swiper(swiperContainer, {
                    loop: false,
                    observer: true,
                    observeSlideChildren: true,
                    watchSlidesProgress: true,
                    slidesPerView: 'auto',
                    on: {
                        slideChange() {
                            $('[id="HeatMapActionsheet"].weui-skin_android').find('li.active').removeClass('active');
                            $('#HeatMapActionsheet li[data-slide="' + _this.HeatMapSwiperObj.activeIndex + '"]').addClass('active');
                        }
                    }
                });
            });
        },
        layerHeatMap() {
            $HeatMapActionsheet = $(this.$refs.HeatMapActionsheet);
            $HeatMapActionsheet.fadeIn(200);
            $HeatMapActionsheet.find('.weui-mask').tap(() => {
                $HeatMapActionsheet.fadeOut(200);
            });
            var maskHeight = $HeatMapActionsheet.find('.weui-mask')[0].offsetHeight,
                swiperSlideHeight = $HeatMapActionsheet.find('.swiper-slide')[0].offsetHeight;

            if (swiperSlideHeight >= maskHeight * 0.95) {
                this.$refs.HeatMapActionsheetSwiper.style.height = $HeatMapActionsheet.find('.weui-mask')[0].offsetHeight * 0.95 + 'px';
            }

            new Swiper(this.$refs.HeatMapActionsheetSwiper, {
                direction: 'vertical',
                loop: false,
                watchSlidesProgress: true,
                freeMode: true,
                slidesPerView: 'auto'
            });
            //$androidActionSheet.fadeIn(200);
            //$androidMask.on('click', function () {
            //    $androidActionSheet.fadeOut(200);
            //});
        },
        onSetHeatMap(index) {
            var _this = this;
            $HeatMapActionsheet = $(_this.$refs.HeatMapActionsheet);
            $('#HeatMapActionsheet.weui-skin_android').find('li.active').removeClass('active');
            $(event.target).addClass('active');
            //this.HeatMap.PageImgSrc = event.target.dataset.url;
            $('.map-container').height(_this.$refs.middle.offsetHeight - 30);
            $HeatMapActionsheet.fadeOut(50);
            _this.HeatMapSwiperObj.slideTo(index);
            _this.HeatMap.Show = true;
            _this.GermBar.Show = false;
            _this.GermYear.Show = false;
            setTimeout(() => {
                $('#heatmap-container').css('display', 'block');
                $('#germbar-container').css('display', 'none');
                $('#germyear-container').css('display', 'none');
            });
        },
        initPhotoSwipeFromDOM(gallerySelector) {
            // 解析来自DOM元素幻灯片数据（URL，标题，大小...）
            var parseThumbnailElements = function (el) {
                var thumbElements = el.childNodes,
                    numNodes = thumbElements.length,
                    items = [],
                    figureEl,
                    linkEl,
                    size,
                    item,
                    divEl;

                for (var i = 0; i < numNodes; i++) {
                    figureEl = thumbElements[i]; // <figure> element
                    // 仅包括元素节点
                    if (figureEl.nodeType !== 1) {
                        continue;
                    }
                    divEl = figureEl.children[0];
                    linkEl = divEl.children[0]; // <a> element
                    size = linkEl.getAttribute('data-size').split('x');
                    // 创建幻灯片对象
                    item = {
                        src: linkEl.getAttribute('href'),
                        w: parseInt(size[0], 10),
                        h: parseInt(size[1], 10)
                    };
                    if (figureEl.children.length > 1) {
                        item.title = figureEl.children[1].innerHTML;
                    }
                    if (linkEl.children.length > 0) {
                        // <img> 缩略图节点, 检索缩略图网址
                        item.msrc = linkEl.children[0].getAttribute('src');
                    }
                    item.el = figureEl; // 保存链接元素 for getThumbBoundsFn
                    items.push(item);
                }
                return items;
            };

            // 查找最近的父节点
            var closest = function closest(el, fn) {
                return el && (fn(el) ? el : closest(el.parentNode, fn));
            };

            // 当用户点击缩略图触发
            var onThumbnailsClick = function (e) {
                e = e || window.event;
                e.preventDefault ? e.preventDefault() : e.returnValue = false;
                var eTarget = e.target || e.srcElement;
                var clickedListItem = closest(eTarget, function (el) {
                    return el.tagName && el.tagName.toUpperCase() === 'FIGURE';
                });
                if (!clickedListItem) {
                    return;
                }
                var clickedGallery = clickedListItem.parentNode,
                    childNodes = clickedListItem.parentNode.childNodes,
                    numChildNodes = childNodes.length,
                    nodeIndex = 0,
                    index;
                for (var i = 0; i < numChildNodes; i++) {
                    if (childNodes[i].nodeType !== 1) {
                        continue;
                    }
                    if (childNodes[i] === clickedListItem) {
                        index = nodeIndex;
                        break;
                    }
                    nodeIndex++;
                }
                if (index >= 0) {
                    openPhotoSwipe(index, clickedGallery);
                }
                return false;
            };

            var photoswipeParseHash = function () {
                var hash = window.location.hash.substring(1),
                    params = {};
                if (hash.length < 5) {
                    return params;
                }
                var vars = hash.split('&');
                for (var i = 0; i < vars.length; i++) {
                    if (!vars[i]) {
                        continue;
                    }
                    var pair = vars[i].split('=');
                    if (pair.length < 2) {
                        continue;
                    }
                    params[pair[0]] = pair[1];
                }
                if (params.gid) {
                    params.gid = parseInt(params.gid, 10);
                }
                return params;
            };

            var openPhotoSwipe = function (index, galleryElement, disableAnimation, fromURL) {
                var pswpElement = document.querySelectorAll('.pswp')[0],
                    gallery,
                    options,
                    items;
                items = parseThumbnailElements(galleryElement);
                // 这里可以定义参数
                options = {
                    barsSize: {
                        top: 100,
                        bottom: 100
                    },
                    fullscreenEl: false,
                    shareButtons: [
                        { id: 'wechat', label: '分享微信', url: '#' },
                        { id: 'weibo', label: '新浪微博', url: '#' },
                        { id: 'download', label: '保存图片', url: '{{raw_image_url}}', download: true }
                    ],
                    galleryUID: galleryElement.getAttribute('data-pswp-uid'),
                    getThumbBoundsFn: function (index) {
                        var thumbnail = items[index].el.getElementsByTagName('img')[0], // find thumbnail
                            pageYScroll = window.pageYOffset || document.documentElement.scrollTop,
                            rect = thumbnail.getBoundingClientRect();
                        return { x: rect.left, y: rect.top + pageYScroll, w: rect.width };
                    }
                };
                if (fromURL) {
                    if (options.galleryPIDs) {
                        for (var j = 0; j < items.length; j++) {
                            if (items[j].pid === index) {
                                options.index = j;
                                break;
                            }
                        }
                    } else {
                        options.index = parseInt(index, 10) - 1;
                    }
                } else {
                    options.index = parseInt(index, 10);
                }
                if (isNaN(options.index)) {
                    return;
                }
                if (disableAnimation) {
                    options.showAnimationDuration = 0;
                }
                gallery = new PhotoSwipe(pswpElement, PhotoSwipeUI_Default, items, options);
                gallery.init();
            };

            var galleryElements = document.querySelectorAll(gallerySelector);
            for (var i = 0, l = galleryElements.length; i < l; i++) {
                galleryElements[i].setAttribute('data-pswp-uid', i + 1);
                galleryElements[i].onclick = onThumbnailsClick;
            }
            var hashData = photoswipeParseHash();
            if (hashData.pid && hashData.gid) {
                openPhotoSwipe(hashData.pid, galleryElements[hashData.gid - 1], true, true);
            }
        },

        /* 柱状图 */
        layerGermBar() {
            $GermBarActionsheet = $(this.$refs.GermBarActionsheet);
            $GermBarActionsheet.fadeIn(20);
            $GermBarActionsheet.find('.weui-mask').tap(() => { $GermBarActionsheet.fadeOut(200); });

            var maskHeight = $GermBarActionsheet.find('.weui-mask')[0].offsetHeight,
                swiperSlideHeight = $GermBarActionsheet.find('.swiper-slide')[0].offsetHeight;

            if (swiperSlideHeight >= maskHeight * 0.95) {
                this.$refs.GermBarActionsheetSwiper.style.height = $GermBarActionsheet.find('.weui-mask')[0].offsetHeight * 0.95 + 'px';
            }

            new Swiper(this.$refs.GermBarActionsheetSwiper, {
                direction: 'vertical',
                loop: false,
                watchSlidesProgress: true,
                freeMode: true,
                slidesPerView: 'auto'
            });
        },
        onSelectGermClass(event) {
            this.HeatMap.Show = false;
            this.GermBar.Show = true;
            this.GermYear.Show = false;

            $GermBarActionsheet = $(this.$refs.GermBarActionsheet);
            $('[id="GermBarActionsheet"].weui-skin_android').find('li.active').removeClass('active');
            $(event.target).addClass('active');

            setTimeout(() => {
                $GermBarActionsheet.fadeOut(20);
                this.loadingGermBarData(event.target.dataset.name, '');
            });
        },
        onSelectAntibiotic(event) {
            this.HeatMap.Show = false;
            this.GermBar.Show = true;
            this.GermYear.Show = false;

            $GermBarActionsheet = $(this.$refs.GermBarActionsheet);
            $('[id="GermBarActionsheet"].weui-skin_android').find('li.active').removeClass('active');
            $(event.target).addClass('active');
            setTimeout(() => {
                $GermBarActionsheet.fadeOut(20);
                this.loadingGermBarData('', event.target.dataset.name);
            });
        },
        onSelectMainGerm(event) {
            this.HeatMap.Show = false;
            this.GermBar.Show = true;
            this.GermYear.Show = false;

            $GermBarActionsheet = $(this.$refs.GermBarActionsheet);
            $('[id="GermBarActionsheet"].weui-skin_android').find('li.active').removeClass('active');
            $(event.target).addClass('active');
            this.loadingMainGermData(event.target.dataset.code);
        },
        onSelectHospitalGerm(event) {
            this.HeatMap.Show = false;
            this.GermBar.Show = true;
            this.GermYear.Show = false;

            $GermBarActionsheet = $(this.$refs.GermBarActionsheet);
            $('[id="GermBarActionsheet"].weui-skin_android').find('li.active').removeClass('active');
            $(event.target).addClass('active');
            this.loadingHospitalGermData(event.target.dataset.code);
        },
        loadingGermBarData(germValue, antibioticValue) {
            _this = this;
            _this.GermBar.PageContentTitle = null;
            var xRotate = 45;

            if (germValue !== '') {

                var germCount = this.loadingGermCount(germValue);
                if (germCount !== '')
                    germCount += '株';

                //选择了细菌分类
                _this.GermBar.PageContentTitle = germCount + germValue + '对抗菌药物的耐药率';

                if (germValue === '克雷伯菌属' || germValue === '柠檬酸杆菌属' || germValue === '大肠埃希菌')
                    xRotate = 45;

            }
            else if (antibioticValue !== '') {
                //选择了抗菌药物
                _this.GermBar.PageContentTitle = '不同细菌对' + antibioticValue + '的耐药率';

            }
            else {
                //什么都没有选择
                //$('#dataError').css('display', 'none');
                //$('.main-content2').css('display', 'block');
            }

            $.ajax({
                type: "POST",
                url: "/Data/GetAntibioticDrugFastData",
                data: {
                    GermValue: germValue,
                    AntibioticValue: antibioticValue
                },
                dataType: "json",
                beforeSend: function () { $('#loadingToast').fadeIn(100); },
                success: function (res) {
                    if (res === null || res === '') {
                        _this.weui_toast_content = '获取数据失败';
                        $('#loadingToast').fadeOut(20);
                        $('#toast').fadeIn(200);
                        setTimeout(function () {
                            $('#toast').fadeOut(200);
                        }, 1200);
                        return;
                    }

                    if (res.Status) {
                        var text = res.Text.split(",");
                        var value = res.Value.split(",");
                        _this.loadingGermBarEchart(text, value, xRotate, 30);

                    }
                    else {
                        text = [];    //类别数组（实际用来盛放X轴坐标值）
                        value = [];    //销量数组（实际用来盛放Y坐标值）
                        _this.loadingGermBarEchart(text, value);
                        _this.weui_toast_content = res.Message;
                        $('#loadingToast').fadeOut(20);
                        $('#toast').fadeIn(200);
                        setTimeout(function () {
                            $('#toast').fadeOut(200);
                        }, 1200);
                    }
                }
            });
        },
        loadingGermCount(germValue) {
            switch (germValue) {
                case '大肠埃希菌': return '36735';
                case '克雷伯菌属': return '27977';
                case '变形杆菌属': return '3646';
                case '肠杆菌属': return '7491';
                case '沙雷菌属': return '2330';
                case '柠檬酸杆菌属': return '1689';
                case '摩根菌属': return '943';
                case '铜绿假单胞菌': return '16562';
                case '不动杆菌属': return '19246';
                case 'MSSA': return '11120';
                case 'MRSA': return '6084';
                case 'MSCNS': return '1586';
                case 'MRCNS': return '6717';
                case '粪肠球菌': return '6693';
                case '屎肠球菌': return '8173';
                //case '肠杆菌属': return '82754'; // 重复
                case '嗜麦芽窄食单胞菌': return '5471';
                case '伯克霍尔德菌属': return '1804';
                case '流感嗜血杆菌': return '5070';
                case '流感嗜血杆菌(儿童)': return '3161';
                case '流感嗜血杆菌(成人)': return '1898';
                case 'PSSP(儿童)': return '2787';
                case 'PISP(儿童)': return '355';
                case 'PRSP(儿童)': return '70';
                case 'PSSP(成人)': return '1288';
                case 'PISP(成人)': return '46';
                case 'PRSP(成人)': return '26';
                default: return '';
            }
        },
        loadingMainGermData(code) {
            var text = [];
            var value = [];
            var title = '';

            switch (code) {

                case '100':
                    title = '190610株临床分离菌株主要菌种分布';
                    text = ["摩根菌属", "沙门菌属", "卡他莫拉菌", "柠檬酸杆菌属", "伯克霍尔德菌属", "草绿色链球菌", "沙雷菌属", "变形杆菌属", "肺炎链球菌", "流感嗜血杆菌", "嗜麦芽窄食单胞菌", "B溶血性链球菌", "肠杆菌属", "凝固酶阴性葡萄球菌", "肠球菌属", "铜绿假单胞菌", "金葡菌", "不动杆菌属", "克雷伯菌属", "大肠埃希菌"];
                    value = [0.49, 0.65, 0.81, 0.89, 1.01, 1.04, 1.22, 1.91, 2.41, 2.66, 2.87, 3.57, 3.93, 4.39, 8.42, 8.69, 9.03, 10.1, 14.68, 19.27];
                    break;
                case '101':
                    title = '76333株呼吸道标本分离菌主要菌种分布';
                    text = ["琼氏不动杆菌", "奇异变形杆菌", "产酸克雷伯菌", "产气肠杆菌", "洋葱伯克霍尔德菌", "黏质沙雷菌", "嗜麦芽窄食单胞菌", "卡他莫拉菌", "阴沟肠杆菌", "化脓链球菌", "大肠埃希菌", "肺炎链球菌", "嗜麦芽窄食单胞菌", "流感嗜血杆菌", "金黄色葡萄球菌", "铜绿假单胞菌", "鲍曼不动杆菌", "肺炎克雷伯菌"];
                    value = [0.3, 0.8, 1, 1.2, 1.6, 1.6, 1.6, 1.9, 3, 3.6, 5, 5.1, 5.8, 6.1, 9.1, 14.5, 17.3, 17.6];
                    break;
                case '102':
                    title = '36635株尿道标本分离菌主要菌种分布';
                    text = ["洋葱伯克霍尔德菌", "恶臭假单胞菌", "嗜麦芽窄食单胞菌", "产气肠杆菌", "产酸克雷伯菌", "摩根摩根菌", "弗劳地柠檬酸杆菌", "金黄色葡萄球菌", "鲍曼不动杆菌", "阴沟肠杆菌", "无乳链球菌", "奇异变形杆菌", "铜绿假单胞菌", "粪肠球菌", "肺炎克雷伯菌", "屎肠球菌", "大肠埃希菌"];
                    value = [0.4, 0.6, 0.7, 0.8, 0.9, 0.9, 1, 1.4, 2.1, 2.2, 2.7, 3.5, 3.7, 8.6, 9.8, 10.6, 46.4];
                    break;
                case '103':
                    title = '29000株血液标本分离菌主要菌种分布';
                    text = ["奇异变形杆菌", "咽峡炎链球菌", "缓症链球菌", "嗜麦芽窄食单胞菌", "黏质沙雷菌", "洋葱伯克霍尔德菌", "肺炎链球菌", "溶血葡萄球菌", "阴沟肠杆菌", "粪肠球菌", "铜绿假单胞菌", "鲍曼不动杆菌", "屎肠球菌", "人葡萄球菌", "金黄色葡萄球菌", "表皮葡萄球菌", "肺炎克雷伯菌", "大肠埃希菌"];
                    value = [0.6, 0.7, 0.7, 0.8, 1, 1.1, 1.3, 2.4, 2.4, 2.7, 3.3, 3.9, 4.4, 7, 7.9, 10.2, 15.3, 22.2];
                    break;
                case '104':
                    title = '22320株伤口脓液标本分离菌主要菌种分布';
                    text = ["鸟肠球菌", "肺炎链球菌", "产气肠杆菌", "肺炎链球菌", "弗劳地柠檬酸杆菌", "产酸克雷伯菌", "嗜麦芽窄食单胞菌", "摩根摩根菌", "产酸克雷伯菌", "化脓链球菌", "黏质沙雷菌", "奇异变形杆菌", "屎肠球菌", "粪肠球菌", "阴沟肠杆菌", "鲍曼不动杆菌", "无乳链球菌", "铜绿假单胞菌", "肺炎克雷伯菌", "大肠埃希菌", "金黄色葡萄球菌"];
                    value = [0.6, 0.7, 0.7, 0.7, 0.8, 0.8, 0.9, 1, 1, 1.1, 1.2, 2.4, 2.8, 3.7, 3.9, 4.3, 4.6, 8.7, 9.1, 20.6, 25.3];
                    break;
                case '105':
                    title = '8331株其他无菌体液标本分离菌主要菌种分布';
                    text = ["星座链球菌", "草绿色链球菌", "鸟肠球菌", "摩根摩根菌", "缓症链球菌", "弗劳地柠檬酸杆菌", "产酸克雷伯菌", "铅黄肠球菌", "产气肠杆菌", "人葡萄球菌", "咽峡炎链球菌", "奇异变形杆菌", "嗜麦芽窄食单胞菌", "溶血葡萄球菌", "金黄色葡萄球菌", "阴沟肠杆菌", "表皮葡萄球菌", "鲍曼不动杆菌", "铜绿假单胞菌", "粪肠球菌", "屎肠球菌", "肺炎克雷伯菌", "大肠埃希菌"];
                    value = [0.8, 0.8, 0.9, 0.9, 0.9, 1, 1, 1.1, 1.1, 1.1, 1.2, 1.2, 2, 2.1, 3.4, 3.5, 4.4, 5, 5.3, 7.9, 10.6, 10.9, 21];
                    break;
                case '106':
                    title = '2708株脑脊液标本分离菌主要菌种分布';
                    text = ["克氏葡萄球菌", "琼氏不动杆菌", "弗劳地柠檬酸杆菌", "无乳链球菌", "黏质沙雷菌", "科氏葡萄球菌", "洛菲不动杆菌", "嗜麦芽窄食单胞菌", "阴沟肠杆菌", "粪肠球菌", "铜绿假单胞菌", "沃氏葡萄球菌", "大肠埃希菌", "肺炎链球菌", "金黄色葡萄球菌", "屎肠球菌", "溶血葡萄球菌", "肺炎克雷伯菌", "人葡萄球菌", "鲍曼不动杆菌", "表皮葡萄球菌"];
                    value = [0.5, 0.6, 0.6, 0.7, 0.8, 0.9, 1.1, 1.1, 1.3, 1.4, 2.3, 2.8, 3.1, 3.1, 3.5, 4.4, 5.5, 8.1, 8.6, 16.1, 21.3];
                    break;
                default:
            }

            this.GermBar.PageContentTitle = title;

            setTimeout(() => {
                this.loadingGermBarEchart(text, value, 45, 30);
                $(this.$refs.GermBarActionsheet).fadeOut(200);
            });
        },
        loadingHospitalGermData(code) {
            var text = [];
            var value = [];
            var title = '';

            switch (code) {
                case '200':
                    title = '各医院金葡菌MR菌株检出率';
                    text = ['sxe', 'hyd', 'nmg', 'ZGH', 'SYH', 'TJJ', 'XJH', 'SZS', 'JJH', 'JLH', 'PUH', 'nxz', 'SCS', 'XYH', 'bch', 'KMH', 'HXH', 'SDH', 'PDH', 'SYF', 'PED', 'JXE', 'bjh', 'SCH', 'GLH', 'RJH', 'GSY', 'ayd', 'LSH', 'GZH', 'HNH', 'HSH', 'TJH', 'SXS'];
                    value = [10.3, 16.2, 19, 19.1, 21.8, 22.1, 22.6, 23.6, 24.9, 25.1, 25.7, 26.3, 27.1, 27.3, 27.7, 28, 28.2, 29.2, 35, 36.5, 36.9, 41.3, 47, 47.1, 47.9, 48.8, 49.1, 51, 51.7, 52, 53.3, 54, 59.9, 62.1];
                    break;
                case '201':
                    title = '各医院凝固酶阴性葡萄球菌MR菌株检出率';
                    text = ['PDH', 'JXE', 'XJH', 'LSH', 'SZS', 'PUH', 'hyd', 'SYF', 'sxe', 'GLH', 'SCS', 'HSH', 'XYH', 'JLH', 'SDH', 'GSY', 'HXH', 'SXS', 'nmg', 'ayd', 'PED', 'bch', 'KMH', 'GZH', 'SCH', 'TJH', 'nxz', 'SYH', 'TJJ', 'bjh', 'RJH', 'HNH', 'ZGH', 'JJH'];
                    value = [49.3, 66.7, 68.5, 69.6, 72.1, 73.6, 74, 75, 75.5, 77.7, 78.4, 78.8, 79.1, 80.1, 80.4, 80.6, 81.3, 82, 82.2, 82.5, 82.6, 83, 83.3, 83.5, 83.9, 84, 84.6, 84.8, 85.2, 85.8, 86.8, 88.7, 100, 100];
                    break;
                case '202':
                    title = '各医院分离铜绿假单胞菌对亚胺培南的耐药率（68-1025株）';
                    text = ['ZGH', 'JXE', 'NXZ', 'PDH', 'PUH', 'XJH', 'JLH', 'HYD', 'GSY', 'NMG', 'AYD', 'GZH', 'HXH', 'SZS', 'LSH', 'GLH', 'SCH', 'SCS', 'BCH', 'SXE', 'TJJ', 'XYH', 'JJH', 'PED', 'SDH', 'TJH', 'SYH', 'HNH', 'RJH', 'SXS', 'BJH', 'HSH', 'SYF', 'KMH'];
                    value = [1.7, 11.8, 11.8, 13, 14.8, 15.7, 16.3, 19.8, 20.4, 20.4, 20.7, 22.1, 22.4, 22.9, 24.1, 24.3, 24.7, 24.9, 25.2, 25.2, 28, 29.8, 30.1, 31, 31.2, 31.6, 33.3, 35.8, 35.8, 40.9, 41.2, 41.2, 43.9, 45.2];
                    break;
                case '203':
                    title = '各医院分离鲍曼不动杆菌对亚胺培南的耐药率（19-1299株）';
                    text = ['LSH', 'JXE', 'ZGH', 'SXE', 'PDH', 'JLH', 'BCH', 'TJJ', 'PED', 'NMG', 'BJH', 'XJH', 'RJH', 'GZH', 'JJH', 'SDH', 'GLH', 'PUH', 'AYD', 'SCH', 'HXH', 'SCS', 'SZS', 'GSY', 'NXZ', 'HSH', 'SYF', 'HYD', 'KMH', 'SYH', 'XYH', 'SXS', 'HNH', 'TJH'];
                    value = [3.8, 21.1, 26.5, 34.1, 37.9, 42.7, 43, 46.9, 50.4, 50.7, 51.5, 54, 55.4, 57, 62.7, 64, 64.4, 71.7, 72.4, 72.6, 72.7, 73.2, 74.2, 74.3, 74.9, 76.7, 78.9, 80.6, 81.2, 83.6, 86.4, 87, 91.3, 91.4];
                    break;
                case '204':
                    title = '各医院分离肺炎克雷伯菌对亚胺培南的耐药率（66-1838株）';
                    text = ['JJH', 'ZGH', 'LSH', 'GSY', 'JLH', 'SXE', 'XJH', 'NXZ', 'NMG', 'PDH', 'GZH', 'SDH', 'HYD', 'TJJ', 'PUH', 'GLH', 'SZS', 'HXH', 'SCS', 'SYH', 'AYD', 'KMH', 'RJH', 'XYH', 'PED', 'BJH', 'SXS', 'SCH', 'BCH', 'SYF', 'TJH', 'JXE', 'HSH', 'HNH'];
                    value = [0, 0.6, 1.7, 2.1, 2.1, 2.5, 3.2, 3.5, 5, 5.5, 5.6, 6.4, 7.3, 7.4, 9.3, 10.4, 15.1, 15.2, 15.8, 19.7, 26.3, 26.3, 27.6, 29.1, 32.1, 33.4, 36.4, 38.7, 40, 42.5, 42.9, 45.5, 52.7, 53.1];
                    break;
                default:
            }

            this.GermBar.PageContentTitle = title;
            setTimeout(() => {
                this.loadingGermBarEchart(text, value, 45, 20);
                $(this.$refs.GermBarActionsheet).fadeOut(200);
            });
        },
        loadingGermBarEchart(text, value, xRotate, barWidth) {
            var _this = this;

            // 重置图表高度
            if (text.length >= 17) {
                _this.$refs.GermBarChart.style.height = '900px';
            }
            else if (text.length === 16) {
                _this.$refs.GermBarChart.style.height = '720px';
            }
            else if (text.length === 15) {
                _this.$refs.GermBarChart.style.height = '690px';
            }
            else if (text.length === 14) {
                _this.$refs.GermBarChart.style.height = '660px';
            }
            else if (text.length === 13) {
                _this.$refs.GermBarChart.style.height = '630px';
            }
            else if (text.length === 12) {
                _this.$refs.GermBarChart.style.height = '600px';
            }
            else if (text.length === 11) {
                _this.$refs.GermBarChart.style.height = '570px';
            }
            else if (text.length === 10) {
                _this.$refs.GermBarChart.style.height = '540px';
            }
            else if (text.length === 9) {
                _this.$refs.GermBarChart.style.height = '510px';
            }
            else if (text.length === 8) {
                _this.$refs.GermBarChart.style.height = '480px';
            }
            else if (text.length === 7) {
                _this.$refs.GermBarChart.style.height = '450px';
            }
            else if (text.length === 6) {
                _this.$refs.GermBarChart.style.height = '420px';
            }
            else if (text.length === 5) {
                _this.$refs.GermBarChart.style.height = '380px';
            }
            else if (text.length === 4) {
                _this.$refs.GermBarChart.style.height = '350px';
            }
            else if (text.length === 3) {
                _this.$refs.GermBarChart.style.height = '320px';
            }
            else {
                _this.$refs.GermBarChart.style.height = '480px';
            }

            // 基于准备好的dom，初始化echarts实例
            if (_this.GermBarEchartObj) {

                echarts.dispose(_this.GermBarEchartObj);
            }

            _this.GermBarEchartObj = echarts.init(_this.$refs.GermBarChart),
                option = {
                    calculable: true,
                    color: ['#015BAA'],
                    title: {
                        text: _this.GermBar.PageContentTitle,
                        subtext: '',
                        left: 'center',
                        show: false
                    },
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: { // 坐标轴指示器，坐标轴触发有效
                            type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
                        }
                    },
                    grid: {
                        top: '1%',
                        left: '5%',
                        right: '8%',
                        containLabel: true
                    },
                    xAxis: [
                        {
                            position: 'top',
                            type: 'value',
                            axisLabel: {
                                show: true,
                                interval: 'auto',
                                formatter: '{value} %',
                                textStyle: {
                                    fontSize: "12px"
                                }
                            },
                            show: true
                        }
                    ],
                    yAxis: [
                        {
                            type: 'category',
                            axisLabel: {
                                //inside: true,
                                interval: 0,
                                rotate: 0,
                                //margin: 10,
                                textStyle: {
                                    fontSize: "14px"
                                }
                            },
                            data: text,  //'替考拉宁', '万古霉素', '利奈唑胺', '利福平', '庆大霉素', '左氧氟沙星', '环丙沙星', '克林霉素', '复方磺胺甲恶唑', '红霉素', '青霉素', '苯唑西林'
                            axisTick: {
                                alignWithLabel: true
                            }
                        }
                    ],
                    //toolbox: {
                    //    show: false,
                    //    orient: 'vertical',
                    //    left: 'right',
                    //    top: 'center',
                    //    feature: {
                    //        dataView: {
                    //            title: "数据",
                    //            readOnly: false
                    //        },
                    //        restore: {},
                    //        saveAsImage: {
                    //            title: "下载",
                    //            type: "jpeg"
                    //        }
                    //    }
                    //},
                    series: [
                        {
                            name: '数值',
                            type: 'bar',
                            barWidth: barWidth,//柱图宽度
                            data: value,//0, 0, 0, 12.7, 31.2, 56.7, 55.7, 42.9, 54.7, 85.7, 100, 100
                            //以下为是否显示，显示位置和显示格式的设置了
                            label: {
                                normal: {
                                    show: true,
                                    position: 'right',
                                    formatter: '{c}'
                                }
                            }
                            //itemStyle: {
                            //    normal: {

                            //        //以下为是否显示，显示位置和显示格式的设置了
                            //        label: {
                            //            show: true,
                            //            position: 'top',
                            //            // formatter: '{c}'
                            //            formatter: '{c}'
                            //        }
                            //    }
                            //},
                        }
                    ],
                    textStyle:
                    {
                        fontSize: 10
                    }
                };

            // 使用刚指定的配置项和数据显示图表。
            _this.GermBarEchartObj.setOption(option);

            setTimeout(() => {
                try {
                    $('#loadingToast').fadeOut(20);
                    _this.loadingSwiper(_this.$refs.GermBarSwiperContainer);
                    setTimeout(() => {
                        $('#heatmap-container').css('display', 'none');
                        $('#germbar-container').css('display', 'block');
                        $('#germyear-container').css('display', 'none');
                    });
                } catch (e) {
                    console.log(e);
                }
            }, 800);
        },

        /* 趋势图 */
        layerGermYear() {
            $GermYearActionsheet = $(this.$refs.GermYearActionsheet);
            $GermYearActionsheet.fadeIn(20);
            $GermYearActionsheet.find('.weui-mask').tap(() => {
                $GermYearActionsheet.fadeOut(200);
            });

            var maskHeight = $GermYearActionsheet.find('.weui-mask')[0].offsetHeight,
                swiperSlideHeight = $GermYearActionsheet.find('.swiper-slide')[0].offsetHeight;

            if (swiperSlideHeight >= maskHeight * 0.9) {
                this.$refs.GermYearActionsheetSwiper.style.height = $GermYearActionsheet.find('.weui-mask')[0].offsetHeight * 0.9 + 'px';
            }

            new Swiper(this.$refs.GermYearActionsheetSwiper, {
                direction: 'vertical',
                loop: false,
                watchSlidesProgress: true,
                freeMode: true,
                slidesPerView: 'auto'
            });
        },
        onSetGermYear(event) {
            this.HeatMap.Show = false;
            this.GermBar.Show = false;
            this.GermYear.Show = true;

            $GermYearActionsheet = $(this.$refs.GermYearActionsheet);
            $('[id="GermYearActionsheet"].weui-skin_android').find('li.active').removeClass('active');
            $(event.target).addClass('active');
            this.GermYear.PageContentTitle = event.target.dataset.name;

            setTimeout(() => {
                this.loadingGermYearItem();
            });
        },
        loadingGermYearItem() {
            var _this = this;
            var legend = [];
            var text = ["2005年", "2006年", "2007年", "2008年", "2009年", "2010年", "2011年", "2012年", "2013年", "2014年", "2015年", "2016年", "2017年", "2018年"];
            var value = [];

            switch (_this.GermYear.PageContentTitle) {
                case '大肠埃希菌对碳青霉烯类耐药变迁':
                    {
                        legend = ['亚胺培南', '美罗培南'];
                        value = [
                            { name: '亚胺培南', type: 'bar', stack: '', data: [1.1, 1.4, 0.7, 1.2, 1.7, 1.6, 1, 0.9, 1, 0.9, 1.4, 1.3, 1.9, 1.5], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '美罗培南', type: 'bar', stack: '', data: [1.4, 0.8, 0.8, 0.9, 1, 1.4, 1.2, 1, 3, 1, 1.6, 1.8, 2.3, 1.5], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case '肺炎克雷伯菌对碳青霉烯类耐药变迁':
                    {
                        legend = ['亚胺培南', '美罗培南'];
                        value = [
                            { name: '亚胺培南', type: 'bar', stack: '', data: [3, 3.4, 2.4, 4, 4.9, 9.2, 9, 10, 10.3, 11, 15.6, 16.1, 20.9, 26.1], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '美罗培南', type: 'bar', stack: '', data: [2.9, 2.6, 2.9, 3.8, 4.8, 9.2, 9, 11.8, 14.1, 14.1, 14.4, 18.8, 24, 28.6], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case '铜绿假单细胞菌对碳青霉烯类耐药变迁':
                    {
                        legend = ['亚胺培南', '美罗培南'];
                        value = [
                            { name: '亚胺培南', type: 'bar', stack: '', data: [32.5, 35.1, 35.8, 30.5, 30.5, 30.8, 29.1, 29.1, 27.1, 29.1, 27.6, 28.7, 23.6], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '美罗培南', type: 'bar', stack: '', data: [31.6, 26.4, 28.5, 24.5, 25.2, 25.8, 25, 27.1, 25.1, 26.1, 23.4, 25.3, 20.9], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case '鲍曼不动杆菌对碳青霉烯类耐药变迁':
                    {
                        legend = ['亚胺培南', '美罗培南'];
                        value = [
                            { name: '亚胺培南', type: 'bar', stack: '', data: [31, 30.1, 35.3, 48.1, 50, 57.1, 60.4, 56.8, 62.8, 62.4, 65.7, 69.7, 66.7], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '美罗培南', type: 'bar', stack: '', data: [39, 40.9, 39.9, 49.3, 52.4, 58.3, 61.4, 61.4, 59.4, 66.7, 72.9, 72.9, 69.3], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case 'CHINET监测历年MRSA和MRCNS检出变迁':
                    {
                        legend = ['MRSA', 'MRCNS'];
                        value = [
                            { name: 'MRSA', type: 'bar', stack: '', data: [69, 58.4, 58, 55.9, 52.7, 51.7, 50.6, 47.9, 45.2, 44.6, 42.2, 38.4, 35.3], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: 'MRCNS', type: 'bar', stack: '', data: [82, 76.3, 77, 75.9, 71.7, 71.6, 74.6, 77.1, 73.5, 83, 82.6, 77.6, 80.3], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case 'MRSA检出率(成人和儿童)':
                    {
                        legend = ['成人', '儿童'];
                        value = [
                            { name: '成人', type: 'bar', stack: '', data: [85.8, 69.3, 67.8, 66.5, 62.2, 61.7, 57.8, 54.4, 49.7, 47.6, 45.8, 45.8, 37.4], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '儿童', type: 'bar', stack: '', data: [18, 17.2, 24.6, 20.5, 21.4, 24.6, 26.4, 25.7, 30.4, 33.4, 31.7, 31.7, 29.4], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case '粪肠球菌和屎肠球菌对万古霉素耐药变迁':
                    {
                        legend = ['粪肠球菌', '屎肠球菌'];
                        value = [
                            { name: '粪肠球菌', type: 'bar', stack: '', data: [0, 0, 0.49, 0.39, 0.28, 0.55, 0.1, 0.26, 0.03, 0.19, 0.2, 0.4, 0.1], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '屎肠球菌', type: 'bar', stack: '', data: [0.35, 1.06, 2.06, 3.21, 3.49, 3.52, 2.6, 2.49, 2.12, 3.5, 2.4, 1.9, 1.4], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case 'CHINET监测主要标本所占比例变迁':
                    {
                        legend = ['呼吸道分泌物', '尿液', '血液', '伤口脓液'];
                        value = [
                            { name: '呼吸道分泌物', type: 'bar', stack: '', data: [45, 50.1, 50, 49, 49.7, 46.9, 45.8, 44.4, 43.2, 41.6, 42.8, 41.6, 40], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '尿液', type: 'bar', stack: '', data: [18, 17.9, 19.6, 19.8, 19.9, 19.9, 22.6, 21.4, 20.9, 22.4, 22.1, 19.1, 19.2], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '血液', type: 'bar', stack: '', data: [9, 9.1, 11, 11.1, 10.8, 11.9, 11.9, 11.7, 13.1, 13.3, 12, 13.3, 15.2], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '伤口脓液', type: 'bar', stack: '', data: [6, 5.4, 4.9, 6.5, 5.3, 5.2, 10.6, 5.6, 9.8, 4.8, 5.1, 7.3, 11.7], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case '主要细菌在革兰阴性杆菌中所占比例变迁':
                    {
                        legend = ['大肠埃希菌', '铜绿假单胞菌', '肺炎克雷伯菌', '鲍曼不动杆菌'];
                        value = [
                            { name: '大肠埃希菌', type: 'bar', stack: '', data: [25.9, 26.6, 27.6, 26.5, 25.8, 26.9, 28.4, 27.3, 27.2, 28.8, 27.8, 27.2, 27.2], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '铜绿假单胞菌', type: 'bar', stack: '', data: [17.4, 17.2, 16.9, 16.4, 15.8, 14.8, 14.1, 14, 13.4, 13, 12.4, 12.1, 12.3], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '肺炎克雷伯菌', type: 'bar', stack: '', data: [14, 13.6, 12.8, 13.6, 14.7, 14.7, 15, 16.5, 17.9, 18.1, 18.2, 17.9, 19.5], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '鲍曼不动杆菌', type: 'bar', stack: '', data: [13.2, 11.1, 11.5, 12.4, 13.4, 14.4, 13.9, 15, 14.6, 14.2, 14.2, 13.6, 13], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case '肠杆菌科细菌对头孢噻肟(或头孢曲松)耐药菌株的检出率变迁':
                    {
                        legend = ['大肠埃希菌', '肺炎克雷伯菌', '奇异变形杆菌'];
                        value = [
                            { name: '大肠埃希菌', type: 'bar', stack: '', data: [51.9, 54.5, 59.5, 61.2, 59.8, 57.9, 57.2, 61.7, 61.0, 61.1, 59.6, 57.4, 58.1], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '肺炎克雷伯菌', type: 'bar', stack: '', data: [49.3, 52.2, 50.9, 51.4, 47.5, 45.2, 46.4, 44.8, 43.6, 40.6, 41.9, 42.7, 44.9], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '奇异变形杆菌', type: 'bar', stack: '', data: [16.0, 25.1, 22.2, 23.8, 25.5, 23.0, 27.8, 30.3, 32.4, 34.7, 33.5, 35.1, 33.9], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case 'CHINET监测历年分离革兰阴性菌和革兰阳性菌所占比例':
                    {
                        legend = ['革兰阴性菌', '革兰阳性菌'];
                        value = [
                            { name: '革兰阴性菌', type: 'bar', stack: '', data: [66.9, 68.2, 65.7, 69.5, 71, 71.6, 71.5, 71.9, 73, 72.6, 70.2, 71.6, 70.8], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '革兰阳性菌', type: 'bar', stack: '', data: [33.1, 31.8, 34.3, 30.5, 29, 28.4, 28.5, 28.1, 27, 27.4, 29.8, 28.4, 29.2], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case 'CHINET监测历年住院和门急诊患者分离菌株数所占比例':
                    {
                        legend = ['住院患者分离菌株', '门诊患者分离菌株'];
                        value = [
                            { name: '住院患者分离菌株', type: 'bar', stack: '', data: [89.6, 82.6, 87, 86.8, 87.5, 87.8, 84.6, 87.3, 86.2, 84.9, 83.5, 86.6, 87.3], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '门诊患者分离菌株', type: 'bar', stack: '', data: [10.4, 17.4, 13, 13.2, 12.5, 12.2, 15.4, 12.7, 13.8, 15.1, 16.5, 13.4, 12.7], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case '儿童患者非脑膜炎肺炎链球菌的分布':
                    {
                        text = ['2015年', '2016年', '2017年'];
                        legend = ['PSSP', 'PISP', 'PRSP'];
                        value = [
                            { name: 'PSSP', type: 'bar', barWidth: 30, stack: '', data: [86.5, 89.6, 86.8], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: 'PISP', type: 'bar', barWidth: 30, stack: '', data: [6.3, 7.4, 11], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: 'PRSP', type: 'bar', barWidth: 30, stack: '', data: [7.2, 3.1, 2.2], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case '成人患者非脑膜炎肺炎链球菌的分布':
                    {
                        text = ['2015年', '2016年', '2017年'];
                        legend = ['PSSP', 'PISP', 'PRSP'];
                        value = [
                            { name: 'PSSP', type: 'bar', barWidth: 30, stack: '', data: [91.8, 95.4, 94.7], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: 'PISP', type: 'bar', barWidth: 30, stack: '', data: [5.6, 3.4, 3.4], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: 'PRSP', type: 'bar', barWidth: 30, stack: '', data: [2.6, 1.2, 1.9], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case '肠杆菌科细菌对亚胺培南耐药变迁':
                    {
                        legend = ['肺炎克雷伯菌', '大肠埃希菌', '阴沟肠杆菌', '弗劳地柠檬酸杆菌'];
                        value = [
                            { name: '肺炎克雷伯菌', type: 'bar', stack: '', data: [3, 3.4, 2.4, 4, 4.9, 9.2, 9, 10, 10.3, 11, 15.6, 16.1, 20.9], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '大肠埃希菌', type: 'bar', stack: '', data: [1.1, 1.4, 0.7, 1.2, 1.7, 1.6, 0.9, 0.9, 1, 0.9, 1.4, 1.3, 1.9], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '阴沟肠杆菌', type: 'bar', stack: '', data: [8.1, 8.5, 5.2, 5.1, 4.9, 5.4, 2.9, 2.6, 3.6, 3.8, 5.6, 3.7, 7.7], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '弗劳地柠檬酸杆菌', type: 'bar', stack: '', data: [11.3, 13.4, 8.7, 10.7, 7.3, 10.8, 8, 4.9, 7.1, 10.7, 12.5, 11.6, 11.7], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case '肠杆菌科细菌对碳青霉烯类的耐药变迁（7996-82754株）':
                    {
                        legend = ['亚胺培南', '美罗培南'];
                        value = [
                            { name: '亚胺培南', type: 'bar', stack: '', data: [3.1, 3.4, 2.2, 3.2, 3.6, 4.6, 4.5, 5, 5.1, 5.3, 7.1, 7, 9.4, 11.4], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '美罗培南', type: 'bar', stack: '', data: [2.1, 1.6, 1.7, 2.1, 2.5, 4.2, 3.8, 4.4, 6.7, 5.4, 7, 7.2, 9.8, 11.6], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                case '流感嗜血杆菌对氨苄西林耐药变迁':
                    {
                        legend = ['儿童', '成人'];
                        value = [
                            { name: '儿童', type: 'bar', stack: '', data: [16.4, 33.1, 34, 37.2, 29.6, 34.2, 33.5, 40.8, 40.9, 45.3, 49.1, 56.9, 56.4], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } },
                            { name: '成人', type: 'bar', stack: '', data: [23, 8, 14.6, 20, 18.7, 27.7, 22.7, 25.7, 36.4, 36, 35.5, 47.2, 44.9], itemStyle: { normal: { label: { show: true, position: 'right', formatter: '{c}' } } } }
                        ];
                    }
                    break;
                default:
                    break;
            }
            _this.loadingGermYearEchart(legend, text, value);
        },
        loadingGermYearEchart(legend, text, value) {
            var _this = this;
            // 重置高度
            if (text.length === 3 && legend.length === 3) {
                _this.$refs.GermYearChart.style.height = '440px';
            }
            else if (text.length > 8 && legend.length >= 4) {
                _this.$refs.GermYearChart.style.height = '1500px';
            }
            else if (text.length > 8 && legend.length === 3) {
                _this.$refs.GermYearChart.style.height = '1200px';
            }
            else if (text.length > 8) {
                _this.$refs.GermYearChart.style.height = '1000px';
            }
            else if (text.length <= 4) {
                _this.$refs.GermYearChart.style.height = '480px';
            }
            else {
                _this.$refs.GermYearChart.style.height = '680px';
            }
            try {
                if (_this.GermBarEchartObj) {
                    echarts.dispose(_this.GermBarEchartObj);
                }
            } catch (e) {
                console.log(e);
            }
            try {
                if (_this.GermYearEchartObj) {
                    echarts.dispose(_this.GermYearEchartObj);
                }
            } catch (e) {
                console.log(e);
            }
            // 基于准备好的dom，初始化echarts实例
            _this.GermYearEchartObj = echarts.init(_this.$refs.GermYearChart),
                option = {
                    color: ['#C1232B', '#015BAA', '#FE8463', '#000F1A', '#FAD860', '#F3A43B', '#60C0DD', '#D7504B', '#C6E579', '#F4E001', '#F0805A', '#26C0C0'],
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {
                            type: 'shadow'
                        }
                    },
                    title: {
                        text: _this.GermYear.PageContentTitle,
                        subtext: '',
                        left: 'center',
                        show: false
                    },
                    legend: {
                        data: legend
                    },
                    grid: {
                        left: '0.1%',
                        right: '8%',
                        bottom: '3%',
                        containLabel: true
                    },
                    calculable: true,
                    xAxis: [
                        {
                            position: 'top',
                            type: 'value',
                            axisLabel: {
                                show: true,
                                onZero: false,
                                interval: 'auto',
                                formatter: '{value} %'
                            },
                            show: true
                        }
                    ],
                    yAxis: [
                        {
                            type: 'category',
                            axisLabel: {
                                textStyle: {
                                    fontSize: "14px"
                                }
                            },
                            axisTick: {
                                show: false
                            },
                            data: text
                        }
                    ],
                    series: value
                };

            // 使用刚指定的配置项和数据显示图表。
            _this.GermYearEchartObj.setOption(option);

            setTimeout(() => {
                try {
                    $(_this.$refs.GermYearActionsheet).fadeOut(200);
                    _this.loadingSwiper(_this.$refs.GermYearSwiperContainer);
                    setTimeout(() => {
                        $('#heatmap-container').css('display', 'none');
                        $('#germbar-container').css('display', 'none');
                        $('#germyear-container').css('display', 'block');
                    });
                } catch (e) {
                    console.log(e);
                }
            });
        },
        loadingSwiper(swiperContainer) {
            if (this.SwiperObj) {
                this.SwiperObj.destroy(false);
            }

            swiperContainer.style.height = this.$refs.middle.offsetHeight + 'px';
            setTimeout(() => {
                this.SwiperObj = new Swiper(swiperContainer, {
                    direction: 'vertical',
                    loop: false,
                    watchSlidesProgress: true,
                    freeMode: true,
                    slidesPerView: 'auto'
                });
            });
        }
    }
});