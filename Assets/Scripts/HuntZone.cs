using UnityEngine;

public class HuntZone : MonoBehaviour
{
    [SerializeField] private PatrolEnemy enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemy.PlayerDetect(collision.transform, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemy.PlayerDetect(null, false);
        }
    }
}