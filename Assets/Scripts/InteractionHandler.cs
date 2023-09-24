using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : NetworkBehaviour
{
    public Transform aimOrigin;
    [SerializeField] LayerMask hittableMask;
    public Transform hand;
    List<LagCompensatedHit> hits = new();

    

    private void OnEnable()
    {
        GetComponentInChildren<AnimEvents>().Take += InteractionHandler_Take; 
    }
    bool interacted;
    private void InteractionHandler_Take()
    {
        interacted = true;
        GetComponentInChildren<Animator>().ResetTrigger("take");
    }


    public override void FixedUpdateNetwork()
    {
       
        if (GetInput(out NetworkInputData data))
        {
            if (data.isInteracted)
            {
                Vector3 secondPoint = transform.position  + transform.forward;
               int hitCount = Runner.LagCompensation.OverlapSphere(secondPoint, 0.5f, Object.InputAuthority, hits, hittableMask, HitOptions.IncludePhysX);
                if (hitCount>0)
                {
                    for (int i = 0; i < hitCount; i++)
                    {
                        if (hits[i].Collider == GetComponent<Collider>())
                            continue;
                        Vector3 dir = (hits[i].Collider.transform.position - transform.position).normalized + Vector3.up*1.5f;
                        // hits[i].Collider.GetComponent<Rigidbody>().velocity= dir*3;
                        hits[i].Collider.GetComponent<PushControl>().Push(dir*3);
                    }
                }
            }
            if (data.isDropped)
            {
                if (hand.childCount > 0)
                {
                    if (hand.GetChild(0).TryGetComponent(out IDroppable droppable))
                    {
                        droppable.Drop();
                    }

                }
            }

        }

        interacted = false;
    }
}
