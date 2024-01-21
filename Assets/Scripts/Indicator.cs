using UnityEngine;

public class ParkingSpotIndicator : MonoBehaviour
{
    public Color indicatorColor = Color.green;
    public float indicatorHeight = 0.1f;
    public float indicatorWidth = 4f;  // Geniþlik
    public float indicatorDepth = 4f;  // Derinlik

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = indicatorColor;
        Gizmos.DrawCube(transform.position + new Vector3(0, indicatorHeight / 2, 0), new Vector3(indicatorWidth, indicatorHeight, indicatorDepth));
    }
}