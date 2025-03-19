using UnityEngine;

public class AnimationOffsetRandomizer : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        
        animator.Play(0, 0, Random.Range(0f, 1f));
    }
}
