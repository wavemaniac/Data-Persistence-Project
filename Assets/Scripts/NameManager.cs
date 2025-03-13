using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NameManager : MonoBehaviour
{

    public static NameManager Instance;

    public string playerName = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerName(TextMeshProUGUI textMesh)
    {
        playerName = textMesh.text;
    }

    public void LoadMain()
    {
        SceneManager.LoadScene(1);
    }
    
}
