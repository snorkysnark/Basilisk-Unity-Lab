using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;

public class MoveTargetHere : Powerable
{
    const float DURATION = 0.5f;

    [SerializeField] Transform target = null;

    private void Awake()
    {
        Assert.IsNotNull(target, "MoveTargetHere missing a target");
    }

    public override void Power()
    {
        target.DOMove(transform.position, DURATION);
        //target.position = transform.position;
    }
}
