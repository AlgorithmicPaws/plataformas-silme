using System.Collections;
using UnityEngine;

public class BirdEnemy : MonoBehaviour
{
    public enum State { Perched, Diving, Returning }

    [Header("Perch")]
    public float perchTime = 2f;
    public float detectionRange = 8f;

    [Header("Dive")]
    public float diveDuration = 0.9f;
    public float arcHeight = 3f;

    [Header("Return")]
    public float returnSpeed = 4f;

    [Header("References")]
    public Animator animator;

    private State _state = State.Perched;
    private Vector2 _perchPos;
    private Transform _player;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _perchPos = transform.position;
        _sr = GetComponent<SpriteRenderer>();
        if (animator == null) animator = GetComponent<Animator>();

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) _player = playerObj.transform;
    }

    private void OnEnable()
    {
        StartCoroutine(BehaviorLoop());
    }

    private IEnumerator BehaviorLoop()
    {
        while (true)
        {
            SetState(State.Perched);
            yield return new WaitForSeconds(perchTime);

            if (_player == null) continue;
            if (Vector2.Distance(transform.position, _player.position) > detectionRange) continue;

            Vector2 diveTarget = _player.position;
            yield return StartCoroutine(DiveArc(_perchPos, diveTarget));

            SetState(State.Returning);
            yield return StartCoroutine(ReturnToPerch());
        }
    }

    private IEnumerator DiveArc(Vector2 from, Vector2 to)
    {
        SetState(State.Diving);

        Vector2 control = (from + to) * 0.5f + Vector2.up * arcHeight;
        float elapsed = 0f;

        while (elapsed < diveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / diveDuration);
            float tEased = t * t;
            transform.position = QuadraticBezier(from, control, to, tEased);
            FlipToward(to);
            yield return null;
        }

        transform.position = to;
    }

    private IEnumerator ReturnToPerch()
    {
        while (Vector2.Distance(transform.position, _perchPos) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, _perchPos, returnSpeed * Time.deltaTime);
            FlipToward(_perchPos);
            yield return null;
        }
        transform.position = _perchPos;
    }

    private void SetState(State newState)
    {
        _state = newState;
        if (animator == null) return;
        animator.SetBool("isPerched", newState == State.Perched);
        animator.SetBool("isDiving",  newState == State.Diving);
    }

    private void FlipToward(Vector2 target)
    {
        if (_sr == null) return;
        _sr.flipX = target.x < transform.position.x;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_state != State.Diving) return;
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerHealth>()?.Damage();
    }

    private static Vector2 QuadraticBezier(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        float u = 1f - t;
        return u * u * p0 + 2f * u * t * p1 + t * t * p2;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Application.isPlaying ? (Vector3)(Vector2)_perchPos : transform.position, 0.3f);
    }
}
