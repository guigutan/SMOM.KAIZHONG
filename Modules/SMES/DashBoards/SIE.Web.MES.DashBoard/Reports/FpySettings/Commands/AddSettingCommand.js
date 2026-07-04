SIE.defineCommand('SIE.Web.MES.DashBoard.Reports.FpySettings.Commands.AddSettingCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                success: function (res) {
                    var data = res.Result;
                    entity.setUpdateDate(data.UpdateDate);
                    entity.setUpdateBy(data.UpdateBy);
                }
            }, me.view);
        }
    },
});