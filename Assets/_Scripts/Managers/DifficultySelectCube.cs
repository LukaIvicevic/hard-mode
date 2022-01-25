using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultySelectCube : MonoBehaviour
{
    [SerializeField]
    private int difficulty = 1;

    [SerializeField]
    private TextMeshPro text;

    [SerializeField]
    private Material unselectedMaterial;

    [SerializeField]
    private Material selectedMaterial;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (text == null)
        {
            Logger.Instance.LogWarning("Text not set on DifficultySelectCube");
            return;
        }
        text.text = difficulty.ToString();

        if (unselectedMaterial == null)
        {
            Logger.Instance.LogWarning("UnselectedMaterial not set on DifficultySelectCube");
        }

        if (selectedMaterial == null)
        {
            Logger.Instance.LogWarning("SelectedMaterial not set on DifficultySelectCube");
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GetDifficulty() == difficulty && meshRenderer.material != selectedMaterial)
        {
            meshRenderer.material = selectedMaterial;
        }
        else if (!(GameManager.Instance.GetDifficulty() == difficulty) && meshRenderer.material != unselectedMaterial)
        {
            meshRenderer.material = unselectedMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var bullet = other.GetComponent<Bullet>();
        if (bullet == null)
        {
            return;
        }

        SelectDifficulty();
    }

    private void SelectDifficulty()
    {
        GameManager.Instance.SelectDifficulty(difficulty);
    }
}