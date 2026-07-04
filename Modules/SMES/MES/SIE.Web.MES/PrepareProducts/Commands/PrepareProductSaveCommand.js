SIE.defineCommand('SIE.Web.MES.PrepareProducts.Commands.PrepareProductSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    //execute: function (view, source) {
    //    var me = this;
    //    var detail = view.findChild("SIE.MES.PrepareProducts.PrepareProductDetail");
    //    var indata = {};
    //    var preProduct = [];
    //    var preProductDetail = [];
    //    var dirtyParent = view.getData().data.items.filter(function (p) {
    //        return p.isDirty();
    //    });
    //    var dirtyChild = detail.getData().data.items.filter(function (p) {
    //        return p.isDirty();
    //    });
    //    dirtyParent.forEach(item => {
    //        preProduct.push(item.data);
    //    });
    //    dirtyChild.forEach(item => {
    //        preProductDetail.push(item.data);
    //    })
    //    indata.Data = Ext.encode({ PrepareProductList: preProduct, PrepareProductDetailList: preProductDetail });
    //    view.execute({
    //        data: indata,
    //        success: function (res) {
    //            SIE.Msg.showInstantMessage('保存成功'.t());
    //            CRT.Event.fire(view.model + '_refresh', view.getCurrent().getId());
    //        }
    //    });
    //}
});
