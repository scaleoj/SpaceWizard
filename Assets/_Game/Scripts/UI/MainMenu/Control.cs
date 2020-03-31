using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour
{
    public void LoadTestGame()
    {
        SceneManager.LoadScene("TestStage");
        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
