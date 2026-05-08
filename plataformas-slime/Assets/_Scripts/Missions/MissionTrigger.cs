using UnityEngine;

public enum TriggerAction { StartMission, CompleteMission, StartAndComplete }

public class MissionTrigger : MonoBehaviour
{
    [Header("Mission")]
    public MissionData mission;
    public TriggerAction action = TriggerAction.StartMission;
    public bool oneShot = true;

    private bool _fired;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (oneShot && _fired) return;
        _fired = true;

        switch (action)
        {
            case TriggerAction.StartMission:
                MissionManager.instance?.StartMission(mission);
                break;
            case TriggerAction.CompleteMission:
                MissionManager.instance?.ForceCompleteMission(mission);
                break;
            case TriggerAction.StartAndComplete:
                MissionManager.instance?.StartMission(mission);
                MissionManager.instance?.ForceCompleteMission(mission);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (mission == null) return;
        Gizmos.color = action == TriggerAction.CompleteMission ? Color.green : Color.yellow;
        var col = GetComponent<Collider2D>();
        if (col != null)
            Gizmos.DrawWireCube(transform.position, col.bounds.size);
    }
}
