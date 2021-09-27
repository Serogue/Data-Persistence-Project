using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Text scoreText;
    public InputField nameInput;

    public string playerName;
    public string hiPlayerName;
    public int hiScore;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHiScore();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            scoreText.text = "Best Score : " + hiPlayerName + " : " + hiScore;
            nameInput.text = playerName;
        }
    }

    private void Start()
    {
        
    }


    [System.Serializable]
    class HiScore
    {
        public int score; //hi score
        public string name; //current name of the player
        public string hiName; //name of the player with highest score
    }

    public void SaveHiScore()
    {
        HiScore toWrite = new HiScore();
        toWrite.score = hiScore;
        toWrite.name = playerName;
        toWrite.hiName = hiPlayerName;

        string path = Application.persistentDataPath + "/savefile.json";

        string json = JsonUtility.ToJson(toWrite);

        File.WriteAllText(path, json);

    }

    public void LoadHiScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HiScore loadedScore = JsonUtility.FromJson<HiScore>(json);

            playerName = loadedScore.name;
            hiPlayerName = loadedScore.hiName;
            hiScore = loadedScore.score;

        }

    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void StartGame()
    {
        playerName = nameInput.text;
        SaveHiScore(); //remember the current name
        SceneManager.LoadScene(1);
    }


}
