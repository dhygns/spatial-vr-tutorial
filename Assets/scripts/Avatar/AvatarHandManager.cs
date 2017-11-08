using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarHandManager : MonoBehaviour {

    public AvatarHand RightHand = null;
    public AvatarHand LeftHand = null;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Check Existion
        if(RightHand == null || LeftHand == null)
        {
            Debug.LogWarning("There is no hand.");
            return;
        }

        if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.8f)
        {
            RightHand.Grab();
        }

        if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) < 0.2f)
        {
            RightHand.Release();
        }

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.8f)
        {
            LeftHand.Grab();
        }

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) < 0.2f)
        {
            LeftHand.Release();
        }
    }
}
