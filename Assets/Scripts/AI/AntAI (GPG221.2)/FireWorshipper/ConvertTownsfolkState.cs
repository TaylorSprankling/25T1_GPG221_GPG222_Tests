using System.Collections;
using Anthill.AI;
using UnityEngine;

public class ConvertTownsfolkState : AntAIState
{
    private FireWorshiperSensor sensor;
    private AudioSource audioSource;
    private GameObject target;
    
    public override void Create(GameObject aGameObject)
    {
        sensor = aGameObject.GetComponent<FireWorshiperSensor>();
        audioSource = aGameObject.GetComponent<AudioSource>();
    }
    
    public override void Enter()
    {
        if (sensor.TargetTownsfolk)
        {
            target = sensor.TargetTownsfolk;
        }
        audioSource.Play();
        StartCoroutine(WaitForSound());
    }
    
    private IEnumerator WaitForSound()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        var position = target.transform.position;
        var rotation = target.transform.rotation;
        sensor.ResetConditions();
        DestroyImmediate(target);
        Instantiate(sensor.FireWorshiperPrefab, position, rotation);
        Finish();
    }
}
