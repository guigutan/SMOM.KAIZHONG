//特种设备定检维修
SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.AddRegularInspectionRepairCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "报修", group: "edit", iconCls: "icon-NetworkNormal icon-blue" },
    view: null,
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity != null && entity.data) {
            //主表的检验结果为不合格时才能报修
            if (entity.getInspectionResult() === 2 && entity.getApprovalStatus() === SIE.Equipments.Enums.ApprovalStatus.Audited.value)
                return true;
        }
        return false;
    },
    execute: function (view, source) {
        this.view = view;
        this.addPage({
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
        var tabId = 'tab_CreateEquipRepairBill_002';

        //防止确认异常和查看异常按钮打开同一个页签
        opt.tabId = tabId;

        var additionParams = {
            equipmentAccountID: this.view.getCurrent().getSpecialEquipmentAccountId(),
            sourceNo: this.view.getCurrent().getInspectionNo(),
             //来源类型 特种设备=3
            sourceType: 3
        };

        opt.params = opt.params || {};
        opt.parmas = Ext.merge(opt.params, additionParams);

        this.callParent(arguments);
    },
});