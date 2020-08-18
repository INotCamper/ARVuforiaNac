using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Private Fields

    private bool anchorActive = false;
    private bool playerActive = false;

    private bool gameRunning = false;

    private Transform playerTransform;
    private IEnumerator spawnNoteTime;
    private MeshRenderer anchorRenderer;

    #endregion



    #region Public Fields

    public float distanceSpawn = 1f;
    public float spawnRate = 1f;
    public GameObject note;

    #endregion



    #region MonoBehaviour Callbakcs

    private void Awake()
    {
        if (!note)
            Debug.LogError("É necessário um objeto para ser instanciado", this);
        spawnNoteTime = SpawnNoteInTime(spawnRate);
    }

    #endregion



    #region Custom Callbacks

    public void SetAnchor(bool state)
    {
        anchorActive = state;
        if (!state)
        {
            LooseTrackStop();
            return;
        }
        CheckActiveStates();
    }

    public void SetPlayer(bool state)
    {
        playerActive = state;
        if (!state)
        {
            LooseTrackStop();
            return;
        }
        CheckActiveStates();
    }

    private void CheckActiveStates()
    {
        if (playerActive && anchorActive)
            EveryTargetActive();
    }

    private void EveryTargetActive()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        anchorRenderer = GameObject.FindGameObjectWithTag("Anchor").GetComponent<MeshRenderer>();
        gameRunning = true;
        StartCoroutine(spawnNoteTime);
    }

    private void LooseTrackStop()
    {
        playerTransform = null;
        anchorRenderer = null;
        gameRunning = false;
    }

    private Vector3 CreateNotePos()
    {
        Vector3 pos = CleanPlayerPos();
        return new Vector3(Random.Range(0, distanceSpawn) * pos.x, pos.y, 0);
    }

    private Vector3 CleanPlayerPos()
    {
        return new Vector3(playerTransform.position.x > 0 ? 1 : -1, playerTransform.position.y, playerTransform.position.z > 0 ? 1 : -1);
    }

    private void SpawnNote()
    {
        Vector3 notePos = CreateNotePos();
        GameObject n = Instantiate(note, notePos, Quaternion.identity);
        n.GetComponent<Note>().yMultiplier = CleanPlayerPos().z;
        Debug.Log("Note instantiated in " + notePos, n);
        anchorRenderer.material.color = new Color
        {
            r = Random.Range(0f, 1f),
            g = Random.Range(0f, 1f),
            b = Random.Range(0f, 1f),
            a = 1
        };
    }

    #endregion



    #region Ienumerator Callbacks

    IEnumerator SpawnNoteInTime(float freq)
    {
        while (gameRunning)
        {
            SpawnNote();
            yield return new WaitForSeconds(freq);
        }
    }

    #endregion
}
