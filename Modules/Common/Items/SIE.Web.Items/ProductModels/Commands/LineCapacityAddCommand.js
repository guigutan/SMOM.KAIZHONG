SIE.defineCommand('SIE.Web.Items.ProductModels.Commands.LineCapacityAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加".t(), group: "edit" },
    getEditEntity: function () {
        var newEntity = new this.view._model();
        var parent = this.view.getParent();
        var productModelId = parent.getCurrent().data.Id;
        if (this.view.isListView) {
            newEntity = this.createNewItem();
        }
        this.onItemCreated(newEntity);
        newEntity.data.ProductModelId = productModelId;
        return newEntity;
    },
});