using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    private int enemyCount;
    public string winScene;

    void Start()
    {
        // Find all objects with the EnemyController component
        EnemyController[] enemies = Object.FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        enemyCount = enemies.Length;
        Debug.Log("Total enemies at start: " + enemyCount);
    }

    public void EnemyDown()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            StartCoroutine(DelaySceneSwitch());
        }
    }
    private IEnumerator DelaySceneSwitch()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(winScene);
    }
}
