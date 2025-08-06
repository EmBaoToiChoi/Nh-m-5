using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public GameObject panel;                  // Panel chứa cốt truyện
    public TMP_Text storyText;                // Text để hiển thị cốt truyện
    public Button btnNext;                    // Nút tiếp tục
    public Button btnSkip;                    // Nút bỏ qua

    public float typingSpeed = 0.05f;         // Tốc độ gõ chữ

    private string[] storyLines = new string[] {
        "Trong bóng tối sâu thẳm của những hầm ngục cổ xưa, nơi từng bước chân đều vang vọng tiếng gầm gừ của những quái vật khát máu, một huyền thoại mới đang được viết nên.",
        "Bạn là Thợ Săn Hầm Ngục: kẻ mang trong mình lòng dũng cảm, sự khéo léo và khát vọng tìm kiếm những báu vật bị lãng quên. Chỉ có bạn mới có thể khám phá những bí mật ẩn giấu, đối mặt với hiểm nguy rình rập và chinh phục những thử thách khắc nghiệt nhất.",
        "Thế giới đã bị bóng tối nuốt chửng từ khi Thánh vật cổ đại bị phong ấn và quên lãng. Những lời đồn về sức mạnh có thể thay đổi cả số phận nhân loại vẫn âm ỉ trong truyền thuyết. Nhưng nay, cánh cửa địa ngục đã mở, quái vật trỗi dậy, sự sống rơi vào hỗn loạn.",
        "Bạn không chỉ chiến đấu vì vàng bạc hay danh vọng. Trong sâu thẳm, một tiếng gọi bí ẩn đang dẫn dắt bạn, liệu đó là số phận, hay một sức mạnh nào đó đang thao túng? Không ai biết… chỉ có máu, thép, và sự kiên cường mới mở lối trong mê cung chết chóc này.",
        "Hãy sẵn sàng… cuộc săn bắt đầu từ đây.Liệu bạn sẽ là người viết nên kết thúc huy hoàng hay chỉ là một cái xác vô danh trong ngục tối?"
    };

    private int currentLine = 0;
    private bool isTyping = false;

    private Coroutine typingCoroutine;

    void Start()
    {
        // Gán sự kiện
        btnNext.onClick.AddListener(NextLine);
        btnSkip.onClick.AddListener(SkipStory);

        ShowStory();
    }

    public void ShowStory()
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

    public void NextLine()
    {
        if (isTyping)
        {
            // Bỏ qua hiệu ứng gõ để hiển thị nhanh toàn bộ dòng
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
            panel.SetActive(false); // Hết cốt truyện thì tắt panel
        }
    }

    public void SkipStory()
    {
        panel.SetActive(false);
    }
}
