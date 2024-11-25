using System;
using cfg;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Jam.Runtime.UI_
{

    public abstract class UIPanel : MonoBehaviour
    {
        [HideInInspector] public UILevel level;
        [HideInInspector] public UIShowMode showMode;
        public MMF_Player showFeedback;
        public MMF_Player hideFeedback;
        public abstract UIPanelId Id { get; }

        public bool IsVisible => gameObject.activeSelf;

        public void Tick(float dt)
        {
            OnTick(dt);
        }

        public void LateTick()
        {
            OnLateTick();
        }

        public void PlayShowingAnim(Action<UIPanel> callback = null)
        {
            if (showFeedback == null)
            {
                callback?.Invoke(this);
            }
            else
            {
                showFeedback.PlayFeedbacks();
                showFeedback.Events.OnComplete.AddListener(() =>
                {
                    callback?.Invoke(this);
                });
            }
        }

        public void PlayHidingAnim(Action<UIPanel> callback = null)
        {
            if (hideFeedback == null)
            {
                callback?.Invoke(this);
            }
            else
            {
                hideFeedback.PlayFeedbacks();
                hideFeedback.Events.OnComplete.AddListener(() =>
                {
                    callback?.Invoke(this);
                });
            }
        }

        public abstract void OnInit();

        public abstract void OnOpen(object userData);
        public abstract void OnClose();

        /// 被对象池回收时
        public virtual void OnRecycle()
        {
        }

        public virtual void OnShow()
        {
        }

        public virtual void OnHide()
        {
        }

        protected virtual void OnTick(float dt)
        {
        }

        protected virtual void OnLateTick()
        {
        }

        protected virtual void OnShowAnimationStart()
        {
        }

        protected virtual void OnHidingAnimationStart()
        {
        }

        protected virtual void OnShowAnimationEnd()
        {
        }

        protected virtual void OnHidingAnimationEnd()
        {
        }
    }

}