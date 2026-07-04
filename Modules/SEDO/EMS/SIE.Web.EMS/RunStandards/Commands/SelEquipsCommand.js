SIE.defineCommand('SIE.Web.EMS.RunStandards.Commands.SelEquipsCommand', {
    extend: 'SIE.Web.EMS.Common.Commands.SelDeviceBillCommand',
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'EquipAccountId',
            targetClassName: 'SIE.Equipments.EquipAccounts.EquipAccountSelect'
        }
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    /**
         * canExecute 是否执行
         * @param {} view 当前视图
         * @returns {}
         */
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return !entity.isNew() && (entity.data.ApprovalStatus == 10 || entity.data.ApprovalStatus == 50) && !entity.isDirty();
        }
        if (entity != null) {
            return true;
        }
        return false;
    },
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        var indata = {};
        var selections = me._targetSelectItems;
        if (selections && selections.items.length > 0) {
            var items = [];
            SIE.each(selections.items, function (sel) {
                var id = sel.getId();
                if (me._sourceViewSelectItems.indexOf(id) === -1) {
                    var item = { SourceId: me._sourceId, DevicePurId: id };
                    items.push(item);
                }
            });
            if (items.length > 0) {
                indata = items;
                me._targetView.execute({
                    data: indata,
                    success: function (res) {
                        win.close();
                        me._ownerView.loadChildData(true);
                    }
                }, me._ownerView);
                return true;
            }
        }
        Ext.Msg.alert('提示'.t(), '没有可提交的数据'.t());
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
                    me.setQueryCriteria(dialogView, me.view.getParent().getCurrent());
                    dialogView._relations[0]._target.tryExecuteQuery();
                    me.setQueryCriteria(dialogView, me.view.getParent().getCurrent());
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },

    setQueryCriteria: function (dialogView, data) {
        //隐藏查询条件的清除按钮
        var clearCommand = dialogView._relations[0]._target._commands.items.first(function (p) {
            return p.meta.command == "SIE.cmd.ClearCondition";
        });

        if (clearCommand) {
            var Id = clearCommand.meta.id;
            document.getElementById(Id).style.display = "none";
        }

        var criteria = dialogView._relations[0]._target.getData();
        if (data.getEquipModelId_Display() != null && data.getEquipModelId_Display() != "") {
            criteria.setModelCode(data.getEquipModelId_Display());
        }
    },

    /// <summary>
    /// 查询块处理-只读为false
    /// </summary>
    /// <param name="block" type="type"></param>
    _queryBlockProcess: function (block) {
        if (block.surrounders) {
            var surround = block.surrounders["0"];
            if (surround) {
                var items = surround.mainBlock.formConfig.items;
                for (var i = 0, len = items.length; i < len; i++) {
                    var item = items[i];
                    if (item.name == "ModelCode") {
                        item.readOnly = true;
                    } else {
                        item.readOnly = false;
                    }
                }
            }
        }
    },
});