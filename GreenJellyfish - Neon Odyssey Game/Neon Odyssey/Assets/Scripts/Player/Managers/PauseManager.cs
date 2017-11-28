﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//---------------------------------------------------------
//-written by: Samuel
//-contributors:
//---------------------------------------------------------

public class PauseManager : MonoBehaviour {

    public GameObject m_topButton = null;
    public string m_menu; //Main menu

    //--------------------------------------------------------------------------------------
    // When game over screen displays, ensure button is initially selected
    //--------------------------------------------------------------------------------------
    public void OnEnable()
    {
        if (m_topButton != null)
            StartCoroutine(HighlightButton(m_topButton)); // Due to unity bug need to set higlighted at end of frame
    }

    //--------------------------------------------------------------------------------------
    // Return to game scene
    //--------------------------------------------------------------------------------------
    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        this.gameObject.SetActive(false);
    }

    //--------------------------------------------------------------------------------------
    // Quit Application
    //--------------------------------------------------------------------------------------
    public void QuitBtn()
    {
        Application.Quit();
    }

    //--------------------------------------------------------------------------------------
    // Return to menu scene
    //--------------------------------------------------------------------------------------
    public void GoToMenuBtn()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(m_menu);
    }

    //--------------------------------------------------------------------------------------
    // Reload scene
    //--------------------------------------------------------------------------------------
    public void RetryLevelBtn()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //--------------------------------------------------------------------------------------
    //  Set the current highlighted button
    //  Due to unity bug need to set higlighted at end of frame
    //
    // Param:
    //		team: whic team is the bullet on, enemy or player layer
    //--------------------------------------------------------------------------------------
    IEnumerator HighlightButton(GameObject button)
    {
        UnityEngine.EventSystems.EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>();

        eventSystem.SetSelectedGameObject(null);

        yield return new WaitForEndOfFrame();
        eventSystem.SetSelectedGameObject(button);
    }
}
