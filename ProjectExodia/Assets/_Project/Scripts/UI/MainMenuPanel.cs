using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExodia
{
    public class MainMenuPanel : MenuPanel
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private MMF_Player startFeedbacks;
        [SerializeField] private MMF_Player quitFeedbacks;

        private void Awake()
        {
            startButton.onClick.AddListener(OnStartButton);
            quitButton.onClick.AddListener(OnQuitButton);
        }

        private void OnStartButton()
        {
            Debug.Log("Start button");
            startFeedbacks.PlayFeedbacks();
        }

        private void OnQuitButton()
        {
            Debug.Log("Quit button");
            quitFeedbacks.PlayFeedbacks();
        }
    }
}