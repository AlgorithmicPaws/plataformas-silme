using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    [Header("Missions")]
    [Tooltip("Missions that start automatically when the scene loads")]
    public List<MissionData> autoStartMissions;

    private readonly Dictionary<MissionData, int> _progress = new Dictionary<MissionData, int>();
    private readonly HashSet<MissionData> _active = new HashSet<MissionData>();
    private readonly HashSet<MissionData> _completed = new HashSet<MissionData>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
    }

    private void OnEnable()
    {
        MissionEvents.OnCoinCollected += HandleCoinCollected;
        MissionEvents.OnEnemyKilled += HandleEnemyKilled;
    }

    private void OnDisable()
    {
        MissionEvents.OnCoinCollected -= HandleCoinCollected;
        MissionEvents.OnEnemyKilled -= HandleEnemyKilled;
    }

    private void Start()
    {
        foreach (var mission in autoStartMissions)
            StartMission(mission);
    }

    public void StartMission(MissionData mission)
    {
        if (mission == null || _active.Contains(mission) || _completed.Contains(mission)) return;
        _active.Add(mission);
        _progress[mission] = 0;
        MissionEvents.MissionProgress(mission, 0, mission.targetAmount);
    }

    public void ForceCompleteMission(MissionData mission)
    {
        if (mission == null || _completed.Contains(mission)) return;
        if (!_active.Contains(mission)) _active.Add(mission);
        CompleteMission(mission);
    }

    private void HandleCoinCollected()
    {
        foreach (var mission in new List<MissionData>(_active))
        {
            if (mission.type == MissionType.CollectCoins)
                AdvanceProgress(mission);
        }
    }

    private void HandleEnemyKilled()
    {
        foreach (var mission in new List<MissionData>(_active))
        {
            if (mission.type == MissionType.KillEnemies)
                AdvanceProgress(mission);
        }
    }

    private void AdvanceProgress(MissionData mission)
    {
        _progress[mission]++;
        int current = _progress[mission];
        MissionEvents.MissionProgress(mission, current, mission.targetAmount);
        if (current >= mission.targetAmount)
            CompleteMission(mission);
    }

    private void CompleteMission(MissionData mission)
    {
        _active.Remove(mission);
        _completed.Add(mission);
        UIManager.instance?.UpdateTextScore(mission.rewardScore);
        MissionEvents.MissionCompleted(mission);
    }

    public bool IsMissionActive(MissionData mission) => _active.Contains(mission);
    public bool IsMissionCompleted(MissionData mission) => _completed.Contains(mission);
    public int GetProgress(MissionData mission) => _progress.TryGetValue(mission, out int p) ? p : 0;
}
