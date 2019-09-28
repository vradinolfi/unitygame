using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Mtree
{

    [System.Serializable]
    public class TreeFunction // All Functions are implemented in one class since Unity doens't Serialize child classes
    {
        public static int positionOffset = 30; // offset in x position from parent
        public static int height = 37;
        public static int width = 200;
        public static int margin = -3;
        public static int deleteButtonSize = 18;
        public GUIStyle nodeStyle;
        public GUIStyle selectedNodeStyle;
        public GUIStyle deleteButtonStyle;

        public int seed;
        public FunctionType type;
        public int id; // id of tree function. Each tree function a same MtreeComponent instance have different id values
        public int position; // x offset of the drawn function
        public Rect rect; // rectangle of gui
        public Rect deleteRect; // rectangle of delete button
        public int parentId; // Unity does not save parent when serilizing, this will be used to refresh parent
        public TreeFunction parent;
        public string name;

        #region function parameters

        // Trunk parameters
        public float TradiusMultiplier = .3f;
        public AnimationCurve Tradius;
        public float Tlength = 10;
        public int Tnumber = 1;
        public float Trandomness = .1f;
        public float ToriginAttraction = .1f;
        public float Tresolution = 1.5f;
        public AnimationCurve TrootShape;
        public float TrootRadius = .25f;
        public float TrootHeight = 1f;
        public float TrootResolution = 3f;
        public int TflareNumber = 5;
        public float TspinAmount = .1f;
        public float TdisplacementStrength = 1f;
        public float TdisplacementSize = 2.5f;
        public float TheightOffset = .5f;


        // Grow Parameters
        public float Glength = 10f;
        public AnimationCurve GlengthCurve = AnimationCurve.Linear(0, 1, 1, .8f);
        public float Gresolution = 1f;
        public float GsplitProba = .1f;
        public float GsplitAngle = .5f;
        public int GmaxSplits = 2;
        public AnimationCurve Gradius = AnimationCurve.EaseInOut(0, 1, 1, 0);
        public AnimationCurve GsplitProbaCurve = AnimationCurve.Linear(0, .5f, 1, 1f);
        public float GsplitRadius = .8f;
        public float Grandomness = .25f;
        public float GupAttraction = .5f;
        public float GgravityStrength = .1f;

        // Split Parameters
        public int Snumber = 15;
        public float SsplitAngle = .5f;
        public int SmaxSplits = 2;
        public float SsplitRadius = .6f;
        public float Sstart = .2f;
        public float Sspread = 1;

        // Branch Parameters
        public float Blength = 7f;
        public AnimationCurve BlengthCurve = AnimationCurve.Linear(0, 1, 1, .8f);
        public float Bresolution = 1f;
        public int Bnumber = 15;
        public float BsplitProba = .2f;
        public float Bangle = .7f;
        public float Brandomness = .3f;
        public AnimationCurve Bshape = AnimationCurve.Linear(0, 1, 1, 0);
        public AnimationCurve BsplitProbaCurve = AnimationCurve.Linear(0, .5f, 1, 1f);
        public float Bradius = .7f;
        public float BupAttraction = .7f;
        public float Bstart = .2f;
        public int BmaxSplits = 4;
        public float BgravityStrength = .1f;


        // Leaf Parameters
        public int LleafType = 4;
        public string[] LleafTypesNames = { "cross", "diamond cross", "diamond", "long", "plane", "custom" };
        public Mesh[] LleafMesh = { Resources.LoadAll<Mesh>("Mtree/branches")[6] };
        public int Lnumber = 250;
        public float LmaxRadius = .25f;
        public float Lsize = 2f;
        public bool LoverrideNormals = true;
        public float LminWeight = .2f;
        public float LmaxWeight = .5f;
        #endregion


        public TreeFunction(int id, FunctionType type, TreeFunction parent)
        {
            UpdateStyle();

            this.id = id;
            this.type = type;
            this.parent = parent;
            parentId = parent == null ? -1 : parent.id;
            name = type.ToString();
            position = parent == null ? 0 : parent.position + positionOffset;
            seed = Random.Range(0, 1000);
            rect = new Rect();
            deleteRect = new Rect();
            if (type == FunctionType.Grow)
            {
                Keyframe[] keys = new Keyframe[2] { new Keyframe(0f, 1f, 0f, 0f), new Keyframe(1f, 0f, -.5f, -1f) };
                Gradius = new AnimationCurve(keys);
            }

            if (type == FunctionType.Branch && parent != null && parent.type != FunctionType.Trunk)
            {
                Blength = 3;
                Bstart = .2f;
                Bangle = .5f;
                Bnumber = 55;
                BmaxSplits = 2;
                Bresolution = 1;
            }
            if (type == FunctionType.Trunk)
            {
                Keyframe[] keys = new Keyframe[2] { new Keyframe(0f, 1f, 0f, 0f), new Keyframe(1f, 0f, -1f, -1f) };
                Tradius = new AnimationCurve(keys);
                Keyframe[] rootKeys = new Keyframe[2] { new Keyframe(0f, 1f, -2f, -2f), new Keyframe(1f, 0f, 0f, 0f) };
                TrootShape = new AnimationCurve(rootKeys);
            }
        }

        public void Execute(MTree tree)
        {
            int selection = parent==null ? 0 : parent.id;
            Random.InitState(seed);
            

            if (type == FunctionType.Trunk)
                tree.AddTrunk(Vector3.down * TheightOffset, Vector3.up, Tlength, Tradius, TradiusMultiplier, Tresolution, Trandomness
                            , id, TrootShape, TrootRadius, TrootHeight, TrootResolution, ToriginAttraction);


            if (type == FunctionType.Grow)
                tree.Grow(Glength, GlengthCurve, Gresolution, GsplitProba, GsplitProbaCurve, GsplitAngle, GmaxSplits, selection
                        , id, Grandomness, Gradius, GsplitRadius, GupAttraction, GgravityStrength, 0f, 0.001f);

            if (type == FunctionType.Split)
                tree.Split(selection, Snumber, SsplitAngle, id, SsplitRadius, SmaxSplits, Sstart, Sspread, 0f);

            if (type == FunctionType.Branch)
                tree.AddBranches(selection, Blength, BlengthCurve, Bresolution, Bnumber, BsplitProba, BsplitProbaCurve, Bangle
                                , Brandomness, Bshape, Bradius, BupAttraction, id, Bstart, BmaxSplits, BgravityStrength, 0f, 0.001f);

            if (type == FunctionType.Leaf)
                tree.AddLeafs(LmaxRadius, Lnumber, LleafMesh, Lsize, LoverrideNormals, LminWeight, LmaxWeight, selection, 70);
        }

        public void UpdateRect(int offset)
        {
            position = parent == null ? 0 : parent.position + positionOffset;
            int y = offset * (height + margin);
            rect.Set(position, y, width, height);
            deleteRect.Set(position + width - 12 - deleteButtonSize, y + (height - deleteButtonSize) / 2, deleteButtonSize, deleteButtonSize);
        }

        public void UpdateStyle()
        {
        #if UNITY_EDITOR
            nodeStyle = new GUIStyle();
            //nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            nodeStyle.normal.background = Resources.Load("Mtree/Sprites/TreeFunctionSprite") as Texture2D;
            nodeStyle.border = new RectOffset(20, 10, 10, 10);
            nodeStyle.alignment = TextAnchor.MiddleCenter;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.fontStyle = FontStyle.Bold;

            selectedNodeStyle = new GUIStyle(nodeStyle);
            selectedNodeStyle.normal.background = Resources.Load("Mtree/Sprites/TreeFunctionSpriteSelected") as Texture2D;

            deleteButtonStyle = new GUIStyle();
            deleteButtonStyle.normal.background = Resources.Load("Mtree/Sprites/DeleteCrossSprite") as Texture2D;
#endif
        }

        public void Draw(bool isSelected = false)
        {
        #if UNITY_EDITOR
            GUIStyle style = isSelected ? selectedNodeStyle : nodeStyle;
            GUI.Box(rect, name, style);

            if (type != FunctionType.Trunk)
                GUI.Button(deleteRect, "", deleteButtonStyle);

            if (parent != null)
            {
                Vector2 start_pos = parent.rect.position + new Vector2(6, height* 6/7);
                Vector2 end_pos = rect.position + new Vector2(6, height / 2);
                Handles.DrawBezier(start_pos, end_pos, start_pos + Vector2.up*15, end_pos + Vector2.left*15, Color.white, null, 4f);
            }
        #endif
        }

        public void RevertOutOfBoundValues()
        {
            if (Bstart > 1)
                Bstart = 0.2f;
            if (Sstart > 1)
                Sstart = 0.2f;
            if (LmaxRadius > 1)
                LmaxRadius = 0.25f;

        }
    }

    public enum FunctionType {Trunk, Grow, Split, Branch, Leaf}

}