using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractButtonHandler : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;
    public float openTime = 1.0f; // Adjust as needed

    private bool doorsOpen = false;

    private float openPositionLeft = 1.5f;
    private float closedPositionLeft = 0.393f;

    private float openPositionRight = -2.10f;
    private float closedPositionRight = -.90f;


    private void OnMouseEnter()
    {
        
        Debug.Log("Mouse Enter - Hovering over the button");
        // Example: Change the button's color or scale
        GetComponent<Renderer>().material.color = Color.red;

    }


    private void OnMouseExit()
    {
  
        Debug.Log("Mouse Exit - No longer hovering over the button");
        
        GetComponent<Renderer>().material.color = Color.white;
    }

   
    private void OnMouseDown()
    {
        Debug.Log("Mouse Down - Button Clicked!");
        ToggleDoors();
    }

    void Update()
    {

    }

    private bool IsMouseOverButton()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        return Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject;
    }

    void ToggleDoors()
    {
        doorsOpen = !doorsOpen;

        Vector3 targetPositionLeft = new Vector3(doorsOpen ? openPositionLeft : closedPositionLeft, leftDoor.position.y, leftDoor.position.z);
        Vector3 targetPositionRight = new Vector3(doorsOpen ? openPositionRight : closedPositionRight, rightDoor.position.y, rightDoor.position.z);

        StartCoroutine(LerpDoors(leftDoor, targetPositionLeft));
        StartCoroutine(LerpDoors(rightDoor, targetPositionRight));
    }


    IEnumerator LerpDoors(Transform door, Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startingPos = door.position;

        while (elapsedTime < openTime)
        {
            door.position = Vector3.Lerp(startingPos, targetPosition, elapsedTime / openTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door.position = targetPosition;
    }
}
