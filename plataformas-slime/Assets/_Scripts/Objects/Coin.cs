using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinScore;
    public AudioClip collectSFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (collectSFX != null)
                AudioSource.PlayClipAtPoint(collectSFX, transform.position);

            UIManager.instance.UpdateTextScore(coinScore);
            Destroy(gameObject);
        }
    }
}
