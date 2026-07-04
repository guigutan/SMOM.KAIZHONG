/**
 * 排班导入命令 
 */
SIE.defineCommand('SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportCommandBase',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-blue" },

    /**
     * 指定导入模板生成方式
     */ 
    _downloadTemplateType: function () {
        this.BehaviorName = 'DownloadCustom';
    },

    /**
     * 导入成功后，刷新界面数据
     * @param view 排班表主视图
     */
    _importSuccess: function (view) {
        var queryView=view._relations[0];
        if (queryView) {
            var cmd = queryView._target.findCmd(SIE.Web.MES.TeamManagement.ShiftSchedules.ScheduleQuery);
            if (cmd)
                cmd.execute(queryView._target);
        }
    }
});