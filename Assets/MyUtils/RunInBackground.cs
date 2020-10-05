#if UNITY_EDITOR
using UnityEngine;
using Utils.CustomAttributes.ReadOnlyWhenPlaying;

namespace Utils
{
    public class RunInBackground : MonoBehaviour
    {
        [SerializeField]
        [ReadOnlyWhenPlaying]
        private bool runInBackground = true;

        private void Start() =>
            Application.runInBackground = runInBackground;
        
        private void OnValidate() =>
            Application.runInBackground = runInBackground;
        
    }
}
#endif