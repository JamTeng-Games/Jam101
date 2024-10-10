using UnityEngine;

namespace Temp.NewUI
{

    public abstract class NewUIPanel : MonoBehaviour
    {
        public UILevel level;
        public UIShowMode showMode;

        protected bool _isShowing;
        protected bool _isCovered;

        public bool IsShowing => _isShowing;
        public bool IsCovered => _isCovered;

        
        public void Drop()
        {
            OnDrop();
        }

        public void Show()
        {
            if (_isShowing)
                return;
            _isShowing = true;
            OnShow();
        }

        public void Hide()
        {
            if (!_isShowing)
                return;
            _isShowing = false;
            OnHide();
        }

        public void Close()
        {
            OnClose();
        }

        public void Tick(float dt)
        {
            OnTick(dt);
        }

        public void CoverBy<T>(T panel) where T : NewUIPanel
        {
            if (_isCovered)
                return;
            _isCovered = true;
            OnCovered(panel);
        }

        public void Reveal()
        {
            if (!_isCovered)
                return;
            _isCovered = false;
            OnRevealed();
        }

        public void LateTick()
        {
        }

        protected abstract void OnDrop();
        protected abstract void OnShow();
        protected abstract void OnHide();
        protected abstract void OnClose();

        protected virtual void OnTick(float dt)
        {
        }

        protected virtual void OnCovered<T>(T panel) where T : NewUIPanel
        {
        }

        protected virtual void OnRevealed()
        {
        }
    }

}