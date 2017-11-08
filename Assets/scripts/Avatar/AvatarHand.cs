using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarHand : MonoBehaviour {

    //velocity
    private Queue<Vector3> __QPosition;
    private Vector3 __PrevPosition = Vector3.zero;
    private Vector3 __CurrPosition = Vector3.zero;
    private Vector3 __AvgVelocity;

    //RigidBody
    private Rigidbody __Rigidbody;

    //candidates list for grabing object
    private List<CatchableObject> __Candidates;

    //grabbed object
    private CatchableObject __Grabbed = null;

    private void Awake()
    {
        __QPosition = new Queue<Vector3>();
        __Candidates = new List<CatchableObject>();
        __Rigidbody = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
        __PrevPosition = transform.position;
        __CurrPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //Setup Position Prev & Curr
        __PrevPosition = __CurrPosition;
        __CurrPosition = transform.position;

        //Enqueue to Q for calculation of a average of velocity.
        __QPosition.Enqueue((__CurrPosition - __PrevPosition) / Time.deltaTime);
        if (__QPosition.Count > 5) __QPosition.Dequeue();

        //calculating average
        __AvgVelocity = Vector3.zero;
        foreach (Vector3 pos in __QPosition.ToArray())
        {
            __AvgVelocity += pos / 5;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //is it possible to be candidate? 
        CatchableObject candidate = other.gameObject.GetComponent<CatchableObject>();
        if (__Candidates == null) return;

        //added to candidates list
        __Candidates.Add(candidate);
    }

    private void OnTriggerExit(Collider other)
    {
        //is it possible to be candidate? 
        CatchableObject candidate = other.gameObject.GetComponent<CatchableObject>();
        if (candidate == null) return;

        //remove from candidates list
        __Candidates.Remove(candidate);
    }

    public void Grab()
    {
        // if there is no candidate, dosen't work anything
        if (__Candidates.Count == 0) return;

        // find closest object
        CatchableObject tmpcobj = null;
        float tmpdist = Mathf.Infinity;

        __Candidates.ForEach((CatchableObject co) =>
        {
            if (tmpcobj == null)
            {
                tmpcobj = co;
            }
            else
            {
                float dist = (transform.position - co.transform.position).sqrMagnitude;
                if(tmpdist > dist)
                {
                    tmpdist = dist;
                    tmpcobj = co;
                }
            }
        });

        if(tmpcobj != null)
        {
            tmpcobj.Grab(gameObject);
            __Grabbed = tmpcobj;
        }
    }

    public void Release()
    {
        if (__Grabbed == null) return;
        __Grabbed.Release(__AvgVelocity);
        __Grabbed = null;
    }
}
