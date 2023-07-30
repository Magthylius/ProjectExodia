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
        [SerializeField] private Animator panelAnimator;
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;


        private void Awake()
        {
            startButton.onClick.AddListener(OnStartButton);
            quitButton.onClick.AddListener(OnQuitButton);
        }

        private void OnStartButton()
        {
            Debug.Log("Start button");
            panelAnimator.SetInteger("ButtonMode", 1);
        }

        private void OnQuitButton()
        {
            Debug.Log("Quit button");
            panelAnimator.SetInteger("ButtonMode", 2);
        }
    }
}