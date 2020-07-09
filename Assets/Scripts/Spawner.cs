using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Powerable
{
    [SerializeField] float radius = 0.5f;
    [SerializeField] WeightedRandomPrefabs prefabs = null;

    public override void Power()
    {
        Vector3 position = transform.position + Random.insideUnitSphere * radius;
        Quaternion rotation = Random.rotation;
        GameObject prefab = prefabs.GetRandom();
        Instantiate(prefab, position, rotation, transform);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
