

using System.Collections.Generic;
using UnityEngine;

namespace Bakery.NPC
{
    internal class NpcSpawner : MonoBehaviour
    {

        [SerializeField] protected List<NpcData> _npcDataList;
        [SerializeField] protected PlacementStrategy _placementStrategy;

        [SerializeField] private bool _spawnOnStart;

        protected readonly INpcFactory _factory = new NpcFactory();

        internal virtual void Start()
        {
            if (_spawnOnStart) Spawn();

        }

        internal virtual void Spawn()
        {
            for (int i = 0; i < _npcDataList.Count; i++)
            {
                NpcController npc = _factory.Create(_npcDataList[i]);
                npc.transform.position = _placementStrategy.Place(transform.position, i);
            }

        }



        internal virtual List<NpcController> Spawn(List<NpcData> npcList)
        {
            var controllerList = new List<NpcController>();
            for (int i = 0; i < npcList.Count; i++)
            {
                var npc = _factory.Create(npcList[i]);
                npc.transform.position = _placementStrategy.Place(transform.position, i);
                controllerList.Add(npc);
            }
            return controllerList;
        }
    }
}