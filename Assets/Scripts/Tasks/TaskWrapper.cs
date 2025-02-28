using System;
using Unity.Burst;

namespace DefaultNamespace
{
    public unsafe delegate void TaskExecuteDelegate(void* taskData);
    [BurstCompile]
    public unsafe struct TaskWrapper
    {
        public void* TaskDataPtr;               // Pointer to task data
        public FunctionPointer<TaskExecuteDelegate> ExecuteFunc; // Function pointer for execution
    }
}