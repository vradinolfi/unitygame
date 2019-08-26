using UnityEngine;

public class RotationSaver : Saver
{
    public Transform transformToSave;


    protected override string SetKey()
    {
        return transformToSave.name + transformToSave.GetType().FullName + uniqueIdentifier;
    }


    protected override void Save()
    {
        saveData.Save(key, transformToSave.rotation);
    }


    protected override void Load()
    {
        Quaternion rotation = Quaternion.identity;

        if (saveData.Load(key, ref rotation))
            transformToSave.rotation = rotation;
    }
}
