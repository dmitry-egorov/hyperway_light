using System;

namespace Common {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class orderAttribute : Attribute {
        public int order;

        public orderAttribute(int order) => this.order = order;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class afterAttribute : Attribute {
        public Type targetType;
        public int orderIncrease;

        public afterAttribute(Type targetType) {
            this.targetType = targetType;
            this.orderIncrease = 10;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class beforeAttribute : Attribute {
        public Type targetType;
        public int orderDecrease;

        public beforeAttribute(Type targetType) {
            this.targetType = targetType;
            this.orderDecrease = 10;
        }
    }
}