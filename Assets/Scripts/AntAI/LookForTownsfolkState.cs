using Anthill.AI;
using UnityEngine;

public class LookForTownsfolkState : AntAIState
{
    private MoveForward moveForward;
    private Wander wander;
    private FireWorshiperSensor sensor;
    
    public override void Create(GameObject aGameObject)
    {
        moveForward = GetComponentInParent<MoveForward>();
        wander = GetComponentInParent<Wander>();
        sensor = GetComponentInParent<FireWorshiperSensor>();
    }
    
    public override void Enter()
    {
        moveForward.enabled = true;
        wander.enabled = true;
    }
    
    public override void Execute(float aDeltaTime, float aTimeScale)
    {
        if (sensor.townsfolkInVision.Count > 0)
        {
            Finish();
        }
    }
    
    public override void Exit()
    {
        moveForward.enabled = false;
        wander.enabled = false;
    }
}
