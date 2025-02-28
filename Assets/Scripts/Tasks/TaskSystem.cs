using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

[BurstCompile]
public partial struct TaskSystem : ISystem
{
    private NativeList<TaskWrapper> taskList;

    public void OnCreate(ref SystemState state)
    {
        // Initialize the NativeList
        taskList = new NativeList<TaskWrapper>(Allocator.Persistent);
    }

    public void OnDestroy(ref SystemState state)
    {
        // Dispose of the NativeList
        if (taskList.IsCreated)
            taskList.Dispose();
    }

    
    [BurstCompile]
    public unsafe void OnUpdate(ref SystemState state)
    {
        // FetchTask  fetchTask = new FetchTask
        // {
        //     ResourceId = 1,
        //     Amount = 10
        // };
        // AddTask(fetchTask, BurstCompiler.CompileFunctionPointer<TaskExecuteDelegate>(fetchTask.Execute));
        // Execute all tasks
        for (int i = 0; i < taskList.Length; i++)
        {
            var taskWrapper = taskList[i];
            taskWrapper.ExecuteFunc.Invoke(taskWrapper.TaskDataPtr);

            // Free the allocated memory for task data
            UnsafeUtility.Free(taskWrapper.TaskDataPtr, Allocator.Persistent);
        }

        // Clear the list after execution
        taskList.Clear();
    }

    public unsafe void AddTask<T>(T task, FunctionPointer<TaskExecuteDelegate> executeFunc) where T : unmanaged
    {
        // Allocate memory for the task
        void* taskDataPtr = UnsafeUtility.Malloc(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), Allocator.Persistent);
        UnsafeUtility.CopyStructureToPtr(ref task, taskDataPtr);

        // Add the task to the list
        taskList.Add(new TaskWrapper
        {
            TaskDataPtr = taskDataPtr,
            ExecuteFunc = executeFunc
        });
    }
}
