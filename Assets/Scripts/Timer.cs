using UnityEngine;
using TMPro;
using System.Collections;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = (int)elapsedTime / 60;
        int seconds = (int)elapsedTime % 60;
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public int GetMinuteCount()
    {
        return (int)elapsedTime / 60;
    }

    public int GetSecondsCount()
    {
        return (int)elapsedTime % 60;
    }
}
