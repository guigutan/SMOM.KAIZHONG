SIE.defineCommand('SIE.Web.ProductIntfc.ProductStorages.Commands.ParamAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加".t(), group: "edit", },  
    onItemCreated: function (entity) {
        if (entity) {
            entity.setQty(0);
        }
    }
});