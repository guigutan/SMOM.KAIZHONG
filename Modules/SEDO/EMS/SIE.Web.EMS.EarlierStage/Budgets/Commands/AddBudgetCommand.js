SIE.defineCommand('SIE.Web.EMS.EarlierStage.Budgets.Commands.AddBudgetCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    showView: function (editEntity) {
        var me = this;
        this.view.execute({
            data: [],
            success: function (res) {
                var info = res.Result;
                me.addPage({
                    entityType: me.view.model,
                    recordId: editEntity.getId(),
                    title: me.getEditViewTitle(editEntity),
                    isDetail: true,
                    isNew: true,
                    params: {
                        action: 0,
                        BudgetNo: info.BudgetNo,
                        Year: info.Year,
                        ApprovalStatus: info.ApprovalStatus,
                        Currency: info.Currency,
                        BudgeGrade: info.BudgeGrade
                    }
                });
            }
        }, me.view);
    }
});