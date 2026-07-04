Ext.define('SIE.Web.ERPInterface.Parameters.Scripts.SyncFactoryItemLabelToFactoryParameterBehavior', {
    onViewReady: function (view) {
        debugger
        view.getControl().up().close();
        CRT.Workbench.addPage({
            entityType: 'SIE.KZ.Base.SmomControl.SyncItemLabel',
            title: '跨组织物料标签同步'.L10N(),
            module: view.module,
            ignoreQuery: false,
            isAggt: true
        });
    }
});