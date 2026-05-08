using System.Collections;
using TMPro;
using UnityEngine;

public class MissionUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject missionPanel;
    public TextMeshProUGUI missionNameText;
    public TextMeshProUGUI missionProgressText;
    public GameObject completedBanner;

    private Coroutine _hideCoroutine;

    private void Awake()
    {
        missionPanel.SetActive(false);
        if (completedBanner != null) completedBanner.SetActive(false);
        MissionEvents.OnMissionProgress += HandleProgress;
        MissionEvents.OnMissionCompleted += HandleCompleted;
    }

    private void OnDestroy()
    {
        MissionEvents.OnMissionProgress -= HandleProgress;
        MissionEvents.OnMissionCompleted -= HandleCompleted;
    }

    private void HandleProgress(MissionData mission, int current, int target)
    {
        if (_hideCoroutine != null) StopCoroutine(_hideCoroutine);

        missionPanel.SetActive(true);
        if (completedBanner != null) completedBanner.SetActive(false);

        missionNameText.text = mission.missionName;
        missionProgressText.text = mission.type == MissionType.ReachZone
            ? mission.description
            : $"{current} / {target}  {mission.description}";
    }

    private void HandleCompleted(MissionData mission)
    {
        missionPanel.SetActive(true);
        missionNameText.text = mission.missionName;
        missionProgressText.text = "Completada!";

        if (completedBanner != null) completedBanner.SetActive(true);

        if (_hideCoroutine != null) StopCoroutine(_hideCoroutine);
        _hideCoroutine = StartCoroutine(HideAfterDelay(2.5f));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        missionPanel.SetActive(false);
        if (completedBanner != null) completedBanner.SetActive(false);
    }
}
