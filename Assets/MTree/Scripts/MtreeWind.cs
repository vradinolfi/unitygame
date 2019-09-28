using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MtreeWind : MonoBehaviour {

    private Renderer[] renderers;
    private MaterialPropertyBlock propBlock;
    public float windStrength = .1f;

	void Awake () {
        renderers = GetComponentsInChildren<Renderer>();
        propBlock = new MaterialPropertyBlock();
        UpdateWind();
    }
	
    public void UpdateWind()
    {
        if (renderers == null)
            renderers = GetComponentsInChildren<Renderer>();
        if (propBlock == null)
            propBlock = new MaterialPropertyBlock();

        foreach (Renderer rend in renderers)
        {
            rend.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_WindStrength", windStrength);
            rend.SetPropertyBlock(propBlock);
        }
    }

    private void OnValidate()
    {
        UpdateWind();
    }
}
