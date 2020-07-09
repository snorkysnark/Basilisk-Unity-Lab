using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Sorter : Powerable
{
    [SerializeField] Vector3 rowStart = Vector3.zero;
    [SerializeField] Vector3 rowEnd = Vector3.right;
    [SerializeField] Transform[] boxes = null;

    private void OnValidate()
    {
        for(int i = 0; i < boxes.Length; i++)
        {
            if(boxes[i] == null) continue;
            boxes[i].position = CalculatePosition(i);
        }
    }

    private Vector3 CalculatePosition(int positionIndex)
    {
        Vector3 from = transform.TransformPoint(rowStart);
        Vector3 to = transform.TransformPoint(rowEnd);
        Vector3 step = (to - from) / (boxes.Length - 1);
        return from + step * positionIndex;
    }

    public override void Power()
    {
        var sorted = boxes.OrderBy((box) => box.localScale.y);
        int position = 0;
        foreach(Transform box in sorted)
        {
            box.position = CalculatePosition(position);
            position++;
        }
    }
}
