//SIE.defineCommand('SIE.Web.Kit.APS.EngineerPlans.Commands.DoScheduleCommand', {
//    meta: { text: "排计划", group: "edit", iconCls: "icon-NetworkNormal icon-blue" },

//    canExecute: function (view) {
//        return true;
//    },
//    execute: function (view, source) {
//        var me = this;
//        me.win = SIE.Window.show({
//            title: '确认日期'.t(),
//            animateTarget: source,
//            items: [{
//                xtype: 'datefield',
//                anchor: '100%',
//                fieldLabel: '排程基准日期',
//                name: 'from_date',
//                dateFormat: Ext.Date.patterns.ISO8601Short,//'Y-m-d',
//                format: Ext.Date.patterns.ISO8601Short,//'Y-m-d',
//                //minValue: Ext.Date.add(new Date(), Ext.Date.DAY, -1),
//                maxValue: new Date()//Ext.Date.add(new Date(), Ext.Date.DAY, 1)
//            }],
//            width: 300,
//            height: 150,
//            modal: true,
//            buttons: ['排程', '取消'],
//            callback: function (btn) {
//                //console.log('aaaa');
//                if (btn != '排程') {
//                    me.win.close();
//                    return ;
//                }

//                var d = me.win.getComponent(0).getValue();
//                if (!d) {
//                    SIE.Msg.showInstantMessage("日期必须确认!".t());
//                    return false;
//                }
//                SIE.Msg.confirm("确定按[" + Ext.Date.format(d, Ext.Date.patterns.ISO8601Short) +"]作为基准日排MI吗？", function () {
//                    view.execute({
//                        data: { date:d},
//                        success: function (res) {           //回调
//                            SIE.Msg.showInstantMessage("排计划完成!".t());
//                            me.win.close();
//                            view.reloadData();
//                        }
//                    });
//                });
//            }
//        });
//    }
//});

SIE.defineCommand('SIE.Web.Kit.APS.EngineerPlans.Commands.DoScheduleCommand', {
    meta: { text: "排计划", group: "edit", iconCls: "icon-NetworkNormal icon-blue" },
    execute: function (view, source) {
        SIE.Msg.askQuestion('是否确认排计划'.t(), function () {
            SIE.Msg.wait("正在排计划中，请稍等...".t());
            view.execute({
                data: [],
                withIds: true,
                success: function (res) {
                    if (res.Success) {
                        SIE.Msg.showInstantMessage("排计划完成!".t());
                    }
                    view.reloadData();
                },
            });
        })
    }
});

