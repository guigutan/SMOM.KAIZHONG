SIE.defineCommand('SIE.Web.ShipPlan.Commands.KittingCommand', {
    ////extend: 'SIE.cmd.Save',
    meta: { text: "齐套分析", group: "edit", iconCls: "icon-CalendarOutline icon-blue" },
    _childData: null,
    _oldPlanId: null,
    childView:null,
    canExecute: function (view) {
        if (view.getSelection().length == 0) {
            return false;
        }
        return true;
        //return view.getData().count() > 0 && view.getData().first().data.ProjectDesc != "" && view.getData().first().data.WarehouseIds != "";//&& view.getData().first().data.ProjectDesc == ""
    },
    execute: function (view, source) {
        var that = this;
        var datas = view.getSelection();
        var flag = datas.any(function (item) {
            return item.data.CreateQty == 0 || (item.getState() == SIE.Web.ShipPlan.DeliveryState.Created || item.getState() == SIE.Web.ShipPlan.DeliveryState.Cancel || item.getState() == SIE.Web.ShipPlan.DeliveryState.Finished) || !item.data.WarehouseId
        });
        if (flag) {
            SIE.Msg.showError("存在需创单数为0或单据状态为创建、完成、取消或发货仓库为空的备料计划!".t());
            return;
        };
        SIE.AutoUI.getMeta({
            async: false,
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            token: view.token,
            model: "SIE.LES.StockPlans.ViewModels.StockPlanDetailViewModel",
            //module:"SIE.ShipPlan.DeliveryPlan,SIE.ShipPlan",
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var model = SIE.getModel('SIE.LES.StockPlans.ViewModels.StockPlanDetailViewModel');
                var newModel = new model();
                detailView.setData(newModel);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "提醒".t(),
                    width: 320,
                    height: 180,
                    items: ui,
                    buttons: [
                        {
                            xtype: "button", text: "确定".t(), handler: function () {
                                var cur = detailView._current.data;
                                win.close();
                                SIE.Msg.wait("正在分配......".t());
                                view.execute({
                                    timeout: 60000,
                                    data: { stockIds: view.getSelectionIds(), BuyOnLoad: cur.BuyOnLoad, MakeOnLoad: cur.MakeOnLoad },
                                    success: function (res2) { //回调
                                        view.loadData();
                                        //console.log("returnData", res2);
                                        SIE.Msg.showInstantMessage("分析成功!".t());
                                        that.showKittingList(view.token, res2.Result,view);
                                    },
                                    error: function (res) {
                                        SIE.Msg.showMessage(res.Message);
                                    },
                                });
                            }
                        },
                    ],
                });
            }
        });
    },


    //展示齐套分析后的数据
    showKittingList: function (token, data, view) {
        var that = this;
        this._childData = data.stockPlanAssigns;
        SIE.AutoUI.getMeta({
            ignoreCommands: false,
            isAggt: true,
            ignoreQuery: true,
            viewGroup: "StockPlanKittingViewGroup",
            token: token,
            model: "SIE.ShipPlan.DeliveryPlan",
            //module: view.module,
            callback: function (res) {
                var listView = SIE.AutoUI.generateAggtControl(res);
                //主页面
                var View = listView._view;
                //子页面
                var newStore = Ext.create('Ext.data.Store', {
                    model: "SIE.ShipPlan.DeliveryPlan",
                    data: data.stockPlans
                });
                that.childView = View._children[0];
                //View._onCurrentChanged = that.meCurrentChange.bind(that);
                View._onCurrentChanged = function (View) {
                    var currentData = this._current.data;
                    if (currentData) {
                        that.setAssignListView(that.childView, currentData);
                    }
                }
                View.getControl().setStore(newStore);
                that.setAssignListView(that.childView, data.stockPlans[0]);
                var ui = listView.getControl();
                var win = SIE.Window.show({
                    title: "齐套分析结果".t(),
                    height: "90%",
                    width: "90%",
                    items: ui,
                    buttons: [],
                });
            }
        });
    },
    //展示预分配的子表数据
    setAssignListView:function(childView, selectData) {
        var that = this;
        var stockPlanId = selectData.Id;
        //if (that._oldPlanId == stockPlanId) {
        //    return;
        //}
        var data = [];
        that._childData.forEach(function (item) {
            if (item.StockPlanId == stockPlanId) {
                data.push(item);
            }
        });
        //SIE.Web.ShipPlan.Commands.KittingCommand
        var newStore = Ext.create('Ext.data.Store', {
            model: "SIE.ShipPlan.ViewModels.StockPlanAssignViewModel",
            data: data
        });
        childView.getControl().setStore(newStore);
        that._oldPlanId = stockPlanId;
    },
    meCurrentChange:function (View) {
        var that = this;        
        var currentData = that._current.data;
        if (currentData) {
            that.setAssignListView(that.childView, currentData);
        }
    }
});