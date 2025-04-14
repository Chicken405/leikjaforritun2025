using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class WinUIButton : MonoBehaviour
{
    private GameManager gameManager;

    public string sceneToLoad;
    public UIDocument buttonDocument;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void OnEnable()
    {
        UIRestart();
    }

    void UIRestart()
    {
        var root = buttonDocument.rootVisualElement;
        var container = root.Q<VisualElement>("container");
        var button = container.Q<Button>("button");

        button.RegisterCallback<ClickEvent>(restart => LoadScene());
    }
    void LoadScene()
    {
        gameManager.CallWin();
        SceneManager.LoadScene(sceneToLoad);
    }
}