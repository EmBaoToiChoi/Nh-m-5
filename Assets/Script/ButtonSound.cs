using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSound : MonoBehaviour
{
    private Button m_btn;

    private void Awake()
    {
        m_btn = GetComponent<Button>();
    }

    void Start()
    {
        if (m_btn == null) return;

        // m_btn.onClick.RemoveAllListeners();
        m_btn.onClick.AddListener(() => PlaySound());
    }

    private void PlaySound()
    {
        if (AudioController.Ins == null) return;

        AudioController.Ins.PlaySound(AudioController.Ins.popSound);
    }

}
