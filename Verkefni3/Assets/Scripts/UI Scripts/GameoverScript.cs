using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameoverScript : MonoBehaviour
{
    private GameManager gameManager;

    public string sceneToLoad;
    public UIDocument buttonDocument;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        ShowCursor();
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

    void ShowCursor()
    {
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    void LoadScene()
    {
        gameManager.CallRestart();
        SceneManager.LoadScene(sceneToLoad);
    }
}