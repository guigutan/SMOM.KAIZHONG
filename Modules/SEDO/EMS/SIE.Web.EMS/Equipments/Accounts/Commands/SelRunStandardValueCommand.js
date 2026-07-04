SIE.defineCommand('SIE.Web.EMS.Equipments.Accounts.Commands.SelRunStandardValueCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue","tooltip":"选择数据后,需再次点击保存" },
    userConfig: {
        dataParams: { specKeyPrototyName: 'Id', targetClassName: 'SIE.EMS.RunStandards.RunStandardValue' }
    },
    /**
     * canExecute 是否执行
     * @param {} view 当前视图
     * @returns {}
     */
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity == null) {
            return false;
        }

        if (entity.data === null || entity.data.INV_ORG_ID === null) {
            return false;
        }

        //新增时，不能点击选择定标项目
        if (entity.isNew()) {
            return false;
        }

        return true;
    },
    _checkParameter: function () {
        /// <summary>
        /// 检查配置的必须参数
        /// </summary>
        var dataParams = this.dataParams;
        if (Ext.isEmpty(dataParams.specKeyPrototyName)) {
            SIE.emptyArgument('specKeyPrototyName');
        }
        if (Ext.isEmpty(dataParams.targetClassName)) {
            SIE.emptyArgument('targetClassName');
        }
        Ext.require(dataParams.targetClassName);
    },
    onLoad: function (store, records, successful, operation, eOpts) {
        var me = this;
        var Ids = [];
        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.Equipments.Accounts.DataQuery.EquipAccountDataQueryer",
            method: "GetRunStandardValueIds",
            params: [me._sourceId],
            async: false,
            token: me.view.token,
            callback: function (res) {
                if (res.Success) {
                    Ids = res.Result;
                }
            }
        });

        var entitys = store.data.items.where(function (p) { return Ids.indexOf(p.getId())>=0; });
        store.setData(entitys);
        this.callParent(arguments);
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
                        if (res.Result.length > 0) {
                            res.Result.forEach(item => {
                                if (me.view.getData().getData().items.findIndex(m => m.getRunStandardValueId() === item.RunStandardValueId) < 0) {
                                    me.view.getData().add(item);
                                }
                            });
                        }
                        win.close();
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
                dialogView.loadData();
            }
        }
    },
   
});