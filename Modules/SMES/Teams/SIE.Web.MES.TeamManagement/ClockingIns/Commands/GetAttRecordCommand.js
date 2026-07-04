SIE.defineCommand('SIE.Web.MES.TeamManagement.ClockingIns.Commands.GetAttRecordCommand', {
    meta: { text: "获取打卡记录", group: "edit", iconCls: "icon-BatchCheck icon-blue" },
    execute: function (view, source) {
        var me = view;
      
            view.execute({
                data: [],
                success: function (res) {
                    var data = res.Result;
                    SIE.Msg.showMessage(data);
                }
            });
        
    }
});