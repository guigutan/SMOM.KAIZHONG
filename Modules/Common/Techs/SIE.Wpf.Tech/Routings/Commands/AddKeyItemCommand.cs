using SIE.Domain;
using SIE.Items;
using SIE.Tech.Routings;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.Tech.Routings.Commands
{
    /// <summary>
    /// 添加关键物料 按钮
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", GroupType = CommandGroupType.Edit)]
    public class AddKeyItemCommand : ListViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">界面</param>
        public override void Execute(ListLogicalView view)
        {
            var itemList = RT.Service.Resolve<ItemController>().GetItems();
            var keyItemList = RT.Service.Resolve<RoutingController>().GetKeyItems();
            var keyItemIds = keyItemList.Select(o => o.Code).ToList();
            var notKeyItemList = new EntityList<Item>();
            notKeyItemList.AddRange(itemList.Where(o => !keyItemIds.Contains(o.Code)));

            var listView = AutoUI.ViewFactory.CreateListView(typeof(Item));
            listView.Data = notKeyItemList;
            CRT.Workbench.ShowDialog(listView, w =>
            {
                w.Title = "添加 关键物料".L10N();
                listView.Control.MouseDoubleClick += (o, e) =>
                {
                    if (listView.Current != null)
                    {
                        w.Close(0);
                    }
                };

                w.Closed += (o, e) =>
                {
                    if (w.Result == 0 && listView.Current != null && listView.SelectedEntities.Count > 0)
                    {
                        var items = new EntityList<Item>();
                        var clist = new EntityList<KeyItem>();
                        items.AddRange(listView.SelectedEntities);
                        items.ForEach(p => p.PersistenceStatus = PersistenceStatus.New);
                        foreach (var item in items)
                        {
                            var keyItem = new KeyItem();
                            keyItem.Item = item;
                            keyItem.Code = item.Code;
                            keyItem.Name = item.Name;
                            keyItem.Description = item.Description;
                            clist.Add(keyItem);
                        }

                        RF.Save(clist);
                        view.QueryView.TryExecuteQuery();
                    }
                };
            });
        }
    }
}
