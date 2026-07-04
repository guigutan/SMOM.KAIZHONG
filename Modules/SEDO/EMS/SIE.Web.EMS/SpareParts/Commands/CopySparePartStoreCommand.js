SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.CopySparePartStoreCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", splitTo: "添加", iconCls: "icon-AddEntity icon-green" },
    onEditting: function (entity) {
        var me = this;
        if (entity) {
            var para = {
                IsStoreCode: true,
                DtlCount: 0
            }

            //生成单号
            me.view.execute({
                data: para,
                success: function (res) {
                    entity.setStoreCode(res.Result);
                    entity.setStoreStatus(0);

                    //子表生成新序列或，清空孙表数据
                    var dtllist = entity.StoreDetailList().data;
                    if (dtllist.items.length > 0) {

                        //生成批次号
                        para.IsStoreCode = false;
                        para.DtlCount = dtllist.items.length; 
                        me.view.execute({
                            data: para,
                            success: function (res) {

                                var batchNos = res.Result;
                                for (i = 0; i < dtllist.items.length; i++) {
                                    dtllist.items[i].OrderNumberList().data.items.removeAll();
                                    dtllist.items[i].setBatchNumber(batchNos[i]);
                                    dtllist.items[i].setStoreStatus(0);
                                };
                            }
                        }, me.view);


                    };
                }
            }, me.view);
        }
    }
});