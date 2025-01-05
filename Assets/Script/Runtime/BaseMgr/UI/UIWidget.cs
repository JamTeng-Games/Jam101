using System.Collections.Generic;
using cfg;
using UnityEngine;

namespace Jam.Runtime.UI_
{

    public abstract class UIWidget : MonoBehaviour, IWidgetOwner
    {
        private int _id;
        protected IWidgetOwner _owner;
        protected Dictionary<int, UIWidget> _widgets = new Dictionary<int, UIWidget>(4);

        public int Id => _id;
        public IWidgetOwner Owner => _owner;

        public abstract UIWidgetId TypeId { get; }
        public Dictionary<int, UIWidget> Widgets => _widgets;
        public bool IsVisible => gameObject.activeSelf;


        public void SetId(int id)
        {
            _id = id;
        }

        public void SetOwner(IWidgetOwner owner)
        {
            _owner = owner;
        }

        public void Tick(float dt)
        {
            OnTick(dt);
        }

        public void LateTick()
        {
            OnLateTick();
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
    }

}