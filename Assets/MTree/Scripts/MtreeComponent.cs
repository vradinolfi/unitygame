using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mtree;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MtreeComponent : MonoBehaviour {

    public List<Mtree.TreeFunction> treeFunctions;
    public int selectedFunctionIndex = 0; // Index of selected Function in editor
    public MTree tree;
    public int Lod = 0;
    public float[] radialResolution = {1, .5f, .25f, .1f};
    public float[] simplifyAngleThreshold = { 3, 10, 20, 30 };
    public float[] simplifyRadiusThreshold = {0, .01f, .05f, .1f};
    public float[] simplifyLeafs = { 0, .2f, .4f, .6f };
    public string saveTreeFolder = "Assets";
    public int polycount = 0;
    private int treeFunctionId = 0; // Id for the next created Function
    private Material leafMaterial;
    public string MtreeVersion;

	void InitializeTree()
    {
        MtreeVersion = MtreeVariables.MtreeVersion;
        tree = new MTree(transform);       
    }

    public void AddTrunkFunction()
    {
        TreeFunction f = new TreeFunction(treeFunctionId, FunctionType.Trunk, null);
        treeFunctions.Add(f);
        treeFunctionId++;
        GenerateTree();
    }

    public void AddGrowFunction()
    {
        AddTreeFunction(FunctionType.Grow);
        GenerateTree();
    }

    public void AddSplitFunction()
    {
        AddTreeFunction(FunctionType.Split);
        GenerateTree();
    }

    public void AddBranchFunction()
    {
        AddTreeFunction(FunctionType.Branch);
        GenerateTree();
    }

    public void AddLeafFunction()
    {
        AddTreeFunction(FunctionType.Leaf);
        GenerateTree();
    }

    public void AddTreeFunction(FunctionType type)
    {
        #if UNITY_EDITOR
        Undo.RecordObject(this, "Added function");
        #endif
        int functionIndex = selectedFunctionIndex + 1;
        int parentPos = treeFunctions[selectedFunctionIndex].position;
        while(functionIndex < treeFunctions.Count && treeFunctions[functionIndex].position > parentPos)
        {
            functionIndex++;
        }
        TreeFunction f = new TreeFunction(treeFunctionId, type, treeFunctions[selectedFunctionIndex]);
        treeFunctions.Insert(functionIndex, f);
        selectedFunctionIndex = functionIndex;
        treeFunctionId++;
        UpdateTreeFunctions();
    }

    public void RandomizeTree()
    {
        foreach (TreeFunction tf in treeFunctions)
        {
            tf.seed = Random.Range(0, 1000);
        }
    }

    public void UpdateTreeFunctions()
    {
        if (treeFunctions == null)
        {
            treeFunctions = new List<Mtree.TreeFunction>();
            AddTrunkFunction();
        }

        int n = treeFunctions.Count;
        bool styleFlushed = false; // true when unity serialization has failed
        for (int i=0; i<n; i++)
        {
            TreeFunction tf = treeFunctions[i];
            if (tf.type != FunctionType.Trunk && tf.parent == null) // Reassigning parent when unity serialization fails to keep it
            {
                styleFlushed = true;
                tf.UpdateStyle();
                int functionId = tf.parentId;
                foreach (TreeFunction f in treeFunctions)
                {
                    if (f.id == functionId)
                    {
                        tf.parent = f;
                        break;
                    }
                }
            }
        }
        if (styleFlushed)
            treeFunctions[0].UpdateStyle();


        for (int i=0; i<n; i++)
        {
            treeFunctions[i].UpdateRect(i);
        }
    }

    public void RemoveFunction(int index)
    {
        #if UNITY_EDITOR
        Undo.RecordObject(this, "Removed function");
        #endif
        Mtree.TreeFunction functionToRemove = treeFunctions[index];
        TreeFunction newParent = functionToRemove.parent;
        
        
        foreach (TreeFunction tf in treeFunctions)
        {
            if (tf.parent != null && tf.parent.id == functionToRemove.id)
                tf.parent = newParent;
        }

        treeFunctions.RemoveAt(index);

        if (selectedFunctionIndex >= index)
            selectedFunctionIndex--;

        UpdateTreeFunctions();
    }

    private void ExecuteFunctions()
    {
        if (tree == null)
            InitializeTree();
        if (treeFunctions == null)
            UpdateTreeFunctions();
        int n = treeFunctions.Count;
        for(int i=0; i<n; i++)
        {
            treeFunctions[i].Execute(tree);
        }
    }

    Mesh CreateMesh(bool ao)
    {
        Mesh mesh = new Mesh();
        if (treeFunctions.Count > 0)
            tree.GenerateMeshData(treeFunctions[0], simplifyLeafs[Lod], radialResolution[Lod]);
        mesh.vertices = tree.verts;
        mesh.normals = tree.normals;
        mesh.uv = tree.uvs;
        Color[] colors = tree.colors;
        mesh.triangles = tree.triangles;
        if (tree.leafTriangles.Length > 0)
        {
            mesh.subMeshCount = 2;
            mesh.SetTriangles(tree.leafTriangles, 1);
        }
        if (ao)
        {
            Ao.BakeAo(ref colors, tree.verts, tree.normals, tree.triangles, tree.leafTriangles, gameObject, 64, 20);
            DestroyImmediate(GetComponent<MeshCollider>());
        }
        mesh.colors = colors;
        GetComponent<MeshFilter>().mesh = mesh;

        polycount = mesh.triangles.Length / 3;
        return mesh;
    }

    #if UNITY_EDITOR
    public GameObject CreateBillboard(string path, string name)
    {
        GameObject camObject = Instantiate(Resources.Load("Mtree/MtreeBillboardCamera") as GameObject); // create billboard and render it
        Camera cam = camObject.GetComponent<Camera>();
        Billboard bill = new Billboard(cam, gameObject, 512, 512);
        bill.SetupCamera();
        string texturePath = path + name + "_billboard.png";
        bill.Render(texturePath);
        DestroyImmediate(camObject);

        Mesh billboardMesh = bill.CreateMesh(); // create billboard mesh
        AssetDatabase.CreateAsset(billboardMesh, path + name + "_LOD4.mesh");

        GameObject billboard = new GameObject(name + "_LOD4"); // create billboard object and assign mesh
        MeshFilter meshFilter = billboard.AddComponent<MeshFilter>();
        meshFilter.mesh = billboardMesh;
        MeshRenderer meshRenderer = billboard.AddComponent<MeshRenderer>();

        Texture billboardTexture = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D)); // create material
        Material mat = bill.CreateMaterial(billboardTexture);
        AssetDatabase.CreateAsset(mat, path + name + "billboard.mat");
        meshRenderer.material = mat;

        return billboard;
    }
    #endif

    public void BakeAo()
    {
        GenerateTree(ao:true);
    }

    public Mesh GenerateTree(bool ao=false)
    {
        if (treeFunctions != null && treeFunctions.Count > 1 && MtreeVersion != MtreeVariables.MtreeVersion)
            return null;
        

        tree = null;
        ExecuteFunctions();
        tree.Simplify(simplifyAngleThreshold[Lod], simplifyRadiusThreshold[Lod]);
        Mesh mesh = CreateMesh(ao);
        UpdateMaterials();
        return mesh;
    }

    public void UpdateMaterials()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (tree.leafTriangles.Length == 0)
        {
            switch (renderer.sharedMaterials.Length)
            {
                case 1:
                    if (renderer.sharedMaterial == null)
                    {
                        Material mat = new Material(Utils.GetBarkShader());
                        renderer.sharedMaterial = mat;
                    }
                    break;

                case 2:
                    leafMaterial = renderer.sharedMaterials[1];
                    renderer.sharedMaterials = new Material[] { renderer.sharedMaterials[0] };
                    break;
            }
        }
        else
        {
            switch (renderer.sharedMaterials.Length)
            {
                case 0:
                    Material barkMat = new Material(Utils.GetBarkShader());
                    renderer.sharedMaterial = barkMat;
                    break;

                case 1:
                    if (leafMaterial == null)
                        leafMaterial = new Material(Utils.GetLeafShader());
                    renderer.sharedMaterials = new Material[] { renderer.sharedMaterials[0], leafMaterial };
                    break;
            }
        }

    }

    #if UNITY_EDITOR
    public Material[] SaveMaterials(string folderPath)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Material[] materialsCopy = new Material[renderer.sharedMaterials.Length];
        int matIndex = 0;
        foreach (Material mat in renderer.sharedMaterials)
        {
            if (AssetDatabase.GetAssetPath(mat).Length == 0)
            {
                string matName = Path.GetFileName(mat.name);
                string matPath = Path.Combine(folderPath, matName + ".mat");
                Material matCopy = new Material(mat);
                materialsCopy[matIndex] = matCopy;
                AssetDatabase.CreateAsset(matCopy, matPath);
            }
            else
            {
                materialsCopy[matIndex] = mat;
            }
            matIndex++;
        }

        return materialsCopy;
    }
    #endif

    private void Start()
    {
        UpdateTreeFunctions();
        GenerateTree();
        Debug.LogWarning("A tree with Mtree component still attached is used, use trees exported as prefabs in order to optimize your scene. To export a tree, select it, go to the 'save as Prefab' tab and click on 'Save as prefab'");
    }

    #if UNITY_EDITOR
    #if UNITY_2018_3
    public void SaveAsPrefab(bool groupedSave = false)
    {
        string name = gameObject.name;
        string path = saveTreeFolder;
        if (string.IsNullOrEmpty(path))
            return;
        if (!System.IO.Directory.Exists(path))
        {
            EditorUtility.DisplayDialog("Invalid Path", "The path is not valid, you can chose it with the find folder button", "Ok");
            return;
        }
        if (AssetDatabase.LoadAssetAtPath(path + "/" + name + ".prefab", typeof(GameObject))) // Overriding prefab dialog
        {
            if (EditorUtility.DisplayDialog("Are you sure?", "The prefab already exists. Do you want to overwrite it?", "Yes", "No"))
            {
                FileUtil.DeleteFileOrDirectory(Path.Combine(path, name + "_meshes"));
            }
            else
            {
                name += "_1";
            }
        }

        Mesh[] meshes = new Mesh[4];
        string meshesFolder = AssetDatabase.CreateFolder(path, name + "_meshes");
        meshesFolder = AssetDatabase.GUIDToAssetPath(meshesFolder) + Path.DirectorySeparatorChar;
        Material[] materials = SaveMaterials(meshesFolder);
        GameObject TreeObject = new GameObject(name); // Tree game object
        LODGroup group = TreeObject.AddComponent<LODGroup>(); // LOD Group
        group.fadeMode = LODFadeMode.CrossFade;
        int lodNumber = 4; // Creating LODs
        LOD[] lods = new LOD[lodNumber + 1];

        // Generating Billboard 
        Lod = 3;
        GenerateTree(true);
        GameObject billboard = CreateBillboard(meshesFolder, name);
        Renderer[] bill_re = new Renderer[1] { billboard.GetComponent<MeshRenderer>() };
        lods[lodNumber] = new LOD(.01f, bill_re);


        for (int lod = lodNumber - 1; lod > -1; lod--) // create and save all LOD meshes
        {
            string meshPath = meshesFolder + name + "_LOD" + lod.ToString() + ".mesh"; //updating path for each LOD
            Lod = lod;
            Mesh mesh = GenerateTree(ao: true);
            meshes[lod] = mesh;
            AssetDatabase.CreateAsset(mesh, meshPath);
        }

        for (int i = 0; i < lodNumber; i++) // assigning lod meshes to LOD array
        {
            GameObject go = new GameObject(name + "_LOD" + i.ToString());
            go.transform.parent = TreeObject.transform;
            MeshFilter mf = go.AddComponent<MeshFilter>();
            mf.mesh = meshes[i];
            Renderer[] re = new Renderer[1] { go.AddComponent<MeshRenderer>() }; // the renderer to put in LODs
            re[0].sharedMaterials = materials;
            float t = Mathf.Pow((i + 1) * 1f / (lodNumber + 1), 1); // float between 0 and 1 following f(x) = pow(x, n)
            lods[i] = new LOD((1 - t) * 0.9f + t * .01f, re); // assigning renderer
            lods[i].fadeTransitionWidth = 0.25f;
        }

        billboard.transform.parent = TreeObject.transform; // making billboard child of tree object


        group.SetLODs(lods); // assigning LODs to lod group
        group.RecalculateBounds();

        string prefabPath = path + "/" + name + ".prefab";
        TreeObject.AddComponent<MtreeWind>();

        Object prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(TreeObject, prefabPath, InteractionMode.AutomatedAction);
        AssetDatabase.SaveAssets();
        DestroyImmediate(TreeObject);

        if (!groupedSave)
        {
            // select newly created prefab in folder
            Selection.activeObject = prefab;
            // Also flash the folder yellow to highlight it
            EditorGUIUtility.PingObject(prefab);
            EditorUtility.DisplayDialog("Prefab saved !", "The prefab is saved, you can now delete the tree and use the prefab instead", "Ok");
        }

    }
#else
    public void SaveAsPrefab(bool groupedSave = false)
    {
        string name = gameObject.name;
        string path = saveTreeFolder;
        if (string.IsNullOrEmpty(path))
            return;

        bool replacePrefab = false;

        if (!System.IO.Directory.Exists(path))
        {
            EditorUtility.DisplayDialog("Invalid Path", "The path is not valid, you can chose it with the find folder button", "Ok");
            return;
        }
        if (AssetDatabase.LoadAssetAtPath(path + "/" + name + ".prefab", typeof(GameObject))) // Overriding prefab dialog
        {
            if (EditorUtility.DisplayDialog("Are you sure?", "The prefab already exists. Do you want to overwrite it?", "Yes", "No"))
            {
                FileUtil.DeleteFileOrDirectory(Path.Combine(path, name + "_meshes"));
                AssetDatabase.Refresh();
                replacePrefab = true;
            }
            else
            {
                name += "_1";
            }
        }

        Mesh[] meshes = new Mesh[4];
        string meshesFolder = AssetDatabase.CreateFolder(path, name + "_meshes");
        meshesFolder = AssetDatabase.GUIDToAssetPath(meshesFolder) + Path.DirectorySeparatorChar;
        Material[] materials = SaveMaterials(meshesFolder);
        GameObject TreeObject = new GameObject(name); // Tree game object
        LODGroup group = TreeObject.AddComponent<LODGroup>(); // LOD Group
        group.fadeMode = LODFadeMode.CrossFade;
        int lodNumber = 4; // Creating LODs
        LOD[] lods = new LOD[lodNumber + 1];

        // Generating Billboard 
        Lod = 3;
        GenerateTree(true);
        GameObject billboard = CreateBillboard(meshesFolder, name);
        Renderer[] bill_re = new Renderer[1] { billboard.GetComponent<MeshRenderer>() };
        lods[lodNumber] = new LOD(.01f, bill_re);


        for (int lod= lodNumber-1; lod>-1; lod--) // create and save all LOD meshes
        {
            string meshPath = meshesFolder + name + "_LOD" + lod.ToString() + ".mesh"; //updating path for each LOD
            Lod = lod;
            Mesh mesh = GenerateTree(ao: true);
            meshes[lod] = mesh;
            AssetDatabase.CreateAsset(mesh, meshPath);
        }

        for (int i=0; i<lodNumber; i++) // assigning lod meshes to LOD array
        {
            GameObject go = new GameObject(name + "_LOD" + i.ToString());
            go.transform.parent = TreeObject.transform;
            MeshFilter mf = go.AddComponent<MeshFilter>();
            mf.mesh = meshes[i];
            Renderer[] re =  new Renderer[1] { go.AddComponent<MeshRenderer>() }; // the renderer to put in LODs
            re[0].sharedMaterials = materials;
            float t = Mathf.Pow((i + 1)*1f / (lodNumber + 1), 1); // float between 0 and 1 following f(x) = pow(x, n)
            lods[i] = new LOD( (1-t)*0.9f + t*.01f, re); // assigning renderer
            lods[i].fadeTransitionWidth = 0.25f;
        }

        billboard.transform.parent = TreeObject.transform; // making billboard child of tree object
        

        group.SetLODs(lods); // assigning LODs to lod group
        group.RecalculateBounds();

        string prefabPath = path + "/" + name + ".prefab";
        TreeObject.AddComponent<MtreeWind>();

        Object prefab;
        if (replacePrefab)
        {
            Object targetPrefab = AssetDatabase.LoadAssetAtPath(path + "/" + name + ".prefab", typeof(GameObject));
            prefab = PrefabUtility.ReplacePrefab(TreeObject, targetPrefab, ReplacePrefabOptions.ConnectToPrefab);
        }
        else
        {
            prefab = PrefabUtility.CreatePrefab(prefabPath, TreeObject, ReplacePrefabOptions.ConnectToPrefab);
        }
        
        AssetDatabase.SaveAssets();
        DestroyImmediate(TreeObject);
        
        if (!groupedSave)
        {
            // select newly created prefab in folder
            Selection.activeObject = prefab;
            // Also flash the folder yellow to highlight it
            EditorGUIUtility.PingObject(prefab);
            EditorUtility.DisplayDialog("Prefab saved !", "The prefab is saved, you can now delete the tree and use the prefab instead", "Ok");
        }

    }
#endif
#endif
}


