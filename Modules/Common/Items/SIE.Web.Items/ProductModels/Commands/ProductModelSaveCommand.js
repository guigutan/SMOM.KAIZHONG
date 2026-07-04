SIE.defineCommand('SIE.Web.Items.ProductModels.Commands.ProductModelSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        var result = false;
        result = view.getData().isDirty();
        if (result == false) {
            var pv = view.getChildren();
            var current = view.getCurrent();
            if (current) {
                for (var i = 0; i < pv.length - 1; i++)
                {
                    if (pv[i].getData().isDirty()) {
                        return true;
                    }
                }
            }
        }
        return result;
    },
    execute: function (view, source) {
        var me = this;
        var children = view.getChildren();
        var indata = {};
        var productModel = [];
        var productModelLineCapacity = [];
        var productModelSkill = [];
        var data = view.getData();
        var childrenData1 = children[0].getData();
        var childrenData2 = children[1].getData();
        for (var i = 0; i < data.data.items.length; i++) {
            productModel[i] = data.data.items[i].data;
        }
        for (var i = 0; i < childrenData1.data.items.length; i++) {
            productModelSkill[i] = childrenData1.data.items[i].data;
        }
        for (var i = 0; i < childrenData2.data.items.length; i++) {
            productModelLineCapacity[i] = childrenData2.data.items[i].data;
        }

        indata.Data = Ext.encode({ ProductModelList: productModel, ProductModelLineCapacityList: productModelLineCapacity, ProductModelSkillList: productModelSkill  });
        view.execute({
            data: indata,
            success: function (res) {
                var operationView = view;
                if (view.associateCmd) {
                    operationView = view.associateCmd.view;
                }
                me._viewReload(operationView);
                view.getData().dirty = false;
                view.getData().phantom = false;
                SIE.Msg.showInstantMessage('保存成功'.t());
            }
        });
    }

});
