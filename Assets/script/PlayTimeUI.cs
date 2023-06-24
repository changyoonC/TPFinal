using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayTimeUI : MonoBehaviour
{
    public Text playTimeText;

    private void Update()
    {
        if (playTimeText != null)
        {
            float playTime = playTimeCheck.Instance.GetPlayTime();
            playTimeText.text = "Play Time: " + playTime.ToString("F0");
        }
    }
}