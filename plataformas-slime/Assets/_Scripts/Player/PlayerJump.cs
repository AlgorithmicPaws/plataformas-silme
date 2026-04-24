using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private AudioSource _audioSource;

    public float jumpForce;
    public AudioClip jumpSFX;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Jump()
    {
        if (Mathf.Abs(_rigidbody.velocity.y) < 0.01f)
        {
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            if (jumpSFX != null)
                _audioSource.PlayOneShot(jumpSFX);
        }
    }
}
