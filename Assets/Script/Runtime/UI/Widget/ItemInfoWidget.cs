using Jam.Cfg;
using Jam.Runtime.Constant;
using UnityEngine;

namespace Jam.Runtime.UI_
{

    public partial class ItemInfoWidget
    {
        private int _itemCfgId;

        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            Refresh((int)userData);
        }

        public override void OnClose()
        {
            _itemCfgId = 0;
        }

        public void Refresh(int itemCfgId)
        {
            _itemCfgId = itemCfgId;
            if (_itemCfgId == 0)
            {
                ShowEmptyInfo();
                return;
            }

            HideEmptyInfo();
            Cfg.Item itemCfg = G.Cfg.TbItem[itemCfgId];
            // icon
            {
                G.Asset.Load(AssetPath.ItemIcon(itemCfg.Icon), typeof(Sprite), assetWrap =>
                {
                    _img_icon.sprite = (Sprite)assetWrap.Asset;
                }, null);
            }
            // quality
            {
                var qualityFrame = $"test_{itemCfg.Quality}";
                G.Asset.Load(AssetPath.ItemIcon(qualityFrame), typeof(Sprite), assetWrap =>
                {
                    _img_icon_quality.sprite = (Sprite)assetWrap.Asset;
                }, null);

                _txt_quality.text = itemCfg.Quality switch
                {
                    ItemQuality.White  => "COMMON",
                    ItemQuality.Blue   => "RARE",
                    ItemQuality.Purple => "EPIC",
                    ItemQuality.Red    => "LEGEND",
                    _                  => "Unknow"
                };
            }
            // name
            {
                _txt_name.text = itemCfg.Name;
            }
            // desc
            {
                _txt_desc.text = itemCfg.Desc;
            }
        }

        private void ShowEmptyInfo()
        {
            _node_info.gameObject.SetActive(false);
            _node_empty_info.gameObject.SetActive(true);
        }

        private void HideEmptyInfo()
        {
            _node_info.gameObject.SetActive(true);
            _node_empty_info.gameObject.SetActive(false);
        }
    }

}