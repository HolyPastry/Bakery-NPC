using System.Collections.Generic;
using UnityEngine;

namespace Bakery.NPC
{


    [CreateAssetMenu(fileName = "NpcData", menuName = "Holypastry/NpcData", order = 0)]
    internal class NpcData : ScriptableObject
    {
        public string Name;

        public float WalkSpeed = 1;
        public float RunSpeed = 2;

        public GameObject Prefab;

    }
}
