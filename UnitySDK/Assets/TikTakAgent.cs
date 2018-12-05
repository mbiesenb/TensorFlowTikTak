using System;
using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TikTakAgent : Agent {

    //public Brain brain;

   public GameObject sensorSuite;

   private SensorSuite sensor;
   private ControlScript controlScript;

   public override void InitializeAgent()
   {
      sensor = sensorSuite.GetComponent<SensorSuite>();
      controlScript   = this.GetComponent<ControlScript>();
   }

    public override void CollectObservations()
    {
      AddVectorObs(sensor.DistGround);
      AddVectorObs(sensor.DistLeft);
      AddVectorObs(sensor.DistLeftCentral);
      AddVectorObs(sensor.DistRight);
      AddVectorObs(sensor.DistRightCentral);
      AddVectorObs(sensor.FlierLateralPosition);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        string log = string.Join(",", Array.ConvertAll<float, String>(vectorAction, Convert.ToString));
        Debug.Log(log);
        switch ((int)vectorAction[0])
        {
            case 1:
                controlScript.SteerLeft(1, false);
                break;
            case 2:
                controlScript.Jump(1,false);
                break;
            case 3:
                controlScript.SteerRight(1, false);
                break;
        }

       if (sensor.DistGround >= 0.9 ||
           sensor.DistLeft >= 0.9 ||
           sensor.DistLeftCentral >= 0.9 ||
           sensor.DistRight >= 0.9 ||
           sensor.DistRightCentral >= 0.9 ||
           (sensor.FlierLateralPosition <= 0.1 || sensor.FlierLateralPosition >= 0.9))
       {
          Done();
          SetReward(-1f);
       }
       else
       {
          SetReward(0.1f);
       }
    }

    public override void AgentReset()
    {
        //controlScript.Reset();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
