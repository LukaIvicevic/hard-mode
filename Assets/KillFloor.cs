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
    private float timeBetweenFlashes;

    [SerializeField]
    private float flashDuration;

    [SerializeField]
    private int numberOfFlashes;

    private void Start()
    {
        SetDefaultMaterials();

        var t = GetEveryOtherTile();
        Detonate(t);
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

    private void Detonate(IEnumerable<KillFloorTile> tiles)
    {
        foreach (var tile in tiles)
        {
            StartCoroutine(Detonate(tile));
        }
    }

    private IEnumerator Detonate(KillFloorTile tile)
    {
        var mesh= tile.GetComponent<MeshRenderer>();
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

        // Check if player was hit

        // Wait extended flashDuration
        yield return new WaitForSeconds(flashDuration * 2);

        // Change to defaultMaterial
        mesh.material = defaultMaterial;
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

    [Serializable]
    public class TileRow
    {
        public List<KillFloorTile> tiles;

        public List<KillFloorTile> GetEveryOtherTile(int startIndex) {
            print(tiles.Count);
            var selectedTiles = new List<KillFloorTile>();

            for (int i = startIndex; i < tiles.Count; i += 2)
            {
                selectedTiles.Add(tiles[i]);
            }

            return selectedTiles;
        }
    }
}


