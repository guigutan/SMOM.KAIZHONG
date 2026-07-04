SIE.defineCommand('SIE.Web.Tech.Processs.Commands.CopyProcessCommand', {
    extend: 'SIE.cmd.Copy',  
    meta: { text: "复制新增", group: "edit", iconCls: "icon-AddEntity icon-green" },
    executeCopy: function () {         
        var me = this;        
        var editEntity = me.getEditEntity();
        me.onEditting(editEntity);
        me.edit(editEntity);
        me.onEdited(editEntity);     
        editEntity.setReferenceTimes(0);
        editEntity.mon(editEntity, 'propertyChanged', SIE.Web.Tech.ProcessCommonFun.ProcessPropertyChanged, me);
    },
});
