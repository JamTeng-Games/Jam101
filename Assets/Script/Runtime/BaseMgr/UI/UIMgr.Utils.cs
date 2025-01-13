using Jam.Cfg;

namespace Jam.Runtime.UI_
{

    public partial class UIMgr
    {
        public void BackTo(UIPanelId panelId)
        {
            // 如果没有开启HomePanel，直接返回
            if (!Has(panelId))
                return;

            // 关闭所有正在加载的Panel
            foreach (var (id, _) in _loadingPanels)
            {
                Close(id);
            }

            // 关闭所有在 panelId 之后打开的Panel
            for (int i = _panelOpStack.Count - 1; i >= 0; i--)
            {
                var panel = _panels[i];
                if (panel.Id == panelId)
                    break;
                if (panel.IsVisible)
                    panel.OnHide();
                panel.OnClose();
                panel.gameObject.SetActive(false);
                _panels.RemoveAt(i);
                _panelOpStack.Remove(panel);
                _recycleQueue.Enqueue(panel);
            }
        }

        public void BackToHome()
        {
            BackTo(UIPanelId.Home);
        }
    }

}