using UnityEngine;
using UnityEditor;
using Mtree;
using System;

[CanEditMultipleObjects]
[CustomEditor(typeof(MtreeComponent))]
public class MtreeEditor: Editor
{
    MtreeComponent tree;
    private string[] lodOptions = { "0", "1", "2", "3" };
    private string[] tabNames = { "Functions", "Quality", "Save as prefab" };
    private int rectHeightMultiplier;
    private int tabIndex = 0; // Used to navigate beetween tabs 
    private bool UndoDirty = false;

    private void OnEnable()
    {
        tree = (MtreeComponent)target;
        tree.UpdateTreeFunctions();
        if (tree.tree == null)
        {
            UpdateTree();
        }
        rectHeightMultiplier = TreeFunction.height + TreeFunction.margin;
        Undo.undoRedoPerformed += UndoCallback;
    }

    public override void OnInspectorGUI()
    {
        if (UndoDirty)
        {
            UndoDirty = false;
            UpdateTree();
        }

        if (IsMultiSelection()) // Editor to display when multiple trees are selected
        {
            DisplayMultiObjectsEditting();
            return; // Not drawing the rest when multpile trees are selected
        }
        
        if (tree.MtreeVersion != Mtree.MtreeVariables.MtreeVersion)
        {
            EditorGUILayout.HelpBox("Warning, this tree was made with a previous version of Mtree, you need to upgrade the tree. Upgrading it may result in unwanted changes.", MessageType.Warning);
            if (GUILayout.Button("Upgrade tree"))
            {
                tree.MtreeVersion = MtreeVariables.MtreeVersion;
                foreach (TreeFunction tf in tree.treeFunctions)
                {
                    tf.RevertOutOfBoundValues();
                }
            }
            return;
        }


        DisplayLodSelection();

        tabIndex = GUILayout.SelectionGrid(tabIndex, tabNames, tabNames.Length);
        
        if (tabIndex == 1) // Quality tab
        {
            DisplayQualityTab();
        }

        else if (tabIndex == 2) // Save as prefab tab
        {
            DisplaySaveTab();
        }

        else
        {
            DisplayFunctionsTab();    
        }

        EditorGUILayout.LabelField("polycount: " + tree.polycount.ToString(), EditorStyles.boldLabel);
    }

    private void DisplayLodSelection()
    {
        EditorGUI.BeginChangeCheck();
        tree.Lod = EditorGUILayout.Popup("LOD", tree.Lod, lodOptions);
        if (EditorGUI.EndChangeCheck())
        {
            UpdateTree();
        }
    }

    private void DisplayQualityTab()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUI.BeginChangeCheck();
        int LOD = tree.Lod;
        tree.radialResolution[LOD] = EditorGUILayout.FloatField("Radial Resolution", tree.radialResolution[LOD]);
        tree.radialResolution[LOD]= Mathf.Max(0, tree.radialResolution[LOD]);
        tree.simplifyAngleThreshold[LOD] = EditorGUILayout.Slider("Simplify angle", tree.simplifyAngleThreshold[LOD], 0, 90);
        tree.simplifyRadiusThreshold[LOD] = EditorGUILayout.Slider("Simplify radius", tree.simplifyRadiusThreshold[LOD], 0, .9f);
        tree.simplifyLeafs[LOD] = EditorGUILayout.Slider("Simplify leafs", tree.simplifyLeafs[LOD], 0, .99f);
        if (EditorGUI.EndChangeCheck())
            UpdateTree();
        EditorGUILayout.EndVertical();
    }

    private void DisplaySaveTab()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        if (GUILayout.Button("Bake ambient occlusion"))
            tree.BakeAo();

        EditorGUILayout.BeginHorizontal();
        tree.saveTreeFolder = EditorGUILayout.TextField("Save Folder", tree.saveTreeFolder);
        if (GUILayout.Button("Find folder"))
        {
            string path = EditorUtility.OpenFolderPanel("save tree location", "Assets", "Assets");
            path = "Assets" + path.Substring(Application.dataPath.Length);
            tree.saveTreeFolder = path;
            AssetDatabase.Refresh();
        }

        EditorGUILayout.EndHorizontal();
        tree.gameObject.name = EditorGUILayout.TextField("name", tree.gameObject.name);

        if (GUILayout.Button("Save as Prefab"))
            tree.SaveAsPrefab();
        EditorGUILayout.EndVertical();
    }

    private void DisplayMultiObjectsEditting()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal();
        string saveTreeFolder = EditorGUILayout.TextField("Save Folder", tree.saveTreeFolder);
        if (GUILayout.Button("Find folder"))
        {
            string path = EditorUtility.OpenFolderPanel("save tree location", "Assets", "Assets");
            if (path.Length > 0)
            {
                path = "Assets" + path.Substring(Application.dataPath.Length);
                saveTreeFolder = path;
                foreach (MtreeComponent t in Array.ConvertAll(targets, item => (MtreeComponent)item))
                {
                    t.saveTreeFolder = saveTreeFolder;
                }
                AssetDatabase.Refresh();
            }            
        }
        EditorGUILayout.EndHorizontal();
        


        if (GUILayout.Button("Save all as Prefabs"))
        {
            saveTreeFolder = tree.saveTreeFolder;
            foreach (MtreeComponent t in Array.ConvertAll(targets, item => (MtreeComponent)item))
            {
                //t.saveTreeFolder = saveTreeFolder;
                t.SaveAsPrefab(groupedSave:true);
            }
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(saveTreeFolder, typeof(UnityEngine.Object));
            // Select the object in the project folder
            Selection.activeObject = obj;
            // Also flash the folder yellow to highlight it
            EditorGUIUtility.PingObject(obj);
        }
        EditorGUILayout.EndVertical();
    }

    private void DisplayFunctionsTab()
    {
        EditorGUIUtility.labelWidth = 135;
        int functionCount = tree.treeFunctions.Count; // Used in multiple for loops
        if (GUILayout.Button("Add function"))
        {
            GenericMenu addFunctionMenu = new GenericMenu();
            addFunctionMenu.AddItem(new GUIContent("Add branch"), false, tree.AddBranchFunction);
            addFunctionMenu.AddItem(new GUIContent("Add Leafs"), false, tree.AddLeafFunction);
            addFunctionMenu.AddItem(new GUIContent("Split"), false, tree.AddSplitFunction);
            addFunctionMenu.AddItem(new GUIContent("Grow"), false, tree.AddGrowFunction);
            addFunctionMenu.ShowAsContext();
        }


        int rectHeight = functionCount * rectHeightMultiplier; // get the height of the drawing window inside inspector
        Rect rect = GUILayoutUtility.GetRect(10, 1000, rectHeight, rectHeight); // Create drawing window
        Event e = Event.current; // Get current event

        if (e.type == EventType.MouseDown && e.button == 0) // If mouse button is pressed, get button pressed and act accordingly
        {
            for (int i = 0; i < functionCount; i++)
            {
                TreeFunction tf = tree.treeFunctions[i];
                if (tf.rect.Contains(e.mousePosition - rect.position))
                {
                    if (tf.type != FunctionType.Trunk && tf.deleteRect.Contains(e.mousePosition - rect.position))
                    {
                        tree.RemoveFunction(i);
                        UpdateTree();
                    }
                    else
                    {
                        Undo.RecordObject(target, "Selected function");
                        tree.selectedFunctionIndex = i;
                    }
                    GUIUtility.ExitGUI();
                    break;
                }
            }
        }

        GUI.BeginClip(rect); // Set origin of coordinates to origin of drawing window
        for (int i = 0; i < functionCount; i++) // Draw each node
        {
            TreeFunction tf = tree.treeFunctions[i];
            if (tf.type != FunctionType.Trunk && tf.parent == null)
                tree.UpdateTreeFunctions();
            tf.Draw(i == tree.selectedFunctionIndex);
        }
        GUI.EndClip();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        TreeFunction f = tree.treeFunctions[tree.selectedFunctionIndex];
        if (GUILayout.Button("Randomize tree"))
        {
            tree.RandomizeTree();
        }
        if (f.type == FunctionType.Trunk)
        {
            f.seed = EditorGUILayout.IntField("Seed", f.seed);
            f.Tlength = EditorGUILayout.FloatField("Length", f.Tlength);
            f.Tlength = Mathf.Max(0.01f, f.Tlength);
            f.TradiusMultiplier = EditorGUILayout.FloatField("Radius", f.TradiusMultiplier);
            f.Tresolution = EditorGUILayout.FloatField("Resolution", f.Tresolution);
            f.Tresolution = Mathf.Max(.01f, f.Tresolution);
            f.ToriginAttraction = EditorGUILayout.Slider("Axis attraction", f.ToriginAttraction, 0, 1);
            f.Tradius = EditorGUILayout.CurveField("Shape", f.Tradius);
            f.TradiusMultiplier = Mathf.Max(0.0001f, f.TradiusMultiplier);
            f.Trandomness = EditorGUILayout.Slider("Randomness", f.Trandomness, 0f, 0.5f);
            f.TdisplacementStrength = EditorGUILayout.FloatField("Displacement strength", f.TdisplacementStrength);
            f.TdisplacementSize = EditorGUILayout.FloatField("Displacement size", f.TdisplacementSize);
            f.TspinAmount = EditorGUILayout.FloatField("Spin amount", f.TspinAmount);
            f.TheightOffset = EditorGUILayout.FloatField("Height Offset", f.TheightOffset);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Root");
            f.TrootShape = EditorGUILayout.CurveField("Root shape", f.TrootShape);
            f.TrootHeight = EditorGUILayout.FloatField("Height", f.TrootHeight);
            f.TrootHeight = Mathf.Max(.01f, f.TrootHeight);
            f.TrootRadius = EditorGUILayout.Slider("Root Radius", f.TrootRadius, 0, 2);
            f.TrootResolution = EditorGUILayout.FloatField("Additional Resolution", f.TrootResolution);
            f.TrootResolution = Mathf.Max(1, f.TrootResolution);
            f.TflareNumber = EditorGUILayout.IntSlider("Flare Number", f.TflareNumber, 0, 10);
            EditorGUILayout.EndVertical();

        }

        else if (f.type == FunctionType.Grow)
        {
            f.seed = EditorGUILayout.IntField("Seed", f.seed);
            EditorGUILayout.BeginHorizontal();
            f.Glength = EditorGUILayout.FloatField("Length", f.Glength);
            f.GlengthCurve = EditorGUILayout.CurveField(f.GlengthCurve);
            EditorGUILayout.EndHorizontal();
            f.Glength = Mathf.Max(.001f, f.Glength);
            f.Gresolution = EditorGUILayout.FloatField("Resolution", f.Gresolution);
            f.Gresolution = Mathf.Max(f.Gresolution, .01f);
            EditorGUILayout.BeginHorizontal();
            f.GsplitProba = EditorGUILayout.Slider("Split proba", f.GsplitProba, 0, 1);
            f.GsplitProbaCurve = EditorGUILayout.CurveField(f.GsplitProbaCurve);
            EditorGUILayout.EndHorizontal();
            f.GsplitAngle = EditorGUILayout.Slider("Split angle", f.GsplitAngle, 0, 2);
            f.Gradius = EditorGUILayout.CurveField("Shape", f.Gradius);
            f.GsplitRadius = EditorGUILayout.Slider("Split radius", f.GsplitRadius, .5f, .999f);
            f.GmaxSplits = EditorGUILayout.IntSlider("Max splits at a time", f.GmaxSplits, 2, 4);
            f.Grandomness = EditorGUILayout.Slider("Randomness", f.Grandomness, 0, 1);
            f.GupAttraction = EditorGUILayout.Slider("Up attraction", f.GupAttraction, 0, 1f);
            f.GgravityStrength = EditorGUILayout.FloatField("Gravity strength", f.GgravityStrength);
        }

        else if (f.type == FunctionType.Split)
        {
            f.seed = EditorGUILayout.IntField("Seed", f.seed);
            f.Snumber = EditorGUILayout.IntField("Number", f.Snumber);
            f.Snumber = Mathf.Max(0, f.Snumber);
            f.SsplitAngle = EditorGUILayout.Slider("Split angle", f.SsplitAngle, 0, 2);
            f.SmaxSplits = EditorGUILayout.IntSlider("Max splits at a time", f.SmaxSplits, 1, 10);
            f.SsplitRadius = EditorGUILayout.Slider("split radius", f.SsplitRadius, 0.001f, 1);
            f.Sstart = EditorGUILayout.Slider("Start", f.Sstart, 0f, 1f);
            f.Sspread = EditorGUILayout.Slider("Height spread", f.Sspread, 0, 1);
        }

        else if (f.type == FunctionType.Branch)
        {
            f.seed = EditorGUILayout.IntField("Seed", f.seed);
            f.Bnumber = EditorGUILayout.IntField("Number", f.Bnumber);
            f.Bnumber = Mathf.Max(0, f.Bnumber);
            EditorGUILayout.BeginHorizontal();
            f.Blength = EditorGUILayout.FloatField("Length", f.Blength);
            f.Blength = Mathf.Max(f.Blength, .001f);
            f.BlengthCurve = EditorGUILayout.CurveField(f.BlengthCurve);
            EditorGUILayout.EndHorizontal();
            f.Bresolution = EditorGUILayout.FloatField("Resolution", f.Bresolution);
            f.Bresolution = Mathf.Max(f.Bresolution, .01f);
            f.Brandomness = EditorGUILayout.Slider("Randomness", f.Brandomness, 0, 1);
            f.Bradius = EditorGUILayout.Slider("Radius", f.Bradius, 0.001f, 1);
            EditorGUILayout.BeginHorizontal();
            f.BsplitProba = EditorGUILayout.Slider("Split proba", f.BsplitProba, 0, 1);
            f.BsplitProbaCurve = EditorGUILayout.CurveField(f.BsplitProbaCurve);
            EditorGUILayout.EndHorizontal();
            f.BmaxSplits = EditorGUILayout.IntSlider("Max splits number", f.BmaxSplits, 1, 5);
            f.Bangle = EditorGUILayout.Slider("Angle", f.Bangle, 0, 2);
            f.BupAttraction = EditorGUILayout.Slider("Up attraction", f.BupAttraction, 0, 1);
            f.BgravityStrength = EditorGUILayout.FloatField("Gravity strength", f.BgravityStrength);
            f.Bstart = EditorGUILayout.Slider("Start", f.Bstart, 0f, 1f);
        }

        else if (f.type == FunctionType.Leaf)
        {
            if (f.LleafTypesNames == null || f.LleafTypesNames.Length == 5)
                f.LleafTypesNames = new string[6] { "cross", "diamond cross", "diamond", "long", "plane", "custom" };
            
            f.LleafType = EditorGUILayout.Popup(f.LleafType, f.LleafTypesNames);
            Mesh[] leafMesh = null;
            switch (f.LleafType)
            {
                case 0:
                    leafMesh = new Mesh[] {Resources.LoadAll<Mesh>("Mtree/branches")[0]};
                    break;
                case 1:
                    leafMesh = new Mesh[] { Resources.LoadAll<Mesh>("Mtree/branches")[1]};
                    break;
                case 2:
                    leafMesh = new Mesh[] { Resources.LoadAll<Mesh>("Mtree/branches")[2], Resources.LoadAll<Mesh>("Mtree/branches")[3]};
                    
                    break;
                case 3:
                    leafMesh = new Mesh[] { Resources.LoadAll<Mesh>("Mtree/branches")[4], Resources.LoadAll<Mesh>("Mtree/branches")[5]};                    
                    break;
                case 4:
                    leafMesh = new Mesh[] { Resources.LoadAll<Mesh>("Mtree/branches")[6]};                    
                    break;
                case 5:
                    leafMesh = new Mesh[] { (Mesh)EditorGUILayout.ObjectField("Mesh: ", f.LleafMesh[0], typeof(Mesh), false) };
                    break;
            }
            f.LleafMesh = leafMesh;
            f.Lnumber = EditorGUILayout.IntField("number", f.Lnumber);
            f.Lnumber = Mathf.Max(f.Lnumber, 0);
            f.Lsize = EditorGUILayout.FloatField("Size", f.Lsize);
            f.Lsize = Mathf.Max(0, f.Lsize);
            f.LmaxRadius = EditorGUILayout.Slider("Max branch radius", f.LmaxRadius, 0, 1);
            f.LmaxRadius = Mathf.Max(0.001f, f.LmaxRadius);
            EditorGUILayout.MinMaxSlider("leafs weight", ref f.LminWeight, ref f.LmaxWeight, -1, 1);
            f.LoverrideNormals = EditorGUILayout.Toggle("Override Normals", f.LoverrideNormals);
        }

        EditorGUILayout.EndVertical();
        if (EditorGUI.EndChangeCheck())
        {
            UpdateTree();
        }
    }

    private void UpdateTree()
    {
        tree.GenerateTree();
    }

    private bool IsMultiSelection()
    {
        return targets.Length > 1;
    }
    
    private void UndoCallback()
    {
        UndoDirty = true;
    }
}


