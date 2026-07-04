/**
* 重写Ext.calendar.view.Weeks
* 实现事件横向排布
*/
Ext.define('SIE.overrides.calendar.view.Weeks', {
    override: 'Ext.calendar.view.Weeks',
    requires: ['Ext.calendar.form.Edit', 'Ext.calendar.form.Add'],
    privates: {
        /**
         * 重写事件构建方法 
         */
        constructEvents: function () {
            var me = this, D = Ext.Date, daysInWeek = Ext.Date.DAYS_IN_WEEK, events = me.getEventSource().getRange(), len = events.length, visibleDays = me.getVisibleDays(), visibleWeeks = me.dateInfo.requiredWeeks, current = D.clone(me.dateInfo.visible.start), eventHeight = me.getEventStyle().fullHeight, maxEvents = Math.floor(me.getDaySizes().heightForEvents / eventHeight), overflow = me.getShowOverflow() === 'bottom', weeks = [], i, j, week, frag, event;
            me.weeks = weeks;
            frag = document.createDocumentFragment();
            for (i = 0; i < visibleWeeks; ++i) {
                week = new Ext.calendar.view.WeeksRenderer({
                    view: me,
                    start: current,
                    days: visibleDays,
                    index: i,
                    overflow: overflow,
                    maxEvents: 10   //超过这个就会自动隐藏，自己看情况设置
                });
                for (j = 0; j < len; ++j) {
                    event = events[j];
                    if (!me.isEventHidden(event)) {
                        week.addIf(event);
                    }
                }
                if (week.hasEvents()) {
                    week.calculate();
                }
                me.processWeek(week, frag);
                weeks.push(week);
                current = D.add(current, D.DAY, daysInWeek, true);
            }
            me.element.appendChild(frag);
        },

        /** 
         * 重写获取事件样式方法
         * @returns {style} style
         * */
        getEventStyle: function () {
            var me = this, eventStyle = me.eventStyle, fakeEvent, el, margin, height;
            if (!eventStyle) {
                fakeEvent = me.createEvent(null, null, true);
                el = fakeEvent.element;
                el.dom.style.visibility = 'hidden';
                me.element.appendChild(el);
                height = el.getHeight();
                margin = el.getMargin();
                margin.height = margin.top + margin.bottom;
                margin.width = margin.left + margin.right;
                me.eventStyle = eventStyle = {
                    margin: margin,
                    height: height,
                    fullHeight: height + margin.height
                };
                fakeEvent.destroy();
            }
            return eventStyle;
        },

        /**
        * 重写处理周方法
        * @param {ListLogicalView} week 周
        * @param {ListLogicalView} frag append事件的fragment
        */
        processWeek: function (week, frag) {
            var me = this,
                rows = week.rows,
                days = week.days,
                overflows = week.overflows,
                cellOffset = week.index * Ext.Date.DAYS_IN_WEEK,
                showOverflow = me.getShowOverflow(),
                cells = me.cells,
                overflowCls = me.$cellOverflowCls,
                overflowText = me.getOverflowText(),
                overflow, row, i, rowLen, j, item, widget, el, cell, len;

            if (rows) {
                for (i = 0, len = rows.length; i < len; ++i) {
                    row = week.compress(i);
                    for (j = 0, rowLen = row.length; j < rowLen; ++j) {
                        item = row[j];
                        if (!item.isEmpty) {
                            widget = me.createEvent(item.event);
                            el = widget.element;
                            el.dom.style.borderRadius = '4px';//设置事件背景圆角，自己看情况设置
                            el.dom.style.textAlign = 'center';
                            frag.appendChild(el.dom);
                            me.positionEvent(el, item, i);
                        }
                    }
                }
            }
            for (i = 0; i < days; ++i) {
                cell = cells[cellOffset + i];
                overflow = overflows && overflows[i];
                if (overflow && overflow.length && showOverflow) {
                    Ext.fly(cell).addCls(overflowCls);
                    cell.firstChild.lastChild.innerHTML = Ext.String.format(overflowText, overflow.length);
                } else {
                    Ext.fly(cell).removeCls(overflowCls);
                }
            }
        },

        /**
        * 重写事件位置方法
        * @param {ListLogicalView} el 元素
        * @param {ListLogicalView} item item
        * @param {ListLogicalView} dayEventsIdx 一天的事件索引
        */
        positionEvent: function (el, item, dayEventsIdx) {
            var me = this,
                daySizes = me.getDaySizes(),
                eventStyle = me.getEventStyle(),
                margin = eventStyle.margin,
                widths = daySizes.widths,
                start = item.start,
                idx = item.localIdx,
                weekIdx = item.weekIdx,
                headerOffset;
            var dayHeight = daySizes.heights[0];
            var dayWidth = daySizes.widths[0];
            var dayEventCount = item.event.data.Count;  //取event中的自定义数据字段
            if (dayEventCount === undefined || dayEventCount === 0)
                dayEventCount = 1;
            var horizontalMargin = margin.height + margin.width + (margin.left + margin.right) * (dayEventCount - 1);  //水平方向所有间距
            var width = (dayWidth - horizontalMargin) / dayEventCount;
            var height = dayHeight - daySizes.headerHeight - margin.top - margin.bottom;
            var top = me.positionSum(0, weekIdx, daySizes.heights) + 15;
            var left = me.positionSum(0, start, widths) + margin[me.startMarginName] + dayEventsIdx * width;
            if (dayEventsIdx > 0)
                left = left + dayEventsIdx * margin.width;
            headerOffset = daySizes.headerHeight + eventStyle.height * idx + (idx + 1) * margin.height;
            el.setTop(top);                    //每天的事件设置相同的上偏离,根据具体情况设置
            el.setLeft(left);          //每天的事件设置相应的左偏离，根据具体情况设置
            el.setWidth(width);
            el.setHeight(height);   //设置事件高度  
            el.dom.style.lineHeight = height + 'px';
        }
    }
}); 