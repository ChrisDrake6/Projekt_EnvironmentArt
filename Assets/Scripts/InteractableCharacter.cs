using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : MonoBehaviour
{
    [SerializeField] GameObject playerInstruction;
    [SerializeField] GameObject indicator;
    [SerializeField] GameObject[] textContainers;
    [SerializeField] AudioClip[] voiceLines;

    AudioSource source;
    bool playerInRange;
    bool firstTalk = true;
    int randomIndex = 0;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && playerInRange)
        {
            if (!firstTalk)
            {
                randomIndex = Random.Range(1, textContainers.Length);
                foreach (GameObject container in textContainers)
                {
                    container.SetActive(false);
                }
                source.Stop();
            }            
            playerInstruction.SetActive(false);
            firstTalk = false;

            textContainers[randomIndex].SetActive(true);
            source.PlayOneShot(voiceLines[randomIndex]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            indicator.SetActive(false);
            playerInstruction.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            indicator.SetActive(true);
            playerInstruction.SetActive(false);

            foreach (GameObject container in textContainers)
            {
                container.SetActive(false);
            }
            source.Stop();
        }
    }
}
