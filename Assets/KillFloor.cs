using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFloor : MonoBehaviour
{
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
        SetDefaultMaterials();

        InvokeRepeating("Detonate", timeBetweenDetonation, timeBetweenDetonation);
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
            case TilePattern.Cross:
                // TODO
            case TilePattern.TargetPlayerCross:
                // TODO
            case TilePattern.TargetPlayerDiagonal:
                // TODO
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

    private enum TilePattern
    {
        EveryOther,
        Cross,
        TargetPlayerCross,
        TargetPlayerDiagonal,
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


