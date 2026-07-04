Ext.define('SIE.Web.Recheck.Common.ItemRecheck.Scripts.RecheckProgramDetailAction', {
    statics: {       
        ReSetDetailData: function (view) {
            var i = 1;
            view.getData().data.items.forEach(function (p) {                
                p.setSort(i);
                p.setRecheckSort("第" + i + "次");
                i++;
            });
            view._parent._current.dirty = true;
            view.syncCmdState(view, true);
        }
    }
});