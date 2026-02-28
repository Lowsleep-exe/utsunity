using UnityEngine;

public class Spike : MonoBehaviour
{
    public ParticleSystem Explode;
    public GameObject GOE;

    public AudioSource crash;

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag != "Player")
        {
            
            Destroy(this.gameObject, 0.1f);
            GameObject Explode = Instantiate(GOE, transform.position, transform.rotation);
            Explode.GetComponent<ParticleSystem>().Play();
            Destroy(Explode, 1.5f);
        }
        else
        {
            if (crash != null)
            {
                crash.Play();
            }
            Destroy(this.gameObject, 0.1f);
            other.gameObject.GetComponent<Health>().HealthD();
        }
    }
}
