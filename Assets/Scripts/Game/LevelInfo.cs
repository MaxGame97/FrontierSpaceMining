using UnityEngine;
using System.Collections;

public class LevelInfo : MonoBehaviour {

    private int levelID;

    public int LevelID { get { return levelID; } set { levelID = value; } }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
