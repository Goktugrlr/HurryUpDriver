using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform vehicle;

    private void LateUpdate()
    {
        Vector3 newPosition = vehicle.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, vehicle.eulerAngles.y, 0f);
    }
}