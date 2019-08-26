using UnityEngine;

public abstract class Saver : MonoBehaviour
{
    public string uniqueIdentifier;
    public SaveData saveData;


    protected string key;


    private SceneController sceneController;


    private void Awake()
    {
        sceneController = FindObjectOfType<SceneController>();

        if(!sceneController)
            throw new UnityException("Scene Controller could not be found, ensure that it exists in the Persistent scene.");
        
        key = SetKey ();
    }


    private void OnEnable()
    {
        sceneController.BeforeSceneUnload += Save;
        sceneController.AfterSceneLoad += Load;
    }


    private void OnDisable()
    {
        sceneController.BeforeSceneUnload -= Save;
        sceneController.AfterSceneLoad -= Load;
    }


    protected abstract string SetKey ();


    protected abstract void Save ();


    protected abstract void Load ();
}
