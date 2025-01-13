using SimpleJSON;
using UnityEngine;

namespace Quantum.Cfg_
{

    public static class Cfg
    {
        private static Tables _tables;
        public static Tables Tb => _tables;

        public static void Init()
        {
            _tables = new Tables(file =>
            {
                var text = Resources.Load<TextAsset>($"QuantumConfig/Gen/{file}");
                return JSON.Parse(text.text);
            });
        }
    }

}