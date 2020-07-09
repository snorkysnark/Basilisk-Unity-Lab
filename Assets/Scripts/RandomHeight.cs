using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHeight : MonoBehaviour
{
    [SerializeField] float minHeight = 0.2f;
    [SerializeField] float maxHeight = 4f;

    private void Start()
    {
        Reroll();
    }

    public void Reroll()
    {
        float newHeight = Random.Range(minHeight, maxHeight);
        transform.localScale = new Vector3(1, newHeight, 1);
    }
}
