SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.AddOrderNumberCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },
    /**
    * 是否允许执行
    * @param {any} view 当前视图
    */
    canExecute: function (view) {
        if (view.getParent().getParent()._selection != null && view.getParent().getParent()._selection.length == 1) {
            if (view.getParent()._selection != null && view.getParent()._selection.length == 1) {
                if (view.getParent()._selection[0].getIsSeqNoCharge() == true && view.getParent().getParent()._selection[0].getStoreStatus() == 0) {
                    //if (view.getParent()._selection[0].getNumber() == view.getData().data.length) return false;
                        return true;//选定行才显示添加属性按钮
                }       
                
            }
        }
        return false;
    },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            entity.setSparePartSiteId_Display(this.view.getParent()._selection[0].getSparePartSiteId_Display());
            entity.setSparePartSiteId(this.view.getParent()._selection[0].getSparePartSiteId());
            if (this.view.getParent().getParent()._selection[0].getStoreType()==2)//入库类型为不良品，序列号状态为不良品
                entity.setOdNbStatus(0);
            else
                entity.setOdNbStatus(1);

        }
    },
});