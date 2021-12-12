using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Jobs;
using static Unity.Collections.Allocator;
using static Unity.Collections.NativeArrayOptions;

namespace Utilities.Collections {
    public static class native_arr_ext {
        public static void copy_from<t>(this NativeArray<t> dst, NativeArray<t> src, int count) where t : struct => NativeArray<t>.Copy(src, dst, count);
        
        public static unsafe void clear<t>(this NativeArray<t> array) where t : struct => UnsafeUtility.MemClear(array.GetUnsafePtr(), array.Length);

        public static unsafe void set<t>(this NativeArray<t> array, t value) where t : struct {
            for (var i = 0; i < array.Length; i++) array[i] = value;
        }

        [WriteAccessRequired] public static unsafe ref t @ref<t>(this NativeArray<t> array, int index) where t : struct {
            array.check(index);
            return ref UnsafeUtility.ArrayElementAsRef<t>(array.GetUnsafePtr(), index);
        }
        
        public static void init<t>(this ref NativeArray<t> arr, int count) where t : struct {
            if (arr.IsCreated) {
                if (arr.Length == count) {
                    arr.clear();
                    return;
                }
                arr.Dispose();
            }
            arr = new NativeArray<t>(count, Persistent, ClearMemory);
        }
        
        public static void init(this ref NativeBitArray arr, int count) {
            if (arr.IsCreated) {
                if (arr.Length == count) {
                    arr.Clear();
                    return;
                }
                arr.Dispose();
            }
            arr = new NativeBitArray(count, Persistent, ClearMemory);
        }
        
        public static void init(this ref TransformAccessArray arr, int count) {
            if (arr.isCreated) arr.Dispose();
            arr = new TransformAccessArray(count);
        }
        
        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        static void check<t>(this NativeArray<t> array, int index) where t : struct {
            var safety = NativeArrayUnsafeUtility.GetAtomicSafetyHandle(array);
            
            if (index < 0 || index > array.Length)
                array.FailOutOfRangeError(index);
            
            AtomicSafetyHandle.ValidateNonDefaultHandle(in safety);
            AtomicSafetyHandle.CheckWriteAndThrow(safety);
        }
        
        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        static void FailOutOfRangeError<t>(this NativeArray<t> array, int index) where t : struct {
            throw new IndexOutOfRangeException($"Index {index} is out of range of '{array.Length}' Length.");
        }
    }
}