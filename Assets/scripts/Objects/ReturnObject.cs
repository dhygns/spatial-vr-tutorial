using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnObject : MonoBehaviour {

    //Maximum Distance from me
    public float AvailableDistance = 1.0f;

    //Target To Return
    private Transform __TargetTransform = null;

    //public bool AutoReturn = false;


    //This RigidBody
    private Rigidbody __Rigidbody = null;
    private void Awake()
    {
        __TargetTransform = GameObject.Find("OVRCameraRig").transform;
        __Rigidbody = GetComponent<Rigidbody>();
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //if there is no target, you don't need to go back to target
        if (__TargetTransform == null) return;

        //else, you have to go back to target inner AvailableDistance

        Vector3 distanceFromTarget = (__TargetTransform.position - transform.position);

        if(distanceFromTarget.magnitude > AvailableDistance)
        {
            __Rigidbody.AddForce(distanceFromTarget);
        }
    }

    public void setTargetToReturn(Transform target)
    {
        __TargetTransform = target;
    }
}
