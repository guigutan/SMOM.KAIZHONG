Ext.define('SIE.Web.Fixtures.Accounts.Scripts.AddCodeAccBehavior',
    {
        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {
            var me = this;  
            var params = CRT.Context.PageContext.getParams();
            if (params) {
                view.tabId = params.tabId;
            }          
            var encodeControl = view.getControl().query('[name=FixtureEncodeId]')[0];           
            encodeControl.page = me;
            encodeControl.mainView = view;
            encodeControl.on('select', me.onSelectChanged);
        },

        /**
         * onSelectChanged 下拉选择变更事件
         * @param {*} combox 下拉框
         * @param {*} selectionRows 选中对象
         * @param {*} oldValue 选中前值
         * @param {*} eOpts eOpts
         */
        onSelectChanged: function (combox, selectionRows, oldValue, eOpts) {
            var me = this;
            var page = me.page;
            var entity = me.mainView.getCurrent();
            if (selectionRows && selectionRows.length > 0) {
                entity.setCode(selectionRows[0].getCode());
                var encodeId = selectionRows[0].getId();
                page.loadAccountState(me.mainView, encodeId);
            }
            else {
                entity.setCode('');
                entity.setAccountState(null);
            }
        },

        /**
         * loadAccountState 获取台账状态
         * @param {*} view 当前视图
         * @param {*} encodeId 工治具编码Id
         */
        loadAccountState: function (view, encodeId) {
            var entity = view.getCurrent();
            SIE.invokeDataQuery({
                type: "SIE.Web.Fixtures.Accounts.DataQuery.AccountDataQueryer",
                method: "GetAddCodeAccInfo",
                params: [encodeId],
                async: false,
                token: view.token,
                callback: function (res) {
                    if (res.Success) {
                        var data = res.Result;
                        entity.setAccountState(data.State);
                        if (data.Account) {
                            entity.setManufacturer(data.Account.Manufacturer);
                            entity.setAssetCode(data.Account.AssetCode);
                            entity.setOriginalSN(data.Account.OriginalSN);
                            entity.setProprietorship(data.Account.Proprietorship);
                            entity.setSupplierId(data.Account.SupplierId);
                            entity.setSupplierId_Display(data.Account.SupplierCode);
                            entity.setSupplierName(data.Account.SupplierName);
                            entity.setCustomerId(data.Account.CustomerId);
                            entity.setCustomerId_Display(data.Account.CustomerCode);
                            entity.setCustomerName(data.Account.CustomerName);
                            entity.setQty("");
                            entity.setUnitPrice("");
                        }
                        else {
                            entity.setManufacturer("");
                            entity.setAssetCode("");
                            entity.setOriginalSN("");
                            //entity.setProprietorship(null);
                            entity.setSupplierId(null);
                            entity.setSupplierId_Display("");
                            entity.setSupplierName("");
                            entity.setCustomerId(null);
                            entity.setCustomerId_Display("");
                            entity.setCustomerName("");
                            entity.setQty("");
                            entity.setUnitPrice("");
                        }
                    }
                },
            });
        }
    });