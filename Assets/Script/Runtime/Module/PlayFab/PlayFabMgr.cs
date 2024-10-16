using J.Core;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Device;

namespace J.Runtime.Module
{
    public class PlayFabMgr
    {
        public static void Login()
        {
            var request = new LoginWithCustomIDRequest()
            {
                CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true
            };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
        }

        private static void OnLoginSuccess(LoginResult result)
        {
            JLog.Debug("Login Success!");
        }

        private static void OnError(PlayFabError error)
        {
            JLog.Error($"PlayFab response error {error.GenerateErrorReport()}");
        }
    }
}