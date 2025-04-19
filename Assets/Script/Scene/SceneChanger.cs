using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad = "Scene1"; // ใส่ชื่อ Scene ที่จะไป
    public AudioSource audioSource;
    public AudioClip clickSound;

    public void ChangeScene()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // หน่วงเวลานิดให้เสียงเล่นเสร็จก่อนเปลี่ยน Scene
        Invoke("LoadScene", 0.3f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}