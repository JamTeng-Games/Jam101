// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2024 Kybernetik //

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Animancer.Editor.Tools
{
    /// <summary>[Editor-Only] [Pro-Only] 
    /// An <see cref="AnimationModifierTool"/> for changing which
    /// <see cref="Sprite"/>s an <see cref="AnimationClip"/> uses.
    /// </summary>
    /// <remarks>
    /// <strong>Documentation:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/manual/tools/remap-sprite-animation">
    /// Remap Sprite Animation</see>
    /// </remarks>
    /// https://kybernetik.com.au/animancer/api/Animancer.Editor.Tools/RemapSpriteAnimationTool
    /// 
    [Serializable]
    public class RemapSpriteAnimationTool : AnimationModifierTool
    {
        /************************************************************************************************************************/

        [SerializeField] private List<Sprite> _NewSprites;

        [NonSerialized] private List<Sprite> _OldSprites;
        [NonSerialized] private bool _OldSpritesAreDirty;
        [NonSerialized] private ReorderableList _OldSpriteDisplay;
        [NonSerialized] private ReorderableList _NewSpriteDisplay;
        [NonSerialized] private EditorCurveBinding _SpriteBinding;
        [NonSerialized] private ObjectReferenceKeyframe[] _SpriteKeyframes;

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override int DisplayOrder => 4;

        /// <inheritdoc/>
        public override string Name => "Remap Sprite Animation";

        /// <inheritdoc/>
        public override string HelpURL => Strings.DocsURLs.RemapSpriteAnimation;

        /// <inheritdoc/>
        public override string Instructions
        {
            get
            {
                if (Animation == null)
                    return "Select the animation you want to remap.";

                if (_OldSprites.Count == 0)
                    return "The selected animation does not use Sprites.";

                return "Assign the New Sprites that you want to replace the Old Sprites with then click Save As." +
                    " You can Drag and Drop multiple Sprites onto the New Sprites list at the same time.";
            }
        }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override void OnEnable(int index)
        {
            base.OnEnable(index);

            _NewSprites ??= new();

            if (Animation == null)
                _NewSprites.Clear();

            _OldSprites = new();

            _OldSpriteDisplay = AnimancerToolsWindow.CreateReorderableObjectList(_OldSprites, "Old Sprites");
            _NewSpriteDisplay = AnimancerToolsWindow.CreateReorderableObjectList(_NewSprites, "New Sprites");
        }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        protected override void OnAnimationChanged()
        {
            base.OnAnimationChanged();
            _OldSpritesAreDirty = true;
        }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override void DoBodyGUI()
        {
            base.DoBodyGUI();
            GatherOldSprites();

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                GUI.enabled = false;
                _OldSpriteDisplay.DoLayoutList();
                GUI.enabled = true;
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                _NewSpriteDisplay.DoLayoutList();
                GUILayout.EndVertical();

                HandleDragAndDropIntoList(GUILayoutUtility.GetLastRect(), _NewSprites, overwrite: true);
            }
            GUILayout.EndHorizontal();

            GUI.enabled = Animation != null;

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Reset"))
                {
                    AnimancerGUI.Deselect();
                    AnimancerToolsWindow.RecordUndo();
                    _NewSprites.Clear();
                    _OldSpritesAreDirty = true;
                }

                if (GUILayout.Button("Save As"))
                {
                    if (SaveAs())
                    {
                        _OldSpritesAreDirty = true;
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        /************************************************************************************************************************/

        /// <summary>Gathers the <see cref="_OldSprites"/> from the <see cref="AnimationModifierTool.Animation"/>.</summary>
        private void GatherOldSprites()
        {
            if (!_OldSpritesAreDirty)
                return;

            _OldSpritesAreDirty = false;

            _OldSprites.Clear();
            _NewSprites.Clear();

            if (Animation == null)
                return;

            var bindings = AnimationUtility.GetObjectReferenceCurveBindings(Animation);
            for (int iBinding = 0; iBinding < bindings.Length; iBinding++)
            {
                var binding = bindings[iBinding];
                if (binding.type == typeof(SpriteRenderer) && binding.propertyName == "m_Sprite")
                {
                    _SpriteBinding = binding;
                    _SpriteKeyframes = AnimationUtility.GetObjectReferenceCurve(Animation, binding);

                    for (int iKeyframe = 0; iKeyframe < _SpriteKeyframes.Length; iKeyframe++)
                    {
                        var reference = _SpriteKeyframes[iKeyframe].value as Sprite;
                        if (reference != null)
                            _OldSprites.Add(reference);
                    }

                    _NewSprites.AddRange(_OldSprites);

                    return;
                }
            }
        }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        protected override void Modify(AnimationClip animation)
        {
            for (int i = 0; i < _SpriteKeyframes.Length; i++)
            {
                _SpriteKeyframes[i].value = _NewSprites[i];
            }

            AnimationUtility.SetObjectReferenceCurve(animation, _SpriteBinding, _SpriteKeyframes);
        }

        /************************************************************************************************************************/
    }
}

#endif

