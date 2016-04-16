using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor TextMeshController extension
/// Author: Insality
/// </summary>
[CustomEditor(typeof(TextMeshController)), CanEditMultipleObjects]
public class TextMeshControllerEditor : Editor {

    private TextMeshController[] _controllers;

    public void OnEnable() {
        _controllers = new TextMeshController[targets.Length];
        for (int i = 0; i < _controllers.Length; ++i) {
            _controllers[i] = targets[i] as TextMeshController;
            _controllers[i].FindTextMesh();
        }
    }
    
    public override void OnInspectorGUI()
    {
        var controller = target as TextMeshController;

        #region Default Values
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("Default Font Values");

        controller.TextMesh.font = (Font)EditorGUILayout.ObjectField("Font", controller.TextMesh.font, typeof(Font), false);
        
        controller.TextMesh.color = EditorGUILayout.ColorField("Text Color", controller.TextMesh.color);
        
        var anchor = (TextAnchor) EditorGUILayout.EnumPopup("Anchor", controller.TextMesh.anchor);
        if (anchor != controller.TextMesh.anchor)
        {
            foreach (var c in _controllers)
            {
                c.TextMesh.anchor = anchor;
                c.RefreshEffectParam();
            }
        }

        var aligment = (TextAlignment) EditorGUILayout.EnumPopup("Aligment", controller.TextMesh.alignment);
        if (aligment != controller.TextMesh.alignment)
        {
            foreach (var c in _controllers)
            {
                c.TextMesh.alignment = aligment;
                c.RefreshEffectParam();
            }
        }

        var fontSize = EditorGUILayout.IntField("Font Atlas Size", controller.TextMesh.fontSize);
        if (fontSize != controller.TextMesh.fontSize)
        {
            foreach (var c in _controllers)
            {
                c.TextMesh.fontSize = fontSize;
                c.RefreshEffectParam();
            }
        }



        #endregion


        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("Text Settings");

        controller.Text = EditorGUILayout.TextArea(controller.Text,GUILayout.Height(32), GUILayout.ExpandHeight(true));
        foreach (var c in _controllers) {
            c.Text = controller.Text;
            c.Commit();
        }

        var size = EditorGUILayout.FloatField("Size", controller.TextMesh.characterSize);
        if (size != controller.TextMesh.characterSize)
        {
            foreach (var c in _controllers)
            {
                c.TextMesh.characterSize = size;
                c.RefreshEffectParam();
            }
        }

        TextMeshStyle selectedEffect = (TextMeshStyle)EditorGUILayout.EnumPopup("Style", controller.TextStyle);
        if (selectedEffect != controller.TextStyle) {
            foreach (var c in _controllers) {
                c.TextStyle = selectedEffect;
                c.RefreshEffect();
            }
        }

        Color outlineColor = controller.OutlineColor;
        Vector2 shadowOffset = controller.ShadowOffset;
        switch (controller.TextStyle)
        {
            case TextMeshStyle.None:
                break;
            case TextMeshStyle.Shadow:
                outlineColor = EditorGUILayout.ColorField("Color", outlineColor);
                if (controller.OutlineColor != outlineColor)
                {
                    controller.OutlineColor = outlineColor;
                    controller.RefreshEffectColor();
                }
                shadowOffset = EditorGUILayout.Vector2Field("Offset", shadowOffset);
                if (shadowOffset != controller.ShadowOffset)
                {
                    controller.ShadowOffset = shadowOffset;
                    controller.RefreshShadowPostion();
                }
                break;
            case TextMeshStyle.Outline:
                outlineColor = EditorGUILayout.ColorField("Color", outlineColor);
                if (controller.OutlineColor != outlineColor)
                {
                    controller.OutlineColor = outlineColor;
                    controller.RefreshEffectColor();
                }

                float outlineWidth = EditorGUILayout.FloatField("Outline Width", controller.OutlineWidth);
                if (outlineWidth != controller.OutlineWidth)
                {
                    controller.OutlineWidth = outlineWidth;
                    controller.RefreshOutlinePosition();
                }

                shadowOffset = EditorGUILayout.Vector2Field("Offset", shadowOffset);
                if (shadowOffset != controller.ShadowOffset)
                {
                    controller.ShadowOffset = shadowOffset;
                    controller.RefreshOutlinePosition();
                }
                break;
        }
    }
}
