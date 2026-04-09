using UnityEngine;
using UnityEngine.Events;

public class MouseClickController : MonoBehaviour
{
    public Vector3 clickPosition;

    public UnityEvent<Vector3> onClickEvent;

    void Update()
    {
        // Get the mouse click position in world space 
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hitInfo))
            {
                Vector3 clickWorldPosition = hitInfo.point;
                Debug.Log(clickWorldPosition);

                clickPosition = clickWorldPosition;

                Debug.Log("TODO: Trigger event");

                onClickEvent?.Invoke(clickPosition);
            }
        }

        Debug.DrawLine(transform.position, clickPosition, Color.yellow);
        DebugExtension.DebugWireSphere(clickPosition, Color.white, 1, -1);

    }

}
