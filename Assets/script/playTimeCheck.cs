using UnityEngine;
using UnityEngine.UI;

public class playTimeCheck : MonoBehaviour
{
    private static playTimeCheck instance;
    public static playTimeCheck Instance { get { return instance; } }

    private float playTime;
    public Text playTimeText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update()
    {
        playTime += Time.deltaTime;
        UpdatePlayTimeText();
    }

    private void UpdatePlayTimeText()
    {
        if (playTimeText != null)
        {
            playTimeText.text = "Play Time: " + playTime.ToString("F0"); // 텍스트 업데이트
        }
    }

    public float GetPlayTime()
    {
        return playTime;
    }
}