using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KillFloor : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve difficultyCurve;

    [SerializeField]
    private Stats stats;

    [SerializeField]
    private Material defaultMaterial;

    [SerializeField]
    private Material flashMaterial;

    [SerializeField]
    private Material killMaterial;

    [SerializeField]
    private List<TileRow> tileRows;

    [SerializeField]
    private float timeBetweenDetonation = 10f;

    [SerializeField]
    private float timeBetweenFlashes = 0.5f;

    [SerializeField]
    private float flashDuration = 0.5f;

    [SerializeField]
    private int numberOfFlashes = 3;

    private void Start()
    {
        AdjustDifficulty();
        SetDefaultMaterials();

        InvokeRepeating("Detonate", timeBetweenDetonation, timeBetweenDetonation);
    }

    private void AdjustDifficulty()
    {
        timeBetweenDetonation = Stats.GetLinearEvaluation(difficultyCurve, stats.killFloorTimeBetweenDetonationsD1, stats.killFloorMaxTimeBetweenDetonationsD10);
    }

    private void SetDefaultMaterials()
    {
        foreach (var tileRow in tileRows)
        {
            foreach (var tile in tileRow.tiles)
            {
                tile.GetComponent<MeshRenderer>().material = defaultMaterial;
            }
        }
    }

    public void Detonate()
    {
        var tiles = new List<KillFloorTile>();
        var pattern = (TilePattern)UnityEngine.Random.Range(0, 4);
        switch (pattern)
        {
            case TilePattern.EveryOther:
                tiles = GetEveryOtherTile();
                break;
            case TilePattern.TargetPlayerCross:
                tiles = GetTargetPlayerCrossTiles();
                break;
            case TilePattern.Cross:
                tiles = GetCrossTiles();
                break;
            case TilePattern.Diagonal:
                tiles = GetDiagonalTiles();
                break;
            default:
                tiles = GetEveryOtherTile();
                break;
        }

        DetonateTiles(tiles);
    }

    private void DetonateTiles(IEnumerable<KillFloorTile> tiles)
    {
        foreach (var tile in tiles)
        {
            StartCoroutine(DetonateTile(tile));
        }
    }

    private IEnumerator DetonateTile(KillFloorTile tile)
    {
        var mesh = tile.GetComponent<MeshRenderer>();
        for (int i = 0; i < numberOfFlashes; i++)
        {
            // Change to flashMaterial
            mesh.material = flashMaterial;

            // Wait flashDuration
            yield return new WaitForSeconds(flashDuration);

            // Change back to defaultMaterial
            mesh.material = defaultMaterial;

            // Wait for timeBetweenFlashes
            yield return new WaitForSeconds(timeBetweenFlashes);
        }

        // Change to killMaterial
        mesh.material = killMaterial;

        // Move the collider down so the player can fall into the lava
        var boxCollider = tile.GetComponent<BoxCollider>();
        boxCollider.center = new Vector3(boxCollider.center.x, boxCollider.center.y - 1, boxCollider.center.z);
        boxCollider.isTrigger = true;

        // Set the tile to active
        tile.SetActive(true);

        // Wait extended flashDuration
        yield return new WaitForSeconds(flashDuration * 2);

        // Change to defaultMaterial
        mesh.material = defaultMaterial;

        // Reset tile
        boxCollider.center = new Vector3(boxCollider.center.x, boxCollider.center.y + 1, boxCollider.center.z);
        boxCollider.isTrigger = false;
        tile.SetActive(false);
    }

    public void ChangeMaterials(IEnumerable<KillFloorTile> tiles)
    {
        foreach (var tile in tiles)
        {
            tile.GetComponent<MeshRenderer>().material = killMaterial;
        }
    }

    private List<KillFloorTile> GetEveryOtherTile()
    {
        var selectedTiles = new List<KillFloorTile>();

        for (int i = 0; i < tileRows.Count; i++)
        {
            // Stagger the rows
            var startIndex = i % 2;
            var tiles = tileRows[i].GetEveryOtherTile(startIndex);
            selectedTiles.AddRange(tiles);
        }

        return selectedTiles;
    }

    private List<KillFloorTile> GetTargetPlayerCrossTiles()
    {
        var selectedTiles = new List<KillFloorTile>();
        var selectedRow = -1;
        var selectedColumn = -1;

        for (int i = 0; i < tileRows.Count; i++)
        {
            for (int k = 0; k < tileRows[i].tiles.Count; k++)
            {
                // OverlapBox to find the tile that the player is currently standing on
                var boxCenter = tileRows[i].tiles[k].transform.position;
                boxCenter = new Vector3(boxCenter.x + 10, boxCenter.y, boxCenter.z + 9);
                var colliders = Physics.OverlapBox(boxCenter, new Vector3(10, 100, 10));
                foreach (var collider in colliders)
                {
                    if (collider.tag == "Player")
                    {
                        selectedRow = i;
                        selectedColumn = k;
                    }
                }
            }
        }

        // Just return a cross pattern if we didn't find the player
        if (selectedRow == -1 || selectedColumn == -1)
        {
            return GetCrossTiles();
        }

        // Select the entire row
        foreach (var tile in tileRows[selectedRow].tiles)
        {
            selectedTiles.Add(tile);
        }

        // Select the entire column
        foreach (var tileRow in tileRows)
        {
            selectedTiles.Add(tileRow.tiles[selectedColumn]);
        }

        return selectedTiles;
    }

    private List<KillFloorTile> GetCrossTiles()
    {
        var selectedTiles = new List<KillFloorTile>();
        var selectedRow = -1;
        var selectedColumn = -1;

        selectedRow = tileRows.Count / 2;
        selectedColumn = tileRows[0].tiles.Count / 2;

        // Select the entire row
        foreach (var tile in tileRows[selectedRow].tiles)
        {
            selectedTiles.Add(tile);
        }

        // Select the entire column
        foreach (var tileRow in tileRows)
        {
            selectedTiles.Add(tileRow.tiles[selectedColumn]);
        }

        return selectedTiles;
    }

    private List<KillFloorTile> GetDiagonalTiles()
    {
        var selectedTiles = new List<KillFloorTile>();


        for (int i = 0; i < tileRows.Count; i++)
        {
            var length = tileRows[i].tiles.Count;
            var first = tileRows[i].tiles[i];
            var second = tileRows[i].tiles[length - 1 - i];
            selectedTiles.Add(first);
            selectedTiles.Add(second);
        }

        return selectedTiles;
    }

    private enum TilePattern
    {
        EveryOther,
        Cross,
        Diagonal,
        TargetPlayerCross,
    }

    [Serializable]
    public class TileRow
    {
        public List<KillFloorTile> tiles;

        public List<KillFloorTile> GetEveryOtherTile(int startIndex) {
            var selectedTiles = new List<KillFloorTile>();

            for (int i = startIndex; i < tiles.Count; i += 2)
            {
                selectedTiles.Add(tiles[i]);
            }

            return selectedTiles;
        }
    }
}


