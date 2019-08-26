using UnityEngine;

public class BehaviourEnableStateSaver : Saver
{
    public Behaviour behaviourToSave;


    protected override string SetKey ()
    {
        return behaviourToSave.name + behaviourToSave.GetType ().FullName + uniqueIdentifier;
    }


    protected override void Save ()
    {
        saveData.Save (key, behaviourToSave.enabled);
    }


    protected override void Load ()
    {
        bool enabledState = false;

        if(saveData.Load (key, ref enabledState))
            behaviourToSave.enabled = enabledState;
    }
}
