/**
 * 备件入库保存命令
 */
SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.SparePartStoreSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    /**
     * 验证实体view._children[0].getData().data.items[0].OrderNumberList.length  sparePartStore.items[2]._StoreDetailList
     * @param {any} view
     */
    //onValidation: function (view) {
    //    var sparePartStore = view.getData().data;
    //    if (sparePartStore.length != 0) {
    //        for (var i = 0; i < sparePartStore.length; i++) {
    //            if (sparePartStore.items[i]._StoreDetailList != undefined) {
    //                if (sparePartStore.items[i]._StoreDetailList.getData().items.length==0) {
    //                    SIE.Msg.showError('入库明细不能为空!'.t());
    //                    return false;
    //                }
    //            }
    //            if (sparePartStore.items[i]._StoreDetailList != null) {
    //                var storeDetai = sparePartStore.items[i]._StoreDetailList.getData();
    //                for (var m = 0; m < storeDetai.length; m++) {
    //                    if (storeDetai.items[m]._OrderNumberList != undefined && storeDetai.items[m].getIsSeqNoCharge() == true) {
    //                        if (storeDetai.items[i]._OrderNumberList.getData().items.length != storeDetai.items[m].getNumber) {
    //                            storeDetai.items[m].setNumber(storeDetai.items[i]._OrderNumberList.getData().items.length);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    //var childCtrls = view.getChildren()[0].getControl().items.items;
        
    //    return true;
    //}

});