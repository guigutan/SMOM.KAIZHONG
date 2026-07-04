SIE.defineCommand('SIE.Web.EMS.AssetReturns.Commands.SaveAssetReturnCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit" },
    canExecute: function (view) {
        if (view.getCurrent() == null) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {

        var me = this;
        var entity = view.getCurrent();
        var equipChildView = view.findChild('SIE.EMS.AssetReturns.AssetReturnEquipment');
        var fixtureChildView = view.findChild('SIE.EMS.AssetReturns.AssetReturnFixture');
        var selections = entity.data.AssetObject == 10 ? equipChildView.getSelection() : fixtureChildView.getSelection();

        if (selections.length == 0) {
            SIE.Msg.showError(entity.data.AssetObject == 10 ? '设备清单没有选中的数据！'.t() : '工治具清单没有选中的数据！'.t());
            return false;
        }

        if (entity.data.AssetObject == 10) {

            for (var i = 0; i < selections.length; i++) {
                if (selections[i].data.ReturnType == null || selections[i].data.ReturnType == 0) {
                    SIE.Msg.showError(Ext.String.format('行号【{0}】归还类型不能为空！'.t(), selections[i].data.LineNo));
                    return false;
                }
                selections[i].data.IsSelected = true;
            }

            equipChildView.getData().getData().items.forEach(function (detail) {
                detail.dirty = true;
            });
        }

        if (entity.data.AssetObject == 20) {
            var fixtureIdArr = [];
            for (var i = 0; i < selections.length; i++) {
                if (selections[i].data.Qty < 1 || selections[i].data.ReturnType == null || selections[i].data.ReturnType == 0 ){
                    SIE.Msg.showError(Ext.String.format('行号【{0}】工治具的归还类型、归还数量不能为空！'.t(), selections[i].data.LineNo));
                    return false;
                }

                if (selections[i].data.ReturnType == 10 && (selections[i].data.QualityStatus == null || selections[i].data.QualityStatus == 0)) {
                    SIE.Msg.showError(Ext.String.format('行号【{0}】工治具的质量状态不能为空！'.t(), selections[i].data.LineNo));
                    return false;
                }

                if (selections[i].data.NotReturnQty < selections[i].data.Qty) {
                    SIE.Msg.showError(Ext.String.format('行号【{0}】工治具的归还数量大于未归还数量！'.t(), selections[i].data.LineNo));
                    return false;
                }

                selections[i].data.IsSelected = true;
            }

            fixtureChildView.getData().getData().items.forEach(function (detail) {
                detail.dirty = true;
            });
        }
        me.doSave(view);
    },
    onSaved: function (view, res) {
        var me = this;
        me.onSavedMsg();
        me.view.getCurrent().markSaved();
    },
    onSavedMsg: function (view, res) {

        Ext.Msg.show({
            title: '提示'.t(),
            message: '保存成功'.t(),
            buttons: Ext.MessageBox.OK,
            icon: Ext.Msg.INFO,
            callback: function () {
                CRT.Workbench.closeCurrentTab();
                CRT.Event.fire("SIE.EMS.AssetReturns.AssetReturn_refresh");
            }
        });
    }
});