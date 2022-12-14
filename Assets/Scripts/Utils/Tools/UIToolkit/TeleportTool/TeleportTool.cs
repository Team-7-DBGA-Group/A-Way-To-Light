#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class TeleportTool : EditorWindow
{
    private GameObject _player;
    private PlayerMovement _playerMovement;
    private CharacterController _characterController;
    private bool _canTeleport = false;

    [ExecuteInEditMode]
    public static void ShowEditor()
    {
        TeleportTool wnd = GetWindow<TeleportTool>();
        wnd.titleContent = new GUIContent("TeleportTool");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Utils/Tools/UIToolkit/TeleportTool/TeleportTool.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        SetupButtonsHandler();
    }

    private void OnEnable()
    {
        if (!Application.isEditor)
        {
            Destroy(this);
        }
        SceneView.duringSceneGui += OnScene;
    }

    private void ClickedTeleport(ClickEvent click)
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = _player.GetComponent<PlayerMovement>();
        _characterController = _player.GetComponent<CharacterController>();
        _characterController.enabled = false;
        _playerMovement.enabled = false;
        if (_player)
            _canTeleport = true;
    }

    private void SetupButtonsHandler()
    {
        Button teleportButton = rootVisualElement.Q<Button>("tpButton");
        teleportButton.RegisterCallback<ClickEvent>(ClickedTeleport);
    }
    
    void OnScene(SceneView scene)
    {
        if (!_canTeleport)
            return;

        scene.Focus();

        Event e = Event.current;

        Vector3 mousePos = e.mousePosition;
        float ppp = EditorGUIUtility.pixelsPerPoint;
        mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
        mousePos.x *= ppp;

        Ray ray = scene.camera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        Handles.color = Color.red;
        if (Physics.Raycast(ray, out hit))
            Handles.DrawWireDisc(hit.point, hit.normal, 1f);

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            if (Physics.Raycast(ray, out hit))
            {
                _player.transform.position = hit.point;
                _playerMovement.enabled = true;
                _characterController.enabled = true;
            }
                
            e.Use();
            _canTeleport = false;
        }

        if (e.type == EventType.MouseDown && e.button == 1)
        {
            _canTeleport = false;
            _playerMovement.enabled = true;
            _characterController.enabled = true;
        }
            
    }
}
#endif