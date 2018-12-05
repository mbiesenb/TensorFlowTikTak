using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMover : MonoBehaviour {

    Rigidbody rigid;

    public Vector3 ForceVector;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
        rigid.AddRelativeForce(ForceVector, ForceMode.VelocityChange);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
