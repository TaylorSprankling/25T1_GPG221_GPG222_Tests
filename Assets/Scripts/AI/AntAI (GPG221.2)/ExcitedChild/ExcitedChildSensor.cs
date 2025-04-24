using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

public enum ExcitedChildScenario
{
    canSeeNewFire = 0,
    isTooCloseToFire = 1,
    isTooFarFromFire = 2,
    hasExpressedJoy = 3
}

public class ExcitedChildSensor : MonoBehaviour, ISense
{
    [SerializeField] private Vision vision;
    [SerializeField] private float fireTooCloseDistance = 1f;
    [SerializeField] private float fireTooFarDistance = 10f;
    [SerializeField] [ReadOnly] private bool canSeeNewFire;
    [SerializeField] [ReadOnly] private bool isTooCloseToFire;
    [SerializeField] [ReadOnly] private bool isTooFarFromFire;
    [SerializeField] [ReadOnly] private bool hasExpressedJoy;
    
    [SerializeField] private List<GameObject> firesShoutedAt;
    
    public float FireTooCloseDistance => fireTooCloseDistance;
    public List<GameObject> FiresShoutedAt { get => firesShoutedAt; set => firesShoutedAt = value; }
    public GameObject TargetFire { get; set; }
    public GameObject FireToRunFrom { get; set; }
    
    public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
    {
        canSeeNewFire = false;
        foreach (ObjectInVision target in vision.Targets)
        {
            if (!target.objectReference) continue; // null check
            
            if (!target.isInVision || firesShoutedAt.Contains(target.objectReference))
            {
                if (target.objectReference == TargetFire)
                {
                    TargetFire = null;
                }
                continue;
            }
            
            if (target.objectReference.layer == LayerMask.NameToLayer("Fire") && !firesShoutedAt.Contains(target.objectReference))
            {
                canSeeNewFire = true;
                if (!TargetFire)
                {
                    TargetFire = target.objectReference;
                }
            }
        }
        
        isTooCloseToFire = false;
        isTooFarFromFire = false;
        if (TargetFire)
        {
            if ((TargetFire.transform.position - transform.position).magnitude < fireTooCloseDistance)
            {
                FireToRunFrom = TargetFire;
                isTooCloseToFire = true;
            }
            else if ((TargetFire.transform.position - transform.position).magnitude > fireTooFarDistance)
            {
                isTooFarFromFire = true;
            }
        }
        
        if (FireToRunFrom)
        {
            if ((FireToRunFrom.transform.position - transform.position).magnitude < fireTooCloseDistance)
            {
                isTooCloseToFire = true;
            }
            else
            {
                isTooCloseToFire = false;
                FireToRunFrom = null;
            }
        }
        
        aWorldState.Set(ExcitedChildScenario.canSeeNewFire, canSeeNewFire);
        aWorldState.Set(ExcitedChildScenario.isTooCloseToFire, isTooCloseToFire);
        aWorldState.Set(ExcitedChildScenario.isTooFarFromFire, isTooFarFromFire);
        aWorldState.Set(ExcitedChildScenario.hasExpressedJoy, hasExpressedJoy);
    }
}
