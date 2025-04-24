using System.Collections;
using Anthill.AI;
using UnityEngine;

public class ShoutYippeeState : AntAIState
{
    private ExcitedChildSensor sensor;
    private AudioSource audioSource;
    
    public override void Create(GameObject aGameObject)
    {
        sensor = aGameObject.GetComponent<ExcitedChildSensor>();
        audioSource = aGameObject.GetComponent<AudioSource>();
    }
    
    public override void Enter()
    {
        audioSource.Play();
        StopAllCoroutines();
        StartCoroutine(WaitForSound());
    }
    
    private IEnumerator WaitForSound()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        sensor.FiresShoutedAt.Add(sensor.TargetFire);
        sensor.TargetFire = null;
        Finish();
    }
}