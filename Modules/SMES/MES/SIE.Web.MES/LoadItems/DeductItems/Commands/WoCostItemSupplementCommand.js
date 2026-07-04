SIE.defineCommand('SIE.Web.MES.LoadItems.DeductItems.Commands.WoCostItemSupplementCommand', {
    meta: { text: "补扣", group: "edit", iconCls: "icon-RefreshData icon-blue" },
    canExecute: function (view) {
        var canExe = true;
        var dataList = view.getSelection();
        if (dataList.length === 0) {
            canExe = false;
        }
        dataList.forEach(entity => {
            //扣料状态 30 失败 10 待提交

            if (entity == null || (entity.getState() !== 30 && entity.getState() !== 10) || entity.getRecordType() !== 30) {
                canExe = false;
            }
        });
        
        return canExe;
    },
    execute: function (view) {
        var data = view.getCurrent().data;
        SIE.Msg.wait("正在补扣......".t());
        view.execute({
            data: data,
            withIds: true,
            selectIds: view.getSelectionIds(),
            success: function (res) {
                SIE.Msg.showMessage("补扣完成,成功" + res.Result.SuccessCount + "笔，失败" + res.Result.FailCount + "笔!".t());
                view.reloadData();
            }
        });
    }
});
