using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MissionFailedController : MonoBehaviour {

	public void RetryMission()
    {
        string levelName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(levelName);
    }

    public void ReturnToHub()
    {
        GameObject levelInfo = GameObject.Find("Level Info(Clone)");

        if (levelInfo != null)
            Destroy(levelInfo);

        SceneManager.LoadScene("Hub");
    }
}
