using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static int scoreBags = 0;
    public bool keyCollected = false;
    float speed = 5.0f;
    public int ending = 0;
    public static bool bagInMaze2 = false;

    public GameObject scalesEmpty;
    public GameObject scalesFull;
    public GameObject bagOne;
    public GameObject bagTwo;
    public GameObject bagThree;

    public GameObject endingBackground;
    public GameObject endingLogo;

    ChangeSceneFade fade;

    public AudioSource keySound;
    public AudioSource bagUpSound;
    public AudioSource bagDownSound;
    public AudioSource closedGateSound;
    public AudioSource openGateSound;
    public AudioSource deathSound;

    void Start()
    {
        fade = FindObjectOfType<ChangeSceneFade>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.W))
            transform.Translate(0, speed*Time.deltaTime, 0);
        
        if(Input.GetKey(KeyCode.A))
            transform.Translate(-speed*Time.deltaTime, 0, 0);

        if(Input.GetKey(KeyCode.S))
            transform.Translate(0, -speed*Time.deltaTime, 0);

        if(Input.GetKey(KeyCode.D))
            transform.Translate(speed*Time.deltaTime, 0, 0);

        if(Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
    }

   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Walls")
        {
            if(Input.GetKey(KeyCode.W))
                transform.Translate(0, -speed*Time.deltaTime, 0);
        
            if(Input.GetKey(KeyCode.A))
                transform.Translate(speed*Time.deltaTime, 0, 0);

            if(Input.GetKey(KeyCode.S))
                transform.Translate(0, speed*Time.deltaTime, 0);

            if(Input.GetKey(KeyCode.D))
                transform.Translate(-speed*Time.deltaTime, 0, 0);
        }

        if(collision.gameObject.tag == "Keys")
        {
            keySound.Play();
            keyCollected = true;
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.tag == "Shadows")
        {
            deathSound.Play();
            if(SceneManager.GetActiveScene().name == "Maze3" && bagInMaze2 == true)
                scoreBags = 1;
            else
                scoreBags = 0;
            StartCoroutine(RestartScene());
        }

        if(collision.gameObject.tag == "Bags")
        {
            bagUpSound.Play();
            scoreBags++;
            if(SceneManager.GetActiveScene().name == "Maze2" && scoreBags == 1)
                bagInMaze2 = true;
            Debug.Log(bagInMaze2);
            Destroy(collision.gameObject);
            Debug.Log("Bags "+scoreBags);
        }

        if(collision.gameObject.tag == "RightGates")
        {
            if(keyCollected == true)
            {
                openGateSound.Play();
                StartCoroutine(ChangeScene());
            }
            else
            {
                closedGateSound.Play();
                Debug.Log("no key");
            }
        }
        
        if(collision.gameObject.tag == "LeftGates")
        {
            closedGateSound.Play();
        }

        if(collision.gameObject.tag == "Scales")
        {
            bagDownSound.Play();
            Debug.Log("Bags "+scoreBags);

            if(scoreBags==3)
            {
                scalesEmpty.SetActive(false);
                scalesFull.SetActive(true);
                bagOne.SetActive(true);
                bagTwo.SetActive(true);
                bagThree.SetActive(true);
                ending = 3;                     // good ending
                StartCoroutine(Waiting(ending));
            }
            else if(scoreBags<3&&scoreBags>0)
            {
                scalesEmpty.SetActive(false);
                scalesFull.SetActive(true);
                if(scoreBags==1)
                    bagOne.SetActive(true);
                if(scoreBags==2)
                {
                    bagThree.SetActive(true);
                    bagTwo.SetActive(true);
                }
                ending = 2;                     // limbo ending
                StartCoroutine(Waiting(ending));
            }
            else if(scoreBags==0)
            {
                ending = 1;                     // bad ending
                StartCoroutine(Waiting(ending));
            }
            else
            {
                ending = 1;
                StartCoroutine(Waiting(ending));
            }
        }        
    }

    IEnumerator Waiting (int end)    
    {
        yield return new WaitForSeconds(2.0f); 

        switch(end)
        {
            case 3:
                Debug.Log("good ending");
                StartCoroutine(GoodEnding());
                break;
                
            case 2:
                Debug.Log("limbo ending");
                StartCoroutine(LimboEnding());
                break;

            case 1:
                Debug.Log("bad ending");
                StartCoroutine(BadEnding());
                break;

            case 0:
                Debug.Log("WRONG");
                break;

        }
        yield return null;
    }

    IEnumerator GoodEnding() 
    {
        endingLogo.GetComponent<Text>().color = new Color(0,0,0,0);
        endingBackground.SetActive(true);
        StartCoroutine(FadeIn(endingBackground.GetComponent<Image>(), 3.0f, "good"));
        yield return null;
    }

    IEnumerator LimboEnding() 
    {
        StartCoroutine(ChangeScene());
        yield return null;
    }

    IEnumerator BadEnding() 
    {
        endingBackground.SetActive(true);
        StartCoroutine(FadeIn(endingBackground.GetComponent<Image>(), 3.0f, "bad"));
        yield return null;
    }

    IEnumerator FadeIn(Image image, float duration, string ending) {
        Color startColor = image.color;
        if (ending == "good")
            startColor = new Color(1,1,1,0);
        else if (ending == "bad")
            startColor = new Color(0,0,0,0);            
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1);
        float time = 0;
        while (time < duration) {
            time += Time.deltaTime;
            image.color = Color.Lerp(startColor, endColor, time/duration);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f); 
        endingLogo.SetActive(true);
    }

    public IEnumerator ChangeScene()
    {
        fade.FadeInScene();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public IEnumerator RestartScene()
    {
        fade.FadeInScene();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    

}
