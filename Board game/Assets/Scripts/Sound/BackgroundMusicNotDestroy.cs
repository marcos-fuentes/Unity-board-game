using UnityEngine;

namespace Sound
{
    public class BackgroundMusicNotDestroy : MonoBehaviour
    {
        private void Awake()
        {
            GameObject[] musicObj = GameObject.FindGameObjectsWithTag("GameMusic");

            if (musicObj.Length > 1)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
    }
}