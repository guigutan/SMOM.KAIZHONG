SIE.defineCommand('SIE.Web.Core.QmsStaticConst.Commands.AddConstCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加".t(), group: "edit" },
    execute: function (view, source) {
        var me = this;
        view.execute({
            data: {},
            success: function (res) {
                var editEntity = me.getEditEntity(res.Result, view);
                view.setCurrent(editEntity);

                me.onEditting(editEntity);
                me.edit(editEntity);
                me.onEdited(editEntity);

                var tabPanel = view.getChildren()[0].getControl().up().up();
                var activeTab = tabPanel.getActiveTab().tab;
                var activeTabModel = activeTab.card.down().SIEView.model;
                var childrenViews = view.getChildren();
                for (var i = 0; i < childrenViews.length; i++) {
                    var key = childrenViews[i].getAssociateStoreKey();
                    childrenViews[i].setData(editEntity[key]);
                }

                me.drawChildren(view);
            }
        });
    },

    getEditEntity: function (childrenData, view) {
        var me = this;
        newEntity = this.createNewItem();
        newEntity.generateId();

        var loadedChildren = Ext.create('Ext.util.MixedCollection');
        if (view) {
            var childrenArray = view.getChildren();
            if (childrenArray) {
                for (var i = 0, len = childrenArray.length; i < len; i++) {
                    var child = childrenArray[i];
                    var childrenStore = child.getData();
                    if (childrenStore) {
                        childrenStore.associateView = child;
                        loadedChildren.add(child.getAssociateKey(), childrenStore);
                    }
                }
            }
        }
        var isNotCopyArtty = me.getNotCopyArtty();
        for (var i = 0; i < loadedChildren.length; i++) {
            var curEntity = loadedChildren.getAt(i);
            var newStore = SIE.data.Utils.createStore({
                model: curEntity.model,
            });
            var source = me.getSource(curEntity, childrenData);        
            for (var j = 0; j < source.length; j++) {
                var newChildEnyity = new curEntity.associateView._model();
                source[j].Id = null;
                source[j]["MsaConstId"] = newEntity.getId();
                isNotCopyArtty.forEach(function (item) {
                    source[j][item] = null;
                });
                newChildEnyity.data = source[j];
                newStore.add(newChildEnyity);
            }
            var storeName = curEntity.associateView.getAssociateStoreKey();
            newEntity[storeName] = newStore;
        }
        return newEntity;
    },

    getSource: function (curEntity, childrenData) {
        var source;
        if (curEntity.model.$className == "SIE.Core.QmsStaticConst.ControlChartConst") {
            source = childrenData.listControlChart;
        }
        else if (curEntity.model.$className == "SIE.Core.QmsStaticConst.StaticConstD2") {
            source = childrenData.listD2;
        }
        else if (curEntity.model.$className == "SIE.Core.QmsStaticConst.StaticConstT") {
            source = childrenData.listT;
        }
        else if (curEntity.model.$className == "SIE.Core.QmsStaticConst.StaticConstK1") {
            source = childrenData.listK1;
        }
        else if (curEntity.model.$className == "SIE.Core.QmsStaticConst.StaticConstK2") {
            source = childrenData.listK2;
        }
        else if (curEntity.model.$className == "SIE.Core.QmsStaticConst.StaticConstK3") {
            source = childrenData.listK3;
        }
        return source;
    },

    drawChildren: function (view) {
        var childrenViews = view.getChildren();
        childrenViews.forEach(function (childrenView, index) {
            if (childrenView.model === "SIE.Core.QmsStaticConst.StaticConstT") {
                var ctl = childrenView.getController();
                if (ctl)
                    ctl.drawGrid(childrenView);
            }
            if (childrenView.model === "SIE.Core.QmsStaticConst.StaticConstD2") {
                var ctl = childrenView.getController();
                if (ctl)
                    ctl.drawGrid(childrenView);
            }
        });
    },

    getNotCopyArtty: function () {
        var isNotCopyArtty = [];
        isNotCopyArtty.push("CreateBy");
        isNotCopyArtty.push("CreateByName");
        isNotCopyArtty.push("CreateBy_Display");
        isNotCopyArtty.push("CreateDate");
        isNotCopyArtty.push("UpdateBy");
        isNotCopyArtty.push("UpdateByName");
        isNotCopyArtty.push("UpdateBy_Display");
        isNotCopyArtty.push("UpdateDate");
        return isNotCopyArtty;
    },

});