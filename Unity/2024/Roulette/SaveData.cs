using System;
using System.Collections.Generic;

namespace Roulette
{
    [Serializable]
    public class SaveData
    {
        public string saveDataName;

        public string passcode;

        public List<Member> members;
    }
}