//点检报修
SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.AddCheckEquipRepairCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "报修", group: "edit", iconCls: "icon-NetworkNormal icon-blue" },
    view: null,
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity != null && entity.data) {
            if (entity.getExeState() == 1 || entity.getExeState() == 3 || entity.getExeState() == 5)
                return false;
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var project = view._children[0].getData().data.items;
        if (project.length <= 0) {
            SIE.Msg.showInstantMessage('请添加点检项目!'.t());
            return false;
        }

        var anyNotHaveCheckResult = false;
        var existNG = false;

        Ext.each(project, function (item) {
            if (item.data.CheckResult == null) {
                anyNotHaveCheckResult = true;
            }
            if (item.data.CheckResult == 0) {
                existNG = true;
            }
        });

        if (anyNotHaveCheckResult) {
            SIE.Msg.showInstantMessage('还有点检项目未有点检结果!'.t());
            return false;
        }

        if (!existNG) {
            SIE.Msg.showInstantMessage('没有不合格的点检项目!'.t());
            return false;
        }

        me.AddRepairPage(me, view)
        //SIE.invokeDataQuery({
        //    type: "SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery",
        //    method: "CheckPlanWithUnFinishRepairBill",
        //    params: [view.getData().getEquipAccountId()],
        //    async: false,
        //    token: view.token,
        //    callback: function (res) {
        //        if (res.Success) {
        //            if (res.Result.length > 0) {
        //                SIE.Msg.askQuestion(Ext.String.format("设备存在未完成的维修单，报修人{0},是否继续报修？".t(), res.Result), function () {
        //                    me.AddRepairPage(me, view)
        //                });
        //            }
        //            else {
        //                me.AddRepairPage(me, view)
        //            }
        //        } else {
        //            SIE.Msg.showError(res.Message);
        //        }
        //    }
        //})
    },
    AddRepairPage: function (me, view) {
        me.addPage({
            entityType: 'SIE.EMS.EquipRepair.EquipRepairs.EquipRepairBill',
            viewGroup: 'CreateRepairBillView',
            isDetail: true,
            isNew: true,
            ignoreQuery: true,
            title: '报修'.L10N(),
            module: view.module,
            isAggt: true
        });
    },
    /**
     * @override 修改页签ViewGroup
     * @param {any} opt  页签参数
     */
    addPage: function (opt) {
        var tabId = 'tab_CreateEquipRepairBill_001';

        //防止确认异常和查看异常按钮打开同一个页签
        opt.tabId = tabId;

        var additionParams = {
            //设备台账ID
            equipmentAccountID: this.view.getData().getEquipAccountId(),
            //来源单号 点检单号
            sourceNo: this.view.getData().getCheckPlanNo(),
            //来源类型 点检=0
            sourceType: 0
        };

        opt.params = opt.params || {};
        opt.parmas = Ext.merge(opt.params, additionParams);

        this.callParent(arguments);
    },
});