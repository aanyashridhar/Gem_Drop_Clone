using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public GameObject manager;
    public ParticleSystem triggerParticles;
    public Color goldColor;
    public GameObject rim;
    public GameObject net;
    private int gamePlay;
    private bool invincible;

    public bool Invincible
    {
        get { return invincible; }
        set { invincible = value; }
    }

    public int GamePlay {
        get {return gamePlay;}
        set {gamePlay = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in net.transform)
        {
            MeshRenderer childRenderer = child.GetComponent<MeshRenderer>();
            if (childRenderer != null)
            {
                childRenderer.sharedMaterial.SetColor("_Color", Color.white);
            }
        }

        foreach (Transform child in rim.transform)
        {
            MeshRenderer childRenderer = child.GetComponent<MeshRenderer>();
            if (childRenderer != null)
            {
                childRenderer.sharedMaterial.SetColor("_Color", Color.red);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        GamePlay = manager.GetComponent<Menu>().GetGamePlay();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("ball"))
        {
            manager.GetComponent<Menu>().AddScore(1);
        }
        else if (other.gameObject.tag.Equals("bomb") && !Invincible)
        {
            if (GamePlay == 1 || GamePlay == 2) {
                manager.GetComponent<Menu>().AddScore(-1);
            }
            else if (GamePlay == 3) {
                manager.GetComponent<Menu>().LostLife();
            }
        }
        else if (other.gameObject.tag.Equals("trophy"))
        {
            Invincible = true;
            triggerParticles.transform.position = other.ClosestPoint(transform.position);
            triggerParticles.Play();
            
            foreach (Transform child in net.transform)
            {
                MeshRenderer childRenderer = child.GetComponent<MeshRenderer>();
                if (childRenderer != null)
                {
                    childRenderer.sharedMaterial.SetColor("_Color", goldColor);
                }
            }

            foreach (Transform child in rim.transform)
            {
                MeshRenderer childRenderer = child.GetComponent<MeshRenderer>();
                if (childRenderer != null)
                {
                    childRenderer.sharedMaterial.SetColor("_Color", goldColor);
                }
            }

            StartCoroutine(PowerUpTimer());
        }
    }

    IEnumerator PowerUpTimer()
    {
        yield return new WaitForSeconds(5);
        
        Invincible = false;

        foreach (Transform child in net.transform)
        {
            MeshRenderer childRenderer = child.GetComponent<MeshRenderer>();
            if (childRenderer != null)
            {
                childRenderer.sharedMaterial.SetColor("_Color", Color.white);
            }
        }

        foreach (Transform child in rim.transform)
        {
            MeshRenderer childRenderer = child.GetComponent<MeshRenderer>();
            if (childRenderer != null)
            {
                childRenderer.sharedMaterial.SetColor("_Color", Color.red);
            }
        }

        StopCoroutine(PowerUpTimer());
    }
}
