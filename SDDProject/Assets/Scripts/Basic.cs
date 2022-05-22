using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Basic : MonoBehaviour
{   public void SwapScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
