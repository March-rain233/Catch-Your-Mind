using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using NPC;
using UnityEditor.Experimental.GraphView;

public class BehaviorStateMachineEditor : EditorWindow
{
    private BehaviorStateMachineView _stateMachineView;
    private InspectorView _inspectorView;

    [MenuItem("Ç³²ÖÓê¤Î¹¤¾ß/ÐÐÎª×´Ì¬»ú±à¼­Æ÷")]
    public static void ShowWindow()
    {
        BehaviorStateMachineEditor wnd = GetWindow<BehaviorStateMachineEditor>();
        wnd.titleContent = new GUIContent("ÐÐÎª×´Ì¬»ú±à¼­Æ÷");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviorStateMachineEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviorStateMachineEditor.uss");
        root.styleSheets.Add(styleSheet);

        _stateMachineView = root.Q<BehaviorStateMachineView>();
        _inspectorView = root.Q<InspectorView>();
        _stateMachineView.OnNodeSelected = OnSelectionChanged;
        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        StateMachineController controller = Selection.activeObject as StateMachineController;
        if (controller)
        {
            _stateMachineView.PopulateView(controller);
            _inspectorView.Clear();
        }
    }

    private void OnSelectionChanged(GraphElement element)
    {
        _inspectorView.UpdateSelection(element);
    }
}