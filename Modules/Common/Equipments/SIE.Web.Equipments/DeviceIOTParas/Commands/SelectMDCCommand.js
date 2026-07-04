SIE.defineCommand('SIE.Web.Equipments.DeviceIOTParas.Commands.SelectMDCCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'Id',
            targetClassName: 'SIE.Equipments.DeviceIOTParas.ViewModles.MDCDetailViewModle',
        },
    },
    canExecute: function (view) {
        return true;
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = me._targetSelectItems.items;

        if (selections && selections.length == 1) {

            var operationDatas = [];

            var equipmentCode = this._targetSelectItems.items[0].getEquipmentCode();

            //获取要填充的数据
            var items = [];
            SIE.invokeDataQuery({
                type: "SIE.Web.Equipments.DeviceIOTParas.DataQuerys.DeviceIOTParaDataQuery",
                method: "GetEquipmentTags",
                params: [equipmentCode],
                async: false,
                token: me._targetView.token,
                callback: function (res) {
                    if (res.Success) {
                        var equipmentTags = JSON.parse(res.Result);

                        equipmentTags.forEach(function (equipmentTag) {
                            var newItem = {};
                            newItem.MDCVariableName = equipmentTag.Name;
                            newItem.PararCode = equipmentTag.Name;
                            newItem.MaxValue = equipmentTag.MaxValue;
                            newItem.MinValue = equipmentTag.MinValue;
                            newItem.MDCVariableCode = equipmentCode;
                            newItem.From = 2;

                            items.push(newItem);
                        });
                    }
                }
            });

            //填充数据
            SIE.each(items, function (item) {
                var DeviceIOTParaId = me._ownerView.getParent().getCurrent().getId();
                var MDCVariableName = item.MDCVariableName;
                var PararCode = item.PararCode;
                var MaxValue = item.MaxValue;
                var MinValue = item.MinValue;
                var MDCVariableCode = MDCVariableCode;
                var From = 2;

                if (me._sourceViewSelectItems.indexOf(id) === -1) {
                    var facilityDetail = {
                        DeviceIOTParaId: DeviceIOTParaId,
                        MDCVariableName: MDCVariableName,
                        PararCode: PararCode,
                        MaxValue: MaxValue,
                        MinValue: MinValue,
                        MDCVariableCode: MDCVariableCode,
                        From: From
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
        else if (selections.length > 1) {
            SIE.Msg.showMessage("请选择一条记录".t());
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    },
    /// <summary>
    /// 加载弹窗视图的数据
    /// </summary>
    _reloadTargetViewData: function () {
        var me = this;
        me._sourceViewSelectItems = this.cloneStore.collect(me.dataParams.specKeyPrototyName);

        var dialogView = me._targetView;
        if (me._targetView !== null) {
            var store = dialogView.getData();
            if (store !== null) {
                me.mon(store, 'load', me.onLoad, this);
                if (dialogView._relations[0]) {
                    //存在查询面板时
                    var clearCM = me._targetView.getConditionView().getCmdControl("SIE.cmd.ClearCondition");
                    clearCM.setHidden(true);
                    var cmds = me._targetView.getConditionView().getCommands();
                    cmds.items.splice(cmds.items.indexOf(clearCM, 1));
                    cmds.keys.splice(cmds.keys.indexOf("SIE.cmd.ClearCondition", 1));

                    var criteria = dialogView._relations[0]._target.getData();
                    var equipModelCode = this._ownerView.getParent().getCurrent().getEquipModelId_Display()//查询条件

                    criteria.setMesModel(equipModelCode);
                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
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
    _gridBlockProcess: function (block) {

        var me = this;

        var gridConfig = block.gridConfig || block.mainBlock.gridConfig;
        gridConfig.selModel = {
            injectCheckbox: 0, //checkbox位于哪一列，默认值为0
            selType: 'checkboxmodel', //checkbox
            checkOnly: true, //只能通过checkbox选择
            mode: ('SINGLE'), //是否多选
        };

        gridConfig.viewConfig = {
            enableTextSelection: true, //启用文本选中
            getRowClass: function (record, index, rowParams, store) {
                var rowClass = me.getRowClass(record, index, rowParams, store);
                if (rowClass) return rowClass;//如果重写了getRowClass方法，返回重写的样式
                if (me._sourceViewSelectItems && me._sourceViewSelectItems.length > 0) {
                    if (Ext.Array.contains(me._sourceViewSelectItems, record.getId())) {
                        return 'gridRowLock'; //添加自定义样式
                    }
                }
            }
        };
    },
});