SIE.defineCommand('SIE.Web.MES.Outsourcing.Commands.InStockAddCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "添加可入库在制品", group: "business", iconCls: "icon-AddEntity icon-green" },

    canExecute: function (view) {
        const outsourcingRequest = view._parent.getCurrent();
        if (outsourcingRequest == null) {
            return false;
        }
        if (view._parent != null && view._parent.getCurrent() != null && view._parent.getCurrent().getData().OutsourcingState == 20 ) {
            return false;
        }
        return !(outsourcingRequest.isNew());
    },
    execute: function (view, source) {
        const me = this;
        me._progressBar = new Ext.ProgressBar({
            renderTo: Ext.getBody(),
            width: 988,
        });
        me._progressBar.show();
        me._progressBar.wait({
            interval: 100,
            duration: 36000000,
            text: '正在添加【在制品委外入库】，请稍候...'.t(),
            increment: 10,
            scope: this,
            fn: function () {

            }
        });

        const outsourcingRequest = view._parent.getCurrent().data;        
      
        view.execute({
            data: outsourcingRequest,
            withIds: false,
            success: function (res) {                
                me._progressBar.hide();
                SIE.Msg.showMessage(res.Result.ReturnMessage);
                view.reloadData();
            }
        });

    }
});