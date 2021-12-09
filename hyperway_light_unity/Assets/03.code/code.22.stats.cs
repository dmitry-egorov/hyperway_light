using static Hyperway.entity_type_props;
using static Hyperway.hyperway;
using static Hyperway.res_type_ext;

namespace Hyperway {
    public partial struct stats {
        public void start () => calculate();
        public void update() => calculate();

        void calculate() {
            total_stored.reset();
            _entities.add_resource_amounts(ref total_stored);
        }
    }

    public partial struct entities {
        public void add_resource_amounts(ref fixed_batch result) =>
            for_each(ref result, (ref fixed_batch result, ref entity_type type) => type.add_resource_amounts(ref result));
    }
    
    public partial struct entity_type {
        public void add_resource_amounts(ref fixed_batch result) {
            if (props.all(stores)) {} else return;

            foreach (var t in all_res_types) {
                var data = resources[t];
                for (var i = 0; i < count; i++) 
                    result[t] += data.stored_arr[i];
            }
        }
    }
}