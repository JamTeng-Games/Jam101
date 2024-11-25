using System;
using Jam.Core;
using Jam.Runtime.Event;
using Jam.Runtime.GameFsm;
using UnityEngine;

namespace Jam.Runtime.UI_
{

    public partial class StartupPanel
    {
        private float _targetProgress;
        private float _progressSpeed = 3f;

        private void OnEnable()
        {
            G.Event.Subscribe<float>(GlobalEventId.ResPipelineProgressUpdate, OnProgressUpdate);
            G.Event.Subscribe(GlobalEventId.StartInitializePackage, OnStartInitPackage);
            G.Event.Subscribe(GlobalEventId.StartUpdatePackageVersion, OnStartUpdatePackageVersion);
            G.Event.Subscribe(GlobalEventId.StartUpdatePackageManifest, OnStartUpdatePackageManifest);
            G.Event.Subscribe<string>(GlobalEventId.PrepareDownloadPackageFiles, OnPrepareDownloadPackageFiles);
            G.Event.Subscribe<DownloadProgress>(GlobalEventId.DownloadPackageFileProgress, UpdateDownloadProgress);
            G.Event.Subscribe(GlobalEventId.UpdateResDone, OnUpdateResDone);

            _btn_confirm_ok.onClick.AddListener(OnClickConfirm);
        }

        private void OnDisable()
        {
        }

        private void OnProgressUpdate(float progress)
        {
            _targetProgress = progress;
        }

        private void OnStartInitPackage()
        {
            _txt_progress_info.text = "Initialize Resource package...";
        }

        private void OnStartUpdatePackageVersion()
        {
            _txt_progress_info.text = "Update package version...";
        }

        private void OnStartUpdatePackageManifest()
        {
            _txt_progress_info.text = "Update package list...";
        }

        private void OnPrepareDownloadPackageFiles(string totalSizeMB)
        {
            _node_confirm.gameObject.SetActive(true);
            _txt_confirm_title.text = "Download Package Files";
            _txt_confirm_context.text = $"Size: {totalSizeMB}MB";
        }

        private void UpdateDownloadProgress(DownloadProgress obj)
        {
            _txt_progress_info.text =
                $"Downloading...{obj.CurrentCount}/{obj.TotalCount}, {obj.CurrentBytes}/{obj.TotalBytes}";
            _targetProgress = obj.CurrentBytes / (float)obj.TotalBytes;
        }

        private void OnUpdateResDone()
        {
            _targetProgress = 1;
            _progressSpeed = 10f;
            _txt_progress_info.text = "Resource update done!";
        }

        private void OnClickConfirm()
        {
            G.Event.Send(GlobalEventId.AgreeDownloadPackageFiles);
        }

        private void Update()
        {
            if (_targetProgress < _slider_download_progress.value)
            {
                _slider_download_progress.value = _targetProgress;
            }
            else
            {
                _slider_download_progress.value =
                    Mathf.Lerp(_slider_download_progress.value, _targetProgress, Time.deltaTime * _progressSpeed);
            }
        }

        private void CloseSelf()
        {
            Destroy(gameObject);
        }
    }

}