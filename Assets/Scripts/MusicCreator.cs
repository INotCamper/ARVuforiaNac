using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MusicCreator : MonoBehaviour
{
    //the audio clip we want to play
    public AudioClip audioTrack;
    //the listener (main camera for this example)
    public AudioListener listener;
    //the number of samples we want to take
    public float sampleRate = 64;
    //how often we want to take a sample
    public float timeSpace = 0.05f;
    //the game object we will be using to represent our samples
    public GameObject visualPrefab;
    public Transform cam;


    private float[] volData;
    private int numSamples;
    [Min(1)]
    public float ctrlXPos = 100;
    //we'll clamp our positions between these values to avoid erratic placements
    public float maxX = 10.0f;
    public float minX = -10.0f;

    public Transform notesBase;

    public Vector3 playerQuad;

    private GameObject audSorGO;

    private bool audioWasPlaying = false;


    void Start()
    {
        audioWasPlaying = false;
        //create audio player game object and position it at the same point as our audio listener
        GameObject audioPlay = new GameObject("audioPlay");
        audioPlay.AddComponent<AudioSource>();
        audioPlay.transform.position = listener.transform.position;
        audSorGO = audioPlay;

        //prep our number of samples, we clamp it between 64 and 8192 since this is the min and the max for the numSamples argument
        numSamples = Mathf.FloorToInt(Mathf.Clamp(sampleRate * timeSpace, 64, 8192));

        //prep our float arrays
        volData = new float[numSamples];

        InvokeRepeating("PlaceNewObject", 0, timeSpace);
    }

    public void StartMusic()
    {
        AudioSource audioSource = audSorGO.GetComponent<AudioSource>();
        audioSource.clip = audioTrack;
        audioSource.volume = .5f;
        audioSource.spatialBlend = 0.0f;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioWasPlaying = false;
        audSorGO.GetComponent<AudioSource>().Stop();
    }

    void PlaceNewObject()
    {
        //get the output data from the listener
        AudioListener.GetOutputData(volData, 0);

        //get the root mean square of the output data (this is the square root of the average of the samples)
        float curVol = RMS(volData);

        //amplify the volume, and maintain our range of minX and maxX
        float xPos = Mathf.Clamp(curVol * ctrlXPos, minX, maxX);

        Debug.Log(xPos);

        if (xPos > 0)
        {
            //instantiate our new visual object, adjusting x, y and z position as we go
            GameObject newVisual = Instantiate(visualPrefab, new Vector3((xPos > maxX ? maxX : xPos) * playerQuad.x, 0, 0), Quaternion.identity);
            newVisual.GetComponent<Note>().yMultiplier = playerQuad.z;
        }
        if (audSorGO.GetComponent<AudioSource>().isPlaying)
        {
            audioWasPlaying = true;
        }
        if (!audSorGO.GetComponent<AudioSource>().isPlaying && audioWasPlaying)
        {
            StartCoroutine(EndMusic(1.5f));
        }
    }

    float RMS(float[] samples)
    {
        float result = 0.0f;
        //add sample values together
        for (int i = 0; i < samples.Length; i++)
        {
            result += samples[i] * samples[i];
        }
        //get the average of the sample values
        result /= samples.Length;
        //return the square root of the average
        return Mathf.Sqrt(result);
    }

    public IEnumerator EndMusic(float t)
    {
        audioWasPlaying = false;
        yield return new WaitForSeconds(t);
        GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().EndGame();
    }
}
