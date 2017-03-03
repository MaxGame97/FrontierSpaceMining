using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class _SplashImage : MonoBehaviour {

    [SerializeField] [Range(1f, 10f)] private float timer = 3f;

    private SpriteRenderer spriteRenderer;
	
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

	// Update is called once per frame
	void Update () {
        if(timer < 0)
        {
            Debug.Log(spriteRenderer.color.r);
            
            if (spriteRenderer.color.r < 0.05f)
                SceneManager.LoadScene("Main Menu");
            else
                spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(0f, 0f, 0f, 1f), 0.05f);
        }
        else
            timer -= Time.unscaledDeltaTime;
	}
}
