using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{

    [SerializeField] string id;
    public string ID { get { return id; } }
    public string ItemName;
    public Texture Icon;
    public string ItemDescription;
    [Range(1, 999)]
    public int MaximumStacks = 1;

    private void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }

    public virtual Item GetCopy()
    {
        return this;
    }

    public virtual void Destroy()
    {

    }
}
