using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Bakery.NPC
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal class Npc
    {
        private readonly NpcData _data;

        public string Name => _data.Name;

        public Npc(NpcData data)
        {
            _data = data;
        }

        public Npc()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is Npc npc &&
                   EqualityComparer<NpcData>.Default.Equals(_data, npc._data);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
