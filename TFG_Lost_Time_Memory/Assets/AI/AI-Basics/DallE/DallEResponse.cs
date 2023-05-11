using System.Collections.Generic;

public class DallEResponse : IBaseResponse

{
    public ApiError Error { get; set; }
    public long Created { get; set; }
    public List<DallEImageData> Data { get; set; }
}