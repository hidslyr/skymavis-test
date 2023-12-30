using UnityEngine;

namespace TurnBaseGame
{
    public struct TouchInfo
    {
        public Vector2 deltaPosition;
        public float deltaTime;
        public int fingerId;
        public TouchPhase phase;
        public Vector2 position;
        public Vector2 rawPosition;
        public int tapCount;
        public bool isDragging;
        public bool isHiding;
    }

    public class InputManager : MonoBehaviour
    {
        public const float DRAG_THRESHOLD = 0.001f;

        public readonly TouchInfo INVALID_TOUCH = new TouchInfo() { fingerId = -1 };

        /// <summary>
        /// Delegate for the OnFingerDown event
        /// </summary>
        /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
        /// <param name="fingerPos">Current position of the finger on the screen</param>
        public delegate void FingerDownEventHandler(int fingerIndex, Vector2 fingerPos);

        /// <summary>
        /// Delegate for the OnFingerUp event
        /// </summary>
        /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
        /// <param name="fingerPos">Current position of the finger on the screen</param>
        public delegate void FingerUpEventHandler(int fingerIndex, Vector2 fingerPos);

        /// <summary>
        /// Delegate for the OnFingernDragBegin event
        /// </summary>
        /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
        /// <param name="fingerPos">Current position of the finger on the screen</param>
        /// <param name="startPos">The initial finger position on the screen.</param>
        /// <remark>Since the finger has to move beyond a certain treshold distance (specified by the moveThreshold property) 
        /// before the gesture registers as a drag motion, fingerPos and startPos are likely to be different if you specified a non-zero moveThreshold.</remark>
        public delegate void FingerDragBeginEventHandler(int fingerIndex, Vector2 fingerPos, Vector2 startPos);

        /// <summary>
        /// Delegate for the OnFingernDragMove event
        /// </summary>
        /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
        /// <param name="fingerPos">Current position of the finger on the screen</param>
        /// <param name="delta">How much the finger has moved since the last update. This is the difference between the previous finger position and the new one.</param>
        public delegate void FingerDragMoveEventHandler(int fingerIndex, Vector2 fingerPos, Vector2 delta);

        /// <summary>
        /// Delegate for the OnFingernDragMove event
        /// </summary>
        /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
        public delegate void FingerDragStandEventHandler(int fingerIndex);

        /// <summary>
        /// Delegate for the OnFingernDragEnd event
        /// </summary>
        /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
        /// <param name="fingerPos">Current position of the finger on the screen</param>
        public delegate void FingerDragEndEventHandler(int fingerIndex, Vector2 fingerPos);

        public delegate void FingerTabEventHandler(int fingerIndex, Vector2 fingerPos);

        public delegate void MouseScrollWheelEventHandler(float delta);

        private TouchInfo[] _touches;
        private int _touchCount;

        public TouchInfo[] touches
        {
            get
            {
                return _touches;
            }
        }

        public int touchCount
        {
            get
            {
                return _touchCount;
            }
        }

#region Events
        /// <summary>
        /// Event fired when a finger's OnDown event fires
        /// </summary>
        public event FingerDownEventHandler OnFingerDown;

        /// <summary>
        /// Event fired when a finger's OnUp event fires
        /// </summary>
        public event FingerUpEventHandler OnFingerUp;


        /// <summary>
        /// Event fired when a finger's drag gesture recognizer OnDragBegin event fires
        /// </summary>
        public event FingerDragBeginEventHandler OnFingerDragBegin;

        /// <summary>
        /// Event fired when a finger's drag gesture recognizer OnDragMove event fires
        /// </summary>
        public event FingerDragMoveEventHandler OnFingerDragMove;

        /// <summary>
        /// Event fired when a finger's drag gesture recognizer OnDragMove event fires
        /// </summary>
        public event FingerDragStandEventHandler OnFingerDragStand;

        /// <summary>
        /// </summary>
        public event FingerDragEndEventHandler OnFingerDragEnd;

        /// <summary>
        /// </summary>
        public event FingerTabEventHandler OnFingerTap;

        /// <summary>
        /// </summary>
        public event MouseScrollWheelEventHandler OnMouseScrollWheelMoved;
        #endregion

        void Start () 
        {
            Init();
        }


        public void Init()
        {
            //Debug.Log("Init input manager");
            _touchCount = 0;

#if UNITY_EDITOR || UNITY_STANDALONE
            _touches = new TouchInfo[1];
#else
            _touches = new TouchInfo[Input.touches.Length];
#endif
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("Input frame" + GSystem.instance.GetCurrentFrame());
#if !DISABLE_TOUCH_SIMULATION && (UNITY_EDITOR || UNITY_STANDALONE)
            if (Input.GetMouseButton(0))
            {
                if (_touchCount <= 0)
                {
                    // Start a new touch
                    //Debug.Log("Start a touch");
                    _touches[0].position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    _touches[0].phase = TouchPhase.Began;
                    _touches[0].isDragging = false;
                    _touches[0].isHiding = false;

                    //Debug.Log("is hiding = " + _touches[0].isHiding);
                    _touchCount = 1;
                    if (!_touches[0].isHiding && OnFingerDown != null)
                        OnFingerDown(0, _touches[0].position);
                }
                else // touchCount > 0
                {
                    //Debug.Log("Move a touch");
                    Vector2 newPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    if (Vector2.Distance(touches[0].position, newPos) > DRAG_THRESHOLD)
                    {
                        if (!_touches[0].isHiding && !touches[0].isDragging && OnFingerDragBegin != null)
                        {
                            OnFingerDragBegin(0, newPos, _touches[0].position);
                        }
                        else if (!_touches[0].isHiding && OnFingerDragMove != null)
                        {
                            OnFingerDragMove(0, newPos, newPos - touches[0].position);
                        }

                        touches[0].phase = TouchPhase.Moved;
                        touches[0].isDragging = true;
                        touches[0].position = newPos;
                    }
                    else
                    {
                        if (!_touches[0].isHiding && touches[0].isDragging && OnFingerDragStand != null)
                        {
                            OnFingerDragStand(0);
                        }
                        touches[0].phase = TouchPhase.Stationary;
                    }
                }
            }
            else // left mouse released
            {
                if (_touchCount > 0)
                {
                    //Debug.Log("End a touch");
                    _touchCount = 0;
                    _touches[0].phase = TouchPhase.Ended;
                    if (!_touches[0].isHiding && _touches[0].isDragging)
                    {
                        if (OnFingerDragEnd != null) OnFingerDragEnd(0, touches[0].position);
                        _touches[0].isDragging = false;
                    }
                    else if (!_touches[0].isHiding)
                    {
                        if (OnFingerTap != null) OnFingerTap(0, touches[0].position);
                    }

                    if (!_touches[0].isHiding && OnFingerUp != null)
                        OnFingerUp(0, touches[0].position);
                }
            }

            float scrollAxis = Input.GetAxis("Mouse ScrollWheel");

            if (scrollAxis != 0)
            {
                OnMouseScrollWheelMoved(scrollAxis);
            }
#else
        _touchCount = Input.touchCount;
        TouchInfo[] newTouches = new TouchInfo[_touchCount];
        //Touch[] realTouches = Input.touches;
        for (int i = 0; i < _touchCount; i++)
        {
            Touch realTouch = Input.GetTouch(i);
            newTouches[i] = GetTouchByFinger(realTouch.fingerId);

            switch (realTouch.phase)
            {
                case TouchPhase.Began:
                    //Debug.Log("Touch(" + i + ") began at " + realTouch.position);
                    newTouches[i].isDragging = false;
                    newTouches[i].isHiding = false;
                    if (!newTouches[i].isHiding && OnFingerDown != null)
                        OnFingerDown(realTouch.fingerId, realTouch.position);
                    break;

                case TouchPhase.Moved:
                    //Debug.Log("Touch(" + i + ") moved at " + realTouch.position);
                    if (!newTouches[i].isDragging)
                    {
                        if (!newTouches[i].isHiding && OnFingerDragBegin != null)
                            OnFingerDragBegin(realTouch.fingerId, realTouch.position, newTouches[i].position);
                    }
                    else
                    {
                        if (OnFingerDragMove != null)
                            OnFingerDragMove(realTouch.fingerId, realTouch.position, realTouch.deltaPosition);
                    }
                    newTouches[i].isDragging = true;            
                    break;

                case TouchPhase.Stationary:
                    if (!newTouches[i].isHiding && newTouches[i].isDragging && OnFingerDragStand != null)
                    {
                        OnFingerDragStand(realTouch.fingerId);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    //Debug.Log("Touch(" + i + ") ended at " + realTouch.position);
                    if (!newTouches[i].isHiding)
                    {
                        if (!newTouches[i].isDragging &&
                            Vector3.Distance(newTouches[i].position, realTouch.position) >= DRAG_THRESHOLD)
                        {
                            if (OnFingerDragBegin != null)
                                OnFingerDragBegin(realTouch.fingerId, realTouch.position, newTouches[i].position);
                            if (OnFingerDragEnd != null)
                                OnFingerDragEnd(realTouch.fingerId, realTouch.position);
                        }
                        else if (newTouches[i].isDragging)
                        {
                            if (OnFingerDragEnd != null)
                                OnFingerDragEnd(realTouch.fingerId, realTouch.position);
                        }
                        else
                        {
                            if (OnFingerTap != null)
                                OnFingerTap(realTouch.fingerId, realTouch.position);
                        }

                        if (OnFingerUp != null)
                            OnFingerUp(realTouch.fingerId, realTouch.position);
                    }

                    newTouches[i].isDragging = false;
                    newTouches[i].isHiding = false;
                    break;
            }

            newTouches[i].deltaPosition = realTouch.deltaPosition;
            newTouches[i].deltaTime = realTouch.deltaTime;
            newTouches[i].fingerId = realTouch.fingerId;
            newTouches[i].phase = realTouch.phase;
            newTouches[i].position = realTouch.position;
            newTouches[i].rawPosition = realTouch.rawPosition;
            newTouches[i].tapCount = realTouch.tapCount;
        }
        _touches = newTouches;
#endif
        }


        public TouchInfo GetTouchByFinger(int fingerId)
        {
            foreach (TouchInfo touch in touches)
            {
                if (touch.fingerId == fingerId)
                    return touch;
            }
            return INVALID_TOUCH;
        }

        public TouchInfo GetTouch(int index)
        {
            return _touches[index];
        }

        public Vector2 GetCursorPos(int fingerIndex, Vector2 defaultValue)
        {
#if !DISABLE_TOUCH_SIMULATION && (UNITY_EDITOR || UNITY_STANDALONE)
            return new Vector2(Input.mousePosition.x, Input.mousePosition.y);
#else
            TouchInfo touch = GetTouchByFinger(fingerIndex);

            if (touch.fingerId == INVALID_TOUCH.fingerId)
                return defaultValue;

            return touch.position;
#endif
        }

        public Vector3 GetRayCastPosition(Vector2 inputPosition, Camera cam, Collider coll)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(inputPosition);
            if (coll.Raycast(ray, out hit, 1000))
            {
                return hit.point;
            }
            return new Vector3(float.MinValue, float.MinValue, float.MinValue);
        }

        public bool RayCastOnCollider(Vector2 inputPosition, Camera cam, Collider coll)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(inputPosition);
            if (coll.Raycast(ray, out hit, 1000))
            {
                return true;
            }
            return false;
        }
    }
}