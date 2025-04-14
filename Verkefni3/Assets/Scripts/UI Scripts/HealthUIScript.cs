using UnityEngine;
using UnityEngine.UIElements;

public class HealthUIScript : MonoBehaviour
{
    public int Health;
    public static HealthUIScript Instance { get; private set; }

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
        scoreLabel = container.Q<Label>("health");
    }

    public void AddHealth(int add)
    {
        Health += add;
        UpdateHealthUI();
    }
    public void DeductHealth(int deduct)
    {
        Health -= deduct;
        UpdateHealthUI();
    }
    public void SetHealth(int amount)
    {
        Health = amount;
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        scoreLabel.text = Health.ToString();
    }
}
