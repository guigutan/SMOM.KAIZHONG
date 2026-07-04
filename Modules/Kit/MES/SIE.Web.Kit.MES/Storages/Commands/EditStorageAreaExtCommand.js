SIE.defineCommand('SIE.Web.Kit.MES.Storages.Commands.EditStorageAreaExtCommand', {
    extend: 'SIE.cmd.Edit',      
    execute: function (view, source) {
        var me = this;
        var parent = this.view.getParent();       
        //if (parent.PersistenceStatus == 0)
        //    parent.PersistenceStatus = 1;
    }
});
