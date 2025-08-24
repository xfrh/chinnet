

/**
 * 判断是否移动端
 * @method 移动端检测
 * @return {Boolean} 是移动端返回true,非移动端返回false
 */
function isMobile() {
    try {
        document.createEvent("TouchEvent");
        return true;
    }
    catch (e) {
        return false;
    }
}

if (isMobile()) {
    location.href = 'http://m.chinets.com';
}