using UnityEngine;
using UnityEngine.UIElements;

public class AmmoUIScript : MonoBehaviour
{
    public int Ammo;
    public static AmmoUIScript Instance { get; private set; }

    [SerializeField] private UIDocument uiDocument;
    private Label ammoLabel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Þessi script hverfur ekki þegar senan breyttist
        }
        else
        {
            Debug.LogWarning("Duplicates of AmmoUIScript found!");
        }
    }
    void OnEnable()
    {
        VisualElement root = uiDocument.rootVisualElement;
        VisualElement container = root.Q<VisualElement>("container");
        ammoLabel = container.Q<Label>("ammo");
    }

    public void AddPoints(int add)
    {
        Ammo += add;
        UpdateScoreUI();
    }
    public void DeductPoints(int deduct)
    {
        Ammo -= deduct;
        UpdateScoreUI();
    }
    public void SetPoints(int amount)
    {
        Ammo = amount;
        UpdateScoreUI();
    }
    public void SetString(string value)
    {
        ammoLabel.text = value;
    }

    public void UpdateScoreUI()
    {
        ammoLabel.text = Ammo.ToString();
    }
}
