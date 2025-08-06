using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    [Header("XP")]
    public float currentXP = 0f;
    public float maxXP = 10f;
    public float increasePerLevel = 5f;

    [Header("Level")]
    public int currentLevel = 0;

    [Header("UI")]
    public TMP_Text levelText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (levelText == null)
        {
            levelText = GameObject.Find("LevelText")?.GetComponent<TMP_Text>();
        }

        LoadXPData(); 

        UpdateUI();
    }



    public void AddXP(float amount)
    {
        currentXP += amount;

        while (currentXP >= maxXP)
        {
            currentXP -= maxXP;
            LevelUp();
        }

        UpdateUI();
    }

    private void LevelUp()
    {
        currentLevel++;
        maxXP += increasePerLevel;

        Debug.Log("🎉 Level Up! Level: " + currentLevel);
        UILevelUp.Instance?.ShowLevelUpUI();
    }

    private void UpdateUI()
    {
        UILevelUp.Instance?.UpdateXPBar(currentXP / maxXP);

        if (levelText != null)
        {
            levelText.text = "LV: " + currentLevel;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        levelText = GameObject.Find("LevelText")?.GetComponent<TMP_Text>();
        UpdateUI();
    }
    [System.Serializable]
    private class XPSaveData
    {
        public int currentLevel;
        public float currentXP;
        public float maxXP;
    }
    public void SaveXPData()
    {
        XPSaveData data = new XPSaveData();
        data.currentLevel = currentLevel;
        data.currentXP = currentXP;
        data.maxXP = maxXP;

        string json = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/xpdata.json", json);

        Debug.Log("✅ XP Data Saved!");
    }

    public void LoadXPData()
    {
        string path = Application.persistentDataPath + "/xpdata.json";

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            XPSaveData data = JsonUtility.FromJson<XPSaveData>(json);

            currentLevel = data.currentLevel;
            currentXP = data.currentXP;
            maxXP = data.maxXP;

            Debug.Log("✅ XP Data Loaded!");
            UpdateUI();
        }
        else
        {
            Debug.Log("⚠️ No XP save file found.");
        }
    }
    private void OnApplicationQuit()
    {
        SaveXPData();
    }




}
