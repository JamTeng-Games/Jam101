using System;
using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace J.Runtime.UI
{
    public abstract class UIPanel : MonoBehaviour
    {
        [HideInInspector] public UILevel level;
        [HideInInspector] public UIShowMode showMode;
        public MMF_Player showFeedback;
        public MMF_Player hideFeedback;

        public bool IsVisible => gameObject.activeSelf;

        public void Tick(float dt)
        {
            OnTick(dt);
        }

        public void LateTick()
        {
            OnLateTick();
        }

        public abstract void OnDrop();
        public abstract void OnLoad();
        public abstract void OnShow();
        public abstract void OnHide();
        public abstract void OnClose();

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