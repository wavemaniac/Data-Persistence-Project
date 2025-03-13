using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;

    public Text BestScoreText;
    
    private bool m_Started = false;
    private int m_Points;
    private int m_bestPoints = 0;
    private string m_bestName = "";
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        UpdateBestScore();
        

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (m_Points > m_bestPoints)
        {
            m_bestPoints = m_Points;
            m_bestName = NameManager.Instance.playerName;
            UpdateBestScore();
            SaveData();
        }
    }

    void UpdateBestScore()
    {
        BestScoreText.text = "Best Score: " + m_bestPoints.ToString() + " - Name: " + m_bestName;
    }


    [System.Serializable]
    public class PersData
    {
        public string name;
        public int score;
    }

    void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PersData data = JsonUtility.FromJson<PersData>(json);

            m_bestName = data.name;
            m_bestPoints = data.score;
        }
    }

    void SaveData()
    {
        PersData data = new PersData();
        data.score = m_Points;
        data.name = NameManager.Instance.playerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
}
