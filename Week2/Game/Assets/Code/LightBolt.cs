using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class LightBolt : MonoBehaviour
{
    // lightning stuff - http://atimarin.wordpress.com/2013/09/25/lightning-strike-on-unity-3d-using-line-renderer/
    // -----------------------------
    private LineRenderer line;
    private Transform myTransform;
    private Transform finalPos;

    public Transform Target;
    public float NumberOfBends = 4;

    public Vector2 RandomStartMin;
    public Vector2 RandomStartMax;

    public Vector2 RandomMiddleMin;
    public Vector2 RandomMiddleMax;

    public Vector2 RandomEndMin;
    public Vector2 RandomEndMax;

    public Light DirectionalLight;

    public string LayerName = "Default";

    public bool SpawnBoltStrikesRandomly = false;
    public float MinInterval = 0.2f;
    public float MaxInterval = 5f;
    // -----------------------------

    // camera shake stuff -  https://gist.github.com/ftvs/5822103
    // -----------------------------

    public bool UseCameraShake = true;

    public Transform camTransform;

    public float ShakeDuration = 0.2f;

    public float CameraShakeAmount = 0.2f;
    public float CameraShakeDecreaseFactor = 1.0f;

    private float shakeTimer;

    Vector3 originalPos;

    // -----------------------------


    // Use this for initialization
    void Start()
    {
        // camera setup
        if (camTransform == null)
            camTransform = Camera.main.transform;

        if (DirectionalLight != null)
            DirectionalLight.enabled = false;

        // line renderer setup
        line = GetComponent<LineRenderer>();
        line.SetVertexCount((int)NumberOfBends);

        myTransform = gameObject.transform;

        finalPos = Target.transform;

        originalPos = camTransform.localPosition;

        if (SpawnBoltStrikesRandomly)
            StartCoroutine(StartRandomBolts(MinInterval, MaxInterval));

        line.renderer.sortingLayerName = LayerName;

    }

    IEnumerator StartRandomBolts(float min, float max)
    {
        var timer = Random.Range(min, max);

        while (true)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = Random.Range(min, max);
                StartCoroutine(SpawnSingleLightBolt(Random.Range(0.1f, 0.4f)));
            }

            yield return null;
        }
    }

    IEnumerator SpawnSingleLightBolt(float duration)
    {
        CreateLightBolt();
        yield return new WaitForSeconds(duration);
        line.enabled = false;
    
        if (DirectionalLight != null)
            DirectionalLight.enabled = false;
    }

    void CreateLightBolt()
    {
        line.enabled = true;

        var start = myTransform.position;

        start.x += Random.Range(RandomStartMin.x, RandomStartMax.x);
        start.y += Random.Range(RandomStartMin.y, RandomStartMax.y);
        line.SetPosition(0, start);

        Vector2 ends = Vector2.zero;

        for (var i = 1; i < (int)NumberOfBends; i++)
        {
            var pos = Vector3.Lerp(myTransform.localPosition, Target.transform.localPosition, i / NumberOfBends);

            pos.x += Random.Range(RandomMiddleMin.x, RandomMiddleMax.x);
            pos.y += Random.Range(RandomMiddleMin.y, RandomMiddleMax.y);

            line.SetPosition(i, pos);

            if (i == (int)NumberOfBends - 1)
                ends = pos;
        }

        var end = finalPos.localPosition;

        ends.x += Random.Range(RandomEndMin.x, RandomEndMax.x);
        ends.y += Random.Range(RandomEndMin.y, RandomEndMax.y);

        line.SetPosition((int)NumberOfBends - 1, ends);
        shakeTimer = ShakeDuration;

        if (DirectionalLight != null)
            DirectionalLight.enabled = true;
    }
    
    void ShakeCamera()
    {
        if (shakeTimer > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * CameraShakeAmount;

            shakeTimer -= Time.deltaTime * CameraShakeDecreaseFactor;
        }
        else
        {
            shakeTimer = 0f;
            camTransform.localPosition = originalPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //line.SetVertexCount((int)NumberOfBends);

        if (Input.GetMouseButtonDown(1)) // right mouse = toggle
            StartCoroutine(SpawnSingleLightBolt(Random.Range(0.1f, 0.4f)));
        else if (Input.GetMouseButton(0)) // left mouse = hold down
            CreateLightBolt();
        else if (Input.GetMouseButtonUp(0))
        {
            line.enabled = false;
            if (DirectionalLight != null)
                DirectionalLight.enabled = false;
        }

        if (UseCameraShake)
            ShakeCamera();
    }
}
