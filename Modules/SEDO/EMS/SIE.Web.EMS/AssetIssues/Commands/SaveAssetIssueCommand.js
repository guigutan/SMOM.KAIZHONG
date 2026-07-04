SIE.defineCommand('SIE.Web.EMS.AssetIssues.Commands.SaveAssetIssueCommand', {
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
        var equipChildView = view.findChild('SIE.EMS.AssetIssues.AssetIssueEquipment');
        var fixtureChildView = view.findChild('SIE.EMS.AssetIssues.AssetIssueFixture');
        var selections = entity.data.AssetObject == 10 ? equipChildView.getSelection() : fixtureChildView.getSelection();

        if (selections.length == 0) {
            SIE.Msg.showError(entity.data.AssetObject == 10 ? '设备清单没有选中的数据！'.t() : '工治具清单没有选中的数据！'.t());
            return false;
        }

        if (entity.data.AssetObject == 10) {
            var equipIdArr = [];
            for (var i = 0; i < selections.length; i++) {
                if (selections[i].data.EquipAccountId == null) {
                    SIE.Msg.showError(Ext.String.format('行号【{0}】设备编码不能为空！'.t(), selections[i].data.LineNo));
                    return false;
                }
                else {
                    if (equipIdArr.indexOf(selections[i].data.EquipAccountId) > -1) {
                        SIE.Msg.showError(Ext.String.format('设备编码【{0}】重复选择！'.t(), selections[i].data.EquipAccountId_Display));
                        return false;
                    }
                    equipIdArr.push(selections[i].data.EquipAccountId);
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
                if (selections[i].data.Qty < 1 || selections[i].data.QualityStatus == null || selections[i].data.QualityStatus == 0 || selections[i].data.StorageLocationId == null) {
                    SIE.Msg.showError(Ext.String.format('行号【{0}】工治具的发放数量、质量状态、发放库位不能为空！'.t(), selections[i].data.LineNo));
                    return false;
                }
                else {
                    if (selections[i].data.NotPickQty < selections[i].data.Qty) {
                        SIE.Msg.showError(Ext.String.format('行号【{0}】工治具的发放数量大于未发放数量！'.t(), selections[i].data.LineNo));
                        return false;
                    }

                    if (selections[i].data.ManageMode == 5) {

                        if (fixtureIdArr.indexOf(selections[i].data.FixtureAccountId) > -1) {
                            SIE.Msg.showError(Ext.String.format('序列号【{0}】重复选择！'.t(), selections[i].data.FixtureAccountId_Display));
                            return false;
                        }
                        fixtureIdArr.push(selections[i].data.FixtureAccountId);
                    }
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
                CRT.Event.fire("SIE.EMS.AssetIssues.AssetIssue_refresh");
            }
        });
    }
});