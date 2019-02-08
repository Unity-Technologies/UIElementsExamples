using System;
using System.Collections.Generic;
using UIElementsExamples;
using UnityEngine;

public class BindingsTestMonoBehaviour : MonoBehaviour
{
    // SerializedPropertyType.Generic
    // NO C# DATA TYPE

    // SerializedPropertyType.Integer
    public int intField = 42;

    // SerializedPropertyType.Boolean
    public bool boolField = true;

    // SerializedPropertyType.Float
    public float floatField = 23.6f;

    // SerializedPropertyType.String
    public string stringField = "String field text.";

    // SerializedPropertyType.Color
    public Color colorField = Color.green;

    // SerializedPropertyType.ObjectReference
    public UnityEngine.Object unityObjectField = null;
    public Material materialField = null;
    public Texture2D texture2DField = null;
    public Shader shaderField = null;

    // SerializedPropertyType.LayerMask
    public LayerMask layerMaskField = new LayerMask();

    // SerializedPropertyType.Enum
    public enum ExampleEnumType
    {
        Zero,
        One,
        Two,
        Three
    }
    public ExampleEnumType enumField = ExampleEnumType.Zero;

    // SerializedPropertyType.Vector2
    public Vector2 vector2Field = new Vector2();

    // SerializedPropertyType.Vector3
    public Vector3 vector3Field = new Vector3();

    // SerializedPropertyType.Vector4
    public Vector4 vector4Field = new Vector4();

    // SerializedPropertyType.Rect
    public Rect rectField = new Rect();

    // SerializedPropertyType.ArraySize
    public int[] intArrayField = new int[4];
    public float[] floatArrayField = new float[4];

    // SerializedPropertyType.Character
    public char charField = 'y';

    // SerializedPropertyType.AnimationCurve
    public AnimationCurve animationCurveField;

    // SerializedPropertyType.Bounds
    public Bounds boundsField = new Bounds();

    // SerializedPropertyType.Gradient
    public Gradient gradientField = new Gradient();

    // SerializedPropertyType.Quaternion
    public Quaternion quaternionField = new Quaternion();

    // SerializedPropertyType.ExposedReference
    // NO C# DATA TYPE

    // SerializedPropertyType.FixedBufferSize
    // NO C# DATA TYPE?

    // SerializedPropertyType.Vector2Int
    public Vector2Int vector2IntField = new Vector2Int();

    // SerializedPropertyType.Vector3Int
    public Vector3Int vector3IntField = new Vector3Int();

    // SerializedPropertyType.RectInt
    public RectInt rectIntField = new RectInt();

    // SerializedPropertyType.BoundsInt
    public BoundsInt boundsIntRect = new BoundsInt();

    //////////////////////////////////////

    // List<int>
    public List<int> listTInt = new List<int>() { 1, 2, 3 };

    // Struct
    [Serializable]
    public struct SimpleStruct
    {
        public int structIntField;
        public float structFloatField;
    }
    public SimpleStruct structField = new SimpleStruct() { structIntField = 42, structFloatField = 23.6f };

    //////////////////////////////////////

    public DefaultDrawerType structDefaultDrawer;
    public DefaultDrawerType[] structDefaultDrawerArray = new DefaultDrawerType[3];
    public IMGUIDrawerType imguiCustomDrawer;
    public UIElementsDrawerType uiElementsCustomDrawer;
    public UIElementsDrawerType[] uiElementsCustomDrawerArray = new UIElementsDrawerType[3];

    //////////////////////////////////////

    public void OnEnable()
    {
        animationCurveField = new AnimationCurve()
        { keys = new Keyframe[2] { new Keyframe(0, 0), new Keyframe(1, 1) } };
    }
}
