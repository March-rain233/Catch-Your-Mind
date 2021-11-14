using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

public class BehaviorTreeEditor : EditorWindow
{
    private TreeView _treeView;
    private InspectorView _inspectorView;
    private Label _label;

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
        _label = root.Q<Label>("name");
        _treeView.OnElementSelected = OnSelectionChanged;
        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        ITree tree = Selection.activeObject as ITree;
        if(tree == null && Selection.activeObject is GameObject)
        {
            tree = Selection.activeGameObject.GetComponent<NPC.BehaviorTreeRunner>()?.BehaviorTree;
        }

        if (tree != null)
        {
            //if (!AssetDatabase.IsMainAsset(tree as UnityEngine.Object)) { return; }
            _label.text = Selection.activeObject .name + " View";
            _treeView.PopulateView(tree);
            _inspectorView.Clear();
        }
    }

    private void OnSelectionChanged(GraphElement element)
    {
        _inspectorView.UpdateSelection(element);
    }

}