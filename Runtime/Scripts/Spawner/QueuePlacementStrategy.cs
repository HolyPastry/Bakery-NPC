using UnityEngine;

namespace Bakery.NPC
{
    [CreateAssetMenu(fileName = "QueuePlacementStrategy", menuName = "Holypastry/QueuePlacementStrategy", order = 0)]
    public class QueuePlacementStrategy : PlacementStrategy
    {
        public override Vector3 Place(Vector3 origin, int index)
        {
            origin.z -= index;//* 0.75f;
            return origin;
        }
    }
}