using System;
using UnityEngine.Device;
using System.Security.Cryptography;

namespace Jam.Core
{

    public static partial class Utils
    {
        public static int GetDeviceID()
        {
            string deviceID = SystemInfo.deviceUniqueIdentifier;
            using var md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(deviceID));
            // 假设我们只使用哈希的前4个字节（32位整数）
            int id = BitConverter.ToInt32(hash, 0);
            return id;
        }
    }

}