SIE.defineCommand('SIE.Web.RedCardManagment.RedCardApplyBills.Commands.AddRedCardApplyBillCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加".t(), group: "edit", },
    execute: function (view, source) {
        var me = this;
        var indata = {};
        var entity = me.getEditEntity();
        indata.Data = Ext.encode(entity.data);
        view.execute({
            data: indata,
            success: function (res) {
                CRT.Workbench.addPage({
                    entityType: me.view.model,
                    recordId: entity.data.Id,
                    title: me.getEditViewTitle(entity),
                    isDetail: true,
                    params: {
                        IsNew: true,
                        No: res.Result.No,
                        ApplyType: res.Result.ApplyType,
                        ApplySource: res.Result.ApplySource,
                    }
                });
            }
        });
    },
});