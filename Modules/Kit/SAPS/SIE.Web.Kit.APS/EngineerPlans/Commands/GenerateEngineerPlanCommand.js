SIE.defineCommand('SIE.Web.Kit.APS.EngineerPlans.Commands.GenerateEngineerPlanCommand', {
    meta: { text: "同步工程计划", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    execute: function (view, source) {
        SIE.Msg.askQuestion('是否确认同步工程计划'.t(), function () {
            SIE.Msg.wait("正在同步工程计划中，请稍等...".t());
            view.execute({
                data: [],
                withIds: true,
                success: function (res) {
                    if (res.Success) {
                        var tempMsg = res.Result.replace(/\n/g, '<br/>');
                        if (tempMsg == "") {
                            tempMsg = "同步成功!";
                        }
                        SIE.Msg.showMessage(tempMsg);
                        view.reloadData();
                    }
                },
            });
        })
    }
});

