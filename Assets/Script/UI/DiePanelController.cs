using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DiePanelController : MonoBehaviour
{
    public static DiePanelController Instance;

    [Header("UI Components")]
    public Image fadeImage;
    public TextMeshProUGUI dieText;
    public float fadeDuration = 1f;

    [Header("Respawn Settings")]
    public Transform respawnPoint;
    public GameObject player;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip fadeInSound;

    private PlayerHealth playerHealth;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();

        // ซ่อน UI ตอนเริ่ม
        fadeImage.gameObject.SetActive(false);
        dieText.gameObject.SetActive(false);
    }

    public void ShowDiePanel()
    {
        StartCoroutine(DieSequence());
    }

    private IEnumerator DieSequence()
    {
        // เปิด UI
        fadeImage.gameObject.SetActive(true);
        dieText.gameObject.SetActive(true);

        // ตั้งค่าเริ่มต้นโปร่งใส
        fadeImage.color = new Color(0, 0, 0, 0);
        dieText.color = new Color(dieText.color.r, dieText.color.g, dieText.color.b, 0);

        // เล่นเสียงตอน fade เข้า
        if (audioSource && fadeInSound)
        {
            audioSource.PlayOneShot(fadeInSound);
        }

        yield return null; // รอ 1 เฟรมก่อนหยุดเวลา
        Time.timeScale = 0;

        // Fade เข้า
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, t);
            dieText.color = new Color(dieText.color.r, dieText.color.g, dieText.color.b, t);
            yield return null;
        }

        // ✅ ให้เวลาเดินต่อหลังจอดำสนิท
        Time.timeScale = 1f;

        yield return new WaitForSeconds(3f); // รอแบบใช้เวลาปกติ

        // รีเซ็ตตำแหน่ง & HP
        player.transform.position = respawnPoint.position;
        playerHealth.health = playerHealth.maxHealth;

        // รีเซ็ต Animator
        Animator anim = player.GetComponent<Animator>();
        if (anim != null)
        {
            anim.Rebind();
            anim.Update(0f);
        }

        // รีเซ็ต isDead (ถ้ามี)
        typeof(PlayerHealth).GetField("isDead", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(playerHealth, false);

        // Fade ออก
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = 1 - (timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, t);
            dieText.color = new Color(dieText.color.r, dieText.color.g, dieText.color.b, t);
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
        dieText.gameObject.SetActive(false);
    }
}
