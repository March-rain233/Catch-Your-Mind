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

    [MenuItem("浅仓雨の工具/行为树编辑器")]
    public static void ShowExample()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("行为树编辑器");
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
        root.Q<ToolbarButton>("load").clicked += LoadAsset;
        root.Q<ToolbarButton>("loadNode").clicked += LoadNode;
        root.Q<ToolbarButton>("sort1").clicked += SortNode_1;
        root.Q<ToolbarButton>("sort2").clicked += SortNode_2;
        root.Q<ToolbarButton>("guid").clicked += ChangeGuid;
        root.Q<ScrollView>().Add(_inspectorView);
        _treeView.OnElementSelected = OnSelectionChanged;
        OnSelectionChange();
    }

    private void ChangeGuid()
    {
        _treeView.ChangeGuid();
    }

    private void SortNode_2()
    {
        _treeView.Sort_2();
    }

    private void SortNode_1()
    {
        _treeView.Sort_1();
    }

    private void LoadNode()
    {
        string path = EditorUtility.OpenFilePanel("选择节点", Application.dataPath, "asset");
        path = path.Replace(Application.dataPath, "Assets");
        NPC.Node node = AssetDatabase.LoadAssetAtPath<NPC.Node>(path);
        if (node == null)
        {
            Debug.Log("文件违规");
            return;
        }
        _treeView.AddSubtree(node.Clone());
    }

    private void LoadAsset()
    {
        string path = EditorUtility.OpenFilePanel("选择行为树", Application.dataPath, "asset");
        path = path.Replace(Application.dataPath, "Assets");
        ITree tree = AssetDatabase.LoadAssetAtPath<NPC.BehaviorTree>(path);
        if(tree == null)
        {
            Debug.Log("文件违规");
            return;
        }
        LoadTree(tree);
    }

    private void LoadTree(ITree tree)
    {
        //if (!AssetDatabase.IsMainAsset(tree as UnityEngine.Object)) { return; }
        _label.text = (tree as Object).name + " View";
        _treeView.PopulateView(tree);
        _inspectorView.Clear();
    }

    private void OnSelectionChange()
    {
        ITree tree = Selection.activeObject as ITree;
        if(tree == null && Selection.activeObject is GameObject)
        {
            tree = Selection.activeGameObject.GetComponent<NPC.BehaviorTreeRunner>()?.BehaviorTree as NPC.BehaviorTree;
            if(tree == null)
            {
                tree = (Selection.activeGameObject.GetComponent<NPC.BehaviorTreeRunner>()?.BehaviorTree as NPC.BehaviorTreeOverride)?.Prototype;
            }
        }
        if (tree == null && Selection.activeObject is GameObject)
        {
            tree = Selection.activeGameObject.GetComponent<TalkSystem>()?.DialogueTree;
        }

        if (tree != null)
        {
            LoadTree(tree);
        }
    }

    private void OnSelectionChanged(GraphElement element)
    {
        _inspectorView.UpdateSelection(element);
    }

}