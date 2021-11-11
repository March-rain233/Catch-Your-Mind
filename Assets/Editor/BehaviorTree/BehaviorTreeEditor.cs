using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class BehaviorTreeEditor : EditorWindow
{
    private TreeView _treeView;
    private InspectorView _inspectorView;

    [MenuItem("Ç³²ÖÓê¤Î¹¤¾ß/ÐÐÎªÊ÷±à¼­Æ÷")]
    public static void ShowExample()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("ÐÐÎªÊ÷±à¼­Æ÷");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviorTree/BehaviorTreeEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviorTree/BehaviorTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        _treeView = root.Q<TreeView>();
        _inspectorView = root.Q<InspectorView>();
        //_treeView.OnNodeSelected = OnSelectionChanged;
        //OnSelectionChange();
    }

    //private void OnSelectionChange()
    //{
    //    StateMachineController controller = Selection.activeObject as StateMachineController;
    //    if (controller)
    //    {
    //        _treeView.PopulateView(controller);
    //        _inspectorView.Clear();
    //    }
    //}

    //private void OnSelectionChanged(GraphElement element)
    //{
    //    _inspectorView.UpdateSelection(element);
    //}
}