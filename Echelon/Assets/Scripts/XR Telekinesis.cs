using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class XRTelekinesis : MonoBehaviour
{
    public Transform leftHandTransform; // Left XR Controller
    public Transform playerTransform;  // XR Rig
    public float rayDistance = 10000f;                  
    public LayerMask telekinesisLayer;                  
    public float moveSpeedMultiplier = 5f;          
    public float throwMultiplier = 0.35f;    
    public float grabSmoothing = 0.05f;                  
    public float maxThrowSpeed = 10f;                

    private InputAction triggerAction;
    private GameObject grabbedObject;
    private Rigidbody grabbedRb;
    private GameObject hoveredObject;
    private Color originalColor;
    private bool isGrabbing = false;
    private Vector3 lastVelocity;
    private Vector3 previousHandLocalPosition; 
    private Vector3 previousPlayerPosition; 

    void Start()
    {
        // Setup Input Actions
        triggerAction = new InputAction("TriggerPress", InputActionType.Button, "<XRController>{LeftHand}/triggerPressed");
        triggerAction.Enable();
        triggerAction.performed += ctx => OnTriggerPressed();
        triggerAction.canceled += ctx => OnTriggerReleased();

        previousHandLocalPosition = GetRelativeHandPosition();
        previousPlayerPosition = playerTransform.position;
    }

    void Update()
    {
        HandleObjectHighlighting();

        if (isGrabbing && grabbedObject != null)
        {
            Vector3 handLocalPosition = GetRelativeHandPosition();
            Vector3 handMovement = (handLocalPosition - previousHandLocalPosition) * moveSpeedMultiplier;

            grabbedObject.transform.position = Vector3.Lerp(grabbedObject.transform.position, grabbedObject.transform.position + handMovement, 1f - grabSmoothing);

            previousHandLocalPosition = handLocalPosition;

            grabbedObject.transform.rotation = Quaternion.identity;

            lastVelocity = Vector3.ClampMagnitude(handMovement / Time.deltaTime, maxThrowSpeed);
        }
    }

    private void HandleObjectHighlighting()
    {
        RaycastHit hit;
        if (Physics.Raycast(leftHandTransform.position, leftHandTransform.forward, out hit, rayDistance, telekinesisLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hoveredObject != hitObject)
            {
                ClearHighlight();
                hoveredObject = hitObject;

                Renderer objRenderer = hoveredObject.GetComponent<Renderer>();
                if (objRenderer != null)
                {
                    originalColor = objRenderer.material.color;
                    Color highlightColor = Color.Lerp(originalColor, new Color(0.6f, 0.8f, 1f), 0.2f); // 20% blend
                    objRenderer.material.color = highlightColor;
                }
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    private void ClearHighlight()
    {
        if (hoveredObject != null)
        {
            Renderer objRenderer = hoveredObject.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                objRenderer.material.color = originalColor; 
            }
            hoveredObject = null;
        }
    }

    private void OnTriggerPressed()
    {
        RaycastHit hit;
        if (Physics.Raycast(leftHandTransform.position, leftHandTransform.forward, out hit, rayDistance, telekinesisLayer))
        {
            grabbedObject = hit.collider.gameObject;
            grabbedRb = grabbedObject.GetComponent<Rigidbody>();

            if (grabbedRb != null)
            {
                grabbedRb.useGravity = false;
                grabbedRb.linearVelocity = Vector3.zero;
            }

            previousHandLocalPosition = GetRelativeHandPosition();
            previousPlayerPosition = playerTransform.position;
            lastVelocity = Vector3.zero;
            isGrabbing = true;
        }
    }

    private void OnTriggerReleased()
    {
        if (grabbedObject != null)
        {
            if (grabbedRb != null)
            {
                grabbedRb.useGravity = true;
                grabbedRb.linearVelocity = lastVelocity * throwMultiplier;
            }

            // Restore original material
            Renderer objRenderer = grabbedObject.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                objRenderer.material.color = originalColor;
            }

            grabbedObject = null;
            grabbedRb = null;
            isGrabbing = false;
        }
    }

    void OnDestroy()
    {
        triggerAction.performed -= ctx => OnTriggerPressed();
        triggerAction.canceled -= ctx => OnTriggerReleased();
        triggerAction.Disable();
    }

    private Vector3 GetRelativeHandPosition()
    {
        return playerTransform.InverseTransformPoint(leftHandTransform.position);
    }
}
