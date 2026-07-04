SIE.defineCommand('SIE.Web.CSM.Suppliers.Commands.AddSupplierCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        me = this;
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    var data = res.Result;
                    entity.setOutsourcingInLocId_Display(data.OutsourcingInLocCode);
                    entity.setOutsourcingInLocId(data.OutsourcingInLocId);
                    entity.setOutsourcingOutLocId_Display(data.OutsourcingOutLocCode);
                    //entity.setOutsourcingInLoc(data.OutsourcingInLoc);
                    entity.setOutsourcingOutLocId(data.OutsourcingOutLocId);
                    //entity.setOutsourcingOutLoc(data.OutsourcingOutLoc);
                    entity.setOutsourcingReceive(data.OutsourcingReceiveType);
                    entity.setOutsourcingUseTime(data.OutsourcingUseTime);
                    entity.setIsHasStorer(data.IsHasStorer);
                }
            }, me.view);

            //this.mon(entity, 'propertyChanged', SIE.Web.CSM.AddressAction.onEntityPropertyChanged, this);
        }
    },
});