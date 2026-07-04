Ext.define('SIE.Web.Items.Items.Behaviors.ItemPropertyValueBehavior', {
    /**
     * 数据加载后
     * @param {*} view 
     */
    onDataLoaded: function (view) {
        view.getData().data.items.forEach(function (item) {
            if (item.getCatalogTypeId() !== "") {
                SIE.invokeDataQuery({
                    type: "SIE.Web.Items.Items.DataQuery.ItemDataQuery",
                    method: "GetCatalogCode",
                    params: [item.getCatalogTypeId(), item.getCatalogCode()],
                    async: false,
                    token: view.token,
                    callback: function (res) {
                        if (res.Success) {
                            item.setValue(res.Result);
                            item.markSaved();
                        }
                    }
                });

            }
        });
    }
});