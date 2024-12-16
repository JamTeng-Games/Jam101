using cfg;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Jam.Runtime.UI_
{

    [UIPanel(UIPanelId.Matching)]
    public partial class MatchingPanel : UIPanel
    {
        public override UIPanelId Id => UIPanelId.Matching;



        private void OnValidate()
        {

        }
    }

}