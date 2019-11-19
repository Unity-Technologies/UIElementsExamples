using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.UIElements.Experimental;

namespace UIElementsExamples
{
    public class E17_Transitions : EditorWindow
    {
        [MenuItem("UIElementsExamples/17_Transitions")]
        static void Init()
        {
            var wnd = GetWindow<E17_Transitions>();
            wnd.titleContent = new GUIContent("Transitions");
        }

        private ValueAnimation<float> xAnim;
        private ValueAnimation<float> yAnim;

        public AnimationCurve customCurve;

        private VisualElement canvas;
        private Button startButton;
        private VisualElement cursor;
        private VisualElement targetElement;

        private CurveField customCurveField;
        private IntegerField durationField;


        private static readonly string cursorTrailClassname = "cursor-trail";
        private List<VisualElement> cursorTrail = new List<VisualElement>();
        private float cursorSize = 9;
        private float trailSize = 5;


        private List<string> easingcurves = new List<string>()
        {
            "CustomAnimationCurve",
            "Linear",
            "Step",
            "InSine",
            "OutSine",
            "InOutSine",
            "InQuad",
            "OutQuad",
            "InOutQuad",
            "InCubic",
            "OutCubic",
            "InOutCubic",
            "InCirc",
            "OutCirc",
            "InOutCirc",
            "InBounce",
            "OutBounce",
            "InOutBounce",
            "InElastic",
            "OutElastic",
            "InOutElastic",
            "InBack",
            "OutBack",
            "InOutBack",
            "InCustomCode",
            "OutCustomCode",
            "InOutCustomCode",
        };


        public static float In(float t)
        {
            if (t < 0.5)
            {
                return t;
            }
            t = t * 2 - 1;

            return 0.5f + t * t * 0.5f;
        }

        public static float Out(float t)
        {
            return 1 - In(1 - t);
        }

        public static float InOut(float t)
        {
            t = t * 2;

            if (t < 1)
            {
                return 0.5f * In(t);
            }
            else
            {
                return 0.5f + 0.5f * Out(t - 1);
            }
        }

        public float SampleCurve(float t)
        {
            return customCurve.Evaluate(t);
        }

        private List<Func<float, float>> easingCurvesFuncs = new List<Func<float, float>>()
        {
            null,
            Easing.Linear,
            Easing.Step,
            Easing.InSine,
            Easing.OutSine,
            Easing.InOutSine,
            Easing.InQuad,
            Easing.OutQuad,
            Easing.InOutQuad,
            Easing.InCubic,
            Easing.OutCubic,
            Easing.InOutCubic,
            Easing.InCirc,
            Easing.OutCirc,
            Easing.InOutCirc,
            Easing.InBounce,
            Easing.OutBounce,
            Easing.InOutBounce,
            Easing.InElastic,
            Easing.OutElastic,
            Easing.InOutElastic,
            Easing.InBack,
            Easing.OutBack,
            Easing.InOutBack,
            In,
            Out,
            InOut
        };


        public E17_Transitions()
        {
            customCurve = new AnimationCurve();
            customCurve.AddKey(new Keyframe(0, 0));
            customCurve.AddKey(new Keyframe(0.3f, 0.7f, -3.2f, -3.2f));
            customCurve.AddKey(new Keyframe(1, 1));
        }

        void OnEnable()
        {
            easingCurvesFuncs[0] = this.SampleCurve;

            var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Examples/Editor/Transitions.uxml");

            var root = this.rootVisualElement;
            var top = asset.CloneTree();
            top.StretchToParentSize();
            top.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/Editor/transitions-style.uss"));
            root.Add(top);


            canvas = root.Q<VisualElement>("canvas");
            var valueContainer = root.Q<VisualElement>("value-container");

            PopupField<string> easingPopup = new PopupField<string>(easingcurves, 0);
            easingPopup.label = "Easing";
            easingPopup.RegisterValueChangedCallback((evt) => SetEasingCurve(easingPopup.value));
            valueContainer.Insert(0, easingPopup);

            customCurveField = root.Q<CurveField>();
            customCurveField.value = customCurve;
            durationField = root.Q<IntegerField>();

            startButton = root.Q<Button>("start-button");
            startButton.clickable.clicked += OnButtonClicked;
            startButton.name = "start-button";

            cursor = new VisualElement();
            canvas.Add(cursor);
            cursor.style.width = cursorSize;
            cursor.style.height = cursorSize;
            cursor.style.backgroundColor = Color.red;

            targetElement = new VisualElement() {name = "target"};
            targetElement.style.position = Position.Absolute;
            canvas.Add(targetElement);

            int defaultDurationMs = 1000;

            xAnim = canvas.experimental.animation.Start(0, 1, defaultDurationMs, (e, v) => OnXChanged(v)).Ease(Easing.Linear).KeepAlive();
            xAnim.Stop();
            xAnim.onAnimationCompleted += UpdateButtonText;

            durationField.SetValueWithoutNotify(xAnim.durationMs);

            yAnim = canvas.experimental.animation.Start(0, 1, defaultDurationMs, (e, v) => OnYChanged(v)).Ease(Easing.Linear).KeepAlive();
            yAnim.Stop();

            cursor.RegisterCallback<GeometryChangedEvent>((e) => UpdateCursorTrail());

            easingPopup.value = easingcurves[easingcurves.Count - 1];
            SetEasingCurve(easingPopup.value);

            targetElement.style.right = 0;
        }

        void SetEasingCurve(string value)
        {
            int index = easingcurves.IndexOf(value);

            if (index < 0 || index >= easingCurvesFuncs.Count)
            {
                index = 0;
            }

            if (index == 0)
            {
                customCurveField.style.display = DisplayStyle.Flex;
            }
            else
            {
                customCurveField.style.display = DisplayStyle.None;
            }

            yAnim.easingCurve = easingCurvesFuncs[index];
        }

        void UpdateButtonText()
        {
            if (xAnim.isRunning)
            {
                startButton.text = "Stop";
            }
            else
            {
                startButton.text = "Start";
            }
        }

        void OnButtonClicked()
        {
            if (xAnim.isRunning)
            {
                xAnim.Stop();
                yAnim.Stop();
            }
            else
            {
                HideCursorTrail();
                xAnim.Start();
                yAnim.Start();
            }

            customCurve = customCurveField.value;

            int duration = Mathf.Clamp(durationField.value, 10, 10000);
            xAnim.durationMs = duration;
            yAnim.durationMs = duration;

            // we do a color animation using the helper methods
            targetElement.experimental.animation.Start(Color.red, Color.yellow, duration, (e, c) => e.style.backgroundColor = c).Ease(yAnim.easingCurve);

            UpdateButtonText();
        }

        void OnYChanged(float yValue)
        {
            yValue = 1 - yValue;
            float newValue = yValue * (canvas.layout.height - cursorSize);

            cursor.style.top = newValue;
            targetElement.style.top = newValue - (targetElement.resolvedStyle.height - cursor.resolvedStyle.height) / 2;
            targetElement.style.left = canvas.layout.width;
        }

        void OnXChanged(float xValue)
        {
            float newValue = xValue * (canvas.layout.width - cursorSize);

            cursor.style.left = newValue;
        }

        private void HideCursorTrail()
        {
            canvas.Query<VisualElement>(null, cursorTrailClassname).ForEach((t) =>
            {
                t.style.display = DisplayStyle.None;
                cursorTrail.Add(t);
            });
        }

        void UpdateCursorTrail()
        {
            VisualElement trail = null;

            if (cursorTrail.Count > 0)
            {
                trail = trail = cursorTrail[0];
                var hover = trail.userData as HoverAnimation;

                if (hover != null)
                {
                    hover.Reset();
                }
                cursorTrail.RemoveAt(0);
            }
            else
            {
                trail = new VisualElement();
                trail.style.position = Position.Absolute;
                trail.name = "trail";
                trail.AddToClassList(cursorTrailClassname);
                trail.style.backgroundColor = Color.blue;
                trail.userData = new HoverAnimation(trail);
                canvas.Add(trail);
            }

            trail.style.display = DisplayStyle.Flex;

            Rect trailPos = new Rect(0, 0, trailSize, trailSize);
            trailPos.center = cursor.layout.center;

            trail.style.left = trailPos.x;
            trail.style.top = trailPos.y;
            trail.style.width = trailPos.width;
            trail.style.height = trailPos.height;
            cursor.BringToFront();
        }
    }


    public class HoverAnimation
    {
        float sizeDiff {get; set;}

        Rect originalRect;
        Rect overSizeRect;

        VisualElement target;
        IValueAnimation currentAnim;

        public HoverAnimation(VisualElement t)
        {
            sizeDiff = 10;
            target = t;

            target.RegisterCallback<MouseEnterEvent>((evt) => OnMouseEnter());
            target.RegisterCallback<MouseLeaveEvent>((evt) => OnMouseLeave());
        }

        public void Reset()
        {
            originalRect.width = 0;
        }

        void ClearAnim(IValueAnimation anim)
        {
            if (currentAnim != null && currentAnim == anim)
            {
                currentAnim = null;
                anim.Stop();
                anim.Recycle();
            }
        }

        void OnMouseEnter()
        {
            ClearAnim(currentAnim);

            if (originalRect.width == 0)
            {
                originalRect = target.layout;
            }

            overSizeRect = originalRect;

            overSizeRect.yMax += sizeDiff;
            overSizeRect.xMax += sizeDiff;

            overSizeRect.xMin -= sizeDiff;
            overSizeRect.yMin -= sizeDiff;

            var anim = target.experimental.animation.Start(
                new StyleValues()
                {
                    top = overSizeRect.y,
                    left = overSizeRect.x,
                    width = overSizeRect.width,
                    height = overSizeRect.height,
                    backgroundColor = Color.green,
                }, 200).Ease(Easing.InCubic).KeepAlive();

            currentAnim = anim;
            anim.OnCompleted(() => ClearAnim(anim));
        }

        void OnMouseLeave()
        {
            ClearAnim(currentAnim);

            var anim = target.experimental.animation.Start(
                new StyleValues()
                {
                    top = originalRect.y,
                    left = originalRect.x,
                    width = originalRect.width,
                    height = originalRect.height,
                    backgroundColor = Color.blue,
                }, 200).Ease(Easing.InCubic).KeepAlive();

            currentAnim = anim;
            anim.OnCompleted(() => ClearAnim(anim));
        }
    }
}
