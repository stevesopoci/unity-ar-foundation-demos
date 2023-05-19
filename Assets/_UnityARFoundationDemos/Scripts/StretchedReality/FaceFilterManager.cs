using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace StretchedReality
{
    public class FaceFilterManager : MonoSingleton<FaceFilterManager>
    {
        [SerializeField]
        private ARFaceManager arFaceManager;

        [HideInInspector]
        public Material FaceFilterMat;

        [SerializeField] private float shift = 0f;
        [SerializeField] private float shiftTime = 10f;
        [SerializeField] private float shiftRange = 0.5f;

        private float timeSinceStart;

        protected override void Awake()
        {
            base.Awake();

            FaceFilterMat = arFaceManager.facePrefab.transform.GetComponent<Renderer>().sharedMaterial;

            timeSinceStart = (3f * shiftTime) / 4f;     
        }

        private void Update()
        {
            FaceFilterMat.SetTextureOffset("_FaceFilterTex", new Vector2(shiftRange * Mathf.Sin(Mathf.PI * 2f / shiftTime * timeSinceStart) + shift, 0f));

            timeSinceStart += Time.deltaTime;
        }
    }
}