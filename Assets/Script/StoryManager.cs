using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [Header("Danh sách ảnh cốt truyện")]
    public Image storyImage;                // UI Image để hiển thị
    public Sprite[] storySprites;           // 7 ảnh cốt truyện
    private int currentIndex = 0;           // Ảnh hiện tại

    [Header("Nút Tiếp tục")]
    public Button continueButton;   
    public Button continueButton1;
    public Button continueButton2;        // Nút tiếp tục

    [Header("Panel hiển thị cốt truyện")]
    public GameObject storyPanel;           // Panel chứa ảnh

    void Start()
    {
        if (storySprites.Length > 0)
        {
            storyImage.sprite = storySprites[0];  // Hiện ảnh đầu tiên
        }

        continueButton.onClick.AddListener(OnContinueClicked);
        continueButton1.onClick.AddListener(OnContinueClicked);
        continueButton2.onClick.AddListener(OnContinueClicked);
    }

    void OnContinueClicked()
    {
        currentIndex++; // Sang ảnh tiếp theo

        if (currentIndex < storySprites.Length)
        {
            // Cập nhật ảnh mới
            storyImage.sprite = storySprites[currentIndex];
        }
        else
        {
            // Đã hết 7 ảnh → tắt panel
            storyPanel.SetActive(false);
        }
    }
}
