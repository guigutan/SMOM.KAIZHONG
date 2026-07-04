SIE.defineCommand('SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.AddEquipRepairCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "保养报修", group: "edit", iconCls: "icon-NetworkNormal icon-blue" },
    view: null,
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity != null && entity.data) {
            if (entity.getExeState() == 1 || entity.getExeState() == 3 || entity.getExeState() == 5)
                return false;
            if (!entity.getWhetherBegin())
                return false;
        }
        return true;
    },
    execute: function (view, source) {
        var project = view._children[0].getData().data.items;

        if (project.length <= 0) {
            SIE.Msg.showInstantMessage('请添加保养项目!'.t());
            return false;
        }

        var existMaintainResult = false;
        var existNG = false;

        Ext.each(project, function (item) {
            if (item.data.MaintainResult == null) {
                existMaintainResult = true;
            }

            if (item.data.MaintainResult == 0) {
                existNG = true;
            }
        });

        if (existMaintainResult) {
            SIE.Msg.showInstantMessage('还有保养项目未有保养结果!'.t());
            return false;
        }

        if (!existNG) {
            SIE.Msg.showInstantMessage('没有不合格的保养项目!'.t());
            return false;
        }

        this.view = view;
        var me = this;
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
        var tabId = 'tab_Maintains_CreateEquipRepairBill_001';

        //防止确认异常和查看异常按钮打开同一个页签
        opt.tabId = tabId;

        var additionParams = {
            //设备台账ID
            equipmentAccountID: this.view.getData().getEquipAccountId(),
            //来源单号 保养单号
            sourceNo: this.view.getData().getMaintainNo(),
            //来源类型 保养=1
            sourceType: 1
        };

        opt.params = opt.params || {};
        opt.parmas = Ext.merge(opt.params, additionParams);

        this.callParent(arguments);
    },
});