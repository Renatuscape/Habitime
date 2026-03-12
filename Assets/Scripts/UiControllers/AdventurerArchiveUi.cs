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
        int totalPrinted = 0;
        gameObject.SetActive(true);

        List<AdventurerData> retiredAdventurers = clock.adventurers.Where((a) => !a.isDead && a.id != clock.activeAdventurer.id).ToList();

        var leftovers = startBlock.Initialise(retiredAdventurers, out totalPrinted);

        int i = leftovers ?? leftovers.Count : 0; // blocks may have more or less than 3 sprite containers in the future

        while (i > 0  && totalPrinted < retiredAdventurers.Count)
        {
            var prefab = Instantiate(archiveBlockPrefab, archiveBlockContainer);

            var archiveBlock = prefab.GetComponent<ArchiveBlock>();
            prefabs.Add(archiveBlock);

            leftovers = archiveBlock.Initialise(retiredAdventurers, out var printed);
            totalPrinted += printed;

            i = leftovers ?? leftovers.Count : 0;

            Debug.Log("Leftover adventurers after printing block: " + i + ". Total printed: " + totalPrinted);
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
