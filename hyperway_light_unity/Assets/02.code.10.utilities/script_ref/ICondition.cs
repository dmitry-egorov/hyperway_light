using UnityEngine;

namespace Utilities.ScriptReferencing {
    public interface ICondition {
        bool is_satisfied(GameObject go);
    }
}