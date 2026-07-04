SIE.defineCommand('SIE.Web.Items.Items.Commands.ItemAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },
    getEditEntity: function () {
        var newEntity = Ext.create(this.view.model);
        if (this.view.isListView) {
            newEntity = this.createNewItem();
        }
        newEntity.phantom = false; //触发只读一下，自动显示编码
        this.onItemCreated(newEntity);
        return newEntity;
    },
    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        this.view.execute({
            data: model,
            isSubmmit: false,
            success: function (res) {
                var data = res.Result;
                //添加时设置编码和状态
                entity.setCode(data.Code);
                entity.phantom = true;
                entity.setState(1);
                ////设置基础资料信息
                var baseDataView = me.view.getChildren()[0];
                var baseDataName = "SIE.Items.Item_" + baseDataView._id;
                if (entity[baseDataName]) {
                    me.view.getChildren()[0].getData().setConsumeMode(0);
                } else {
                    var baseDataStore = SIE.data.Utils.createStore({
                        model: "SIE.Items.Item",
                    });
                    var baseDataEntity = new baseDataView._model();
                    baseDataEntity.setConsumeMode(0);
                    baseDataStore.add(baseDataEntity);
                    entity[baseDataName] = baseDataStore;
                }
                //添加三条分类维护信息
                var itemCategoryList = data.ItemCategoryList;
                var childView = me.view.getChildren()[1];
                var cName = "ItemCategoryList_" + childView._id;
                var store = childView.getData();
                if (entity[cName]) {
                    for (var i = 0; i < itemCategoryList.length; i++) {
                        var newEntity = new childView._model();
                        newEntity.data = itemCategoryList[i];
                        newEntity.setCreateDate(null);
                        newEntity.setUpdateDate(null);
                        store.add(newEntity);
                    }
                } else {
                    var newStore = SIE.data.Utils.createStore({
                        model: store.model,
                    });
                    for (var i = 0; i < itemCategoryList.length; i++) {
                        var newEntity = new childView._model();
                        newEntity.data = itemCategoryList[i];
                        newEntity.setCreateDate(null);
                        newEntity.setUpdateDate(null);
                        newStore.add(newEntity);
                    }
                    entity[cName] = newStore;
                }
            }
        }, me.view);
    }
});