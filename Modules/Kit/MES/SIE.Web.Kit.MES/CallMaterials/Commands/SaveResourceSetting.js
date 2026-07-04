SIE.defineCommand('SIE.Web.Kit.MES.CallMaterials.Commands.SaveResourceSetting', {
    meta: { text: "资源保存", group: "edit" },    
    execute: function (view, source) {
        var me = view;
        me.execute({
            command: Ext.getClassName(this),
            data: source.resourceId,
            success: function (res) {                
            }
        });
    }
});