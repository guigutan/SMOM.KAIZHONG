SIE.defineCommand('SIE.Web.Fixtures.Repairs.Commands.AddFixtureRepairCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    /**
    * @override 执行
    * @param {} view 视图
    * @param {} source 
    * @returns {} 
    */
    execute: function (view, source) {
        var me = this;
        var editEntity = this.getEditEntity();
        view.execute({
            data: { Id: 0 },
            success: function (res) {
                if (res.Success) {
                    if (res.Result.errMsg) {
                        SIE.Msg.showWarning(res.Result.errMsg);
                    }
                    else {
                        res.Result.Id = editEntity.data.Id;
                        CRT.Workbench.addPage({
                            entityType: me.view.model,
                            recordId: editEntity.data.Id,
                            title: me.getEditViewTitle(editEntity),
                            isDetail: true,
                            params: {
                                No: res.Result.data.No,
                                RepairState: res.Result.data.RepairState,
                                ApplyById: res.Result.data.ApplyById,
                                ApplyDate: res.Result.data.ApplyDate,
                                ApplyByName: res.Result.data.ApplyByName,
                            }
                        });
                    }
                }
            }
        });
    }
});