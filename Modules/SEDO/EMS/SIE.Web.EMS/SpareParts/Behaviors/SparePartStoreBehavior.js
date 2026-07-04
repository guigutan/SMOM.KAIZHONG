Ext.define('SIE.Web.EMS.SpareParts.Behaviors.SparePartStoreBehavior', {
        /**
        * view生命周期函数--view生成后
        * @param {*} view 生成的view
        */
        onCreated: function (view) {
            var entity = CRT.Context.PageContext.getCurrentRecord();
            var params = CRT.Context.PageContext.getParams();

            if (params) {
                entity.setId(params.Id);
                entity.setStoreCode(params.StoreCode);
                entity.setInboundType(params.InboundType);
                entity.setWarehouseId(params.WarehouseId);
                entity.setWarehouseId_Display(params.WarehouseId_Display);
                entity.setQualityStatus(params.QualityStatus);
                entity.setMessage("请先维护【库位】！".t());
                entity.setCreateDate(params.CreateDate);
            }
        },
        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
    onViewReady: function (view) {

            var me = this;
            var entity = view.getCurrent();

            //加载出库明细的Store
            var applyChildView = view.findChild('SIE.EMS.SpareParts.StoreDetail');
            var tabPanel = applyChildView.getControl().ownerCt.ownerCt;
            tabPanel.setActiveTab(0);
            view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
        },
        onEntityPropertyChanged: function (e) {

            if (e.property === 'StorageLocationId' || e.property === 'QualityStatus' || e.property === 'SparePartId') {

                if (e.entity.data.StorageLocationId == null) {
                    e.entity.setMessage("请先维护【库位】！".t());
                }
                else if (e.entity.data.QualityStatus == null) {
                    e.entity.setMessage("请先维护【质量状态】！".t());
                }
                else {
                    setTimeout(function () {
                        if (e.entity.data.SparePartId != null) {

                            if (e.entity.data.IsSelectSparePart) {
                                if (e.entity.data.ControlMethod == 10) {

                                    var dtlChildView = e.entity.belongsView.findChild('SIE.EMS.SpareParts.StoreDetail');
                                    var dtlStore = dtlChildView.getData();

                                    for (var i = 0; i < dtlStore.getCount(); i++) {
                                        var record = dtlStore.getAt(i);
                                        if (record.data.SparePartId == e.entity.data.SparePartId && record.data.QualityStatus == e.entity.data.QualityStatus) {
                                            record.setUnitPrice(e.entity.data.UnitPrice);
                                            record.setStorageLocationId_Display(e.entity.data.StorageLocationId_Display);
                                            record.setStorageLocationId(e.entity.data.StorageLocationId);
                                        }
                                    }
                                    e.entity.setMessage("入库成功，请继续扫描【序列号】/【批次号】/【备件编码】！".t());
                                }
                                if (e.entity.data.ControlMethod == 20) {
                                    e.entity.setMessage("请扫描批次号！".t());
                                }
                                if (e.entity.data.ControlMethod == 30) {
                                    e.entity.setMessage("请扫描序列号！".t());
                                }
                            }
                        }
                        else {
                            e.entity.setMessage("请扫描【序列号】/【批次号】/【备件编码】！".t());
                        }
                    }, 0);
                }
            }
        }
    });
