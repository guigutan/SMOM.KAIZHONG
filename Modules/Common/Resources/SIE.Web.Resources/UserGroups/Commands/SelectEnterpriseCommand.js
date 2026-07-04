SIE.defineCommand('SIE.Web.Resources.UserGroups.Commands.SelectEnterpriseCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    meta: { text: "选择工厂", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'EnterpriseId',
            targetClassName: 'SIE.Resources.Employees.EmployeeEnterpriseSelect',
            targetCriteriaClassName: 'SIE.Resources.Employees.EmployeeEnterpriseSelectCriteria'
        },
    },
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        /* post数据结构*/
        var indata = {};
        /* post数据结构*/
        var selections = this._targetSelectItems.items;
        if (selections && selections.length > 0) {
            var userInRoles = [];
            SIE.each(selections, function (item) {
                var enterpriseId = item.getId();
                if (me._sourceViewSelectItems.indexOf(enterpriseId) === -1) {
                    var userInRole = { UserGroupId: me._sourceId, EnterpriseId: enterpriseId };
                    userInRoles.push(userInRole);
                }
            });
            if (userInRoles.length > 0) {
                indata = userInRoles;
                me._targetView.execute({
                    data: indata,
                    success: function (res) {
                        win.close();  //关闭模态窗口
                        me._ownerView.loadChildData(true); //重载视图数据
                    }
                }, me._ownerView);
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
        var dialogView = me._targetView;
        var oldData = this.cloneStore.getData();
        me._sourceViewSelectItems = [];
        for (var i = 0; i < oldData.length; i++) {
            me._sourceViewSelectItems.push(oldData.items[i].getEnterpriseId())
        }
        if (me._targetView !== null) {
            var store = dialogView.getData();
            if (store !== null) {
                me.mon(store, 'load', me.onLoad, this);
                if (dialogView._relations[0]) {

                    //存在查询面板时
                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
    _loadSourceViewAllData: function (view, source) {
        /// <summary>
        /// 加载源视图的所有数据(不分页)
        /// </summary>
        var me = this;
        if (view) {
            var cfg = {
                scope: this,
                callback: function (records, operation, success) {
                    this.cloneStore._loaded = success;
                    var currIds = view.getData().data.items.map(function (item) { return item.getEnterpriseId(); });
                    for (var i = 0; i < this.cloneStore.getData().items.length;) {
                        if (!currIds.contains(this.cloneStore.getData().items[i].getEnterpriseId())) {
                            this.cloneStore.getData().remove(this.cloneStore.getData().items[i])
                        }
                        else {
                            i++;
                        }
                    }

                    var parent = view.getParent();
                    var sourceId;
                    if (parent) {
                        sourceId = parent.getCurrent().getId();
                    }
                    else {
                        if (view.getCurrent()) {   //没有父实体时，当前实体可能为空
                            sourceId = view.getCurrent().getId();
                        }
                    }
                    me._sourceId = sourceId;
                    var model = view.model;
                    if (model) {
                        me.getViewMeta(source);
                    }
                }
            }
            var store = view.getData();
            this.cloneStore = store.clone({ pageSize: this.gridCfg.pageSize }); //克隆数据store
            this.cloneStore.load(cfg);
        }
    },
});
