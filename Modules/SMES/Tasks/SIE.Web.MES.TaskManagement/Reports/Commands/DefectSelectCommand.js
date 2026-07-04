SIE.defineCommand('SIE.Web.MES.TaskManagement.Reports.DefectSelectCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (listView) {
        return listView._parent.getCurrent() != null;
    },
    _targetSelectItems: [],//操作弹窗视图选择的项
    _sourceViewSelectItems: [],
    execute: function (listView) {
        var me = this;
        if (!this.viewMeta) {
            SIE.AutoUI.getMeta({
                ignoreCommands: true,
                isAggt: true,
                token: listView.token,
                model: "SIE.Defects.Defect",
                viewGroup: 'SelectionModelView',
                callback: function (res) {
                    var mainBlock;
                    if (res.mainBlock)
                        mainBlock = res.mainBlock;
                    else
                        mainBlock = res;
                    var btnId = listView.currentBtnId;
                    var labelField = Ext.getCmp(btnId).ownerCt.items.items.first(function (p) { return p.name == 'quexianValue'; });
                    var view = SIE.AutoUI.generateAggtControl(res);
                    var ui = view.getControl();
                    var defectView = view;
                    //设置看不见的条件,让页面只能查工单产品的数据                   
                    var dialogView = view._view;
                    var win = SIE.Window.show({
                        title: "选择缺陷".t(),
                        width: '60%',
                        height: '80%',
                        items: ui,
                        callback: function (btn) {
                            if (btn == "确定".t()) {
                                var selections = defectView._view._OwnView._targetSelectItems.items;
                                var textStr = "";
                                var idStr = [];
                                selections.forEach(function (p) {
                                    idStr.push(p.data.Id);
                                    textStr += p.data.Description + ",";
                                });
                                labelField.setValue(textStr.substring(0, textStr.length - 1));
                                var task = listView._parent.getCurrent().data;
                                listView.dicConfig[task.Id] = idStr;
                            }
                        }
                    });

                    me.dialogView = dialogView;
                    dialogView._OwnView = me;
                    me.recordIds = [];
                    listView.dicConfig = {};
                    var task = listView._parent.getCurrent().data;
                    if (listView.dicConfig[task.Id]) {
                        me.sourceIds = listView.dicConfig[task.Id];
                        var valueIds = listView.dicConfig[task.Id];
                        valueIds.forEach(function (p) {
                            me.recordIds.push(p);
                            me._sourceViewSelectItems.push(p);
                        })
                    }
                         
                    me.setGridListeners();
                    me._targetSelectItems = { items: [], keys: [] };
                    dialogView.loadData({
                        callback: function (res) {
                            me.setSelected();
                            me._sourceViewSelectItems = [];
                        }
                    });
                    dialogView.getControl().mon(dialogView.getData(), 'load', me.setSelected, me);
                }
            });
        }
    },
    setSelected: function () {
        var me = this;
        if (me.dialogView.getData().data) {
            var records = me.dialogView.getData().data.items;
            var sourceViewSelectItems = me.recordIds;
            if (records && records.length > 0 && sourceViewSelectItems && sourceViewSelectItems.length > 0) {
                var selModel = me.dialogView.getSelectionModel();
                for (var i = 0, len = records.length; i < len; i++) {
                    var record = records[i];
                    if (sourceViewSelectItems.where(function (p) { return p == record.data.Id; }).length > 0) {
                        selModel.select(record, true, true); //勾选上.
                        me.onSelect(null, record);
                    }
                }
            }
        }
    },
    setGridListeners: function () {
        /// <summary>
        /// grid 绑定事件
        /// </summary>
        var me = this;
        var grid = me.dialogView.getControl();
        me.mon(grid.getSelectionModel(), {
            scope: me,
            select: me.onSelect,
            beforedeselect: me.onBeforeDeselect,
            deselect: me.onDeselect,
        });
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
        var idx = Ext.Array.indexOf(this._targetSelectItems.keys, record.getId(), 0);
        if (idx === -1) {
            this._targetSelectItems.keys.push(record.getId());
            this._targetSelectItems.items.push(record);
            if (this.recordIds.length == 0 || this.recordIds.where(function (p) { return p == record.getId(); }).length == 0) {
                this.recordIds.push(record.getId());
            }
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
            var idx = Ext.Array.indexOf(this._sourceViewSelectItems, record.getId(), 0);
            if (idx > -1) {
                return false;
            }
        }
    },
    onDeselect: function (selModel, record, index, eOpts) {
        /// <summary>
        /// 取消选择事件 
        /// </summary>
        /// <param name="selModel" type="Ext.selection.RowModel">选择模式</param>
        /// <param name="record" type="Ext.data.Model">选择的记录</param>
        /// <param name="index" type="Number">行索引号</param>
        /// <param name="eOpts" type="Object">The options object passed to Ext.util.Observable.addListener.</param>
        //console.log('取消勾选checkbox后，获得选中行索引:' + index);
        if (record) {
            var idx = Ext.Array.indexOf(this._targetSelectItems.keys, record.getId(), 0);
            if (idx > -1) {
                Ext.Array.removeAt(this._targetSelectItems.keys, idx);
                Ext.Array.removeAt(this._targetSelectItems.items, idx);
                Ext.Array.removeAt(this._sourceViewSelectItems, idx);
                if (this.recordIds.length > 0 && this.recordIds.where(function (p) { return p == record.getId(); }).length > 0) {
                    this.recordIds.remove(record.getId());
                }
            }
        }
    },
});
