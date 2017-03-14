using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenuAnimator : MonoBehaviour {

    private Animator mainPanel;
    private Animator optionsPanel;

    private bool mainShowing = false;
    private bool mainSide = false;
    private bool optionsShowing = false;

    void Start()
    {
        mainPanel = GameObject.Find("PausePanel").GetComponent<Animator>();
        optionsPanel = GameObject.Find("OptionsPanel").GetComponent<Animator>();
    }

    void CheckPanels()
    {
        if(mainPanel == null && optionsPanel == null)
        {
            mainPanel = GameObject.Find("PausePanel").GetComponent<Animator>();
            optionsPanel = GameObject.Find("OptionsPanel").GetComponent<Animator>();
        }
    }

    public void ToggleOptionsPanel()
    {
        CheckPanels();
        if (optionsShowing && mainSide)
        {
            optionsPanel.SetTrigger("Close");
            mainPanel.SetTrigger("MoveRight");
            optionsShowing = false;
            mainSide = false;
        }
        else
        {
            optionsPanel.SetTrigger("Open");
            mainPanel.SetTrigger("MoveLeft");
            optionsShowing = true;
            mainSide = true;
        }
    }

    public void TogglePausePanel()
    {
        CheckPanels();
        if (mainShowing)
        {
            if(optionsShowing && mainSide)
            {
                optionsPanel.SetTrigger("InstantClose");
                mainPanel.SetTrigger("InstantRight");
                optionsShowing = false;
                mainSide = false;
            }
            mainPanel.SetTrigger("Close");
            mainShowing = false;
        }
        else
        {
            mainPanel.SetTrigger("Open");
            mainShowing = true;
        }
    }

}
