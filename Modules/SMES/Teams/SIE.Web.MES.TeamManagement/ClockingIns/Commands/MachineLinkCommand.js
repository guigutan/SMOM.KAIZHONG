SIE.defineCommand('SIE.Web.MES.TeamManagement.ClockingIns.Commands.MachineLinkCommand', {
    meta: { text: "测试连接", group: "edit", iconCls: "icon-LinkVariant icon-blue" },
    canExecute: function (view) {
        return view.getCurrent() != null && view.getCurrent().data.IpAddress != null && view.getCurrent().data.IpAddress != "" && view.getCurrent().data.Port != null;
    },
    execute: function (view, source) {
        var me = view;
        var entity = view.getCurrent();
        if (entity) {
            Ext.MessageBox.show({
                msg: '正在连接考勤机'.t(),
                progressText: '...',
                width: 300,
                modal:true,
                wait: {
                    interval: 200
                }
            });
            //me.timer = Ext.defer(function () {
            //    me.timer = null;
            //    Ext.MessageBox.hide();
            //}, 15000);
            view.execute({
                data: entity.data,
                success: function (res) {
                    var data = res.Result;
                    if (data != "" && data != null) {
                        entity.setModel(data.Model);
                        entity.setSN(data.SN);
                        SIE.Msg.showMessage('连接成功！'.t());
                    }
                    else SIE.Msg.showMessage('连接失败！'.t());
                }
            });
        }
    }
});