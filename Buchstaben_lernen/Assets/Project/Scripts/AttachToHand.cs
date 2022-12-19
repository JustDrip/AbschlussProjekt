using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class AttachToHand : MonoBehaviour
{
    private Interactable inter;

    // Start is called before the first frame update
    void Start()
    {
        inter = GetComponent<Interactable>();
    }

    void OnHandHoverBegin(Hand hand)
    {
        hand.ShowGrabHint();
    }

    void OnHandHoverEnd(Hand hand)
    {
        hand.HideGrabHint();
    }

    void HandHoverUpdate(Hand hand)
    {
        GrabTypes grab = hand.GetGrabStarting();

        if(inter.attachedToHand == null && grab!= GrabTypes.None)
        {
            
            hand.AttachObject(gameObject, grab);
            hand.HoverLock(inter);
            hand.HideGrabHint();

        }
        else if(hand.IsGrabEnding(gameObject))
        {
            hand.DetachObject(gameObject);
            hand.HoverUnlock(inter);
        }

    }
}
