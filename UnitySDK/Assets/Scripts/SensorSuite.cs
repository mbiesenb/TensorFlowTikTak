using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorSuite : MonoBehaviour {

    [SerializeField]
    private Transform Left, Right, LeftCentral, RightCentral, Ground, Flier;
    [SerializeField]
    private float MaxDistLeft, MaxDistRight, MaxDistLeftCentral, MaxDistRightCentral, MaxDistGround, LateralScale;
    public float DistLeft,DistRight,DistLeftCentral,DistRightCentral,DistGround, FlierLateralPosition;
    private bool LeftTriggered, RightTriggered, LeftCentralTriggered, RightCentralTriggered, GroundTriggered;


	// Update is called once per frame
	void FixedUpdate () {
        DistLeft = 0;
        DistRight = 0;
        DistLeftCentral = 0;
        DistRightCentral = 0;
        DistGround = 0;
        RaycastHit hit;
        LeftTriggered = Physics.Raycast(Left.position, Vector3.back, out hit, MaxDistLeft);
        if(LeftTriggered)
        {
            DistLeft = Vector3.Distance(Left.position, hit.point);
            DistLeft = 1f - DistLeft / MaxDistLeft;
        }
        RightTriggered = Physics.Raycast(Right.position, Vector3.back, out hit, MaxDistRight);
        if(RightTriggered)
        {
            DistRight = Vector3.Distance(Right.position, hit.point);
            DistRight = 1f - DistRight / MaxDistRight;
        }
        LeftCentralTriggered = Physics.Raycast(LeftCentral.position, Vector3.back, out hit, MaxDistLeftCentral);
        if(LeftCentralTriggered)
        {
            DistLeftCentral = Vector3.Distance(LeftCentral.position, hit.point);
            DistLeftCentral = 1f - DistLeftCentral / MaxDistLeftCentral;
        }
        RightCentralTriggered = Physics.Raycast(RightCentral.position, Vector3.back, out hit, MaxDistRightCentral);
        if(RightCentralTriggered)
        {
            DistRightCentral = Vector3.Distance(RightCentral.position, hit.point);
            DistRightCentral = 1f - DistRightCentral / MaxDistRightCentral;
        }
        GroundTriggered = Physics.Raycast(Ground.position, Vector3.back, out hit, MaxDistGround);
        if(GroundTriggered)
        {
            DistGround = Vector3.Distance(Ground.position, hit.point);
            DistGround = 1f - DistGround / MaxDistGround;
        }

        FlierLateralPosition = (transform.position.x - Flier.transform.position.x+LateralScale)/200*LateralScale;

    }
}
