using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractorwithE : MonoBehaviour
{
    public Transform attachmentPoint; // Set this to the empty GameObject on the player
    public float interactionDistance = 2f; // The distance at which the player can interact with objects
    public float throwForce = 10f; // The force applied when throwing an item
    private GameObject carriedItem = null;
    private bool isDoorRotated = false;
    private Quaternion originalDoorRotation;
    private Vector3 attachmentPointOffset;

    void Start()
    {
        // Store the initial offset from the camera to the attachment point
        attachmentPointOffset = Camera.main.transform.InverseTransformPoint(attachmentPoint.position);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                if (hit.collider.CompareTag("Item"))
                {
                    if (carriedItem == null)
                    {
                        PickUpItem(hit.collider.gameObject);
                    }
                    else
                    {
                        DropItem();
                    }
                }
                else if (hit.collider.CompareTag("Door"))
                {
                    RotateDoor(hit.collider.gameObject);
                }
            }
            else if (carriedItem != null)
            {
                DropItem();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && carriedItem != null)
        {
            ThrowItem();
        }

        // Update the attachment point to follow the camera direction
        if (carriedItem != null)
        {
            UpdateAttachmentPoint();
        }
    }

    void PickUpItem(GameObject item)
    {
        carriedItem = item;
        item.transform.SetParent(attachmentPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    void DropItem()
    {
        carriedItem.transform.SetParent(null);
        Rigidbody rb = carriedItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        carriedItem = null;
    }

    void ThrowItem()
    {
        carriedItem.transform.SetParent(null);
        Rigidbody rb = carriedItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.VelocityChange);
        }
        carriedItem = null;
    }

    void RotateDoor(GameObject door)
    {
        if (!isDoorRotated)
        {
            originalDoorRotation = door.transform.rotation;
            door.transform.Rotate(0, -90, 0);
        }
        else
        {
            door.transform.rotation = originalDoorRotation;
        }
        isDoorRotated = !isDoorRotated;
    }

    void UpdateAttachmentPoint()
    {
        // Update the attachment point's position to maintain the offset from the camera
        attachmentPoint.position = Camera.main.transform.TransformPoint(attachmentPointOffset);
        attachmentPoint.rotation = Camera.main.transform.rotation;
    }
}