Ext.define('SIE.Web.EMS.SpareParts.OutDepots.Behaviors.PickOutDepotBehavior',
    {
        /**
        * view生命周期函数--view生成后
        * @param {*} view 生成的view
        */
        onCreated: function (view) {
            var entity = CRT.Context.PageContext.getCurrentRecord();
            var params = CRT.Context.PageContext.getParams();
            if (params) {
                entity.setId(params.Id);
                entity.setNo(params.No);
                entity.setOutDepotType(params.OutDepotType);
                entity.setWarehouseId(params.WarehouseId);
                entity.setWarehouseId_Display(params.WarehouseId_Display);
                entity.setGetDepartmentId(params.GetDepartmentId);
                entity.setGetDepartmentId_Display(params.GetDepartmentId_Display);
                entity.setQualityStatus(params.QualityStatus);
                entity.setMessage("请扫描序列号/批次号/备件编码！".t());
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
            entity.data["IsBarcode"] = true;//标记当前是否处于扫描条码的状态
            entity.data["IsExistDetail"] = false;//标记当前界面是否有出库明细数据
            view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);

            //加载出库明细的Store
            var applyChildView = view.findChild('SIE.EMS.SpareParts.OutDepots.Details.OutDepotDetail');
            var tabPanel = applyChildView.getControl().ownerCt.ownerCt;
            tabPanel.setActiveTab(1);
            tabPanel.setActiveTab(2);
            tabPanel.setActiveTab(0);
        },
        onEntityPropertyChanged: function (e) {

            if (e.property === 'OutDepotType') {
                var supplierChildView = e.entity.belongsView.findChild('SIE.EMS.SpareParts.OutDepots.OutDepot');
                supplierChildView.getCurrent().setRepairOutDepotType(e.value);
            }

            if (e.property === 'StorageLocationId') {

                if (!Ext.isEmpty(e.entity.data.ScanValue) && e.entity.data.StorageLocationId != null) {

                    e.entity.setIsBarcode(false);
                    e.entity.setMessage("请输入备件的出库数量后回车！".t());
                }
            }

            if (e.property === 'WarehouseId' || e.property === 'QualityStatus' || (e.property === 'SparePartId' && Ext.isEmpty(e.entity.data.ScanValue))) {

                if (e.entity.data.WarehouseId == null || e.entity.data.QualityStatus == null) {
                    e.entity.setMessage("请先维护【出库仓库】/【质量状态】！".t());
                }
                else {
                    if (e.entity.data.SparePartId != null) {

                        setTimeout(function () {
                            if (e.entity.data.ControlMethod == 10) {
                                e.entity.setIsBarcode(false);
                                e.entity.setMessage("请输入备件的出库数量后回车！".t());
                            }
                            if (e.entity.data.ControlMethod == 20) {
                                e.entity.setIsBarcode(true);
                                e.entity.setMessage("请扫描批次号！".t());
                            }
                            if (e.entity.data.ControlMethod == 30) {
                                e.entity.setIsBarcode(true);
                                e.entity.setMessage("请扫描序列号！".t());
                            }
                        }, 0);
                    }
                    else {
                        e.entity.setIsBarcode(true);
                        e.entity.setMessage("请扫描【序列号】/【批次号】/【备件编码】！".t());
                    }
                }
            }
        }
    });
