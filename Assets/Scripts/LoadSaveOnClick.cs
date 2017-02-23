using UnityEngine;
using System.Collections;

public class LoadSaveOnClick : MonoBehaviour {

    private SaveLoadGame globalController;

    void Start()
    {
        globalController = GameObject.FindGameObjectWithTag("Global Game Controller").GetComponent<SaveLoadGame>();
    }

    public void LoadSave(int saveID)
    {
        globalController.Load(saveID);
    }

}
