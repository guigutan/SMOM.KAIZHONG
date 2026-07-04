SIE.defineCommand('SIE.Web.EMS.RunStandards.Commands.SelRunStandardProjectCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'ProjectDetailId',
            targetClassName: 'SIE.EMS.Equipments.Models.EquipModelRepairProject'
        }
    },
    /**
     * canExecute 是否执行
     * @param {} view 当前视图
     * @returns {}
     */
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity == null) {
            //所属父对象（设备型号）为空 不能点击选择润滑项目
            return false;
        }

        //新增时，不能点击选择润滑项目
        if (entity.isNew()) {
            return false;
        }
        //未选择设备型号时候设置不可用
        if (!view.getParent().getCurrent().getEquipModelId()) {
            return false;
        }

        return true;
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
                    var item = {
                        SourceId: me._sourceId,
                        ProjectDetailId: id,
                    };
                    items.push(item);
                }
            });
            if (items.length > 0) {
                indata = items;
                me._targetView.execute({
                    data: indata,
                    success: function (res) {
                        debugger;
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
                if (dialogView._relations[0]) {
                    //存在查询面板时
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
        if (data != null && data.getEquipModelId() != null) {
            criteria.setEquipModelId(data.getEquipModelId());
        }
    },
});