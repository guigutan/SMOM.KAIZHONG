Ext.define("SIE.Web.Inventory.Commom.DCInputAction", {
    statics: {
        onEntityPropertyChanged: function (e) {
            var me = this;
            var entity = e.entity;
            var data = e.entity.data;
            if (e.property.length > 0) {
                if (e.property == 'YearWeek' && data.YearWeek) {
                    if (data.Input.length == 4 && !isNaN(data.Input)) {
                        var defaultYear, defaultWeek;
                        defaultYear = '20' + data.Input.substring(0, 2);
                        defaultWeek = data.Input.substring(2, 4);
                        if (defaultWeek == '01')
                            entity.setTransform(defaultYear + '-01-01');
                        else if (defaultWeek == '00' || parseInt(defaultWeek) > 54)
                            entity.setTransform(null);
                        else {
                            var weekRange = SIE.Web.Inventory.Commom.DCInputAction.getYearWeekRange(defaultYear, defaultWeek);
                            entity.setTransform(weekRange[2]);
                        }
                    } else
                        entity.setTransform(null);
                }
                if (e.property == 'WeekYear' && data.WeekYear) {
                    if (data.Input.length == 4 && !isNaN(data.Input)) {
                        var defaultYear, defaultWeek;
                        defaultYear = '20' + data.Input.substring(2, 4);
                        defaultWeek = data.Input.substring(0, 2);
                        if (defaultWeek == '01')
                            entity.setTransform(defaultYear + '-01-01');
                        else if (defaultWeek == '00' || parseInt(defaultWeek) > 54)
                            entity.setTransform(null);
                        else {
                            var weekRange = SIE.Web.Inventory.Commom.DCInputAction.getYearWeekRange(defaultYear, defaultWeek);
                            entity.setTransform(weekRange[2]);
                        }
                    } else
                        entity.setTransform(null);
                }
                if (e.property == 'YearMonthDay' && data.YearMonthDay) {
                    if (data.Input.length == 6 && !isNaN(data.Input)) {
                        var transformValue = SIE.Web.Inventory.Commom.DCInputAction.getYearMonthDay(data.Input);
                        entity.setTransform(transformValue);
                    } else
                        entity.setTransform(null);
                }
                if (e.property == 'Input' && data.YearWeek) {
                    if (data.Input.length == 4 && !isNaN(data.Input)) {
                        var defaultYear, defaultWeek;
                        defaultYear = '20' + data.Input.substring(0, 2);
                        defaultWeek = data.Input.substring(2, 4);
                        if (defaultWeek == '01')
                            entity.setTransform(defaultYear + '-01-01');
                        else if (defaultWeek == '00' || parseInt(defaultWeek) > 54)
                            entity.setTransform(null);
                        else {
                            var weekRange = SIE.Web.Inventory.Commom.DCInputAction.getYearWeekRange(defaultYear, defaultWeek);
                            entity.setTransform(weekRange[2]);
                        }
                    } else
                        entity.setTransform(null);
                }
                if (e.property == 'Input' && data.WeekYear) {
                    if (data.Input.length == 4 && !isNaN(data.Input)) {
                        var defaultYear, defaultWeek;
                        defaultYear = '20' + data.Input.substring(2, 4);
                        defaultWeek = data.Input.substring(0, 2);
                        if (defaultWeek == '01')
                            entity.setTransform(defaultYear + '-01-01');
                        else if (defaultWeek == '00' || parseInt(defaultWeek) > 54)
                            entity.setTransform(null);
                        else {
                            var weekRange = SIE.Web.Inventory.Commom.DCInputAction.getYearWeekRange(defaultYear, defaultWeek);
                            entity.setTransform(weekRange[2]);
                        }
                    } else
                        entity.setTransform(null);
                }
                if (e.property == 'Input' && data.YearMonthDay) {
                    if (data.Input.length == 6 && !isNaN(data.Input)) {
                        var transformValue = SIE.Web.Inventory.Commom.DCInputAction.getYearMonthDay(data.Input);
                        entity.setTransform(transformValue);
                    } else
                        entity.setTransform(null);
                }
            }
        },
        getYearWeekRange: function (year, weekNum) {
            var year = year;
            var d = null;
            var firstDay = null;

            var weekDay = SIE.Web.Inventory.Commom.DCInputAction.getDayEveryDay(year, weekNum);
            d = weekDay[0];//获取对应周数的第一天

            var firstWeekDay = SIE.Web.Inventory.Commom.DCInputAction.getDayEveryDay(year, "01");

            //计算当年的第一周有没有包含1月1日
            if (!firstWeekDay.contains(year + "-1-1")) {
                firstDay = SIE.Web.Inventory.Commom.DCInputAction.GetDateStr(-7, d);//当前日期前推7天的日期
            }
            else
                firstDay = d;//当前日期

            var weekRange = SIE.Web.Inventory.Commom.DCInputAction.getDateRange(firstDay);//常规的传入时间返回周的范围(周一到周天) return 格式["2016-12-26","2017-1-1"]

            if (firstDay.substring(0, 4) === year) {
                weekRange[0] = SIE.Web.Inventory.Commom.DCInputAction.GetDateStr(0, weekRange[0]);
                weekRange[1] = SIE.Web.Inventory.Commom.DCInputAction.GetDateStr(0, weekRange[1]);
            } else {
                weekRange[0] = null;
                weekRange[1] = null;
            }


            //返回当前日期为[年，周数，周的范围start,周的范围end],按照周一到周日为一周
            return [year, weekNum, weekRange[0], weekRange[1]];
        },
        GetDateStr: function (AddDayCount, date) {
            var dd = new Date(date);
            dd.setDate(dd.getDate() + AddDayCount);//获取AddDayCount天后的日期
            var y = dd.getFullYear();
            var m = (dd.getMonth() + 1) < 10 ? "0" + (dd.getMonth() + 1) : (dd.getMonth() + 1);//获取当前月份的日期，不足10补0
            var d = dd.getDate() < 10 ? "0" + dd.getDate() : dd.getDate();//获取当前几号，不足10补0
            return y + "-" + m + "-" + d;
        },
        /*
        *这个方法是获取周对应的日期范围(常规的一周为周一到周天为一周
        * 参数datevalue如：2017-01-01)
        */
        getDateRange: function (datevalue) {
            var dateValue = datevalue;
            var arr = dateValue.split("-")
            //月份-1 因为月份从0开始 构造一个Date对象
            var date = new Date(arr[0], arr[1] - 1, arr[2]);

            var dateOfWeek = date.getDay();//返回当前日期的在当前周的某一天（0～6--周日到周一）

            var dateOfWeekInt = parseInt(dateOfWeek, 10);//转换为整型

            if (dateOfWeekInt == 0) {//如果是周日
                dateOfWeekInt = 7;
            }
            var aa = 7 - dateOfWeekInt;//当前于周末相差的天数

            var temp2 = parseInt(arr[2], 10);//按10进制转换，以免遇到08和09的时候转换成0
            var sunDay = temp2 + aa;//当前日期的周日的日期
            var monDay = sunDay - 6//当前日期的周一的日期

            var startDate = new Date(arr[0], arr[1] - 1, monDay);
            var endDate = new Date(arr[0], arr[1] - 1, sunDay);

            var sm = parseInt(startDate.getMonth()) + 1;//月份+1 因为月份从0开始
            var em = parseInt(endDate.getMonth()) + 1;

            var start = startDate.getFullYear() + "-" + sm + "-" + startDate.getDate();
            var end = endDate.getFullYear() + "-" + em + "-" + endDate.getDate();
            var result = new Array();
            result.push(start);
            result.push(end);

            return result;
        },
        /*
        *传入年，周数，获取周数对应的所有日期
         */
        getDayEveryDay: function (year, index) {
            var d = new Date(year, 0, 1);
            while (d.getDay() != 1) {
                d.setDate(d.getDate() + 1);
            }
            var to = new Date(year + 1, 0, 1);
            var i = 1;
            var arr = [];
            for (var from = d; from < to;) {
                if (i == index) {
                    arr.push(from.getFullYear() + "-" + (from.getMonth() + 1) + "-" + from.getDate());
                }
                var j = 6;
                while (j > 0) {
                    from.setDate(from.getDate() + 1);
                    if (i == index) {
                        arr.push(from.getFullYear() + "-" + (from.getMonth() + 1) + "-" + from.getDate());
                    }
                    j--;
                }
                if (i == index) {
                    return arr;
                }
                from.setDate(from.getDate() + 1);
                i++;
            }
        },
        //选择年月日时间格式转换
        getYearMonthDay: function (inputValue) {
            var year, month, day, transformValue;
            year = '20' + inputValue.substring(0, 2);
            month = inputValue.substring(2, 4);
            if (parseInt(month) > 12)
                transformValue = null;
            else {
                day = inputValue.substring(4, 6);
                var MonthDays = SIE.Web.Inventory.Commom.DCInputAction.getMonthDays(year, month);
                if (parseInt(day) > parseInt(MonthDays))
                    transformValue = null;
                else
                    transformValue = year + '-' + month + '-' + day;
            }
            return transformValue;
        },
        //获取某一年份的某一月份的天数
        getMonthDays: function (year, month) {
            var d = new Date(year, month, 0);
            return d.getDate(); 
        },
        getInvalidDate: function (startDate, days) {
            var startTime = new Date(startDate).getTime();
            var diff = days * 86400 * 1000;
            var endTime = startTime + diff;
            var d = new Date(endTime);
            var CurrentDate = "";
            CurrentDate += (d.getFullYear());
            if ((d.getMonth() + 1) > 9) {
                CurrentDate += "-" + (d.getMonth() + 1);
            }
            else {
                CurrentDate += "-0" + (d.getMonth() + 1);
            }
            if ((d.getDate()) > 9) {
                CurrentDate += "-" + (d.getDate());
            }
            else {
                CurrentDate += "-0" + (d.getDate());
            }
            return CurrentDate;
        }
    }
});