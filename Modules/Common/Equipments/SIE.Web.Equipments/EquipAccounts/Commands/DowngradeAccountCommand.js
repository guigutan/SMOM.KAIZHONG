SIE.defineCommand('SIE.Web.Equipments.EquipAccounts.Commands.DowngradeAccountCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "降级", group: "business", iconCls: "icon-Download icon-green" },

    canExecute: function (view) {
        var account = view.getCurrent();
        if (account == null) {
            return false;
        }

        var accountList = view.getSelection();
        if (accountList == null || accountList.length > 1) {
            return false;
        }

        return true;
    },
    execute: function (view, source) {
        var account = view.getCurrent();

        SIE.AutoUI.getMeta({
            model: 'SIE.Web.EMS.Equipments.Accounts.ViewModels.DowngradeAccountViewModel',
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = new detailView._model();
                entity.setCurrentAccountId(account.data.Id);
                detailView.setData(entity);
                var ui = detailView.getControl();

                var win = SIE.Window.show({
                    title: "选择降级到的目标设备台账".t(),
                    width: 500,
                    height: 200,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {

                            var data = view.getCurrent().data;
                            if (entity.data.EquipAccountId == null)
                                SIE.Msg.showMessage("目标父台账不能为空".t());
                            else
                                SIE.Msg.askQuestion(Ext.String.format('确定[降级]设备台账[{0}]?'.t(), data.Code), function () {

                                    view.execute({
                                        data: entity.data.EquipAccountId,//父台账ID
                                        withIds: true,
                                        selectIds: view.getSelectionIds(),//选中台账ID
                                        success: function (res) { //回调
                                            win.close();
                                            view.reloadData();
                                        }
                                    });

                                });

                            //返回false，确定后不关闭窗口
                            return false;
                        }
                    }
                });
            },
        });
    }
});