using UnityEngine;

public class BehaviourControllerCustomPath : BehaviourController
{
    [SerializeField] private CustomPathCalculator customPathCalculator;
    [SerializeField] private bool stopOnTargetReached = true;
}
