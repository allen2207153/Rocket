using UnityEngine;

public class PlayerVfxHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        VfxManager.Instance?.HandleCollision(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        VfxManager.Instance?.HandleCollision(collision);
    }
}