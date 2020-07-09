using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    const string TAG_INTERACTIBLE = "Interactible";

    [SerializeField] float raycastDistance = 5;
    [SerializeField] LayerMask layerMask = default;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, layerMask))
            {
                IInteractible interactible;
                if(hit.transform.tag == TAG_INTERACTIBLE && hit.transform.TryGetComponent<IInteractible>(out interactible))
                {
                    interactible.Interact();
                }
            }
        }
    }
}
