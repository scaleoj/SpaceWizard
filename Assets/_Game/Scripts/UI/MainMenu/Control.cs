using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour
{
    public void LoadTestGame()
    {
        Debug.Log("Errorr rorororororro");
        SceneManager.LoadScene("TestStage");
        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
