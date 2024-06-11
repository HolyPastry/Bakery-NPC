using UnityEngine;

namespace Bakery.NPC
{

    internal interface INpcFactory
    {
        NpcController Create(NpcData data);
    }

    internal class NpcFactory : INpcFactory
    {
        public virtual NpcController Create(NpcData data)
        {
            var obj = GameObject.Instantiate(data.Prefab);
            var npc = obj.GetComponent<NpcController>();
            npc.Init(Vector3.zero, data);
            return npc;
        }
    }
}