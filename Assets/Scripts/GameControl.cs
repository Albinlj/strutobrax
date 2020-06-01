using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using UnityEngine.SocialPlatforms;

public class GameControl : MonoBehaviour {

    public GameObject adControls;
    public int score;
    public int highScoreHard;
    public int highScoreEasy;
    public GameObject[] glassKulor;

    public Text scoreText;
    public Text timerText;
    public Camera cam;
    public Vector3 levelbounds;
    public Vector3 resolution;

    public float comboTime;

    public GameObject glasskula;
    public GameObject mintglasskula;
    public GameObject braxen;
    public PlayerControl player;
    public int livesRemaining;
    public float lifeLossTime;
    public float meltSpeed;
    public GameObject restartButton;
    public GameObject popUp;
    public Canvas renderCanvas;
    public bool dead;
    public Vector3 spawnDistance;
    public Vector3 kulaSpawnPosition;
    public Text highscoreText;
    public float timeOfNewGame;
    public float camShakeAmount = 1;

    //Enemies
    public GameObject crabSpawner;
    public GameObject crab;
    public float crabLeft;
    public bool crabExists;
    public int braxCount;
    int braxMaxAmount;
    int braxMaxAmountMax;
    public float braxenSpeedMultiplier;
    public float braxenSpeedMultiplierIncrease;


    //Menus & UI
    public bool isPaused;
    public GameObject pauseScreen;
    public GameObject gameOverScreen;
    public Text finalScoreText;
    public GameObject mainMenuScreen;
    public GameObject chooseDifficultyScreen;



    float pauseTime = 0;
    public bool mute;
    public bool hardMode;

    public bool deadScreenOn;


    // Use this for initialization
    void Start() {

        //look for local high score
        highScoreHard = PlayerPrefs.GetInt("HscoreHard");
        highScoreEasy = PlayerPrefs.GetInt("HscoreEasy");


        Screen.sleepTimeout = SleepTimeout.NeverSleep;  // Disable screen dimming


        //Set up camera
        if (cam == null) {
            cam = Camera.main;
        }
        resolution = new Vector3(Screen.width, Screen.height, 0);
        levelbounds = cam.ScreenToWorldPoint(resolution);

        mute = false;
        isPaused = true;

        OpenMainMenu();
        pauseScreen.SetActive(false);
        gameOverScreen.SetActive(false);

        ////Initialize Google Play Platform
        //PlayGamesPlatform.DebugLogEnabled = true;
        //PlayGamesPlatform.Activate();
        //Social.localUser.Authenticate((bool success) => {
        //    // handle success or failure
        //});

        Time.timeScale = 0.0f;



        dead = true;
        livesRemaining = 3;
        lifeLossTime = 3;
        deadScreenOn = false;


    }



    void Update() {
        //Handle Touches
        if (Input.touchCount > 0) {
            if (dead == false && isPaused == false) {
                if (Time.realtimeSinceStartup > pauseTime + 0.5f) {
                    PauseGame();
                }
            }
            if (dead && deadScreenOn) {
                NewGame();
            }
        }

        if (Time.time > crabLeft + 10 && !crabExists && !dead) {
            SpawnCrab();
        }

        if (Input.GetKey("p")) {
            if (Time.realtimeSinceStartup > pauseTime + 0.5f) {
                PauseGame();
                pauseTime = Time.realtimeSinceStartup;
            }
        }

        if (Input.GetKey("e")) {
            EndGame();
        }
        if (Input.GetKey("c")) {
            if (crabExists == false) {
                SpawnCrab();
            }
        }
    }



    public void OpenMainMenu() {
        scoreText.enabled = false;
        timerText.enabled = false;
        pauseScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
        chooseDifficultyScreen.SetActive(false);
        player.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void ChooseDifficulty() {
        mainMenuScreen.SetActive(false);
        chooseDifficultyScreen.SetActive(true);
    }

    public void HardDifficulty() {
        hardMode = true;
        braxMaxAmount = 5;
        braxMaxAmountMax = 9;
        meltSpeed = 30;
        braxenSpeedMultiplierIncrease = 1.05f;
        comboTime = 0.8f;
    }

    public void EasyDifficulty() {
        hardMode = false;
        braxMaxAmount = 3;
        braxMaxAmountMax = 4;
        meltSpeed = 15;
        braxenSpeedMultiplierIncrease = 1.025f;
        comboTime = 1f;
    }

    public void NewGame() {
        //destroy all enemies and food, and set count to 0
        GameObject[] braxar = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject brax in braxar) {
            Destroy(brax);
        }
        braxCount = 0;
        GameObject[] glassar = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject glass in glassar) {
            Destroy(glass);
        }
        
        //Reset player and lives
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.transform.position = new Vector3(0, 0, 0);
        livesRemaining = 3;
        dead = false;

        //time related operations
        isPaused = false;
        Time.timeScale = 1;
        crabLeft = Time.time;

        //Enable and disable UI
        mainMenuScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        chooseDifficultyScreen.SetActive(false);
        scoreText.enabled = true;
        timerText.enabled = true;

        //Reset variables
        score = 0;
        timeOfNewGame = Time.time;
        if (hardMode) {
            braxMaxAmount = 5;
            braxenSpeedMultiplier = 1.0f;
        }
        else {
            braxMaxAmount = 3;
            braxenSpeedMultiplier = 0.65f;
        }
        crabExists = false;
        deadScreenOn = false;



        SpawnGlasskula(0);
        SpawnGlasskula(0);
        SpawnGlasskula(0);
        UpdateLives();
        UpdateScore(0, 0);

        StartCoroutine(SpawnerBraxen());
        StartCoroutine(IncreaseDifficulty());


        pauseTime = Time.realtimeSinceStartup;
    }



    IEnumerator IncreaseDifficulty() {
        while (true) {
            yield return new WaitForSeconds(5);


            braxenSpeedMultiplier = braxenSpeedMultiplier * braxenSpeedMultiplierIncrease;
            if (braxMaxAmount < braxMaxAmountMax) {
                braxMaxAmount += 1;
            }

        }
    }

    public void PauseGame() {
        isPaused = !isPaused;
        pauseScreen.SetActive(isPaused);
        if (isPaused) {
            Time.timeScale = 0.0f;
        }
        else {
            Time.timeScale = 1.0f;
        }
        pauseTime = Time.realtimeSinceStartup;
    }

    IEnumerator SpawnerBraxen() {
        while (livesRemaining > 0) {
            if (braxCount < braxMaxAmount) {
                SpawnBraxen();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }



    public void LoseLife() {
        if (Time.time > lifeLossTime + 1f) {
            if (!dead) {
                cam.GetComponent<CameraShake>().shake = camShakeAmount;
                player.ljud[2].Play();
                livesRemaining -= 1;
                lifeLossTime = Time.time;
                UpdateLives();
                Handheld.Vibrate();
                if (livesRemaining <= 0) {
                    EndGame();
                }
            }
        }

    }

    void UpdateLives() {
        timerText.text = "Lives: " + livesRemaining;
    }

    public void UpdateScore(int glassScore, int comboCount) {
        score += glassScore * (comboCount + 1);
        scoreText.text = "Score: " + score;
        //Check Combo Achievements
        if (comboCount == 9) {
            Social.ReportProgress("CgkIlNHiv84VEAIQAw", 100.0f, (bool success) => {
                // handle success or failure
            });
        }
        if (comboCount == 19) {
            Social.ReportProgress("CgkIlNHiv84VEAIQBA", 100.0f, (bool success) => {
                // handle success or failure
            });
        }
        if (comboCount == 29) {
            Social.ReportProgress("CgkIlNHiv84VEAIQAg", 100.0f, (bool success) => {
                // handle success or failure
            });
        }

        //Check for achievements
        if (score >= 10000) {
            Social.ReportProgress("CgkIlNHiv84VEAIQBQ", 100.0f, (bool success) => {
                // handle success or failure
            });
        }
        if (score >= 50000) {
            Social.ReportProgress("CgkIlNHiv84VEAIQCA", 100.0f, (bool success) => {
                // handle success or failure
            });
        }
        if (score >= 100000) {
            Social.ReportProgress("CgkIlNHiv84VEAIQCQ", 100.0f, (bool success) => {
                // handle success or failure
            });
        }
        if (Time.time >= timeOfNewGame + 60) {
            Social.ReportProgress("CgkIlNHiv84VEAIQAQ", 100.0f, (bool success) => {
                // handle success or failure
            });
        }
    }

    public void SpawnPopup(Vector3 worldSpawnPosition, int glassScore, int comboCount) {
        Vector2 viewportPoint = cam.WorldToScreenPoint(worldSpawnPosition);
        GameObject tempTextBox = Instantiate(popUp, viewportPoint, transform.rotation) as GameObject;
        //Parent to the panel and position it
        tempTextBox.transform.SetParent(renderCanvas.transform, false);

        tempTextBox.transform.position = viewportPoint;


        //Set text string
        glassScore = glassScore * (comboCount + 1);
        tempTextBox.transform.GetChild(0).GetComponent<Text>().text = glassScore.ToString();
        Text comboText = tempTextBox.transform.GetChild(1).GetComponent<Text>();

        if (comboCount == 0) {
            comboText.text = "";
        }
        else {
            comboText.text = "Combo x" + (comboCount + 1) + "!";
        }
    }

    public void SpawnGlasskula(int glasskultype) {

        //type 0 = vanlig, type 1 = is

        float safezone = 2;
        kulaSpawnPosition = new Vector3(UnityEngine.Random.Range(-levelbounds.x + safezone, levelbounds.x - safezone), UnityEngine.Random.Range(-levelbounds.y + safezone, levelbounds.y - safezone), 0);
        spawnDistance = kulaSpawnPosition - player.transform.position;

        while (spawnDistance.magnitude < 2.5f) {
            kulaSpawnPosition = new Vector3(UnityEngine.Random.Range(-levelbounds.x + safezone, levelbounds.x - safezone), UnityEngine.Random.Range(-levelbounds.y + safezone, levelbounds.y - safezone), 0);
            spawnDistance = kulaSpawnPosition - player.transform.position;
        }

        Quaternion spawnRotation = Quaternion.identity;
        GameObject kulTyp = glasskula;
        //if (glasskultype == 0) {
        //    kulTyp = glasskula;
        //}
        //else {
        //    kulTyp = mintglasskula;
        //}

        Instantiate(kulTyp, kulaSpawnPosition, spawnRotation);
        //		if (UnityEngine.Random.Range (0, 30) <= 1 && glasskultype == 0) {
        //			SpawnGlasskula(1);
        //		}
    }



    public void SpawnBraxen() {
        bool isFacingRight = randomBoolean();
        float safeZone = 1.0f;
        if (isFacingRight == true) {
            Vector3 spawnPosition = new Vector3(-(levelbounds.x + safeZone), UnityEngine.Random.Range(-levelbounds.y, levelbounds.y), 0);
            Quaternion SpawnRotation = Quaternion.AngleAxis(UnityEngine.Random.Range(-25, 25), Vector3.forward);
            Instantiate(braxen, spawnPosition, SpawnRotation);
        }
        if (isFacingRight == false) {
            Vector3 SpawnPosition = new Vector3((levelbounds.x + safeZone), UnityEngine.Random.Range(-levelbounds.y, levelbounds.y), 0);
            Quaternion SpawnRotation = Quaternion.AngleAxis(UnityEngine.Random.Range(155, 205), Vector3.forward);
            Instantiate(braxen, SpawnPosition, SpawnRotation);
        }
        braxCount += 1;
    }

    void SpawnCrab() {
        Instantiate(crab, crabSpawner.transform.position, Quaternion.AngleAxis(UnityEngine.Random.Range(-25, 25), Vector3.forward));
        crabExists = true;
    }

    void EndGame() {
        deadScreenOn = true;
        dead = true;
        scoreText.enabled = false;
        timerText.enabled = false;
        StopAllCoroutines();

        //Check if highscore is beaten
        if (hardMode) {
            if (score > highScoreHard) {
                PlayerPrefs.SetInt("HScoreHard", score);
                highScoreHard = score;
            }
        }
        else {
            if (score > highScoreEasy) {
                PlayerPrefs.SetInt("HScoreEasy", score);
                highScoreEasy = score;
            }
        }

        //shows score and difficultydependant highscore
        if (hardMode) {
            finalScoreText.text = "Score\t\t\t" + score + "\nhigh score\t\t" + highScoreHard;
        }
        else {
            finalScoreText.text = "Score\t\t\t" + score + "\nhigh score\t\t" + highScoreEasy;
        }
        gameOverScreen.SetActive(true);

        //Report score to Google Play
        if (hardMode) {
            Social.ReportScore(score, "CgkIlNHiv84VEAIQBg", (bool success) => {
                // handle success or failure
            });
        }
        else {
            Social.ReportScore(score, "CgkIlNHiv84VEAIQBw", (bool success) => {
                // handle success or failure
            });
        }

    }



    bool randomBoolean() {
        if (UnityEngine.Random.value >= 0.5) {
            return true;
        }
        return false;
    }
}