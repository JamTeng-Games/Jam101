using System;
using Jam.Core;
using Jam.Runtime.Constant;
using Jam.Runtime.Data_;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Jam.Runtime.UI_
{

    public partial class ItemCellWidget
    {
        private BagItem _data;
        private Action<ItemCellWidget> _clickAction;

        public BagItem Data => _data;

        public override void OnInit()
        {
        }

        public override void OnOpen(object userData)
        {
            _img_icon.AddEventListener(EventTriggerType.PointerDown, OnClickSelf);

            Refresh(userData as BagItem);
        }

        public override void OnClose()
        {
            _clickAction = null;
            _img_icon.RemoveAllEventListeners();
        }

        protected override void OnTick(float dt)
        {
        }

        public void Refresh(BagItem newData)
        {
            _data = newData;
            int itemCfgId = newData.id;
            Cfg.Item itemCfg = G.Cfg.TbItem[itemCfgId];
            // quality
            {
                var qualityFrame = $"test_{itemCfg.Quality}";
                G.Asset.Load(AssetPath.ItemIcon(qualityFrame), typeof(Sprite), assetWrap =>
                {
                    _img_quality.sprite = (Sprite)assetWrap.Asset;
                }, null);
            }
            // icon
            {
                G.Asset.Load(AssetPath.ItemIcon(itemCfg.Icon), typeof(Sprite), assetWrap =>
                {
                    _img_icon.sprite = (Sprite)assetWrap.Asset;
                }, null);
            }
            // count
            {
                _txt_Count.gameObject.SetActive(newData.amount > 1);
                _txt_Count.text = newData.amount.ToString();
            }
        }

        public void SetClickEvent(Action<ItemCellWidget> action)
        {
            _clickAction = action;
        }

        public void ClearClickEvent()
        {
            _clickAction = null;
        }

        public void Select()
        {
            _node_select.gameObject.SetActive(true);
        }

        public void Unselect()
        {
            _node_select.gameObject.SetActive(false);
        }

        public void DisableSelect()
        {
            Unselect();
            _img_icon.DisableEventListeners();
        }

        private void OnClickSelf(BaseEventData evt)
        {
            _clickAction?.Invoke(this);
        }
    }

}