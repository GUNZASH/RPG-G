using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillManager : MonoBehaviour
{
    [System.Serializable]
    public class Skill
    {
        public Button skillButton;       // ปุ่ม UI ของสกิล
        public GameObject effect;        // เอฟเฟกต์ที่แสดง (ถ้ามี)
        public float cooldownTime;       // เวลาคูลดาวน์
        public float duration;           // ระยะเวลาของบัฟ (ถ้ามี)
        public int requiredLevel;        // เลเวลที่ต้องการปลดล็อค
        public string skillType;         // ชนิดของสกิล ("buffDamage", "heal", "warp", "buffAuraBlade")
        public AudioClip soundEffect;
        public AudioSource audioSource;
    }

    public Skill[] skills;
    public TMP_Text messageText;         // ข้อความแจ้งเตือน "You are not ready to use"
    public Transform warpPoint;          // จุดวาร์ป
    public LevelUp levelUp;              // อ้างอิงระบบเลเวล
    public PlayerHealth playerHealth;    // อ้างอิงระบบ HP
    public PlayerController playerController; // อ้างอิงระบบโจมตี

    private bool[] isCooldown;



    void Start()
    {
        isCooldown = new bool[skills.Length];

        for (int i = 0; i < skills.Length; i++)
        {
            int index = i; // ใช้ index ใหม่เพราะ Lambda
            skills[i].skillButton.onClick.AddListener(() => UseSkill(index));
            UpdateSkillUI(index);
        }

        messageText.gameObject.SetActive(false); // ซ่อนข้อความตั้งแต่แรก
    }

    void Update()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            UpdateSkillUI(i);
        }
    }

    void UpdateSkillUI(int index)
    {
        if (levelUp.level < skills[index].requiredLevel)
        {
            skills[index].skillButton.image.color = new Color(1, 1, 1, 0.5f); // สีจาง
        }
        else
        {
            skills[index].skillButton.image.color = new Color(1, 1, 1, isCooldown[index] ? 0.5f : 1f);
        }
    }

    public void UseSkill(int index)
    {
        Debug.Log("Trying to use skill: " + index); // เช็คว่าสกิลถูกกดหรือไม่

        if (levelUp.level < skills[index].requiredLevel)
        {
            Debug.Log("Level not high enough!");
            StartCoroutine(ShowMessage("Can't Use That"));
            return;
        }

        if (isCooldown[index])
        {
            Debug.Log("Skill is on cooldown!");
            return;
        }

        Debug.Log("Skill Activated!");
        ActivateSkill(skills[index]); // ใช้สกิล
        StartCoroutine(StartCooldown(index, skills[index].cooldownTime));
    }

    void ActivateSkill(Skill skill)
    {
        Debug.Log("ActivateSkill() Called for: " + skill.skillType);

        switch (skill.skillType)
        {
            case "buffDamage":
                Debug.Log("Applying Damage Buff...");
                StartCoroutine(BuffDamage(skill.duration));
                break;

            case "heal":
                Debug.Log("Healing Player...");
                playerHealth.health = Mathf.Min(playerHealth.health + 100f, playerHealth.maxHealth);
                Debug.Log("Player Healed! Current HP: " + playerHealth.health);
                break;

            case "warp":
                Debug.Log("Warping Player to " + warpPoint.position);
                playerController.transform.position = warpPoint.position;
                break;

            case "buffAuraBlade":
                Debug.Log("Activating Aura Blade...");
                StartCoroutine(BuffAuraBlade(skill.duration));
                break;

            default:
                Debug.LogError("Unknown skill type: " + skill.skillType);
                break;
        }

        if (skill.effect)
        {
            skill.effect.SetActive(true);
            StartCoroutine(HideEffectAfter(skill.effect, skill.duration));
        }

        if (skill.soundEffect != null && skill.audioSource != null)
        {
            skill.audioSource.PlayOneShot(skill.soundEffect);
        }
    }

    IEnumerator BuffDamage(float duration)
    {
        Debug.Log("Buff Damage Activated! Before: " + playerController.attackDamage);
        playerController.attackDamage += 30f;
        Debug.Log("After Buff: " + playerController.attackDamage);
        yield return new WaitForSeconds(duration);
        playerController.attackDamage -= 30f;
        Debug.Log("Buff Expired. Current Damage: " + playerController.attackDamage);
    }

    IEnumerator BuffAuraBlade(float duration)
    {
        Debug.Log("Aura Blade Activated! Before: " + playerController.isAuraBladeActive);
        playerController.isAuraBladeActive = true;
        Debug.Log("After Buff: " + playerController.isAuraBladeActive);
        yield return new WaitForSeconds(duration);
        playerController.isAuraBladeActive = false;
        Debug.Log("Aura Blade Expired!");
    }

    IEnumerator StartCooldown(int index, float cooldownTime)
    {
        isCooldown[index] = true;
        UpdateSkillUI(index);
        yield return new WaitForSeconds(cooldownTime);
        isCooldown[index] = false;
        UpdateSkillUI(index);
    }

    IEnumerator HideEffectAfter(GameObject effect, float time)
    {
        yield return new WaitForSeconds(time);
        effect.SetActive(false);
    }

    IEnumerator ShowMessage(string text)
    {
        messageText.text = text;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        messageText.gameObject.SetActive(false);
    }
}
