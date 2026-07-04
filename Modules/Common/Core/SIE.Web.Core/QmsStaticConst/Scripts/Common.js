/**
 * d2*表类型
 */
Ext.define('SIE.Core.QmsStaticConst.StaticConstD2Type', {
    statics: {
        d2: 0,
        cd: 1,
        V: 2,
        D2s: 3,
    }
});

Ext.define('SIE.Static.ControlCharts.Helper', {
    statics: {
        /**
         * 四舍五入
         * @param {any} num 数值
         * @param {any} fractiondigits  精度
         */
        round: function (num, fractiondigits) {
            if (isNaN(num))
                return "";
            var powDigit = Math.pow(10, fractiondigits);
            return Math.round(num * powDigit) / powDigit;
        }
    }
});