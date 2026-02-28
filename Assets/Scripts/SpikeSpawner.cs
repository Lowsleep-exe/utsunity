using UnityEngine;
using System.Collections;

public class SpikeSpawner : MonoBehaviour
{
    public GameObject Spikes;
    public GameObject Ores;
    public bool spawn;
    public int OreToSpikeCount;

    private float timer;
    public void StartG()
    {
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (spawn == true)
        {
            float fuckoffto = Random.Range(-9.50f, 9.50f);
            transform.position = new Vector3(fuckoffto, transform.position.y);


            int chance = Random.Range(0, 100);

            if (chance > OreToSpikeCount)
            {
                Instantiate(Spikes, transform.position, transform.rotation);

            }
            else if (chance <= OreToSpikeCount)
            {
                GameObject ore = Instantiate(Ores, transform.position, transform.rotation);
                Destroy(ore, 10f);
            }

            float spawnChance = Random.Range(0.25f, 1.75f);

            yield return new WaitForSeconds(spawnChance);
        }

        
    }
}
