
Ext.define('SIE.Web.EMS.Checks.Plans.Scripts.BatchAddCheckPlanBehavior',
    {
        /**
        * view生命周期函数--view聚合后
        * @param {*} view 生成的view
        */
        onViewReady: function (view) {
            var me = this;
            //设置主表model
            var entity = new view._model();
            view.setData(entity);
            var entity = view.getCurrent();
            view.mon(entity, 'propertyChanged', function (e) {
                //1.0 添加周期类型-值改变监听
                if (e.property == "CheckCycleType") {
                    me.checkCycleTypeValueChange(e, view);
                }
            }, view);
        },
        checkCycleTypeValueChange: function (e, view) {
            var me = this;
            me.setDateTimecontrol(e, view);
            //清空子列表数据
            var children = view.getChildren();
            var items = [];
            children[0].getData().setData(items);//周期类型有变化要清掉台账
        },
        setDateTimecontrol: function (e, view) {
            var ctr = view.getControl();
            var beginDay = ctr.down('[name=CheckBeginDate]');
            var endDay = ctr.down('[name=CheckEndDate]');
            var whetherAcrossDay = ctr.down('[name=WhetherAcrossDay]');
            var entity = e.entity;

            beginDay.setReadOnly(0);
            endDay.setReadOnly(0);
            whetherAcrossDay.setReadOnly(0);

            if (e.value == 1) {   //白班
                entity.setCheckBeginDate(new Date());
                entity.getCheckBeginDate().setHours(7);
                entity.getCheckBeginDate().setMinutes(0);
                entity.setCheckEndDate(new Date());
                entity.getCheckEndDate().setHours(19);
                entity.getCheckEndDate().setMinutes(0);
                entity.setWhetherAcrossDay(0);
                beginDay.setReadOnly(1);
                endDay.setReadOnly(1);
                whetherAcrossDay.setReadOnly(1);
            }
            if (e.value == 2) {   //晚班
                entity.setCheckBeginDate(new Date());
                entity.getCheckBeginDate().setHours(19);
                entity.getCheckBeginDate().setMinutes(0);
                entity.setCheckEndDate(new Date());
                entity.getCheckEndDate().setHours(7);
                entity.getCheckEndDate().setMinutes(0);
                entity.setWhetherAcrossDay(1);
                beginDay.setReadOnly(1);
                endDay.setReadOnly(1);
                whetherAcrossDay.setReadOnly(1);
            }
        },
    });