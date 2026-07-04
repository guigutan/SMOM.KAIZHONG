Ext.define('SIE.Web.EMS.SpareParts.Behaviors.ScanStoreDetailBehavior',
    {
        /**
        * view生命周期函数--view生成后
        * @param {*} view 生成的view
        */
        onCreated: function (view) {

            var entity = CRT.Context.PageContext.getCurrentRecord();

            if (!entity) {
                entity = new view._model();
            }

            if (entity.data.CreateDate == null) {
                SIE.invokeDataQuery({
                    method: 'GetSparePartStoreNo',
                    params: [],
                    async: false,
                    action: 'queryer',
                    type: 'SIE.Web.EMS.SpareParts.DataQuery.SparePartDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Success) {
                            entity.setStoreCode(res.Result);
                        }
                    }
                });
            }
        },
        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {
            var me = this;
            var entity = view.getCurrent();
            entity.data["IsExistDetail"] = false;//标记当前界面是否有出库明细数据
            entity.data["InboundStatus"] = 10;//设置入库初始状态为待入库
            view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
        },
        onEntityPropertyChanged: function (e) {

            if (e.property === 'StorePartType' || e.property === 'WarehouseId' || e.property === 'SparePartId'
                || e.property === 'QualityStatus' || e.property === 'Number' || e.property === 'StorageLocationId') {

                if (e.entity.data.StorePartType == null || e.entity.data.WarehouseId == null) {
                    e.entity.setMessage("请先维护【拆机件/原件】/【仓库】！".t());
                }
                else {
                    setTimeout(function () {

                        if (e.entity.data.SparePartId != null) {

                            if (e.entity.data.IsSelectSparePart) {

                                //勾选是否生成新标签
                                e.entity.setIsCreateNewLabel(false);
                                if (e.entity.data.StorePartType == 10
                                    && (e.entity.data.ControlMethod == 20 && (e.entity.data.PartOutDepotDetailId == null || e.entity.data.PartOutDepotDetailId == 0))) {
                                    e.entity.setIsCreateNewLabel(true);
                                }
                                if (e.entity.data.StorePartType == 5 && e.entity.data.ControlMethod == 20) {
                                    e.entity.setIsCreateNewLabel(true);
                                }

                                if (e.entity.data.ControlMethod == 10) {
                                    e.entity.data.ScanValue = e.entity.data.SparePartId_Display;
                                }
                                if (e.entity.data.ControlMethod == 20) {
                                    e.entity.setMessage("请扫描批次号！".t());
                                    e.entity.data.ScanValue = null;
                                }
                                if (e.entity.data.ControlMethod == 30) {
                                    e.entity.setMessage("请扫描序列号！".t());
                                    e.entity.data.ScanValue = null;
                                }
                            }

                            if (!Ext.isEmpty(e.entity.data.ScanValue) || e.entity.data.IsCreateNewLabel) {
                                if (e.entity.data.QualityStatus == null) {
                                    e.entity.setMessage("请维护本次入库备件的质量状态！".t());
                                }
                                else if (e.entity.data.Number == null) {
                                    e.entity.setMessage("请维护本次入库备件的入库数量！".t());
                                }
                                else if (e.entity.data.StorageLocationId == null) {
                                    e.entity.setMessage("请维护本次入库备件的库位！".t());
                                }
                                else {
                                    e.entity.setMessage("请点击确认按钮生成入库明细！".t());
                                }
                            }
                        }
                        else {
                            e.entity.data.ScanValue = null;
                            e.entity.setMessage("请扫描【序列号】/【批次号】/【备件编码】！".t());
                        }
                    }, 0);
                }
            }

            if (e.property === 'PartOutDepotDetailId') {

                //备件管控方式：批次号
                if (e.entity.data.ControlMethod == 20) {
                    //20 拆机件
                    if (e.entity.data.StorePartType == 5) {
                        //	表头选择了拆机件，有无选择出库单都默认勾选，不可修改
                        e.entity.setIsCreateNewLabel(true);
                    } else {

                        //10 原件或空
                        if (e.entity.data.PartOutDepotDetailId != null) {
                            //	表头选择了原件且选择了出库单则默认不勾选，不可修改
                            e.entity.setIsCreateNewLabel(false);
                        }
                        else {
                            //	表头选择了原件但没有选择出库单则默认勾选，不可修改
                            e.entity.setIsCreateNewLabel(true);
                        }
                    }
                }

                if (e.entity.data.PartOutDepotDetailId != null) {

                    setTimeout(function () {
                        var dtlChildView = e.entity.belongsView.findChild('SIE.EMS.SpareParts.StoreDetail');
                        var dtlStore = dtlChildView.getData();

                        var numberList = dtlStore.getData().items.where(function (p) { return p.getPartOutDepotDetailId() == e.entity.data.PartOutDepotDetailId; })
                            .select(function (p) { return parseInt(p.getNumber()); });
                        var number = numberList.sum();
                        console.log(e.entity.data.CanReturnQty);
                        console.log(number);
                        e.entity.setCanReturnQty(e.entity.data.CanReturnQty - number);
                    }, 0);

                }
                else {
                    e.entity.setCanReturnQty(null);
                }
            }
        }
    });
