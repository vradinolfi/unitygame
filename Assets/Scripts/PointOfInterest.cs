using UnityEngine;
using TMPro;

public class PointOfInterest : MonoBehaviour
{

    [SerializeField] string textOnInspect;
    public GameObject textPopup;
    [Space]
    public bool requiresKey;
    public GameObject key;

    private TextMeshProUGUI ingameText;
    private bool isInRange;
    private bool gamePaused = false;
    private bool hasKey = false;

    // Start is called before the first frame update
    void Start()
    {
        ingameText = textPopup.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {


            if (requiresKey && !hasKey)
            {
                Inspect();
            } else if (requiresKey && hasKey)
            {
                Unlock();
            } else
            {
                Inspect();
            }


        }
    }

    private void Inspect()
    {
        if (isInRange && !gamePaused)
        {

            ingameText.text = textOnInspect;
            textPopup.SetActive(true);
            Time.timeScale = 0;
            gamePaused = true;

        }
        else
        {
            if (isInRange)
            {

                Time.timeScale = 1f;
                gamePaused = false;
                textPopup.SetActive(false);

            }
        }
    }

    private void Unlock()
    {
        if (isInRange && !gamePaused)
        {

            ingameText.text = "You used the ";
            textPopup.SetActive(true);
            Time.timeScale = 0;
            gamePaused = true;

        }
        else
        {
            if (isInRange)
            {

                Time.timeScale = 1f;
                gamePaused = false;
                textPopup.SetActive(false);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}
