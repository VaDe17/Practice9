using UnityEngine;

public class ZPositioning : MonoBehaviour
{
    [SerializeField] private Transform mainPosition;

    private void FixedUpdate()
    {
        transform.position = new Vector3 (transform.position.x, transform.position.y, mainPosition.position.y);
    }
}
