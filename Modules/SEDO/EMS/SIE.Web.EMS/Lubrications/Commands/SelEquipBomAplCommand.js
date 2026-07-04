SIE.defineCommand('SIE.Web.EMS.Lubrications.Commands.SelEquipBomAplCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    meta: { text: "选择设备BOM", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'SparePartId',
            targetClassName: 'SIE.EMS.Equipments.Boms.EquipBomDetailSel'
        }
    },
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return entity.data.INV_ORG_ID !== null && (entity.getLubricationStatus() == 10 || entity.getLubricationStatus() == 20);
        }
        return false;
    },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = me._targetSelectItems.items;
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var sparePartId = item.getSparePartId();
                if (me._sourceViewSelectItems.indexOf(sparePartId) === -1) {
                    var lubricationSparePart = { LubricationId: me._sourceId, SparePartId: sparePartId };
                    operationDatas.push(lubricationSparePart);
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

                    //查询实体赋值
                    var modelCode = me.view.getParent().getCurrent().getEquipModelCode();
                    var criteria = dialogView._relations[0]._target.getData();
                    criteria.setModelCode(modelCode);
                    criteria.setIsReadOnly(true)

                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
    onLoad: function (store, records, successful, operation, eOpts) {
        /// <summary>
        /// 根据数据实现勾选上
        /// </summary>
        var me = this;
        if ((me._sourceViewSelectItems && me._sourceViewSelectItems.length > 0)
            || (me._targetSelectItems && me._targetSelectItems.items.length > 0)) {
            var selModel = me._targetView.getSelectionModel();
            if (records && records.length > 0) {
                for (var i = 0, len = records.length; i < len; i++) {
                    var record = records[i];
                    if (me._sourceViewSelectItems.indexOf(record.getSparePartId()) > -1) {
                        selModel.select(record, true, true); //勾选上.
                    }
                    if (me._targetSelectItems.keys.indexOf(record.getSparePartId()) > -1) {
                        selModel.select(record, true, true);
                    }
                }
            }
        }
    },
    onSelect: function (selModel, record, index, eOpts) {
        /// <summary>
        /// 选择事件
        /// </summary>
        /// <param name="selModel" type="Ext.selection.RowModel">选择模式</param>
        /// <param name="record" type="Ext.data.Model">选择的记录</param>
        /// <param name="index" type="Number">行索引号</param>
        /// <param name="eOpts" type="Object">The options object passed to Ext.util.Observable.addListener.</param>
        //console.log('勾选了checkbox后，获得选中行索引:' + index);
        var idx = Ext.Array.indexOf(this._targetSelectItems.keys, record.getSparePartId(), 0);
        if (idx === -1) {
            this._targetSelectItems.keys.push(record.getSparePartId());
            this._targetSelectItems.items.push(record);
        }
    },
    onBeforeDeselect: function (selModel, record, index, eOpts) {
        /// <summary>
        /// 取消选择前事件
        /// </summary>
        /// <param name="selModel" type="Ext.selection.RowModel">选择模式</param>
        /// <param name="record" type="Ext.data.Model">选择的记录</param>
        /// <param name="index" type="Number">行索引号</param>
        /// <param name="eOpts" type="Object">The options object passed to Ext.util.Observable.addListener.</param>
        if (record) {
            var idx = Ext.Array.indexOf(this._sourceViewSelectItems, record.getSparePartId(), 0);
            if (idx > -1) {
                return false;
            }
        }
    },

    onDeselect: function (selModel, record, index, eOpts) {

        if (record) {
            var idx = Ext.Array.indexOf(this._targetSelectItems.keys, record.getSparePartId(), 0);
            if (idx > -1) {
                //var item = this._targetSelectItems.items[idx];
                Ext.Array.removeAt(this._targetSelectItems.keys, idx);
                Ext.Array.removeAt(this._targetSelectItems.items, idx);
            }
        }
    },
    _gridBlockProcess: function (block) {
        /// <summary>
        /// grid 处理
        /// </summary>
        /// <param name="block" type="type"></param>
        var me = this;
        var multiSelect = me.gridCfg.multiSelect;
        var gridConfig = block.gridConfig || block.mainBlock.gridConfig;
        gridConfig.selModel = {
            injectCheckbox: 0, //checkbox位于哪一列，默认值为0
            selType: 'checkboxmodel', //checkbox
            checkOnly: true, //只能通过checkbox选择
            mode: (multiSelect ? 'MULTI' : 'SINGLE'), //是否多选
        };
        gridConfig.viewConfig = {
            enableTextSelection: true, //启用文本选中
            getRowClass: function (record, index, rowParams, store) {
                var rowClass = me.getRowClass(record, index, rowParams, store);
                if (rowClass) return rowClass;//如果重写了getRowClass方法，返回重写的样式
                if (me._sourceViewSelectItems && me._sourceViewSelectItems.length > 0) {
                    if (Ext.Array.contains(me._sourceViewSelectItems, record.getSparePartId())) {
                        return 'gridRowLock'; //添加自定义样式
                    }
                }
            }
        };
    },
});