using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    ChangeSceneFade fade;

    void Start()
    {
        fade = FindObjectOfType<ChangeSceneFade>();

        fade.FadeOutScene();
    }
}
