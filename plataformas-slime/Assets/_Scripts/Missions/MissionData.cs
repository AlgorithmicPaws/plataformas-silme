using UnityEngine;

public enum MissionType { ReachZone, CollectCoins, KillEnemies }

[CreateAssetMenu(fileName = "NewMission", menuName = "Missions/Mission Data")]
public class MissionData : ScriptableObject
{
    public string missionName;
    [TextArea] public string description;
    public MissionType type;
    [Tooltip("Ignored for ReachZone missions")]
    public int targetAmount;
    public int rewardScore;
}
