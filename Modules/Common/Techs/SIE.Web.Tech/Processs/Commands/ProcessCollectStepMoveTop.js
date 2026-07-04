SIE.defineCommand('SIE.Web.Tech.Processs.Commands.ProcessCollectStepMoveTop', {
    extend: 'SIE.Web.Common.Sort.Commands.MoveTopCommand',
    meta: { text: "置顶", group: "edit", iconCls: "icon-AlignTop icon-blue" },
    execute: function (listView, source) {
        var me = listView;
        var data = listView.getData();
        var items = data.data.items;
        var entity = listView.getCurrent();
        var index = this.getItemsIndex(items, entity);
        items[index].data.INDEX_ = items[0].data.INDEX_;
        items[index].data.Step = items[0].data.Step;

        var EntityMetadataList = [];
        EntityMetadataList.push(items[index].data);
        //重新排序
        for (var i = 0; i < index; i++) {
            items[i].data.INDEX_ = items[i].data.INDEX_ + 1;
            items[i].data.Step = items[i].data.Step + 1;
            EntityMetadataList.push(items[i].data);
        }
        items.splice(index, 1);
        items.splice(0, 0, entity);
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