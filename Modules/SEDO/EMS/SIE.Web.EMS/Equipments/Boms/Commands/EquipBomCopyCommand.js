SIE.defineCommand('SIE.Web.EMS.Equipments.Boms.Commands.EquipBomCopyCommand', {
    meta: { text: "复制备件", group: "edit", iconCls: "iconfont icon-ContentCopy icon-green"},
    canExecute: function (view) {
        var selection = view.getSelection();
        var flag = true;
        if (selection == null || selection.length <= 0) {
            return false;
        }
        selection.forEach(item => {
            if (item.isDirty()) {
                flag = false;
                return;
            }
        });
        return flag;
    },
    execute: function (view) {
        var me = this;
        var selectIds = view.getSelectionIds();
        SIE.AutoUI.getMeta({
            model: 'SIE.EMS.Equipments.Boms.EquipBomSelect',
            viewGroup:"ShowSelectViewStr",
            ignoreChild: true,
            ignoreCommands: false,
            isReadonly: false,
            ignoreQuery: false,
            isAggt: true,
            callback: function (res) {
                var blocks = res;
                blocks.mainBlock.gridConfig.selModel = {
                    selType: 'checkboxmodel',
                    mode: 'SINGLE',
                };
                var ui = SIE.AutoUI.generateAggtControl(blocks);
                var listView = ui.getView();
                me.setQueryCriteria(listView, selectIds);
                var filter = {
                    Method: 'GetEquipBomsExceptIds',
                    Parameters: [selectIds]
                };
                filter = Ext.encode(filter);
                listView.loadData({
                    filter: filter,
                    action: 'queryer',
                    token: me.token,
                    type: 'SIE.Web.EMS.Equipments.Boms.EquipBomDataQueryer',
                });
                var items = ui.getControl();
                var win = SIE.Window.show({
                    title: "选择复制的设备Bom".t(),
                    width: 900,
                    height: 520,
                    items: items,
                    callback: function (btn) {
                        if (btn === "确定".t()) {
                            var sel = listView.getSelectedEntities();
                            var itemList = [];
                            sel.forEach(p => {
                                itemList.push(p.getData().Id);
                            });
                            if (itemList.length <= 0) {
                                SIE.Msg.showMessage("没有可提交的数据".t());
                                return;
                            }
                            var data = {
                                NewAddCopyIds: selectIds,
                                CopyDataSourceId: itemList[0]
                            }
                            view.execute({
                                data: data,
                                success: function (res) { //回调
                                    view.reloadData();
                                }
                            })
                        }
                    }
                })
            }
        });
    },
    //设置查询条件
    setQueryCriteria: function (dialogView, selectIds) {
        var criteria = dialogView._relations[0]._target.getData();
        if (criteria) {
            criteria.setExceptIds(selectIds);
        }
    },
})