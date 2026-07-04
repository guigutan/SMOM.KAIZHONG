SIE.defineCommand('SIE.Web.DIST.Distribution.Commands.BillLabelPrintCommand', {
    meta: { text: "标签打印", group: "edit", iconCls: "icon-People icon-blue" },
    extend: 'SIE.cmd.Edit',
    canExecute: function (view) {
        if (view.getCurrent() == null) {//选中项修改
            return false;
        }
        var entity = view.getCurrent().data;
        if (entity.PrintQty < (entity.ReturnQty + entity.NgReturnQty)) {//选中项的配送数量小于等于0才能修改
            return true;
        }
        return false;
    },
    execute: function (view, source) {
        var me = this;
        var meta = null;
        me.initData(view, function (entity) {
            SIE.AutoUI.getMeta({
                async: false,
                isAggt: true,
                isDetail: true,
                model: "SIE.Web.DIST.Distribution.BillLabelPrintViewModel",
                callback: function (res) {
                    var mainBlock;
                    if (res.mainBlock)
                        mainBlock = res.mainBlock;
                    else
                        mainBlock = res;
                    var detailView = SIE.AutoUI.createDetailView(mainBlock, entity);
                    var ui = detailView.getControl();
                    SIE.Window.show({
                        title: "标签打印".t(),
                        buttons: ['生成'.t(), '打印'.t()],
                        width: 600,
                        height: 280,
                        items: ui,
                        callback: function (btn) {
                            if (btn == "生成".t()) {
                                SIE.Msg.showError("暂时生成功能未实现".t());
                            }
                            if (btn == "打印".t()) {
                                SIE.Msg.showError("暂时打印功能未实现".t());
                            }
                            return false;
                        }
                    });
                }
            });
        }); 
    },

    initData: function (view, callback) {
        var entitys = view.getSelection();
        var model = SIE.getModel('SIE.Web.DIST.Distribution.BillLabelPrintViewModel');
        var newEntity = new model();
        var indata = {};
        indata.data = Ext.encode(entitys[0].data);
        SIE.invokeCommand({
            async: false,
            data: indata,
            cmd: "SIE.Web.DIST.Distribution.Commands.BillLabelPrintCommand",
            token: view.token,
            callback: function (res) {
                if (res.Success !== true) {
                    SIE.MessageBox.showError(res.Message);
                    return;
                }
                var info = res.Result;
                newEntity.setNGLabelNumber(info.NGLabelNumber);
                newEntity.setItemCode(info.ItemCode);
                newEntity.setItemName(info.ItemName);
                newEntity.setNgReturnQty(info.NgReturnQty);
                newEntity.setReturnQty(info.ReturnQty);
                newEntity.setNGLabelNumber(info.NGLabelNumber);
                callback(newEntity); 
            }
        });
    }
});