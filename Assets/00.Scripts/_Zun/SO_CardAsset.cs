using UnityEngine;

[CreateAssetMenu(fileName = "newcard", menuName = "SO/CardAsset")]
public class SO_CardAsset : ScriptableObject
{
    public string description;
    public Sprite cardImage;
    public ProjectileSO projectileSO;
}
