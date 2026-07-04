//润滑报修
SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.AddLubricationRepairCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "报修", group: "edit", iconCls: "icon-NetworkNormal icon-blue" },
    view: null,
    canExecute: function (view) {
        return true;
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
        var tabId = 'tab_CreateEquipRepairBill_001';

        //防止确认异常和查看异常按钮打开同一个页签
        opt.tabId = tabId;

        var additionParams = {
            //设备台账ID
            equipmentAccountID: this.view.getData().getEquipAccountId(),
            //来源单号 点检单号
            sourceNo: this.view.getData().getLubricationNo(),
            //来源类型 润滑=4
            sourceType: 4
        };

        opt.params = opt.params || {};
        opt.parmas = Ext.merge(opt.params, additionParams);

        this.callParent(arguments);
    },
});