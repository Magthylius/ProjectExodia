using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInstance : MonoBehaviour
{
    [SerializeField] private Renderer renderer;

    public Renderer Renderer => renderer;
}
