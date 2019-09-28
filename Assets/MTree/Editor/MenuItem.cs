using UnityEngine;
using UnityEditor;

public class MenuItems
{
    [MenuItem("GameObject/Mtree/Create Tree")]
    private static void NewMenuOption()
    {
        GameObject tree = new GameObject("tree");
        MtreeComponent mtree = tree.AddComponent<MtreeComponent>();
        mtree.GenerateTree();
        Selection.activeGameObject = tree;

    }
}
