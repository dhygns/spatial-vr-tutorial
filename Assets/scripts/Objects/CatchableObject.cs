using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchableObject : MonoBehaviour
{
    public float MovingSpeed = 1.0f;
    public Vector3 TargetPosition;
    public Vector3 TargetRotation;

    //Target To moving
    private Vector3 __TargetPosition;
    private Vector3 __TargetRotation;

    //Released Parent Transform
    private Transform __ReleasedParent;

    //RigidBody
    private Rigidbody __Rigidbody;


    //Behaviour Logic
    private delegate void _Logic (float dt);
    private _Logic _logic = null;

    private void Awake()
    {
        __TargetPosition = Vector3.zero;
        __ReleasedParent = GameObject.Find("_Env").transform;
        __Rigidbody = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start()    
    {

    }

    // Update is called once per frame
    void Update()
    {
        //update Logic
        if(_logic != null) _logic(Time.deltaTime);
    }
    
    // Logic Uncatched Idle
    private void _logic_released(float dt)
    {

    }

    //Logic Catched Idle
    private void _logic_grabbed(float dt)
    {
        //translate to Target
        __TargetPosition += (TargetPosition - __TargetPosition) * MovingSpeed * dt;
        __TargetRotation += (TargetRotation - __TargetRotation) * MovingSpeed * dt;

        //set transforms 
        transform.localPosition = __TargetPosition;
        transform.localEulerAngles = __TargetRotation;
    }


    public void Grab(GameObject Hand)
    {
        //set Parent
        transform.parent = Hand.transform;

        //setup Rigidbody
        __Rigidbody.useGravity = false;
        __Rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        //set Logic
        _logic = _logic_grabbed;
    }

    public void Release(Vector3 velocity)
    {
        //set Parent
        transform.parent = null;

        //setup Rigidbody
        __Rigidbody.useGravity = true;
        __Rigidbody.constraints = RigidbodyConstraints.None;
        __Rigidbody.velocity = velocity;

        //set Logic
        _logic = _logic_released;
    }
}
