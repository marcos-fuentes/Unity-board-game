using UnityEngine;

namespace UI
{
    public class FPSCounter : MonoBehaviour
    {
    
        /* Assign this script to any object in the Scene to display frames per second */

        public float updateInterval = 0.5f; //How often should the number update

        float accum = 0.0f;
        int frames = 0;
        float timeleft;
        float fps;

        GUIStyle textStyle = new GUIStyle();
        GUIStyle systemInfoStyle = new GUIStyle();

        public bool isRunning = false;

        // Use this for initialization
        void Start()
        {
            timeleft = updateInterval;
            textStyle.fontStyle = FontStyle.Bold;
            textStyle.normal.textColor = Color.yellow;
            textStyle.fontSize = 30;

            systemInfoStyle.fontStyle = FontStyle.Bold;
            systemInfoStyle.normal.textColor = Color.white;
            systemInfoStyle.fontSize = 20;
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

            #if UNITY_IOS
            GUI.Label(new Rect(5, 35, 100, 15), "Iphone Gen: " + UnityEngine.iOS.Device.generation , systemInfoStyle);
            #endif

            #if UNITY_ANDROID
            GUI.Label(new Rect(5, 35, 100, 15), "Model: " + SystemInfo.deviceModel , systemInfoStyle);
            #endif
            GUI.Label(new Rect(5, 50, 100, 15), "OS Version: " + SystemInfo.operatingSystem , systemInfoStyle);
            GUI.Label(new Rect(5, 65, 100, 15), "RAM: " + SystemInfo.systemMemorySize + "MB", systemInfoStyle);
        }
    }
}