using Jam.Runtime.Event;
using Jam.Runtime.Helpers;

namespace Jam.Runtime.UI_
{

    public partial class MoneyWidget
    {
        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            G.Event.Subscribe<int, int>(GlobalEventId.MoneyAdd, OnMoneyUpdate);
            G.Event.Subscribe<int, int>(GlobalEventId.MoneyCost, OnMoneyUpdate);
            G.Event.Subscribe(GlobalEventId.MoneyUpdateAll, OnMoneyUpdateAll);
            Refresh();
        }

        public override void OnClose()
        {
            G.Event.Unsubscribe<int, int>(GlobalEventId.MoneyAdd, OnMoneyUpdate);
            G.Event.Unsubscribe<int, int>(GlobalEventId.MoneyCost, OnMoneyUpdate);
            G.Event.Unsubscribe(GlobalEventId.MoneyUpdateAll, OnMoneyUpdateAll);
        }

        private void Refresh()
        {
            _node_gold.gameObject.SetActive(false);
            _node_gem.gameObject.SetActive(false);

            var gold = MoneyBagHelper.GetMoneyById(1);
            // var gem = MoneyBagHelper.GetMoneyById(2);
            if (gold != null)
            {
                _node_gold.gameObject.SetActive(true);
                _txt_gold.text = gold.amount.ToString();
            }
            // if (gem != null)
            // {
            //     _node_gem.gameObject.SetActive(true);
            //     _txt_gem.text = gem.amount.ToString();
            // }
        }

        private void OnMoneyUpdateAll()
        {
            Refresh();
        }

        private void OnMoneyUpdate(int id, int amount)
        {
            Refresh();
        }
    }

}