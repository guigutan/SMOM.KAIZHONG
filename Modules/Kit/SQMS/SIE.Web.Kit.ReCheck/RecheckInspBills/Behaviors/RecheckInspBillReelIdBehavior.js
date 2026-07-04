Ext.define('SIE.Web.Kit.Recheck.RecheckInspBills.Behaviors.RecheckInspBillReelIdBehavior',
    {
        /**
        * view生命周期函数--view生成前
        * @param {*} meta 实体视图元数据
        * @param {*} curEntity 当前操作实体(可空)
        */
        beforeCreate: function (meta, curEntity) {
            var toolBar = meta.gridConfig.dockedItems.first(function (p) { return p.xtype == "toolbar" });
            toolBar.items.push({
                xtype: 'textfield',
                fieldLabel: '扫描ReelID'.t(),
                labelAlign: 'right',
                emptyText: '扫描或回车确认',
                minWidth: 400,
                enableKeyEvents: true,
                listeners: {
                    keyup: this.onKeyUp
                }
            });
        },

        /**SN输入框输入 */
        onKeyUp: function (ctr, e, eOpts) {
            if (e.keyCode == 13 || e.keyCode == 108) {
                //回车
                var val = ctr.value.trim();
                if (Ext.isEmpty(val)) return;
                var view = this.up('grid').SIEView;
                var bill = view.getParent().getCurrent();
                if (bill.getInspectionStatus() === SIE.Enum.QMS.Common.InspectionStatus.Inspectioned)  //单据已检，不可再操作
                    return;
                var store = view.getData();
                if (store && store.find("ReelId", val) >= 0) {
                    SIE.Msg.showInstantMessage("扫描ReelID已经存在。".L10N());
                    return;
                }

                //添加新抽样明细记录方法
                var addNewReelIdDetailFun = function (reelIdView, reelIdVal, qty) {
                    var record = new (reelIdView.getModel());
                    record.generateId();
                    record.setReelId(reelIdVal);
                    record.setQuannity(qty);
                    reelIdView.getData().insert(0, record);
                    ctr.setValue("");
                };

                //校验SN是否存在于报检单据的条码中
                SIE.invokeDataQuery({
                    type: "SIE.Web.Kit.ReCheck.RecheckInspBills.DataQueryers.KitRecheckInspBillsQueryer",
                    method: "GetReelIDInAsnNo",
                    params: [val, bill.getId()],
                    token: view.getToken(),
                    success: function (res) {
                        if (!res.Result)
                            addNewReelIdDetailFun(view, val, null);
                        else {
                            var reelIdInfo = res.Result;
                            if (reelIdInfo)
                                addNewReelIdDetailFun(view, reelIdInfo.ReelID, reelIdInfo.Qty);
                        }
                    }
                });
            }
        },
    });

