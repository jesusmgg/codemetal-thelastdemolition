using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public GameObject enemyFighterObject;
    public GameObject enemyCivilObject;
    public GameObject enemyFakeBossObject;
    public GameObject enemyTrueBossObject;

    public AudioSource musicPlayer;
    public AudioClip music1;
    public AudioClip music2;
    public AudioClip musicBoss;
    public AudioClip musicMenu;
    public AudioClip titleAudio;

    public Camera mainCamera;

    public Canvas HUDCanvas;
    public GameObject possibleTargetUIElement;
    public GameObject currentTargetUIElement;
    public GameObject lockingTargetUIElement;
    
    GameObject currentTargetUIGameObject;
    Timer currentTargetUITimer;
    
    public List<GameObject> enemiesList;
    int currentTarget;

    public Timer gameTimer;

    public TextParser dialog;
    int dialogStep;
    
    void Start()
    {
        currentTarget = 0;
        currentTargetUIGameObject = null;
        
        gameTimer = gameObject.AddComponent<Timer>();
        currentTargetUITimer = gameObject.AddComponent<Timer>();
        
        gameTimer.Start();
        currentTargetUITimer.Start();

        dialogStep = 0;

        Time.timeScale = 0.0f;
        
        musicPlayer.Stop();
        musicPlayer.clip = musicMenu;
        musicPlayer.loop = true;
        musicPlayer.Play();
        musicPlayer.PlayOneShot(titleAudio);
    }

    void Update()
    {
        enemiesList.RemoveAll(item => item == null);
        
        bool inputPreviousTarget = Input.GetButtonDown("LB_1");
        bool inputNextTarget = Input.GetButtonDown("RB_1");

        if (inputNextTarget)
        {
            currentTarget += 1;
        }
        
        if (inputPreviousTarget)
        {
            currentTarget -= 1;
        }

        if (currentTarget < 0) currentTarget = enemiesList.Count;
        if (currentTarget > enemiesList.Count - 1) currentTarget = 0;

        int i = 0;
        foreach (GameObject enemy in enemiesList)
        {
            if (enemy == null) continue;
            
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();

            if (inputNextTarget || inputPreviousTarget)
            {
                if (i == currentTarget)
                {
                    Destroy(enemyScript.targetUIElement);

                    enemyScript.targetUIElement = Instantiate(currentTargetUIElement);
                    enemyScript.targetUIElement.transform.SetParent(HUDCanvas.transform, false);

                    currentTargetUIGameObject = enemyScript.targetUIElement;

                    GetComponent<PlayerWeaponsScript>().currentMissileTarget = enemy;
                }
                else
                {
                    Destroy(enemyScript.targetUIElement);

                    enemyScript.targetUIElement = Instantiate(possibleTargetUIElement);
                    enemyScript.targetUIElement.transform.SetParent(HUDCanvas.transform, false);
                }
            }

            Vector3 screenPoint = mainCamera.WorldToScreenPoint(enemy.transform.position);
            if (screenPoint.z > 0)
            {
                enemyScript.targetUIElement.GetComponent<RectTransform>().anchoredPosition =
                    (Vector2)screenPoint - HUDCanvas.GetComponent<RectTransform>().sizeDelta / 2.0f;
            }

            i++;
        }
        
        BlinkCurrentTarget();
        
        GameScript();
    }

    void SpawnEnemy(GameObject enemyObject, Vector3 position)
    {
        GameObject enemy = Instantiate(enemyObject, position, Quaternion.identity);
        EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();

        enemyScript.player = gameObject;
        enemyScript.targetUIElement = Instantiate(possibleTargetUIElement);
        enemyScript.targetUIElement.transform.SetParent(HUDCanvas.transform, false);
        
        enemiesList.Add(enemy);
    }

    void BlinkCurrentTarget()
    {
        if (currentTargetUIGameObject != null)
        {
            if (currentTargetUITimer.GetTime() > 0.5f)
            {
                currentTargetUIGameObject.GetComponent<CanvasRenderer>().SetAlpha(
                    currentTargetUIGameObject.GetComponent<CanvasRenderer>().GetAlpha() > 0.0f ? 1.0f : 0.0f);
                currentTargetUITimer.Reset();
            }
        }
    }

    void GameScript()
    {
        print(enemiesList.Count);
        if (gameTimer.GetTime() > 10.0f && dialogStep == 0)
        {
            dialog.Pasar(5);
            dialogStep++;
            
            SpawnEnemy(enemyCivilObject, new Vector3(6000, 100, 5000));
            SpawnEnemy(enemyCivilObject, new Vector3(8000, 100, 5000));
            SpawnEnemy(enemyCivilObject, new Vector3(7000, 100, 4000));
            
            gameTimer.Reset();
            
            musicPlayer.Stop();
            musicPlayer.clip = music1;
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
        
        if (gameTimer.GetTime() > 3.0f && dialogStep == 1)
        {
            dialog.Pasar(15);
            dialogStep++;
        }
        
        if (dialogStep == 2 && enemiesList.Count == 2)
        {
            dialog.Pasar(7);
            dialogStep++;
        }
        
        if (dialogStep == 3 && enemiesList.Count == 0)
        {
            dialogStep++;
            gameTimer.Reset();
        }
        
        if (dialogStep == 4 && gameTimer.GetTime() > 5.0f)
        {
            dialog.Pasar(4);
            dialogStep++;
            gameTimer.Reset();
            
            SpawnEnemy(enemyFighterObject, new Vector3(11000, 100, 7450));
            SpawnEnemy(enemyFighterObject, new Vector3(12000, 100, 7000));
            SpawnEnemy(enemyFighterObject, new Vector3(12000, 100, 4000));
            
            musicPlayer.Stop();
            musicPlayer.clip = music2;
            musicPlayer.loop = true;
            musicPlayer.Play();
        }

        if (dialogStep == 5 && gameTimer.GetTime() > 3.0f)
        {
            dialog.Pasar(6);
            
            dialogStep++;
            gameTimer.Reset();
        }
        
        if (dialogStep == 6 && enemiesList.Count == 0)
        {
            dialog.Pasar(6);
            
            dialogStep++;
            gameTimer.Reset();
        }
        
        if (dialogStep == 7 && gameTimer.GetTime() > 10.0f)
        {
            musicPlayer.Stop();
            musicPlayer.clip = musicBoss;
            musicPlayer.loop = true;
            musicPlayer.Play();
            
            SpawnEnemy(enemyFighterObject, new Vector3(4000, 100, 11150));
            SpawnEnemy(enemyFighterObject, new Vector3(5000, 100, 7000));
            SpawnEnemy(enemyFighterObject, new Vector3(5000, 100, 12000));
            SpawnEnemy(enemyFighterObject, new Vector3(10000, 100, 12000));
            SpawnEnemy(enemyFighterObject, new Vector3(11000, 100, 11000));
            SpawnEnemy(enemyFighterObject, new Vector3(11000, 100, 2000));
            
            SpawnEnemy(enemyFakeBossObject, new Vector3(4000, 100, 1000));
            
            dialogStep++;
            gameTimer.Reset();
        }
    }
}