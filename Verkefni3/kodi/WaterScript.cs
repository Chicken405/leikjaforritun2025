using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterScript : MonoBehaviour
{
    private GameManager gameManager;
    public string sceneToLoad;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    private void OnCollisionEnter(Collision hit)
    {
        // Ef player
        if (hit.gameObject.CompareTag("Player"))
        {
            gameManager.CallDeath();
        }
    }
}
