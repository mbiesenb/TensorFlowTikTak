using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScript : MonoBehaviour {

    private Rigidbody rigid;

    public float GroundDistance, StabilizingForce, SidewaysForce, JumpForce, MinimumClearance;

    private float steeringRightForce, steeringLeftForce, jumpingForce;
        
    public bool isJumping, justCrashed;
    
    public ControlMode controlMode;

    Vector3 initialPosition;
    Quaternion initialRotation;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        InstanceManager.Instance.RegisterInstance(this, transform.parent.GetComponentInChildren<SensorSuite>());
	}
	
	// Update is called once per frame
	void Update () {
        if (controlMode == ControlMode.automatic)
            return;
        if (Input.GetKey(KeyCode.A))
            SteerLeft(1,true);
        if (Input.GetKey(KeyCode.D))
            SteerRight(1,true);
        if (Input.GetKeyDown(KeyCode.Space))
            Jump(1,true);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float groundDistance = Vector3.Distance(transform.position, hit.point);
            float repulsionFactor = GroundDistance- groundDistance;
            if (repulsionFactor > 0)
                isJumping = false;
            rigid.AddRelativeForce(Vector3.up * StabilizingForce*repulsionFactor);
        }
            rigid.AddRelativeForce(Vector3.right * SidewaysForce*steeringRightForce);
            rigid.AddRelativeForce(Vector3.left * SidewaysForce*steeringLeftForce);
        if (!isJumping && jumpingForce-.1f>0f)
        {
            isJumping = true;
            Vector3 nVelocity = rigid.velocity;
            nVelocity.y = JumpForce*jumpingForce;
            rigid.velocity = nVelocity;
        }
        StopSteering();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (justCrashed)
            return;
        InstanceManager.Instance.NotifyOfCrash(this);
        StartCoroutine(Reset());
    }

    public void SteerRight(float force, bool manualOverride)
    {
        if (controlMode == ControlMode.automatic && manualOverride)
            return;
        steeringRightForce = Mathf.Clamp01(force);
    }

    public void SteerLeft(float force, bool manualOverride)
    {
        if (controlMode == ControlMode.automatic && manualOverride)
            return;
        steeringLeftForce = Mathf.Clamp01(force);
    }

    public void Jump(float force, bool manualOverride)
    {
        if (controlMode == ControlMode.automatic && manualOverride)
            return;
        jumpingForce = Mathf.Clamp01(force);
    }
    public void StopSteering()
    {
        steeringRightForce=0;
        steeringLeftForce=0;
        jumpingForce=0;
    }

    public IEnumerator Reset()
    {
        justCrashed = true;
        yield return new WaitForSeconds(1);
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
        justCrashed = false;
    }
}

public enum ControlMode
{
    manual,
    automatic
};
