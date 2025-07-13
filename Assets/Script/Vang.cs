using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Vang : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
   // [SerializeField] private Animator anim;
    [SerializeField] private int scoreValue = 1;

    void Start()
    {
        int savedScore = PlayerPrefs.GetInt("Player");
        UpdateScoreText(savedScore);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int currentScore = PlayerPrefs.GetInt("Player");
            currentScore += scoreValue;
            PlayerPrefs.SetInt("Player", currentScore);
            PlayerPrefs.Save();
            UpdateScoreText(currentScore);
            Destroy(this.gameObject);
        }
    }

 private void UpdateScoreText(int score)
    {
        if (scoreText != null)
      {
           scoreText.text = "X " + score;
       }
    }
}
