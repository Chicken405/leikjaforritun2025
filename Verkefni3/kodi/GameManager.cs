using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public string deathScene;
    public bool key;
    public bool chest;

    public UIDocument scoreUIDocument;
    public UIDocument highScoreUIDocument;
    public UIDocument ammoUIDocument;
    public UIDocument healthUIDocument;
    public UIDocument WinUIButton;
    public UIDocument WinUI;

    private bool isDebouncing = false; // Debounce fyrir takka

    private void Awake()
    {
        HideAllUI();
        DontDestroyOnLoad(gameObject); // Þessi gameObject hverfur ekki þegar senan breyttist
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "World")
        {
            // Hverfur músinn
            if (Input.GetMouseButtonDown(0) && !isDebouncing)
            {
                isDebouncing = true; // debounce
                UnityEngine.Cursor.visible = false;
                StartCoroutine(LockCursor());
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowCursor();
        }
    }

    // Coroutine that locks the cursor
    public IEnumerator LockCursor()
    {
        // Festa músinn á miðjunni
        yield return new WaitForSeconds(0.3f);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        isDebouncing = false;
    }

    public void ShowCursor()
    {
        // Sýnir músinn
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    public void CallStart()
    {
        // UI displays
        scoreUIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        healthUIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        ammoUIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        highScoreUIDocument.rootVisualElement.style.display = DisplayStyle.None;
        WinUI.rootVisualElement.style.display = DisplayStyle.None;
        WinUIButton.rootVisualElement.style.display = DisplayStyle.None;
    }

    public void CallDeath()
    {
        // Transfer score values
        ScoreUIScript scoreUIScript = GetComponent<ScoreUIScript>();
        scoreUIScript.NewHighScore();
        HighScoreUIScript highScoreUIScript = GetComponent<HighScoreUIScript>();
        highScoreUIScript.Score = scoreUIScript.highScore;

        highScoreUIScript.UpdateScoreUI();

        // UI displays
        scoreUIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        healthUIDocument.rootVisualElement.style.display = DisplayStyle.None;
        ammoUIDocument.rootVisualElement.style.display = DisplayStyle.None;
        WinUI.rootVisualElement.style.display = DisplayStyle.None;
        WinUIButton.rootVisualElement.style.display = DisplayStyle.None;
        highScoreUIDocument.rootVisualElement.style.display = DisplayStyle.Flex;


        SceneManager.LoadScene(deathScene);
    }

    public void CallRestart()
    {
        // Endurræsa
        ScoreUIScript scoreUIScript = GetComponent<ScoreUIScript>();
        scoreUIScript.Score = 0;
        scoreUIScript.UpdateScoreUI();

        // UI displays
        scoreUIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        healthUIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        ammoUIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        highScoreUIDocument.rootVisualElement.style.display = DisplayStyle.None;
        WinUI.rootVisualElement.style.display = DisplayStyle.None;
        WinUIButton.rootVisualElement.style.display = DisplayStyle.None;
    }

    public void CallWinUIButton()
    {
        ScoreUIScript scoreUIScript = GetComponent<ScoreUIScript>();
        // Ef með lykill og 100 stig
        if (WinUIButton.rootVisualElement.style.display == DisplayStyle.None && chest && scoreUIScript.Score >= 100)
        {
            ShowCursor();
            WinUIButton.rootVisualElement.style.display = DisplayStyle.Flex;
        }
    }
    public void CallWin()
    {
        ShowCursor();
        // Fær nýa high score
        ScoreUIScript scoreUIScript = GetComponent<ScoreUIScript>();
        scoreUIScript.NewHighScore();
        HighScoreUIScript highScoreUIScript = GetComponent<HighScoreUIScript>();
        highScoreUIScript.Score = scoreUIScript.highScore;

        highScoreUIScript.UpdateScoreUI();

        // UI displays
        scoreUIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        healthUIDocument.rootVisualElement.style.display = DisplayStyle.None;
        ammoUIDocument.rootVisualElement.style.display = DisplayStyle.None;
        WinUI.rootVisualElement.style.display = DisplayStyle.None;
        WinUIButton.rootVisualElement.style.display = DisplayStyle.None;
        highScoreUIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
    }
    private void HideAllUI()
    {
        // UI displays
        scoreUIDocument.rootVisualElement.style.display = DisplayStyle.None;
        healthUIDocument.rootVisualElement.style.display = DisplayStyle.None;
        ammoUIDocument.rootVisualElement.style.display = DisplayStyle.None;
        highScoreUIDocument.rootVisualElement.style.display = DisplayStyle.None;
        WinUI.rootVisualElement.style.display = DisplayStyle.None;
        WinUIButton.rootVisualElement.style.display = DisplayStyle.None;
    }
}
