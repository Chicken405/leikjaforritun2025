using UnityEngine;
using UnityEngine.UIElements;

public class ScoreUIScript : MonoBehaviour
{
    public int Score;
    public int highScore;
    public static ScoreUIScript Instance { get; private set; }

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
        scoreLabel = container.Q<Label>("score");
    }

    public void AddPoints(int add)
    {
        Score += add;
        UpdateScoreUI();
    }
    public void DeductPoints(int deduct)
    {
        Score -= deduct;
        UpdateScoreUI();
    }
    public void NewHighScore()
    {
        if (Score > highScore)
        {
            highScore = Score;
        }
    }

    public void UpdateScoreUI()
    {
        scoreLabel.text = Score.ToString();
        CheckScore();
    }
    public void CheckScore()
    {
        if (Score >= 100)
        {
            GameObject gameManager = GameObject.FindWithTag("GameManager");
            if (!gameObject)
            {
                Debug.LogError("No GameManager found! Very bad!!!");
            }
            GameManager gameManagerScript = gameManager.GetComponent<GameManager>();
            gameManagerScript.CallWinUIButton();
        }
    }
}
