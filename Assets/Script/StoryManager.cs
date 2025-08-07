using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;
    public TMP_Text storyText;
    public Button btnNext;
    public Button btnSkip;
    public float typingSpeed = 0.05f;

    [Header("Story Settings")]
    [Range(1, 8)] public int storyIndex = 1; // Từ 1 đến 8
    [TextArea(3, 10)] public string[] storyLines;

    private int currentLine = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private string StoryKey => $"Story{storyIndex}HasBeenShown";

    void Start()
    {
        int index = storyIndex - 1;

        if (StoryState.GetFlag(index))
        {
            PlayerPrefs.DeleteKey(StoryKey);
            StoryState.ClearFlag(index);
        }

        if (PlayerPrefs.GetInt(StoryKey, 0) == 1)
        {
            panel.SetActive(false);
            return;
        }

        btnNext.onClick.AddListener(NextLine);
        btnSkip.onClick.AddListener(SkipStory);

        ShowStory();
    }

    void ShowStory()
    {
        panel.SetActive(true);
        currentLine = 0;
        StartTyping();
    }

    void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(storyLines[currentLine]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        storyText.text = "";
        foreach (char c in line)
        {
            storyText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            storyText.text = storyLines[currentLine];
            isTyping = false;
            return;
        }

        currentLine++;
        if (currentLine < storyLines.Length)
        {
            StartTyping();
        }
        else
        {
            PlayerPrefs.SetInt(StoryKey, 1);
            PlayerPrefs.Save();
            panel.SetActive(false);
        }
    }

    void SkipStory()
    {
        PlayerPrefs.SetInt(StoryKey, 1);
        PlayerPrefs.Save();
        panel.SetActive(false);
    }
}
