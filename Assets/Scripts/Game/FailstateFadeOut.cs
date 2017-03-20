using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FailstateFadeOut : MonoBehaviour {

    [SerializeField] [Range(1f, 10f)] private float timer = 3f;             // The time it takes until the image starts to fade
    [SerializeField] [Range(0.01f, 1f)] private float fadeSpeed = 0.05f;    // The fade speed

    private Image fadePanel;

    private CanvasGroup missionFailedCanvas;
	
    void Start()
    {
        fadePanel = GameObject.Find("Fade Panel").GetComponent<Image>();

        missionFailedCanvas = GameObject.Find("Mission Failed Panel").GetComponent<CanvasGroup>();
    }

	// Update is called once per frame
	void Update () {
        if(timer < 0f)
        {
            if (fadePanel.color.a >= 0.99f)
            {
                fadePanel.color = new Color(0f, 0f, 0f, 1f);
                
                GameObject.Find("Game Controller").GetComponent<GameControllerBehaviour>().PauseToggle();

                GameObject.Find("UI Controller").SetActive(false);

                missionFailedCanvas.alpha = 1f;
                missionFailedCanvas.interactable = true;
                missionFailedCanvas.blocksRaycasts = true;

                Destroy(gameObject);
            }
            else
                fadePanel.color = Color.Lerp(fadePanel.color, new Color(0f, 0f, 0f, 1f), fadeSpeed * Time.timeScale);
        }
        else
            timer -= Time.deltaTime;
	}
}
