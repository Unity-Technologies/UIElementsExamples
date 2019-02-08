using System;

namespace UIElementsExamples
{
    [Serializable]
    public class DefaultDrawerType
    {
        public enum IngredientUnit { Spoon, Cup, Bowl, Piece }
        public string name;
        public int amount = 1;
        public IngredientUnit unit;
    }

    [Serializable]
    public class IMGUIDrawerType
    {
        public enum IngredientUnit { Spoon, Cup, Bowl, Piece }
        public string name;
        public int amount = 1;
        public IngredientUnit unit;
    }

    [Serializable]
    public class UIElementsDrawerType
    {
        public enum IngredientUnit { Spoon, Cup, Bowl, Piece }
        public string name;
        public int amount = 1;
        public IngredientUnit unit;
    }
}
