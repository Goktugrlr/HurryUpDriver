using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    public float rotationSpeed;

    private Transform target;

    void Update()
    {
            target = GameObject.FindGameObjectWithTag("Target").transform;

            Vector3 direction = target.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);

            toRotation *= Quaternion.Euler(0f, -90f, 0f);

            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        
    }
}
