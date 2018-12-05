using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TikTakAgent : Agent {

    //public Brain brain;
    public ControlScript controlScript;
    public SensorSuite sensorSuite;

    public override void InitializeAgent()
    {
        sensorSuite     = GameObject.FindObjectOfType <SensorSuite>() as SensorSuite;
        controlScript   = GameObject.FindObjectOfType<ControlScript>() as ControlScript;
        sensorSuite     = GameObject.FindObjectOfType<SensorSuite>() as SensorSuite;
    }

    public override void CollectObservations()
    {
        AddVectorObs(sensorSuite.DistLeft);
        AddVectorObs(sensorSuite.DistRight);
        AddVectorObs(sensorSuite.DistLeftCentral);
        AddVectorObs(sensorSuite.DistRightCentral);
        AddVectorObs(sensorSuite.DistGround);
        AddVectorObs(sensorSuite.FlierLateralPosition);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        int index = -1;
        double tempMin = 0.0f;
            
        for (int i = 0;  i < vectorAction.Length; i++)
        {
            if ( vectorAction[i] > tempMin && index != -1)
            {
                index = i;
            }
        }
            switch ( index )
        {
            case 0:
                controlScript.SteerLeft(1, false);
                    break;
            case 1:
                controlScript.Jump(1,false);
                break;
            case 2:
                controlScript.SteerRight(1, false);
                break;
            case 3:
                break;
               
        }
        SetReward(0.1f);

        if ( controlScript.justCrashed)
        {
            AddReward(-1f);
        }
        else
        {
            AddReward(0.1f);
        }
    }

    public override void AgentReset()
    {
        Done();
        controlScript.Reset();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
