using UnityEngine;

namespace Bakery.NPC
{
    [CreateAssetMenu(fileName = "DefaultPlacementStrategy", menuName = "Holypastry/DefaultPlacementStrategy", order = 0)]
    public class PlacementStrategy : ScriptableObject
    {
        public virtual Vector3 Place(Vector3 origin) => origin;

        public virtual Vector3 Place(Vector3 origin, int index) => origin - new Vector3(index, 0, 0);

    }
}