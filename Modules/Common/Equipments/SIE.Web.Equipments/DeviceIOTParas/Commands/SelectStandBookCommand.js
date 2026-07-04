SIE.defineCommand('SIE.Web.Equipments.DeviceIOTParas.Commands.SelectStandBookCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'EquipAccountId',
            targetClassName: 'SIE.EMS.Equipments.Accounts.AccountByModleType',
            targetCriteriaClassName: 'SIE.EMS.Equipments.Accounts.Criterias.AccountByModleTypeCriteria'
        },
    },
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();

        if (entity == null) {
            //所属父对象（设备型号）为空 不能点击选择
            return false;
        }

        if (entity.data === null || entity.data.INV_ORG_ID === null) {
            return false;
        }

        //新增时，不能点击选择校验项目
        if (entity.isNew()) {
            return false;
        }

        return true;
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = me._targetSelectItems.items;
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {


                var DeviceIOTParaId = me._ownerView.getParent().getCurrent().getId();
                var EquipAccountId = item.getId();
                var EquipmentName = item.getName();
                var UnitType = item.getModelCode();
                var ModelName = item.getModelName();
                var DeviceType = item.getEquipTypeName();
                var Workshop = item.getWorkShopName();
                var ProductLine = item.getResourceName();
                var Local = item.getInstallationLocation();

                if (me._sourceViewSelectItems.indexOf(id) === -1) {
                    var facilityDetail = {
                        DeviceIOTParaId: DeviceIOTParaId,
                        EquipAccountId: EquipAccountId,
                        EquipmentName: EquipmentName,
                        UnitType: UnitType,
                        ModelName: ModelName,
                        DeviceType: DeviceType,
                        Workshop: Workshop,
                        ProductLine: ProductLine,
                        Local: Local
                    };

                    operationDatas.push(facilityDetail);
                }
            });
            indata = operationDatas;
            me._targetView.execute({
                data: indata,
                success: function (res) {
                    win.close();
                    me._ownerView.loadChildData(true);
                }
            }, me._ownerView);
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.L10N());
        }
    },
    _popupWin: function (ui, source) {
        /// <summary>
        /// 弹窗口
        /// </summary>
        /// <param name="ui" type="type"></param>
        /// <param name="source" type="type"></param>
        var me = this;
        me._targetView = ui._view;
        if (me.win && me.win.animateTarget == source) {
            return;
        }
        //弹窗
        me.win = SIE.Window.show({
            title: '选择'.t()+" " + me._targetView.label.t(),
            animateTarget: source, items: ui.getControl(),
            width: 800, height: 420,
            listeners: {
                close: function () {
                    me.lastClickTime = 0;
                }
            },
            //buttons: ['确定', '关闭'], //自定义按钮名称
            callback: function (btn) {
                if (btn === '确定'.t()) {
                    var elapsed = Ext.now() - me.lastClickTime;
                    var interval = me.getExecuteInterval();
                    if (elapsed >= interval) {
                        me.lastClickTime = Ext.now();
                        if (me._targetSelectItems.keys.length > 0) {
                            me.save(me.win);
                            return false; //阻止窗口关闭，在save中根据返回结果处理
                        } else {
                            SIE.Msg.showWarning('没有可提交的数据'.t());   //没有选择数据点击确定时，窗口直接关闭了
                            return false;
                        }
                    }
                    return false;
                }
            }
        });
        SIE.Window.winAutoSize(me.win);
        me.setGridListeners();
        me._targetSelectItems = { items: [], keys: [] };
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


                    var criteria = dialogView._relations[0]._target.getData();
                    var EquipModelId = this._ownerView.getParent().getCurrent().getEquipModelId();//查询条件
                    var EquipModelId_Display = this._ownerView.getParent().getCurrent().getEquipModelId_Display()
                    criteria.setEquipModelId(EquipModelId);//设置查询条件
                    criteria.setEquipModelId_Display(EquipModelId_Display);
                    //criteria.setIsReadOnly(true);
                    //criteria.IsInvalid = true;
                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
});