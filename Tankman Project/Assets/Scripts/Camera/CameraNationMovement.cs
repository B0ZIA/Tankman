using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraNationMovement : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private List<NationPoint> nationPoints;

    private NationManager.Nation currentNation;


    private void Awake()
    {
        currentNation = NationManager.myNation;
    }

    private void Update()
    {
        if (currentNation != NationManager.myNation)
        {
            currentNation = NationManager.myNation;
            ChangeNationCamera(currentNation);
        }
    }

    private void ChangeNationCamera(NationManager.Nation nation)
    {
        camera.GetComponent<CameraFollow>().target = nationPoints.First(p => p.nation == nation).point;
    }

    [Serializable]
    public struct NationPoint
    {
        public NationManager.Nation nation;
        public Transform point;

        public NationPoint(NationManager.Nation nation, Transform point)
        {
            this.nation = nation;
            this.point = point;
        }
    }

}
