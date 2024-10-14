using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace J.Runtime.UI
{
    public abstract class UIPanel : MonoBehaviour
    {
        public UILevel level;
        public UIShowMode showMode;

        protected bool isActive;

        public bool IsActive => gameObject.activeSelf;

        // public void Show()
        // {
        //     if (isActive)
        //         return;
        //     // TODO: Animation
        //     OnShow();
        // }
        //
        // public void Hide()
        // {
        //     if (!isActive)
        //         return;
        //     // TODO: Animation
        //     OnHide();
        // }
        //
        // public void Close()
        // {
        //     OnClose();
        // }

        public void Tick(float dt)
        {
            OnTick(dt);
        }

        public void LateTick()
        {
            OnLateTick();
        }

        public abstract void OnDrop();
        public abstract void OnShow();
        public abstract void OnHide();
        public abstract void OnClose();

        public virtual void DoShowAnimation(Action<UIPanel> callback)
        {
            callback?.Invoke(this);
        }

        public virtual void DoHidingAnimation(Action<UIPanel> callback)
        {
            callback?.Invoke(this);
        }

        protected virtual void OnTick(float dt)
        {
        }

        protected virtual void OnLateTick()
        {
        }
    }
}