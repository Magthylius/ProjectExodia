using System;
using System.Collections;
using System.Collections.Generic;
using ProjectExodia;
using UnityEngine;

public class BackdropHandler : MonoBehaviour
{
    private SpriteRenderer _sr;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        LevelTransitionManager.OnCountryChange += UpdateTexture;
    }

    private void OnDisable()
    {
        LevelTransitionManager.OnCountryChange -= UpdateTexture;

    }

    void UpdateTexture(CountryPack country)
    {
        _sr.sprite = country.BackDrop;
    }
}
