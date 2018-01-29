using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasarGUI : MonoBehaviour
{
    public List<GameObject> guis = new List<GameObject>();
    private int temporal = 0;

    public AudioSource musicPlayer;
    public AudioClip morse;
    public GameControllerScript gameController;

    // Use this for initialization
    void Start()
    {
        temporal = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("A_1") && temporal < 2)
        {
            temporal++;
            for (int i = 0; i < 3; i++)
            {
                if (i == 1) musicPlayer.PlayOneShot(morse);
                guis[i].SetActive(false);
                
                if (i == 2)
                { 
                    if (gameController.gameTimer != null) gameController.gameTimer.Reset();
                    Time.timeScale = 1.0f;
                }
            }

            guis[temporal].SetActive(true);
        }
    }
}