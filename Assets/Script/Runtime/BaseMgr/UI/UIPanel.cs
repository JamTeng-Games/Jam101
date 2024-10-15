using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace J.Runtime.UI
{
    public abstract class UIPanel : MonoBehaviour
    {
        [HideInInspector] public UILevel level;
        [HideInInspector] public UIShowMode showMode;

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

        public virtual void DoShowAnimation(Action<UIPanel> callback = null)
        {
            callback?.Invoke(this);
        }

        public virtual void DoHidingAnimation(Action<UIPanel> callback = null)
        {
            callback?.Invoke(this);
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