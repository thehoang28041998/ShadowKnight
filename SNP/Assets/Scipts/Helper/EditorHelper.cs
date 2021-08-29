using System;
using UnityEditor;
using UnityEngine;

namespace Scipts.Helper {
    public static class EditorHelper {
        public class Indent : IDisposable {
            private int indentLevel;

            public Indent(int indentLevel) {
                this.indentLevel = indentLevel;
                EditorGUI.indentLevel += indentLevel;
            }

            public void Dispose() {
                EditorGUI.indentLevel -= indentLevel;
            }
        }

        public class IndentPadding : IDisposable {
            private RectOffset padding = new RectOffset(10, 0, 0, 0);

            public IndentPadding(int indentWidth) {
                padding.left = indentWidth;
                GUIStyle gs = new GUIStyle();
                gs.padding = padding;
                EditorGUILayout.BeginVertical(gs);
                EditorGUIUtility.labelWidth -= padding.left;
            }

            public void Dispose() {
                EditorGUIUtility.labelWidth += padding.left;
                EditorGUILayout.EndVertical();
            }
        }

    #region Group

        public class EditorGUILabelWidth : IDisposable {
            private float width;

            private float originalWidth;

            public EditorGUILabelWidth(float width) {
                this.width = width;
                originalWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = width;
            }

            public void Dispose() {
                EditorGUIUtility.labelWidth = originalWidth;
            }
        }

        public class DisabledGroup : IDisposable {
            private bool isDisabled;

            public DisabledGroup(bool isDisabled) {
                this.isDisabled = isDisabled;
                EditorGUI.BeginDisabledGroup(isDisabled);
            }

            public void Dispose() {
                EditorGUI.EndDisabledGroup();
            }
        }

        public class Vertical : IDisposable {
            private readonly int pixels;

            public Vertical(string text) {
                GUILayout.BeginVertical(text);
            }

            public Vertical(GUIStyle style, string text = "", int pixels = 0) {
                this.pixels = pixels;
                if (string.IsNullOrEmpty(text)) {
                    GUILayout.BeginVertical(text, style);
                }
                else {
                    style.normal.textColor = Color.red;
                    GUILayout.BeginVertical(text, style);
                }
            }

            public void Dispose() {
                GUILayout.EndVertical();
                GUILayout.Space(pixels);
            }
        }

        public class Horizontal : IDisposable {
            private readonly int pixels;

            public Horizontal() {
                GUILayout.BeginHorizontal();
            }

            public Horizontal(GUIStyle style, string text = "", int pixels = 0) {
                this.pixels = pixels;
                GUILayout.BeginHorizontal(text, style);
            }

            public void Dispose() {
                GUILayout.EndHorizontal();
                GUILayout.Space(pixels);
            }
        }

        public class ScrollView : IDisposable {
            private ScrollPosition scrollPosition;

            public ScrollView(ScrollPosition sp) {
                this.scrollPosition = sp;
                scrollPosition.ScrollPos = EditorGUILayout.BeginScrollView(
                        scrollPosition.ScrollPos, false, false
                );
            }

            public void Dispose() {
                EditorGUILayout.EndScrollView();
            }

            public class ScrollPosition {
                private Vector2 scrollPos;

                public Vector2 ScrollPos {
                    get { return scrollPos; }
                    set { scrollPos = value; }
                }
            }
        }

    #endregion

    #region Label

        public static void SetText(string text) {
            GUILayout.Label(text);
        }

        public static void SetText(string text, Color color, int fontSize, bool bold = false) {
            GUILayout.Label(text, LabelStyle(color, fontSize, bold));
        }

        public static GUIStyle LabelStyle(Color color, int fontSize, bool bold = false) {
            GUIStyle style = new GUIStyle(EditorStyles.label);
            if (bold) style = new GUIStyle(EditorStyles.boldLabel);
            style.normal.textColor = color;
            style.fontSize = fontSize;
            return style;
        }


        public static GUIStyle ToggleStyle(Color color, int fontSize, FontStyle fontStyle) {
            GUIStyle style = new GUIStyle(EditorStyles.toggleGroup);
            style.normal.textColor = color;
            style.fontStyle = fontStyle;
            style.fontSize = fontSize;
            return style;
        }

    #endregion

    }
}
