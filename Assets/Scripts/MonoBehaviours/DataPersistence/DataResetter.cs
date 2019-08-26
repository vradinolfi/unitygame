using UnityEngine;

public class DataResetter : MonoBehaviour
{
    public ResettableScriptableObject[] resettableScriptableObjects;


	private void Awake ()
    {
	    for (int i = 0; i < resettableScriptableObjects.Length; i++)
	    {
	        resettableScriptableObjects[i].Reset ();
	    }
	}
}
