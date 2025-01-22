namespace Jam.Runtime.Event
{

    public enum GlobalEventId
    {
        ResPipelineDone,

        LoadCfgSuccess,
        LoadCfgFailure,

        InputEnterMode,
        InputExitMode,

        PanelOpen,
        PanelClosed,

        LoginSuccess,
        EnterCombat,
        ExitCombat,

        // Init res
        ResPipelineProgressUpdate,
        StartInitializePackage,
        StartUpdatePackageVersion,
        StartUpdatePackageManifest,
        PrepareDownloadPackageFiles,
        AgreeDownloadPackageFiles,
        StartDownloadPackageFiles,
        DownloadPackageFileProgress,
        UpdateResDone,
        InitResourceFailed,
        PackageVersionUpdateFailed,
        PatchManifestUpdateFailed,

        //
        RoleDataUpdate,
        ItemAdd,
        ItemRemove,
        ItemUpdateAll,
        ItemAnyUpdate,
        MoneyAdd,
        MoneyCost,
        MoneyUpdateAll,
        ShopRefresh,
        ShopGoodsUpdate,
        ShopGoodsRemove,
        RoundUpdate,
        HeroChange,
        
        // room
        CreateRoomFail,
        JoinRoomSuccess,
        JoinRoomFail,
        LeaveRoomSuccess,
        LeaveRoomFail,
        RoomUserInfosUpdate,
        RoomUserEnter,
        RoomUserLeave,
        RoomUserReadyUpdate,
        RoomUserChat,
        RoomListUpdate,
        
        // combat
        CombatTimeUpdate,   // 倒计时
        KillHero,

        END,
    }

}