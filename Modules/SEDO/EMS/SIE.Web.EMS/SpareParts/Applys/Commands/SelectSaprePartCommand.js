SIE.defineCommand('SIE.Web.EMS.SpareParts.Applys.Commands.SelectSaprePartCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'SparePartId',
            targetClassName: 'SIE.EMS.SpareParts.SparePart',
            //targetCriteriaClassName: 'SIE.EMS.SpareParts.Criterias.SparePartByEquipModelCriteria'
        },
    },
    canExecute: function (view) {
        
        var parent = view._parent;
        
        if (parent == null || parent.getCurrent() == null) {
            return false;
        }

        var state = parent.getCurrent().getAuditState();

        if (state != 0 && state != 2) {
            return false;
        }
        return true;
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        //获取页面弹选框新勾选的数据
        var newSelectedDatas = this._targetSelectItems.items;
        //按钮所在view
        var ownerView = this._ownerView;
        for (var i = 0; i < newSelectedDatas.length; i++) {
            //创建新的
            var newItem = ownerView.createNewItem();

            var sparePartId = newSelectedDatas[i].getId();
            var sparePartId_Display = newSelectedDatas[i].getSparePartCode()
            var sparePartName = newSelectedDatas[i].getSparePartName();
            var specification = newSelectedDatas[i].getSpecification();
            newItem.setSparePartId(sparePartId);
            newItem.setSparePartId_Display(sparePartId_Display);
            newItem.setSparePartName(sparePartName);
            newItem.setSpecification(specification);            
            newItem.setEquipModelCode(newSelectedDatas[i].getEquipModel());
            newItem.setUnitName(newSelectedDatas[i].getUnitName());

            //绑定事件属性变更事件            
            win.mon(newItem, 'propertyChanged', this._propertyChanged, win);

        }
        //刷新页面，不刷新会导致学生姓名显示不了
        ownerView.refresh();
        //关闭页面
        win.close();
    },
    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        me._sourceViewSelectItems = this.cloneStore.collect(me.dataParams.specKeyPrototyName);
        var dialogView = me._targetView;
        if (me._targetView !== null) {
            var store = dialogView.getData();
            if (store !== null) {
                me.mon(store, 'load', me.onLoad, this);
                if (dialogView._relations[0]) { //存在查询面板时
                    var clearCM = me._targetView.getConditionView().getCmdControl("SIE.cmd.ClearCondition");
                    clearCM.setHidden(true);
                    var cmds = me._targetView.getConditionView().getCommands();
                    cmds.items.splice(cmds.items.indexOf(clearCM, 1));
                    cmds.keys.splice(cmds.keys.indexOf("SIE.cmd.ClearCondition", 1));

                    var criteria = dialogView._relations[0]._target.getData();
                    //debugger;
                    criteria.getCreateDate().BeginValue = null;
                    criteria.getCreateDate().EndValue = null;
                    //var EquipModelId = this._ownerView.getParent().getCurrent().getEquipModelId();//查询条件
                    var equipModel = this._ownerView.getParent().getCurrent().getEquipModelId_Display();
                    //var EquipModelCode = this._ownerView.getParent().getCurrent().getEquipModelId_Display();
                    //criteria.setEquipModelId(EquipModelId);//设置查询条件
                    criteria.setEquipModel(equipModel);
                    criteria.setCreateDate();
                    //criteria.setEquipModelCode(EquipModelCode);
                    //criteria.setIsReadOnly(true);
                    //criteria.IsInvalid = true;
                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
    _propertyChanged: function (e) {
        var me = this;

        var entity = e.entity;//变更的实体
        var property = e.property;//变更的字段
        
        if (property == "SparePartDepotId") {
            var depotId = entity.getSparePartDepotId();
            //获取库存数量
            if (depotId != null) {

                //获取备件类型 单位 设备型号
                SIE.invokeDataQuery({
                    method: 'GetPartDepotCount',
                    params: [partId, depotId],
                    async: false,
                    action: 'queryer',
                    type: 'SIE.Web.EMS.SpareParts.Applys.DataQuerys.SparePartAppDataQuery',
                    token: me.token,
                    success: function (res) {
                        if (res.Result == null) {
                            return;
                        }
                        var sparePartSite = res.Result.SparePartSite;
                        var depotAmount = res.Result.DepotAmount;

                        if (res.Result.DepotAmount != null) {
                            entity.setDepotAmount(depotAmount);
                        } else {
                            entity.setDepotAmount();
                        }


                        if (sparePartSite != null) {
                            entity.setSparePartSiteId_Display(sparePartSite.Code);
                            entity.setSparePartSiteId(sparePartSite.Id);
                        } else {
                            entity.setSparePartSiteId_Display();
                            entity.setSparePartSiteId();
                        }
                    }
                });
            }
        }
    },
});