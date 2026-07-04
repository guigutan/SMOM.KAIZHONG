Ext.define('SIE.Web.Packages.QrCodeParseRules.QrCodeParseRuleDetailBehavior',
    {
        /**
        * view生命周期函数--view生成前
        * @param {*} meta 实体视图元数据
        * @param {*} curEntity 当前操作实体(可空)
        */
        beforeCreate: function (meta, curEntity) {
            var toolBar = meta.gridConfig.dockedItems.first(function (p) { return p.xtype == "toolbar" });
            var me = this;
            if (toolBar && toolBar.items) {
                toolBar.items.push({
                    xtype: 'textfield',
                    name: 'txtReelId',
                    hideLabel: true,
                    emptyText: '请扫描二维码测试',
                    minWidth: 400,
                    enableKeyEvents: true,
                    listeners: {
                        keyup: me.onKeyUp
                    }
                });
            }
        },

        /**SN输入框输入 */
        onKeyUp: function (ctr, e, opt) {
            if (e.keyCode == 13 || e.keyCode == 108) {
                //回车
                var val = ctr.value.trim();
                if (Ext.isEmpty(val)) return;
                var view = this.up('grid').SIEView;
                var bill = view.getParent().getCurrent();
                if (bill.data.CreateBy > 0) {
                    //校验SN是否存在于报检单据的条码中
                    SIE.invokeDataQuery({
                        type: "SIE.Web.Packages.QrCodeParseRules.DataQueryer.QrCodeDataQueryer",
                        method: "QrCodeTest",
                        params: [bill.getId(), val],
                        token: view.getToken(),
                        success: function (res) {
                            if (res.Result && res.Result.length > 0) {
                                for (var i = 0; i < res.Result.length; i++) {
                                    var item = res.Result[i];
                                    var rec = view.getData().data.items.first(function (f) { return f.data.ParseField == item.QrCodeKeyVal; });
                                    rec.setTestResult(item.QrCodeValue);
                                    rec.markSaved();
                                }
                            }
                            else
                                SIE.Msg.showMessage("没有匹配结果".t())
                        }
                    });
                }
                else {
                    SIE.Msg.showMessage("请先保存规则后扫描".t())
                }
            }
        },

        /**
         * 加载数据
         * @param {any} view
         */
        onDataLoaded: function (view) {
        }
    });

