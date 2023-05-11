using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseResponse
{
    ApiError Error { get; set; }
}
