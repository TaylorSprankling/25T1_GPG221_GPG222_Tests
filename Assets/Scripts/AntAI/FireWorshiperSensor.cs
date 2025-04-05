using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

public enum FireWorshiperScenario
{
    isOnFire = 0,
    canSeeFire = 1,
    isCloseToFire = 2,
    canSeeTownsfolk = 3,
    isCloseToTownsfolk = 4,
    hasPraisedFire = 5,
    hasConvertedTownsfolk = 6
}

public class FireWorshiperSensor : MonoBehaviour, ISense
{
    [SerializeField] private Vision vision;
    [SerializeField] private Proximity proximity;
    
    [SerializeField] private bool isOnFire;
    [SerializeField] private bool canSeeFire;
    [SerializeField] private bool isCloseToFire;
    [SerializeField] private bool hasPraisedFire;
    [SerializeField] private bool canSeeTownsfolk;
    [SerializeField] private bool isCloseToTownsfolk;
    [SerializeField] private bool hasConvertedTownsfolk;
    
    public List<Transform> firesInVision;
    public List<Transform> townsfolkInVision;
    
    public bool HasPraisedFire { get => hasPraisedFire; set => hasPraisedFire = value; }
    public bool HasConvertedTownsfolk { get => hasConvertedTownsfolk; set => hasConvertedTownsfolk = value; }
    
    public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
    {
        foreach (ObjectInVision target in vision.Targets)
        {
            switch (target.isInVision)
            {
                case false when firesInVision.Contains(target.objectReference.transform):
                    firesInVision.Remove(target.objectReference.transform);
                    continue;
                case false when townsfolkInVision.Contains(target.objectReference.transform):
                    townsfolkInVision.Remove(target.objectReference.transform);
                    continue;
                case false:
                    continue;
            }
            
            if (target.objectReference.layer == LayerMask.NameToLayer("Townsfolk") && !townsfolkInVision.Contains(target.objectReference.transform))
            {
                townsfolkInVision.Add(target.objectReference.transform);
            }
            else if (target.objectReference.layer == LayerMask.NameToLayer("Fire") && !firesInVision.Contains(target.objectReference.transform))
            {
                firesInVision.Add(target.objectReference.transform);
            }
        }
        canSeeFire = firesInVision.Count > 0;
        canSeeTownsfolk = townsfolkInVision.Count > 0;
        
        foreach (Collider col in proximity.CollidersInProximity)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Fire"))
            {
                isCloseToFire = true;
                break;
            }
            isCloseToFire = false;
        }
        
        foreach (Collider col in proximity.CollidersInProximity)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Townsfolk"))
            {
                isCloseToTownsfolk = true;
                break;
            }
            isCloseToTownsfolk = false;
        }
        
        breakLoop:
        aWorldState.Set(FireWorshiperScenario.isOnFire, false); // not used currently
        aWorldState.Set(FireWorshiperScenario.canSeeFire, canSeeFire);
        aWorldState.Set(FireWorshiperScenario.isCloseToFire, isCloseToFire);
        aWorldState.Set(FireWorshiperScenario.hasPraisedFire, hasPraisedFire);
        aWorldState.Set(FireWorshiperScenario.canSeeTownsfolk, canSeeTownsfolk);
        aWorldState.Set(FireWorshiperScenario.isCloseToTownsfolk, isCloseToTownsfolk);
        aWorldState.Set(FireWorshiperScenario.hasConvertedTownsfolk, hasConvertedTownsfolk);
    }
    
    public void ResetConditions()
    {
        isOnFire = false;
        canSeeFire = false;
        isCloseToFire = false;
        hasPraisedFire = false;
        canSeeTownsfolk = false;
        isCloseToTownsfolk = false;
        hasConvertedTownsfolk = false;
    }
}
