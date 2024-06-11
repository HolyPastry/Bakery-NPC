using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bakery.NPC
{
    public class NPCTest : MonoBehaviour
    {
        [SerializeField] private NpcController npcController1;
        [SerializeField] private NpcController npcController2;
        [SerializeField] private NpcController npcController3;
        [SerializeField] private NpcController npcController4;

        [SerializeField] private Transform WalkToPosition;

        [SerializeField] private Transform WanderCenter;

        [SerializeField] private Transform RunToPosition;

        // Start is called before the first frame update
        void Start()
        {
            npcController2.GoTo(WalkToPosition.position, isRunning: false);
            npcController1.GoTo(RunToPosition.position, isRunning: true);
            npcController3.Wander(WanderCenter.position, 10f);
            npcController4.Follow(npcController3);

        }


    }
}
