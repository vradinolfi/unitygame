using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVFlicker : MonoBehaviour
{
    Light testLight;
    public float minWaitTime;
    public float maxWaitTime;
    public float minIntensity;
    public float maxIntensity;

    void Start()
    {
        testLight = GetComponent<Light>();
        StartCoroutine(Flashing());
    }

    IEnumerator Flashing()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            //testLight.enabled = !testLight.enabled;
            testLight.intensity = Random.Range(minIntensity, maxIntensity);

        }
    }
}
