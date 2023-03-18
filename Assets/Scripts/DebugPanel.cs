using TMPro;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] TMP_Text debugLabel;

    public void Init()
    {
        Application.logMessageReceived += OnLogMessageReceived;
    }

    public void Clear()
    {
        debugLabel.text = string.Empty;
    }

    private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
    {
        debugLabel.text += $"[{type.ToString()}] {condition}\n";
    }
}
