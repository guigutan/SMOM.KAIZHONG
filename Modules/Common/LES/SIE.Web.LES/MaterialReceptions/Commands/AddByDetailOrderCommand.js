SIE.defineCommand('SIE.Web.LES.MaterialReceptions.Commands.AddByDetailOrderCommand', {
    meta: { text: "添加明细", group: "edit", iconCls: "icon-AddEntity icon-green" },
    execute: function (entity) {
        var textValue = "";
        var scanType = 0;
        if (this.view.viewGroup == "AddDetailPageView") {
            textValue = Ext.getCmp('MaterialReceptions_addDetail').value
            scanType = 1;
            this._getDetail(entity, textValue, scanType);
            Ext.getCmp('MaterialReceptions_addDetail').setValue('');
        }
        else {
            textValue = Ext.getCmp('MaterialReceptions_addOrder').value
            scanType = 2;
            var me = this;
            var viewData = this.view.getData();
            if (viewData.data.length != 0) {
                Ext.MessageBox.confirm("提示".t(), "重新获取明细，将清空界面现有记录，是否继续?".t(), function (btnId) {
                    if (btnId == "yes") {
                        viewData.removeAll();
                        me._getDetail(entity, textValue, scanType);
                        Ext.getCmp('MaterialReceptions_addOrder').setValue('');
                    }
                    else {
                        cancel = true;
                    }
                });
            }
            else {
                this._getDetail(entity, textValue, scanType);
                Ext.getCmp('MaterialReceptions_addOrder').setValue('');
            }
        }
    },
    _getStoreIds: function (entity) {
        var validationIds = [];
        var data = entity.getData().data.items;
        if (data.length != 0) {
            data.forEach(item => {
                validationIds.push(item.data.DetailId);
            });
        }
        return validationIds;
    },
    _isInStoreIds: function (validationIds, thisId) {
        for (var i = 0; i < validationIds.length; i++) {
            if (thisId === validationIds[i]) {
                return true;
            }
        }
        return false;
    },
    _getDetail: function (entity, textValue, scanType) {
        var me = this;
        var scanedRecords = entity.getData().data.items;
        var scanedList = []
        scanedRecords.forEach(item => {
            scanedList.push(item.data);
        });
        var indata = {};
        indata.data = Ext.encode({ LabelNo: textValue, ScanType: scanType, ScanRecords: scanedList });
        this.view.execute({
            data: indata,
            success: function (res) {
                if (res.Success) {
                    var e = entity;
                    var validationIds = me._getStoreIds(e);
                    var data = res.Result;
                    if (scanType === 2) {
                        entity.getControl().setStore(data);
                    }
                    else {
                        entity.getControl().getStore().add(data);
                    }
                }
            }
        });
    }
});
