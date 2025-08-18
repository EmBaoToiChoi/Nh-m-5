using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ArrowGuide : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Tag của player, mặc định: Player")]
    public string playerTag = "Player1";
    [Tooltip("Thời gian mờ dần trước khi xóa")]
    public float fadeOutTime = 0.25f;
    [Tooltip("Âm khi biến mất (không bắt buộc)")]
    public AudioClip vanishSfx;
    public AudioSource audioSource; // có thể để null

    [Header("Optional: Nhấp nháy để dễ thấy")]
    public bool pulse = true;
    public float pulseSpeed = 3f;
    public float pulseMin = 0.8f;
    public float pulseMax = 1.2f;

    private SpriteRenderer[] renderers;
    private bool isVanishing;

    void Awake()
    {
        // Bảo đảm collider là trigger
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;

        // Lấy tất cả SpriteRenderer (cả con)
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (pulse && !isVanishing)
        {
            float s = Mathf.Lerp(pulseMin, pulseMax, (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f);
            transform.localScale = new Vector3(5, 5, 5);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isVanishing) return;
        if (other.CompareTag(playerTag))
        {
            StartCoroutine(FadeAndDestroy());
        }
    }

    IEnumerator FadeAndDestroy()
    {
        isVanishing = true;

        if (vanishSfx && audioSource)
        {
            audioSource.PlayOneShot(vanishSfx);
        }

        float t = 0f;
        // Lưu màu ban đầu
        Color[] startColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            if (renderers[i]) startColors[i] = renderers[i].color;

        // Tắt va chạm để không bị kích hoạt nhiều lần
        var col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            for (int i = 0; i < renderers.Length; i++)
            {
                if (!renderers[i]) continue;
                Color c = startColors[i];
                c.a = a;
                renderers[i].color = c;
            }
            yield return null;
        }

        Destroy(gameObject);
    }
}
