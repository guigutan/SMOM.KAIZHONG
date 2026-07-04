Ext.define("SIE.Web.LES.LesStockCounts.Scripts.LesStockCountDtlAction", {
    statics: {
        onEntityPropertyChanged: function (e) {
            var me = this;
            var entity = e.entity;
            var data = e.entity.data;
            var value = e.value;
            if (e.property.length > 0 && !(e.entity.getData()[e.property] == e.oldvalue)) {
                if (e.property == 'ActualCountQty') {
                    if (data.ActualCountQty != null) {
                        entity.setDiffCountQty(((data.ActualCountQty*1000000000) - (data.Qty*1000000000))/1000000000);
                        var employee = CRT.Context.GlobalContext.getContext('userInfo');
                        entity.setCountById_Display(employee.Name);
                        entity.setCountById(employee.EmployeeId);
                        entity.setCountDate(new Date());
                        if (entity.getDiffCountQty() == 0) {
                            entity.setLesStockCountDetailResult(0)
                        } else {
                            entity.setLesStockCountDetailResult(10)
                        }
                    }
                    else {
                        entity.setCountById(null);
                        entity.setCountDate(null);
                        entity.setDiffCountQty(null);
                        entity.setLesStockCountDetailResult(null);
                    }
                }
                if (e.property == 'AnalysisResult')
                {
                    if (data.AnalysisResult != null) {
                        if (data.AnalysisResult == 0) {
                            entity.setIsAllowAdjust(true);
                        } else {
                            entity.setIsAllowAdjust(false);
                        }
                    } else {
                        entity.setIsAllowAdjust(false);
                    }
                    
                }
                if (e.property == 'LesStockCountDetailResult' && data.LesStockCountDetailResult != null && data.LesStockCountDetailResult == 0) {
                    entity.setAnalysisResult(null);
                    entity.setResultDesc(null);
                }
            }
        },
        adjustPropertyChanged: function (e) {
            var entity = e.entity;
            var data = e.entity.data;
            if (e.property.length > 0 && !(e.entity.getData()[e.property] == e.oldvalue)) {
                if (e.property == 'ActualyQty') {
                    var actQty = entity.data.ActualQty;
                    var qty = entity.data.Qty;
                    entity.setDiffQty(qty - actQty);
                }
            }
        }
    }
});