Ext.define('SIE.Web.EMS.SpecialEquipment.RegularInspections.RegularInspectionBehavior',
    {
        /**
         * view生命周期函数--view生成前
         * @param {*} meta 实体视图元数据
         * @param {*} curEntity 当前操作实体(可空)
         */
        onCreated: function (view) {

        },

        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {
            //code here
            var me = this;
            var entity = view.getCurrent();
            
            SIE.invokeDataQuery({
                //显示事件编号和事件名称（因为CriticalEventId_Display为空，要重新查询一下）
                type: "SIE.Web.EMS.SpecialEquipment.RegularInspections.DataQueryers.RegularInspectionDataQueryer",
                method: "GetBillInfoById",
                params: [entity.data.Id],
                token: view.getToken(),
                async: false,
                callback: function callback(res) {
                    var billInfo = res.Result;
                    if (res.Success && res.Result !== null) {
                        Ext.apply(entity.data, billInfo);
                    }
                }
            });
            var targetView = view;  //填写报告视图
            if (!this.isTabExist) {
                //如果已打开对应的填写报告页签，则不再处理
                if (view) {(new SIE.Web.EMS.Common.Script.EmsCommonHelper()).getViewController(view, SIE.Web.EMS.SpecialEquipment.RegularInspections.Scripts.RegularInsDtlController); //关联控制器
                }
            }
        },
    });
