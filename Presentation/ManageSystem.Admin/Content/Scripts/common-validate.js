
// 使用js验证的相关公共函数，请勿随意修改


//验证正整数
function IsPositiveInteger(value)
{
    var reg= /^\+?[1-9][0-9]*$/;　　//正整数 
    return reg.test(value);
}    