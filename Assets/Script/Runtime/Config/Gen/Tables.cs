
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Jam.Runtime;
using Jam.Runtime.Asset;
using Jam.Runtime.Constant;
using SimpleJSON;
using UnityEngine;

namespace Jam.Cfg
{
public partial class Tables
{
    private System.Action<bool> _onLoadedDone;
    private int _totalCount = 0;
    private int _loadCount = 0;
    private bool _anyCfgLoadFailed = false;

    public TbItem TbItem { get; private set; }
    public TbReward TbReward { get; private set; }
    public TbUIPanelConfig TbUIPanelConfig { get; private set; }
    public TbUIWidgetConfig TbUIWidgetConfig { get; private set; }
    public TbHero TbHero { get; private set; }
    public TbSkill TbSkill { get; private set; }
    public TbShop TbShop { get; private set; }
    public TbDice TbDice { get; private set; }
    public TbColor TbColor { get; private set; }

    private void LoadCfgImpl(System.Action<bool> onLoadedDone)
    {
        _onLoadedDone = onLoadedDone;
        _totalCount++;
        G.Asset.Load(GetAssetPath("tbitem"), typeof(TextAsset), LoadAssetCallback, "tbitem");
        _totalCount++;
        G.Asset.Load(GetAssetPath("tbreward"), typeof(TextAsset), LoadAssetCallback, "tbreward");
        _totalCount++;
        G.Asset.Load(GetAssetPath("tbuipanelconfig"), typeof(TextAsset), LoadAssetCallback, "tbuipanelconfig");
        _totalCount++;
        G.Asset.Load(GetAssetPath("tbuiwidgetconfig"), typeof(TextAsset), LoadAssetCallback, "tbuiwidgetconfig");
        _totalCount++;
        G.Asset.Load(GetAssetPath("tbhero"), typeof(TextAsset), LoadAssetCallback, "tbhero");
        _totalCount++;
        G.Asset.Load(GetAssetPath("tbskill"), typeof(TextAsset), LoadAssetCallback, "tbskill");
        _totalCount++;
        G.Asset.Load(GetAssetPath("tbshop"), typeof(TextAsset), LoadAssetCallback, "tbshop");
        _totalCount++;
        G.Asset.Load(GetAssetPath("tbdice"), typeof(TextAsset), LoadAssetCallback, "tbdice");
        _totalCount++;
        G.Asset.Load(GetAssetPath("tbcolor"), typeof(TextAsset), LoadAssetCallback, "tbcolor");
    }

    private void ResolveRef()
    {
        TbItem.ResolveRef(this);
        TbReward.ResolveRef(this);
        TbUIPanelConfig.ResolveRef(this);
        TbUIWidgetConfig.ResolveRef(this);
        TbHero.ResolveRef(this);
        TbSkill.ResolveRef(this);
        TbShop.ResolveRef(this);
        TbDice.ResolveRef(this);
        TbColor.ResolveRef(this);
    }

    private void LoadAssetCallback(AssetHandleWrap wrap)
    {
        if (_anyCfgLoadFailed)
            return;

        if (!wrap.IsSuccess)
        {
            _anyCfgLoadFailed = true;
            _onLoadedDone?.Invoke(false);
            return;
        }

        _loadCount++;
        string jsonText = ((TextAsset)wrap.Asset).text;
        JSONNode json = JSONNode.Parse(jsonText);

        if ((string)wrap.UserData == "tbitem")
        {
            TbItem = new TbItem(json);
        }
        if ((string)wrap.UserData == "tbreward")
        {
            TbReward = new TbReward(json);
        }
        if ((string)wrap.UserData == "tbuipanelconfig")
        {
            TbUIPanelConfig = new TbUIPanelConfig(json);
        }
        if ((string)wrap.UserData == "tbuiwidgetconfig")
        {
            TbUIWidgetConfig = new TbUIWidgetConfig(json);
        }
        if ((string)wrap.UserData == "tbhero")
        {
            TbHero = new TbHero(json);
        }
        if ((string)wrap.UserData == "tbskill")
        {
            TbSkill = new TbSkill(json);
        }
        if ((string)wrap.UserData == "tbshop")
        {
            TbShop = new TbShop(json);
        }
        if ((string)wrap.UserData == "tbdice")
        {
            TbDice = new TbDice(json);
        }
        if ((string)wrap.UserData == "tbcolor")
        {
            TbColor = new TbColor(json);
        }

        if (_loadCount == _totalCount)
        {
            ResolveRef();
            _onLoadedDone?.Invoke(true);
        }
    }

    private string GetAssetPath(string assetName)
    {
        return AssetPath.Cfg(assetName, false);
    }
}

}
