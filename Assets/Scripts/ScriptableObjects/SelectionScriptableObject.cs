using UnityEngine;

[CreateAssetMenu(fileName = "SelectionScriptableObject", menuName = "ScriptableObjects/Selection")]
public class SelectionScriptableObject : ScriptableObject
{
    //Base stats for towers
    [SerializeField]
    GameObject selection;
    public GameObject Selection {get => selection; set => selection = value; } 
    [SerializeField]
    string name;
    public string Name { get => name; set => name = value; }
    [SerializeField]
    string description;
    public string Description { get => description; set => description = value; }
    [SerializeField]
    Sprite sprite;
    public Sprite Sprite { get => sprite; set => sprite = value; }
    [SerializeField]
    int cost;
    public int Cost { get => cost; set => cost = value; }
}
