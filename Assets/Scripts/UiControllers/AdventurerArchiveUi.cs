using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerArchiveUi : MonoBehaviour
{
    public GameObject archiveBlockPrefab;
    public RectTransform archiveBlockContainer;
    public ArchiveBlock startBlock;
    public List<ArchiveBlock> prefabs;

    public void Initialise(ClockData clock)
    {
        gameObject.SetActive(true);

        List<AdventurerData> retiredAdventurers = clock.adventurers.Where((a) => !a.isDead && a.id != clock.activeAdventurer.id).ToList();

        var leftovers = startBlock.Initialise(retiredAdventurers);

        while (leftovers.Count > 0)
        {
            var prefab = Instantiate(archiveBlockPrefab, archiveBlockContainer);

            var archiveBlock = prefab.GetComponent<ArchiveBlock>();
            prefabs.Add(archiveBlock);

            leftovers = archiveBlock.Initialise(retiredAdventurers);
        }
    }

    private void OnDisable()
    {
        foreach (var prefab in prefabs)
        {
            Destroy(prefab);
        }

        prefabs.Clear();
    }
}
