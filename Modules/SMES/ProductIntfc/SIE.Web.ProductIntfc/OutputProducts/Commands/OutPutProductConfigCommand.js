SIE.defineCommand('SIE.Web.ProductIntfc.OutputProducts.Commands.OutputProductConfigCommand', {
    extend: 'SIE.Web.Common.Configs.Commands.ModuleConfigCommand',
    meta: { text: "配置项", group: "system", iconCls: "icon-ListConfig icon-blue" },
    showPage: function (view) {
        var keyId = view.model.replace(/[.|,]/g, '') + '_moduleConfig';
        var title = "联/副产品入库".t() + ' - ' + '配置项'.t();
        var moduleKey = 'SIE.Common.Configs.Module.ModuleConfig,SIE.Common';
        CRT.Workbench.addPage({
            tabId: keyId,
            title: title,
            entityType: moduleKey,
            module: moduleKey,
            params: {
                EntityType: view.model
            }
        });
    }
});