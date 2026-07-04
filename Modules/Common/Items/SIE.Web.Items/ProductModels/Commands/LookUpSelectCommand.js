//弹窗选择命令基类
SIE.defineCommand('SIE.Web.CommonCommand.LookUpSelectCommand', {
    meta: { text: "选择", group: "edit", iconCls: "iconfont icon-PlaylistCheck icon-blue" },
    _curUserId: null, //操作的用户id
    _ownerViewSelecteds: null, //主人视图已选择项
    _operationView: null,//操作弹窗视图
    _selects: null,//弹窗视图选择的项
    model:null,
    selectCallback : function(win,data){

    },

    canExecute: function (view) {
        var p = view.getParent();
        if (!p) { return true; }
        return p.getCurrent() !== null;
    },
    execute: function (listView, source) {
        var me = this;
        me._curUserId = listView.getParent().getCurrent().getId();
        var selectItems = listView.getData().getData().items;
        me._ownerViewSelecteds = me._getSelectids(selectItems);
        var model = listView.model;
        if (model) {
            SIE.AutoUI.getMeta({
                model: model,
                ignoreChild: true,
                ignoreCommands: true,
                isReadonly: true,
                callback: function (res) {
                    var blocks = res;
                    me._queryBlockProcess(blocks);
                    blocks.mainBlock.gridConfig.selModel = {
                        selType: 'checkboxmodel',
                        singleSelect: false, //是否单选
                        checkOnly: true, //只允许用户通过复选框选中
                        pruneRemoved: false //默认true，翻页保持勾选
                    };
                    me._gridBlockProcess(blocks);
                    var ui = SIE.AutoUI.generateAggtControl(blocks);
                    me._popupWin(ui, source);
                    return me.selectCallback;
                }
            });
        }
    },

    //--------------------private---------------
    _getSelectids: function (array) {
        if (array) {
            var me = this;
            me._ownerViewSelecteds = [];
            SIE.each(array, function (item) {
                me._ownerViewSelecteds.push(item.getId());
            });
            return me._ownerViewSelecteds;
        }
        return null;
    },
    _popupWin: function (ui, source) {
        var me = this;
        me._operationView = ui._view;
        //弹窗
        var win = SIE.Window.show({
            title: ('选择' + me._operationView.label).t(),
            animateTarget: source, items: ui.getControl(),
            width: 800, height: 500,
            //buttons: ['确定'.t(), '关闭'.t()], //自定义按钮
            callback: function (btn) {
                if (btn.t() === '确定'.t()) {
                    me._save(win);
                }
            }
        });
        // bind event
        var grid = me._operationView.getControl();
        grid.on({
            select: me._onSelect,
            deselect: me._onDeselect,
            beforeselect: me._onBeforeselect,
            scope: this
        });
        //加载数据
        me._selects = { items: [], keys: [] };
        me._reloadViewData();
    },
    _reloadViewData: function () {
        /// <summary>
        /// 加载弹窗视图数据
        /// </summary>
        var me = this;
        me._operationView.loadData({
            callback: function () {
                me._initSelection(); //实现数据勾选
            }
        });
    },
    _initSelection: function () {
        /// <summary>
        /// 根据数据实现勾选上
        /// </summary>
        var me = this;
        if (me._ownerViewSelecteds && me._ownerViewSelecteds.length > 0) {
            var selModel = me._operationView.getSelectionModel();
            var userGroupStore = me._operationView.getData();
            for (var i = 0, len = userGroupStore.getCount() ; i < len; i++) {
                var userGroup = userGroupStore.getAt(i);
                if (Ext.Array.contains(me._ownerViewSelecteds, userGroup.getId())) {
                    selModel.select(userGroup, true); //勾选上.
                }
            }
        }
    },
    _onBeforeselect: function (selModel, record, index, eOpts) {
        //选择前事件
        //if (record.getId() !== 0) {
        //    return false;
        //}
    },
    _onSelect: function (selModel, record, index, eOpts) {
        //选择事件
        console.log('勾选了checkbox后，获得选中行的index:'.t() + index);
        var idx = Ext.Array.indexOf(this._selects.keys, record.getId(), 0);
        if (idx > -1) {
            var item = this._selects.items[idx];
            // 处理状态为CU
        }
        else {
            this._selects.keys.push(record.getId());
            this._selects.items.push(record);
        }
    },
    _onDeselect: function (selModel, record, index, eOpts) {
        //取消选择事件 
        console.log('取消勾选checkbox后，获得的record 中的index:'.t() + index);
        if (record) {
            var idx = Ext.Array.indexOf(this._selects.keys, record.getId(), 0);
            if (idx > -1) {
                var item = this._selects.items[idx];
                // 处理状态为D
            }
        }
    },
    _queryBlockProcess: function (block) {
        var items = block.surrounders["0"].mainBlock.formConfig.items;
        for (var i = 0, len = items.length; i < len; i++) {
            var item = items[i];
            item.readOnly = false;
        }
    },
    _gridBlockProcess: function (block) {
        //grid row 样式处理
        var me = this;
        block.mainBlock.gridConfig.viewConfig = {
            getRowClass: function (record, index, rowParams, store) {
                if (me._ownerViewSelecteds && me._ownerViewSelecteds.length > 0) {
                    if (Ext.Array.contains(me._ownerViewSelecteds, record.getId())) {
                        return 'getRowClassLock'; //添加自定义样式
                    }
                }
            }
        };
    },
    _save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        /* post数据结构*/
        var indata = {};
        /* post数据结构*/
        var selections = this._operationView.getSelection();
        if (selections && selections.length > 0) {
            var userInUserGroups = [];
            SIE.each(selections, function (item) {
                var userGroupId = item.getId();
                if (me._ownerViewSelecteds.indexOf(userGroupId) === -1) {
                    var userInUserGroup = { ParentId: me._curUserId, ChildId: userGroupId };
                    userInUserGroups.push(userInUserGroup);
                }
            });
            indata = userInUserGroups;
            me.selectCallback(win,indata);
            //me._operationView.execute({
            //    data: indata,
            //    success: function (res) {
            //        win.close();
            //        me._ownerView.loadChildData();
            //    }
            //}, me._ownerView);
        }
        else {
            Ext.Msg.alert('提示'.t(), '没有可提交的数据'.t());
        }
    },

});