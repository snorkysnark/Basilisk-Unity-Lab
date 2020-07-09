using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Renderer))]
public class Star : Powerable
{
    [SerializeField] Material onMaterial = null;
    [SerializeField] Material offMaterial = null;
    [SerializeField] GameObject myLight = null;

    private Renderer m_renderer;
    private bool on = false;

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
        Assert.IsNotNull(myLight, "Star has no light");
    }

    public override void Power()
    {
        on = !on;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        m_renderer.material = on ? onMaterial : offMaterial;
        myLight.SetActive(on);
    }
}
