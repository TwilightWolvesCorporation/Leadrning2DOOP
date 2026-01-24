using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    private bool _isGrounded;
    
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer != 3) return;
        _isGrounded = true;
        pc.PlayerIsGrounded(true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer != 3) return;
        _isGrounded = false;
        pc.PlayerIsGrounded(false);
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer != 3 || _isGrounded) return;
        _isGrounded = true;
        pc.PlayerIsGrounded(true);
    }
    
}