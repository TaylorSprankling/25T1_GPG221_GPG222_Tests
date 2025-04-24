using Anthill.AI;
using UnityEngine;

public class RunFromFireState : AntAIState
{
    private MoveForward moveForward;
    private TurnTowards turnTowards;
    private ExcitedChildSensor sensor;
    
    public override void Create(GameObject aGameObject)
    {
        moveForward = GetComponentInParent<MoveForward>();
        turnTowards = GetComponentInParent<TurnTowards>();
        sensor = GetComponentInParent<ExcitedChildSensor>();
    }
    
    public override void Enter()
    {
        turnTowards.HasTarget = true;
        moveForward.enabled = true;
        turnTowards.enabled = true;
    }
    
    public override void Execute(float aDeltaTime, float aTimeScale)
    {
        var targetPoint = ((sensor.FireToRunFrom.transform.position - transform.position) * -1f) + transform.position;
        turnTowards.TargetPosition = targetPoint;
        Debug.DrawLine(transform.position, targetPoint, Color.red);
    }
    
    public override void Exit()
    {
        sensor.FireToRunFrom = null;
        turnTowards.HasTarget = false;
        moveForward.enabled = false;
        turnTowards.enabled = false;
    }
}