SIE.defineCommand('SIE.Web.EMS.Tpms.Commands.AddRecordCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
     execute: function (view, source) {
         var editEntity = this.getEditEntity();
         CRT.Workbench.addPage({
            tabId: 'tab_addRecord',
            title: '添加-TPM操作记录',
            entityType: view.model,
            recordId: editEntity.data.Id,
            entityType: 'SIE.EMS.Tpms.TpmRecord', 
            viewGroup:"TpmRecordAddView",
            isDetail:true,
        });
    },
});