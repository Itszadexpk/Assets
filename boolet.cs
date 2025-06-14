using UnityEngine;

public class boolet : MonoBehaviour
{
    public float bulletSpeed = 2f;
    private Rigidbody rb;

    public GameObject bulletHitParticles;
    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
    }
    void Update()
    {
        //rb.MovePosition(transform.position + (transform.forward * bulletSpeed));
    }
    void OnCollisionEnter(Collision collision)
    {
        GameObject particles = Instantiate(bulletHitParticles, collision.contacts[0].point, Quaternion.identity);
        particles.transform.forward = collision.contacts[0].normal;
        Destroy(gameObject);
    }
}
