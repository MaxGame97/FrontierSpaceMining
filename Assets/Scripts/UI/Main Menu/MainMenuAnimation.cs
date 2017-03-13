using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuAnimation : MonoBehaviour {

    private Animator mainPanel;
    private Animator playPanel;
    private Animator optionsPanel;

    private bool mainShowing = true;
    private bool mainSide = false;
    private bool playShowing = false;
    private bool optionsShowing = false;


    void Start()
    {
        mainPanel = GameObject.Find("MainMenuPanel").GetComponent<Animator>();
        playPanel = GameObject.Find("PlayGamePanel").GetComponent<Animator>();
        optionsPanel = GameObject.Find("OptionsPanel").GetComponent<Animator>();
    }

    public void TogglePlayPanel()
    {
        if (playShowing)
        {
            playPanel.SetTrigger("Close");
            mainPanel.SetTrigger("MoveToRight");
            playShowing = false;
            mainSide = false;
        }
        else
        {
            if (optionsPanel && mainSide)
            {
                optionsPanel.SetTrigger("Close");
                playPanel.SetTrigger("Open");
                optionsShowing = false;
            }
            else
            {
                mainPanel.SetTrigger("MoveToLeft");
                playPanel.SetTrigger("InstantOpen");
                mainSide = true;
            }
            playShowing = true;
        }
    }

    public void ToggleOptionsPanel()
    {
        if (optionsShowing)
        {
            optionsPanel.SetTrigger("Close");
            mainPanel.SetTrigger("MoveToRight");
            optionsShowing = false;
            mainSide = false;
        }
        else
        {
            if (playPanel && mainSide)
            {
                playPanel.SetTrigger("Close");
                optionsPanel.SetTrigger("Open");
                playShowing = false;
            }
            else
            {
                mainPanel.SetTrigger("MoveToLeft");
                optionsPanel.SetTrigger("InstantOpen");
                mainSide = true;
            }
            optionsShowing = true;
        }
    }

}
