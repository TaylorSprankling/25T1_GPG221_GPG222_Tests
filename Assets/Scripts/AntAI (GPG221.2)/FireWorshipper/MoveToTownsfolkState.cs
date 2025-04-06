using Anthill.AI;
using UnityEngine;

public class MoveToTownsfolkState : AntAIState
{
    private MoveForward moveForward;
    private TurnTowards turnTowards;
    private FireWorshiperSensor sensor;
    
    public override void Create(GameObject aGameObject)
    {
        moveForward = GetComponentInParent<MoveForward>();
        turnTowards = GetComponentInParent<TurnTowards>();
        sensor = GetComponentInParent<FireWorshiperSensor>();
    }
    
    public override void Enter()
    {
        turnTowards.TargetPosition = sensor.TargetTownsfolk.transform.position;
        turnTowards.HasTarget = true;
        moveForward.enabled = true;
        turnTowards.enabled = true;
    }
    
    public override void Execute(float aDeltaTime, float aTimeScale)
    {
        if (sensor.TargetTownsfolk)
        {
            turnTowards.TargetPosition = sensor.TargetTownsfolk.transform.position;
        }
    }
    
    public override void Exit()
    {
        turnTowards.HasTarget = false;
        moveForward.enabled = false;
        turnTowards.enabled = false;
    }
}
