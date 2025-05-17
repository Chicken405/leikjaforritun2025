using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneSwitch : MonoBehaviour
{
    public string sceneToLoad; // Set this in the Inspector
    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;

        Button switchButton = root.Q<Button>("ClickButton");
        switchButton.clicked += SwitchScene;
    }
    void SwitchScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene name is not set.");
        }
    }
}
