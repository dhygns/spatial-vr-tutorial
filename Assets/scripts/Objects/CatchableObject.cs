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
    private delegate void _Logic(float dt);
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
        if (_logic != null) _logic(Time.deltaTime);
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
        //__Rigidbody.useGravity = false;
        __Rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        //set Target Transform
        __TargetPosition = transform.localPosition;
        __TargetRotation = transform.localEulerAngles;

        //set Logic
        _logic = _logic_grabbed;
    }

    public void Release(Vector3 velocity)
    {
        //if it is already released, continue;
        if (transform.parent == __ReleasedParent) return;
        //set Parent
        transform.parent = __ReleasedParent;

        //setup Rigidbody
        //__Rigidbody.useGravity = true;
        __Rigidbody.constraints = RigidbodyConstraints.None;
        __Rigidbody.velocity = velocity;
        __Rigidbody.AddTorque(Random.rotation.eulerAngles * 0.1f);

        //set Logic
        _logic = _logic_released;
    }

    public bool IsParent(Transform target) { return transform.parent == target; }
    public bool IsGrabbed() { return transform.parent != __ReleasedParent; }
}
