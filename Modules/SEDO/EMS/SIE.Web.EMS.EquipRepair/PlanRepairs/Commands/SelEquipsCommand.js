SIE.defineCommand('SIE.Web.EMS.EquipRepairs.Commands.SelEquipsCommand', {
    extend: 'SIE.Web.EMS.Common.Commands.SelDeviceBillCommand',
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    /**
         * canExecute 是否执行
         * @param {} view 当前视图
         * @returns {}
         */
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return entity.data.INV_ORG_ID !=null && !entity.isNew()
                && (entity.data.ApprovalStatus == 10 || entity.data.ApprovalStatus == 50);
        }
        return false;
    },
    onLoad: function (store, records, successful, operation, eOpts) {
        var me = this;
        this.callParent(arguments);
    },
});