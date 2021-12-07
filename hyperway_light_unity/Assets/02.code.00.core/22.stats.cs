using static Hyperway.entity_type.props_;
using static Hyperway.hyperway;

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
        public void add_resource_amounts(ref batch result) => 
            for_each(ref result, (ref batch result, ref entity_type type) => type.add_resource_amounts(ref result));
    }
    
    public partial struct entity_type {
        public void add_resource_amounts(ref batch result) {
            if (props.all(stores)) {} else return;

            for (var i = 0; i < count; i++)
                result += resources_amount[i];
        }
    }
}