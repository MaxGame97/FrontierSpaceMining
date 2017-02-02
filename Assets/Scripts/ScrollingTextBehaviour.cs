using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class ScrollingTextBehaviour : MonoBehaviour {

    [SerializeField] private Text scrollingText;
    [SerializeField] private float speed;
    [SerializeField] private int loops;

    private Vector3 startPos;

    private int currentLoops = 0;

    // Use this for initialization
    void Start () {

        startPos = new Vector3(scrollingText.preferredWidth / 2 + 192, 19.2f);
        ResetText();
    }

    // Update is called once per frame
    void Update()
    {

        Scroll();

    }

    public void setText(string text)
    {
        scrollingText.text = text;
        ResetText();
        currentLoops = 0;
    }

    void ResetText()
    {
        scrollingText.transform.position = startPos;
    }

    void Scroll()
    {
        if(currentLoops < loops) { 

            scrollingText.transform.position += new Vector3(-speed, 0);

            if (scrollingText.transform.position.x < -scrollingText.preferredWidth / 2)
            {
                ResetText();
                currentLoops++;
            }
            
        }
        else
        {
            scrollingText.text = "";
        }
    }
}
