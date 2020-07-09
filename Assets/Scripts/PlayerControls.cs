using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(CharacterController))]
public class PlayerControls : MonoBehaviour
{
    [SerializeField] Transform head = null;
    [SerializeField] float mouseSensitivity = 1;
    [SerializeField] float moveSpeed = 1;
    [SerializeField, Range(-90, 0)] float minHeadAngle = -90;
    [SerializeField, Range(0, 90)] float maxHeadAngle = 90;
    [SerializeField] bool flipY = true;


    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 mouseInput;
    private float headAngle = 0;

#region unity-events
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Assert.IsNotNull(head, "Character has no head transform");
        Assert.IsTrue(head.parent = transform, "Character's head transform should be its child");
    }

    private void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = (flipY ? -1 : 1) * Input.GetAxisRaw("Mouse Y");
        mouseInput = new Vector2(mouseX, mouseY) * mouseSensitivity;
        UpdateRotation(1);
    }

    private void FixedUpdate()
    {
        UpdateMovement(Time.fixedDeltaTime);
    }
#endregion

#region private-methods
    private void UpdateRotation(float deltaTime)
    {
        Vector2 mouseInputSmooth = mouseInput * deltaTime;
        transform.Rotate(0, mouseInputSmooth.x, 0);
        headAngle = Mathf.Clamp(headAngle + mouseInputSmooth.y, minHeadAngle, maxHeadAngle);
        head.localRotation = Quaternion.AngleAxis(headAngle, Vector3.right);
    }

    private void UpdateMovement(float deltaTime)
    {
        Vector3 globalMove = transform.forward * moveInput.y + transform.right * moveInput.x;
        controller.Move(globalMove * moveSpeed * deltaTime);
    }
#endregion
}
