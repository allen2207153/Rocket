using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 50, 0); // ��?���x (x, y, z)

    private void Update()
    {
        // ��?��?����
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
