SIE.defineCommand('SIE.Web.Items.ProductModels.Commands.SelectProductModel', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'Id', targetClassName: 'SIE.Items.ProductModel' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        /* post数据结构*/
        var indata = {};
        /* post数据结构*/
        var selections = this._targetSelectItems.items;
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var productModelId = item.getId();
                if (me._sourceViewSelectItems.indexOf(productModelId) === -1) {
                    var familyInCategory = { ProductFamilyId: me._sourceId, ProductModelId: productModelId };
                    operationDatas.push(familyInCategory);
                }
            });
            indata = operationDatas;
            SIE.invokeDataQuery({
                method: 'SetProductModel',
                action: 'queryer',
                params: [indata],
                type: 'SIE.Web.Items.ProductModels.ProductModelDataQuery',
                token: me._ownerView.getToken(),
                success: function (res) {
                    me._ownerView.loadChildData(true);
                    win.close();
                }
            });           
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    }
    // end 
});