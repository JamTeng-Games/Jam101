using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Jam.Core;
using Jam.Runtime;
using Jam.Runtime.Net_;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jam
{

    public class TestG : MonoBehaviour
    {
        [Button]
        private void Connect()
        {
            G.Net.Connect("192.168.31.128", 8001, succ =>
            {
                if (succ)
                {
                    Debug.Log("Connect succ");
                }
                else
                {
                    Debug.Log("Connect fail");
                }
            });

            G.Net.AddMessageHook(NetCmd.CS_Login, (id, packet) =>
            {
                LoginMsg m = packet.Decode<LoginMsg>();
                Debug.Log($"Recv Hook {packet.CmdId}, {m}");
            });
            
            G.Net.AddMessageHook(NetCmd.SC_AlreadyLogin, (id, packet) =>
            {
                LoginMsg m = packet.Decode<LoginMsg>();
                Debug.Log($"Recv Hook {id}, {m}");
            });
        }

        [Button]
        private void Login()
        {
            LoginMsg msg = new LoginMsg();
            msg.player_id = Utils.GetDeviceID();
            var packet = Packet.Create((int)NetCmd.CS_Login);
            packet.Encode(msg);
            G.Net.Send(packet);
        }

        [Button]
        private void Test2(string ip, int port)
        {
            G.Net.Connect(ip, port, succ =>
            {
                if (succ)
                {
                    Debug.Log("Connect succ");
                }
                else
                {
                    Debug.Log("Connect fail");
                }
            });
        }

        [Button]
        private void TestSendPacket()
        {
            TestBigMsg big = new TestBigMsg();
            big.big = 97;
            TestMsg msg = new TestMsg();
            msg.msg = "Test";
            msg.coin = 97;
            big.msg = msg;
            var packet = Packet.Create(7);
            packet.Encode(big);
            G.Net.Send(packet);
        }

        [Serializable]
        public class TestBigMsg
        {
            public int big;
            public TestMsg msg;
        }

        [Serializable]
        public class TestMsg
        {
            public int coin;
            public string msg;

            public override string ToString()
            {
                return $"coin: {coin}, msg: {msg}";
            }
        }

        [Serializable]
        public class LoginMsg
        {
            public int player_id;

            public override string ToString()
            {
                return $"player_id: {player_id}";
            }
        }
    }

}