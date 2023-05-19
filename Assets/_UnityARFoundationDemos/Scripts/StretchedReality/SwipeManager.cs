using System;
using UnityEngine;
using UnityEngine.Events;

namespace StretchedReality
{
    public class SwipeManager : MonoBehaviour
    {
        public float SwipeTreshold = 50f;
        public float TimeThreshold = 0.3f;

        public UnityEvent OnSwipeLeft;
        public UnityEvent OnSwipeRight;
        public UnityEvent OnSwipeUp;
        public UnityEvent OnSwipeDown;

        private Vector2 fingerDown;
        private DateTime fingerDownTime;
        private Vector2 fingerUp;
        private DateTime fingerUpTime;

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                fingerDown = Input.mousePosition;
                fingerUp = Input.mousePosition;
                fingerDownTime = DateTime.Now;
            }

            if (Input.GetMouseButtonUp(0))
            {
                fingerDown = Input.mousePosition;
                fingerUpTime = DateTime.Now;

                CheckSwipe();
            }
#endif

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerDown = touch.position;
                    fingerUp = touch.position;
                    fingerDownTime = DateTime.Now;
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    fingerDown = touch.position;
                    fingerUpTime = DateTime.Now;

                    CheckSwipe();
                }
            }
        }

        private void CheckSwipe()
        {
            float duration = (float)fingerUpTime.Subtract(fingerDownTime).TotalSeconds;
            if (duration > TimeThreshold) return;

            float deltaX = fingerDown.x - fingerUp.x;
            float deltaY = fingerDown.y - fingerUp.y;

            if (Mathf.Abs(deltaX) > SwipeTreshold && Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
            {
                if (deltaX > 0)
                {
                    OnSwipeRight.Invoke();

                    // Do something...
                }
                else if (deltaX < 0)
                {
                    OnSwipeLeft.Invoke();

                    // Do something...
                }
            }
            else if (Mathf.Abs(deltaY) > SwipeTreshold && Mathf.Abs(deltaY) > Mathf.Abs(deltaX))
            {
                if (deltaY > 0)
                {
                    OnSwipeUp.Invoke();

                    TextureManager.Instance.TextureIndex++;
                }
                else if (deltaY < 0)
                {
                    OnSwipeDown.Invoke();

                    TextureManager.Instance.TextureIndex--;
                }
            }

            fingerUp = fingerDown;
        }
    }
}