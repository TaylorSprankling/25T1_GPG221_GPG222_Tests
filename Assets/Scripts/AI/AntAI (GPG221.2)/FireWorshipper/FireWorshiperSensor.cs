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
    [SerializeField] private GameObject fireWorshiperPrefab;
    
    [SerializeField] [ReadOnly] private bool isOnFire;
    [SerializeField] [ReadOnly] private bool canSeeFire;
    [SerializeField] [ReadOnly] private bool isCloseToFire;
    [SerializeField] [ReadOnly] private bool hasPraisedFire;
    [SerializeField] [ReadOnly] private bool canSeeTownsfolk;
    [SerializeField] [ReadOnly] private bool isCloseToTownsfolk;
    [SerializeField] [ReadOnly] private bool hasConvertedTownsfolk;
    
    public GameObject TargetFire { get; private set; }
    public GameObject TargetTownsfolk { get; private set; }
    public GameObject FireWorshiperPrefab => fireWorshiperPrefab;
    public bool HasPraisedFire { get => hasPraisedFire; set => hasPraisedFire = value; }
    
    public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
    {
        canSeeFire = false;
        canSeeTownsfolk = false;
        foreach (ObjectInVision target in vision.Targets)
        {
            if (!target.objectReference) continue; // null check
            
            if (!target.isInVision)
            {
                if (target.objectReference == TargetFire)
                {
                    TargetFire = null;
                }
                else if (target.objectReference == TargetTownsfolk)
                {
                    TargetTownsfolk = null;
                }
                continue;
            }
            
            switch (target.objectReference.layer)
            {
                case 10: // Fire
                    canSeeFire = true;
                    if (!TargetFire)
                    {
                        TargetFire = target.objectReference;
                    }
                    continue;
                case 11: // Townsfolk
                    canSeeTownsfolk = true;
                    if (!TargetTownsfolk)
                    {
                        TargetTownsfolk = target.objectReference;
                    }
                    continue;
            }
        }
        
        foreach (Collider col in proximity.CollidersInProximity)
        {
            if (col == null) continue;
            if (col.gameObject.layer == LayerMask.NameToLayer("Fire"))
            {
                isCloseToFire = true;
                break;
            }
            isCloseToFire = false;
        }
        
        foreach (Collider col in proximity.CollidersInProximity)
        {
            if (col == null) continue;
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
