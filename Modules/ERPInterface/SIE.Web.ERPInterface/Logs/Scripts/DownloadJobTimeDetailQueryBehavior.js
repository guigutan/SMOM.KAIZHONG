Ext.define('SIE.Web.ERPInterface.Scripts.DownloadJobTimeDetailQueryBehavior',
    {
        /**
        * view生命周期函数--view生成前
        * @param {*} meta 实体视图元数据
        * @param {*} curEntity 当前操作实体(可空)
        */
        beforeCreate: function (meta, curEntity) {
            var toolBar = meta.gridConfig.dockedItems.first(function (p) { return p.xtype == "toolbar" });
            var me = this;
            if (toolBar.items)
                toolBar.items.push({
                    xtype: 'textfield',
                    name: 'txtRequestStr',
                    fieldLabel: '搜索请求报文'.t(),
                    labelAlign: 'right',
                    emptyText: '搜索请求报文内容或回车确认',
                    minWidth: 400,
                    enableKeyEvents: true,
                    listeners: {
                        keyup: me.onKeyUp
                    }
                });
        },

        /**SN输入框输入 */
        onKeyUp: function (ctr, e, opt) {
            if (e.keyCode == 13 || e.keyCode == 108) {
                //回车
                var val = ctr.value.trim();
                var view = this.up('grid').SIEView;
                var bill = view.getParent().getCurrent();
                if (bill == null) return;

                //添加新抽样明细记录方法
                var addNewDownloadJobTimeDetailFun = function (dtlView, models) {
                    dtlView.getControl().getStore().clearData();
                    dtlView.getControl().setStore(dtlView.getControl().getStore());

                    for (let index = 0; index < models.items.length; index++) {
                        let dtl = models.items[index].data;
                        var record = new (dtlView.getModel());
                        record.generateId();
                        record.setState(dtl.State);
                        record.setErpBatchId(dtl.ErpBatchId);
                        record.setRequestStr(dtl.RequestStr);
                        record.setResponseStr(dtl.ResponseStr);
                        record.setRequestDate(dtl.RequestDate);
                        record.setResponseDate(dtl.ResponseDate);
                        record.setResponseCode(dtl.ResponseCode);
                        record.setResponseMessage(dtl.ResponseMessage);
                        record.setDownloadJobTimeId(dtl.DownloadJobTimeId);
                        dtlView.getControl().getStore().getData().insert(index, record)
                        bill.dirty = true;
                        view.getParent().syncCmdState(view.getParent(), true);
                        record.markSaved();
                    };
                };

                //校验SN是否存在于报检单据的条码中
                SIE.invokeDataQuery({
                    type: "SIE.Web.ERPInterface.Logs.DataQueryer.DownloadExcDataQueryer",
                    method: "GetRequestStr",
                    params: [bill.getId(), val],
                    token: view.getToken(),
                    success: function (res) {
                        if (res.Success && res.Result.data.length > 0) {
                            addNewDownloadJobTimeDetailFun(view, res.Result.data);
                        }
                    }
                });
            }
        },

        /**
         * 加载数据
         * @param {any} view
         */
        onDataLoaded: function (view) {

        }
    });

