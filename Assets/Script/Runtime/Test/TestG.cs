using System;
using System.Collections;
using System.Collections.Generic;
using Jam.Runtime;
using Jam.Runtime.Net_;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jam
{

    public class TestG : MonoBehaviour
    {
        [Button]
        private void Test1()
        {
            G.Net.Connect("192.168.31.128", 8888, succ =>
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
            TestMsg msg = new TestMsg();
            msg.msg = "Test";
            msg.coin = 97;
            var packet = Packet.Create(7);
            packet.Encode(msg);
            G.Net.Send(packet);
        }

        [Serializable]
        public class TestMsg
        {
            public int coin;
            public string msg;
        }
    }

}