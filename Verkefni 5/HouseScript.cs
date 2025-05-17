using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseScript : MonoBehaviour
{
    public string winSceneName = "Win";
    public void Victory()
    {
        print("as");
        SceneManager.LoadScene("Win");
    }
}
