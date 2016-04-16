using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity component for easy Text usage. Added shadow and outline for text mesh
/// Author: Insality
/// </summary>

[RequireComponent(typeof(TextMesh))]
public class TextMeshController : MonoBehaviour
{
    public TextMesh TextMesh;

    public string Text;

    public TextMeshStyle TextStyle = TextMeshStyle.None;
    public List<TextMesh> OutlineClones;
    public float OutlineWidth = 2;
    public Color OutlineColor = Color.black;
    public Vector2 ShadowOffset = new Vector2(1, -1);

    private void Start() {
        TextMesh = GetComponent<TextMesh>();
    }

    public void FindTextMesh() {
        if (TextMesh == null) {
            TextMesh = GetComponent<TextMesh>();
            SetDefaultValues();
        }
        if (OutlineClones == null) {
            OutlineClones = new List<TextMesh>();
        }
    }

    private void SetDefaultValues()
    {
        TextMesh.anchor = TextAnchor.MiddleCenter;
        TextMesh.alignment = TextAlignment.Center;
        TextMesh.fontSize = 64;
        TextMesh.characterSize = 6;
    }

    public void Commit() {
        TextMesh.text = Text;
        foreach (var copy in OutlineClones) {
            copy.text = Text;
        }
    }

    public void RefreshEffect() {
        ClearEffect();
        switch (TextStyle) {
            case TextMeshStyle.None:
                break;
            case TextMeshStyle.Outline:
                CreateOutline();
                break;
            case TextMeshStyle.Shadow:
                CreateShadow();
                break;
        }
    }

    public void RefreshEffectParam() {
        foreach (var copy in OutlineClones) {
            if (copy != null) {
                copy.alignment = TextMesh.alignment;
                copy.characterSize = TextMesh.characterSize;
                copy.font = TextMesh.font;
                copy.lineSpacing = TextMesh.lineSpacing;
                copy.anchor = TextMesh.anchor;
            }
        }
    }

    private void CreateOutline()
    {
        const int outlineWidth = 8;

        OutlineClones = new List<TextMesh>();
        for (int i = 0; i < outlineWidth; ++i) {
            var tmpGO = Instantiate(gameObject);
            OutlineClones.Add(tmpGO.GetComponent<TextMesh>());
            foreach (Transform child in tmpGO.transform)
            {
                DestroyImmediate(child.gameObject);
            }

            var controller = OutlineClones[i].GetComponent<TextMeshController>();
            DestroyImmediate(controller);
        }
        for (int i = 0; i < outlineWidth; ++i) {
            OutlineClones[i].transform.parent = transform;
            OutlineClones[i].transform.localPosition = GetOutlinePostionByIndex(i);
            OutlineClones[i].color = OutlineColor;
        }
    }

    private void CreateShadow() {
        OutlineClones = new List<TextMesh>();
        var tmpGO = Instantiate(gameObject);
        foreach (Transform child in tmpGO.transform)
        {
            DestroyImmediate(child.gameObject);
        }

        OutlineClones.Add(tmpGO.GetComponent<TextMesh>());
        var controller = OutlineClones[0].GetComponent<TextMeshController>();
        DestroyImmediate(controller);
        OutlineClones[0].transform.parent = transform;
        OutlineClones[0].transform.localPosition = new Vector3(ShadowOffset.x, ShadowOffset.y){z = 0.1f};
        OutlineClones[0].color = OutlineColor;
    }

    private Vector3 GetOutlinePostionByIndex(int index) {
        float halfOutline = OutlineWidth / 2;
        const float zCoord = 0.1f;
        switch (index) {
            case 0: return new Vector3(OutlineWidth, 0, zCoord);
            case 1: return new Vector3(-OutlineWidth, 0, zCoord);
            case 2: return new Vector3(0, OutlineWidth, zCoord);
            case 3: return new Vector3(0, -OutlineWidth, zCoord);
            case 4: return new Vector3(halfOutline, halfOutline, zCoord);
            case 5: return new Vector3(-halfOutline, halfOutline, zCoord);
            case 6: return new Vector3(halfOutline, -halfOutline, zCoord);
            case 7: return new Vector3(-halfOutline, -halfOutline, zCoord);
        }
        return Vector3.zero;
    }

    private void ClearEffect() {
        foreach (var copy in OutlineClones) {
            if (copy != null) {
                DestroyImmediate(copy.gameObject);
            }
        }
        OutlineClones.Clear();
    }

    public void RefreshEffectColor() {
        foreach (var copy in OutlineClones) {
            if (copy != null) {
                copy.color = OutlineColor;
            }
        }
    }

    public void RefreshShadowPostion()
    {
        Vector3 offset = ShadowOffset;
        offset.z = 0.1f;

        OutlineClones[0].transform.localPosition = offset;
    }

    public void RefreshOutlinePosition() {
        for (int i = 0; i < OutlineClones.Count; ++i) {
            OutlineClones[i].transform.localPosition = GetOutlinePostionByIndex(i);
        }
    }

    public void SetText(string text)
    {
        Text = text;
        Commit();
    }
}

public enum TextMeshStyle
{
    None = 0,
    Shadow = 1,
    Outline = 2,
}