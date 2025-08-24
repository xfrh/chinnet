
// 使用js验证的相关公共函数，请勿随意修改

//验证是否为空或null；
function IsEmpty(value) {
    return (value === null || value === "");
}

//验证正整数
function IsPositiveInteger(value) {
    var reg = /^\+?[1-9][0-9]*$/;　　//正整数 
    return reg.test(value);
}

//验证数字
function IsNumber(value) {
    var reg = /^\+?[0-9][0-9]*$/;　　//正整数 
    return reg.test(value);
}

//是否是手机号码
function IsPhoneNumber(value) {
    var reg = /^[1]{1}[0-9]{10}$/;　　//手机号码
    return reg.test(value);
}

//是否是邮箱地址
function IsEmail2(value) {
    var reg = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$/;
    return reg.test(value);
}

// https://jqueryvalidation.org/email-method/
IsEmail = function (value) {

    // From https://html.spec.whatwg.org/multipage/forms.html#valid-e-mail-address
    // Retrieved 2014-01-14
    // If you have a problem with this implementation, report a bug against the above spec
    // Or use custom methods to implement your own email validation
    return /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/.test(value);
};

/**
 * 判断是不是手机号
 * @param {any} value 手机号
 * @returns {boolean} 返回判断结果
 */
function IsMobileNumber(value) {
    return value && /^[1](([3|5|8][\d])|([4][1,4,5,6,7,8,9])|([6][5,6])|([7][3,4,5,6,7,8])|([9][8,9]))[\d]{8}$/.test(value);
}