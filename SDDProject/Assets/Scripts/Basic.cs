using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Basic : MonoBehaviour
{
    [SerializeField] private Text playerName;

    public void SwapScene(string sceneName)
    {
        if (playerName)
        {
            if (playerName.text != "")
                PlayerPrefs.SetString("playerName", playerName.text);
            else
                PlayerPrefs.SetString("playerName", "玩家");
        }

        SceneManager.LoadScene(sceneName);
    }
}
