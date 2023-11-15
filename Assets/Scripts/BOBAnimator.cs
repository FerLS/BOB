using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOBAnimator : MonoBehaviour
{
    public delegate void onAnim(int state);


    Animator animator;
    public Arrastrator arrastrator;
    private void Start()
    {
        animator = GetComponent<Animator>();

        arrastrator.OnDrag += DragingAnimation;
        BOBIA.OnMove += MoveAnimation;
    }

    public void DragingAnimation(int dragging)
    {
        animator.SetBool("drag", dragging == 1? true : false);
    }
    public void MoveAnimation(int moving)
    {
        animator.SetInteger("move", moving);
    }
}
