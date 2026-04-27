using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGraphNode : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI Text;

    private GraphNode Node;

    public void Reset()
    {
        SetColor(Node.CanVisit ? Color.white : Color.gray);
        SetText($"ID: {Node.Id}\nWeight: {Node.Weight}");
    }

    public void SetNode(GraphNode node)
    {
        Node = node;
    }

    public void SetColor(Color color)
    {
        Image.color = color;
    }

    public void SetText(string text)
    {
        Text.text = text;
    }
}