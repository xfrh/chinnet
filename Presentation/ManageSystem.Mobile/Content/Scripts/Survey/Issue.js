Vue.use(VeeValidate, { locale: 'zh_CN' });

var vm = new Vue({
    el: '#app',
    data: {
        mySwiper: null,
        pageSwiper: null,
        swiperOption: {
            direction: 'vertical',
            freeMode: true,
            observer: true,
            observeSlideChildren: true,
            slidesPerView: 'auto',
            mousewheel: {
                releaseOnEdges: true
            },
            watchSlidesProgress: true,
            resistanceRatio: 0,
            slidesOffsetAfter: 25
        },
        survey: {
            title: null,
            content: null
        },
        beginAnswer: false,
        showSubmit: false,
        q26EndHeight: 0,
        page_loading: null,
        isSubmit: false,
        /********************** Page 2 *******************************/
        Q1: {
            title: '1. 大肠埃希菌 检出株数（株/年）？',
            type: 'input',
            name: 'Q1',
            placeholder: '填写大于0的整数',
            answer: null
        },
        Q2: {
            title: '2. 碳青霉烯类耐药大肠埃希菌（CR-ECO） 检出株数（株/年）?',
            type: 'input',
            name: 'Q2',
            placeholder: '填写大于0的整数',
            answer: null
        },
        Q3: {
            title: '3. CR-ECO主要标本来源有哪些？（列举前三标本来源及百分比）',
            options: [
                { id: 'checkbox__3_1', name: '_3_biaoben', text: '痰', value: '痰' },
                { id: 'checkbox__3_2', name: '_3_biaoben', text: '血液', value: '血液' },
                { id: 'checkbox__3_3', name: '_3_biaoben', text: '粪便', value: '粪便' },
                { id: 'checkbox__3_4', name: '_3_biaoben', text: '肺泡灌洗液', value: '肺泡灌洗液' },
                { id: 'checkbox__3_5', name: '_3_biaoben', text: '脓液/伤口', value: '脓液/伤口' },
                { id: 'checkbox__3_6', name: '_3_biaoben', text: '尿道', value: '尿道' },
                { id: 'checkbox__3_7', name: '_3_biaoben', text: '中心静脉导管', value: '中心静脉导管' },
                { id: 'checkbox__3_8', name: '_3_biaoben', text: '穿刺液', value: '穿刺液' },
                { id: 'checkbox__3_9', name: '_3_biaoben', text: '其他：', value: '其他' }
            ],
            answer: [],
            other: null,
            rate: null,
            picker: [
                { label: '0-5%', value: '0-5%' },
                { label: '6-10%', value: '6-10%' },
                { label: '11-15%', value: '11-15%' },
                { label: '16-20%', value: '16-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51%以上', value: '51%以上' }
            ]
        },
        Q4: {
            title: '4. CR-ECO主要科室来源有哪些？（列举前三科室来源及百分比）',
            options: [
                { id: 'checkbox__4_1', name: '_4_keshi', text: '血液科', value: '血液科' },
                { id: 'checkbox__4_2', name: '_4_keshi', text: 'ICU', value: 'ICU' },
                { id: 'checkbox__4_3', name: '_4_keshi', text: '呼吸科', value: '呼吸科' },
                { id: 'checkbox__4_4', name: '_4_keshi', text: '感染科', value: '感染科' },
                { id: 'checkbox__4_5', name: '_4_keshi', text: '移植科', value: '移植科' },
                { id: 'checkbox__4_6', name: '_4_keshi', text: '其他：', value: '其他' }
            ],
            answer: [],
            other: null,
            rate: null,
            picker: [
                { label: '0-5%', value: '0-5%' },
                { label: '6-10%', value: '6-10%' },
                { label: '11-15%', value: '11-15%' },
                { label: '16-20%', value: '16-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51%以上', value: '51%以上' }
            ]
        },
        /********************** Page 3 *******************************/
        Q5: {
            title: '5. 肺炎克雷伯菌检出株数（株/年）？',
            type: 'input',
            name: 'Q5',
            placeholder: '填写大于0的整数',
            answer: null
        },
        Q6: {
            title: '6. 碳青霉烯类耐药肺炎克雷伯菌（CR-KPN）检出株数（株/年）?',
            type: 'input',
            name: 'Q6',
            placeholder: '填写大于0的整数',
            answer: null
        },
        Q7: {
            title: '7. CR-KPN主要标本来源有哪些？（列举前三位标本及百分比）',
            options: [
                { id: 'checkbox__7_1', name: '_7_biaoben', text: '痰', value: '痰' },
                { id: 'checkbox__7_2', name: '_7_biaoben', text: '血液', value: '血液' },
                { id: 'checkbox__7_3', name: '_7_biaoben', text: '粪便', value: '粪便' },
                { id: 'checkbox__7_4', name: '_7_biaoben', text: '肺泡灌洗液', value: '肺泡灌洗液' },
                { id: 'checkbox__7_5', name: '_7_biaoben', text: '脓液/伤口', value: '脓液/伤口' },
                { id: 'checkbox__7_6', name: '_7_biaoben', text: '尿道', value: '尿道' },
                { id: 'checkbox__7_7', name: '_7_biaoben', text: '中心静脉导管', value: '中心静脉导管' },
                { id: 'checkbox__7_8', name: '_7_biaoben', text: '穿刺液', value: '穿刺液' },
                { id: 'checkbox__7_9', name: '_7_biaoben', text: '其他：', value: '其他' }
            ],
            answer: [],
            other: null,
            rate: null,
            picker: [
                { label: '0-5%', value: '0-5%' },
                { label: '6-10%', value: '6-10%' },
                { label: '11-15%', value: '11-15%' },
                { label: '16-20%', value: '16-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51%以上', value: '51%以上' }
            ]
        },
        Q8: {
            title: '8. CR-KPN主要科室来源有哪些（列举前三科室来源及百分比）',
            options: [
                { id: 'checkbox__8_1', name: '_8_keshi', text: '血液科', value: '血液科' },
                { id: 'checkbox__8_2', name: '_8_keshi', text: 'ICU', value: 'ICU' },
                { id: 'checkbox__8_3', name: '_8_keshi', text: '呼吸科', value: '呼吸科' },
                { id: 'checkbox__8_4', name: '_8_keshi', text: '感染科', value: '感染科' },
                { id: 'checkbox__8_5', name: '_8_keshi', text: '移植科', value: '移植科' },
                { id: 'checkbox__8_6', name: '_8_keshi', text: '其他：', value: '其他' }
            ],
            answer: [],
            other: null,
            rate: null,
            picker: [
                { label: '0-5%', value: '0-5%' },
                { label: '6-10%', value: '6-10%' },
                { label: '11-15%', value: '11-15%' },
                { label: '16-20%', value: '16-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51%以上', value: '51%以上' }
            ]
        },
        /********************** Page 4 *******************************/
        Q9: {
            title: '9. 阴沟肠杆菌检出株数（株/年）？',
            type: 'input',
            name: 'Q9',
            placeholder: '填写大于0的整数',
            answer: null
        },
        Q10: {
            title: '10. 碳青霉烯类耐药阴沟肠杆菌（CR-ECL）检出株数（株/年）?',
            type: 'input',
            name: 'Q10',
            placeholder: '填写大于0的整数',
            answer: null
        },
        Q11: {
            title: '11. CR-ECL主要标本来源有哪些？（列举前三位标本及百分比）',
            options: [
                { id: 'checkbox__11_1', name: '_11_biaoben', text: '痰', value: '痰' },
                { id: 'checkbox__11_2', name: '_11_biaoben', text: '血液', value: '血液' },
                { id: 'checkbox__11_3', name: '_11_biaoben', text: '粪便', value: '粪便' },
                { id: 'checkbox__11_4', name: '_11_biaoben', text: '肺泡灌洗液', value: '肺泡灌洗液' },
                { id: 'checkbox__11_5', name: '_11_biaoben', text: '脓液/伤口', value: '脓液/伤口' },
                { id: 'checkbox__11_6', name: '_11_biaoben', text: '尿道', value: '尿道' },
                { id: 'checkbox__11_7', name: '_11_biaoben', text: '中心静脉导管', value: '中心静脉导管' },
                { id: 'checkbox__11_8', name: '_11_biaoben', text: '穿刺液', value: '穿刺液' },
                { id: 'checkbox__11_9', name: '_11_biaoben', text: '其他：', value: '其他' }
            ],
            answer: [],
            other: null,
            rate: null,
            picker: [
                { label: '0-5%', value: '0-5%' },
                { label: '6-10%', value: '6-10%' },
                { label: '11-15%', value: '11-15%' },
                { label: '16-20%', value: '16-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51%以上', value: '51%以上' }
            ]
        },
        Q12: {
            title: '12. CR-ECO主要科室来源有哪些？（列举前三科室来源及百分比）',
            options: [
                { id: 'checkbox__12_1', name: '_12_keshi', text: '血液科', value: '血液科' },
                { id: 'checkbox__12_2', name: '_12_keshi', text: 'ICU', value: 'ICU' },
                { id: 'checkbox__12_3', name: '_12_keshi', text: '呼吸科', value: '呼吸科' },
                { id: 'checkbox__12_4', name: '_12_keshi', text: '感染科', value: '感染科' },
                { id: 'checkbox__12_5', name: '_12_keshi', text: '移植科', value: '移植科' },
                { id: 'checkbox__12_6', name: '_12_keshi', text: '其他：', value: '其他' }
            ],
            answer: [],
            other: null,
            rate: null,
            picker: [
                { label: '0-5%', value: '0-5%' },
                { label: '6-10%', value: '6-10%' },
                { label: '11-15%', value: '11-15%' },
                { label: '16-20%', value: '16-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51%以上', value: '51%以上' }
            ]
        },
        /********************** Page 5 *******************************/
        Q13: {
            title: '13. 黏质沙雷菌检出株数（株/年）？',
            type: 'input',
            name: 'Q13',
            placeholder: '填写大于0的整数',
            answer: null
        },
        Q14: {
            title: '14. 碳青霉烯类耐药黏质沙雷菌（CR-SMA）检出株数（株/年）?',
            type: 'input',
            name: 'Q14',
            placeholder: '填写大于0的整数',
            answer: null
        },
        Q15: {
            title: '15. CR-SMA主要标本来源有哪些？（列举前三位标本及百分比）',
            options: [
                { id: 'checkbox__15_1', name: '_15_biaoben', text: '痰', value: '痰' },
                { id: 'checkbox__15_2', name: '_15_biaoben', text: '血液', value: '血液' },
                { id: 'checkbox__15_3', name: '_15_biaoben', text: '粪便', value: '粪便' },
                { id: 'checkbox__15_4', name: '_15_biaoben', text: '肺泡灌洗液', value: '肺泡灌洗液' },
                { id: 'checkbox__15_5', name: '_15_biaoben', text: '脓液/伤口', value: '脓液/伤口' },
                { id: 'checkbox__15_6', name: '_15_biaoben', text: '尿道', value: '尿道' },
                { id: 'checkbox__15_7', name: '_15_biaoben', text: '中心静脉导管', value: '中心静脉导管' },
                { id: 'checkbox__15_8', name: '_15_biaoben', text: '穿刺液', value: '穿刺液' },
                { id: 'checkbox__15_9', name: '_15_biaoben', text: '其他：', value: '其他' }
            ],
            answer: [],
            other: null,
            rate: null,
            picker: [
                { label: '0-5%', value: '0-5%' },
                { label: '6-10%', value: '6-10%' },
                { label: '11-15%', value: '11-15%' },
                { label: '16-20%', value: '16-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51%以上', value: '51%以上' }
            ]
        },
        Q16: {
            title: '16. CR-SMA主要科室来源有哪些？（列举前三科室来源及百分比）',
            options: [
                { id: 'checkbox__16_1', name: '_16_keshi', text: '血液科', value: '血液科' },
                { id: 'checkbox__16_2', name: '_16_keshi', text: 'ICU', value: 'ICU' },
                { id: 'checkbox__16_3', name: '_16_keshi', text: '呼吸科', value: '呼吸科' },
                { id: 'checkbox__16_4', name: '_16_keshi', text: '感染科', value: '感染科' },
                { id: 'checkbox__16_5', name: '_16_keshi', text: '移植科', value: '移植科' },
                { id: 'checkbox__16_6', name: '_16_keshi', text: '其他：', value: '其他' }
            ],
            answer: [],
            other: null,
            rate: null,
            picker: [
                { label: '0-5%', value: '0-5%' },
                { label: '6-10%', value: '6-10%' },
                { label: '11-15%', value: '11-15%' },
                { label: '16-20%', value: '16-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51%以上', value: '51%以上' }
            ]
        },
        /********************** Page 6 *******************************/
        Q17: {
            title: '17. 铜绿假单胞菌检出株数（株/年）？',
            type: 'input',
            name: 'Q17',
            placeholder: '填写大于0的整数',
            answer: null
        },
        Q18: {
            title: '18. 碳青霉烯类耐药铜绿假单胞菌（CR-PAE）检出株数（株/年）?',
            type: 'input',
            name: 'Q18',
            placeholder: '填写大于0的整数',
            answer: null
        },
        Q19: {
            title: '19. CR-PAE主要标本来源有哪些？（列举前三位标本及百分比）',
            options: [
                { id: 'checkbox__19_1', name: '_19_biaoben', text: '痰', value: '痰' },
                { id: 'checkbox__19_2', name: '_19_biaoben', text: '血液', value: '血液' },
                { id: 'checkbox__19_3', name: '_19_biaoben', text: '粪便', value: '粪便' },
                { id: 'checkbox__19_4', name: '_19_biaoben', text: '肺泡灌洗液', value: '肺泡灌洗液' },
                { id: 'checkbox__19_5', name: '_19_biaoben', text: '脓液/伤口', value: '脓液/伤口' },
                { id: 'checkbox__19_6', name: '_19_biaoben', text: '尿道', value: '尿道' },
                { id: 'checkbox__19_7', name: '_19_biaoben', text: '中心静脉导管', value: '中心静脉导管' },
                { id: 'checkbox__19_8', name: '_19_biaoben', text: '穿刺液', value: '穿刺液' },
                { id: 'checkbox__19_9', name: '_19_biaoben', text: '其他：', value: '其他' }
            ],
            answer: [],
            other: null,
            rate: null,
            picker: [
                { label: '10-20%', value: '10-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51-60%', value: '51-60%' },
                { label: '61-70%', value: '61-70%' },
                { label: '71%以上', value: '71%以上' }
            ]
        },
        Q20: {
            title: '20. CR-PAE主要科室来源有哪些？（列举前三科室来源及百分比）',
            options: [
                { id: 'checkbox__20_1', name: '_20_keshi', text: '血液科', value: '血液科' },
                { id: 'checkbox__20_2', name: '_20_keshi', text: 'ICU', value: 'ICU' },
                { id: 'checkbox__20_3', name: '_20_keshi', text: '呼吸科', value: '呼吸科' },
                { id: 'checkbox__20_4', name: '_20_keshi', text: '感染科', value: '感染科' },
                { id: 'checkbox__20_5', name: '_20_keshi', text: '移植科', value: '移植科' },
                { id: 'checkbox__20_6', name: '_20_keshi', text: '其他：', value: '其他' }
            ],
            answer: [],
            other: null,
            rate: null,
            picker: [
                { label: '10-20%', value: '10-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51-60%', value: '51-60%' },
                { label: '61-70%', value: '61-70%' },
                { label: '71%以上', value: '71%以上' }
            ]
        },
        /********************** Page 7 *******************************/
        Q21: {
            title: '21. 鲍曼不动杆菌检出株数（株/年）？',
            type: 'input',
            name: 'Q21',
            placeholder: '填写大于0的整数',
            answer: null
        },
        Q22: {
            title: '22. 碳青霉烯类耐药鲍曼不动杆菌（CR-ABA）检出株数（株/年）?',
            type: 'input',
            name: 'Q22',
            placeholder: '填写大于0的整数',
            answer: null
        },
        Q23: {
            title: '23. CR-ABA主要标本来源有哪些？（列举前三位标本及百分比）',
            options: [
                { id: 'checkbox__23_1', name: '_23_biaoben', text: '痰', value: '痰' },
                { id: 'checkbox__23_2', name: '_23_biaoben', text: '血液', value: '血液' },
                { id: 'checkbox__23_3', name: '_23_biaoben', text: '粪便', value: '粪便' },
                { id: 'checkbox__23_4', name: '_23_biaoben', text: '肺泡灌洗液', value: '肺泡灌洗液' },
                { id: 'checkbox__23_5', name: '_23_biaoben', text: '脓液/伤口', value: '脓液/伤口' },
                { id: 'checkbox__23_6', name: '_23_biaoben', text: '尿道', value: '尿道' },
                { id: 'checkbox__23_7', name: '_23_biaoben', text: '中心静脉导管', value: '中心静脉导管' },
                { id: 'checkbox__23_8', name: '_23_biaoben', text: '穿刺液', value: '穿刺液' },
                { id: 'checkbox__23_9', name: '_23_biaoben', text: '其他：', value: '其他' }
            ],
            answer: [],
            other: null,
            rate: null,
            picker: [
                { label: '10-20%', value: '10-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51-60%', value: '51-60%' },
                { label: '61-70%', value: '61-70%' },
                { label: '71%以上', value: '71%以上' }
            ]
        },
        Q24: {
            title: '24. CR-ABA主要科室来源有哪些？（列举前三科室来源及百分比）',
            options: [
                { id: 'checkbox__24_1', name: '_24_keshi', text: '血液科', value: '血液科' },
                { id: 'checkbox__24_2', name: '_24_keshi', text: 'ICU', value: 'ICU' },
                { id: 'checkbox__24_3', name: '_24_keshi', text: '呼吸科', value: '呼吸科' },
                { id: 'checkbox__24_4', name: '_24_keshi', text: '感染科', value: '感染科' },
                { id: 'checkbox__24_5', name: '_24_keshi', text: '移植科', value: '移植科' },
                { id: 'checkbox__24_6', name: '_24_keshi', text: '其他：', value: '其他' }
            ],
            answer: [],
            other: null,
            rate: null,
            picker: [
                { label: '10-20%', value: '10-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51-60%', value: '51-60%' },
                { label: '61-70%', value: '61-70%' },
                { label: '71%以上', value: '71%以上' }
            ]
        },
        /********************** Page 8 *******************************/
        Q25: {
            title: '25. 目前实验室检测碳青霉烯酶的方法',
            type: 'checkbox',
            name: 'Q25',
            options: [
                { id: 'checkbox__25_1', name: 'Q25_checkbox', text: '(1) 仅以药敏试验结果判断CRE', value: '(1) 仅以药敏试验结果判断CRE' },
                { id: 'checkbox__25_2', name: 'Q25_checkbox', text: '(2) 改良Hodge试验', value: '(2) 改良Hodge试验' },
                { id: 'checkbox__25_3', name: 'Q25_checkbox', text: '(3) Carba NP', value: '(3) Carba NP' },
                { id: 'checkbox__25_4', name: 'Q25_checkbox', text: '(4) mCIM和eCIM', value: '(4) mCIM和eCIM' },
                { id: 'checkbox__25_5', name: 'Q25_checkbox', text: '(5) EDTA和APB抑制试验', value: '(5) EDTA和APB抑制试验' },
                { id: 'checkbox__25_6', name: 'Q25_checkbox', text: '(6) 金标免疫快速检测技术', value: '(6) 金标免疫快速检测技术' },
                { id: 'checkbox__25_7', name: 'Q25_checkbox', text: '(7) 常规PCR技术', value: '(7) 常规PCR技术' },
                { id: 'checkbox__25_8', name: 'Q25_checkbox', text: '(8) GeneXpert', value: '(8) GeneXpert' },
                { id: 'checkbox__25_9', name: 'Q25_checkbox', text: '(9) 国产碳青霉烯酶基因检测试剂盒', value: '(9) 国产碳青霉烯酶基因检测试剂盒' }
            ],
            answer: []
        },
        Q26: {
            title: '26. 药敏试验是否常规报告CRE所产碳青霉烯酶型别？',
            type: 'radio',
            name: 'Q26',
            options: [
                { text: '否', value: '否' },
                { text: '是', value: '是' }
            ],
            answer: null,
            other: null
        },
        /********************** Page 9 *******************************/
        Q27: {
            title: '27. 您单位分离的CRE主要产哪种碳青霉烯酶及所占比例？',
            type: 'radio',
            name: 'Q27',
            options: [
                { id: 'radio__27_1', name: 'Q27_radio', text: '(1) KPC 及所占比例%：', value: '(1) KPC 及所占比例：', name_input: 'Q27_input_1', rate: null },
                { id: 'radio__27_2', name: 'Q27_radio', text: '(2) NDM 及所占比例%：', value: '(2) NDM 及所占比例：', name_input: 'Q27_input_2', rate: null },
                { id: 'radio__27_3', name: 'Q27_radio', text: '(3) OXA-48 及所占比例%：', value: '(3) OXA-48 及所占比例：', name_input: 'Q27_input_3', rate: null },
                { id: 'radio__27_4', name: 'Q27_radio', text: '(4) IPM 及所占比例%：', value: '(4) IPM 及所占比例：', name_input: 'Q27_input_4', rate: null },
                { id: 'radio__27_5', name: 'Q27_radio', text: '(5) VIM 及所占比例%：', value: '(5) VIM 及所占比例：', name_input: 'Q27_input_5', rate: null },
                { id: 'radio__27_6', name: 'Q27_radio', text: '(6) 其他 及所占比例%：', value: '(6) 其他 及所占比例：', name_input: 'Q27_input_6', rate: null }
            ],
            answer: null
        },
        Q28: {
            title: '28. 实验室分离到CRE菌株时，补充加做以下哪些药物的药敏试验？',
            type: 'checkbox',
            name: 'Q28',
            options: [
                { id: 'checkbox__28_1', name: 'Q28_checkbox', text: '(1) 多黏菌素', value: '(1) 多黏菌素' },
                { id: 'checkbox__28_2', name: 'Q28_checkbox', text: '(2) 替加环素', value: '(2) 替加环素' },
                { id: 'checkbox__28_3', name: 'Q28_checkbox', text: '(3) 头孢他啶-阿维巴坦', value: '(3) 头孢他啶-阿维巴坦' },
                { id: 'checkbox__28_4', name: 'Q28_checkbox', text: '(4) 磷霉素', value: '(4) 磷霉素' },
                { id: 'checkbox__28_5', name: 'Q28_checkbox', text: '(5) 氯霉素', value: '(5) 氯霉素' },
                { id: 'checkbox__28_6', name: 'Q28_checkbox', text: '(6) 联合药敏试验', value: '(6) 联合药敏试验' },
                { id: 'checkbox__28_7', name: 'Q28_checkbox', text: '(7) 未补充其他药物', value: '(7) 未补充其他药物' }
            ],
            answer: []
        },
        /********************** Page 10 *******************************/
        Q29: {
            title: '医院名称',
            hospital: null
        },
        Q30: {
            title: '您的姓名',
            name: null
        },
        Q31: {
            title: '请输入您的手机号码',
            mobile: null
        }
    },
    watch: {
        'Q3.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q3.answer.length > 3) {
                    that.Q3.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        },
        'Q4.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q4.answer.length > 3) {
                    that.Q4.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        },
        'Q7.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q7.answer.length > 3) {
                    that.Q7.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        },
        'Q8.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q8.answer.length > 3) {
                    that.Q8.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        },
        'Q11.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q11.answer.length > 3) {
                    that.Q11.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        },
        'Q12.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q12.answer.length > 3) {
                    that.Q12.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        },
        'Q15.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q15.answer.length > 3) {
                    that.Q15.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        },
        'Q16.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q16.answer.length > 3) {
                    that.Q16.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        },
        'Q19.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q19.answer.length > 3) {
                    that.Q19.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        },
        'Q20.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q20.answer.length > 3) {
                    that.Q20.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        },
        'Q23.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q23.answer.length > 3) {
                    that.Q23.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        },
        'Q24.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q24.answer.length > 3) {
                    that.Q24.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        },
        'Q26.answer': function myfunction(newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                that.showSubmit = that.Q26.answer === '否';
                if (that.showSubmit) {
                    if (that.q26EndHeight === 0) {
                        that.q26EndHeight = that.mySwiper.translate - 36;
                    }
                    that.mySwiper.setTranslate(that.q26EndHeight);
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        }
    },
    created: function () {
        var that = this;
        that.$nextTick(function () {
            that.page_loading = weui.loading('页面加载中');
        });
    },
    mounted: function () {
        var that = this;
        that.$nextTick(function () {
            axios.post('/Survey/Issue_OnInit', { id: GetQueryString("id"), token: localStorage.getItem('token') || null }).then(res => {
                if (that.page_loading) {
                    setTimeout(function () {
                        that.page_loading.hide(function () {
                            that.page_loading = null;
                        });
                    }, 800);
                }
                if (res.data.status) {
                    if (res.status === 200) {
                        that.survey = res.data.survey;
                        setTimeout(function () {
                            that.mySwiper = new Swiper(that.$data.mySwiper, that.swiperOption);
                        }, 50);
                    }
                    else {
                        weui.alert('页面初始化失败');
                    }
                } else {
                    weui.alert(res.data.message);
                }
            }).catch(ex => {
                if (that.page_loading) {
                    setTimeout(function () {
                        that.page_loading.hide(function () {
                            that.page_loading = null;
                        });
                    }, 800);
                }
                weui.alert('页面初始化失败');

            });

        });
    },
    methods: {
        uuid: function (len, radix) {
            var chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'.split('');
            var uuid = [], i;
            radix = radix || chars.length;

            if (len) {
                // Compact form
                for (i = 0; i < len; i++) uuid[i] = chars[0 | Math.random() * radix];
            } else {
                // rfc4122, version 4 form
                var r;

                // rfc4122 requires these characters
                uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
                uuid[14] = '4';

                // Fill in random data.  At i==19 set the high bits of clock sequence as
                // per rfc4122, sec. 4.1.5
                for (i = 0; i < 36; i++) {
                    if (!uuid[i]) {
                        r = 0 | Math.random() * 16;
                        uuid[i] = chars[(i === 19) ? (r & 0x3) | 0x8 : r];
                    }
                }
            }

            return uuid.join('');
        },
        onQ3Picker: function (e) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q3.picker[0].value;
                if (that.Q3.rate && that.Q3.rate.length > 0) {
                    defaultValue = that.Q3.rate;
                }

                weui.picker(that.Q3.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        that.Q3.rate = result[0].value;
                    },
                    id: that.uuid(8, 10)
                });
            });
        },
        onQ4Picker: function (e) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q4.picker[0].value;
                if (that.Q4.rate && that.Q4.rate.length > 0) {
                    defaultValue = that.Q4.rate;
                }

                weui.picker(that.Q4.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        that.Q4.rate = result[0].value;
                    },
                    id: that.uuid(8, 10)
                });
            });
        },
        onQ7Picker: function (e) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q7.picker[0].value;
                if (that.Q7.rate && that.Q7.rate.length > 0) {
                    defaultValue = that.Q7.rate;
                }

                weui.picker(that.Q7.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        that.Q7.rate = result[0].value;
                    },
                    id: that.uuid(8, 10)
                });
            });
        },
        onQ8Picker: function (e) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q8.picker[0].value;
                if (that.Q8.rate && that.Q8.rate.length > 0) {
                    defaultValue = that.Q8.rate;
                }

                weui.picker(that.Q8.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        that.Q8.rate = result[0].value;
                    },
                    id: that.uuid(8, 10)
                });
            });
        },
        onQ11Picker: function (e) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q11.picker[0].value;
                if (that.Q11.rate && that.Q11.rate.length > 0) {
                    defaultValue = that.Q11.rate;
                }

                weui.picker(that.Q11.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        that.Q11.rate = result[0].value;
                    },
                    id: that.uuid(8, 10)
                });
            });
        },
        onQ12Picker: function (e) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q12.picker[0].value;
                if (that.Q12.rate && that.Q12.rate.length > 0) {
                    defaultValue = that.Q12.rate;
                }

                weui.picker(that.Q12.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        that.Q12.rate = result[0].value;
                    },
                    id: that.uuid(8, 10)
                });
            });
        },
        onQ15Picker: function (e) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q15.picker[0].value;
                if (that.Q15.rate && that.Q15.rate.length > 0) {
                    defaultValue = that.Q15.rate;
                }

                weui.picker(that.Q15.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        that.Q15.rate = result[0].value;
                    },
                    id: that.uuid(8, 10)
                });
            });
        },
        onQ16Picker: function (e) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q16.picker[0].value;
                if (that.Q16.rate && that.Q16.rate.length > 0) {
                    defaultValue = that.Q16.rate;
                }

                weui.picker(that.Q16.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        that.Q16.rate = result[0].value;
                    },
                    id: that.uuid(8, 10)
                });
            });
        },
        onQ19Picker: function (e) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q19.picker[0].value;
                if (that.Q19.rate && that.Q19.rate.length > 0) {
                    defaultValue = that.Q19.rate;
                }

                weui.picker(that.Q19.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        that.Q19.rate = result[0].value;
                    },
                    id: that.uuid(8, 10)
                });
            });
        },
        onQ20Picker: function (e) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q20.picker[0].value;
                if (that.Q20.rate && that.Q20.rate.length > 0) {
                    defaultValue = that.Q20.rate;
                }

                weui.picker(that.Q20.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        that.Q20.rate = result[0].value;
                    },
                    id: that.uuid(8, 10)
                });
            });
        },
        onQ23Picker: function (e) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q23.picker[0].value;
                if (that.Q23.rate && that.Q23.rate.length > 0) {
                    defaultValue = that.Q23.rate;
                }

                weui.picker(that.Q23.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        that.Q23.rate = result[0].value;
                    },
                    id: that.uuid(8, 10)
                });
            });
        },
        onQ24Picker: function (e) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q24.picker[0].value;
                if (that.Q24.rate && that.Q24.rate.length > 0) {
                    defaultValue = that.Q24.rate;
                }

                weui.picker(that.Q24.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        that.Q24.rate = result[0].value;
                    },
                    id: that.uuid(8, 10)
                });
            });
        },
        onBeginAnswer() {
            var that = this;
            that.$nextTick(function () {
                that.beginAnswer = true;
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                    that.mySwiper.slideNext();
                }, 50);
            });
        },
        onPrevSlide() {
            var that = this;
            that.$nextTick(function () {
                that.beginAnswer = true;
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                    that.mySwiper.slidePrev();
                }, 50);
            });
        },
        onNextSlide() {
            var that = this;
            that.$nextTick(function () {
                that.beginAnswer = true;
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                    that.mySwiper.slideNext();
                }, 50);
            });
        },
        onShowSubmit() {
            var that = this;
            that.showSubmit = true;
        },
        onHideSubmit() {
            var that = this;
            that.showSubmit = false;
        },
        onSubmit() {
            var that = this;
            that.$nextTick(function () {
                that.$validator.validateAll().then(result => {
                    if (result) {

                        //#region 多选验证 Q3-Q25
                        if (that.Q3.answer.length === 0) {
                            weui.topTips('3题未作答');
                            return false;
                        }
                        if (that.Q3.answer.length !== 3) {
                            weui.topTips('3题必须选择3项标本来源');
                            return false;
                        }
                        if (that.Q4.answer.length === 0) {
                            weui.topTips('4题未作答');
                            return false;
                        }
                        if (that.Q4.answer.length !== 3) {
                            weui.topTips('4题必须选择3项科室来源');
                            return false;
                        }
                        if (that.Q7.answer.length === 0) {
                            weui.topTips('7题未作答');
                            return false;
                        }
                        if (that.Q7.answer.length !== 3) {
                            weui.topTips('7题必须选择3项标本来源');
                            return false;
                        }
                        if (that.Q8.answer.length === 0) {
                            weui.topTips('8题未作答');
                            return false;
                        }
                        if (that.Q8.answer.length !== 3) {
                            weui.topTips('8题必须选择3项科室来源');
                            return false;
                        }
                        if (that.Q11.answer.length === 0) {
                            weui.topTips('11题未作答');
                            return false;
                        }
                        if (that.Q11.answer.length !== 3) {
                            weui.topTips('11题必须选择3项标本来源');
                            return false;
                        }
                        if (that.Q12.answer.length === 0) {
                            weui.topTips('12题未作答');
                            return false;
                        }
                        if (that.Q12.answer.length !== 3) {
                            weui.topTips('12题必须选择3项科室来源');
                            return false;
                        }
                        if (that.Q15.answer.length === 0) {
                            weui.topTips('15题未作答');
                            return false;
                        }
                        if (that.Q15.answer.length !== 3) {
                            weui.topTips('15题必须选择3项标本来源');
                            return false;
                        }
                        if (that.Q16.answer.length === 0) {
                            weui.topTips('16题未作答');
                            return false;
                        }
                        if (that.Q16.answer.length !== 3) {
                            weui.topTips('16题必须选择3项科室来源');
                            return false;
                        }
                        if (that.Q19.answer.length === 0) {
                            weui.topTips('19题未作答');
                            return false;
                        }
                        if (that.Q19.answer.length !== 3) {
                            weui.topTips('19题必须选择3项标本来源');
                            return false;
                        }
                        if (that.Q20.answer.length === 0) {
                            weui.topTips('20题未作答');
                            return false;
                        }
                        if (that.Q20.answer.length !== 3) {
                            weui.topTips('20题必须选择3项科室来源');
                            return false;
                        }
                        if (that.Q23.answer.length === 0) {
                            weui.topTips('23题未作答');
                            return false;
                        }
                        if (that.Q23.answer.length !== 3) {
                            weui.topTips('23题必须选择3项标本来源');
                            return false;
                        }
                        if (that.Q24.answer.length === 0) {
                            weui.topTips('24题未作答');
                            return false;
                        }
                        if (that.Q24.answer.length !== 3) {
                            weui.topTips('24题必须选择3项科室来源');
                            return false;
                        }
                        if (that.Q25.answer.length === 0) {
                            weui.topTips('25题未作答');
                            return false;
                        }
                        //#endregion

                        var data = [
                            { Key: 'id', Value: GetQueryString('id') },
                            { Key: 'token', Value: localStorage.getItem('token') || null }
                        ];

                        //#region Q3, Q4答案处理
                        var Q3_Answer = [];
                        that.Q3.answer.map(item => {
                            if (item === '其他') {
                                Q3_Answer.push(item + ':' + that.Q3.other);
                            }
                            else {
                                Q3_Answer.push(item);
                            }
                        });
                        Q3_Answer.push('百分比:' + that.Q3.rate);

                        var Q4_Answer = [];
                        that.Q4.answer.map(item => {
                            if (item === '其他') {
                                Q4_Answer.push(item + ':' + that.Q4.other);
                            }
                            else {
                                Q4_Answer.push(item);
                            }
                        });
                        Q4_Answer.push('百分比:' + that.Q4.rate);
                        //#endregion

                        //#region Q7, Q8答案处理
                        var Q7_Answer = [];
                        that.Q7.answer.map(item => {
                            if (item === '其他') {
                                Q7_Answer.push(item + ':' + that.Q7.other);
                            }
                            else {
                                Q7_Answer.push(item);
                            }
                        });
                        Q7_Answer.push('百分比:' + that.Q7.rate);

                        var Q8_Answer = [];
                        that.Q8.answer.map(item => {
                            if (item === '其他') {
                                Q8_Answer.push(item + ':' + that.Q8.other);
                            }
                            else {
                                Q8_Answer.push(item);
                            }
                        });
                        Q8_Answer.push('百分比:' + that.Q8.rate);
                        //#endregion

                        //#region Q11, Q12答案处理
                        var Q11_Answer = [];
                        that.Q11.answer.map(item => {
                            if (item === '其他') {
                                Q11_Answer.push(item + ':' + that.Q11.other);
                            }
                            else {
                                Q11_Answer.push(item);
                            }
                        });
                        Q11_Answer.push('百分比:' + that.Q11.rate);

                        var Q12_Answer = [];
                        that.Q12.answer.map(item => {
                            if (item === '其他') {
                                Q12_Answer.push(item + ':' + that.Q12.other);
                            }
                            else {
                                Q12_Answer.push(item);
                            }
                        });
                        Q12_Answer.push('百分比:' + that.Q12.rate);
                        //#endregion

                        //#region Q15, Q16答案处理
                        var Q15_Answer = [];
                        that.Q15.answer.map(item => {
                            if (item === '其他') {
                                Q15_Answer.push(item + ':' + that.Q15.other);
                            }
                            else {
                                Q15_Answer.push(item);
                            }
                        });
                        Q15_Answer.push('百分比:' + that.Q15.rate);

                        var Q16_Answer = [];
                        that.Q16.answer.map(item => {
                            if (item === '其他') {
                                Q16_Answer.push(item + ':' + that.Q16.other);
                            }
                            else {
                                Q16_Answer.push(item);
                            }
                        });
                        Q16_Answer.push('百分比:' + that.Q16.rate);
                        //#endregion

                        //#region Q19, Q20答案处理
                        var Q19_Answer = [];
                        that.Q19.answer.map(item => {
                            if (item === '其他') {
                                Q19_Answer.push(item + ':' + that.Q19.other);
                            }
                            else {
                                Q19_Answer.push(item);
                            }
                        });
                        Q19_Answer.push('百分比:' + that.Q19.rate);

                        var Q20_Answer = [];
                        that.Q20.answer.map(item => {
                            if (item === '其他') {
                                Q20_Answer.push(item + ':' + that.Q20.other);
                            }
                            else {
                                Q20_Answer.push(item);
                            }
                        });
                        Q20_Answer.push('百分比:' + that.Q20.rate);
                        //#endregion

                        //#region Q23, Q24答案处理
                        var Q23_Answer = [];
                        that.Q23.answer.map(item => {
                            if (item === '其他') {
                                Q23_Answer.push(item + ':' + that.Q23.other);
                            }
                            else {
                                Q23_Answer.push(item);
                            }
                        });
                        Q23_Answer.push('百分比:' + that.Q23.rate);

                        var Q24_Answer = [];
                        that.Q24.answer.map(item => {
                            if (item === '其他') {
                                Q24_Answer.push(item + ':' + that.Q24.other);
                            }
                            else {
                                Q24_Answer.push(item);
                            }
                        });
                        Q24_Answer.push('百分比:' + that.Q24.rate);
                        //#endregion

                        //#region 26题答案处理
                        var Q26_Answer = null;
                        if (that.Q26.answer === '是') {
                            Q26_Answer = '是,采用方法:' + that.Q26.other;
                        }
                        else {
                            Q26_Answer = '否';
                        }
                        //#endregion

                        //#region 1-4题
                        data.push({ Key: 'Content1', Value: that.Q1.answer });
                        data.push({ Key: 'Content2', Value: that.Q2.answer });
                        data.push({ Key: 'Content3', Value: Q3_Answer.join(',') });
                        data.push({ Key: 'Content4', Value: Q4_Answer.join(',') });
                        //data.Content1 = that.Q1.answer;
                        //data.Content2 = that.Q2.answer;
                        //data.Content3 = Q3_Answer.join(',');
                        //data.Content4 = Q4_Answer.join(',');
                        //#endregion

                        //#region 5-8题
                        data.push({ Key: 'Content5', Value: that.Q5.answer });
                        data.push({ Key: 'Content6', Value: that.Q6.answer });
                        data.push({ Key: 'Content7', Value: Q7_Answer.join(',') });
                        data.push({ Key: 'Content8', Value: Q7_Answer.join(',') });
                        //data.Content5 = that.Q5.answer;
                        //data.Content6 = that.Q6.answer;
                        //data.Content7 = Q7_Answer.join(',');
                        //data.Content8 = Q8_Answer.join(',');
                        //#endregion

                        //#region 9-12题
                        data.push({ Key: 'Content9', Value: that.Q9.answer });
                        data.push({ Key: 'Content10', Value: that.Q10.answer });
                        data.push({ Key: 'Content11', Value: Q11_Answer.join(',') });
                        data.push({ Key: 'Content12', Value: Q12_Answer.join(',') });
                        //data.Content9 = that.Q9.answer;
                        //data.Content10 = that.Q10.answer;
                        //data.Content11 = Q11_Answer.join(',');
                        //data.Content12 = Q12_Answer.join(',');
                        //#endregion

                        //#region 13-16题
                        data.push({ Key: 'Content13', Value: that.Q13.answer });
                        data.push({ Key: 'Content14', Value: that.Q14.answer });
                        data.push({ Key: 'Content15', Value: Q15_Answer.join(',') });
                        data.push({ Key: 'Content16', Value: Q16_Answer.join(',') });
                        //data.Content13 = that.Q13.answer;
                        //data.Content14 = that.Q14.answer;
                        //data.Content15 = Q15_Answer.join(',');
                        //data.Content16 = Q16_Answer.join(',');
                        //#endregion

                        //#region 17-20题
                        data.push({ Key: 'Content17', Value: that.Q17.answer });
                        data.push({ Key: 'Content18', Value: that.Q18.answer });
                        data.push({ Key: 'Content19', Value: Q19_Answer.join(',') });
                        data.push({ Key: 'Content20', Value: Q20_Answer.join(',') });
                        //data["Content17"] = that.Q17.answer;
                        //data["Content18"] = that.Q18.answer;
                        //data["Content19"] = Q19_Answer.join(',');
                        //data["Content20"] = Q20_Answer.join(',');
                        //#endregion

                        //#region 21-24题
                        data.push({ Key: 'Content21', Value: that.Q21.answer });
                        data.push({ Key: 'Content22', Value: that.Q22.answer });
                        data.push({ Key: 'Content23', Value: Q23_Answer.join(',') });
                        data.push({ Key: 'Content24', Value: Q24_Answer.join(',') });
                        //data["Content21"] = that.Q21.answer;
                        //data["Content22"] = that.Q22.answer;
                        //data["Content23"] = Q23_Answer.join(',');
                        //data["Content24"] = Q24_Answer.join(',');
                        //#endregion

                        //#region 25-26题
                        data.push({ Key: 'Content25', Value: that.Q25.answer.join(',') });
                        data.push({ Key: 'Content26', Value: Q26_Answer });
                        //data["Content25"] = that.Q25.answer.join(',');
                        //data["Content26"] = Q26_Answer;
                        //#endregion


                        //#region 29-31题
                        data.push({ Key: 'Content29', Value: that.Q29.hospital });
                        data.push({ Key: 'Content30', Value: that.Q30.name });
                        data.push({ Key: 'Content31', Value: that.Q31.mobile });
                        //#endregion

                        if (that.Q26.answer === '是') {
                            //#region 27题答案处理
                            var Q27_Answer = null;
                            var Q27_checked = that.Q27.options.filter(function (item, index) {
                                return item.value === that.Q27.answer;
                            });
                            Q27_Answer = that.Q27.answer + Q27_checked[0].rate + '%';
                            //#endregion

                            //#region 27-28题
                            data.push({ Key: 'Content27', Value: Q27_Answer });
                            data.push({ Key: 'Content28', Value: that.Q28.answer.join(',') });
                            //#endregion

                        }
                        else {
                            //#region 27-28题
                            data.push({ Key: 'Content27', Value: null });
                            data.push({ Key: 'Content28', Value: null });
                            //#endregion
                        }

                        that.page_loading = weui.loading('正在提交');
                        axios.post('/Survey/Issue_OnSubmit', data).then(res => {
                            if (that.page_loading) {
                                setTimeout(function () {
                                    that.page_loading.hide(function () {
                                        that.page_loading = null;
                                    });
                                }, 50);
                            }

                            if (res.status === 200) {
                                if (res.data.status) {
                                    // 转至成功页面
                                    that.isSubmit = true;
                                    setTimeout(function () {
                                        that.mySwiper.updateSlides();
                                    }, 50);
                                }
                                else {
                                    weui.alert(res.data.message);
                                }
                            }
                        }).catch(ex => {
                            if (that.page_loading) {
                                setTimeout(function () {
                                    that.page_loading.hide(function () {
                                        that.page_loading = null;
                                    });
                                }, 50);
                            }
                            weui.alert('提交失败');
                        });
                    }
                    else {
                        if (that.page_loading) {
                            setTimeout(function () {
                                that.page_loading.hide(function () {
                                    that.page_loading = null;
                                });
                            }, 50);
                        }
                        weui.topTips('请完善答题信息');
                    }
                });
            });
        },
        isWeiXin() {
            if (typeof window.WeixinJSBridge === 'undefined' || typeof window.WeixinJSBridge.invoke === 'undefined') {
                return false;
            }
            else {
                return true;
            }
        },
        onClosePage() {
            var that = this;
            if (that.isWeiXin()) {
                WeixinJSBridge.call('closeWindow');
            }
            else {
                window.location.href = "about:blank";
            }
        }
    }
});