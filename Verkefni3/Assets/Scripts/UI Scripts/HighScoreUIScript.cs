using UnityEngine;
using UnityEngine.UIElements;

public class HighScoreUIScript : MonoBehaviour
{
    public int Score;
    public static HighScoreUIScript Instance { get; private set; }

    [SerializeField] private UIDocument uiDocument;
    private Label scoreLabel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicates of ScoreUIScript found!");
        }

    }
    void OnEnable()
    {
        VisualElement root = uiDocument.rootVisualElement;
        VisualElement container = root.Q<VisualElement>("container");
        scoreLabel = container.Q<Label>("high-score");
        UpdateScoreUI();
    }

    public void UpdateScoreUI()
    {
        scoreLabel.text = Score.ToString();
    }
}
