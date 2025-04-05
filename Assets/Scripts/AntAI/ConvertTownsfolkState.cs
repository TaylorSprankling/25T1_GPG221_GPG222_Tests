using System.Collections;
using Anthill.AI;
using UnityEngine;

public class ConvertTownsfolkState : AntAIState
{
    private FireWorshiperSensor sensor;
    private AudioSource audioSource;
    
    public override void Create(GameObject aGameObject)
    {
        sensor = aGameObject.GetComponent<FireWorshiperSensor>();
        audioSource = aGameObject.GetComponent<AudioSource>();
    }
    
    public override void Enter()
    {
        audioSource.Play();
        StartCoroutine(WaitForSound());
    }
    
    private IEnumerator WaitForSound()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        // Transform townsfolk object into Fire Worshipper object
        sensor.ResetConditions();
        Finish();
    }
}
