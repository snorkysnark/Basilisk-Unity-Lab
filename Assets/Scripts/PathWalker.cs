using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PathWalker : MonoBehaviour
{
    [SerializeField] float moveTime = 0.05f;

    public void WalkPath(List<Vector3> path)
    {
        DOTween.Kill(transform);
        StopAllCoroutines();
        StartCoroutine(WalkPathCoroutine(path));
    }

    IEnumerator WalkPathCoroutine(List<Vector3> path)
    {
        transform.position = path[0];
        for(int i = 1; i < path.Count; i++)
        {
            Vector3 point = path[i];
            yield return transform.DOMove(point, moveTime).WaitForCompletion();
        }
    }
}
