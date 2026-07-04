Ext.define('SIE.Web.EMS.ViceTransfers.TransferBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var entity = view.getCurrent();
        var params = CRT.Context.PageContext.getParams();
        var viceAssetObject = 10;
        if (params) {
            me.action = params.action;
            if (params.action === 0) {
                SIE.invokeDataQuery({
                    method: 'GetNewViceTransfers',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.ViceTransfers.ViceTransfersDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Result) {
                            var info = res.Result.data.items[0].data;
                            entity.setTransferNo(info.TransferNo);
                            entity.setTransferStatus(info.TransferStatus);
                            entity.setApprovalStatus(info.ApprovalStatus);
                            entity.setApplyDate(info.ApplyDate);
                            entity.setApplicantId(info.ApplicantId);
                            entity.setApplicantId_Display(info.ApplicantName);
                        }
                    }
                });
            }
        }
        else {
            viceAssetObject = entity.getViceAssetObject();
            entity.setViceAssetObject(0);
        }
        if (entity) {
            view.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, view);
        }
        entity.setViceAssetObject(viceAssetObject);
    },
    _onEntityPropertyChanged: function (e) {
        var data = e.entity;
        if (e.property.length > 0) {
            if (e.property == "ViceAssetObject") {

                var tabPanel = this._children[0].getControl().ownerCt.ownerCt;
                if (!tabPanel) return;
                var tabPanelItems = tabPanel.tabBar.items.items;
                var sparePart = "SIE.EMS.ViceTransfers.ViceTransferSparePart";
                var fixture = "SIE.EMS.ViceTransfers.ViceTransferFixture";
                var i = 0;
                var showIndex = -1;
                Ext.each(tabPanelItems, function (item) {
                    var itemModel = tabPanel.items.items[i].items.items[0].SIEView.model;
                    i++;
                    if (itemModel == sparePart) {
                        e.value == 10 ? item.show() : item.hide();
                        if (e.value == 10 && showIndex < 0)
                            showIndex = 0;
                    }
                    if (itemModel == fixture) {
                        e.value == 20 ? item.show() : item.hide();
                        if (e.value == 20 && showIndex < 0)
                            showIndex = 1;
                    }
                });
                tabPanel.setActiveTab(showIndex);

                if (e.value === 10)//清空所有备件的子页签
                {
                    this._children.find(m => m.model == "SIE.EMS.ViceTransfers.ViceTransferSparePart").getData().data.removeAll()
                }
                else if (e.value === 20)//清空所有备件的子页签
                {
                    this._children.find(m => m.model == "SIE.EMS.ViceTransfers.ViceTransferFixture").getData().data.removeAll()
                }
                else if (e.value === 30) {
                    this._children.find(m => m.model == "SIE.EMS.ViceTransfers.ViceTransferSparePart").getData().data.removeAll()
                    this._children.find(m => m.model == "SIE.EMS.ViceTransfers.ViceTransferFixture").getData().data.removeAll()
                    tabPanel.setActiveTab(2);
                }
                else {

                }
            }
            // 切换来源仓库清空明细
            if (e.property == "WarehouseId") {
                if (data.getViceAssetObject() === 10) {
                    this._children.find(m => m.model == "SIE.EMS.ViceTransfers.ViceTransferSparePart").getData().data.removeAll()
                }
                else if (data.getViceAssetObject() === 20) {
                    this._children.find(m => m.model == "SIE.EMS.ViceTransfers.ViceTransferFixture").getData().data.removeAll()
                }
                else if (data.getViceAssetObject() === 30) {
                    this._children.find(m => m.model == "SIE.EMS.ViceTransfers.ViceTransferSparePart").getData().data.removeAll()
                    this._children.find(m => m.model == "SIE.EMS.ViceTransfers.ViceTransferFixture").getData().data.removeAll()
                }
                else {

                }
            }
        }
    },
});