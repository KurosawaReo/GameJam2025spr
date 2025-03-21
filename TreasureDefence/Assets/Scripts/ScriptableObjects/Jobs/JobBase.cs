using Gloval;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobBase : ScriptableObject
{
    [Header("基本情報系")]
    [Tooltip("ジョブのID")]
    public string jobId = "Job-0000";

    [Tooltip("ジョブの名前")]
    public string jobName = "JobName";
}
