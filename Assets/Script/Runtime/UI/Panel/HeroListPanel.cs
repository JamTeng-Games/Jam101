using System.Collections.Generic;
using Jam.Cfg;
using Jam.Runtime.Data_;
using Jam.Runtime.Net_;

namespace Jam.Runtime.UI_
{

    public partial class HeroListPanel
    {
        private List<int> _heroCardWidgetIds;

        public override void OnInit()
        {
            _heroCardWidgetIds = new List<int>(32);
        }

        public override void OnOpen(object userData)
        {
            _btn_back.onClick.AddListener(OnClickBack);
            RefreshHeroes();
        }

        public override void OnClose()
        {
            _btn_back.onClick.RemoveListener(OnClickBack);
            this.ClearWidgets();
            _heroCardWidgetIds.Clear();
        }

        private void RefreshHeroes()
        {
            var heroListCfg = G.Cfg.TbHero.DataList;
            for (int i = 0; i < heroListCfg.Count; i++)
            {
                var heroCfg = heroListCfg[i];
                if (_heroCardWidgetIds.Count <= i)
                {
                    int widgetId = this.AddWidget(UIWidgetId.HeroCard,
                                                  _node_content,
                                                  w => (w as HeroCardWidget).SetClickEvent(OnClickHeroCard),
                                                  userData: heroCfg.Id);
                    _heroCardWidgetIds.Add(widgetId);
                }
                else
                {
                    var widget = this.GetWidget<HeroCardWidget>(_heroCardWidgetIds[i]);
                    widget?.Refresh(heroCfg.Id);
                    widget?.SetClickEvent(OnClickHeroCard);
                }
            }
        }

        private void OnClickHeroCard(HeroCardWidget widget)
        {
            G.Net.Send(NetCmd.CS_ChooseHero, new ChooseHeroReq() { id = widget.HeroCfgId });
            G.UI.BackToHome();
        }

        private void OnClickBack()
        {
            G.UI.Back();
        }
    }

}