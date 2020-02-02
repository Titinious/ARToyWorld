using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MyComponent
{
    [SerializeField]
    protected string animationName = "a1";


    Animator animator;

    protected override void _set(Dictionary<string, object> args = null)
    {
        base._set(args);

        animator = this.GetComponent<Animator>();
    }

    public void Play(string _animationName)
    {
        animator.Play(_animationName);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator.Play(animationName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
