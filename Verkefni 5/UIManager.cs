using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    Label healthLabel;
    Label scoreLabel;
    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        healthLabel = root.Q<Label>("health-label");
        scoreLabel = root.Q<Label>("score-label");
    }
    // Breyttir bara labels
    public void SetHealthText(string newText)
    {
        if (healthLabel != null)
            healthLabel.text = newText;
    }

    public void SetScoreText(string newText)
    {
        if (scoreLabel != null)
            scoreLabel.text = newText;
    }
}
