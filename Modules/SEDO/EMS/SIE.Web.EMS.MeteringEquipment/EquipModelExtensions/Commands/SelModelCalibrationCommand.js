SIE.defineCommand('SIE.Web.EMS.MeteringEquipment.EquipModelExtensions.Commands.SelModelCalibrationCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'InspectionRuleId', targetClassName: 'SIE.EMS.InspectionRules.InspectionRule' },
    },
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity == null || entity.data == null) {
            //所属父对象（检验规程）为空 或 数据为空时，不能点击选择检验项目
            return false;
        }
        //新增时，不能点击选择检验项目
        if (entity.isNew()) {
            return false;
        }
        return true;
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        /* post数据结构*/
        var indata = {};
        /* post数据结构*/
        var selections = me._targetSelectItems;
        if (selections && selections.items.length > 0) {
            var operationDatas = [];
            SIE.each(selections.items, function (item) {
                var itemId = item.getId();
                if (me._sourceViewSelectItems.indexOf(itemId) === -1) {
                    var equipModelRegularInspection = { EquipModelId: me._sourceId, InspectionRuleId: itemId };
                    operationDatas.push(equipModelRegularInspection);
                }
            });
            if (operationDatas.length > 0) {
                indata = operationDatas;
                me._targetView.execute({
                    data: indata,
                    success: function (res) {
                        win.close();  //关闭模态窗口
                        me._ownerView.loadChildData(true); //重载视图数据
                    }
                }, me._ownerView);
                return true;
            }
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    },
    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        me._sourceViewSelectItems = this.cloneStore.collect(me.dataParams.specKeyPrototyName);
        var dialogView = me._targetView;
        if (me._targetView !== null) {
            var store = dialogView.getData();
            if (store !== null) {
                me.mon(store, 'load', me.onLoad, this);
                if (dialogView._relations[0]) { //存在查询面板时
                    var clearCM = me._targetView.getConditionView().getCmdControl("SIE.cmd.ClearCondition");
                    clearCM.setHidden(true);
                    var cmds = me._targetView.getConditionView().getCommands();
                    cmds.items.splice(cmds.items.indexOf(clearCM, 1));
                    cmds.keys.splice(cmds.keys.indexOf("SIE.cmd.ClearCondition", 1));
                    var criteria = dialogView._relations[0]._target.getData().data;
                    criteria.InspectionRuleType = 20;
                    criteria.IsReadonly = true;
                    dialogView._relations[0]._target.tryExecuteQuery();

                    dialogView.loadData({
                        callback: function (res) {
                            var inspectionRuleTypeField = dialogView._relations[0]._target.getControl().getForm().findField("InspectionRuleType");
                            if (inspectionRuleTypeField) {
                                inspectionRuleTypeField.setReadOnly(true);
                            }
                        }
                    });
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    }
});