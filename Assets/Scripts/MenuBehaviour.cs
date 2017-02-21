using UnityEngine;
using System.Collections;

public class MenuBehaviour : MonoBehaviour {

    public static Canvas control;

    private Canvas menuCanvas;

	// Use this for initialization
	void Awake () {
        menuCanvas = gameObject.GetComponentInParent<Canvas>();

  /*      if (control == null)
        {
            DontDestroyOnLoad(menuCanvas.gameObject);
            control = menuCanvas;
        }
        else if (control != menuCanvas)
        {
            Destroy(menuCanvas.gameObject);
        }

*/
    }


    public void ShowMenu()
    {
        menuCanvas.enabled = true;
    }
    public void CloseMenu()
    {
        menuCanvas.enabled = false;
    }



}
