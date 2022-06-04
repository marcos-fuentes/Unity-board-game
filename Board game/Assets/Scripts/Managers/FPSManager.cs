using UnityEngine;

public class FPSManager : MonoBehaviour
{
    
    public static FPSManager Instance;
    
    private void Awake() => Instance = this;
    
    /* Assign this script to any object in the Scene to display frames per second */

    public float updateInterval = 0.5f; //How often should the number update

    float accum = 0.0f;
    int frames = 0;
    float timeleft;
    float fps;

    GUIStyle textStyle = new GUIStyle();

    public bool isRunning = false;

    // Use this for initialization
    void Start()
    {
        timeleft = updateInterval;
        textStyle.fontStyle = FontStyle.Bold;
        textStyle.normal.textColor = Color.yellow;
        textStyle.fontSize = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning) 
            
            return;
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            fps = (accum / frames);
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }

    void OnGUI()
    {
        if (!isRunning) return;
            //Display the fps and round to 2 decimals
        GUI.Label(new Rect(5, 5, 100, 25), fps.ToString("F2") + "FPS", textStyle);
    }
}