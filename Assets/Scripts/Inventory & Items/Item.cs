using UnityEngine;

public abstract class Item : Interactable, IHoverableDescription {

    //***************************
    // Variables
    //***************************
    [SerializeField]
    protected string m_name;
    [SerializeField]
    protected string m_description;
    [SerializeField]
    protected Sprite m_sprite;


    //***************************
    // Getter & Setterdw
    //***************************
    public string hoveredTitle { get { return m_name; } }
    public string hoveredDescription {  get { return m_description; } }
    public string itemName { get { return m_name; } set{ m_name = value; } }
    public string description { get { return m_description; } set { m_description = value; } }
    public Sprite sprite { get { return m_sprite; } set { m_sprite = value; } }

    protected virtual void Awake()
    {
        m_sprite = GetComponent<SpriteRenderer>().sprite;
        interactionName = "Pick Up " + m_name;
    }
}
