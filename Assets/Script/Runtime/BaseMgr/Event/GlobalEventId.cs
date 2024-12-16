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
    }

}