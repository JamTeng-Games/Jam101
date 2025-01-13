using System;
using Jam.Core;
using Jam.Runtime.Constant;
using UnityEngine;

namespace Jam.Runtime.UI_
{

    public partial class HeroCardWidget
    {
        private int _heroCfgId = 0;
        private Action<HeroCardWidget> _clickAction;

        public int HeroCfgId => _heroCfgId;

        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _btn_hero.onClick.AddListener(OnClickSelf);
            Refresh((int)userData);
        }

        public override void OnClose()
        {
            _btn_hero.onClick.RemoveListener(OnClickSelf);
        }

        protected override void OnTick(float dt)
        {
        }

        public void Refresh(int heroCfgId)
        {
            _heroCfgId = heroCfgId;
            Cfg.Hero heroCfg = G.Cfg.TbHero[heroCfgId];
            // name
            {
                _txt_name.text = heroCfg.Name;
            }
            // icon
            {
                G.Asset.Load(AssetPath.HeroIcon(heroCfg.HeadIcon), typeof(Sprite), assetWrap =>
                {
                    _img_hero.sprite = (Sprite)assetWrap.Asset;
                }, null);
            }
            // quality
            {
                var colorType = heroCfg.Quality.ToString();
                var colorHex = G.Cfg.TbColor[colorType].Value;
                var qualityColor = Utils.Color.FromHex(colorHex);
                _img_quality.color = qualityColor;
                _img_quality_glow.color = qualityColor;
                // _img_quality_gradient.color = qualityColor;
            }
        }

        private void OnClickSelf()
        {
            _clickAction?.Invoke(this);
        }

        public void SetClickEvent(System.Action<HeroCardWidget> action)
        {
            _clickAction = action;
        }

        public void ClearClickEvent()
        {
            _clickAction = null;
        }
    }

}