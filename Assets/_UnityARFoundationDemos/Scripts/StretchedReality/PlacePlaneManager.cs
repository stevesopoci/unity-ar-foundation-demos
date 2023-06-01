using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace StretchedReality
{
    [RequireComponent(typeof(ARRaycastManager))]
    [RequireComponent(typeof(ARPlaneManager))]
    public class PlacePlaneManager : MonoBehaviour
    {
        private List<ARRaycastHit> arHits = new List<ARRaycastHit>();

        private ARRaycastManager arRaycastManager;

        private PlaneAlignment previousAlignment;

        [SerializeField] private GameObject arCamera;
        [SerializeField] private GameObject spawningARObject;
        private GameObject spawnedARObject;
        
        private bool isARObjectPlaced = false;
        private bool startTouchTimer = false;

        private float touchTimer = 0.0f;

        private void Start()
        {
            arRaycastManager = GetComponent<ARRaycastManager>();
        }

        private void Update()
        {
            WaitToPlaceSpawningARObject();
            PlaceSpawningARObject();
        }

        private void WaitToPlaceSpawningARObject()
        {
            Vector3 rayEmitPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);

            if (arRaycastManager.Raycast(rayEmitPosition, arHits, TrackableType.PlaneWithinPolygon))
            {
                if (spawnedARObject == null)
                {
                    Destroy(spawnedARObject);

                    spawnedARObject = Instantiate(spawningARObject);

                    isARObjectPlaced = false;
                }
                else
                {
                    PlaneAlignmentHandler(arHits);
                }
            }
        }

        private void PlaceSpawningARObject()
        {
            if (!isARObjectPlaced && spawnedARObject != null && Input.touchCount > 0)
            {
                if (startTouchTimer)
                {
                    touchTimer += Time.deltaTime;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    startTouchTimer = true;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Began && Input.touchCount > 1)
                {
                    startTouchTimer = false;

                    touchTimer = 0.3f;
                }

                if (Input.GetTouch(0).phase == TouchPhase.Ended && touchTimer < 0.3f)
                {
                    startTouchTimer = false;

                    touchTimer = 0.0f;

                    Vector3 rayEmitPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);

                    if (arRaycastManager.Raycast(rayEmitPosition, arHits, TrackableType.PlaneWithinPolygon))
                    {
                        PlaneAlignmentHandler(arHits);

                        spawnedARObject = null;

                        isARObjectPlaced = true;
                    }
                }
            }
            else
            {
                touchTimer = 0.0f;
            }
        }

        private void PlaneAlignmentHandler(List<ARRaycastHit> arHits)
        {
            int lastIndex = arHits.Count - 1;
            var hitPose = arHits[lastIndex].pose;
            
            TrackableId lastPlaneID = arHits[lastIndex].trackableId;
            ARPlane lastARPlane = GetComponent<ARPlaneManager>().GetPlane(lastPlaneID);

            foreach (ARRaycastHit arHit in arHits)
            {
                if (arHit.trackableId != lastPlaneID)
                {
                    TrackableId planeID = arHit.trackableId;

                    ARPlane arPlane = GetComponent<ARPlaneManager>().GetPlane(planeID);
                    arPlane.gameObject.SetActive(false);
                }
            }

            if (lastARPlane.alignment.ToString().Equals("HorizontalUp") || lastARPlane.alignment.ToString().Equals("HorizontalDown"))
            {
                spawnedARObject.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                spawnedARObject.transform.rotation = Quaternion.Euler(0, arCamera.transform.eulerAngles.y, 0);
            }
            else if (lastARPlane.alignment.ToString().Equals("Vertical"))
            {
                spawnedARObject.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
            }
        }
    }
}