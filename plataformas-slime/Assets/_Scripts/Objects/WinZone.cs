using UnityEngine;

public class WinZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<NewInput>().enabled = false;
            other.GetComponent<PlayerMovement1>().enabled = false;
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            UIManager.instance.ShowVictory();
        }
    }
}
