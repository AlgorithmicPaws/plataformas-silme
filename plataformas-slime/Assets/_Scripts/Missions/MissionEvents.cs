using System;

public static class MissionEvents
{
    public static event Action OnCoinCollected;
    public static event Action OnEnemyKilled;
    public static event Action<MissionData, int, int> OnMissionProgress;
    public static event Action<MissionData> OnMissionCompleted;

    public static void CoinCollected() => OnCoinCollected?.Invoke();
    public static void EnemyKilled() => OnEnemyKilled?.Invoke();
    public static void MissionProgress(MissionData data, int current, int target) =>
        OnMissionProgress?.Invoke(data, current, target);
    public static void MissionCompleted(MissionData data) => OnMissionCompleted?.Invoke(data);
}
