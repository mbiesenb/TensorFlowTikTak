using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceManager : MonoBehaviour {

    /// <summary>
    /// Contains a list of all controller and sensor suite pairs
    /// </summary>
    private Dictionary<ControlScript, SensorSuite> Instances;

	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// This gets called when a Flier crashes.
    /// </summary>
    /// <param name="controller">The controller of the flier that just crashed</param>
    public void NotifyOfCrash(ControlScript controller)
    {
        SensorSuite crashedSuite;
        if (Instances.TryGetValue(controller, out crashedSuite))
            Debug.Log("Crash by: " + controller.name + " using Sensor Suite:" + crashedSuite.name);
        //What shall we do with a crash-ed flier, what shall we do with a crash-ed flier...
    }

    #region maintenance stuff

    public static InstanceManager self;

    void Awake()
    {
        if (self == null)
        {
            self = this;
            Instances = new Dictionary<ControlScript, SensorSuite>();
        }
        if (self != this)
            Destroy(this);
    }

    public void RegisterInstance(ControlScript control, SensorSuite sensors)
    {
        Instances.Add(control, sensors);
    }

    public static InstanceManager Instance
    {
        get
        {
            if (self == null)
            {
                GameObject blargh = new GameObject("InstanceManager");
                self = blargh.AddComponent<InstanceManager>();
            }
            return self;
        }
    }
    #endregion maintenance stuff
}
