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
        UpdateUI();
        if (levelText == null)
        {
            levelText = GameObject.Find("LevelText")?.GetComponent<TMP_Text>();
        }

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
}
