#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class MultipleObjectPlacing : EditorWindow
{
    private GameObject _prefab;
    private float _distanceBetweenObjects;
    private float _scale;

    private bool _canPlaceObjects = false;
    private bool _isFirstClick = true;
    private bool _directionLocked = false;

    private Vector3 _firstPosition;
    private Vector3 _secondPosition;
    private Vector3 _meshSize;

    private Color _handlesColor;

    public static void ShowEditor()
    {
        MultipleObjectPlacing wnd = GetWindow<MultipleObjectPlacing>();
        wnd.titleContent = new GUIContent("MultipleObjectPlacing");
    }
    private void OnEnable()
    {
        if (!Application.isEditor)
        {
            Destroy(this);
        }
        SceneView.duringSceneGui += OnScene;
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Utils/Tools/UIToolkit/MultipleObjectPlacingTool/MultipleObjectPlacing.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        if (!_directionLocked)
            _handlesColor = Color.blue;
        else
            _handlesColor = Color.cyan;

        SetupButtonsHandler();
    }

    private void ClickedPlace(ClickEvent click)
    { 
        //_prefab = rootVisualElement.Q<ObjectField>("GameObjectField").;
        ObjectField objField = rootVisualElement.Q<ObjectField>("gameObjectField");
        _prefab = objField.value as GameObject;

        _meshSize = _prefab.GetComponentInChildren<MeshRenderer>().bounds.size;

        FloatField scaleField = rootVisualElement.Q<FloatField>("scaleField");
        _scale = scaleField.value;
        if (_scale < 1)
        {
            _scale = 1;
            scaleField.SetValueWithoutNotify(_scale);
        }

        _meshSize *= _scale;

        if (_prefab)
        {
            _isFirstClick = true;
            _canPlaceObjects = true;
        }
    }

    private void OnScene(SceneView scene)
    {
        if (!_canPlaceObjects)
            return;

        Event e = Event.current;

        // Focus the mouse on scene
        scene.Focus();

        // Get mouse pos
        Vector3 mousePos = e.mousePosition;
        float ppp = EditorGUIUtility.pixelsPerPoint;
        mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
        mousePos.x *= ppp;

        // Check where mouse raycast is colliding and draw circle
        Ray ray = scene.camera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftShift)
        {
            _directionLocked = true;
            SceneView.focusedWindow.ShowNotification(new GUIContent("Direction Locked"),2);
            _handlesColor = Color.cyan;
        }

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftControl)
        {
            _directionLocked = false;
            SceneView.focusedWindow.ShowNotification(new GUIContent("Direction Unlocked"),2);
            _handlesColor = Color.blue;
        }

        if (_isFirstClick)
            ShowIndicatorOnScene(ray, out hit, _handlesColor);
        else
        {
            ShowIndicatorOnScene(ray, out hit, _handlesColor);
            Handles.color = Color.green;
            Handles.DrawLine(_firstPosition, hit.point);
            Handles.color = _handlesColor;
        }

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            if (!_isFirstClick)
            {
                _secondPosition = hit.point;
                float distance = Vector3.Distance(_firstPosition, _secondPosition);

                Vector3 direction = (_secondPosition - _firstPosition).normalized;
                Vector3 directionNN = _secondPosition - _firstPosition;
                if (_directionLocked)
                {
                    direction.x = Mathf.RoundToInt(direction.x);
                    direction.y = Mathf.RoundToInt(direction.y);
                    direction.z = Mathf.RoundToInt(direction.z);
                }

                // Calculate distance based on direction
                float distanceBetweenObjects = _meshSize.x;
                if(_directionLocked && direction != Vector3.forward && direction != Vector3.right && direction != Vector3.left && direction != Vector3.back)
                {
                    // Diagonal
                   distanceBetweenObjects = (Mathf.Sqrt(Mathf.Pow(_meshSize.x, 2) * 2))/ 2;
                }
                _distanceBetweenObjects = distanceBetweenObjects;

                while (distanceBetweenObjects < distance)
                {
                    Vector3 pos = _firstPosition + direction * distanceBetweenObjects;
                    Vector3 lookPos = Vector3.zero;

                    if (!_directionLocked)
                        lookPos = _secondPosition - _firstPosition;
                    else
                        lookPos = direction;

                    lookPos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(lookPos);
                    GameObject obj = Instantiate(_prefab, pos, rotation * Quaternion.Euler(0f, -90f, 0f));
                    obj.transform.localScale = new Vector3(_scale,_scale, _scale);
                    distanceBetweenObjects += _distanceBetweenObjects;
                }
                _firstPosition = _secondPosition;
                return;
            }
            _firstPosition = hit.point;
            _isFirstClick = false;
        }

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape)
            _canPlaceObjects = false;
    }

    private void SetupButtonsHandler()
    {
        Button teleportButton = rootVisualElement.Q<Button>("btnPlace");
        teleportButton.RegisterCallback<ClickEvent>(ClickedPlace);
    }

    void ShowIndicatorOnScene(Ray ray, out RaycastHit hit, Color color)
    {
        Handles.color = color;
        if (Physics.Raycast(ray, out hit))
            Handles.DrawWireDisc(hit.point, hit.normal, 1f);
    }
}
#endif