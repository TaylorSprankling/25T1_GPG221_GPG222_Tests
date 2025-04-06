using Anthill.AI;
using UnityEngine;

public class LookForTownsfolkState : AntAIState
{
    private MoveForward moveForward;
    private Wander wander;
    
    public override void Create(GameObject aGameObject)
    {
        moveForward = GetComponentInParent<MoveForward>();
        wander = GetComponentInParent<Wander>();
    }
    
    public override void Enter()
    {
        moveForward.enabled = true;
        wander.enabled = true;
    }
    
    public override void Exit()
    {
        moveForward.enabled = false;
        wander.enabled = false;
    }
}
