SIE.defineCommand('SIE.Web.Tech.Processs.Commands.ProcessCollectStepMoveBottom', {
    extend: 'SIE.Web.Common.Sort.Commands.MoveBottomCommand',
    meta: { text: "置底", group: "edit", iconCls: "icon-AlignBottom icon-blue" },
    execute: function (listView, source) {
        var me = listView;
        var data = listView.getData();
        var items = data.data.items;
        var entity = listView.getCurrent();
        var index = this.getItemsIndex(items, entity);
        items[index].data.INDEX_ = items[items.length - 1].data.INDEX_;
        items[index].data.Step = items[items.length - 1].data.Step;
        var EntityMetadataList = [];
        //重新排序
        for (var i = index + 1; i < items.length; i++) {
            items[i].data.INDEX_ = items[i].data.INDEX_ - 1;
            items[i].data.Step = items[i].data.Step - 1;
            EntityMetadataList.push(items[i].data);
        }
        EntityMetadataList.push(items[index].data);
        items.splice(index, 1);
        items.push(entity);
        this.setItems(data, items);

        listView.setCurrent(null);
        listView.setCurrent(entity);
        
        listView.execute({
            data: EntityMetadataList,
            success: function (res) {
                return true;
            }
        });
    },
});