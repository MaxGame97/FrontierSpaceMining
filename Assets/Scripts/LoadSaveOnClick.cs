using UnityEngine;
using System.Collections;

public class LoadSaveOnClick : MonoBehaviour {

    private SaveLoadGame globalController;

    void Start()
    {
        globalController = GameObject.Find("GlobalGameController").GetComponent<SaveLoadGame>();
    }

    public void LoadSave(int saveID)
    {
        globalController.Load(saveID);
    }

}
